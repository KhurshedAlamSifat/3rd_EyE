using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _3rdEyE.Models;
using _3rdEyE.BLL;
using _3rdEyE.ManagingTools;
using System.Globalization;
using System.IO;
using System.Web.Script.Serialization;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace _3rdEyE.Controllers
{
    public class _APIController : BaseAPIController
    {
        BaseBLL bll = new BaseBLL();

        #region Access
        [HttpPost]
        public JsonResult Login(AppUser user)
        {
            try
            {
                var currentUser = bll.db.AppUsers.Where(u => u.AppUserType != "EndLevelRequisitionAgent" && u.UniqueIDNumber == user.UniqueIDNumber && u.Password == user.Password && u.IsDeleted == false).Select(
                m =>
                new
                {
                    m.PK_User,
                    m.FullName,
                    UserType = m.AppUserType
                }).FirstOrDefault();

                if (currentUser != null)
                {
                    var _currentUser = bll.db.AppUsers.Where(u => u.PK_User == currentUser.PK_User).FirstOrDefault();
                    _currentUser.FID = user.FID;
                    bll.db.SaveChanges();
                    CommonClass.SetCurrentUser(_currentUser);
                    return Json(new { flag = "YES", data = currentUser }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var dealer = bll.db.Dealers.Where(d => d.DealerCode == user.UniqueIDNumber && d.Password == user.Password).Select(
                    m =>
                    new
                    {
                        PK_User = m.PK_Dealer,
                        FullName = m.DealerCode,
                        FK_Vehicle = m.FK_Vehicle,
                        UserType = "Dealer"
                    }).FirstOrDefault();

                    if (dealer != null)
                    {
                        if (dealer.FK_Vehicle != null && bll.db.VehicleTrackings.Where(vt => vt.PK_Vehicle == dealer.FK_Vehicle).Any())
                        {
                            var vehicleTracking = bll.db.VehicleTrackings.Where(vt => vt.PK_Vehicle == dealer.FK_Vehicle).Select(
                                vt => new
                                {
                                    vt.PK_Vehicle,
                                    vt.VehicleTrackingInformation.Vehicle.RegistrationNumber,
                                    vt.Latitude,
                                    vt.Longitude,
                                    vt.EngineStatus,
                                    vt.Speed
                                }
                                ).FirstOrDefault();
                            return Json(new { flag = "YES", data = dealer, vehicleTracking = vehicleTracking }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { flag = "YES", data = dealer }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { flag = "NO" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { flag = "ERR" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult LogOut()
        {
            SessionClass.ClearAllSession();
            return Json(new { flag = "YES" }, JsonRequestBehavior.AllowGet);
        }

        public string SendFCM_Notification_Single(Guid? PK_AppUser, string Category, string Title, string SubTitle)
        {
            try
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
                        UserDefinedMessage = "API/SendFCM_Notification_Multiple"
                    }
                    );
                bll.db.SaveChanges();
                return "Error";
            }
        }

        public string SendFCM_Notification_Multiple(List<Guid> PK_AppUsers, string Category, string Title, string SubTitle)
        {
            try
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
                        UserDefinedMessage = "API/SendFCM_Notification_Multiple"
                    }
                    );
                bll.db.SaveChanges();
                return "Error";
            }
        }

        #endregion

        #region EndLevelRequisitionAgent
        [HttpPost]
        public JsonResult EndLevelRequisitionAgent_PreCreate(AppUser user)
        {
            try
            {
                Random random = new Random();
                var OTP = random.Next(1000, 9999).ToString();
                var SMSResponse = SendSMS(user.ContactNumber, "Your 3rd Eye Regirstration OTP: " + OTP);
                return Json(new { status = "OK", OTP, SMSResponse }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return Json(new { status = "Error", message = exception.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult EndLevelRequisitionAgent_PostCreate(AppUser model)
        {
            try
            {
                var db_model = bll.db.AppUsers.Where(m => m.PRG_Type == "External" && m.AppUserType == "EndLevelRequisitionAgent" && m.ContactNumber == model.ContactNumber).FirstOrDefault();
                if (db_model == null)
                {
                    db_model = new AppUser();
                    db_model.PK_User = Guid.NewGuid();
                    db_model.IsDeleted = false;
                    db_model.CreatedAt = DateTime.Now;
                    db_model.PRG_Type = "External";
                    db_model.AppUserType = "EndLevelRequisitionAgent";
                    db_model.IsRegistrationCompleted = false;

                    db_model.FullName = model.FullName;
                    db_model.ContactNumber = model.ContactNumber;
                    db_model.UniqueIDNumber = "ELRA" + model.ContactNumber;
                    db_model.FID = model.FID;
                    db_model.IMEI = model.IMEI;
                    bll.db.AppUsers.Add(db_model);
                    bll.db.SaveChanges();
                    return Json(new { status = "OK", db_model.PK_User, message = "EndLevelRequisitionAgent Created" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db_model.FullName = model.FullName;
                    db_model.FID = model.FID;
                    db_model.IMEI = model.IMEI;
                    bll.db.SaveChanges();
                    return Json(new { status = "OK", db_model.PK_User, message = "EndLevelRequisitionAgent Updated" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception exception)
            {
                return Json(new { status = "Error", message = exception.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult EndLevelRequisition_Create(IndividualRequisition model)
        {
            //return Json(model, JsonRequestBehavior.AllowGet);
            //# validation
            string modelValidator = ValidationStatus.OK;

            if (modelValidator != ValidationStatus.OK)
            {
                return Json(new { status = "ValidationFailed", message = modelValidator }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                var individualRequisition = new IndividualRequisition();
                individualRequisition.IsEndLevelRequisition = true;
                individualRequisition.PK_IndividualRequisition = Guid.NewGuid();
                individualRequisition.IsDeleted = false;

                individualRequisition.CreatedAt = DateTime.Now;
                individualRequisition.FK_RequisitionAgent = model.FK_RequisitionAgent;

                individualRequisition.FK_Depo_From = model.FK_Depo_From;
                individualRequisition.StartingLocation = model.StartingLocation;
                individualRequisition.StartingLatitude = model.StartingLatitude;
                individualRequisition.StartingLongitude = model.StartingLongitude;
                individualRequisition.FK_Depo_To = model.FK_Depo_To;
                individualRequisition.FinishingLocation = model.FinishingLocation;
                individualRequisition.FinishingLatitude = model.FinishingLatitude;
                individualRequisition.FinishingLongitude = model.FinishingLongitude;
                //individualRequisition.VehicleType = model.VehicleType;
                individualRequisition.VehicleTypeLayer1 = model.VehicleTypeLayer1;
                individualRequisition.VehicleTypeLayer1_english = bll.db.RequisitionVehicleTypes.Where(m => m.Layer1 == model.VehicleTypeLayer1).Select(m => m.Layer1_english).FirstOrDefault();
                individualRequisition.VehicleTypeLayer1_bangla = bll.db.RequisitionVehicleTypes.Where(m => m.Layer1 == model.VehicleTypeLayer1).Select(m => m.Layer1_bangla).FirstOrDefault();
                individualRequisition.VehicleTypeLayer2 = model.VehicleTypeLayer2;
                individualRequisition.VehicleTypeLayer2_english = bll.db.RequisitionVehicleTypes.Where(m => m.Layer2 == model.VehicleTypeLayer2).Select(m => m.Layer2_english).FirstOrDefault();
                individualRequisition.VehicleTypeLayer2_bangla = bll.db.RequisitionVehicleTypes.Where(m => m.Layer2 == model.VehicleTypeLayer2).Select(m => m.Layer2_bangla).FirstOrDefault();
                individualRequisition.VehicleTypeLayer3 = model.VehicleTypeLayer3;
                individualRequisition.VehicleTypeLayer3_english = bll.db.RequisitionVehicleTypes.Where(m => m.Layer3 == model.VehicleTypeLayer3).Select(m => m.Layer3_english).FirstOrDefault();
                individualRequisition.VehicleTypeLayer3_bangla = bll.db.RequisitionVehicleTypes.Where(m => m.Layer3 == model.VehicleTypeLayer3).Select(m => m.Layer3_bangla).FirstOrDefault();


                individualRequisition.WantedCount = model.WantedCount;
                individualRequisition.PossibleJourneyStartDateTime = model.PossibleJourneyStartDateTime;
                individualRequisition.Status = 0;

                bll.db.IndividualRequisitions.Add(individualRequisition);
                //# notification for own depo's agent
                var notificatoinList = new List<RequisitionAgentNotification>();
                var biddingList = new List<IndividualRequisitionBidding>();
                List<Guid> FK_FirebaseAppUser = new List<Guid>();
                string NumbersToSMS = "";
                var CurrentUser = bll.db.AppUsers.Where(m => m.PK_User == individualRequisition.FK_RequisitionAgent).FirstOrDefault();
                if (CurrentUser.FK_Depo != null)
                {
                    var ownDepotOtherAgent = bll.db.AppUsers.Where(m => m.IsDeleted == false && m.FK_Depo == CurrentUser.FK_Depo && m.PK_User != CurrentUser.PK_User && (m.AppUserType == "Internal" || m.AppUserType == "External Transport Agent")).ToList();
                    foreach (var agent in ownDepotOtherAgent)
                    {
                        var bid = new IndividualRequisitionBidding()
                        {
                            PK_IndividualRequisitionBidding = Guid.NewGuid(),
                            FK_IndividualRequisition = individualRequisition.PK_IndividualRequisition,
                            FK_RequisitionAgent_Bidder = agent.PK_User,
                            Status = 0
                        };

                        var notification = new RequisitionAgentNotification()
                        {
                            FK_RequisitionAgent = agent.PK_User,
                            Status = 0,
                            Title = "New requisition by- " + CurrentUser.FullName,
                            ViewLink = "/IndividualRequisitionBidding/ViewForBidder_ToBid?PK_IndividualRequisitionBidding=" + bid.PK_IndividualRequisitionBidding,
                            SubTitle = "Agent- " + CurrentUser.FullName + " of" + " Depot-" + CurrentUser.Depo.Name + " wants " + individualRequisition.WantedCount + " " + individualRequisition.VehicleTypeLayer1_english + ":" + individualRequisition.VehicleTypeLayer2_english + ":" + individualRequisition.VehicleTypeLayer3_english + ". From:" + individualRequisition.StartingLocation + ". To:" + individualRequisition.FinishingLocation + ". On: " + individualRequisition.PossibleJourneyStartDateTime.ToString("dd/MM/yyyy H:mm"),
                            CreatedAt = DateTime.Now,
                            Category = RequistionNotificationCategory.Individual_Requisition_Bid_Created
                        };
                        notificatoinList.Add(notification);
                        biddingList.Add(bid);
                        if (!string.IsNullOrEmpty(agent.ContactNumber))
                        {
                            NumbersToSMS = NumbersToSMS + agent.ContactNumber + ",";
                        }
                    }
                    if (ownDepotOtherAgent.Count > 0)
                    {
                        FK_FirebaseAppUser.AddRange(ownDepotOtherAgent.Select(m => m.PK_User).ToList());
                    }
                }

                //# notification for proposed depot's agent
                var proposedDepots = bll.db.RequisitionAgentProposedDepoes.Where(m => m.FK_RequisitionAgent == CurrentUser.PK_User && m.WillPropose == true && m.FK_Depo != CurrentUser.FK_Depo).ToList();
                foreach (var depo in proposedDepots)
                {
                    var otherDeposAgents = bll.db.AppUsers.Where(m => m.IsDeleted == false && m.FK_Depo == depo.FK_Depo && m.PK_User != CurrentUser.PK_User && (m.AppUserType == "Internal" || m.AppUserType == "External Transport Agent")).ToList();
                    foreach (var agent in otherDeposAgents)
                    {
                        if (notificatoinList.Where(m => m.FK_RequisitionAgent == agent.PK_User).Any())
                        {
                            continue;
                        }
                        var bid = new IndividualRequisitionBidding()
                        {
                            PK_IndividualRequisitionBidding = Guid.NewGuid(),
                            FK_IndividualRequisition = individualRequisition.PK_IndividualRequisition,
                            FK_RequisitionAgent_Bidder = agent.PK_User,
                            Status = 0
                        };

                        var notification = new RequisitionAgentNotification()
                        {
                            FK_RequisitionAgent = agent.PK_User,
                            Status = 0,
                            Title = "New requisition by- " + CurrentUser.FullName,
                            ViewLink = "/IndividualRequisitionBidding/ViewForBidder_ToBid?PK_IndividualRequisitionBidding=" + bid.PK_IndividualRequisitionBidding,
                            SubTitle = "Agent- " + CurrentUser.FullName + " of" + " Depot-" + CurrentUser.Depo.Name + " wants " + individualRequisition.WantedCount + " " + individualRequisition.VehicleTypeLayer1_english + ":" + individualRequisition.VehicleTypeLayer2_english + ":" + individualRequisition.VehicleTypeLayer3_english + ". From:" + individualRequisition.StartingLocation + ". To:" + individualRequisition.FinishingLocation + ". On: " + individualRequisition.PossibleJourneyStartDateTime.ToString("dd/MM/yyyy H:mm"),
                            CreatedAt = DateTime.Now,
                            Category = RequistionNotificationCategory.Individual_Requisition_Created
                        };
                        notificatoinList.Add(notification);
                        biddingList.Add(bid);
                        if (!string.IsNullOrEmpty(agent.ContactNumber))
                        {
                            NumbersToSMS = NumbersToSMS + agent.ContactNumber + ",";
                        }
                    }
                    if (otherDeposAgents.Count > 0)
                    {
                        FK_FirebaseAppUser.AddRange(otherDeposAgents.Select(m => m.PK_User).ToList());
                    }
                }
                bll.db.IndividualRequisitionBiddings.AddRange(biddingList);
                bll.db.RequisitionAgentNotifications.AddRange(notificatoinList);
                bll.db.SaveChanges();

                //# SMS
                if (!string.IsNullOrEmpty(NumbersToSMS) && NumbersToSMS.Last() == ',')
                {
                    NumbersToSMS = NumbersToSMS.Substring(0, NumbersToSMS.Length - 1);
                }
                string messageBody = "Agent- " + CurrentUser.FullName + " of" + " Depot-" + CurrentUser.Depo.Name + " wants " + individualRequisition.WantedCount + " " + individualRequisition.VehicleTypeLayer1_english + ":" + individualRequisition.VehicleTypeLayer2_english + ":" + individualRequisition.VehicleTypeLayer3_english + ". From:" + individualRequisition.StartingLocation + ". To:" + individualRequisition.FinishingLocation + ". On: " + individualRequisition.PossibleJourneyStartDateTime.ToString("dd/MM/yyyy H:mm") + "\ncall for detail:" + CurrentUser.ContactNumber;
                //var sms_response = SendSMS(NumbersToSMS, messageBody);

                //# Firebase notifier
                var Title = "New requisition by- " + CurrentUser.FullName;
                var SubTitle = "Agent- " + CurrentUser.FullName + " of" + " Depot-" + CurrentUser.Depo.Name + " wants " + individualRequisition.WantedCount + " " + individualRequisition.VehicleTypeLayer1_english + ":" + individualRequisition.VehicleTypeLayer2_english + ":" + individualRequisition.VehicleTypeLayer3_english + ". From:" + individualRequisition.StartingLocation + ". To:" + individualRequisition.FinishingLocation + ". On: " + individualRequisition.PossibleJourneyStartDateTime.ToString("dd/MM/yyyy H:mm");
                var Category = RequistionNotificationCategory.Individual_Requisition_Created;
                var fcm_response = SendFCM_Notification_Multiple(FK_FirebaseAppUser, Category, Title, SubTitle);

                return Json(new { status = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return Json(new { status = "Error", message = exception.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult EndLevelRequisition_GetVehicleTracking(Guid PK_IndividualRequisition)
        {
            List<Dictionary<string, string>> dictioneryList = new List<Dictionary<string, string>>();
            DataTable dataTable = new DataTable();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter adpt = new SqlDataAdapter();
            cmd.Connection = (SqlConnection)bll.db.Database.Connection;
            string query = "";
            query = "EXEC Report_GetVehicleTrackingForIndividualRequisition '" + PK_IndividualRequisition + "'";
            cmd.CommandText = query;
            adpt.SelectCommand = cmd;
            dataTable.Reset();
            adpt.Fill(dataTable);
            dictioneryList.AddRange(GetTableRows(dataTable));
            return Json(dictioneryList, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region Depo
        public JsonResult GetDepoes_All()
        {
            var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
            var list = bll.db.Depoes.Where(m => m.IsDeleted == false && m.PK_Depo != _invalidDepoPK && m.Latitude != null && m.Longitude != null).Select(m => new
            {
                m.PK_Depo,
                m.Name,
                m.Latitude,
                m.Longitude
            }).OrderBy(m => m.Name).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Requisition_Common
        Dictionary<string, string> VehicleTypesDict = new Dictionary<string, string> { { "Ambulance", "Ambulance" }, { "Bus", "Bus" }, { "Cargo Truck", "Cargo Truck" }, { "Cargo Truck - Open", "Cargo Truck - Open" }, { "Cargo VAN", "Cargo VAN" }, { "Open VAN", "Open VAN" }, { "Concrete Mixer", "Concrete Mixer" }, { "Covered Van", "Covered Van" }, { "Delivery Van", "Delivery Van" }, { "Milk Tanker", "Milk Tanker" }, { "Mini Bus", "Mini Bus" }, { "Mini Truck", "Mini Truck" }, { "Mobile Crance", "Mobile Crance" }, { "Motor Car", "Motor Car" }, { "OMNI Bus", "OMNI Bus" }, { "Pickup", "Pickup" }, { "Refrigerator Van", "Refrigerator Van" }, { "Tank Lorry", "Tank Lorry" }, { "Tipper", "Tipper" }, { "Trailers", "Trailers" }, { "Water Tanker", "Water Tanker" } };
        Dictionary<double, string> VehicleCapacityDict = new Dictionary<double, string> { { 0.8, "0.8 ton" }, { 1, "1 ton" }, { 1.5, "1.5 tons" }, { 2, "2 tons" }, { 3, "3 tons" }, { 5, "5 tons" }, { 7, "7 tons" }, { 12, "12 tons" }, { 20, "20 tons" } };

        public JsonResult GetInitialData()
        {
            var RequisitionVehicleTypeList = new
            {
                Layer1 = bll.db.RequisitionVehicleTypes.GroupBy(m => m.Layer1).Select(m => new { Key = m.Key, VehicleTypeLayer1 = m.Key, ValueEnglish = m.FirstOrDefault().Layer1_english, ValueBangla = m.FirstOrDefault().Layer1_bangla }).ToList(),
                Layer2 = bll.db.RequisitionVehicleTypes.GroupBy(m => new { m.Layer1, m.Layer2 }).Select(m => new { ParentKey = m.FirstOrDefault().Layer1, Key = m.FirstOrDefault().Layer1 + "#" + m.FirstOrDefault().Layer2, VehicleTypeLayer2 = m.Key.Layer2, ValueEnglish = m.FirstOrDefault().Layer2_english, ValueBangla = m.FirstOrDefault().Layer2_bangla }),
                Layer3 = bll.db.RequisitionVehicleTypes.GroupBy(m => new { m.Layer1, m.Layer2, m.Layer3 }).Select(m => new { ParentKey = m.FirstOrDefault().Layer1 + "#" + m.FirstOrDefault().Layer2, Key = m.FirstOrDefault().Layer1 + "#" + m.FirstOrDefault().Layer2 + "#" + m.FirstOrDefault().Layer3, VehicleTypeLayer3 = m.Key.Layer3, ValueEnglish = m.FirstOrDefault().Layer3_english, ValueBangla = m.FirstOrDefault().Layer3_bangla }),
            };
            var VehicleTypeList = VehicleTypesDict.Select(m => new { Id = m.Key, Text = m.Value });
            var VehicleCapacityList = VehicleCapacityDict.Select(m => new { Id = m.Key, Text = m.Value });
            var DriverList = bll.db.Drivers.Where(m => m.IsDeleted != true).Select(m => new { Id = m.PK_Driver, m.Name, m.PhoneNumber }).ToList();
            var VehicleList = bll.db.Vehicles.Where(m => m.IsDeleted != true && m.OWN_MHT_DHT == "OWN").Select(m => new { Id = m.PK_Vehicle, m.RegistrationNumber }).ToList();
            var ContructualCompanyList = bll.db.ContructualRequisitionCompanies.Where(m => m.IsDeleted != true).Select(m => new { m.PK_ContructualRequisitionCompany, m.Name });
            return Json(new { VehicleTypeList, VehicleCapacityList, DriverList, VehicleList, ContructualCompanyList, RequisitionVehicleTypeList }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Requisition_Individual
        [HttpPost]
        public JsonResult IndividualRequisition_Create(IndividualRequisition model)
        {
            //return Json(model, JsonRequestBehavior.AllowGet);
            //# validation
            string modelValidator = ValidationStatus.OK;

            if (modelValidator != ValidationStatus.OK)
            {
                return Json(new { status = "ValidationFailed", message = modelValidator }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                var individualRequisition = new IndividualRequisition();
                individualRequisition.PK_IndividualRequisition = Guid.NewGuid();
                individualRequisition.IsDeleted = false;

                individualRequisition.CreatedAt = DateTime.Now;
                individualRequisition.FK_RequisitionAgent = model.FK_RequisitionAgent;

                individualRequisition.FK_Depo_From = model.FK_Depo_From;
                individualRequisition.StartingLocation = model.StartingLocation;
                individualRequisition.StartingLatitude = model.StartingLatitude;
                individualRequisition.StartingLongitude = model.StartingLongitude;
                individualRequisition.FK_Depo_To = model.FK_Depo_To;
                individualRequisition.FinishingLocation = model.FinishingLocation;
                individualRequisition.FinishingLatitude = model.FinishingLatitude;
                individualRequisition.FinishingLongitude = model.FinishingLongitude;
                //individualRequisition.VehicleType = model.VehicleType;
                individualRequisition.VehicleTypeLayer1 = model.VehicleTypeLayer1;
                individualRequisition.VehicleTypeLayer1_english = bll.db.RequisitionVehicleTypes.Where(m => m.Layer1 == model.VehicleTypeLayer1).Select(m => m.Layer1_english).FirstOrDefault();
                individualRequisition.VehicleTypeLayer1_bangla = bll.db.RequisitionVehicleTypes.Where(m => m.Layer1 == model.VehicleTypeLayer1).Select(m => m.Layer1_bangla).FirstOrDefault();
                individualRequisition.VehicleTypeLayer2 = model.VehicleTypeLayer2;
                individualRequisition.VehicleTypeLayer2_english = bll.db.RequisitionVehicleTypes.Where(m => m.Layer2 == model.VehicleTypeLayer2).Select(m => m.Layer2_english).FirstOrDefault();
                individualRequisition.VehicleTypeLayer2_bangla = bll.db.RequisitionVehicleTypes.Where(m => m.Layer2 == model.VehicleTypeLayer2).Select(m => m.Layer2_bangla).FirstOrDefault();
                individualRequisition.VehicleTypeLayer3 = model.VehicleTypeLayer3;
                individualRequisition.VehicleTypeLayer3_english = bll.db.RequisitionVehicleTypes.Where(m => m.Layer3 == model.VehicleTypeLayer3).Select(m => m.Layer3_english).FirstOrDefault();
                individualRequisition.VehicleTypeLayer3_bangla = bll.db.RequisitionVehicleTypes.Where(m => m.Layer3 == model.VehicleTypeLayer3).Select(m => m.Layer3_bangla).FirstOrDefault();


                individualRequisition.WantedCount = model.WantedCount;
                individualRequisition.PossibleJourneyStartDateTime = model.PossibleJourneyStartDateTime;
                individualRequisition.Status = 0;

                bll.db.IndividualRequisitions.Add(individualRequisition);
                //# notification for own depo's agent
                var notificatoinList = new List<RequisitionAgentNotification>();
                var biddingList = new List<IndividualRequisitionBidding>();
                List<Guid> FK_FirebaseAppUser = new List<Guid>();
                string NumbersToSMS = "";
                var CurrentUser = bll.db.AppUsers.Where(m => m.PK_User == individualRequisition.FK_RequisitionAgent).FirstOrDefault();
                if (CurrentUser.FK_Depo != null)
                {
                    var ownDepotOtherAgent = bll.db.AppUsers.Where(m => m.IsDeleted == false && m.FK_Depo == CurrentUser.FK_Depo && m.PK_User != CurrentUser.PK_User && (m.AppUserType == "Internal" || m.AppUserType == "External Transport Agent")).ToList();
                    foreach (var agent in ownDepotOtherAgent)
                    {
                        var bid = new IndividualRequisitionBidding()
                        {
                            PK_IndividualRequisitionBidding = Guid.NewGuid(),
                            FK_IndividualRequisition = individualRequisition.PK_IndividualRequisition,
                            FK_RequisitionAgent_Bidder = agent.PK_User,
                            Status = 0
                        };

                        var notification = new RequisitionAgentNotification()
                        {
                            FK_RequisitionAgent = agent.PK_User,
                            Status = 0,
                            Title = "New requisition by- " + CurrentUser.FullName,
                            ViewLink = "/IndividualRequisitionBidding/ViewForBidder_ToBid?PK_IndividualRequisitionBidding=" + bid.PK_IndividualRequisitionBidding,
                            SubTitle = "Agent- " + CurrentUser.FullName + " of" + " Depot-" + CurrentUser.Depo.Name + " wants " + individualRequisition.WantedCount + " " + individualRequisition.VehicleTypeLayer1_english + ":" + individualRequisition.VehicleTypeLayer2_english + ":" + individualRequisition.VehicleTypeLayer3_english + ". From:" + individualRequisition.StartingLocation + ". To:" + individualRequisition.FinishingLocation + ". On: " + individualRequisition.PossibleJourneyStartDateTime.ToString("dd/MM/yyyy H:mm"),
                            CreatedAt = DateTime.Now,
                            Category = RequistionNotificationCategory.Individual_Requisition_Bid_Created
                        };
                        notificatoinList.Add(notification);
                        biddingList.Add(bid);
                        if (!string.IsNullOrEmpty(agent.ContactNumber))
                        {
                            NumbersToSMS = NumbersToSMS + agent.ContactNumber + ",";
                        }
                    }
                    if (ownDepotOtherAgent.Count > 0)
                    {
                        FK_FirebaseAppUser.AddRange(ownDepotOtherAgent.Select(m => m.PK_User).ToList());
                    }
                }

                //# notification for proposed depot's agent
                var proposedDepots = bll.db.RequisitionAgentProposedDepoes.Where(m => m.FK_RequisitionAgent == CurrentUser.PK_User && m.WillPropose == true && m.FK_Depo != CurrentUser.FK_Depo).ToList();
                foreach (var depo in proposedDepots)
                {
                    var otherDeposAgents = bll.db.AppUsers.Where(m => m.IsDeleted == false && m.FK_Depo == depo.FK_Depo && m.PK_User != CurrentUser.PK_User && (m.AppUserType == "Internal" || m.AppUserType == "External Transport Agent")).ToList();
                    foreach (var agent in otherDeposAgents)
                    {
                        if (notificatoinList.Where(m => m.FK_RequisitionAgent == agent.PK_User).Any())
                        {
                            continue;
                        }
                        var bid = new IndividualRequisitionBidding()
                        {
                            PK_IndividualRequisitionBidding = Guid.NewGuid(),
                            FK_IndividualRequisition = individualRequisition.PK_IndividualRequisition,
                            FK_RequisitionAgent_Bidder = agent.PK_User,
                            Status = 0
                        };

                        var notification = new RequisitionAgentNotification()
                        {
                            FK_RequisitionAgent = agent.PK_User,
                            Status = 0,
                            Title = "New requisition by- " + CurrentUser.FullName,
                            ViewLink = "/IndividualRequisitionBidding/ViewForBidder_ToBid?PK_IndividualRequisitionBidding=" + bid.PK_IndividualRequisitionBidding,
                            SubTitle = "Agent- " + CurrentUser.FullName + " of" + " Depot-" + CurrentUser.Depo.Name + " wants " + individualRequisition.WantedCount + " " + individualRequisition.VehicleTypeLayer1_english + ":" + individualRequisition.VehicleTypeLayer2_english + ":" + individualRequisition.VehicleTypeLayer3_english + ". From:" + individualRequisition.StartingLocation + ". To:" + individualRequisition.FinishingLocation + ". On: " + individualRequisition.PossibleJourneyStartDateTime.ToString("dd/MM/yyyy H:mm"),
                            CreatedAt = DateTime.Now,
                            Category = RequistionNotificationCategory.Individual_Requisition_Created
                        };
                        notificatoinList.Add(notification);
                        biddingList.Add(bid);
                        if (!string.IsNullOrEmpty(agent.ContactNumber))
                        {
                            NumbersToSMS = NumbersToSMS + agent.ContactNumber + ",";
                        }
                    }
                    if (otherDeposAgents.Count > 0)
                    {
                        FK_FirebaseAppUser.AddRange(otherDeposAgents.Select(m => m.PK_User).ToList());
                    }
                }
                bll.db.IndividualRequisitionBiddings.AddRange(biddingList);
                bll.db.RequisitionAgentNotifications.AddRange(notificatoinList);
                bll.db.SaveChanges();

                //# SMS
                if (!string.IsNullOrEmpty(NumbersToSMS) && NumbersToSMS.Last() == ',')
                {
                    NumbersToSMS = NumbersToSMS.Substring(0, NumbersToSMS.Length - 1);
                }
                string messageBody = "Agent- " + CurrentUser.FullName + " of" + " Depot-" + CurrentUser.Depo.Name + " wants " + individualRequisition.WantedCount + " " + individualRequisition.VehicleTypeLayer1_english + ":" + individualRequisition.VehicleTypeLayer2_english + ":" + individualRequisition.VehicleTypeLayer3_english + ". From:" + individualRequisition.StartingLocation + ". To:" + individualRequisition.FinishingLocation + ". On: " + individualRequisition.PossibleJourneyStartDateTime.ToString("dd/MM/yyyy H:mm") + "\ncall for detail:" + CurrentUser.ContactNumber;
                //var sms_response = SendSMS(NumbersToSMS, messageBody);

                //# Firebase notifier
                var Title = "New requisition by- " + CurrentUser.FullName;
                var SubTitle = "Agent- " + CurrentUser.FullName + " of" + " Depot-" + CurrentUser.Depo.Name + " wants " + individualRequisition.WantedCount + " " + individualRequisition.VehicleTypeLayer1_english + ":" + individualRequisition.VehicleTypeLayer2_english + ":" + individualRequisition.VehicleTypeLayer3_english + ". From:" + individualRequisition.StartingLocation + ". To:" + individualRequisition.FinishingLocation + ". On: " + individualRequisition.PossibleJourneyStartDateTime.ToString("dd/MM/yyyy H:mm");
                var Category = RequistionNotificationCategory.Individual_Requisition_Created;
                var fcm_response = SendFCM_Notification_Multiple(FK_FirebaseAppUser, Category, Title, SubTitle);

                return Json(new { status = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return Json(new { status = "Error", message = exception.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetIndividualRequistionList_ForClient(Guid Pk_AppUser)
        {
            var list = bll.db.IndividualRequisitions.AsEnumerable().Where(m => m.FK_RequisitionAgent == Pk_AppUser).Select(m => new
            {
                m.PK_IndividualRequisition,
                From = m.StartingLocation,
                To = m.FinishingLocation,
                m.FK_Depo_From,
                m.FK_Depo_To,
                m.VehicleTypeLayer1,
                m.VehicleTypeLayer1_english,
                m.VehicleTypeLayer1_bangla,
                m.VehicleTypeLayer2,
                m.VehicleTypeLayer2_english,
                m.VehicleTypeLayer2_bangla,
                m.VehicleTypeLayer3,
                m.VehicleTypeLayer3_english,
                m.VehicleTypeLayer3_bangla,
                m.WantedCount,
                m.Status,
                BidCount = m.IndividualRequisitionBiddings.Where(n => n.Status == 1).Count()
            }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetIndividualRequisitionBiddingList_Bidded_ByIndividualRequisition(Guid FK_IndividualRequisition)
        {
            var BiddingList = (from bid in bll.db.IndividualRequisitionBiddings.AsEnumerable()
                               where bid.FK_IndividualRequisition == FK_IndividualRequisition
                               && bid.Status == 1
                               select new
                               {
                                   bid.PK_IndividualRequisitionBidding,
                                   ManagableQuantity = bid.ManagableQuantity,
                                   PricePerQuantity = bid.PricePerQuantity,
                                   RequisitionAgent_UserName = bid.AppUser.UniqueIDNumber,
                                   RequisitionAgent_FullName = bid.AppUser.FullName,
                                   RequisitionAgent_ContactNumber = bid.AppUser.ContactNumber,
                                   Depo_Name = bid.AppUser.Depo.Name,
                               }
                               ).ToList();
            return Json(BiddingList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ApproveIndividualRequisition(FormCollection form)
        {
            try
            {
                Guid PK_IndividualRequisition = Guid.Parse(form["PK_IndividualRequisition"]);
                var individualRequisition = bll.db.IndividualRequisitions.Where(m => m.PK_IndividualRequisition == PK_IndividualRequisition).FirstOrDefault();
                var CurrentUser = bll.db.AppUsers.Where(m => m.PK_User == individualRequisition.FK_RequisitionAgent).FirstOrDefault();
                individualRequisition.Status = 1;

                var PK_IndividualRequisitionBiddings = form["PK_IndividualRequisitionBiddings"].TrimEnd(',').Split(',');
                var ApprovedQuantities = form["ApprovedQuantities"].TrimEnd(',').Split(',');

                for (int i = 0; i < PK_IndividualRequisitionBiddings.Length; i++)
                {
                    var _PK_IndividualRequisitionBidding = Guid.Parse(PK_IndividualRequisitionBiddings[i]);
                    var bid = bll.db.IndividualRequisitionBiddings.Where(m => m.PK_IndividualRequisitionBidding == _PK_IndividualRequisitionBidding).FirstOrDefault();
                    bid.Status = 2;
                    bid.ApprovedQuantity = Convert.ToInt32(ApprovedQuantities[i]);
                    //# notification
                    bll.db.RequisitionAgentNotifications.Add(new RequisitionAgentNotification()
                    {
                        FK_RequisitionAgent = bid.FK_RequisitionAgent_Bidder,
                        Status = 0,
                        Title = "Bidding is approved",
                        ViewLink = "/IndividualRequisitionBidding/ViewForBidder_Approved?PK_IndividualRequisitionBidding=" + bid.PK_IndividualRequisitionBidding,
                        SubTitle = "Bidding is approved",
                        CreatedAt = DateTime.Now,
                        Category = RequistionNotificationCategory.Individual_Requisition_Approved
                    });

                    var _agent = bll.db.AppUsers.Where(m => m.PK_User == bid.FK_RequisitionAgent_Bidder).FirstOrDefault();
                    //# SMS
                    if (!string.IsNullOrEmpty(_agent.ContactNumber))
                    {
                        string messageBody = "Agent- " + CurrentUser.FullName + " Approved " + bid.ApprovedQuantity + " " + individualRequisition.VehicleTypeLayer1_english + ":" + individualRequisition.VehicleTypeLayer2_english + ":" + individualRequisition.VehicleTypeLayer3_english + ". " + "\ncall for detail:" + CurrentUser.ContactNumber;
                        //var sms_response = SendSMS(_agent.ContactNumber, messageBody);
                    }

                    //# Firebase notifier
                    var Title = "Bidding is approved";
                    var SubTitle = "Agent- " + CurrentUser.FullName + " Approved " + bid.ApprovedQuantity + " " + individualRequisition.VehicleTypeLayer1_english + ":" + individualRequisition.VehicleTypeLayer2_english + ":" + individualRequisition.VehicleTypeLayer3_english + ". " + "\ncall for detail:" + CurrentUser.ContactNumber;
                    var Category = RequistionNotificationCategory.Individual_Requisition_Approved;
                    var fcm_response = SendFCM_Notification_Single(_agent.PK_User, Category, Title, SubTitle);
                }
                bll.db.SaveChanges();//# Must be here bll.db.SaveChanges(), Once
                var remainingBiddedBidding = bll.db.IndividualRequisitionBiddings.Where(m => m.FK_IndividualRequisition == PK_IndividualRequisition && m.Status == 1).ToList();
                foreach (var bid in remainingBiddedBidding)
                {
                    bid.Status = -2;
                    bll.db.RequisitionAgentNotifications.Add(new RequisitionAgentNotification()
                    {
                        FK_RequisitionAgent = bid.AppUser.PK_User,
                        Status = 0,
                        Title = "Bidding is disapproved",
                        ViewLink = "/IndividualRequisitionBidding/ViewForBidder_Disapproved?PK_IndividualRequisitionBidding=" + bid.PK_IndividualRequisitionBidding,
                        SubTitle = "Bidding is disapproved",
                        CreatedAt = DateTime.Now,
                        Category = RequistionNotificationCategory.Individual_Requisition_Rejected
                    });

                    var _agent = bll.db.AppUsers.Where(m => m.PK_User == bid.FK_RequisitionAgent_Bidder).FirstOrDefault();
                    //# SMS
                    if (!string.IsNullOrEmpty(_agent.ContactNumber))
                    {
                        string messageBody = "Agent- " + CurrentUser.FullName + " rejected " + bid.ApprovedQuantity + " " + individualRequisition.VehicleTypeLayer1_english + ":" + individualRequisition.VehicleTypeLayer2_english + ":" + individualRequisition.VehicleTypeLayer3_english + ". " + "\ncall for detail:" + CurrentUser.ContactNumber;
                        //var sms_response = SendSMS(_agent.ContactNumber, messageBody);
                    }

                    //# Firebase notifier
                    var Title = "Bidding is rejected";
                    var SubTitle = "Agent- " + CurrentUser.FullName + " rejected " + bid.ApprovedQuantity + " " + individualRequisition.VehicleTypeLayer1_english + ":" + individualRequisition.VehicleTypeLayer2_english + ":" + individualRequisition.VehicleTypeLayer3_english + ". " + "\ncall for detail:" + CurrentUser.ContactNumber;
                    var Category = RequistionNotificationCategory.Individual_Requisition_Rejected;
                    var fcm_response = SendFCM_Notification_Single(_agent.PK_User, Category, Title, SubTitle);
                }
                bll.db.SaveChanges();
                return Json(new { status = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return Json(new { status = "Error", message = exception.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetIndividualRequisitionBiddingList_ForBidder(Guid FK_AppUser)
        {
            var list = bll.db.IndividualRequisitionBiddings.AsEnumerable().Where(c => c.FK_RequisitionAgent_Bidder == FK_AppUser && c.Status != -1).Select(c => new
            {
                AgentName = c.AppUser.FullName,
                DepoName = c.AppUser.Depo != null ? c.AppUser.Depo.Name : "",
                TransportCompanyName = c.AppUser.TransportCompany != null ? c.AppUser.TransportCompany.Name : "",
                c.PK_IndividualRequisitionBidding,
                c.IndividualRequisition.FK_Depo_From,
                c.IndividualRequisition.StartingLocation,
                c.IndividualRequisition.StartingLatitude,
                c.IndividualRequisition.StartingLongitude,
                c.IndividualRequisition.FK_Depo_To,
                c.IndividualRequisition.FinishingLocation,
                c.IndividualRequisition.FinishingLatitude,
                c.IndividualRequisition.FinishingLongitude,
                c.IndividualRequisition.VehicleTypeLayer1,
                c.IndividualRequisition.VehicleTypeLayer1_english,
                c.IndividualRequisition.VehicleTypeLayer1_bangla,
                c.IndividualRequisition.VehicleTypeLayer2,
                c.IndividualRequisition.VehicleTypeLayer2_english,
                c.IndividualRequisition.VehicleTypeLayer2_bangla,
                c.IndividualRequisition.VehicleTypeLayer3,
                c.IndividualRequisition.VehicleTypeLayer3_english,
                c.IndividualRequisition.VehicleTypeLayer3_bangla,
                c.IndividualRequisition.WantedCount,
                PossibleJourneyStartDateTime = c.IndividualRequisition.PossibleJourneyStartDateTime.ToShortDateString() + " " + c.IndividualRequisition.PossibleJourneyStartDateTime.ToShortTimeString(),
                c.Status
            }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult IndividualRequisition_ToBid(IndividualRequisitionBidding model)
        {
            try
            {
                var db_model = bll.db.IndividualRequisitionBiddings.Find(model.PK_IndividualRequisitionBidding);
                if (db_model.IndividualRequisition.Status != 0)
                {
                    return Json(new { status = "Unsuccess", message = "Sorry, the Requisition could not be bidded as the requisition is not open anymore" }, JsonRequestBehavior.AllowGet);
                }
                db_model.Status = 1;
                db_model.ManagableQuantity = model.ManagableQuantity;
                db_model.PricePerQuantity = model.PricePerQuantity;
                bll.db.RequisitionAgentNotifications.Add(new RequisitionAgentNotification()
                {
                    FK_RequisitionAgent = db_model.IndividualRequisition.AppUser.PK_User,
                    Status = 0,
                    Title = "New bid by -" + db_model.AppUser.FullName + " [" + db_model.AppUser.UniqueIDNumber + "]",
                    ViewLink = "/IndividualRequisition/ViewForClient_ToApprove?PK_IndividualRequisition=" + db_model.FK_IndividualRequisition,
                    SubTitle = "New bid by -" + db_model.AppUser.FullName + " [" + db_model.AppUser.UniqueIDNumber + "] " + db_model.ManagableQuantity + " " + db_model.IndividualRequisition.VehicleTypeLayer1_english + ":" + db_model.IndividualRequisition.VehicleTypeLayer2_english + ":" + db_model.IndividualRequisition.VehicleTypeLayer3_english + ", BDT " + db_model.PricePerQuantity + " each",
                    CreatedAt = DateTime.Now,
                    Category = RequistionNotificationCategory.Individual_Requisition_Bid_Created
                });
                bll.db.SaveChanges();

                //# SMS
                var NumbersToSMS = db_model.IndividualRequisition.AppUser.ContactNumber;
                if (!string.IsNullOrEmpty(NumbersToSMS) && NumbersToSMS.Last() == ',')
                {
                    NumbersToSMS = NumbersToSMS.Substring(0, NumbersToSMS.Length - 1);
                }
                string messageBody = "New bid by -" + db_model.AppUser.FullName + " [" + db_model.AppUser.UniqueIDNumber + "] " + db_model.ManagableQuantity + " " + db_model.IndividualRequisition.VehicleTypeLayer1_english + ":" + db_model.IndividualRequisition.VehicleTypeLayer2_english + ":" + db_model.IndividualRequisition.VehicleTypeLayer3_english + ", BDT " + db_model.PricePerQuantity + " each";
                //var sms_response = SendSMS(NumbersToSMS, messageBody);

                //# Firebase notifier
                var FK_FirebaseAppUser = db_model.IndividualRequisition.AppUser.PK_User;
                var Title = "New bid by -" + db_model.AppUser.FullName + " [" + db_model.AppUser.UniqueIDNumber + "]";
                var SubTitle = "New bid by -" + db_model.AppUser.FullName + " [" + db_model.AppUser.UniqueIDNumber + "] " + db_model.ManagableQuantity + " " + db_model.IndividualRequisition.VehicleTypeLayer1_english + ":" + db_model.IndividualRequisition.VehicleTypeLayer2_english + ":" + db_model.IndividualRequisition.VehicleTypeLayer3_english + ", BDT " + db_model.PricePerQuantity + " each";
                var Category = RequistionNotificationCategory.Individual_Requisition_Bid_Created;
                var fcm_response = SendFCM_Notification_Single(FK_FirebaseAppUser, Category, Title, SubTitle);
                return Json(new { status = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return Json(new { status = "Error", message = exception.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Requisition_Contructual
        [HttpPost]
        public JsonResult ContructualRequisition_Create(FormCollection form)
        {
            ContructualRequisition model = new ContructualRequisition();
            try
            {
                model.PK_ContructualRequisition = Guid.NewGuid();
                model.FK_RequisitionAgent = Guid.Parse(form["FK_RequisitionAgent"]);
                model.FK_ContructualRequisitionCompany = Guid.Parse(form["FK_ContructualRequisitionCompany"]);
                model.ContructAcitivatingDate = Convert.ToDateTime(form["ContructAcitivatingDate"]);
                model.ContructDeactivatingDate = Convert.ToDateTime(form["ContructDeactivatingDate"]);
                var requisitionAgent = bll.db.AppUsers.Where(m => m.PK_User == model.FK_RequisitionAgent).FirstOrDefault();
                if (requisitionAgent != null)
                {
                    model.REF_FK_Depo = requisitionAgent.FK_Depo;
                }
                model.CreatedAt = DateTime.Now;

                var contructualRequisitionDetailList = new List<ContructualRequisitionDetail>();
                if (!string.IsNullOrEmpty(form["VehicleTypeLayer1"])
                && !string.IsNullOrEmpty(form["VehicleTypeLayer2"])
                && !string.IsNullOrEmpty(form["VehicleTypeLayer3"])
                && !string.IsNullOrEmpty(form["StartingLocation"])
                && !string.IsNullOrEmpty(form["FinishingLocation"])
                && !string.IsNullOrEmpty(form["PricePerVehicle"]))
                {
                    var contructualRequisitionDetail = new ContructualRequisitionDetail();
                    contructualRequisitionDetail.PK_ContructualRequisitionDetail = Guid.NewGuid();
                    contructualRequisitionDetail.FK_ContructualRequisition = model.PK_ContructualRequisition;
                    contructualRequisitionDetail.IsDeleted = false;
                    contructualRequisitionDetail.VehicleTypeLayer1 = form["VehicleTypeLayer1"];
                    contructualRequisitionDetail.VehicleTypeLayer1_english = bll.db.RequisitionVehicleTypes.Where(m => m.Layer1 == contructualRequisitionDetail.VehicleTypeLayer1).Select(m => m.Layer1_english).FirstOrDefault();
                    contructualRequisitionDetail.VehicleTypeLayer1_bangla = bll.db.RequisitionVehicleTypes.Where(m => m.Layer1 == contructualRequisitionDetail.VehicleTypeLayer1).Select(m => m.Layer1_bangla).FirstOrDefault();

                    contructualRequisitionDetail.VehicleTypeLayer2 = form["VehicleTypeLayer2"];
                    contructualRequisitionDetail.VehicleTypeLayer2_english = bll.db.RequisitionVehicleTypes.Where(m => m.Layer2 == contructualRequisitionDetail.VehicleTypeLayer2).Select(m => m.Layer2_english).FirstOrDefault();
                    contructualRequisitionDetail.VehicleTypeLayer2_bangla = bll.db.RequisitionVehicleTypes.Where(m => m.Layer2 == contructualRequisitionDetail.VehicleTypeLayer2).Select(m => m.Layer2_bangla).FirstOrDefault();

                    contructualRequisitionDetail.VehicleTypeLayer3 = form["VehicleTypeLayer3"];
                    contructualRequisitionDetail.VehicleTypeLayer3_english = bll.db.RequisitionVehicleTypes.Where(m => m.Layer3 == contructualRequisitionDetail.VehicleTypeLayer3).Select(m => m.Layer3_english).FirstOrDefault();
                    contructualRequisitionDetail.VehicleTypeLayer3_bangla = bll.db.RequisitionVehicleTypes.Where(m => m.Layer3 == contructualRequisitionDetail.VehicleTypeLayer3).Select(m => m.Layer3_bangla).FirstOrDefault();

                    contructualRequisitionDetail.StartingLocation = form["StartingLocation"];
                    contructualRequisitionDetail.FinishingLocation = form["FinishingLocation"];
                    contructualRequisitionDetail.PricePerVehicle = Convert.ToInt64(form["PricePerVehicle"]);
                    contructualRequisitionDetailList.Add(contructualRequisitionDetail);
                }
                if (contructualRequisitionDetailList.Count() == 0)
                {
                    return Json(new { status = "ValidationFailed", message = "contructualRequisitionDetailList in empty" }, JsonRequestBehavior.AllowGet);
                }
                bll.db.ContructualRequisitions.Add(model);
                bll.db.ContructualRequisitionDetails.AddRange(contructualRequisitionDetailList);

                bll.db.SaveChanges();
                return Json(new { status = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return Json(new { status = "Error", message = exception.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetContructualRequisitionListForContructualAgent(Guid Id)
        {
            var contructualAgent = bll.db.AppUsers.Where(m => m.PK_User == Id).FirstOrDefault();
            var today = DateTime.Today;
            var ContructualRequisitionList = (from cont in bll.db.ContructualRequisitions
                                              where cont.FK_ContructualRequisitionCompany == contructualAgent.FK_ContructualRequisitionCompany && today >= cont.ContructAcitivatingDate && today <= cont.ContructDeactivatingDate
                                              join contDet in bll.db.ContructualRequisitionDetails on cont.PK_ContructualRequisition equals contDet.FK_ContructualRequisition
                                              where contDet.IsDeleted != true
                                              select new
                                              {
                                                  PK_ContructualRequisitionDetail = contDet.PK_ContructualRequisitionDetail,
                                                  contDet.VehicleTypeLayer1,
                                                  contDet.VehicleTypeLayer1_english,
                                                  contDet.VehicleTypeLayer1_bangla,
                                                  contDet.VehicleTypeLayer2,
                                                  contDet.VehicleTypeLayer2_english,
                                                  contDet.VehicleTypeLayer2_bangla,
                                                  contDet.VehicleTypeLayer3,
                                                  contDet.VehicleTypeLayer3_english,
                                                  contDet.VehicleTypeLayer3_bangla,
                                                  StartingLocation = contDet.StartingLocation,
                                                  FinishingLocation = contDet.FinishingLocation
                                              }).ToList();
            return Json(new { ContructualRequisitionList }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetContructualRequisitionEntryListForContructualAgent(Guid Id)
        {
            var contructualAgent = bll.db.AppUsers.Where(m => m.PK_User == Id).FirstOrDefault();
            var list = bll.db.ContructualRequisitionDetailEntries.AsEnumerable().Where(m => m.Status == 0 && m.ContructualRequisitionDetail.ContructualRequisition.FK_ContructualRequisitionCompany == contructualAgent.FK_ContructualRequisitionCompany).Select(m => new
            {
                PK_ContructualRequisitionDetailEntry = m.PK_ContructualRequisitionDetailEntry,
                ContructualRequisitionCompany = m.ContructualRequisitionDetail.ContructualRequisition.ContructualRequisitionCompany.Name,
                ExecutionDate = CommonClass.ConvertToDateString(m.ExecutionDate),
                ContructualAgentName = m.AppUser.FullName,
                ContructualAgentContactNumber = m.AppUser.ContactNumber,
                StartingLocation = m.ContructualRequisitionDetail.StartingLocation,
                FinishingLocation = m.ContructualRequisitionDetail.FinishingLocation,
                m.ContructualRequisitionDetail.VehicleTypeLayer1,
                m.ContructualRequisitionDetail.VehicleTypeLayer1_english,
                m.ContructualRequisitionDetail.VehicleTypeLayer1_bangla,
                m.ContructualRequisitionDetail.VehicleTypeLayer2,
                m.ContructualRequisitionDetail.VehicleTypeLayer2_english,
                m.ContructualRequisitionDetail.VehicleTypeLayer2_bangla,
                m.ContructualRequisitionDetail.VehicleTypeLayer3,
                m.ContructualRequisitionDetail.VehicleTypeLayer3_english,
                m.ContructualRequisitionDetail.VehicleTypeLayer3_bangla,
                VehicleCount = m.VehicleCount
            }).ToList();
            return Json(new { list }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Create_ContructualRequisitionDetailEntry(ContructualRequisitionDetailEntry model)
        {
            //# validation
            string modelValidator = ValidationStatus.OK;

            if (modelValidator != ValidationStatus.OK)
            {
                return Json(new { status = "ValidationFailed", message = modelValidator }, JsonRequestBehavior.AllowGet);
            }

            try
            {

                var db_model = new ContructualRequisitionDetailEntry();
                db_model.PK_ContructualRequisitionDetailEntry = Guid.NewGuid();
                db_model.FK_ContructualRequisitionDetail = model.FK_ContructualRequisitionDetail;
                db_model.VehicleCount = model.VehicleCount;
                db_model.ExecutionDate = model.ExecutionDate;
                db_model.Status = 0;
                db_model.FK_AppUser_AppliedBy = model.FK_AppUser_AppliedBy;
                db_model.AppliedAt = DateTime.Now;
                bll.db.ContructualRequisitionDetailEntries.Add(db_model);
                //# notification for concerned internal agent
                var CurrentUser = bll.db.AppUsers.Where(m => m.PK_User == model.FK_AppUser_AppliedBy).FirstOrDefault();
                var notificatoinList = new List<RequisitionAgentNotification>();
                if (CurrentUser.ContructualRequisitionCompany != null)
                {
                    var contractualCompanyInternalAgents = bll.db.AppUsers.Where(m => m.IsDeleted == false && m.FK_Depo == CurrentUser.FK_Depo && m.PK_User != CurrentUser.PK_User && (m.AppUserType == "Internal" || m.AppUserType == "External Transport Agent")).ToList();
                    var contructualRequisitionDetail = bll.db.ContructualRequisitionDetails.Where(m => m.PK_ContructualRequisitionDetail == db_model.FK_ContructualRequisitionDetail).FirstOrDefault();
                    var NumbersToSMS = "";
                    foreach (var agent in contractualCompanyInternalAgents)
                    {
                        var notification = new RequisitionAgentNotification()
                        {
                            FK_RequisitionAgent = agent.PK_User,
                            Status = 0,
                            Title = "Cotractual vehicle request by- " + CurrentUser.FullName,
                            ViewLink = "/ContructualRequisition/Approve_ContructualRequisitionDetailEntry?PK_ContructualRequisitionDetailEntry=" + db_model.PK_ContructualRequisitionDetailEntry,
                            SubTitle = "Agent- " + CurrentUser.FullName + " " + "Contractual company-" + CurrentUser.ContructualRequisitionCompany.Name + ". needs " + db_model.VehicleCount + " " + contructualRequisitionDetail.VehicleTypeLayer1_english + ". ",
                            CreatedAt = DateTime.Now,
                            Category = RequistionNotificationCategory.Individual_Requisition_Created
                        };
                        notificatoinList.Add(notification);
                        if (!string.IsNullOrEmpty(agent.ContactNumber))
                        {
                            NumbersToSMS = NumbersToSMS + agent.ContactNumber + ",";
                        }
                    }
                    if (!string.IsNullOrEmpty(NumbersToSMS) && NumbersToSMS.Last() == ',')
                    {
                        NumbersToSMS = NumbersToSMS.Substring(0, NumbersToSMS.Length - 1);
                    }
                    string messageBody = "Agent- " + CurrentUser.FullName + " " + "Contractual company-" + CurrentUser.ContructualRequisitionCompany.Name + ". needs " + db_model.VehicleCount + " " + contructualRequisitionDetail.VehicleTypeLayer1_english + " : " + contructualRequisitionDetail.VehicleTypeLayer2_english + " : " + contructualRequisitionDetail.VehicleTypeLayer3_english + ". ";
                    //var response = SendSMS(NumbersToSMS, messageBody);
                }

                bll.db.SaveChanges();
                return Json(new { status = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return Json(new { status = "Error", message = exception.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetContructualRequisitionEntryListForInternalAgent(Guid Id)
        {
            var internalAgent = bll.db.AppUsers.Where(m => m.PK_User == Id).FirstOrDefault();
            var surpervisedContructualCompanies = internalAgent.AppUserSurpervisedContructualCompanies.Select(cc => cc.FK_ContructualRequisitionCompany).ToList();
            var list = bll.db.ContructualRequisitionDetailEntries.Where(m => m.Status == 0 && surpervisedContructualCompanies.Contains(m.ContructualRequisitionDetail.ContructualRequisition.FK_ContructualRequisitionCompany)).Select(m => new
            {
                PK_ContructualRequisitionDetailEntry = m.PK_ContructualRequisitionDetailEntry,
                ContructualRequisitionCompany = m.ContructualRequisitionDetail.ContructualRequisition.ContructualRequisitionCompany.Name,
                ExecutionDate = CommonClass.ConvertToDateString(m.ExecutionDate),
                ContructualAgentName = m.AppUser.FullName,
                ContructualAgentContactNumber = m.AppUser.ContactNumber,
                StartingLocation = m.ContructualRequisitionDetail.StartingLocation,
                FinishingLocation = m.ContructualRequisitionDetail.FinishingLocation,
                m.ContructualRequisitionDetail.VehicleTypeLayer1,
                m.ContructualRequisitionDetail.VehicleTypeLayer1_english,
                m.ContructualRequisitionDetail.VehicleTypeLayer1_bangla,
                m.ContructualRequisitionDetail.VehicleTypeLayer2,
                m.ContructualRequisitionDetail.VehicleTypeLayer2_english,
                m.ContructualRequisitionDetail.VehicleTypeLayer2_bangla,
                m.ContructualRequisitionDetail.VehicleTypeLayer3,
                m.ContructualRequisitionDetail.VehicleTypeLayer3_english,
                m.ContructualRequisitionDetail.VehicleTypeLayer3_bangla,
                VehicleCount = m.VehicleCount
            }).ToList();
            return Json(new { list }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Approve_ContructualRequisitionDetailEntry(Guid PK_ContructualRequisitionDetailEntry, Guid FK_AppUser_ApprovedBy)
        {
            try
            {
                var model = bll.db.ContructualRequisitionDetailEntries.Where(m => m.PK_ContructualRequisitionDetailEntry == PK_ContructualRequisitionDetailEntry).FirstOrDefault();
                var CurrentUser = bll.db.AppUsers.Where(m => m.PK_User == FK_AppUser_ApprovedBy).FirstOrDefault();
                model.Status = 1;
                model.AppliedAt = DateTime.Now;
                model.FK_AppUser_ApprovedBy = CurrentUser.PK_User;

                //# Notification
                var notification = new RequisitionAgentNotification()
                {
                    FK_RequisitionAgent = model.FK_AppUser_AppliedBy,
                    Status = 0,
                    Title = "Approved Contructual Requsition Request by- " + CurrentUser.FullName,
                    ViewLink = "/ContructualRequisition/ContructualRequisitionDetailList_ExternalAgent?PK_ContructualRequisitionDetailEntry=" + PK_ContructualRequisitionDetailEntry,
                    SubTitle = "Agent- " + CurrentUser.FullName + " " + "Depot-" + CurrentUser.Depo.Name + " Approved Contructual Requsition Request",
                    CreatedAt = DateTime.Now,
                    Category = RequistionNotificationCategory.Contractual_Requisition_Entry_Approved
                };

                var _agent = bll.db.AppUsers.Where(m => m.PK_User == model.FK_AppUser_AppliedBy).FirstOrDefault();
                //# SMS
                if (!string.IsNullOrEmpty(_agent.ContactNumber))
                {
                    string messageBody = "Agent- " + CurrentUser.FullName + " approved contractual requsition" + model.VehicleCount + " " + model.ContructualRequisitionDetail.VehicleTypeLayer1_english + " : " + model.ContructualRequisitionDetail.VehicleTypeLayer2_english + " : " + model.ContructualRequisitionDetail.VehicleTypeLayer3_english + ". " + "\ncall for detail:" + CurrentUser.ContactNumber;
                    //var sms_response = SendSMS(_agent.ContactNumber, messageBody);
                }

                //# Firebase notifier
                var Title = "Contractual Requisition is approved";
                var SubTitle = "Agent- " + CurrentUser.FullName + " " + "Depot-" + CurrentUser.Depo.Name + " approved contractual requsition" + model.VehicleCount + " " + model.ContructualRequisitionDetail.VehicleTypeLayer1_english + " : " + model.ContructualRequisitionDetail.VehicleTypeLayer2_english + " : " + model.ContructualRequisitionDetail.VehicleTypeLayer3_english + ". " + "\ncall for detail:" + CurrentUser.ContactNumber;
                var Category = RequistionNotificationCategory.Contractual_Requisition_Entry_Approved;
                var fcm_response = SendFCM_Notification_Single(_agent.PK_User, Category, Title, SubTitle);

                bll.db.SaveChanges();
                return Json(new { status = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                return Json(new { status = "Error", message = exception.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Requisition_Instant
        [HttpPost]
        public JsonResult InstantRequisition_Create(InstantRequisition model)
        {
            return Json(new { status = "OK" }, JsonRequestBehavior.AllowGet);
            //# validation
            //string modelValidator = ValidationStatus.OK;

            //if (modelValidator != ValidationStatus.OK)
            //{
            //    return Json(new { status = "ValidationFailed", message = modelValidator }, JsonRequestBehavior.AllowGet);
            //}

            //try
            //{
            //    //# filter for db_model
            //    var db_model = new InstantRequisition();
            //    db_model.PK_InstantRequisition = Guid.NewGuid();
            //    db_model.IsDeleted = false;
            //    db_model.CreatedAt = DateTime.Now;
            //    db_model.FK_RequisitionAgent = model.FK_RequisitionAgent;
            //    db_model.REF_FK_Depo = bll.db.AppUsers.Where(m => m.PK_User == model.FK_RequisitionAgent).FirstOrDefault().FK_Depo;
            //    db_model.FK_Vehicle = model.FK_Vehicle;
            //    db_model.FK_Driver = model.FK_Driver;
            //    db_model.DriverStaffID = model.DriverStaffID;
            //    db_model.DriverName = model.DriverName;
            //    db_model.StartingLocation = model.StartingLocation;
            //    db_model.StartingLatitude = model.StartingLatitude;
            //    db_model.StartingLongitude = model.StartingLongitude;
            //    db_model.FinishingLocation = model.FinishingLocation;
            //    db_model.FinishingLatitude = model.FinishingLatitude;
            //    db_model.FinishingLongitude = model.FinishingLongitude;
            //    db_model.PriceTotal = model.PriceTotal;
            //    db_model.AdvancedPaymentToDriver = model.AdvancedPaymentToDriver;
            //    db_model.ResponsibleParsonContactNumber = model.ResponsibleParsonContactNumber;
            //    db_model.Note = model.Note;
            //    db_model.IsPaid = false;


            //    bll.db.InstantRequisitions.Add(db_model);
            //    bll.db.SaveChanges();
            //    return Json(new { status = "OK" }, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception exception)
            //{
            //    return Json(new { status = "Error", message = exception.Message }, JsonRequestBehavior.AllowGet);
            //}
        }
        public JsonResult GetDriverInfoFromHRIS(string DriverStaffID)
        {
            var Status = "";
            var res = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://hris.prangroup.com:8686/api/hrisapi.svc/Staff/" + DriverStaffID);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    res = reader.ReadToEnd();
                    if (res == "{\"StaffResult\":\"[]\"}")
                    {
                        Status = "NotFound";
                        return Json(new { Status }, JsonRequestBehavior.AllowGet);
                    }
                    res = res.Replace("\\", "");
                    res = res.Replace("\"[", "[").Replace("]\"", "]");
                    int start = res.IndexOf("NAME") + 7;
                    res = res.Substring(start);
                    int end = res.IndexOf("\"");
                    var Name = res.Substring(0, end);
                    Status = "OK";
                    return Json(new { Status, Name }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Status = "NotConnected";
                return Json(new { Status }, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region Tracking
        public JsonResult GetData(Guid PK_User)
        {
            List<Dictionary<string, string>> dictioneryList = new List<Dictionary<string, string>>();
            DataTable dataTable = new DataTable();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter adpt = new SqlDataAdapter();
            cmd.Connection = (SqlConnection)bll.db.Database.Connection;
            string query = "";
            query = "EXEC Report_GetVehicleTracking '" + PK_User + "'";
            cmd.CommandText = query;
            adpt.SelectCommand = cmd;
            dataTable.Reset();
            adpt.Fill(dataTable);
            dictioneryList.AddRange(GetTableRows(dataTable));
            return Json(dictioneryList, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult GetData_Single_firstTime(Guid PK_Vehicle)
        //{
        //    Dictionary<string, string> dictionery = new Dictionary<string, string>();
        //    DataTable dataTable = new DataTable();
        //    SqlCommand cmd = new SqlCommand();
        //    SqlDataAdapter adpt = new SqlDataAdapter();
        //    cmd.Connection = (SqlConnection)bll.db.Database.Connection;
        //    string query = "";
        //    query = "EXEC Tracking_GetNextDeviceData '" + PK_Vehicle + "', '" + DateTime.Now.AddMinutes(-2).ToString() + "'";
        //    cmd.CommandText = query;
        //    adpt.SelectCommand = cmd;
        //    dataTable.Reset();
        //    adpt.Fill(dataTable);
        //    dictionery = GetTableRows(dataTable).FirstOrDefault();
        //    if (dictionery == null)
        //    {
        //        query = "EXEC Tracking_GetLastDeviceData'" + PK_Vehicle + "'";
        //        cmd.CommandText = query;
        //        adpt.SelectCommand = cmd;
        //        dataTable.Reset();
        //        adpt.Fill(dataTable);
        //        dictionery = GetTableRows(dataTable).FirstOrDefault();
        //    }
        //    return Json(dictionery, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult GetData_Single(Guid PK_Vehicle)
        //{
        //    Dictionary<string, string> dictionery = new Dictionary<string, string>();
        //    DataTable dataTable = new DataTable();
        //    SqlCommand cmd = new SqlCommand();
        //    SqlDataAdapter adpt = new SqlDataAdapter();
        //    cmd.Connection = (SqlConnection)bll.db.Database.Connection;
        //    string query = "";
        //    query = "EXEC Report_GetVehicleTracking_Single '" + PK_Vehicle + "'";
        //    cmd.CommandText = query;
        //    adpt.SelectCommand = cmd;
        //    dataTable.Reset();
        //    adpt.Fill(dataTable);
        //    dictionery = GetTableRows(dataTable).FirstOrDefault();
        //    return Json(dictionery, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult GetData_Single(Guid PK_Vehicle, DateTime? PreviousUpdateTime)
        {
            if (PreviousUpdateTime == null)
            {
                PreviousUpdateTime = DateTime.Now.AddMinutes(-2);
            }
            Dictionary<string, string> dictionery = new Dictionary<string, string>();
            DataTable dataTable = new DataTable();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter adpt = new SqlDataAdapter();
            cmd.Connection = (SqlConnection)bll.db.Database.Connection;
            string query = "";
            query = "EXEC Tracking_GetNextDeviceData '" + PK_Vehicle + "', '" + PreviousUpdateTime.ToString() + "'";
            cmd.CommandText = query;
            adpt.SelectCommand = cmd;
            dataTable.Reset();
            adpt.Fill(dataTable);
            dictionery = GetTableRows(dataTable).FirstOrDefault();
            if (dictionery == null)
            {
                query = "EXEC Tracking_GetLastDeviceData'" + PK_Vehicle + "'";
                cmd.CommandText = query;
                adpt.SelectCommand = cmd;
                dataTable.Reset();
                adpt.Fill(dataTable);
                dictionery = GetTableRows(dataTable).FirstOrDefault();
            }
            return Json(dictionery, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Dealer
        public JsonResult UnassignVehicleForDealer(Guid PK_User)
        {
            try
            {
                var dealer = bll.db.Dealers.Where(d => d.PK_Dealer == PK_User).FirstOrDefault();
                if (dealer != null)
                {
                    dealer.FK_Vehicle = null;
                    bll.db.SaveChanges();
                }
                return Json(new { status = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                bll.db.AppErrorLogs.Add(
                    new AppErrorLog()
                    {
                        ErrorMessage = e.Message,
                        ErrorTime = DateTime.Now,
                        UserDefinedMessage = "API/UnassignVehicleForDealer"
                    }
                    );
                bll.db.SaveChanges();
                return Json(new { status = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult AssignVehicleForDealer(string RegNo)
        {
            var delco = new List<string>() { "1111", "2222", "3333" };

            var list = bll.db.Dealers.Where(dc => delco.Contains(dc.DealerCode)).ToList();
            var vehicle = bll.db.Vehicles.Where(v => v.RegistrationNumber == RegNo).FirstOrDefault();
            foreach (var item in list)
            {
                item.FK_Vehicle = vehicle.PK_Vehicle;
            }
            bll.db.SaveChanges();
            return Json(new { status = "OK" }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
