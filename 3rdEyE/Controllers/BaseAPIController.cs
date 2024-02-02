using _3rdEyE.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace _3rdEyE.Controllers
{
    public class BaseAPIController : Controller
    {
        public DBEnityModelContainer db = new DBEnityModelContainer();

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

        public string SendSMS(string numbers, string message)
        {
            string content = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://sms.prangroup.com/postman/api/sendsms?userid=210077&password=22a95dc8997b24bd752b71e04aca8705&msisdn=" + numbers + "&masking=28585&message=" + message + "&unicode=false");//pass=prg@1234
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
                content = e.Message;
            }
            return content;
        }

        public void SendMail_Single(string receiverMail, string mailSubject, string mailBody_HTML)
        {
            //var _email = "automation@mis.prangroup.com";
            //var _epass = "aaaaAAAA0000";
            try
            {
                //MailMessage mail = new MailMessage();
                //mail.To.Add(receiverMail);
                //mail.From = new MailAddress(_email);
                //mail.Subject = mailSubject;
                //mail.Body = mailBody_HTML;
                //mail.IsBodyHtml = true;
                //SmtpClient sc = new SmtpClient("mail.mis.prangroup.com");
                ////SmtpClient sc = new SmtpClient("172.17.4.106");
                //sc.EnableSsl = false;
                //sc.Credentials = new NetworkCredential(_email, _epass);
                //sc.Port = 25;
                //sc.Send(mail);
            }
            catch (Exception e)
            {
                db.AppErrorLogs.Add(
                  new AppErrorLog()
                  {
                      ErrorMessage = e.Message,
                      ErrorTime = DateTime.Now,
                      UserDefinedMessage = "Base/SendMail_Single"
                  }
                  );
                db.SaveChanges();
            }
        }

        public string SendFCMNotification(string token, string title, string message, string id)
        {
            try
            {
                string url = "http://172.17.2.112/fcm.php";
                WebClient wc = new WebClient();
                //wc.QueryString.Add("token", "ExponentPushToken[GeTzHpE5arh_zLTFWZaTBT]");
                //wc.QueryString.Add("title", "parameter 3 value.");
                //wc.QueryString.Add("message", "www.stopbyte.com");
                //wc.QueryString.Add("id", "6");
                wc.QueryString.Add("token", token);
                wc.QueryString.Add("title", title);
                wc.QueryString.Add("message", message);
                wc.QueryString.Add("id", id);
                var data = wc.UploadValues(url, "POST", wc.QueryString);
                var responseString = UnicodeEncoding.UTF8.GetString(data);
                return responseString;
            }
            catch (Exception e)
            {
                //db.AppErrorLogs.Add(
                //    new AppErrorLog()
                //    {
                //        ErrorMessage = e.Message,
                //        ErrorTime = DateTime.Now,
                //        UserDefinedMessage = "API/SendFCM_Notification_Multiple"
                //    }
                //    );
                //db.SaveChanges();
                return "Error";
            }
        }

        public string SendFCM_Notification_Single_New(string token, string title, string message, string id, string type)
        {
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
            }
            catch (Exception e)
            {
                db.AppErrorLogs.Add(
                    new AppErrorLog()
                    {
                        ErrorMessage = e.Message,
                        ErrorTime = DateTime.Now,
                        UserDefinedMessage = "Base/SendFCM_Notification_Single_New"
                    }
                    );
                db.SaveChanges();
                return "Error";
            }
        }
    }
}