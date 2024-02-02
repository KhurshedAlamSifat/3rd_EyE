using _3rdEyE.BLL;
using _3rdEyE.CustomModels;
using _3rdEyE.Models;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace _3rdEyE.Controllers
{
    public class AccountingController : BaseController
    {
        Dictionary<string, string> PRG_Types = new Dictionary<string, string> { { "PRAN", "PRAN" }, { "RFL", "RFL" } };
        Dictionary<string, string> DB_Months = new Dictionary<string, string> {
            { "3rdEyE_TrackingDataBase_2019_03", "March-2019" },
            { "3rdEyE_TrackingDataBase_2019_04", "April-2019" },
            { "3rdEyE_TrackingDataBase_2019_05", "May-2019" },
            { "3rdEyE_TrackingDataBase_2019_06", "June-2019" },
            { "3rdEyE_TrackingDataBase_2019_07", "July-2019" } ,
            { "3rdEyE_TrackingDataBase_2019_08", "August-2019" },
            { "3rdEyE_TrackingDataBase_2019_09", "September-2019" },
            { "3rdEyE_TrackingDataBase_2019_10", "October-2019" },
            { "3rdEyE_TrackingDataBase_2019_11", "November-2019" },
            { "3rdEyE_TrackingDataBase_2019_12", "December-2019" },
            { "3rdEyE_TrackingDataBase_2020_01", "January-2020" },
            { "3rdEyE_TrackingDataBase_2020_02", "February-2020" },
            { "3rdEyE_TrackingDataBase_2020_03", "March-2020" },
            { "3rdEyE_TrackingDataBase_2020_04", "April-2020" },
            { "3rdEyE_TrackingDataBase_2020_05", "May-2020" },
            { "3rdEyE_TrackingDataBase_2020_06", "June-2020" },
            { "3rdEyE_TrackingDataBase_2020_07", "July-2020" },
            { "3rdEyE_TrackingDataBase_2020_08", "August-2020" },
            { "3rdEyE_TrackingDataBase_2020_09", "Sepetember-2020" },
            { "3rdEyE_TrackingDataBase_2020_10", "October-2020" },
            { "3rdEyE_TrackingDataBase_2020_11", "November-2020" },
            { "3rdEyE_TrackingDataBase_2020_12", "December-2020" },
        };
        public ActionResult MonthlyBill(string PRG_Type, string DB_Month)
        {
            if (!string.IsNullOrEmpty(PRG_Type) && !string.IsNullOrEmpty(DB_Month))
            {
                ViewBag.PRG_Types = new SelectList(PRG_Types, "Key", "Value", PRG_Type);
                ViewBag.DB_Months = new SelectList(DB_Months, "Key", "Value", DB_Month);
                string query = "";
                List<Dictionary<string, string>> dictioneryDetailList = new List<Dictionary<string, string>>();
                DataTable dataTable = new DataTable();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandTimeout = 600;
                SqlDataAdapter adpt = new SqlDataAdapter();
                cmd.Connection = (SqlConnection)bll.db.Database.Connection;
                query = "select '" + PRG_Type + "' as 'PRG_Type', '" + DB_Month + "' as 'DB_Month', Depo.PK_Depo as 'FK_Depo', Depo.Name as 'DepoName', Company.Name as 'CompanyName', Vehicle.RegistrationNumber, t_1.FK_Vehicle, (COUNT(t_1.Date) - 1) as 'DateCount' from " +
                "(select distinct(DeviceData.FK_Vehicle) as 'FK_Vehicle', CAST(DeviceData.UpdateTime AS DATE) as 'Date' from[" + DB_Month + "].dbo.DeviceData " +
                "group by DeviceData.FK_Vehicle, CAST(DeviceData.UpdateTime AS DATE)) t_1 " +
                "join Vehicle on t_1.FK_Vehicle = Vehicle.PK_Vehicle " +
                "join Depo on Vehicle.FK_Depo = Depo.PK_Depo " +
                "join Company on Vehicle.FK_Company = Company.PK_Company " +
                "where Depo.PRG_Type = '" + PRG_Type + "' " +
                "group by t_1.FK_Vehicle, Vehicle.RegistrationNumber, Depo.PK_Depo, Depo.Name, Company.Name " +
                "having (COUNT(t_1.Date) - 1) >= 5 " +
                "order by Depo.Name";
                cmd.CommandText = query;
                adpt.SelectCommand = cmd;
                dataTable.Reset();
                adpt.Fill(dataTable);
                dictioneryDetailList.AddRange(GetTableRows(dataTable));
                var BillEntryList = new List<MonthlyBillEntry>();
                foreach (var item in dictioneryDetailList)
                {
                    var billEntry = new MonthlyBillEntry();
                    billEntry.PRG_Type = item["PRG_Type"];
                    billEntry.DB_Month = item["DB_Month"];
                    billEntry.FK_Depo = Guid.Parse(item["FK_Depo"]);
                    billEntry.DepoName = item["DepoName"];
                    billEntry.CompanyName = item["CompanyName"];
                    billEntry.RegistrationNumber = item["RegistrationNumber"];
                    billEntry.FK_Vehicle = Guid.Parse(item["FK_Vehicle"]);
                    billEntry.DateCount = Convert.ToInt32(item["DateCount"]);
                    BillEntryList.Add(billEntry);
                }

                if (!bll.db.MonthlyBillEntries.Where(m => m.PRG_Type == PRG_Type && m.DB_Month == DB_Month).Any())
                {
                    bll.db.MonthlyBillEntries.AddRange(BillEntryList);
                    bll.db.SaveChanges();
                }
                View(BillEntryList);
            }
            ViewBag.PRG_Types = new SelectList(PRG_Types, "Key", "Value", PRG_Type);
            ViewBag.DB_Months = new SelectList(DB_Months, "Key", "Value", DB_Month);
            return View();
        }
        public ActionResult GenerateMonthlyDetailBill(string PRG_Type, string DB_Month)
        {
            if (!string.IsNullOrEmpty(PRG_Type) && !string.IsNullOrEmpty(DB_Month))
            {
                string query = "";
                List<Dictionary<string, string>> dictioneryDetailList = new List<Dictionary<string, string>>();
                DataTable dataTable = new DataTable();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandTimeout = 600;
                SqlDataAdapter adpt = new SqlDataAdapter();
                cmd.Connection = (SqlConnection)bll.db.Database.Connection;
                query = "select '" + PRG_Type + "' as 'PRG_Type', '" + DB_Month + "' as 'DB_Month', Depo.PK_Depo as 'FK_Depo', Depo.Name as 'DepoName', Company.Name as 'CompanyName', Vehicle.RegistrationNumber, t_1.FK_Vehicle, (COUNT(t_1.Date) - 1) as 'DateCount' from " +
                "(select distinct(DeviceData.FK_Vehicle) as 'FK_Vehicle', CAST(DeviceData.UpdateTime AS DATE) as 'Date' from[" + DB_Month + "].dbo.DeviceData " +
                "group by DeviceData.FK_Vehicle, CAST(DeviceData.UpdateTime AS DATE)) t_1 " +
                "join Vehicle on t_1.FK_Vehicle = Vehicle.PK_Vehicle " +
                "join Depo on Vehicle.FK_Depo = Depo.PK_Depo " +
                "join Company on Vehicle.FK_Company = Company.PK_Company " +
                "where Depo.PRG_Type = '" + PRG_Type + "' " +
                "group by t_1.FK_Vehicle, Vehicle.RegistrationNumber, Depo.PK_Depo, Depo.Name, Company.Name " +
                "having (COUNT(t_1.Date) - 1) >= 5 " +
                "order by Depo.Name";
                cmd.CommandText = query;
                adpt.SelectCommand = cmd;
                dataTable.Reset();
                adpt.Fill(dataTable);
                dictioneryDetailList.AddRange(GetTableRows(dataTable));
                var BillEntryList = new List<MonthlyBillEntry>();
                foreach (var item in dictioneryDetailList)
                {
                    var billEntry = new MonthlyBillEntry();
                    billEntry.PRG_Type = item["PRG_Type"];
                    billEntry.DB_Month = item["DB_Month"];
                    billEntry.FK_Depo = Guid.Parse(item["FK_Depo"]);
                    billEntry.DepoName = item["DepoName"];
                    billEntry.CompanyName = item["CompanyName"];
                    billEntry.RegistrationNumber = item["RegistrationNumber"];
                    billEntry.FK_Vehicle = Guid.Parse(item["FK_Vehicle"]);
                    billEntry.DateCount = Convert.ToInt32(item["DateCount"]);
                    BillEntryList.Add(billEntry);
                }
                View(BillEntryList);
            }
            return View();
        }

        public string SendMonthlyBillMail_BulkCall()
        {
            var res_PRAN = "";
            var res_RFL = "";
            var _DB_Month = "3rdEyE_TrackingDataBase_" + DateTime.Now.AddMonths(-1).Year + "_" + (DateTime.Now.AddMonths(-1).Month.ToString().Count() == 1 ? "0" + DateTime.Now.AddMonths(-1).Month.ToString() : DateTime.Now.AddMonths(-1).Month.ToString());
            var _BillingDate = String.Format("{0:dd'-'MMMM'-'yy}", DateTime.Now);
            var _DueDate = String.Format("{0:dd'-'MMMM'-'yy}", DateTime.Now.AddDays(5));
            var _BillingPeriod = String.Format("{0:MMMM'-'yyyy}", DateTime.Now.AddMonths(-1));

            var guid = Guid.NewGuid();
            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "SendMonthlyBillMail_BulkCall-PRAN-Start-" + guid,
                      CallingTime = DateTime.Now,
                      UserDefinedMessage = ""
                  }
                  );
            bll.db.SaveChanges();
            try
            {
                var _Group = "PRAN";
                var Mail_Subject = "3rd EyE (" + _Group + ") Bill for the month of " + _BillingPeriod;
                //var Mail_ToList = new List<string>() { "dist191@prangroup.com", "piptpt8@pip.prangroup.com", "tpt3@prangroup.com" };
                var Mail_ToList = new List<string> { "dist191@prangroup.com", "piptpt8@pip.prangroup.com", "tpt3@prangroup.com" };
                var Mail_CcList = new List<string>() { "automation17@mis.prangroup.com", "mis3@mis.prangroup.com", "mis7@mis.prangroup.com", "mis78@mis.prangroup.com", "dist100@prangroup.com", "dist@prangroup.com" };
                var Mail_Body = "Dear Valued Client";
                Mail_Body = Mail_Body + "<P> Good day and thank you to stay with SKYLAB </P>";
                Mail_Body = Mail_Body + "<P> Attached is your Billing Statement of 3rd EyE for the month of " + _BillingPeriod + " </P>";
                Mail_Body = Mail_Body + "<P> If you have any query feel free to communicate with your business partner. </P>";
                Mail_Body = Mail_Body + "<P> <b>Note:</b> This is system generated mail, no need to reply </P>";

                res_PRAN = SendMailWithAttachments(_Group, _DB_Month, _BillingPeriod, _BillingDate, _DueDate, Mail_Subject, Mail_Body, Mail_ToList, Mail_CcList);
            }
            catch (Exception e)
            {
                var errrorMessage = "";
                do
                {
                    errrorMessage = "#" + errrorMessage + e.Message;
                    if (e.InnerException != null)
                    {
                        e = e.InnerException;
                    }
                    else
                    {
                        break;
                    }
                } while (true);
                bll.db.ServiceCalls.Add(
                new ServiceCall()
                {
                    CallingTime = DateTime.Now,
                    CallingMessage = "SendMonthlyBillMail_BulkCall-PRAN-Error" + guid,
                    UserDefinedMessage = errrorMessage
                }
                );
                bll.db.SaveChanges();
            }
            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "SendMonthlyBillMail_BulkCall-PRAN-End-" + guid,
                      CallingTime = DateTime.Now,
                      UserDefinedMessage = ""
                  }
                  );
            bll.db.SaveChanges();

            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "SendMonthlyBillMail_BulkCall-RFL-Start-" + guid,
                      CallingTime = DateTime.Now,
                      UserDefinedMessage = ""
                  }
                  );
            bll.db.SaveChanges();
            try
            {
                var _Group = "RFL";
                var Mail_Subject = "3rd EyE (" + _Group + ") Bill for the month of " + _BillingPeriod;
                var Mail_ToList = new List<string>() { "dist404@rflgroupbd.com", "rfl456@rflgroupbd.com", "rfl578@rflgroupbd.com" };
                var Mail_CcList = new List<string>() { "automation17@mis.prangroup.com", "mis3@mis.prangroup.com", "mis7@mis.prangroup.com", "mis78@mis.prangroup.com", "dist121@prangroup.com", "dist370@prangroup.com" };
                var Mail_Body = "Dear Valued Client";
                Mail_Body = Mail_Body + "<P> Good day and thank you to stay with SKYLAB </P>";
                Mail_Body = Mail_Body + "<P> Attached is your Billing Statement of 3rd EyE for the month of " + _BillingPeriod + " </P>";
                Mail_Body = Mail_Body + "<P> If you have any query feel free to communicate with your business partner. </P>";
                Mail_Body = Mail_Body + "<P> <b>Note:</b> This is system generated mail, no need to reply </P>";

                res_RFL = SendMailWithAttachments(_Group, _DB_Month, _BillingPeriod, _BillingDate, _DueDate, Mail_Subject, Mail_Body, Mail_ToList, Mail_CcList);
            }
            catch (Exception e)
            {
                var errrorMessage = "";
                do
                {
                    errrorMessage = "#" + errrorMessage + e.Message;
                    if (e.InnerException != null)
                    {
                        e = e.InnerException;
                    }
                    else
                    {
                        break;
                    }
                } while (true);
                bll.db.ServiceCalls.Add(
                new ServiceCall()
                {
                    CallingTime = DateTime.Now,
                    CallingMessage = "SendMonthlyBillMail_BulkCall-RFL-Error" + guid,
                    UserDefinedMessage = errrorMessage
                }
                );
                bll.db.SaveChanges();
            }
            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "SendMonthlyBillMail_BulkCall-RFL-End-" + guid,
                      CallingTime = DateTime.Now,
                      UserDefinedMessage = ""
                  }
                  );
            bll.db.SaveChanges();
            return "Sent res_PRAN:" + res_PRAN + " res_RFL:" + res_RFL;
        }
        public string SendMailWithAttachments(string PRG_Type, string DB_Month, string BillingPeriod, string BillingDate, string DueDate, string Mail_Subject, string Mail_Body, List<string> Mail_ToList, List<string> Mail_CcList)
        {
            string query = "";
            List<Dictionary<string, string>> dictioneryDetailList = new List<Dictionary<string, string>>();
            DataTable dataTable = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = int.MaxValue;
            SqlDataAdapter adpt = new SqlDataAdapter();
            cmd.Connection = (SqlConnection)bll.db.Database.Connection;
            query = "select '" + PRG_Type + "' as 'PRG_Type', '" + DB_Month + "' as 'DB_Month', Depo.PK_Depo as 'FK_Depo', Depo.Name as 'DepoName', Company.Name as 'CompanyName', Vehicle.RegistrationNumber, t_1.FK_Vehicle, (COUNT(t_1.Date) - 1) as 'DateCount' from " +
            "(select distinct(DeviceData.FK_Vehicle) as 'FK_Vehicle', CAST(DeviceData.UpdateTime AS DATE) as 'Date' from[" + DB_Month + "].dbo.DeviceData " +
            "group by DeviceData.FK_Vehicle, CAST(DeviceData.UpdateTime AS DATE)) t_1 " +
            "join Vehicle on t_1.FK_Vehicle = Vehicle.PK_Vehicle " +
            "join Depo on Vehicle.FK_Depo = Depo.PK_Depo " +
            "join Company on Vehicle.FK_Company = Company.PK_Company " +
            "where Depo.PRG_Type = '" + PRG_Type + "' " +
            "group by t_1.FK_Vehicle, Vehicle.RegistrationNumber, Depo.PK_Depo, Depo.Name, Company.Name " +
            "having (COUNT(t_1.Date) - 1) >= 5 " +
            "order by Depo.Name";
            cmd.CommandText = query;
            adpt.SelectCommand = cmd;
            dataTable.Reset();
            adpt.Fill(dataTable);
            dictioneryDetailList.AddRange(GetTableRows(dataTable));
            var BillEntryList = new List<MonthlyBillEntry>();
            foreach (var item in dictioneryDetailList)
            {
                var billEntry = new MonthlyBillEntry();
                billEntry.PRG_Type = item["PRG_Type"];
                billEntry.DB_Month = item["DB_Month"];
                billEntry.FK_Depo = Guid.Parse(item["FK_Depo"]);
                billEntry.DepoName = item["DepoName"];
                billEntry.CompanyName = item["CompanyName"];
                billEntry.RegistrationNumber = item["RegistrationNumber"];
                billEntry.FK_Vehicle = Guid.Parse(item["FK_Vehicle"]);
                billEntry.DateCount = Convert.ToInt32(item["DateCount"]);
                BillEntryList.Add(billEntry);
            }
            //var BillEntryList = new List<MonthlyBillEntry>();
            //BillEntryList.Add(new MonthlyBillEntry() { DepoName = "Dpl- Chittagong Shodor 12", RegistrationNumber = "D1 V1" });
            //BillEntryList.Add(new MonthlyBillEntry() { DepoName = "D1", RegistrationNumber = "D1 V2" });
            //BillEntryList.Add(new MonthlyBillEntry() { DepoName = "D1", RegistrationNumber = "D1 V3" });
            //BillEntryList.Add(new MonthlyBillEntry() { DepoName = "D2", RegistrationNumber = "D2 V1" });
            //BillEntryList.Add(new MonthlyBillEntry() { DepoName = "D2", RegistrationNumber = "D2 V2" });
            //BillEntryList.Add(new MonthlyBillEntry() { DepoName = "D2", RegistrationNumber = "D2 V3" });

            try
            {
                var _email = "automation@mis.prangroup.com";
                var _epass = "aaaaAAAA0000";

                SmtpClient sc = new SmtpClient("mail.mis.prangroup.com");
                //SmtpClient sc = new SmtpClient("172.17.4.106");
                sc.EnableSsl = false;
                sc.Credentials = new NetworkCredential(_email, _epass);
                sc.Port = 25;


                MailMessage mail = new MailMessage();
                foreach (var to in Mail_ToList)
                {
                    mail.To.Add(to);
                }
                foreach (var cc in Mail_CcList)
                {
                    mail.CC.Add(cc);
                }
                //mail.To.Add("automation17@mis.prangroup.com");
                mail.From = new MailAddress(_email);
                mail.Subject = Mail_Subject;
                mail.Body = Mail_Body;
                mail.IsBodyHtml = true;


                //# pdf
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                StringReader sr = new StringReader(RenderPartialViewToString("GetMonthlyBillSummuryView", PRG_Type, BillingPeriod, BillingDate, DueDate, BillEntryList));

                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                MemoryStream ms1 = new MemoryStream();
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, ms1);
                pdfDoc.Open();
                htmlparser.Parse(sr);
                pdfDoc.Close();
                byte[] bytes = ms1.ToArray();
                ms1.Close();
                mail.Attachments.Add(new Attachment(new MemoryStream(bytes), "Bill.pdf"));

                //# xl
                MemoryStream ms2 = new MemoryStream(Encoding.ASCII.GetBytes(RenderPartialViewToString("GetMonthlyBillDetailView", PRG_Type, BillingPeriod, BillingDate, DueDate, BillEntryList)));
                mail.Attachments.Add(new Attachment(ms2, "Detail.xls", "application/vnd.ms-excel"));
                sc.Send(mail);

                return "mail-sent";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string RenderPartialViewToString(string viewName, string PRG_Type, string BillingPeriod, string BillingDate, string DueDate, List<MonthlyBillEntry> BillEntryList)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");
            ViewBag.PRG_Type = PRG_Type;
            ViewBag.BillingPeriod = BillingPeriod;
            ViewBag.BillingDate = BillingDate;
            ViewBag.DueDate = DueDate;
            ViewBag.BillEntryList = BillEntryList;
            Int64 totalAmount = BillEntryList.Count() * 200;
            ViewBag.totalAmountInWord = SpellCurrency(totalAmount);

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                var res = sw.GetStringBuilder().ToString();
                return res;
            }
        }
        public ActionResult GetMonthlyBillSummuryView(string PRG_Type, string BillingPeriod, string BillingDate, string DueDate, List<MonthlyBillEntry> BillEntryList)
        {
            ViewBag.PRG_Type = PRG_Type;
            ViewBag.BillingPeriod = BillingPeriod;
            ViewBag.BillingDate = BillingDate;
            ViewBag.DueDate = DueDate;
            ViewBag.BillEntryList = BillEntryList;
            Int64 totalAmount = BillEntryList.Count() * 200;
            ViewBag.totalAmountInWord = SpellCurrency(totalAmount);

            return View();
        }
        public ActionResult GetMonthlyBillDetailView(List<MonthlyBillEntry> BillEntryList)
        {
            ViewBag.BillEntryList = BillEntryList;
            return View();
        }

        public ActionResult _GetMonthlyBillSummuryView()
        {
            var BillEntryList = new List<MonthlyBillEntry>();
            BillEntryList.Add(new MonthlyBillEntry() { DepoName = "D1", RegistrationNumber = "D1 V1" });
            BillEntryList.Add(new MonthlyBillEntry() { DepoName = "D1", RegistrationNumber = "D1 V2" });
            BillEntryList.Add(new MonthlyBillEntry() { DepoName = "D1", RegistrationNumber = "D1 V3" });
            BillEntryList.Add(new MonthlyBillEntry() { DepoName = "D2", RegistrationNumber = "D2 V1" });
            BillEntryList.Add(new MonthlyBillEntry() { DepoName = "D2", RegistrationNumber = "D2 V2" });
            BillEntryList.Add(new MonthlyBillEntry() { DepoName = "D2", RegistrationNumber = "D2 V3" });
            ViewBag.PRG_Type = "Test";
            ViewBag.BillingPeriod = "Test-";
            ViewBag.BillingDate = "20-Test-19";
            ViewBag.DueDate = "25-Test-19";
            ViewBag.BillEntryList = BillEntryList;
            Int64 totalAmount = BillEntryList.Count() * 200;
            ViewBag.totalAmountInWord = SpellCurrency(totalAmount);
            return View("GetMonthlyBillSummuryView");
        }


        #region Money In Word
        public string SpellDigit(Int64 digit)
        {
            var dict = new Dictionary<Int64, string>();
            dict.Add(0, ""); dict.Add(1, "One "); dict.Add(2, "Two "); dict.Add(3, "Three "); dict.Add(4, "Four ");
            dict.Add(5, "Five "); dict.Add(6, "Six "); dict.Add(7, "Seven "); dict.Add(8, "Eight "); dict.Add(9, "Nine ");

            dict.Add(11, "Eleven "); dict.Add(12, "Twelve "); dict.Add(13, "Thirteen "); dict.Add(14, "Fourteen "); dict.Add(15, "Fifteen ");
            dict.Add(16, "Sixteen "); dict.Add(17, "Seventeen "); dict.Add(18, "Eighteen "); dict.Add(19, "Nineteen ");

            dict.Add(20, "Twenty "); dict.Add(30, "Thirty "); dict.Add(40, "Fourty "); dict.Add(50, "Fifty "); dict.Add(60, "Sixty "); dict.Add(70, "Seventy "); dict.Add(80, "Eighty "); dict.Add(90, "Ninety ");

            return dict[digit];
        }

        public string SpellNumber(Int64 digit)
        {
            var res = "";
            if (digit > 20)
            {
                Int64 decimalNumber = digit / 10;
                Int64 unitNumber = digit % 10;
                res = SpellDigit(decimalNumber * 10) + SpellDigit(unitNumber);
            }
            else
            {
                res = SpellDigit(digit);
            }
            return res;
        }

        public string SpellCurrency(Int64 number)
        {
            var res = "";
            if (number >= 10000000)
            {
                Int64 _digit = number / 10000000;
                res = res + SpellNumber(_digit) + "Crore ";
                number = number % 10000000;
            }
            if (number >= 100000)
            {
                Int64 _digit = number / 100000;
                res = res + SpellNumber(_digit) + "Lakh ";
                number = number % 100000;
            }
            if (number >= 1000)
            {
                Int64 _digit = number / 1000;
                res = res + SpellNumber(_digit) + "Thousand ";
                number = number % 1000;
            }
            if (number >= 100)
            {
                Int64 _digit = number / 100;
                res = res + SpellNumber(_digit) + "Hundred ";
                number = number % 100;
            }
            if (number > 0)
            {
                Int64 _digit = number;
                res = res + SpellNumber(_digit);
                number = number % 100;
            }
            return res;
        }
        #endregion
    }
}