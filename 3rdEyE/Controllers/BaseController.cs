using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _3rdEyE.Models;
using _3rdEyE.ManagingTools;
using _3rdEyE.BLL;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Text;
using System.Configuration;
using System.Net.Mail;
using Newtonsoft.Json;
using _3rdEyE.ViewModels;

namespace _3rdEyE.Controllers
{
    public class BaseController : Controller
    {
        public AppUser CurrentUser;
        public BaseBLL bll = new BaseBLL();

        public BaseController()
        {
            CurrentUser = CommonClass.GetCurrentUser();
            if (CurrentUser != null)
            {
                ViewBag.CurrentUser = CurrentUser;
                var CurrentLoginID = CommonClass.GetCurrentLoginID();
                ViewBag.CurrentLoginID = CurrentLoginID;

                List<WebMenu> RoleMenuList = CommonClass.GetRoleMenuList();
                ViewBag.RoleMenuList = RoleMenuList;

                //List<Permission> RolePermissionList = CommonClass.GetRolePermissionList();
                //ViewBag.RolePermissionList = RolePermissionList;

                ViewBag.IsNotifiable = CurrentUser.IsNotifiable == true ? true : false;//Is_CurrentUser_also_a_RequisitionAgent
            }
        }

        public StaffResult GetUserDetrailFromHRISApi(string staffID)
        {
            StaffResult staffDetail = null;
            string res_row = "";
            try
            {
                WebClient client = new WebClient();
                res_row = client.DownloadString("http://hris.prangroup.com:8686/api/hrisapi.svc/Staff/" + staffID);

                if (res_row != "{\"StaffResult\":\"[]\"}" || res_row != "{\"StaffResult\":\"Value cannot be null.\\u000d\\u000aParameter name: source\"}")
                {
                    res_row = res_row.Replace("\\", "");
                    res_row = res_row.Replace("\"[", "[").Replace("]\"", "]");
                    var _HRISAPIData = JsonConvert.DeserializeObject<VM_HRISAPIData>(res_row);
                    if (_HRISAPIData.StaffResult.Count > 0)
                    {
                        staffDetail = _HRISAPIData.StaffResult.FirstOrDefault();
                    }
                }
            }
            catch (Exception)
            {
            }
            return staffDetail;
        }
        public JsonResult GetUserInfoFromHRIS_Json(string staffID)
        {
            var staffDetail = GetUserDetrailFromHRISApi(staffID);
            return Json(staffDetail, JsonRequestBehavior.AllowGet);
        }

        public void SendMail_Single(string receiverMail = "automation17@mis.prangroup.com", string mailSubject = "Test mail subject", string mailBody_HTML = "Test mail body")
        {
            var _email = "automation@mis.prangroup.com";
            var _epass = "aaaaAAAA0000";
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(receiverMail);
                mail.From = new MailAddress(_email);
                mail.Subject = mailSubject;
                mail.Body = mailBody_HTML;
                mail.IsBodyHtml = true;
                SmtpClient sc = new SmtpClient("mail.mis.prangroup.com");
                //SmtpClient sc = new SmtpClient("172.17.4.106");
                sc.EnableSsl = false;
                sc.Credentials = new NetworkCredential(_email, _epass);
                sc.Port = 25;
                sc.Send(mail);
            }
            catch (Exception e)
            {
                bll.db.AppErrorLogs.Add(
                  new AppErrorLog()
                  {
                      ErrorMessage = e.Message,
                      ErrorTime = DateTime.Now,
                      UserDefinedMessage = "Base/SendMail_Single"
                  }
                  );
                bll.db.SaveChanges();
            }
        }
        public void SendMail_Multiple(List<string> MailToList, List<string> MailCcList, string mailSubject, string mailBody_HTML)
        {
            var _email = "automation@mis.prangroup.com";
            var _epass = "aaaaAAAA0000";
            try
            {
                MailMessage mail = new MailMessage();
                foreach (var to in MailToList)
                {
                    mail.To.Add(to);
                }
                foreach (var cc in MailCcList)
                {
                    mail.CC.Add(cc);
                }
                mail.From = new MailAddress(_email);
                mail.Subject = mailSubject;
                mail.Body = mailBody_HTML;
                mail.IsBodyHtml = true;
                SmtpClient sc = new SmtpClient("mail.mis.prangroup.com");
                //SmtpClient sc = new SmtpClient("172.17.4.106");
                sc.EnableSsl = false;
                sc.Credentials = new NetworkCredential(_email, _epass);
                sc.Port = 25;
                sc.Send(mail);
            }
            catch (Exception e)
            {
                bll.db.AppErrorLogs.Add(
                  new AppErrorLog()
                  {
                      ErrorMessage = e.Message,
                      ErrorTime = DateTime.Now,
                      UserDefinedMessage = "Base/SendMail_Multiple"
                  }
                  );
                bll.db.SaveChanges();
            }
        }
        public string SendSMS(string numbers, string message)
        {
            string content = "";
            //try
            //{
            //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://sms.prangroup.com/postman/api/sendsms?userid=210077&password=22a95dc8997b24bd752b71e04aca8705&msisdn=" + numbers + "&masking=28585&message=" + message + "&unicode=false");//pass=prg@1234
            //    request.Method = "GET";
            //    request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 58.0.3029.110 Safari / 537.36";
            //    request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //    using (Stream stream = response.GetResponseStream())
            //    {
            //        using (StreamReader sr = new StreamReader(stream))
            //        {
            //            content = sr.ReadToEnd();
            //        }
            //    }
            //}
            //catch (Exception e)
            //{
            //    content = e.Message;
            //}
            return content;
        }

