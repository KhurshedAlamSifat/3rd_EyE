using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using _3rdEyE.Models;
using _3rdEyE.ManagingTools;
using System.Globalization;
using System.Net;
using System.Text;
using System.IO;

namespace _3rdEyE.Controllers
{
    public class DairyVehicleManagementController : BaseController
    {
        public ActionResult UpdateList()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var list = bll.db.Vehicles.Where(m => m.OWN_MHT_DHT == "OWN" && m.Internal_ShowTemperature == true && m.GpsIMEINumber != null && (m.Depo.Category == "Dairy" || m.Depo.Category == "Tasty/Mithai")).ToList();
            return View(list);
        }
        [HttpPost]
        public ActionResult UpdateList(FormCollection form)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var item_total = Convert.ToInt32(form["item_total"]);
            for (int i = 1; i <= item_total; i++)
            {
                var _Destination = form["Destination_" + i];
                var _StartTime = form["StartTime_" + i];
                var _EndTime = form["EndTime_" + i];
                if (!string.IsNullOrEmpty(_StartTime) && !string.IsNullOrEmpty(_EndTime))
                {
                    DairyVehicle dairyVehicle;// = new DairyVehicle()
                    var _PK_DairyVehicle = Guid.Parse(form["PK_DairyVehicle_" + i]);
                    dairyVehicle = bll.db.DairyVehicles.Where(m => m.PK_DairyVehicle == _PK_DairyVehicle).FirstOrDefault();
                    if (dairyVehicle == null)
                    {
                        dairyVehicle = new DairyVehicle()
                        {
                            PK_DairyVehicle = Guid.Parse(form["PK_DairyVehicle_" + i]),
                            Destination = form["Destination_" + i],
                            StartTime = DateTime.ParseExact(form["StartTime_" + i], "yyyy-MM-dd h:mm tt", CultureInfo.InvariantCulture),
                            EndTime = DateTime.ParseExact(form["EndTime_" + i], "yyyy-MM-dd h:mm tt", CultureInfo.InvariantCulture),
                            FK_AppUser_CreatedBy = CurrentUser.PK_User,
                            CreatedAt = DateTime.Now
                        };
                        bll.db.DairyVehicles.Add(dairyVehicle);
                    }
                    else
                    {
                        dairyVehicle.PK_DairyVehicle = Guid.Parse(form["PK_DairyVehicle_" + i]);
                        dairyVehicle.Destination = form["Destination_" + i];
                        dairyVehicle.StartTime = DateTime.ParseExact(form["StartTime_" + i], "yyyy-MM-dd h:mm tt", CultureInfo.InvariantCulture);
                        dairyVehicle.EndTime = DateTime.ParseExact(form["EndTime_" + i], "yyyy-MM-dd h:mm tt", CultureInfo.InvariantCulture);
                        dairyVehicle.FK_AppUser_CreatedBy = CurrentUser.PK_User;
                        dairyVehicle.CreatedAt = DateTime.Now;
                    }
                }
            }
            bll.db.SaveChanges();
            var list = bll.db.Vehicles.Where(m => m.OWN_MHT_DHT == "OWN" && m.Internal_ShowTemperature == true && m.GpsIMEINumber != null && (m.Depo.Category == "Dairy" || m.Depo.Category == "Tasty/Mithai")).ToList();
            return View(list);
        }


        public void TrySendTemperatureAlertEmail()
        {
            var guid = Guid.NewGuid();
            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "TrySendTemperatureAlertEmail-Start-" + guid,
                      CallingTime = DateTime.Now,
                      UserDefinedMessage = ""
                  }
                  );
            bll.db.SaveChanges();

            var startingTime = DateTime.Now.AddMinutes(-10);
            var DepoCategory = "Dairy";

            if (bll.db.VehicleTrackings.Where(m => m.VehicleTrackingInformation.Vehicle.Depo.Category == DepoCategory &&
            m.VehicleTrackingInformation.Vehicle.Internal_ShowTemperature == true &&
            m.VehicleTrackingInformation.Vehicle.LocationInOrOut == false &&
            m.UpdateTime > startingTime && m.Speed > 0 && m.Temperature > 9
            ).Any())
            {
                try
                {
                    string mailSubject = "Temperature Limit Exceed Notification (Dairy)";
                    string url = "";
#if !DEBUG
                url = "http://172.17.9.160/DairyVehicleManagement/TemperatureAlertEmailBodyGenerator?DepoCategory="+DepoCategory;
#else
                    url = "http://localhost:1089/DairyVehicleManagement/TemperatureAlertEmailBodyGenerator?DepoCategory=" + DepoCategory;
#endif
                    WebRequest request = WebRequest.Create(url);
                    request.Timeout = int.MaxValue;
                    WebResponse response = request.GetResponse();
                    string mailBody_HTML = "";
                    using (System.IO.Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                        mailBody_HTML = reader.ReadToEnd();
                    }

                    var MailToList = new List<string>() { 
                        "automation17@mis.prangroup.com",
                        "Distic@prangroup.com",
                        "piptpt10@pip.prangroup.com",
                        "dairydist22@pip.prangroup.com",
                        "piptpt82@pip.prangroup.com",
                        "pipdist30@pip.prangroup.com",
                        "pipdist38@pip.prangroup.com",
                        "dist100@prangroup.com",
                        "piptpt8@pip.prangroup.com"
                    };
                    var MailCCList = new List<string>() { };
                    SendMail_Multiple(MailToList, MailCCList, mailSubject, mailBody_HTML);
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
                            CallingMessage = "TrySendTemperatureAlertEmail-Error" + guid,
                            UserDefinedMessage = errrorMessage
                        }
                     );
                    bll.db.SaveChanges();
                }
            }

            DepoCategory = "Tasty/Mithai";
            if (bll.db.VehicleTrackings.Where(m => m.VehicleTrackingInformation.Vehicle.Depo.Category == DepoCategory &&
            m.VehicleTrackingInformation.Vehicle.Internal_ShowTemperature == true &&
            m.VehicleTrackingInformation.Vehicle.LocationInOrOut == false &&
            m.UpdateTime > startingTime && m.Speed > 0 && m.Temperature > 9
            ).Any())
            {
                try
                {
                    string mailSubject = "Temperature Limit Exceed Notification(Tasty/Mithai)";
                    string url = "";
#if !DEBUG
                url = "http://172.17.9.160/DairyVehicleManagement/TemperatureAlertEmailBodyGenerator?DepoCategory="+DepoCategory;
#else
                    url = "http://localhost:1089/DairyVehicleManagement/TemperatureAlertEmailBodyGenerator?DepoCategory=" + DepoCategory;
#endif
                    WebRequest request = WebRequest.Create(url);
                    request.Timeout = int.MaxValue;
                    WebResponse response = request.GetResponse();
                    string mailBody_HTML = "";
                    using (System.IO.Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                        mailBody_HTML = reader.ReadToEnd();
                    }

                    var MailToList = new List<string>() { "automation17@mis.prangroup.com",
                        "bbltt17@prangroup.com",
                        "pran782@prangroup.com"
                    };
                    var MailCCList = new List<string>() { };
                    SendMail_Multiple(MailToList, MailCCList, mailSubject, mailBody_HTML);
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
                            CallingMessage = "TrySendTemperatureAlertEmail-Error" + guid,
                            UserDefinedMessage = errrorMessage
                        }
                     );
                    bll.db.SaveChanges();
                }
            }

            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "TrySendTemperatureAlertEmail-End" + guid,
                      CallingTime = DateTime.Now,
                  }
                  );
            bll.db.SaveChanges();
        }
        public ActionResult TemperatureAlertEmailBodyGenerator(string DepoCategory)
        {
            var startingTime = DateTime.Now.AddMinutes(-10);
            var list = bll.db.VehicleTrackings.Where(m => m.VehicleTrackingInformation.Vehicle.Depo.Category == DepoCategory &&
            m.VehicleTrackingInformation.Vehicle.Internal_ShowTemperature == true &&
            m.VehicleTrackingInformation.Vehicle.LocationInOrOut == false &&
            m.UpdateTime > startingTime && m.Speed > 0 && m.Temperature > 9
            ).ToList();
            return View(list);
        }
    }
}