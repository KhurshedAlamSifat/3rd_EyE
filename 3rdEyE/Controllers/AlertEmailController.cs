using _3rdEyE.BLL;
using _3rdEyE.CustomModels;
using _3rdEyE.ManagingTools;
using _3rdEyE.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace _3rdEyE.Controllers
{
    public class AlertEmailController : BaseController
    {
        public ActionResult Index()
        {
            return View(bll.db.AlertEmails.Where(m => m.IsDeleted != true).ToList());
        }

        public ActionResult Create()
        {
            ViewBag.Depoes = bll.db.Depoes.Where(m => m.IsDeleted != true && (!m.Category.Contains("Physical"))).OrderBy(m => m.Name).ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection form)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var email = new AlertEmail();
            email.PK_AlertEmail = Guid.NewGuid();
            email.IsDeleted = false;
            email.MailAddress = form["MailAddress"].Trim().ToLower();

            var EventAlert_1 = form["EventAlert_1"];
            email.EventAlert_1 = EventAlert_1 == "true" ? true : false;

            var EventAlert_2 = form["EventAlert_2"];
            email.EventAlert_2 = EventAlert_2 == "true" ? true : false;

            var EventAlert_3 = form["EventAlert_3"];
            email.EventAlert_3 = EventAlert_3 == "true" ? true : false;

            var PoliceCaseAlert_1 = form["PoliceCaseAlert_1"];
            email.PoliceCaseAlert_1 = PoliceCaseAlert_1 == "true" ? true : false;

            var PoliceCaseAlert_2 = form["PoliceCaseAlert_2"];
            email.PoliceCaseAlert_2 = PoliceCaseAlert_2 == "true" ? true : false;

            if (bll.db.AlertEmails.Where(m => m.MailAddress == email.MailAddress).Any())
            {
                CreateAlertMessage(AlertMessageType.Danger, "Validation Failure", "Email is already added.");
                ViewBag.Depoes = bll.db.Depoes.Where(m => m.IsDeleted != true && (!m.Category.Contains("Physical"))).OrderBy(m => m.Name).ToList();
                return View(email);
            }
            bll.db.AlertEmails.Add(email);
            bll.db.SaveChanges();
            var PermittedDepoes = form["PermittedDepoes"];
            try
            {
                var PermittedDepoe_FKs = PermittedDepoes.Split(',');
                if (PermittedDepoe_FKs.Count() > 0)
                {
                    foreach (var FK in PermittedDepoe_FKs)
                    {
                        if (FK == "")
                        {
                            break;
                        }
                        bll.db.AlertEmailAttachedDepoes.Add(
                            new AlertEmailAttachedDepo()
                            {
                                FK_AlertEmail = email.PK_AlertEmail,
                                FK_Depo = Guid.Parse(FK),
                                IsAttachable = true
                            }
                         );
                    }
                }
                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "Requisition-permission is successfully updated.");
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                ViewBag.Depoes = bll.db.Depoes.Where(m => m.IsDeleted != true && (!m.Category.Contains("Physical"))).OrderBy(m => m.Name).ToList();
                return View(email);
            }
        }
        public ActionResult Edit(Guid id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.AlertEmails.Where(m => m.PK_AlertEmail == id).FirstOrDefault();
            return View(model);
        }
        public JsonResult GetAccessibleDepoListOfUser(Guid FK_AlertEmail)
        {
            var list = (from depo in bll.db.Depoes.Where(m => m.IsDeleted != true && (!m.Category.Contains("Physical"))).AsEnumerable()
                        select new
                        {
                            FK_Depo = depo.PK_Depo,
                            DepoName = depo.Name,
                            depo.PRG_Type,
                            IsAccessible = bll.db.AlertEmailAttachedDepoes.Where(m => m.FK_Depo == depo.PK_Depo && m.FK_AlertEmail == FK_AlertEmail && m.IsAttachable == true).Any()
                        }
                        ).OrderBy(m => m.DepoName).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Edit(FormCollection form)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var PK_AlertEmail = Guid.Parse(form["PK_AlertEmail"]);
            var email = bll.db.AlertEmails.Where(m => m.PK_AlertEmail == PK_AlertEmail).FirstOrDefault();
            email.MailAddress = form["MailAddress"].Trim().ToLower();

            var EventAlert_1 = form["EventAlert_1"];
            email.EventAlert_1 = EventAlert_1 == "true" ? true : false;

            var EventAlert_2 = form["EventAlert_2"];
            email.EventAlert_2 = EventAlert_2 == "true" ? true : false;

            var EventAlert_3 = form["EventAlert_3"];
            email.EventAlert_3 = EventAlert_3 == "true" ? true : false;

            var PoliceCaseAlert_1 = form["PoliceCaseAlert_1"];
            email.PoliceCaseAlert_1 = PoliceCaseAlert_1 == "true" ? true : false;

            var PoliceCaseAlert_2 = form["PoliceCaseAlert_2"];
            email.PoliceCaseAlert_2 = PoliceCaseAlert_2 == "true" ? true : false;

            var PermittedDepoes = form["PermittedDepoes"];
            try
            {

                var PermittedDepoe_FKs = PermittedDepoes.Split(',');
                var oldList = bll.db.AlertEmailAttachedDepoes.Where(m => m.FK_AlertEmail == PK_AlertEmail).ToList();
                foreach (var item in oldList)
                {
                    //# Update existing permission
                    if (PermittedDepoe_FKs.Contains(item.FK_Depo.ToString()))
                    {
                        item.IsAttachable = true;
                    }
                    else
                    {
                        item.IsAttachable = false;
                    }
                }
                if (PermittedDepoe_FKs.Count() > 0)
                {

                    foreach (var FK in PermittedDepoe_FKs)
                    {
                        if (FK == "")
                        {
                            break;
                        }
                        //# Add new parmision
                        if (!oldList.Where(m => m.FK_Depo.ToString() == FK).Any())
                        {
                            bll.db.AlertEmailAttachedDepoes.Add(
                            new AlertEmailAttachedDepo()
                            {
                                FK_AlertEmail = PK_AlertEmail,
                                FK_Depo = Guid.Parse(FK),
                                IsAttachable = true
                            }
                            );
                        }
                    }
                }
                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "Permission is successfully updated.");
                return RedirectToAction("Edit", new { id = PK_AlertEmail });
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                return RedirectToAction("Edit", new { id = PK_AlertEmail });
            }
        }
        public ActionResult Delete(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var model = bll.db.AlertEmails.Find(id);
                if (model != null)
                {
                    try
                    {
                        model.IsDeleted = true;
                        bll.db.SaveChanges();
                        CreateAlertMessage(AlertMessageType.Success, "Success", "Email is successfully deleted.");
                        return RedirectToAction("Index");
                    }
                    catch (Exception exception)
                    {
                        CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }


        #region Veicle Maturity
        public string TrySendVeicleAgrrementAlertEmail()
        {
            try
            {
                var _email = "automation@mis.prangroup.com";
                var _epass = "aaaaAAAA0000";

                var today = DateTime.Now.AddDays(-5);
                var theday = DateTime.Now.AddDays(-5);
                var _list = bll.db.Vehicles.Where(m => (m.Internal_FinancingAgrementMaturityDate >= theday || m.Internal_FinancingAgrementMaturityDate < today) && m.Internal_FinancingAgrementIsMatured != true).OrderBy(m => m.Internal_FinancingAgrementMaturityDate).ToList();
                if (_list.Count() == 0)
                {
                    return "No Vehicle Found";
                }

                SmtpClient sc = new SmtpClient("mail.mis.prangroup.com");
                //SmtpClient sc = new SmtpClient("172.17.4.106");
                sc.EnableSsl = false;
                sc.Credentials = new NetworkCredential(_email, _epass);
                sc.Port = 25;

                var Mail_Subject = "3rd EyE: Vehicle Agreement Maturity Alert";
                var Mail_ToList = new List<string>() { "automation17@mis.prangroup.com" };
                //var Mail_CcList = new List<string>() { "automation11@mis.prangroup.com" };
                MailMessage mail = new MailMessage();
                foreach (var to in Mail_ToList)
                {
                    mail.To.Add(to);
                }
                //foreach (var cc in Mail_CcList)
                //{
                //    mail.CC.Add(cc);
                //}

                mail.From = new MailAddress(_email);
                mail.Subject = Mail_Subject;
                string url = "http://172.17.9.160/AlertEmail/VeicleAgrrementtEmailBodyGenerator";
                WebClient myWebClient = new WebClient();
                byte[] myDataBuffer = myWebClient.DownloadData(url);
                string mailBody_HTML = Encoding.UTF8.GetString(myDataBuffer);
                mail.Body = mailBody_HTML;
                mail.IsBodyHtml = true;

                sc.Send(mail);
                return "Sent";
            }
            catch (Exception e)
            {
                bll.db.AppErrorLogs.Add(
                   new AppErrorLog()
                   {
                       ErrorMessage = e.Message,
                       ErrorTime = DateTime.Now,
                       UserDefinedMessage = "AlertEmail/SendMail1"
                   }
                   );
                bll.db.SaveChanges();
                return "Error";
            }
        }
        public ActionResult VeicleAgrrementEmailBodyGenerator()
        {
            var today = DateTime.Now.AddDays(-5);
            var theday = DateTime.Now.AddDays(-5);
            var _list = bll.db.Vehicles.Where(m => (m.Internal_FinancingAgrementMaturityDate >= theday || m.Internal_FinancingAgrementMaturityDate < today) && m.Internal_FinancingAgrementIsMatured != true).OrderBy(m => m.Internal_FinancingAgrementMaturityDate).ToList();
            var bllVehicle = new BLL_Vehicle();
            var list = _list.Select(m => bllVehicle.ConvertToViewModel(m)).ToList();
            return View(list);
        }
        #endregion

        public string SendMail1(string receiverMail, string mailSubject, string mailBody_HTML)
        {
            SendMail_Single("automation17@mis.prangroup.com", "SendMail1", "<h1>Alhamdulillah</h1>");
            return "ok";
        }
        public string SendMail2(string receiverMail, string mailSubject, string mailBody_HTML)
        {
            SendMail_2("automation17@mis.prangroup.com", "SendMail2", "<h1>Alhamdulillah</h1>");
            return "ok";
        }
        public string SendMail3(string receiverMail, string mailSubject, string mailBody_HTML)
        {
            SendMail_3("automation17@mis.prangroup.com", "SendMail3", "<h1>Alhamdulillah</h1>");
            return "ok";
        }

        public void SendMail_2(string receiverMail, string mailSubject, string mailBody_HTML)
        {
            try
            {
                HttpClient client = new HttpClient();
                var values = new Dictionary<string, string>
                {
                   { "token" , "mail"},
                   { "to", receiverMail },
                   { "subject", mailSubject},
                   { "msg", mailBody_HTML },
                   { "display", "3rdEyE-Mail" },
                };
                var content = new FormUrlEncodedContent(values);
                var response = client.PostAsync("http://172.17.4.154/PRISM/Project/Models/ticket_model.php", content);
                //var responseString = await response.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                bll.db.AppErrorLogs.Add(
                  new AppErrorLog()
                  {
                      ErrorMessage = e.Message,
                      ErrorTime = DateTime.Now,
                      UserDefinedMessage = "AlertEmail/SendMail2"
                  }
                  );
                bll.db.SaveChanges();
            }
        }

        string SendMail_3(string receiverMail, string mailSubject, string mailBody_HTML)
        {
            string content = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://172.17.4.154/PRISM/Project/Models/ticket_model.php?token=mail&to=" + receiverMail + "&subject=" + mailSubject + "&msg=" + mailBody_HTML + "&display=3rdEyE-Mail");
                request.Method = "GET";
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 58.0.3029.110 Safari / 537.36";
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        content = sr.ReadToEnd();
                    }
                }
            }
            catch (Exception e)
            {
                bll.db.AppErrorLogs.Add(
                  new AppErrorLog()
                  {
                      ErrorMessage = e.Message,
                      ErrorTime = DateTime.Now,
                      UserDefinedMessage = "AlertEmail/SendMail3"
                  }
                  );
                bll.db.SaveChanges();
            }
            return content;
        }


    }
}