        public string SendFCM_Notification_Single(Guid? PK_AppUser, string Category, string Title, string SubTitle)
        {
            var _PK_AppUser = PK_AppUser != null ? PK_AppUser : Guid.Parse("35ca81cd-ff9c-4d0f-a806-d4f4b039c32f");//A random guid
            var appUserFID = bll.db.AppUsers.Where(m => m.PK_User == _PK_AppUser && m.FID != null).Select(m => m.FID).FirstOrDefault();
            if (appUserFID == null)
            {
                return "NotFound";
            }
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("Category", Category);
            data.Add("Title", Title);
            data.Add("SubTitle", SubTitle);
            var request = new
            {
                to = appUserFID,
                data = new
                {
                    data
                }
            };
            var json = new JavaScriptSerializer().Serialize(request);
            Byte[] byteArray = Encoding.UTF8.GetBytes(json);
            try
            {
                var FCM_ServerID = ConfigurationManager.AppSettings["FCM_SERVER_KEY"];
                var FCM_SENDER_ID = ConfigurationManager.AppSettings["FCM_SENDER_ID"];
                //WebRequest webRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                WebRequest webRequest = WebRequest.Create("http://172.17.4.154/fcm/");
                webRequest.Method = "post";
                webRequest.ContentType = "application/json";
                webRequest.ContentLength = byteArray.Length;
                Stream stream = webRequest.GetRequestStream();
                stream.Write(byteArray, 0, byteArray.Length);
                stream.Close();

                WebResponse webResponse = webRequest.GetResponse();
                stream = webResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(stream);

                var finalResponse = streamReader.ReadToEnd();

                streamReader.Close();
                stream.Close();
                webResponse.Close();
                return "Done";
            }
            catch (Exception e)
            {
                bll.db.AppErrorLogs.Add(
                    new AppErrorLog()
                    {
                        ErrorMessage = e.Message,
                        ErrorTime = DateTime.Now,
                        UserDefinedMessage = "Base/SendFCM_Notification_Single"
                    }
                    );
                bll.db.SaveChanges();
                return "Error";
            }
        }
        public string HRIS()
        {
            try
            {
                //Body: {"Key":"HR1S019XX78LOGIN","UserName":"223511","Password":"123"}
                //string url = "http://hris.prangroup.com:8686/api/hrisapi.svc/UserLoginValidate";
                //WebClient wc = new WebClient();
                //wc.QueryString.Add("Key", "HR1S019XX78LOGIN");
                //wc.QueryString.Add("UserName", "210077");
                //wc.QueryString.Add("Password", "770012");
                //var data = wc.UploadValues(url, "POST", wc.QueryString);
                //var responseString = UnicodeEncoding.UTF8.GetString(data);
                //return responseString;

                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://hris.prangroup.com:8686/api/hrisapi.svc/UserLoginValidate");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = new JavaScriptSerializer().Serialize(new
                    {
                        Key = "HR1S019XX78LOGIN",
                        UserName = "210077",
                        Password = "770012"
                    });

                    streamWriter.Write(json);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }
                return "";

            }
            catch (Exception e)
            {
                bll.db.AppErrorLogs.Add(
                    new AppErrorLog()
                    {
                        ErrorMessage = e.Message,
                        ErrorTime = DateTime.Now,
                        UserDefinedMessage = "Base/SendFCM_Notification_Single_New"
                    }
                    );
                bll.db.SaveChanges();
                return "Error";
            }
        }
        public string SendFCM_Notification_Single_New(string token, string title, string message, string id, string type)
        {
            return "code is commented";
            try
            {
                string url = "http://172.17.2.112/fcm.php";
                WebClient wc = new WebClient();
                wc.QueryString.Add("token", token);
                wc.QueryString.Add("title", title);
                wc.QueryString.Add("message", message);
                wc.QueryString.Add("id", id);
                wc.QueryString.Add("type", type);
                var data = wc.UploadValues(url, "POST", wc.QueryString);
                var responseString = UnicodeEncoding.UTF8.GetString(data);
                return responseString;
                return "";

            }
            catch (Exception e)
            {
                bll.db.AppErrorLogs.Add(
                    new AppErrorLog()
                    {
                        ErrorMessage = e.Message,
                        ErrorTime = DateTime.Now,
                        UserDefinedMessage = "Base/SendFCM_Notification_Single_New"
                    }
                    );
                bll.db.SaveChanges();
                return "Error";
            }
        }
        public string SendFCM_Notification_Single_Test()
        {
            //string token = "ExponentPushToken[GeTzHpE5arh_zLTFWZaTBT]"; string title = "3rdEyE"; string body = "A new requistion has been added"; string id = "";
            try
            {

                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://172.17.2.112/fcm.php");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = new JavaScriptSerializer().Serialize(new
                    {
                        token = "ExponentPushToken[GeTzHpE5arh_zLTFWZaTBT]",
                        title = "3rdEyE",
                        message = "A new requistion has been added",
                        body = "A new requistion has been added",
                        id = "6"
                    });

                    streamWriter.Write(json);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                var result = "";
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                return result;
            }
            catch (Exception e)
            {
                return "Error";
            }
        }


        public string SendFCM_Notification_Multiple(List<Guid> PK_AppUsers, string Category, string Title, string SubTitle)
        {
            var appUserFIDs = bll.db.AppUsers.Where(m => PK_AppUsers.Contains(m.PK_User) && m.FID != null).Select(m => m.FID).ToList();
            if (appUserFIDs.Count == 0)
            {
                return "NotFound";
            }

            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("Category", Category);
            data.Add("Title", Title);
            data.Add("SubTitle", SubTitle);

            var request = new
            {
                registration_ids = appUserFIDs,
                data = new
                {
                    data
                }
            };

            var json = new JavaScriptSerializer().Serialize(request);
            Byte[] byteArray = Encoding.UTF8.GetBytes(json);
            try
            {
                var FCM_ServerID = ConfigurationManager.AppSettings["FCM_SERVER_KEY"];
                var FCM_SENDER_ID = ConfigurationManager.AppSettings["FCM_SENDER_ID"];
                //WebRequest webRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                WebRequest webRequest = WebRequest.Create("http://172.17.4.154/fcm/");
                webRequest.Method = "post";
                webRequest.ContentType = "application/json";
                webRequest.ContentLength = byteArray.Length;
                Stream stream = webRequest.GetRequestStream();
                stream.Write(byteArray, 0, byteArray.Length);
                stream.Close();

                WebResponse webResponse = webRequest.GetResponse();
                stream = webResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(stream);

                var finalResponse = streamReader.ReadToEnd();

                streamReader.Close();
                stream.Close();
                webResponse.Close();
                return "Done";
            }
            catch (Exception e)
            {
                bll.db.AppErrorLogs.Add(
                    new AppErrorLog()
                    {
                        ErrorMessage = e.Message,
                        ErrorTime = DateTime.Now,
                        UserDefinedMessage = "Base/SendFCM_Notification_Multiple"
                    }
                    );
                bll.db.SaveChanges();
                return "Error";
            }
        }

        public static List<Dictionary<string, string>> GetTableRows(DataTable dtData)
        {
            List<Dictionary<string, string>> lstRows = new List<Dictionary<string, string>>();
            Dictionary<string, string> dictRow = null;

            foreach (DataRow dr in dtData.Rows)
            {
                dictRow = new Dictionary<string, string>();
                foreach (DataColumn col in dtData.Columns)
                {
                    dictRow.Add(col.ColumnName, dr[col].ToString());
                }
                lstRows.Add(dictRow);
            }
            return lstRows;
        }

        //# DO NOT CHANGE
        public static class AlertMessageType
        {
            public const string Information = "INFORMATION";
            public const string Danger = "DANGER";
            public const string Warning = "WARNING";
            public const string Success = "SUCCESS";
        }

        //# USED FOR ActivityStatus Column in all models
        public Dictionary<bool, string> ActivityStatusesDict = new Dictionary<bool, string> { { true, "Active" }, { false, "Inactive" } };
        public Dictionary<bool, string> YesNoDict = new Dictionary<bool, string> { { true, "Yes" }, { false, "No" } };

        public void CreateAlertMessage(string messageType, string messageTitle, string messageBody)
        {
            TempData.Add("ALERT_MESSAGE_TYPE", messageType);
            TempData.Add("ALERT_MESSAGE_TITLE", messageTitle);
            TempData.Add("ALERT_MESSAGE_BODY", messageBody);
        }


    }
}