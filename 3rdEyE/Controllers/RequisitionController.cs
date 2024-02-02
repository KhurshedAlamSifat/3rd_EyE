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
using System.IO;
using _3rdEyE.BLLs;
using System.Globalization;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Text;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System.Configuration;

namespace _3rdEyE.Controllers
{
    public class RequisitionController : BaseController
    {
        List<string> RequisitionStatus = new List<string> { "Created", "Updated", "Accepted", "Rejected" };
        List<string> RequisitionDeliveryTypes = new List<string> { "Depot", "Party", "Export", "Local-Import", "Local-Store", "F-Pump", "SCM", "3rd-Party" };
        List<string> RequisitionProductTypes = new List<string> {"FG","Damage","Accessories","Chemical","Machineries","Raw Material","Trading item",
            "Scrap","Recycle","Servicing Item","PDL-Item","3rd-P-Product","Oil","Livestock","Others"};
        //List<string> LoadingConditions = new List<string> { "Load", "Empty" };
        Dictionary<string, string> PRG_Types_Forwarding_Dict = new Dictionary<string, string> { { "PRAN", "PRAN" }, { "RFL", "RFL" } };
        Dictionary<double, double> WantedQuantityDict = new Dictionary<double, double> { { 0.25, 0.25 }, { 0.5, 0.5 }, { 0.75, 0.75 },
            { 1, 1 },{ 2, 2 }, { 3, 3 },{ 4, 4 }, { 5, 5 },{ 6, 6 }, { 7, 7 },{ 8, 8 },{ 9, 9 }, { 10, 10 },
        { 11, 11 },{ 12, 12 }, { 13, 13 },{ 14, 14 }, { 15, 15 },{ 16, 16 }, { 17, 17 },{ 18, 18 },{ 19, 19 }, { 20, 20 }};
        List<string> NonVehicleTypes = new List<string> { "Troller Boat", "Hand Trolley", "Van", "Adjustment", "By Hand", "Cargo Ship" };
        Dictionary<string, string> MailReceiverGroupDict = new Dictionary<string, string>() { { "automation17@mis.prangroup.com", "SAIMUM" } };
        public RequisitionController()
        {
            bll.db.Database.CommandTimeout = int.MaxValue;

            if (!CommonClass.IsInvalidAccess())
            {
                //PRG_Type = CurrentUser.PRG_Type;
            }
        }
        static class InternalTripStatus
        {
            public const string Created = "Created";
            public const string Assigned = "Assigned";
            public const string EnteredStartingLocation = "Entered Starting Location";
            //public const string StartedLoading = "Started Loading";
            public const string Cancelled = "Cancelled";
            public const string Started = "Started";

            //public const string FinishedLoading = "Finished Loading";
            public const string StartedEmptyTrip = "Started Empty Trip";
            //public const string CreatedBill = "Created Bill";
            //public const string PaidBill = "Paid Bill";
            //public const string LeftStartingLoaction = "Left Starting Loaction";
            public const string EnteredFinishingLocation = "Entered Finishing Location";
            //public const string StartedUnloading = "Started Unloading";

            //public const string FinishedUnloading = "Finished Unloading";
            //public const string FinishedEmptyTrip = "Finished Empty Trip";
            public const string Finished = "Finished";
        }
        static class ExternalTripStatus
        {
            public const string Assigned = "Assigned";
            public const string EnteredStartingLocation = "Entered Starting Location";
            public const string LeftStartingLoaction = "Left Starting Loaction";
            public const string EnteredFinishingLocation = "Entered Finishing Location";
            public const string LeftFinishingLocation = "Left Finishing Location";
        }

        //# Dahsboard
        public ActionResult RequisitionDashBoard()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var starting_limit = DateTime.Now.AddDays(-1).Date;
            var ending_limit = DateTime.Now.Date;

            bll.db.Database.CommandTimeout = int.MaxValue;

            var yesterday_query = bll.db.Requisitions.Include("RequisitionTrips").Where(m => m.IsDeleted != true && m.PossibleJourneyStartDateTime > starting_limit && m.PossibleJourneyStartDateTime < ending_limit);
            if (CurrentUser.PRG_Type != "ALL")
            {
                yesterday_query = yesterday_query.Where(m => m.PRG_Type == CurrentUser.PRG_Type);
            }
            var yesterday_list = yesterday_query.ToList();

            var today_query = bll.db.Requisitions.Include("RequisitionTrips").Where(m => m.IsDeleted != true && m.PossibleJourneyStartDateTime > ending_limit);
            if (CurrentUser.PRG_Type != "ALL")
            {
                today_query = today_query.Where(m => m.PRG_Type == CurrentUser.PRG_Type);
            }
            var today_list = today_query.ToList();

            ViewBag.PRG_Type = CurrentUser.PRG_Type;
            return View(new Tuple<List<Requisition>, List<Requisition>>(yesterday_list, today_list));
        }
        public ActionResult DashBoard_MGT(string PRG_Type)
        {
            if (string.IsNullOrEmpty(PRG_Type))
            {
                PRG_Type = CurrentUser.PRG_Type;
            }
            var starting_limit = DateTime.Now.AddDays(-1);
            var demanded_trip_query = bll.db.RequisitionTrips.Where(m => m.IsDeleted != true && m.Requisition.IsDeleted != true && m.Requisition.PossibleJourneyStartDateTime > starting_limit && m.StatusText != InternalTripStatus.Created);
            if (PRG_Type != "ALL")
            {
                demanded_trip_query = demanded_trip_query.Where(m => m.Requisition.PRG_Type == PRG_Type);
            }
            var demanded_trip_list = demanded_trip_query
                .Include(m => m.Requisition)
                .Include(m => m.Requisition.Location)
                .Include(m => m.Requisition.Location1).ToList();
            ViewBag.PRG_Type = PRG_Type;

            //var towards_trip_list = bll.db.RequisitionTrips.Where(m => m.IsDeleted != true && m.Requisition.IsDeleted != true && m.StatusText == InternalTripStatus.Started && m.StartedAt > starting_limit).Where(m => m.Vehicle != null && (m.Vehicle.FK_RequisitionTrip_CurrentAssigner == null))
            //    .Include(m => m.Requisition)
            //    .Include(m => m.Requisition.Location)
            //    .Include(m => m.Requisition.Location1)
            //    .Include(m => m.Vehicle).ToList();
            var towards_trip_list = new List<RequisitionTrip>();

            return View(new Tuple<List<RequisitionTrip>, List<RequisitionTrip>>(demanded_trip_list, towards_trip_list));
        }
        public ActionResult DashBoard_TPT(String LocationZone)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var starting_limit = DateTime.Now.AddDays(-1);
            var internal_trip = new List<RequisitionTrip>();
            var external_trip = new List<RequisitionTrip>();

            if (LocationZone != null)
            {
                var towardDepoes = bll.db.Locations.Where(m => m.IsDeleted != true && m.LocationZone == LocationZone).Select(m => m.PK_Location).ToList();
                internal_trip = bll.db.RequisitionTrips.Where(m => m.IsDeleted != true && m.Requisition.IsDeleted != true && m.OWN_MHT_DHT == "OWN" && m.StatusText == InternalTripStatus.Started).Where(m => towardDepoes.Contains(m.Requisition.FK_Location_To ?? new Guid()) && m.Vehicle != null && m.StartedAt > starting_limit && (m.Vehicle.FK_RequisitionTrip_CurrentAssigner == null || m.Vehicle.FK_RequisitionTrip_CurrentAssigner == CurrentUser.PK_User)).ToList();
                external_trip = bll.db.RequisitionTrips.Where(m => m.IsDeleted != true && m.Requisition.IsDeleted != true && m.OWN_MHT_DHT != "OWN" && m.StatusText == InternalTripStatus.Started).Where(m => towardDepoes.Contains(m.Requisition.FK_Location_To ?? new Guid()) && m.Vehicle != null && m.StartedAt > starting_limit && (m.Vehicle.FK_RequisitionTrip_CurrentAssigner == null || m.Vehicle.FK_RequisitionTrip_CurrentAssigner == CurrentUser.PK_User)).ToList();
                ViewBag.LocationZones = new SelectList(bll.db.Locations.Where(m => m.IsDeleted != true).Select(m => new { Key = m.LocationZone, Value = m.LocationZone }).Distinct(), "Key", "Value", LocationZone);
            }
            else
            {
                ViewBag.LocationZones = new SelectList(bll.db.Locations.Where(m => m.IsDeleted != true).Select(m => new { Key = m.LocationZone, Value = m.LocationZone }).Distinct(), "Key", "Value", CurrentUser.Depo.LocationZone);
            }
            return View(new Tuple<List<RequisitionTrip>, List<RequisitionTrip>>(internal_trip, external_trip));
        }


        public ActionResult Create_Multi_Import()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            if (CurrentUser.PRG_Type == "ALL")
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", "Your PRG_Type is ALL. Only PRAN/RFL/CS user can creatre requisition ");
                return Redirect("/Home/Index");
            }

            ViewBag.Locations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name", CurrentUser.FK_Location);
            ViewBag.Location_LocationType = bll.db.Locations.Where(m => m.PK_Location == CurrentUser.FK_Location).Select(m => m.LocationType).FirstOrDefault();
            ViewBag.PranOrganizations = new SelectList(bll.db.PranOrganizations.Where(m => m.FK_Location == CurrentUser.FK_Location), "OrganizationCode", "OrganizationName", CurrentUser.FK_Location);
            ViewBag.LocationDepartments_From = new SelectList(bll.db.LocationDepartments.Where(m => m.FK_Location == CurrentUser.FK_Location && m.PRG_Type == CurrentUser.PRG_Type), "PK_LocationDepartment", "DepartmentCode", CurrentUser.FK_Location);
            ViewBag.RequisitionVehicleTypes = new SelectList(bll.db.RequisitionVehicleTypes.Where(m => m.IsDeleted != true), "PK_RequisitionVehicleType", "Title_English");
            ViewBag.WantedQuantityDict = new SelectList(WantedQuantityDict, "Key", "Value");
            ViewBag.MailReceiverGroupDict = new SelectList(MailReceiverGroupDict, "Key", "Value");
            //ViewBag.LoadingConditions = new SelectList(LoadingConditions);

            var _today = DateTime.Now.Date;
            ViewBag.AlreadyOpenedCount = bll.db.Requisitions.Where(m => m.IsDeleted != true && m.FK_AppUser_Client == CurrentUser.PK_User && m.CreatedAt > _today).Count();
            return View();
        }
        [HttpPost]
        public ActionResult Create_Multi_Import(DM_RequisitionList dm_model)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            try
            {
                bll.db.Database.CommandTimeout = 60;
                var model_list = new List<Requisition>();
                foreach (var model in dm_model.RequisitionList)
                {
                    model.IsDeleted = false;
                    model.CreatedAt = DateTime.Now;
                    model.FK_AppUser_Client = CurrentUser.PK_User;
                    model.PRG_Type = CurrentUser.PRG_Type;
                    model.FK_ReferenceDepo = CurrentUser.FK_Depo;

                    if (!string.IsNullOrEmpty(model.OrganizationCode))
                    {
                        model.OrganizationName = bll.db.PranOrganizations.Where(m => m.OrganizationCode == model.OrganizationCode).FirstOrDefault().OrganizationName;
                    }
                    model.StatusText = "Created";

                    model_list.Add(model);
                }
                bll.db.Requisitions.AddRange(model_list);
                bll.db.SaveChanges();
                foreach (var item in model_list)
                {
                    item.TrackingID = "D " + item.PK_Requisition;
                }
                bll.db.SaveChanges();


                //# Save files is server
                foreach (var item in dm_model.RequisitionList)
                {
                    string virtualFolderPath = "";
                    if (item.AttachedFile1 != null || item.AttachedFile2 != null || item.AttachedFile3 != null)
                    {
                        virtualFolderPath = CommonClass.ImageDirectory + "Requisition_Import_Document/" + item.PK_Requisition + "/";
                        //# create folder
                        string physicalFolderPath = Path.Combine(Server.MapPath(virtualFolderPath));
                        if (!Directory.Exists(physicalFolderPath))
                        {
                            Directory.CreateDirectory(physicalFolderPath);
                        }
                    }

                    if (item.AttachedFile1 != null)
                    {
                        string virtualFilePath = virtualFolderPath + "AttachedFile1" + "." + item.AttachedFile1.FileName.Split('.').Last();
                        item.AttachedFile1.SaveAs(Path.Combine(Server.MapPath(virtualFilePath)));
                        item.AttachedFile1_Path = virtualFilePath;
                    }
                    if (item.AttachedFile2 != null)
                    {
                        string virtualFilePath = virtualFolderPath + "AttachedFile2" + "." + item.AttachedFile2.FileName.Split('.').Last();
                        item.AttachedFile2.SaveAs(Path.Combine(Server.MapPath(virtualFilePath)));
                        item.AttachedFile2_Path = virtualFilePath;
                    }
                    if (item.AttachedFile3 != null)
                    {
                        string virtualFilePath = virtualFolderPath + "AttachedFile3" + "." + item.AttachedFile3.FileName.Split('.').Last();
                        item.AttachedFile3.SaveAs(Path.Combine(Server.MapPath(virtualFilePath)));
                        item.AttachedFile3_Path = virtualFilePath;
                    }
                }
                bll.db.SaveChanges();

                //# Send mail
                foreach (var item in dm_model.RequisitionList)
                {
                    SendRequisitionMail(item.PK_Requisition);
                }
                CreateAlertMessage(AlertMessageType.Success, "Success", "Demands are successfully added.");
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
            }
            return RedirectToAction("Create_Multi_Import");
        }
        public void SendRequisitionMail(Int64 PK_Requisition)
        {
            try
            {
                var model = bll.db.Requisitions.Include("Location").Include("Location1").Include("AppUser").Where(m => m.PK_Requisition == PK_Requisition).FirstOrDefault();
                var Mail_Subject = "Demand" + " " + model.StatusText + " " + model.TrackingID;
                var Mail_Body = "Dear Concern<br>";
                Mail_Body = Mail_Body + "Your requisition is " + model.StatusText + " <br>"
                    + "From: " + model.Location.Name + " " + model.StartingLocation + "<br>"
                    + "To: " + model.Location1.Name + " " + model.FinishingLocation + "<br>"
                    + "Wanted quantity: " + model.WantedCount + "<br>"
                    + "Accepted quantity: " + model.AcceptedCount + "<br>"
                    + "Required At: " + CommonClass.ConvertToDateTimeString(model.PossibleJourneyStartDateTime) + "<br>"
                    + "Note: " + model.ClientNote + "<br>";

                MailMessage mail = new MailMessage();
                mail.To.Add("automation17@mis.prangroup.com");
                if (model.MailReceiverGroup != null)
                {
                    mail.To.Add(model.MailReceiverGroup);
                }
                var _email = "automation@mis.prangroup.com";
                var _epass = "aaaaAAAA0000";
                mail.From = new MailAddress(model.AppUser.Email ?? _email);
                mail.Subject = Mail_Subject;
                mail.Body = Mail_Body;
                mail.IsBodyHtml = true;
                if (!string.IsNullOrEmpty(model.AttachedFile1_Path))
                {
                    mail.Attachments.Add(new Attachment(Path.Combine(Server.MapPath(model.AttachedFile1_Path))));
                }
                if (!string.IsNullOrEmpty(model.AttachedFile2_Path))
                {
                    mail.Attachments.Add(new Attachment(Path.Combine(Server.MapPath(model.AttachedFile2_Path))));
                }
                if (!string.IsNullOrEmpty(model.AttachedFile3_Path))
                {
                    mail.Attachments.Add(new Attachment(Path.Combine(Server.MapPath(model.AttachedFile3_Path))));
                }
                SmtpClient sc = new SmtpClient("mail.mis.prangroup.com");
                sc.EnableSsl = false;
                sc.Credentials = new NetworkCredential(_email, _epass);
                sc.Port = 25;
                sc.Send(mail);
                model.MailSentAt = DateTime.Now;
                bll.db.SaveChanges();
            }
            catch (Exception e)
            {

            }
        }

        public class DM_RequisitionList
        {
            public List<Requisition> RequisitionList { get; set; }
        }

        public ActionResult Create_Multi()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            if (CurrentUser.PRG_Type == "ALL")
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", "Your PRG_Type is ALL. Only PRAN/RFL/CS user can creatre requisition ");
                return Redirect("/Home/Index");
            }

            ViewBag.Locations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name", CurrentUser.FK_Location);
            ViewBag.Location_LocationType = bll.db.Locations.Where(m => m.PK_Location == CurrentUser.FK_Location).Select(m => m.LocationType).FirstOrDefault();
            if (CurrentUser.PRG_Type == "PRAN")
            {
                ViewBag.Organizations = new SelectList(bll.db.PranOrganizations.Where(m => m.FK_Location == CurrentUser.FK_Location).OrderBy(m => m.OrganizationCode).Select(m => new { m.OrganizationCode, OrganizationName = m.OrganizationCode + " / " + m.OrganizationName }), "OrganizationCode", "OrganizationName", CurrentUser.FK_Location);
            }
            else
            {
                ViewBag.Organizations = new SelectList(bll.db.RFLOrganizations.Where(m => m.FK_Location == CurrentUser.FK_Location).OrderBy(m => m.OrganizationCode).Select(m => new { m.OrganizationCode, OrganizationName = m.OrganizationCode + " / " + m.OrganizationName }), "OrganizationCode", "OrganizationName", CurrentUser.FK_Location);
            }

            ViewBag.RequisitionDeliveryTypes = new SelectList(RequisitionDeliveryTypes);
            ViewBag.RequisitionProductTypes = new SelectList(RequisitionProductTypes);

            ViewBag.LocationDepartments_From = new SelectList(bll.db.LocationDepartments.Where(m => m.FK_Location == CurrentUser.FK_Location && m.PRG_Type == CurrentUser.PRG_Type), "PK_LocationDepartment", "DepartmentCode", CurrentUser.FK_Location);
            ViewBag.RequisitionVehicleTypes = new SelectList(bll.db.RequisitionVehicleTypes.Where(m => m.IsDeleted != true), "PK_RequisitionVehicleType", "Title_English");
            ViewBag.WantedQuantityDict = new SelectList(WantedQuantityDict, "Key", "Value");
            //ViewBag.LoadingConditions = new SelectList(LoadingConditions);

            var _today = DateTime.Now.Date;
            ViewBag.AlreadyOpenedCount = bll.db.Requisitions.Where(m => m.IsDeleted != true && m.FK_AppUser_Client == CurrentUser.PK_User && m.CreatedAt > _today).Count();
            return View();
        }
        [HttpPost]
        public ActionResult Create_Multi(DM_RequisitionList dm_model)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            try
            {
                bll.db.Database.CommandTimeout = 60;
                var model_list = new List<Requisition>();
                foreach (var model in dm_model.RequisitionList)
                {
                    model.IsDeleted = false;
                    model.CreatedAt = DateTime.Now;
                    model.FK_AppUser_Client = CurrentUser.PK_User;
                    model.PRG_Type = CurrentUser.PRG_Type;
                    model.FK_ReferenceDepo = CurrentUser.FK_Depo;

                    if (!string.IsNullOrEmpty(model.OrganizationCode) && CurrentUser.PRG_Type == "PRAN")
                    {
                        model.OrganizationName = bll.db.PranOrganizations.Where(m => m.OrganizationCode == model.OrganizationCode).FirstOrDefault().OrganizationName;
                    }
                    else if (!string.IsNullOrEmpty(model.OrganizationCode) && CurrentUser.PRG_Type == "RFL")
                    {
                        model.OrganizationName = bll.db.RFLOrganizations.Where(m => m.OrganizationCode == model.OrganizationCode).FirstOrDefault().OrganizationName;
                    }
                    model.StatusText = "Created";

                    model_list.Add(model);
                }
                bll.db.Requisitions.AddRange(model_list);
                bll.db.SaveChanges();
                foreach (var item in model_list)
                {
                    item.TrackingID = "D " + item.PK_Requisition;
                }
                bll.db.SaveChanges();

                CreateAlertMessage(AlertMessageType.Success, "Success", "Demands are successfully added.");
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
            }
            return RedirectToAction("Create_Multi");
        }

        public ActionResult Edit(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.Requisitions.Where(m => m.IsDeleted != true && m.IsDeleted != true && m.PK_Requisition == id).FirstOrDefault();
            ViewBag.Locations1 = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name", model.FK_Location_From);
            if (CurrentUser.PRG_Type == "PRAN")
            {
                ViewBag.Organizations = new SelectList(bll.db.PranOrganizations.Where(m => m.FK_Location == CurrentUser.FK_Location), "OrganizationCode", "OrganizationName", CurrentUser.FK_Location);
            }
            else
            {
                ViewBag.Organizations = new SelectList(bll.db.RFLOrganizations.Where(m => m.FK_Location == CurrentUser.FK_Location), "OrganizationCode", "OrganizationName", CurrentUser.FK_Location);
            }
            ViewBag.LocationDepartments_From = new SelectList(bll.db.LocationDepartments.Where(m => m.FK_Location == model.FK_Location_From), "PK_LocationDepartment", "DepartmentCode", model.FK_LocationDepartment_From);
            ViewBag.Locations2 = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name", model.FK_Location_To);
            ViewBag.LocationDepartments_To = new SelectList(bll.db.LocationDepartments.Where(m => m.FK_Location == model.FK_Location_To), "PK_LocationDepartment", "DepartmentCode", model.FK_LocationDepartment_To);
            ViewBag.RequisitionVehicleTypes = new SelectList(bll.db.RequisitionVehicleTypes.Where(m => m.IsDeleted != true), "PK_RequisitionVehicleType", "Title_English", model.FK_RequisitionVehicleType);
            ViewBag.WantedQuantityDict = new SelectList(WantedQuantityDict, "Key", "Value", model.WantedCount);
            //ViewBag.LoadingConditions = new SelectList(LoadingConditions, model.LoadingCondition);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(FormCollection form)
        {
            var _pk = Convert.ToInt64(form["PK_Requisition"]);
            Requisition model = bll.db.Requisitions.Where(m => m.IsDeleted != true && m.IsDeleted != true && m.PK_Requisition == _pk).FirstOrDefault();
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            try
            {
                if (model.StatusText == "Created" || model.StatusText == "Updated")
                {
                    model.UpdatedAt = DateTime.Now;

                    if (!string.IsNullOrEmpty(form["FK_Location_From"]))
                    {
                        model.FK_Location_From = Guid.Parse(form["FK_Location_From"]);
                    }
                    model.StartingLocation = form["StartingLocation"];
                    if (!string.IsNullOrEmpty(form["FK_Location_To"]))
                    {
                        model.FK_Location_To = Guid.Parse(form["FK_Location_To"]);
                    }
                    if (!string.IsNullOrEmpty(form["OrganizationCode"]) && CurrentUser.PRG_Type == "PRAN")
                    {
                        model.OrganizationCode = form["OrganizationCode"];
                        model.OrganizationName = bll.db.PranOrganizations.Where(m => m.OrganizationCode == model.OrganizationCode).FirstOrDefault().OrganizationName;
                    }
                    else if (!string.IsNullOrEmpty(form["OrganizationCode"]) && CurrentUser.PRG_Type == "RFL")
                    {
                        model.OrganizationCode = form["OrganizationCode"];
                        model.OrganizationName = bll.db.RFLOrganizations.Where(m => m.OrganizationCode == model.OrganizationCode).FirstOrDefault().OrganizationName;
                    }
                    else
                    {
                        model.OrganizationCode = null;
                        model.OrganizationName = null;

                    }
                    if (!string.IsNullOrEmpty(form["FK_LocationDepartment_From"]))
                    {
                        model.FK_LocationDepartment_From = Convert.ToInt64(form["FK_LocationDepartment_From"]);
                    }
                    else
                    {
                        model.FK_LocationDepartment_From = null;
                    }
                    model.FinishingLocation = form["FinishingLocation"];
                    model.FK_RequisitionVehicleType = Convert.ToInt32(form["FK_RequisitionVehicleType"]);
                    model.WantedCount = Convert.ToDouble(form["WantedCount"]);
                    model.PossibleJourneyStartDateTime = DateTime.ParseExact(form["PossibleJourneyStartDateTime"], "yyyy-MM-dd h:mm tt", CultureInfo.InvariantCulture);
                    model.ClientNote = form["ClientNote"];
                    model.StatusText = "Updated";
                    bll.db.SaveChanges();

                    CreateAlertMessage(AlertMessageType.Success, "Success", "Demand is successfully updated.");
                    return RedirectToAction("RequisitionIndex_Client");
                }
                else
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", "Demand could not be updated anymore. Current Status: " + model.StatusText);
                    return RedirectToAction("RequisitionIndex_Client");
                }
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
            }
            ViewBag.Locations1 = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name", model.FK_Location_From);
            if (CurrentUser.PRG_Type == "PRAN")
            {
                ViewBag.Organizations = new SelectList(bll.db.PranOrganizations.Where(m => m.FK_Location == CurrentUser.FK_Location), "OrganizationCode", "OrganizationName", model.OrganizationCode);
            }
            else
            {
                ViewBag.Organizations = new SelectList(bll.db.RFLOrganizations.Where(m => m.FK_Location == CurrentUser.FK_Location), "OrganizationCode", "OrganizationName", model.OrganizationCode);
            }
            ViewBag.LocationDepartments_From = new SelectList(bll.db.LocationDepartments.Where(m => m.FK_Location == model.FK_Location_From), "PK_LocationDepartment", "DepartmentCode", model.FK_LocationDepartment_From);
            ViewBag.Locations2 = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name", model.FK_Location_To);
            ViewBag.RequisitionVehicleTypes = new SelectList(bll.db.RequisitionVehicleTypes.Where(m => m.IsDeleted != true), "PK_RequisitionVehicleType", "Title_English", model.FK_RequisitionVehicleType);
            ViewBag.WantedQuantityDict = new SelectList(WantedQuantityDict, "Key", "Value", model.WantedCount);
            //ViewBag.LoadingConditions = new SelectList(LoadingConditions, model.LoadingCondition);
            return View(model);
        }


        public ActionResult View(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.Requisitions.Where(m => m.IsDeleted != true && m.IsDeleted != true && m.PK_Requisition == id).FirstOrDefault();
            return View(model);
        }

        public ActionResult Approve(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.Requisitions.Where(m => m.IsDeleted != true && m.IsDeleted != true && m.PK_Requisition == id).FirstOrDefault();
            if (model.StatusText == "Created" || model.StatusText == "Updated")
            {
                return View(model);
            }
            else
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", model.TrackingID + " is already " + model.StatusText);
                return RedirectToAction("RequisitionIndex_Approver");
            }
        }
        [HttpPost]
        public ActionResult Approve(FormCollection form)
        {
            var _pk = Convert.ToInt64(form["PK_Requisition"]);
            Requisition model = bll.db.Requisitions.Where(m => m.IsDeleted != true && m.IsDeleted != true && m.PK_Requisition == _pk).FirstOrDefault();
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            try
            {
                if (model.StatusText == "Created" || model.StatusText == "Updated")
                {
                    model.VerifiedAt = DateTime.Now;
                    model.FK_AppUser_Verifier = CurrentUser.PK_User;

                    model.StatusText = form["StatusText"];
                    model.AcceptedCount = Convert.ToDouble(form["AcceptedCount"]);
                    bll.db.SaveChanges();
                    //# internalTrip
                    var stillRequired = model.AcceptedCount;
                    var _internalTripSL = 1;
                    while (model.StatusText == "Accepted" && stillRequired > 0)
                    {
                        var internalTrip = new RequisitionTrip();
                        internalTrip.FK_Requisition = model.PK_Requisition;
                        //internalTrip.FK_Vehicle = Guid.Parse(InternalVehicle.FK_Vehicle);
                        //internalTrip.Driver_Staff_ID = InternalVehicle.Driver_Staff_ID;
                        //internalTrip.Driver_Name = InternalVehicle.Driver_Name;
                        internalTrip.FK_AppUser_Assigner = CurrentUser.PK_User;
                        internalTrip.IsDeleted = false;
                        //internalTrip.AssingedAt = DateTime.Now;
                        if (stillRequired > 1)
                        {
                            internalTrip.WantedCount = 1;
                            stillRequired = stillRequired - 1;
                        }
                        else
                        {
                            internalTrip.WantedCount = stillRequired;
                            stillRequired = 0;
                        }

                        internalTrip.StatusText = InternalTripStatus.Created;
                        //if (_vehicle.LocationInOrOut == true && _vehicle.FK_DepoInOut == Requisition.FK_Depo_From && _vehicle.FK_RequisitionTrip_Pending == null && _vehicle.FK_RequisitionTrip_Current == null)
                        //{
                        //    internalTrip.StatusText = InternalTripStatus.EnteredStartingLocation;
                        //}
                        bll.db.RequisitionTrips.Add(internalTrip);
                        bll.db.SaveChanges();
                        internalTrip.TrackingID = "R " + model.PK_Requisition.ToString() + "-" + _internalTripSL.ToString();
                        _internalTripSL = _internalTripSL + 1;
                        bll.db.SaveChanges();
                    }

                    //# Notify Raiser by Mail
                    if (!string.IsNullOrEmpty(model.AppUser.Email))
                    {
                        var Mail_Subject = "Demand" + " " + model.StatusText + " " + model.TrackingID;
                        var Mail_Body = "Dear Concern<br>";
                        Mail_Body = Mail_Body + "Your requisition is " + model.StatusText + " <br>"
                            + "From: " + model.Location.Name + " " + model.StartingLocation + "<br>"
                            + "To: " + model.Location1.Name + " " + model.FinishingLocation + "<br>"
                            + "Wanted quantity: " + model.WantedCount + "<br>"
                            + "Accepted quantity: " + model.AcceptedCount + "<br>"
                            + "Required At: " + CommonClass.ConvertToDateTimeString(model.PossibleJourneyStartDateTime) + "\n";
                        SendMail_Single(model.AppUser.Email, Mail_Subject, Mail_Body);
                    }

                    CreateAlertMessage(AlertMessageType.Success, "Success", "Demand is successfully verified.");
                    return RedirectToAction("RequisitionIndex_Approver");
                }
                else
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", model.TrackingID + " is already " + model.StatusText);
                    return RedirectToAction("RequisitionIndex_Approver");
                }
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult BulkApprove(FormCollection form)
        {
            var redirectUrl = Request.UrlReferrer.ToString();
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            try
            {
                var AcceptedCounts = form["AcceptedCounts"].Split(',');
                var PK_Requisitions = form["PK_Requisitions"].Split(',');
                for (int i = 0; i < PK_Requisitions.Length; i++)
                {
                    if (string.IsNullOrEmpty(PK_Requisitions[i]))
                    {
                        break;
                    }
                    var _pk = Convert.ToInt64(PK_Requisitions[i]);
                    Requisition model = bll.db.Requisitions.Where(m => m.IsDeleted != true && m.IsDeleted != true && m.PK_Requisition == _pk).FirstOrDefault();
                    if (model.StatusText == "Created" || model.StatusText == "Updated")
                    {
                        model.VerifiedAt = DateTime.Now;
                        model.FK_AppUser_Verifier = CurrentUser.PK_User;

                        model.StatusText = "Accepted";
                        model.AcceptedCount = Convert.ToDouble(AcceptedCounts[i]);
                        //model.ApproverNote = form["ApproverNote"];
                        bll.db.SaveChanges();
                        //# internalTrip
                        var stillRequired = model.AcceptedCount;
                        var _internalTripSL = 1;
                        while (stillRequired != 0)
                        {
                            var internalTrip = new RequisitionTrip();
                            internalTrip.FK_Requisition = model.PK_Requisition;
                            //internalTrip.FK_Vehicle = Guid.Parse(InternalVehicle.FK_Vehicle);
                            //internalTrip.Driver_Staff_ID = InternalVehicle.Driver_Staff_ID;
                            //internalTrip.Driver_Name = InternalVehicle.Driver_Name;
                            internalTrip.FK_AppUser_Assigner = CurrentUser.PK_User;
                            internalTrip.IsDeleted = false;
                            //internalTrip.AssingedAt = DateTime.Now;
                            if (stillRequired > 1)
                            {
                                internalTrip.WantedCount = 1;
                                stillRequired = stillRequired - 1;
                            }
                            else
                            {
                                internalTrip.WantedCount = stillRequired;
                                stillRequired = 0;
                            }

                            internalTrip.StatusText = InternalTripStatus.Created;
                            //if (_vehicle.LocationInOrOut == true && _vehicle.FK_DepoInOut == Requisition.FK_Depo_From && _vehicle.FK_RequisitionTrip_Pending == null && _vehicle.FK_RequisitionTrip_Current == null)
                            //{
                            //    internalTrip.StatusText = InternalTripStatus.EnteredStartingLocation;
                            //}
                            bll.db.RequisitionTrips.Add(internalTrip);
                            bll.db.SaveChanges();
                            internalTrip.TrackingID = "R " + model.PK_Requisition.ToString() + "-" + _internalTripSL.ToString();
                            _internalTripSL = _internalTripSL + 1;
                            bll.db.SaveChanges();
                        }
                        //# Notify Raiser by Mail
                        if (!string.IsNullOrEmpty(model.AppUser.Email))
                        {
                            var Mail_Subject = "Demand" + " " + model.StatusText + " " + model.TrackingID;
                            var Mail_Body = "Dear Concern<br>";
                            Mail_Body = Mail_Body + "Your requisition is " + model.StatusText + " <br>"
                                + "From: " + model.Location.Name + " " + model.StartingLocation + "<br>"
                                + "To: " + model.Location1.Name + " " + model.FinishingLocation + "<br>"
                                + "Wanted quantity: " + model.WantedCount + "<br>"
                                + "Accepted quantity: " + model.AcceptedCount + "<br>"
                                + "Required At: " + CommonClass.ConvertToDateTimeString(model.PossibleJourneyStartDateTime) + "\n";
                            SendMail_Single(model.AppUser.Email, Mail_Subject, Mail_Body);
                        }
                    }
                    else
                    {
                        CreateAlertMessage(AlertMessageType.Warning, "Warning", "Demand could not be verified anymore. Current status: " + model.StatusText);
                    }
                }
                CreateAlertMessage(AlertMessageType.Success, "Success", PK_Requisitions.Length + " Demands are successfully Verified.");
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
            }
            return Redirect(redirectUrl);
        }

        public ActionResult RequisitionIndex_Client(DateTime? StartingDate, DateTime? EndingDate, String TrackingId, Guid? FK_AppUser_Client, Guid? FK_Location_From, Guid? FK_Location_To, String StatusText)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var list = new List<Models.vw_Requisition>();
            var now = DateTime.Now;
            var query = bll.db.vw_Requisition.AsQueryable();
            if (CurrentUser.PRG_Type != "ALL")
            {
                query = query.Where(c => c.PRG_Type == CurrentUser.PRG_Type);
            }
            if (StartingDate != null)
            {
                var _StartingDate = StartingDate != null ? StartingDate : new DateTime();
                query = query.Where(m => m.PossibleJourneyStartDateTime > _StartingDate);
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", _StartingDate);
            }
            else
            {
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", now.Date);
            }
            if (EndingDate != null)
            {
                var _EndingDate = EndingDate != null ? EndingDate : new DateTime();
                query = query.Where(m => m.PossibleJourneyStartDateTime < _EndingDate);
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", _EndingDate);
            }
            else
            {
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", now.AddDays(1).Date);
            }
            if (!string.IsNullOrEmpty(TrackingId))
            {
                query = query.Where(m => m.TrackingID.Contains(TrackingId));
            }
            ViewBag.TrackingId = TrackingId;
            if (FK_AppUser_Client != null)
            {
                query = query.Where(m => m.FK_AppUser_Client == FK_AppUser_Client);
                var starting_limit = DateTime.Now.Date.AddDays(-7);
                var cliesntsQuery = bll.db.Requisitions.Where(m => m.IsDeleted != true && m.CreatedAt > starting_limit);
                if (CurrentUser.PRG_Type != "ALL")
                {
                    cliesntsQuery = cliesntsQuery.Where(m => m.PRG_Type == CurrentUser.PRG_Type);
                }
                var _cliesnts = cliesntsQuery.Select(m => new { m.AppUser.PK_User, Text = m.AppUser.FullName + ":" + m.AppUser.Depo.Name }).Distinct().OrderBy(m => m.Text);
                ViewBag.Clients = new SelectList(_cliesnts, "PK_User", "Text", FK_AppUser_Client);
            }
            else
            {
                var starting_limit = DateTime.Now.Date.AddDays(-7);
                var cliesntsQuery = bll.db.Requisitions.Where(m => m.IsDeleted != true && m.CreatedAt > starting_limit);
                if (CurrentUser.PRG_Type != "ALL")
                {
                    cliesntsQuery = cliesntsQuery.Where(m => m.PRG_Type == CurrentUser.PRG_Type);
                }
                var _cliesnts = cliesntsQuery.Select(m => new { m.AppUser.PK_User, Text = m.AppUser.FullName + ":" + m.AppUser.Depo.Name }).Distinct().OrderBy(m => m.Text);
                ViewBag.Clients = new SelectList(_cliesnts, "PK_User", "Text");
            }
            if (FK_Location_From != null)
            {
                query = query.Where(m => m.FK_Location_From == FK_Location_From);
                ViewBag.FromLocations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name", FK_Location_From);
            }
            else
            {
                ViewBag.FromLocations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name");
            }
            if (FK_Location_To != null)
            {
                query = query.Where(m => m.FK_Location_To == FK_Location_To);
                ViewBag.ToLocations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name", FK_Location_To);
            }
            else
            {
                ViewBag.ToLocations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name");
            }
            if (!string.IsNullOrEmpty(StatusText))
            {
                query = query.Where(m => m.StatusText == StatusText);
                ViewBag.StatusTexts = new SelectList(RequisitionStatus, StatusText);
            }
            else
            {
                ViewBag.StatusTexts = new SelectList(RequisitionStatus, StatusText);
            }
            if (StartingDate != null || EndingDate != null || (!string.IsNullOrEmpty(TrackingId)) || FK_AppUser_Client != null || FK_Location_From != null || FK_Location_To != null || (!string.IsNullOrEmpty(StatusText)))
            {
                list = query.AsQueryable().ToList();
            }
            return View(list);
        }
        public ActionResult RequisitionIndex_Approver(DateTime? StartingDate, DateTime? EndingDate, String TrackingId, Guid? FK_AppUser_Client, Guid? FK_Location_From, Guid? FK_Location_To, String StatusText)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var list = new List<Models.vw_Requisition>();
            var now = DateTime.Now;
            var query = bll.db.vw_Requisition.AsQueryable();
            if (CurrentUser.PRG_Type != "ALL")
            {
                query = query.Where(c => c.PRG_Type == CurrentUser.PRG_Type);
            }
            if (StartingDate != null)
            {
                var _StartingDate = StartingDate != null ? StartingDate : new DateTime();
                query = query.Where(m => m.PossibleJourneyStartDateTime > _StartingDate);
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", _StartingDate);
            }
            else
            {
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", now.Date);
            }
            if (EndingDate != null)
            {
                var _EndingDate = EndingDate != null ? EndingDate : new DateTime();
                query = query.Where(m => m.PossibleJourneyStartDateTime < _EndingDate);
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", _EndingDate);
            }
            else
            {
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", now.AddDays(1).Date);
            }
            if (!string.IsNullOrEmpty(TrackingId))
            {
                query = query.Where(m => m.TrackingID.Contains(TrackingId));
            }
            ViewBag.TrackingId = TrackingId;
            if (FK_AppUser_Client != null)
            {
                query = query.Where(m => m.FK_AppUser_Client == FK_AppUser_Client);
                var starting_limit = DateTime.Now.Date.AddDays(-7);
                var cliesntsQuery = bll.db.Requisitions.Where(m => m.IsDeleted != true && m.CreatedAt > starting_limit);
                if (CurrentUser.PRG_Type != "ALL")
                {
                    cliesntsQuery = cliesntsQuery.Where(m => m.PRG_Type == CurrentUser.PRG_Type);
                }
                var _cliesnts = cliesntsQuery.Select(m => new { m.AppUser.PK_User, Text = m.AppUser.FullName + ":" + m.AppUser.Depo.Name }).Distinct().OrderBy(m => m.Text);
                ViewBag.Clients = new SelectList(_cliesnts, "PK_User", "Text", FK_AppUser_Client);
            }
            else
            {
                var starting_limit = DateTime.Now.Date.AddDays(-7);
                var cliesntsQuery = bll.db.Requisitions.Where(m => m.IsDeleted != true && m.CreatedAt > starting_limit);
                if (CurrentUser.PRG_Type != "ALL")
                {
                    cliesntsQuery = cliesntsQuery.Where(m => m.PRG_Type == CurrentUser.PRG_Type);
                }
                var _cliesnts = cliesntsQuery.Select(m => new { m.AppUser.PK_User, Text = m.AppUser.FullName + ":" + m.AppUser.Depo.Name }).Distinct().OrderBy(m => m.Text);
                ViewBag.Clients = new SelectList(_cliesnts, "PK_User", "Text");
            }
            if (FK_Location_From != null)
            {
                query = query.Where(m => m.FK_Location_From == FK_Location_From);
                ViewBag.FromLocations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name", FK_Location_From);
            }
            else
            {
                ViewBag.FromLocations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name");
            }
            if (FK_Location_To != null)
            {
                query = query.Where(m => m.FK_Location_To == FK_Location_To);
                ViewBag.ToLocations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name", FK_Location_To);
            }
            else
            {
                ViewBag.ToLocations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name");
            }
            if (!string.IsNullOrEmpty(StatusText))
            {
                query = query.Where(m => m.StatusText == StatusText);
                ViewBag.StatusTexts = new SelectList(RequisitionStatus, StatusText);
            }
            else
            {
                ViewBag.StatusTexts = new SelectList(RequisitionStatus, StatusText);
            }
            if (StartingDate != null || EndingDate != null || (!string.IsNullOrEmpty(TrackingId)) || FK_AppUser_Client != null || FK_Location_From != null || FK_Location_To != null || (!string.IsNullOrEmpty(StatusText)))
            {
                list = query.AsQueryable().ToList();
            }
            return View(list);
        }

        //# RequisitionTrip
        public ActionResult RequisitionTripIndex_Client(DateTime? StartingDate, DateTime? EndingDate, String TrackingId, Guid? FK_AppUser_Client, Guid? FK_Location_From, Guid? FK_Location_To, String RegistrationNumber)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var list_RequisitionTrip = new List<vw_RequisitionTrip>();
            var now = DateTime.Now;
            var query_RequisitionTrip = bll.db.vw_RequisitionTrip.AsQueryable();
            if (CurrentUser.PRG_Type != "ALL")
            {
                query_RequisitionTrip = query_RequisitionTrip.Where(c => c.PRG_Type == CurrentUser.PRG_Type || c.PRG_Type_ForwaredTo == CurrentUser.PRG_Type);
            }

            if (StartingDate != null)
            {
                var _StartingDate = StartingDate != null ? StartingDate : new DateTime();
                query_RequisitionTrip = query_RequisitionTrip.Where(m => m.PossibleJourneyStartDateTime > _StartingDate);
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", _StartingDate);
            }
            else
            {
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", now.Date);
            }
            if (EndingDate != null)
            {
                var _EndingDate = EndingDate != null ? EndingDate : new DateTime();
                query_RequisitionTrip = query_RequisitionTrip.Where(m => m.PossibleJourneyStartDateTime < _EndingDate);
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", _EndingDate);
            }
            else
            {
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", now.AddDays(1).Date);
            }
            if (!string.IsNullOrEmpty(TrackingId))
            {
                query_RequisitionTrip = query_RequisitionTrip.Where(m => m.TrackingID.Contains(TrackingId));
            }
            ViewBag.TrackingId = TrackingId;
            if (FK_AppUser_Client != null)
            {
                query_RequisitionTrip = query_RequisitionTrip.Where(m => m.FK_AppUser_Client == FK_AppUser_Client);
                var starting_limit = DateTime.Now.Date.AddDays(-7);
                var cliesntsQuery = bll.db.Requisitions.Where(m => m.IsDeleted != true && m.CreatedAt > starting_limit);
                if (CurrentUser.PRG_Type != "ALL")
                {
                    cliesntsQuery = cliesntsQuery.Where(m => m.PRG_Type == CurrentUser.PRG_Type);
                }
                var _cliesnts = cliesntsQuery.Select(m => new { m.AppUser.PK_User, Text = m.AppUser.FullName + ":" + m.AppUser.Depo.Name }).Distinct().OrderBy(m => m.Text);
                ViewBag.Clients = new SelectList(_cliesnts, "PK_User", "Text", FK_AppUser_Client);
            }
            else
            {
                var starting_limit = DateTime.Now.Date.AddDays(-7);
                var cliesntsQuery = bll.db.Requisitions.Where(m => m.IsDeleted != true && m.CreatedAt > starting_limit);
                if (CurrentUser.PRG_Type != "ALL")
                {
                    cliesntsQuery = cliesntsQuery.Where(m => m.PRG_Type == CurrentUser.PRG_Type);
                }
                var _cliesnts = cliesntsQuery.Select(m => new { m.AppUser.PK_User, Text = m.AppUser.FullName + ":" + m.AppUser.Depo.Name }).Distinct().OrderBy(m => m.Text);
                ViewBag.Clients = new SelectList(_cliesnts, "PK_User", "Text");
            }
            if (FK_Location_From != null)
            {
                query_RequisitionTrip = query_RequisitionTrip.Where(m => (m.FK_Location_From == FK_Location_From || (m.IsForwarded == true && m.FK_Location_ForwardedTo == FK_Location_From)));
                ViewBag.FromLocations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name", FK_Location_From);
            }
            else
            {
                ViewBag.FromLocations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name");
            }
            if (FK_Location_To != null)
            {
                query_RequisitionTrip = query_RequisitionTrip.Where(m => m.FK_Location_To == FK_Location_To);
                ViewBag.ToLocations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name", FK_Location_To);
            }
            else
            {
                ViewBag.ToLocations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name");
            }
            //RegistrationNumber
            if (!string.IsNullOrEmpty(RegistrationNumber))
            {
                query_RequisitionTrip = query_RequisitionTrip.Where(m => m.FK_Vehicle != null && m.RegistrationNumber.Contains(RegistrationNumber));
            }
            ViewBag.RegistrationNumber = RegistrationNumber;

            if (StartingDate != null || EndingDate != null || (!string.IsNullOrEmpty(TrackingId)) || FK_AppUser_Client != null || FK_Location_From != null || FK_Location_To != null || (!string.IsNullOrEmpty(RegistrationNumber)))
            {
                list_RequisitionTrip = query_RequisitionTrip.AsQueryable().ToList();
            }
            return View(list_RequisitionTrip);
        }
        public ActionResult RequisitionTripIndex_Assigner(DateTime? StartingDate, DateTime? EndingDate, String TrackingId, Guid? FK_AppUser_Client, Guid? FK_Location_From, Guid? FK_Location_To, String RegistrationNumber)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var list_RequisitionTrip = new List<vw_RequisitionTrip>();
            var now = DateTime.Now;
            var query_RequisitionTrip = bll.db.vw_RequisitionTrip.AsQueryable();
            if (CurrentUser.PRG_Type != "ALL")
            {
                query_RequisitionTrip = query_RequisitionTrip.Where(c => c.PRG_Type == CurrentUser.PRG_Type || c.PRG_Type_ForwaredTo == CurrentUser.PRG_Type);
            }

            if (StartingDate != null)
            {
                var _StartingDate = StartingDate != null ? StartingDate : new DateTime();
                query_RequisitionTrip = query_RequisitionTrip.Where(m => m.PossibleJourneyStartDateTime > _StartingDate);
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", _StartingDate);
            }
            else
            {
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", now.Date);
            }
            if (EndingDate != null)
            {
                var _EndingDate = EndingDate != null ? EndingDate : new DateTime();
                query_RequisitionTrip = query_RequisitionTrip.Where(m => m.PossibleJourneyStartDateTime < _EndingDate);
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", _EndingDate);
            }
            else
            {
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", now.AddDays(1).Date);
            }
            if (!string.IsNullOrEmpty(TrackingId))
            {
                query_RequisitionTrip = query_RequisitionTrip.Where(m => m.TrackingID.Contains(TrackingId));
            }
            ViewBag.TrackingId = TrackingId;
            if (FK_AppUser_Client != null)
            {
                query_RequisitionTrip = query_RequisitionTrip.Where(m => m.FK_AppUser_Client == FK_AppUser_Client);
                var starting_limit = DateTime.Now.Date.AddDays(-7);
                var cliesntsQuery = bll.db.Requisitions.Where(m => m.IsDeleted != true && m.CreatedAt > starting_limit);
                if (CurrentUser.PRG_Type != "ALL")
                {
                    cliesntsQuery = cliesntsQuery.Where(m => m.PRG_Type == CurrentUser.PRG_Type);
                }
                var _cliesnts = cliesntsQuery.Select(m => new { m.AppUser.PK_User, Text = m.AppUser.FullName + ":" + m.AppUser.Depo.Name }).Distinct().OrderBy(m => m.Text);
                ViewBag.Clients = new SelectList(_cliesnts, "PK_User", "Text", FK_AppUser_Client);
            }
            else
            {
                var starting_limit = DateTime.Now.Date.AddDays(-7);
                var cliesntsQuery = bll.db.Requisitions.Where(m => m.IsDeleted != true && m.CreatedAt > starting_limit);
                if (CurrentUser.PRG_Type != "ALL")
                {
                    cliesntsQuery = cliesntsQuery.Where(m => m.PRG_Type == CurrentUser.PRG_Type);
                }
                var _cliesnts = cliesntsQuery.Select(m => new { m.AppUser.PK_User, Text = m.AppUser.FullName + ":" + m.AppUser.Depo.Name }).Distinct().OrderBy(m => m.Text);
                ViewBag.Clients = new SelectList(_cliesnts, "PK_User", "Text");
            }
            if (FK_Location_From != null)
            {
                query_RequisitionTrip = query_RequisitionTrip.Where(m => (m.FK_Location_From == FK_Location_From || (m.IsForwarded == true && m.FK_Location_ForwardedTo == FK_Location_From)));
                ViewBag.FromLocations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name", FK_Location_From);
            }
            else
            {
                ViewBag.FromLocations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name");
            }
            if (FK_Location_To != null)
            {
                query_RequisitionTrip = query_RequisitionTrip.Where(m => m.FK_Location_To == FK_Location_To);
                ViewBag.ToLocations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name", FK_Location_To);
            }
            else
            {
                ViewBag.ToLocations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name");
            }
            //RegistrationNumber
            if (!string.IsNullOrEmpty(RegistrationNumber))
            {
                query_RequisitionTrip = query_RequisitionTrip.Where(m => m.FK_Vehicle != null && m.RegistrationNumber.Contains(RegistrationNumber));
            }
            ViewBag.RegistrationNumber = RegistrationNumber;

            if (StartingDate != null || EndingDate != null || (!string.IsNullOrEmpty(TrackingId)) || FK_AppUser_Client != null || FK_Location_From != null || FK_Location_To != null || (!string.IsNullOrEmpty(RegistrationNumber)))
            {
                list_RequisitionTrip = query_RequisitionTrip.AsQueryable().ToList();
            }
            return View(list_RequisitionTrip);
        }

        public ActionResult RequisitionTripIndex_Report()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var list_RequisitionTrip = new List<vw_RequisitionTrip>();
            var now = DateTime.Now;
            var query_RequisitionTrip = bll.db.vw_RequisitionTrip.AsQueryable();
            if (CurrentUser.PRG_Type != "ALL")
            {
                query_RequisitionTrip = query_RequisitionTrip.Where(c => c.PRG_Type == CurrentUser.PRG_Type || c.PRG_Type_ForwaredTo == CurrentUser.PRG_Type);
            }

            ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", now.Date);
            ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", now.AddDays(1).Date);
            ViewBag.TrackingId = "";
            var starting_limit = DateTime.Now.Date.AddDays(-7);

            var cliesntsQuery = bll.db.Requisitions.Where(m => m.IsDeleted != true && m.CreatedAt > starting_limit);
            if (CurrentUser.PRG_Type != "ALL")
            {
                cliesntsQuery = cliesntsQuery.Where(m => m.PRG_Type == CurrentUser.PRG_Type);
            }
            var _cliesnts = cliesntsQuery.Select(m => new { m.AppUser.PK_User, Text = m.AppUser.FullName + ":" + m.AppUser.Depo.Name }).Distinct().OrderBy(m => m.Text);

            ViewBag.Clients = new SelectList(_cliesnts, "PK_User", "Text");
            ViewBag.FromLocations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name");
            ViewBag.ToLocations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name");
            ViewBag.RegistrationNumber = "";
            return View(list_RequisitionTrip);
        }
        public ActionResult RequisitionTripIndex_Report_Export(DateTime? StartingDate, DateTime? EndingDate, String TrackingId, Guid? FK_AppUser_Client, Guid? FK_Location_From, Guid? FK_Location_To, String RegistrationNumber)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var list_RequisitionTrip = new List<vw_RequisitionTrip>();
            var query_RequisitionTrip = bll.db.vw_RequisitionTrip.Where(m => m.FK_Vehicle != null).AsQueryable();
            if (CurrentUser.PRG_Type != "ALL")
            {
                query_RequisitionTrip = query_RequisitionTrip.Where(c => c.PRG_Type == CurrentUser.PRG_Type || c.PRG_Type_ForwaredTo == CurrentUser.PRG_Type);
            }

            if (StartingDate != null)
            {
                var _StartingDate = StartingDate != null ? StartingDate : new DateTime();
                query_RequisitionTrip = query_RequisitionTrip.Where(m => m.PossibleJourneyStartDateTime > _StartingDate);
            }
            if (EndingDate != null)
            {
                var _EndingDate = EndingDate != null ? EndingDate : new DateTime();
                query_RequisitionTrip = query_RequisitionTrip.Where(m => m.PossibleJourneyStartDateTime < _EndingDate);
            }
            if (!string.IsNullOrEmpty(TrackingId))
            {
                query_RequisitionTrip = query_RequisitionTrip.Where(m => m.TrackingID.Contains(TrackingId));
            }
            if (FK_AppUser_Client != null)
            {
                query_RequisitionTrip = query_RequisitionTrip.Where(m => m.FK_AppUser_Client == FK_AppUser_Client);
                var starting_limit = DateTime.Now.Date.AddDays(-7);
                var cliesntsQuery = bll.db.Requisitions.Where(m => m.IsDeleted != true && m.CreatedAt > starting_limit);
                if (CurrentUser.PRG_Type != "ALL")
                {
                    cliesntsQuery = cliesntsQuery.Where(m => m.PRG_Type == CurrentUser.PRG_Type);
                }
                var _cliesnts = cliesntsQuery.Select(m => new { m.AppUser.PK_User, Text = m.AppUser.FullName + ":" + m.AppUser.Depo.Name }).Distinct().OrderBy(m => m.Text);
            }
            else
            {
                var starting_limit = DateTime.Now.Date.AddDays(-7);
                var cliesntsQuery = bll.db.Requisitions.Where(m => m.IsDeleted != true && m.CreatedAt > starting_limit);
                if (CurrentUser.PRG_Type != "ALL")
                {
                    cliesntsQuery = cliesntsQuery.Where(m => m.PRG_Type == CurrentUser.PRG_Type);
                }
                var _cliesnts = cliesntsQuery.Select(m => new { m.AppUser.PK_User, Text = m.AppUser.FullName + ":" + m.AppUser.Depo.Name }).Distinct().OrderBy(m => m.Text);
            }
            if (FK_Location_From != null)
            {
                query_RequisitionTrip = query_RequisitionTrip.Where(m => (m.FK_Location_From == FK_Location_From || (m.IsForwarded == true && m.FK_Location_ForwardedTo == FK_Location_From)));
            }
            else
            {
            }
            if (FK_Location_To != null)
            {
                query_RequisitionTrip = query_RequisitionTrip.Where(m => m.FK_Location_To == FK_Location_To);
            }
            //RegistrationNumber
            if (!string.IsNullOrEmpty(RegistrationNumber))
            {
                query_RequisitionTrip = query_RequisitionTrip.Where(m => m.FK_Vehicle != null && m.RegistrationNumber.Contains(RegistrationNumber));
            }

            if (StartingDate != null || EndingDate != null || (!string.IsNullOrEmpty(TrackingId)) || FK_AppUser_Client != null || FK_Location_From != null || FK_Location_To != null || (!string.IsNullOrEmpty(RegistrationNumber)))
            {
                list_RequisitionTrip = query_RequisitionTrip.ToList();
            }
            DataTable dt = new DataTable("sheet1");

            dt.Columns.Add("Tr ID", typeof(string));
            dt.Columns.Add("GP", typeof(string));
            dt.Columns.Add("O/M/D", typeof(string));
            dt.Columns.Add("Using As", typeof(string));
            dt.Columns.Add("Vehicle", typeof(string));
            dt.Columns.Add("Vehicle Type", typeof(string));
            dt.Columns.Add("Capacity Ton", typeof(string));
            dt.Columns.Add("Brand Model", typeof(string));
            dt.Columns.Add("Demand V.Type", typeof(string));
            dt.Columns.Add("From", typeof(string));
            dt.Columns.Add("Fr.Dept", typeof(string));
            dt.Columns.Add("Fr.Detail", typeof(string));
            dt.Columns.Add("To", typeof(string));
            dt.Columns.Add("To Dept", typeof(string));
            dt.Columns.Add("To Detail", typeof(string));
            dt.Columns.Add("Organization", typeof(string));
            dt.Columns.Add("From Site Code", typeof(string));
            dt.Columns.Add("Delivery Type", typeof(string));
            dt.Columns.Add("Product Type", typeof(string));
            dt.Columns.Add("Raiser", typeof(string));
            dt.Columns.Add("Demand Note", typeof(string));
            dt.Columns.Add("LC Nnumber", typeof(string));
            dt.Columns.Add("Job Nnumber", typeof(string));
            dt.Columns.Add("Unit Nnumber", typeof(string));
            dt.Columns.Add("Weight(KG)", typeof(decimal));
            dt.Columns.Add("Required Time", typeof(string));
            dt.Columns.Add("Available At", typeof(string));
            dt.Columns.Add("Transport Agency", typeof(string));
            dt.Columns.Add("Driver Info", typeof(string));
            dt.Columns.Add("Drv.Contact", typeof(string));
            dt.Columns.Add("Total Amnt", typeof(decimal));
            dt.Columns.Add("Assigning Note", typeof(string));
            dt.Columns.Add("Assigner", typeof(string));
            dt.Columns.Add("TravelTime(h:m)", typeof(string));
            dt.Columns.Add("REFF", typeof(string));

            foreach (var item in list_RequisitionTrip)
            {
                DataRow dr = dt.NewRow();
                dr[00] = item.TrackingID;
                dr[01] = item.OracleDB_GPNumber;
                dr[02] = item.OWN_MHT_DHT;
                dr[03] = item.PRG_Type + "-" + item.Trip_PRG_Type;
                dr[04] = item.RegistrationNumber;
                dr[05] = item.VehicleType;
                dr[06] = item.CapacityTon;
                dr[07] = item.VehicleBrand_Name + "-" + item.VehicleModel_Title;
                dr[08] = item.RequisitionVehicleType_Title_English;
                dr[09] = item.Location_From_Name;
                dr[10] = item.Department_From_Code;
                dr[11] = item.StartingLocation;
                dr[12] = item.Location_To_Name;
                dr[13] = item.Department_To_Code;
                dr[14] = item.FinishingLocation;
                dr[15] = item.OrganizationCode;
                dr[16] = item.Location_From_SiteCode;
                dr[17] = item.DeliveryType;
                dr[18] = item.ProductType;
                dr[19] = item.AppUser_Client_UniqueIDNumber + "-" + item.AppUser_Client_FullName;
                dr[20] = item.ClientNote;
                dr[21] = item.LCNumber;
                dr[22] = item.JobNo;
                dr[23] = item.UnitNo;
                if (item.WeightInKG != null)
                {
                    dr[24] = item.WeightInKG;
                }
                dr[25] = item.PossibleJourneyStartDateTime.ToString();
                dr[26] = item.FinalWantedAtDateTime.ToString();
                dr[27] = item.TransportAgency_Name;
                dr[28] = item.Driver_Staff_ID + " " + item.Driver_Name;
                dr[29] = item.Driver_ContactNumber;
                if (item.TotalAmount != null)
                {
                    dr[30] = item.TotalAmount;
                }
                dr[31] = item.AssigningNote;
                dr[32] = item.AppUser_Assigner_UniqueIDNumber + "-" + item.AppUser_Assigner_FullName;
                dr[33] = item.TravelTime;
                dr[34] = item.PK_RequisitionTrip;
                dt.Rows.Add(dr);
            }
            var memoryStream = new MemoryStream();
            using (var excelPackage = new ExcelPackage(memoryStream))
            {
                var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells["A1"].LoadFromDataTable(dt, true, TableStyles.None);
                return File(excelPackage.GetAsByteArray(), "application/octet-stream", "RequisitionTrip_Report.xlsx");
            }
        }


        public ActionResult RequisitionTrip_Forward(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            Session[SessionClass.URLToRedirect] = Request.UrlReferrer;
            var model = bll.db.RequisitionTrips.Where(m => m.PK_RequisitionTrip == id && m.IsDeleted != true && m.IsForwarded != true && (m.StatusText == InternalTripStatus.Created || (m.StatusText == InternalTripStatus.Cancelled && m.OracleDB_IsPushed != true))).FirstOrDefault();
            ViewBag.PRG_Types_Forwarding_Dict = new SelectList(PRG_Types_Forwarding_Dict, "Key", "Value");
            ViewBag.Locations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false && (m.LocationType == "Factory" || m.LocationType == "Depo")).OrderBy(m => m.Name), "PK_Location", "Name");
            return View(model);
        }
        [HttpPost]
        public ActionResult RequisitionTrip_Forward(FormCollection form)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var PK_RequisitionTrip = Convert.ToInt64(form["PK_RequisitionTrip"]);
            var model = bll.db.RequisitionTrips.Where(m => m.PK_RequisitionTrip == PK_RequisitionTrip && m.IsDeleted != true && m.IsForwarded != true && (m.StatusText == InternalTripStatus.Created || (m.StatusText == InternalTripStatus.Cancelled && m.OracleDB_IsPushed != true))).FirstOrDefault();
            if (model != null)
            {
                model.IsForwarded = true;
                model.FK_AppUser_ForwardedBy = CurrentUser.PK_User;
                model.ForwardedAt = DateTime.Now;
                model.PRG_Type_ForwaredTo = form["PRG_Type_ForwaredTo"];
                model.FK_Location_ForwardedTo = Guid.Parse(form["FK_Location_ForwardedTo"]);
                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "Requisition successfully frowared.");
                var url = Session[SessionClass.URLToRedirect].ToString();
                return Redirect(url);
            }
            else
            {
                CreateAlertMessage(AlertMessageType.Danger, "Danger", "Requisition is not be forward. Current status: " + model.StatusText);
                var url = Session[SessionClass.URLToRedirect].ToString();
                return Redirect(url);
            }
        }

        public ActionResult AssignTrip_Vehicle(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var requisitionTrip = bll.db.RequisitionTrips.Where(m => m.PK_RequisitionTrip == id && m.IsDeleted != true && (m.StatusText == InternalTripStatus.Created || (m.StatusText == InternalTripStatus.Cancelled && m.OracleDB_IsPushed != true))).FirstOrDefault();

            var now = DateTime.Now;

            ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", now.Date);
            ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", now.AddDays(1).Date);

            var starting_limit = DateTime.Now.Date.AddDays(-7);
            var cliesntsQuery = bll.db.Requisitions.Where(m => m.IsDeleted != true && m.CreatedAt > starting_limit);
            if (CurrentUser.PRG_Type != "ALL")
            {
                cliesntsQuery = cliesntsQuery.Where(m => m.PRG_Type == CurrentUser.PRG_Type);
            }
            var _cliesnts = cliesntsQuery.Select(m => new { m.AppUser.PK_User, Text = m.AppUser.FullName + ":" + m.AppUser.Depo.Name }).Distinct().OrderBy(m => m.Text);
            ViewBag.Clients = new SelectList(_cliesnts, "PK_User", "Text");

            ViewBag.FromLocations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name");
            ViewBag.ToLocations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name");

            ViewBag.TransportAgency = new SelectList(bll.db.TransportAgencies.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_TransportAgency", "Name");
            return View(requisitionTrip);
        }
        [HttpPost]
        public ActionResult AssignTrip_Vehicle(FormCollection form)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var _PK_RequisitionTrip_0 = Convert.ToInt64(form["PK_RequisitionTrip_0"]);
            RequisitionTrip requisitionTrip = bll.db.RequisitionTrips.Where(m => m.IsDeleted != true && m.Requisition.IsDeleted != true && m.PK_RequisitionTrip == _PK_RequisitionTrip_0 && (m.StatusText == InternalTripStatus.Created || (m.StatusText == InternalTripStatus.Cancelled && m.OracleDB_IsPushed != true))).FirstOrDefault();
            if (requisitionTrip == null)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", "Requisition could not be re-assigned. Current Status: " + requisitionTrip.StatusText);
                return RedirectToAction("RequisitionTripIndex_Assigner");
            }
            try
            {
                var _pk_vheicle = Guid.Parse(form["FK_Vehicle"]);
                var _vehicle = bll.db.Vehicles.Where(m => m.PK_Vehicle == _pk_vheicle).Where(m => (m.OWN_MHT_DHT == "OWN" || m.OWN_MHT_DHT == "MHT" || m.OWN_MHT_DHT == "DHT") /*&& accessibleDepoes.Contains(m.FK_Depo)*/
                ).FirstOrDefault();
                if (true/*_vehicle.FK_RequisitionTrip_CurrentAssigner == null || _vehicle.FK_RequisitionTrip_CurrentAssigner == CurrentUser.PK_User*/)
                {
                    requisitionTrip.FK_Vehicle = _vehicle.PK_Vehicle;
                    requisitionTrip.OWN_MHT_DHT = _vehicle.OWN_MHT_DHT;
                    requisitionTrip.PRG_Type = CurrentUser.PRG_Type;
                    requisitionTrip.FinalWantedAtDateTime = DateTime.Parse(form["FinalWantedAtDateTime"]);
                    var conflictingTrip = bll.db.RequisitionTrips.Where(m => m.FK_Vehicle == requisitionTrip.FK_Vehicle && m.FinalWantedAtDateTime == requisitionTrip.FinalWantedAtDateTime).FirstOrDefault();
                    if (conflictingTrip != null)
                    {
                        CreateAlertMessage(AlertMessageType.Warning, "Warning", _vehicle.RegistrationNumber + " Can not be assigned at " + requisitionTrip.FinalWantedAtDateTime + ". Already assigned for " + conflictingTrip.TrackingID + " at the same time.");
                        return RedirectToAction("AssignTrip_Vehicle", new { id = _PK_RequisitionTrip_0 });
                    }
                    requisitionTrip.Driver_Staff_ID = form["Driver_Staff_ID"];
                    requisitionTrip.Driver_Name = form["Driver_Name"];
                    requisitionTrip.Driver_ContactNumber = form["Driver_ContactNumber"];
                    if (!string.IsNullOrEmpty(form["TotalAmount"]))
                    {
                        requisitionTrip.TotalAmount = long.Parse(form["TotalAmount"]);
                    }
                    if (!string.IsNullOrEmpty(form["CommissionAmount"]))
                    {
                        requisitionTrip.CommissionAmount = long.Parse(form["CommissionAmount"]);
                    }
                    if (!string.IsNullOrEmpty(form["AdvanceAmount"]))
                    {
                        requisitionTrip.AdvanceAmount = long.Parse(form["AdvanceAmount"]);
                    }
                    requisitionTrip.PendingAmount = requisitionTrip.TotalAmount - (requisitionTrip.CommissionAmount ?? 0);
                    if (!string.IsNullOrEmpty(form["FK_TransportAgency"]))
                    {
                        requisitionTrip.FK_TransportAgency = Int64.Parse(form["FK_TransportAgency"]);
                    }
                    requisitionTrip.AssigningNote = form["AssigningNote"];

                    requisitionTrip.FK_AppUser_Assigner = CurrentUser.PK_User;
                    requisitionTrip.StatusText = InternalTripStatus.Assigned;
                    requisitionTrip.AssingedAt = DateTime.Now;
                    bll.db.SaveChanges();
                    _vehicle.FK_RequisitionTrip_CurrentAssigner = CurrentUser.PK_User;
                    if (NonVehicleTypes.Contains(_vehicle.VehicleType))
                    {
                        _vehicle.FK_RequisitionTrip_CurrentAssigner = null;
                    }
                    bll.db.SaveChanges();

                    if (_vehicle.OWN_MHT_DHT == "DHT")
                    {
                        var _DHT_RequisitionTrip = new DHT_RequisitionTrip();
                        _DHT_RequisitionTrip.FK_Vehicle = _vehicle.PK_Vehicle;
                        _DHT_RequisitionTrip.RegistrationNumber = _vehicle.RegistrationNumber;
                        _DHT_RequisitionTrip.FK_RequisitionTrip = requisitionTrip.PK_RequisitionTrip;
                        _DHT_RequisitionTrip.FK_Location_From = requisitionTrip.Requisition.FK_Location_From;
                        _DHT_RequisitionTrip.FK_Location_To = requisitionTrip.Requisition.FK_Location_To;
                        _DHT_RequisitionTrip.FinalWantedAtDateTime = requisitionTrip.FinalWantedAtDateTime;
                        _DHT_RequisitionTrip.CreatedAt = DateTime.Now;
                        bll.db.DHT_RequisitionTrip.Add(_DHT_RequisitionTrip);
                        bll.db.SaveChanges();
                    }

                    //# Notify Raiser Mail
                    if (!string.IsNullOrEmpty(requisitionTrip.Requisition.AppUser.Email))
                    {
                        try
                        {
                            var Mail_Subject = "Requisition " + requisitionTrip.StatusText + " " + requisitionTrip.TrackingID;
                            var Mail_Body = "Dear Concern, The below requisition is " + requisitionTrip.StatusText + ".<br>";
                            Mail_Body = Mail_Body + "<b>Requisition Detail</b>" + "<br>" +
                                "Demand: " + requisitionTrip.TrackingID + "<br>" +
                                "Requistion: " + requisitionTrip.TrackingID + "<br>" +
                                "Vehicle: " + requisitionTrip.Vehicle.RegistrationNumber + " " +
                                requisitionTrip.Vehicle.OWN_MHT_DHT + " " +
                                requisitionTrip.Vehicle.VehicleType + "<br>" +

                                "Driver: " + (string.IsNullOrEmpty(requisitionTrip.Driver_Staff_ID) ? "" : requisitionTrip.Driver_Staff_ID) + " " +
                                (string.IsNullOrEmpty(requisitionTrip.Driver_Name) ? "" : requisitionTrip.Driver_Name) + " " +
                                (string.IsNullOrEmpty(requisitionTrip.Driver_ContactNumber) ? "" : requisitionTrip.Driver_ContactNumber) + "<br>" +

                                "Total Amount: " + (requisitionTrip.TotalAmount != null ? "" : requisitionTrip.TotalAmount + " TK") + " " + "<br>" +
                                "From: " + requisitionTrip.Requisition.Location.Name + " " + requisitionTrip.Requisition.StartingLocation + "<br>" +
                                "To: " + requisitionTrip.Requisition.Location1.Name + " " + requisitionTrip.Requisition.FinishingLocation + "<br>" +
                                "Available At: " + requisitionTrip.FinalWantedAtDateTime + "<br>" +
                                "Assigning Note: " + requisitionTrip.AssigningNote + "<br>";
                            SendMail_Single(requisitionTrip.Requisition.AppUser.Email, Mail_Subject, Mail_Body);
                        }
                        catch (Exception e)
                        {
                        }
                    }

                    //# Child Trips
                    var PK_RequisitionTrip_SL = Convert.ToInt32(form["PK_RequisitionTrip_SL"]);
                    for (int index = 1; index <= PK_RequisitionTrip_SL; index++)
                    {
                        //var _TrackingID = form["Pk_RequisitionTrip" + index];
                        var _PK_RequisitionTrip = form["PK_RequisitionTrip_" + index];
                        if (string.IsNullOrEmpty(_PK_RequisitionTrip))
                        {
                            continue;
                        }
                        var __PK_RequisitionTrip = Convert.ToInt64(_PK_RequisitionTrip);
                        RequisitionTrip _requisitionTrip = bll.db.RequisitionTrips.Where(m => m.IsDeleted != true && m.Requisition.IsDeleted != true && m.PK_RequisitionTrip == __PK_RequisitionTrip && (m.StatusText == InternalTripStatus.Created || (m.StatusText == InternalTripStatus.Cancelled && m.OracleDB_IsPushed != true))).FirstOrDefault();
                        if (_requisitionTrip != null)
                        {
                            //var _pk_vheicle = Guid.Parse(form["FK_Vehicle"]);
                            //var _vehicle = bll.db.Vehicles.Where(m => m.PK_Vehicle == _pk_vheicle).Where(m => (m.OWN_MHT_DHT == "OWN" || m.OWN_MHT_DHT == "MHT" || m.OWN_MHT_DHT == "DHT") /*&& accessibleDepoes.Contains(m.FK_Depo)*/
                            //).FirstOrDefault();
                            _requisitionTrip.FK_Vehicle = _vehicle.PK_Vehicle;
                            _requisitionTrip.OWN_MHT_DHT = _vehicle.OWN_MHT_DHT;
                            _requisitionTrip.PRG_Type = CurrentUser.PRG_Type;
                            _requisitionTrip.Driver_Staff_ID = form["Driver_Staff_ID"];
                            _requisitionTrip.Driver_Name = form["Driver_Name"];
                            _requisitionTrip.Driver_ContactNumber = form["Driver_ContactNumber"];
                            //if (!string.IsNullOrEmpty(form["TotalAmount_" + index]))
                            //{
                            //    _requisitionTrip.TotalAmount = long.Parse(form["TotalAmount_" + index]);
                            //}
                            _requisitionTrip.AssigningNote = form["AssigningNote"];

                            _requisitionTrip.FK_AppUser_Assigner = CurrentUser.PK_User;
                            _requisitionTrip.StatusText = InternalTripStatus.Assigned;
                            _requisitionTrip.AssingedAt = DateTime.Now;

                            requisitionTrip.IsParent = true;
                            _requisitionTrip.FK_RequisitionTrip_Parent = requisitionTrip.PK_RequisitionTrip;
                            bll.db.SaveChanges();

                            //# Notify Raiser Mail
                            if (!string.IsNullOrEmpty(_requisitionTrip.Requisition.AppUser.Email))
                            {
                                try
                                {
                                    var Mail_Subject = "Requisition " + _requisitionTrip.StatusText + " " + _requisitionTrip.TrackingID;
                                    var Mail_Body = "Dear Concern, The below requisition is " + _requisitionTrip.StatusText + ".<br>";
                                    Mail_Body = Mail_Body + "<b>Requisition Detail</b>" + "<br>" +
                                        "Demand: " + _requisitionTrip.TrackingID + "<br>" +
                                        "Requistion: " + _requisitionTrip.TrackingID + "<br>" +
                                        "Vehicle: " + _requisitionTrip.Vehicle.RegistrationNumber + " " +
                                        _requisitionTrip.Vehicle.OWN_MHT_DHT + " " +
                                        _requisitionTrip.Vehicle.VehicleType + "<br>" +

                                        "Driver: " + (string.IsNullOrEmpty(_requisitionTrip.Driver_Staff_ID) ? "" : _requisitionTrip.Driver_Staff_ID) + " " +
                                        (string.IsNullOrEmpty(_requisitionTrip.Driver_Name) ? "" : _requisitionTrip.Driver_Name) + " " +
                                        (string.IsNullOrEmpty(_requisitionTrip.Driver_ContactNumber) ? "" : _requisitionTrip.Driver_ContactNumber) + "<br>" +

                                        "Total Amount: " + (_requisitionTrip.TotalAmount != null ? "" : _requisitionTrip.TotalAmount + " TK") + " " + "<br>" +
                                        "From: " + _requisitionTrip.Requisition.Location.Name + " " + _requisitionTrip.Requisition.StartingLocation + "<br>" +
                                        "To: " + _requisitionTrip.Requisition.Location1.Name + " " + _requisitionTrip.Requisition.FinishingLocation + "<br>" +
                                        "Available At: " + _requisitionTrip.FinalWantedAtDateTime + "<br>" +
                                        "Assigning Note: " + _requisitionTrip.AssigningNote + "<br>";
                                    SendMail_Single(_requisitionTrip.Requisition.AppUser.Email, Mail_Subject, Mail_Body);
                                }
                                catch (Exception e)
                                {
                                }
                            }
                        }
                    }

                    //# Oracle PRAN Insert
                    if (requisitionTrip.Requisition.PRG_Type == "PRAN" && requisitionTrip.Requisition.OrganizationCode != null && requisitionTrip.Requisition.OrganizationName != null)
                    {
                        try
                        {
                            var now = DateTime.Now;
                            var sqlCommandList = new List<string>();
                            var ERP_Id = requisitionTrip.Vehicle.ERP_Id != null && requisitionTrip.Requisition.PRG_Type == requisitionTrip.Vehicle.Depo.PRG_Type ? requisitionTrip.Vehicle.ERP_Id : 55555;
                            var sqlCommand = "insert into T_REQE_PRAN (REQE_ID,REQE_TRID,REQE_DAT,VEH_NUM,VEH_ID,IDATE,ORG_CODE,ORG_NAME) values (" +
                                "'" + requisitionTrip.PK_RequisitionTrip + "'," +
                                "'" + requisitionTrip.TrackingID + "'," +
                                "to_date('" + ((DateTime)requisitionTrip.FinalWantedAtDateTime).ToString("yyyy-MM-dd HH:mm:ss") + "', 'yyyy-mm-dd hh24:mi:ss')," +
                                "'" + requisitionTrip.Vehicle.RegistrationNumber + "'," +
                                "'" + ERP_Id + "'," +
                                "to_date('" + now.ToString("yyyy-MM-dd HH:mm:ss") + "', 'yyyy-mm-dd hh24:mi:ss')," +
                                "'" + requisitionTrip.Requisition.OrganizationCode + "', " +
                                "'" + requisitionTrip.Requisition.OrganizationName + "'" +
                                ")";
                            sqlCommandList.Add(sqlCommand);
                            var res = OracleRequisitionTrip_PRAN_DBHelper.DbSaveChanges(sqlCommandList);
                            if (res == true)
                            {
                                requisitionTrip.OracleDB_IsPushed = true;
                                requisitionTrip.OracleDB_PushedAt = now;
                                bll.db.SaveChanges();
                            }
                        }
                        catch (Exception e)
                        {
                            bll.db.AppErrorLogs.Add(
                                  new AppErrorLog()
                                  {
                                      ErrorMessage = e.Message,
                                      ErrorTime = DateTime.Now,
                                      UserDefinedMessage = "Requisition/AssignTrip_Vehicle-Push-PRAN"
                                  }
                                );
                            bll.db.SaveChanges();
                        }
                    }

                    //# Oracle RFL Insert
                    if (requisitionTrip.Requisition.PRG_Type == "RFL" && requisitionTrip.Requisition.OrganizationCode != null && requisitionTrip.Requisition.OrganizationName != null)
                    {
                        try
                        {
                            var now = DateTime.Now;
                            var sqlCommandList = new List<string>();
                            var ERP_Id = requisitionTrip.Vehicle.ERP_Id != null && requisitionTrip.Requisition.PRG_Type == requisitionTrip.Vehicle.Depo.PRG_Type ? requisitionTrip.Vehicle.ERP_Id : 55555;
                            var sqlCommand = "insert into T_REQE_RFL (REQE_ID,REQE_TRID,REQE_DAT,VEH_NUM,VEH_ID,IDATE,ORG_CODE,ORG_NAME) values (" +
                                            "'" + requisitionTrip.PK_RequisitionTrip + "'," +
                                            "'" + requisitionTrip.TrackingID + "'," +
                                            "to_date('" + ((DateTime)requisitionTrip.FinalWantedAtDateTime).ToString("yyyy-MM-dd HH:mm:ss") + "', 'yyyy-mm-dd hh24:mi:ss')," +
                                            "'" + requisitionTrip.Vehicle.RegistrationNumber + "'," +
                                            "'" + ERP_Id + "'," +
                                            "to_date('" + now.ToString("yyyy-MM-dd HH:mm:ss") + "', 'yyyy-mm-dd hh24:mi:ss')," +
                                            "'" + requisitionTrip.Requisition.OrganizationCode + "', " +
                                            "'" + requisitionTrip.Requisition.OrganizationName + "'" +
                                            ")";
                            sqlCommandList.Add(sqlCommand);
                            var res = OracleRequisitionTrip_RFL_DBHelper.DbSaveChanges(sqlCommandList);
                            if (res == true)
                            {
                                requisitionTrip.OracleDB_IsPushed = true;
                                requisitionTrip.OracleDB_PushedAt = now;
                                bll.db.SaveChanges();
                            }
                        }
                        catch (Exception e)
                        {
                            bll.db.AppErrorLogs.Add(
                                  new AppErrorLog()
                                  {
                                      ErrorMessage = e.Message,
                                      ErrorTime = DateTime.Now,
                                      UserDefinedMessage = "Requisition/AssignTrip_Vehicle-Push-RFL"
                                  }
                                );
                            bll.db.SaveChanges();
                        }
                    }

                    CreateAlertMessage(AlertMessageType.Success, "Success", "Requisition is successfully assigned to a vehicle.");
                    return RedirectToAction("RequisitionTripIndex_Assigner");
                }
                else
                {
                    var lastAssigner = bll.db.AppUsers.Where(m => m.PK_User == _vehicle.FK_RequisitionTrip_CurrentAssigner).FirstOrDefault();
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", _vehicle.RegistrationNumber + " Can not be assigned. Already assigned by " + lastAssigner.FullName + " " + lastAssigner.Depo.Name);
                    return RedirectToAction("RequisitionTripIndex_Assigner");
                }
            }
            catch (Exception e)
            {
                CreateAlertMessage(AlertMessageType.Danger, "Warning", "Internal Error Ocuured.");
                return RedirectToAction("RequisitionTripIndex_Assigner");
            }
        }

        public ActionResult ReassignTrip_Vehicle(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var requisitionTrip = bll.db.RequisitionTrips.Where(m => m.PK_RequisitionTrip == id && m.IsDeleted != true && m.StatusText == InternalTripStatus.Assigned).FirstOrDefault();
            var childRequisitionTrip = requisitionTrip.IsParent == true ? bll.db.RequisitionTrips.Where(m => m.FK_RequisitionTrip_Parent == id).ToList() : new List<RequisitionTrip>();
            if (requisitionTrip == null)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", "Requisition could not be re-assigned. Current Status: " + requisitionTrip.StatusText);
                return RedirectToAction("RequisitionTripIndex_Assigner");
            }
            else if (requisitionTrip.OracleDB_GPNumber != null)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", "Requisition could not be re-assigned. Already GP created. GP no: " + requisitionTrip.OracleDB_GPNumber);
                return RedirectToAction("RequisitionTripIndex_Assigner");
            }
            else
            {
                //go forward
            }
            ViewBag.TransportAgency = new SelectList(bll.db.TransportAgencies.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_TransportAgency", "Name", requisitionTrip.FK_TransportAgency);
            return View(new Tuple<RequisitionTrip, List<RequisitionTrip>>(requisitionTrip, childRequisitionTrip));
        }
        [HttpPost]
        public ActionResult ReassignTrip_Vehicle(FormCollection form)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var _PK_RequisitionTrip_0 = Convert.ToInt64(form["PK_RequisitionTrip_0"]);
            var requisitionTrip = bll.db.RequisitionTrips.Where(m => m.PK_RequisitionTrip == _PK_RequisitionTrip_0 && m.IsDeleted != true && m.StatusText == InternalTripStatus.Assigned).FirstOrDefault();
            if (requisitionTrip == null)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", "Requisition could not be re-assigned. Current Status: " + requisitionTrip.StatusText);
                return RedirectToAction("RequisitionTripIndex_Assigner");
            }
            else if (requisitionTrip.OracleDB_GPNumber != null)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", "Requisition could not be re-assigned. Already GP created. GP no: " + requisitionTrip.OracleDB_GPNumber);
                return RedirectToAction("RequisitionTripIndex_Assigner");
            }
            else if (requisitionTrip.Requisition.PRG_Type == "PRAN" && requisitionTrip.Requisition.OrganizationCode != null && requisitionTrip.Requisition.OrganizationName != null)
            {
                try
                {
                    var query = "SELECT * FROM T_REQE_PRAN where REQE_ID = '" + requisitionTrip.PK_RequisitionTrip + "'";
                    DataTable res = OracleRequisitionTrip_PRAN_DBHelper.ExecuteSelectCommand(query);
                    var REQE_GPNO = res.Rows[0]["REQE_GPNO"].ToString();
                    if (!string.IsNullOrEmpty(REQE_GPNO))
                    {
                        CreateAlertMessage(AlertMessageType.Warning, "Warning", "Requisition could not be re-assigned. Already GP created. GP no: " + REQE_GPNO);
                        return RedirectToAction("RequisitionTripIndex_Assigner");
                    }
                }
                catch (Exception e)
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", "Please try later. GP server not connected.");
                    return RedirectToAction("RequisitionTripIndex_Assigner");
                }
            }
            else if (requisitionTrip.Requisition.PRG_Type == "RFL" && requisitionTrip.Requisition.OrganizationCode != null && requisitionTrip.Requisition.OrganizationName != null)
            {
                try
                {
                    var query = "SELECT * FROM T_REQE_RFL where REQE_ID = '" + requisitionTrip.PK_RequisitionTrip + "'";
                    DataTable res = OracleRequisitionTrip_RFL_DBHelper.ExecuteSelectCommand(query);
                    var REQE_GPNO = res.Rows[0]["REQE_GPNO"].ToString();
                    if (!string.IsNullOrEmpty(REQE_GPNO))
                    {
                        CreateAlertMessage(AlertMessageType.Warning, "Warning", "Requisition could not be re-assigned. Already GP created. GP no: " + REQE_GPNO);
                        return RedirectToAction("RequisitionTripIndex_Assigner");
                    }
                }
                catch (Exception e)
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", "Please try later. GP server not connected.");
                    return RedirectToAction("RequisitionTripIndex_Assigner");
                }
            }
            else
            {
                //go forward
            }

            try
            {
                var _pk_vheicle = Guid.Parse(form["FK_Vehicle"]);
                var _vehicle = bll.db.Vehicles.Where(m => m.PK_Vehicle == _pk_vheicle).Where(m => (m.OWN_MHT_DHT == "OWN" || m.OWN_MHT_DHT == "MHT" || m.OWN_MHT_DHT == "DHT") /*&& accessibleDepoes.Contains(m.FK_Depo)*/
                ).FirstOrDefault();
                if (true/*_vehicle.FK_RequisitionTrip_CurrentAssigner == null || _vehicle.FK_RequisitionTrip_CurrentAssigner == CurrentUser.PK_User*/)
                {
                    requisitionTrip.FK_Vehicle = _vehicle.PK_Vehicle;
                    requisitionTrip.OWN_MHT_DHT = _vehicle.OWN_MHT_DHT;
                    requisitionTrip.PRG_Type = CurrentUser.PRG_Type;
                    requisitionTrip.FinalWantedAtDateTime = DateTime.Parse(form["FinalWantedAtDateTime"]);
                    var conflictingTrip = bll.db.RequisitionTrips.Where(m => m.FK_Vehicle == requisitionTrip.FK_Vehicle && m.FinalWantedAtDateTime == requisitionTrip.FinalWantedAtDateTime).FirstOrDefault();
                    if (conflictingTrip != null)
                    {
                        CreateAlertMessage(AlertMessageType.Warning, "Warning", _vehicle.RegistrationNumber + " Can not be assigned at " + requisitionTrip.FinalWantedAtDateTime + ". Already assigned for " + conflictingTrip.TrackingID + " at the same time.");
                        return RedirectToAction("AssignVehicle_Trip", new { PK_Vehicle = requisitionTrip.FK_Vehicle });
                    }
                    requisitionTrip.Driver_Staff_ID = form["Driver_Staff_ID"];
                    requisitionTrip.Driver_Name = form["Driver_Name"];
                    requisitionTrip.Driver_ContactNumber = form["Driver_ContactNumber"];
                    if (!string.IsNullOrEmpty(form["TotalAmount"]))
                    {
                        requisitionTrip.TotalAmount = long.Parse(form["TotalAmount"]);
                    }
                    else
                    {
                        requisitionTrip.TotalAmount = null;
                    }
                    if (!string.IsNullOrEmpty(form["CommissionAmount"]))
                    {
                        requisitionTrip.CommissionAmount = long.Parse(form["CommissionAmount"]);
                    }
                    else
                    {
                        requisitionTrip.CommissionAmount = null;
                    }
                    if (!string.IsNullOrEmpty(form["AdvanceAmount"]))
                    {
                        requisitionTrip.AdvanceAmount = long.Parse(form["AdvanceAmount"]);
                    }
                    else
                    {
                        requisitionTrip.AdvanceAmount = null;
                    }
                    requisitionTrip.PendingAmount = requisitionTrip.TotalAmount - (requisitionTrip.CommissionAmount ?? 0);
                    if (!string.IsNullOrEmpty(form["FK_TransportAgency"]))
                    {
                        requisitionTrip.FK_TransportAgency = Int64.Parse(form["FK_TransportAgency"]);
                    }
                    else
                    {
                        requisitionTrip.FK_TransportAgency = null;
                    }
                    requisitionTrip.AssigningNote = form["AssigningNote"];

                    requisitionTrip.FK_AppUser_Assigner = CurrentUser.PK_User;
                    requisitionTrip.StatusText = InternalTripStatus.Assigned;
                    requisitionTrip.AssingedAt = DateTime.Now;
                    bll.db.SaveChanges();
                    _vehicle.FK_RequisitionTrip_CurrentAssigner = CurrentUser.PK_User;
                    if (NonVehicleTypes.Contains(_vehicle.VehicleType))
                    {
                        _vehicle.FK_RequisitionTrip_CurrentAssigner = null;
                    }
                    bll.db.SaveChanges();

                    if (_vehicle.OWN_MHT_DHT == "DHT")
                    {
                        var _DHT_RequisitionTrip = new DHT_RequisitionTrip();
                        _DHT_RequisitionTrip.FK_Vehicle = _vehicle.PK_Vehicle;
                        _DHT_RequisitionTrip.RegistrationNumber = _vehicle.RegistrationNumber;
                        _DHT_RequisitionTrip.FK_RequisitionTrip = requisitionTrip.PK_RequisitionTrip;
                        _DHT_RequisitionTrip.FK_Location_From = requisitionTrip.Requisition.FK_Location_From;
                        _DHT_RequisitionTrip.FK_Location_To = requisitionTrip.Requisition.FK_Location_To;
                        _DHT_RequisitionTrip.FinalWantedAtDateTime = requisitionTrip.FinalWantedAtDateTime;
                        _DHT_RequisitionTrip.CreatedAt = DateTime.Now;
                        bll.db.DHT_RequisitionTrip.Add(_DHT_RequisitionTrip);
                        bll.db.SaveChanges();
                    }

                    //# Notify Raiser Mail
                    if (!string.IsNullOrEmpty(requisitionTrip.Requisition.AppUser.Email))
                    {
                        try
                        {
                            var Mail_Subject = "Re:Requisition " + requisitionTrip.StatusText + " " + requisitionTrip.TrackingID;
                            var Mail_Body = "Dear Concern, The below requisition is " + requisitionTrip.StatusText + ".<br>";
                            Mail_Body = Mail_Body + "<b>Requisition Detail</b>" + "<br>" +
                                "Demand: " + requisitionTrip.TrackingID + "<br>" +
                                "Requistion: " + requisitionTrip.TrackingID + "<br>" +
                                "Vehicle: " + requisitionTrip.Vehicle.RegistrationNumber + " " +
                                requisitionTrip.Vehicle.OWN_MHT_DHT + " " +
                                requisitionTrip.Vehicle.VehicleType + "<br>" +

                                "Driver: " + (string.IsNullOrEmpty(requisitionTrip.Driver_Staff_ID) ? "" : requisitionTrip.Driver_Staff_ID) + " " +
                                (string.IsNullOrEmpty(requisitionTrip.Driver_Name) ? "" : requisitionTrip.Driver_Name) + " " +
                                (string.IsNullOrEmpty(requisitionTrip.Driver_ContactNumber) ? "" : requisitionTrip.Driver_ContactNumber) + "<br>" +

                                "Total Amount: " + (requisitionTrip.TotalAmount != null ? "" : requisitionTrip.TotalAmount + " TK") + " " + "<br>" +
                                "From: " + requisitionTrip.Requisition.Location.Name + " " + requisitionTrip.Requisition.StartingLocation + "<br>" +
                                "To: " + requisitionTrip.Requisition.Location1.Name + " " + requisitionTrip.Requisition.FinishingLocation + "<br>" +
                                "Available At: " + requisitionTrip.FinalWantedAtDateTime + "<br>" +
                                "Assigning Note: " + requisitionTrip.AssigningNote + "<br>";
                            SendMail_Single(requisitionTrip.Requisition.AppUser.Email, Mail_Subject, Mail_Body);
                        }
                        catch (Exception e)
                        {
                        }
                    }

                    //# Child Trips
                    if (requisitionTrip.IsParent == true)
                    {
                        var pendingChildTripList = bll.db.RequisitionTrips.Where(m => m.FK_RequisitionTrip_Parent == requisitionTrip.PK_RequisitionTrip).ToList();
                        foreach (var _requisitionTrip in pendingChildTripList)
                        {
                            _requisitionTrip.FK_Vehicle = _vehicle.PK_Vehicle;
                            _requisitionTrip.OWN_MHT_DHT = _vehicle.OWN_MHT_DHT;
                            _requisitionTrip.PRG_Type = CurrentUser.PRG_Type;
                            _requisitionTrip.Driver_Staff_ID = form["Driver_Staff_ID"];
                            _requisitionTrip.Driver_Name = form["Driver_Name"];
                            _requisitionTrip.Driver_ContactNumber = form["Driver_ContactNumber"];
                            //if (!string.IsNullOrEmpty(form["TotalAmount_" + index]))
                            //{
                            //    _requisitionTrip.TotalAmount = long.Parse(form["TotalAmount_" + index]);
                            //}
                            _requisitionTrip.AssigningNote = form["AssigningNote"];

                            _requisitionTrip.FK_AppUser_Assigner = CurrentUser.PK_User;
                            _requisitionTrip.StatusText = InternalTripStatus.Assigned;
                            _requisitionTrip.AssingedAt = DateTime.Now;

                            requisitionTrip.IsParent = true;
                            _requisitionTrip.FK_RequisitionTrip_Parent = requisitionTrip.PK_RequisitionTrip;
                            bll.db.SaveChanges();

                            //# Notify Raiser Mail
                            if (!string.IsNullOrEmpty(_requisitionTrip.Requisition.AppUser.Email))
                            {
                                try
                                {
                                    var Mail_Subject = "Re:Requisition " + _requisitionTrip.StatusText + " " + _requisitionTrip.TrackingID;
                                    var Mail_Body = "Dear Concern, The below requisition is " + _requisitionTrip.StatusText + ".<br>";
                                    Mail_Body = Mail_Body + "<b>Requisition Detail</b>" + "<br>" +
                                        "Demand: " + _requisitionTrip.TrackingID + "<br>" +
                                        "Requistion: " + _requisitionTrip.TrackingID + "<br>" +
                                        "Vehicle: " + _requisitionTrip.Vehicle.RegistrationNumber + " " +
                                        _requisitionTrip.Vehicle.OWN_MHT_DHT + " " +
                                        _requisitionTrip.Vehicle.VehicleType + "<br>" +

                                        "Driver: " + (string.IsNullOrEmpty(_requisitionTrip.Driver_Staff_ID) ? "" : _requisitionTrip.Driver_Staff_ID) + " " +
                                        (string.IsNullOrEmpty(_requisitionTrip.Driver_Name) ? "" : _requisitionTrip.Driver_Name) + " " +
                                        (string.IsNullOrEmpty(_requisitionTrip.Driver_ContactNumber) ? "" : _requisitionTrip.Driver_ContactNumber) + "<br>" +

                                        "Total Amount: " + (_requisitionTrip.TotalAmount != null ? "" : _requisitionTrip.TotalAmount + " TK") + " " + "<br>" +
                                        "From: " + _requisitionTrip.Requisition.Location.Name + " " + requisitionTrip.Requisition.StartingLocation + "<br>" +
                                        "To: " + _requisitionTrip.Requisition.Location1.Name + " " + _requisitionTrip.Requisition.FinishingLocation + "<br>" +
                                        "Available At: " + _requisitionTrip.FinalWantedAtDateTime + "<br>" +
                                        "Assigning Note: " + _requisitionTrip.AssigningNote + "<br>";
                                    SendMail_Single(_requisitionTrip.Requisition.AppUser.Email, Mail_Subject, Mail_Body);
                                }
                                catch (Exception e)
                                {
                                }
                            }
                        }
                    }

                    //# Oracle PRAN Update
                    if (requisitionTrip.Requisition.PRG_Type == "PRAN" && requisitionTrip.Requisition.OrganizationCode != null && requisitionTrip.Requisition.OrganizationName != null)
                    {
                        try
                        {
                            var now = DateTime.Now;
                            var sqlCommandList = new List<string>();
                            var ERP_Id = requisitionTrip.Vehicle.ERP_Id != null && requisitionTrip.Requisition.PRG_Type == requisitionTrip.Vehicle.Depo.PRG_Type ? requisitionTrip.Vehicle.ERP_Id : 55555;
                            var sqlCommand = "update T_REQE_PRAN set " +
                                "REQE_DAT = to_date('" + ((DateTime)requisitionTrip.FinalWantedAtDateTime).ToString("yyyy-MM-dd HH:mm:ss") + "', 'yyyy-mm-dd hh24:mi:ss')," +
                                "VEH_NUM = '" + requisitionTrip.Vehicle.RegistrationNumber + "'," +
                                "VEH_ID = '" + ERP_Id + "'," +
                                "EDATE = to_date('" + now.ToString("yyyy-MM-dd HH:mm:ss") + "', 'yyyy-mm-dd hh24:mi:ss')" +
                                " Where REQE_ID = '" + requisitionTrip.PK_RequisitionTrip + "'";
                            sqlCommandList.Add(sqlCommand);
                            var res = OracleRequisitionTrip_PRAN_DBHelper.DbSaveChanges(sqlCommandList);
                            //In case of not inserted yet
                            if (res == false)
                            {
                                sqlCommandList.Clear();
                                sqlCommand = "insert into T_REQE_PRAN (REQE_ID,REQE_TRID,REQE_DAT,VEH_NUM,VEH_ID,IDATE,ORG_CODE,ORG_NAME) values (" +
                                "'" + requisitionTrip.PK_RequisitionTrip + "'," +
                                "'" + requisitionTrip.TrackingID + "'," +
                                "to_date('" + ((DateTime)requisitionTrip.FinalWantedAtDateTime).ToString("yyyy-MM-dd HH:mm:ss") + "', 'yyyy-mm-dd hh24:mi:ss')," +
                                "'" + requisitionTrip.Vehicle.RegistrationNumber + "'," +
                                "'" + ERP_Id + "'," +
                                "to_date('" + now.ToString("yyyy-MM-dd HH:mm:ss") + "', 'yyyy-mm-dd hh24:mi:ss')," +
                                "'" + requisitionTrip.Requisition.OrganizationCode + "', " +
                                "'" + requisitionTrip.Requisition.OrganizationName + "'" +
                                ")";
                                sqlCommandList.Add(sqlCommand);
                                res = OracleRequisitionTrip_PRAN_DBHelper.DbSaveChanges(sqlCommandList);
                            }
                            if (res == true)
                            {
                                requisitionTrip.OracleDB_IsPushed = true;
                                requisitionTrip.OracleDB_PushedAt = now;
                                bll.db.SaveChanges();
                            }
                        }
                        catch (Exception e)
                        {
                            bll.db.AppErrorLogs.Add(
                                  new AppErrorLog()
                                  {
                                      ErrorMessage = e.Message,
                                      ErrorTime = DateTime.Now,
                                      UserDefinedMessage = "Requisition/RessignTrip_Vehicle-Push-PRAN"
                                  }
                                );
                            bll.db.SaveChanges();
                        }
                    }


                    //# RFL Oracle Push
                    if (requisitionTrip.Requisition.PRG_Type == "RFL" && requisitionTrip.Requisition.OrganizationCode != null && requisitionTrip.Requisition.OrganizationName != null)
                    {
                        try
                        {
                            var now = DateTime.Now;
                            var sqlCommandList = new List<string>();
                            var ERP_Id = requisitionTrip.Vehicle.ERP_Id != null && requisitionTrip.Requisition.PRG_Type == requisitionTrip.Vehicle.Depo.PRG_Type ? requisitionTrip.Vehicle.ERP_Id : 55555;
                            var sqlCommand = "update T_REQE_RFL set " +
                                "REQE_DAT = to_date('" + ((DateTime)requisitionTrip.FinalWantedAtDateTime).ToString("yyyy-MM-dd HH:mm:ss") + "', 'yyyy-mm-dd hh24:mi:ss')," +
                                "VEH_NUM = '" + requisitionTrip.Vehicle.RegistrationNumber + "'," +
                                "VEH_ID = '" + ERP_Id + "'," +
                                "EDATE = to_date('" + now.ToString("yyyy-MM-dd HH:mm:ss") + "', 'yyyy-mm-dd hh24:mi:ss')" +
                                " Where REQE_ID = '" + requisitionTrip.PK_RequisitionTrip + "'";
                            sqlCommandList.Add(sqlCommand);
                            var res = OracleRequisitionTrip_RFL_DBHelper.DbSaveChanges(sqlCommandList);
                            //#In case of not inserted yet
                            if (res == false)
                            {
                                sqlCommandList.Clear();
                                sqlCommand = "insert into T_REQE_RFL (REQE_ID,REQE_TRID,REQE_DAT,VEH_NUM,VEH_ID,IDATE,ORG_CODE,ORG_NAME) values (" +
                                             "'" + requisitionTrip.PK_RequisitionTrip + "'," +
                                             "'" + requisitionTrip.TrackingID + "'," +
                                             "to_date('" + ((DateTime)requisitionTrip.FinalWantedAtDateTime).ToString("yyyy-MM-dd HH:mm:ss") + "', 'yyyy-mm-dd hh24:mi:ss')," +
                                             "'" + requisitionTrip.Vehicle.RegistrationNumber + "'," +
                                             "'" + ERP_Id + "'," +
                                             "to_date('" + now.ToString("yyyy-MM-dd HH:mm:ss") + "', 'yyyy-mm-dd hh24:mi:ss')," +
                                             "'" + requisitionTrip.Requisition.OrganizationCode + "', " +
                                             "'" + requisitionTrip.Requisition.OrganizationName + "'" +
                                             ")";
                                sqlCommandList.Add(sqlCommand);
                                res = OracleRequisitionTrip_RFL_DBHelper.DbSaveChanges(sqlCommandList);
                            }
                            if (res == true)
                            {
                                requisitionTrip.OracleDB_IsPushed = true;
                                requisitionTrip.OracleDB_PushedAt = now;
                                bll.db.SaveChanges();
                            }
                        }
                        catch (Exception e)
                        {
                            bll.db.AppErrorLogs.Add(
                                  new AppErrorLog()
                                  {
                                      ErrorMessage = e.Message,
                                      ErrorTime = DateTime.Now,
                                      UserDefinedMessage = "Requisition/RessignTrip_Vehicle-Push-RFL"
                                  }
                                );
                            bll.db.SaveChanges();
                        }
                    }

                    CreateAlertMessage(AlertMessageType.Success, "Success", "Requisition is successfully reassigned to a vehicle.");
                    return RedirectToAction("RequisitionTripIndex_Assigner");
                }
                else
                {
                    var lastAssigner = bll.db.AppUsers.Where(m => m.PK_User == _vehicle.FK_RequisitionTrip_CurrentAssigner).FirstOrDefault();
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", _vehicle.RegistrationNumber + "Can not be assigned. Already assigned by " + lastAssigner.FullName + " " + lastAssigner.Depo.Name);
                    return RedirectToAction("RequisitionTripIndex_Assigner");
                }

            }
            catch (Exception e)
            {
                CreateAlertMessage(AlertMessageType.Danger, "Warning", "Internal Error Ocuured.");
                return RedirectToAction("RequisitionTripIndex_Assigner");
            }
        }

        public ActionResult AssignVehicle_Trip(Guid PK_Vehicle)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var vehicle = bll.db.Vehicles.Where(m => m.IsDeleted != true && m.PK_Vehicle == PK_Vehicle).FirstOrDefault();

            var now = DateTime.Now;
            var query = bll.db.Requisitions.Where(m => m.IsDeleted != true);

            ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", now.Date);
            ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", now.AddDays(1).Date);

            var starting_limit = DateTime.Now.Date.AddDays(-7);
            var cliesntsQuery = bll.db.Requisitions.Where(m => m.IsDeleted != true && m.CreatedAt > starting_limit);
            if (CurrentUser.PRG_Type != "ALL")
            {
                cliesntsQuery = cliesntsQuery.Where(m => m.PRG_Type == CurrentUser.PRG_Type);
            }
            var _cliesnts = cliesntsQuery.Select(m => new { m.AppUser.PK_User, Text = m.AppUser.FullName + ":" + m.AppUser.Depo.Name }).Distinct().OrderBy(m => m.Text);
            ViewBag.Clients = new SelectList(_cliesnts, "PK_User", "Text");

            ViewBag.FromLocations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name");
            ViewBag.ToLocations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name");

            ViewBag.TransportAgency = new SelectList(bll.db.TransportAgencies.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_TransportAgency", "Name");
            return View(vehicle);
        }
        [HttpPost]
        public ActionResult AssignVehicle_Trip(FormCollection form)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var _PK_RequisitionTrip_0 = Convert.ToInt64(form["PK_RequisitionTrip_0"]);
            RequisitionTrip requisitionTrip = bll.db.RequisitionTrips.Where(m => m.IsDeleted != true && m.Requisition.IsDeleted != true && m.PK_RequisitionTrip == _PK_RequisitionTrip_0 && (m.StatusText == InternalTripStatus.Created || (m.StatusText == InternalTripStatus.Cancelled && m.OracleDB_IsPushed != true))).FirstOrDefault();
            try
            {
                if (requisitionTrip != null)
                {
                    var _pk_vheicle = Guid.Parse(form["FK_Vehicle"]);
                    var _vehicle = bll.db.Vehicles.Where(m => m.PK_Vehicle == _pk_vheicle).Where(m => (m.OWN_MHT_DHT == "OWN" || m.OWN_MHT_DHT == "MHT" || m.OWN_MHT_DHT == "DHT") /*&& accessibleDepoes.Contains(m.FK_Depo)*/
                    ).FirstOrDefault();
                    if (true/*_vehicle.FK_RequisitionTrip_CurrentAssigner == null || _vehicle.FK_RequisitionTrip_CurrentAssigner == CurrentUser.PK_User*/)
                    {
                        requisitionTrip.FK_Vehicle = _vehicle.PK_Vehicle;
                        requisitionTrip.OWN_MHT_DHT = _vehicle.OWN_MHT_DHT;
                        requisitionTrip.PRG_Type = CurrentUser.PRG_Type;
                        requisitionTrip.FinalWantedAtDateTime = DateTime.Parse(form["FinalWantedAtDateTime"]);
                        var conflictingTrip = bll.db.RequisitionTrips.Where(m => m.FK_Vehicle == requisitionTrip.FK_Vehicle && m.FinalWantedAtDateTime == requisitionTrip.FinalWantedAtDateTime).FirstOrDefault();
                        if (conflictingTrip != null)
                        {
                            CreateAlertMessage(AlertMessageType.Warning, "Warning", _vehicle.RegistrationNumber + " Can not be assigned at " + requisitionTrip.FinalWantedAtDateTime + ". Already assigned for " + conflictingTrip.TrackingID + " at the same time.");
                            return RedirectToAction("AssignVehicle_Trip", new { PK_Vehicle = requisitionTrip.FK_Vehicle });
                        }
                        requisitionTrip.Driver_Staff_ID = form["Driver_Staff_ID"];
                        requisitionTrip.Driver_Name = form["Driver_Name"];
                        requisitionTrip.Driver_ContactNumber = form["Driver_ContactNumber"];
                        if (!string.IsNullOrEmpty(form["TotalAmount"]))
                        {
                            requisitionTrip.TotalAmount = long.Parse(form["TotalAmount"]);
                        }
                        if (!string.IsNullOrEmpty(form["CommissionAmount"]))
                        {
                            requisitionTrip.CommissionAmount = long.Parse(form["CommissionAmount"]);
                        }
                        if (!string.IsNullOrEmpty(form["AdvanceAmount"]))
                        {
                            requisitionTrip.AdvanceAmount = long.Parse(form["AdvanceAmount"]);
                        }
                        requisitionTrip.PendingAmount = requisitionTrip.TotalAmount - (requisitionTrip.CommissionAmount ?? 0);
                        if (!string.IsNullOrEmpty(form["FK_TransportAgency"]))
                        {
                            requisitionTrip.FK_TransportAgency = Int64.Parse(form["FK_TransportAgency"]);
                        }
                        requisitionTrip.AssigningNote = form["AssigningNote"];

                        requisitionTrip.FK_AppUser_Assigner = CurrentUser.PK_User;
                        requisitionTrip.StatusText = InternalTripStatus.Assigned;
                        requisitionTrip.AssingedAt = DateTime.Now;
                        bll.db.SaveChanges();
                        _vehicle.FK_RequisitionTrip_CurrentAssigner = CurrentUser.PK_User;
                        if (NonVehicleTypes.Contains(_vehicle.VehicleType))
                        {
                            _vehicle.FK_RequisitionTrip_CurrentAssigner = null;
                        }
                        bll.db.SaveChanges();

                        if (_vehicle.OWN_MHT_DHT == "DHT")
                        {
                            var _DHT_RequisitionTrip = new DHT_RequisitionTrip();
                            _DHT_RequisitionTrip.FK_Vehicle = _vehicle.PK_Vehicle;
                            _DHT_RequisitionTrip.RegistrationNumber = _vehicle.RegistrationNumber;
                            _DHT_RequisitionTrip.FK_RequisitionTrip = requisitionTrip.PK_RequisitionTrip;
                            _DHT_RequisitionTrip.FK_Location_From = requisitionTrip.Requisition.FK_Location_From;
                            _DHT_RequisitionTrip.FK_Location_To = requisitionTrip.Requisition.FK_Location_To;
                            _DHT_RequisitionTrip.FinalWantedAtDateTime = requisitionTrip.FinalWantedAtDateTime;
                            _DHT_RequisitionTrip.CreatedAt = DateTime.Now;
                            bll.db.DHT_RequisitionTrip.Add(_DHT_RequisitionTrip);
                            bll.db.SaveChanges();
                        }

                        //# Notify Raiser Mail
                        if (!string.IsNullOrEmpty(requisitionTrip.Requisition.AppUser.Email))
                        {
                            try
                            {
                                var Mail_Subject = "Requisition " + requisitionTrip.StatusText + " " + requisitionTrip.TrackingID;
                                var Mail_Body = "Dear Concern, The below requisition is " + requisitionTrip.StatusText + ".<br>";
                                Mail_Body = Mail_Body + "<b>Requisition Detail</b>" + "<br>" +
                                    "Demand: " + requisitionTrip.TrackingID + "<br>" +
                                    "Requistion: " + requisitionTrip.TrackingID + "<br>" +
                                    "Vehicle: " + requisitionTrip.Vehicle.RegistrationNumber + " " +
                                    requisitionTrip.Vehicle.OWN_MHT_DHT + " " +
                                    requisitionTrip.Vehicle.VehicleType + "<br>" +

                                    "Driver: " + (string.IsNullOrEmpty(requisitionTrip.Driver_Staff_ID) ? "" : requisitionTrip.Driver_Staff_ID) + " " +
                                    (string.IsNullOrEmpty(requisitionTrip.Driver_Name) ? "" : requisitionTrip.Driver_Name) + " " +
                                    (string.IsNullOrEmpty(requisitionTrip.Driver_ContactNumber) ? "" : requisitionTrip.Driver_ContactNumber) + "<br>" +

                                    "Total Amount: " + (requisitionTrip.TotalAmount != null ? "" : requisitionTrip.TotalAmount + " TK") + " " + "<br>" +
                                    "From: " + requisitionTrip.Requisition.Location.Name + " " + requisitionTrip.Requisition.StartingLocation + "<br>" +
                                    "To: " + requisitionTrip.Requisition.Location1.Name + " " + requisitionTrip.Requisition.FinishingLocation + "<br>" +
                                    "Available At: " + requisitionTrip.FinalWantedAtDateTime + "<br>" +
                                    "Assigning Note: " + requisitionTrip.AssigningNote + "<br>";
                                SendMail_Single(requisitionTrip.Requisition.AppUser.Email, Mail_Subject, Mail_Body);
                            }
                            catch (Exception e)
                            {
                            }
                        }

                        //# Child Trips
                        var PK_RequisitionTrip_SL = Convert.ToInt32(form["PK_RequisitionTrip_SL"]);
                        for (int index = 1; index <= PK_RequisitionTrip_SL; index++)
                        {
                            //var _TrackingID = form["Pk_RequisitionTrip" + index];
                            var _PK_RequisitionTrip = form["PK_RequisitionTrip_" + index];
                            if (string.IsNullOrEmpty(_PK_RequisitionTrip))
                            {
                                continue;
                            }
                            var __PK_RequisitionTrip = Convert.ToInt64(_PK_RequisitionTrip);
                            RequisitionTrip _requisitionTrip = bll.db.RequisitionTrips.Where(m => m.IsDeleted != true && m.Requisition.IsDeleted != true && m.PK_RequisitionTrip == __PK_RequisitionTrip && (m.StatusText == InternalTripStatus.Created || (m.StatusText == InternalTripStatus.Cancelled && m.OracleDB_IsPushed != true))).FirstOrDefault();
                            if (_requisitionTrip != null)
                            {
                                //var _pk_vheicle = Guid.Parse(form["FK_Vehicle"]);
                                //var _vehicle = bll.db.Vehicles.Where(m => m.PK_Vehicle == _pk_vheicle).Where(m => (m.OWN_MHT_DHT == "OWN" || m.OWN_MHT_DHT == "MHT" || m.OWN_MHT_DHT == "DHT") /*&& accessibleDepoes.Contains(m.FK_Depo)*/
                                //).FirstOrDefault();
                                _requisitionTrip.FK_Vehicle = _vehicle.PK_Vehicle;
                                _requisitionTrip.OWN_MHT_DHT = _vehicle.OWN_MHT_DHT;
                                _requisitionTrip.PRG_Type = CurrentUser.PRG_Type;
                                _requisitionTrip.Driver_Staff_ID = form["Driver_Staff_ID"];
                                _requisitionTrip.Driver_Name = form["Driver_Name"];
                                _requisitionTrip.Driver_ContactNumber = form["Driver_ContactNumber"];
                                //if (!string.IsNullOrEmpty(form["TotalAmount_" + index]))
                                //{
                                //    _requisitionTrip.TotalAmount = long.Parse(form["TotalAmount_" + index]);
                                //}
                                _requisitionTrip.AssigningNote = form["AssigningNote"];

                                _requisitionTrip.FK_AppUser_Assigner = CurrentUser.PK_User;
                                _requisitionTrip.StatusText = InternalTripStatus.Assigned;
                                _requisitionTrip.AssingedAt = DateTime.Now;

                                requisitionTrip.IsParent = false;
                                _requisitionTrip.FK_RequisitionTrip_Parent = requisitionTrip.PK_RequisitionTrip;
                                bll.db.SaveChanges();

                                //# Notify Raiser Mail
                                if (!string.IsNullOrEmpty(_requisitionTrip.Requisition.AppUser.Email))
                                {
                                    try
                                    {
                                        var Mail_Subject = "Requisition " + _requisitionTrip.StatusText + " " + _requisitionTrip.TrackingID;
                                        var Mail_Body = "Dear Concern, The below requisition is " + _requisitionTrip.StatusText + ".<br>";
                                        Mail_Body = Mail_Body + "<b>Requisition Detail</b>" + "<br>" +
                                            "Demand: " + _requisitionTrip.TrackingID + "<br>" +
                                            "Requistion: " + _requisitionTrip.TrackingID + "<br>" +
                                            "Vehicle: " + _requisitionTrip.Vehicle.RegistrationNumber + " " +
                                            _requisitionTrip.Vehicle.OWN_MHT_DHT + " " +
                                            _requisitionTrip.Vehicle.VehicleType + "<br>" +

                                            "Driver: " + (string.IsNullOrEmpty(_requisitionTrip.Driver_Staff_ID) ? "" : _requisitionTrip.Driver_Staff_ID) + " " +
                                            (string.IsNullOrEmpty(_requisitionTrip.Driver_Name) ? "" : _requisitionTrip.Driver_Name) + " " +
                                            (string.IsNullOrEmpty(_requisitionTrip.Driver_ContactNumber) ? "" : _requisitionTrip.Driver_ContactNumber) + "<br>" +

                                            "Total Amount: " + (_requisitionTrip.TotalAmount != null ? "" : _requisitionTrip.TotalAmount + " TK") + " " + "<br>" +
                                            "From: " + _requisitionTrip.Requisition.Location.Name + " " + requisitionTrip.Requisition.StartingLocation + "<br>" +
                                            "To: " + _requisitionTrip.Requisition.Location1.Name + " " + _requisitionTrip.Requisition.FinishingLocation + "<br>" +
                                            "Available At: " + _requisitionTrip.FinalWantedAtDateTime + "<br>" +
                                            "Assigning Note: " + _requisitionTrip.AssigningNote + "<br>";
                                        SendMail_Single(_requisitionTrip.Requisition.AppUser.Email, Mail_Subject, Mail_Body);
                                    }
                                    catch (Exception e)
                                    {
                                    }
                                }
                            }
                        }

                        //# PRAN Oracle Push
                        if (requisitionTrip.Requisition.PRG_Type == "PRAN" && requisitionTrip.Requisition.OrganizationCode != null && requisitionTrip.Requisition.OrganizationName != null)
                        {
                            try
                            {
                                var now = DateTime.Now;
                                var sqlCommandList = new List<string>();
                                var ERP_Id = requisitionTrip.Vehicle.ERP_Id != null && requisitionTrip.Requisition.PRG_Type == requisitionTrip.Vehicle.Depo.PRG_Type ? requisitionTrip.Vehicle.ERP_Id : 55555;
                                var sqlCommand = "insert into T_REQE_PRAN (REQE_ID,REQE_TRID,REQE_DAT,VEH_NUM,VEH_ID,IDATE,ORG_CODE,ORG_NAME) values (" +
                                    "'" + requisitionTrip.PK_RequisitionTrip + "'," +
                                    "'" + requisitionTrip.TrackingID + "'," +
                                    "to_date('" + ((DateTime)requisitionTrip.FinalWantedAtDateTime).ToString("yyyy-MM-dd HH:mm:ss") + "', 'yyyy-mm-dd hh24:mi:ss')," +
                                    "'" + requisitionTrip.Vehicle.RegistrationNumber + "'," +
                                    "'" + ERP_Id + "'," +
                                    "to_date('" + now.ToString("yyyy-MM-dd HH:mm:ss") + "', 'yyyy-mm-dd hh24:mi:ss')," +
                                    "'" + requisitionTrip.Requisition.OrganizationCode + "', " +
                                    "'" + requisitionTrip.Requisition.OrganizationName + "'" +
                                    ")";
                                sqlCommandList.Add(sqlCommand);
                                var res = OracleRequisitionTrip_PRAN_DBHelper.DbSaveChanges(sqlCommandList);
                                if (res == true)
                                {
                                    requisitionTrip.OracleDB_IsPushed = true;
                                    requisitionTrip.OracleDB_PushedAt = now;
                                    bll.db.SaveChanges();
                                }
                            }
                            catch (Exception e)
                            {
                                bll.db.AppErrorLogs.Add(
                                      new AppErrorLog()
                                      {
                                          ErrorMessage = e.Message,
                                          ErrorTime = DateTime.Now,
                                          UserDefinedMessage = "Requisition/AssignVehicle_Trip-Push-PRAN"
                                      }
                                    );
                                bll.db.SaveChanges();
                            }
                        }

                        //# RFL Oracle Push
                        if (requisitionTrip.Requisition.PRG_Type == "RFL" && requisitionTrip.Requisition.OrganizationCode != null && requisitionTrip.Requisition.OrganizationName != null)
                        {
                            try
                            {
                                var now = DateTime.Now;
                                var sqlCommandList = new List<string>();
                                var ERP_Id = requisitionTrip.Vehicle.ERP_Id != null && requisitionTrip.Requisition.PRG_Type == requisitionTrip.Vehicle.Depo.PRG_Type ? requisitionTrip.Vehicle.ERP_Id : 55555;
                                var sqlCommand = "insert into T_REQE_RFL (REQE_ID,REQE_TRID,REQE_DAT,VEH_NUM,VEH_ID,IDATE,ORG_CODE,ORG_NAME) values (" +
                                    "'" + requisitionTrip.PK_RequisitionTrip + "'," +
                                    "'" + requisitionTrip.TrackingID + "'," +
                                    "to_date('" + ((DateTime)requisitionTrip.FinalWantedAtDateTime).ToString("yyyy-MM-dd HH:mm:ss") + "', 'yyyy-mm-dd hh24:mi:ss')," +
                                    "'" + requisitionTrip.Vehicle.RegistrationNumber + "'," +
                                    "'" + ERP_Id + "'," +
                                    "to_date('" + now.ToString("yyyy-MM-dd HH:mm:ss") + "', 'yyyy-mm-dd hh24:mi:ss')," +
                                    "'" + requisitionTrip.Requisition.OrganizationCode + "', " +
                                    "'" + requisitionTrip.Requisition.OrganizationName + "'" +
                                    ")";
                                sqlCommandList.Add(sqlCommand);
                                var res = OracleRequisitionTrip_RFL_DBHelper.DbSaveChanges(sqlCommandList);
                                if (res == true)
                                {
                                    requisitionTrip.OracleDB_IsPushed = true;
                                    requisitionTrip.OracleDB_PushedAt = now;
                                    bll.db.SaveChanges();
                                }
                            }
                            catch (Exception e)
                            {
                                bll.db.AppErrorLogs.Add(
                                      new AppErrorLog()
                                      {
                                          ErrorMessage = e.Message,
                                          ErrorTime = DateTime.Now,
                                          UserDefinedMessage = "Requisition/AssignVehicle_Trip-Push-RFL"
                                      }
                                    );
                                bll.db.SaveChanges();
                            }
                        }

                        CreateAlertMessage(AlertMessageType.Success, "Success", "Requisition is successfully assigned to a vehicle.");
                        return RedirectToAction("RequisitionTripIndex_Assigner");
                    }
                    else
                    {
                        var lastAssigner = bll.db.AppUsers.Where(m => m.PK_User == _vehicle.FK_RequisitionTrip_CurrentAssigner).FirstOrDefault();
                        CreateAlertMessage(AlertMessageType.Warning, "Warning", _vehicle.RegistrationNumber + "Can not be assigned. Already assigned by " + lastAssigner.FullName + " " + lastAssigner.Depo.Name);
                        return RedirectToAction("RequisitionTripIndex_Assigner");
                    }
                }
                else
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", "Requisition could not be assigned. Current status:" + requisitionTrip.StatusText);
                    return RedirectToAction("RequisitionTripIndex_Assigner");
                }
            }
            catch (Exception e)
            {
                CreateAlertMessage(AlertMessageType.Danger, "Warning", "Internal Error Ocuured.");
                return RedirectToAction("RequisitionTripIndex_Assigner");
            }
        }

        public ActionResult RequisitionTrip_Link(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.RequisitionTrips.Where(m => m.PK_RequisitionTrip == id && m.IsDeleted != true && m.StatusText == InternalTripStatus.Assigned).FirstOrDefault();
            var linkedTrips = new List<RequisitionTrip>();
            if (model != null)
            {
                linkedTrips = bll.db.RequisitionTrips.Where(m => m.IsDeleted != true && m.FK_RequisitionTrip_Parent == model.PK_RequisitionTrip).ToList();
            }

            var now = DateTime.Now;

            ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", now.Date);
            ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", now.AddDays(1).Date);

            var starting_limit = DateTime.Now.Date.AddDays(-7);
            var cliesntsQuery = bll.db.Requisitions.Where(m => m.IsDeleted != true && m.CreatedAt > starting_limit);
            if (CurrentUser.PRG_Type != "ALL")
            {
                cliesntsQuery = cliesntsQuery.Where(m => m.PRG_Type == CurrentUser.PRG_Type);
            }
            var _cliesnts = cliesntsQuery.Select(m => new { m.AppUser.PK_User, Text = m.AppUser.FullName + ":" + m.AppUser.Depo.Name }).Distinct().OrderBy(m => m.Text);
            ViewBag.Clients = new SelectList(_cliesnts, "PK_User", "Text");

            ViewBag.FromLocations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name");
            ViewBag.ToLocations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name");
            return View(new Tuple<RequisitionTrip, List<RequisitionTrip>>(model, linkedTrips));
        }
        [HttpPost]
        public ActionResult RequisitionTrip_Link(FormCollection form)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var _PK_RequisitionTrip_0 = Convert.ToInt64(form["PK_RequisitionTrip_0"]);
            RequisitionTrip requisitionTrip = bll.db.RequisitionTrips.Where(m => m.IsDeleted != true && m.Requisition.IsDeleted != true && m.PK_RequisitionTrip == _PK_RequisitionTrip_0 && m.StatusText == InternalTripStatus.Assigned).FirstOrDefault();
            try
            {
                if (requisitionTrip != null)
                {
                    //# Child Trips
                    var PK_RequisitionTrip_SL = Convert.ToInt32(form["PK_RequisitionTrip_SL"]);
                    for (int index = 1; index <= PK_RequisitionTrip_SL; index++)
                    {
                        //var _TrackingID = form["Pk_RequisitionTrip" + index];
                        var _PK_RequisitionTrip = form["PK_RequisitionTrip_" + index];
                        if (string.IsNullOrEmpty(_PK_RequisitionTrip))
                        {
                            continue;
                        }
                        var __PK_RequisitionTrip = Convert.ToInt64(_PK_RequisitionTrip);
                        RequisitionTrip _requisitionTrip = bll.db.RequisitionTrips.Where(m => m.IsDeleted != true && m.Requisition.IsDeleted != true && m.PK_RequisitionTrip == __PK_RequisitionTrip && (m.StatusText == InternalTripStatus.Created || (m.StatusText == InternalTripStatus.Cancelled && m.OracleDB_IsPushed != true))).FirstOrDefault();
                        if (_requisitionTrip != null)
                        {
                            //var _pk_vheicle = Guid.Parse(form["FK_Vehicle"]);
                            //var _vehicle = bll.db.Vehicles.Where(m => m.PK_Vehicle == _pk_vheicle).Where(m => (m.OWN_MHT_DHT == "OWN" || m.OWN_MHT_DHT == "MHT" || m.OWN_MHT_DHT == "DHT") /*&& accessibleDepoes.Contains(m.FK_Depo)*/
                            //).FirstOrDefault();
                            _requisitionTrip.FK_Vehicle = requisitionTrip.FK_Vehicle;
                            _requisitionTrip.OWN_MHT_DHT = requisitionTrip.OWN_MHT_DHT;
                            _requisitionTrip.PRG_Type = CurrentUser.PRG_Type;
                            _requisitionTrip.Driver_Staff_ID = requisitionTrip.Driver_Staff_ID;
                            _requisitionTrip.Driver_Name = requisitionTrip.Driver_Name;
                            _requisitionTrip.Driver_ContactNumber = requisitionTrip.Driver_ContactNumber;
                            if (_requisitionTrip.TotalAmount != null)
                            {
                                _requisitionTrip.TotalAmount = requisitionTrip.TotalAmount;
                            }
                            _requisitionTrip.FK_AppUser_Assigner = CurrentUser.PK_User;
                            _requisitionTrip.StatusText = InternalTripStatus.Assigned;
                            _requisitionTrip.AssingedAt = DateTime.Now;

                            requisitionTrip.IsParent = false;
                            _requisitionTrip.FK_RequisitionTrip_Parent = requisitionTrip.PK_RequisitionTrip;
                            bll.db.SaveChanges();

                            //# Notify Raiser Mail
                            if (!string.IsNullOrEmpty(_requisitionTrip.Requisition.AppUser.Email))
                            {
                                try
                                {
                                    var Mail_Subject = "Requisition " + _requisitionTrip.StatusText + " " + _requisitionTrip.TrackingID;
                                    var Mail_Body = "Dear Concern, The below requisition is " + _requisitionTrip.StatusText + ".<br>";
                                    Mail_Body = Mail_Body + "<b>Requisition Detail</b>" + "<br>" +
                                        "Demand: " + _requisitionTrip.TrackingID + "<br>" +
                                        "Requistion: " + _requisitionTrip.TrackingID + "<br>" +
                                        "Vehicle: " + requisitionTrip.Vehicle.RegistrationNumber + " " +
                                        requisitionTrip.Vehicle.OWN_MHT_DHT + " " +
                                        requisitionTrip.Vehicle.VehicleType + "<br>" +

                                        "Driver: " + (string.IsNullOrEmpty(requisitionTrip.Driver_Staff_ID) ? "" : requisitionTrip.Driver_Staff_ID) + " " +
                                        (string.IsNullOrEmpty(requisitionTrip.Driver_Name) ? "" : requisitionTrip.Driver_Name) + " " +
                                        (string.IsNullOrEmpty(requisitionTrip.Driver_ContactNumber) ? "" : requisitionTrip.Driver_ContactNumber) + "<br>" +

                                        "Total Amount: " + (requisitionTrip.TotalAmount != null ? "" : requisitionTrip.TotalAmount + " TK") + " " + "<br>" +
                                        "From: " + requisitionTrip.Requisition.Location.Name + " " + requisitionTrip.Requisition.StartingLocation + "<br>" +
                                        "To: " + requisitionTrip.Requisition.Location1.Name + " " + _requisitionTrip.Requisition.FinishingLocation + "<br>" +
                                        "Available At: " + requisitionTrip.FinalWantedAtDateTime + "<br>";
                                    SendMail_Single(_requisitionTrip.Requisition.AppUser.Email, Mail_Subject, Mail_Body);
                                }
                                catch (Exception e)
                                {
                                }
                            }
                        }
                    }
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Requisition is successfully linked with other trip.");
                    return RedirectToAction("RequisitionTrip_Link", new { id = _PK_RequisitionTrip_0 });
                }
                else
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", "Requisition could not be assigned as it is not in Assigend status.");
                    return RedirectToAction("RequisitionTrip_Link", new { id = _PK_RequisitionTrip_0 });
                }
            }
            catch (Exception e)
            {
                CreateAlertMessage(AlertMessageType.Danger, "Warning", "Internal Error Ocuured.");
                return RedirectToAction("RequisitionTrip_Link", new { id = _PK_RequisitionTrip_0 });
            }
        }

        //#ExternalTrip
        public ActionResult ExternalTripIndex(DateTime? StartingDate, DateTime? EndingDate, Guid? FK_Location_From, Guid? FK_Location_To)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }

            var _query_InterCompanyRequisition_ExternalVehicle = bll.db.InterCompanyRequisition_ExternalVehicle.Where(m => 1 == 1);
            var _query_RequisitionTrip = bll.db.RequisitionTrips.Where(c => c.IsDeleted != true && c.OWN_MHT_DHT == "DHT" && c.FK_Vehicle != null);
            var _query_RequisitionTrip_Finished = bll.db.RequisitionTrip_Finished.Where(c => c.IsDeleted != true && c.OWN_MHT_DHT == "DHT" && c.FK_Vehicle != null);

            if (CurrentUser.PRG_Type != "ALL")
            {
                _query_RequisitionTrip = _query_RequisitionTrip.Where(c => c.Requisition.PRG_Type == CurrentUser.PRG_Type);
                _query_RequisitionTrip_Finished = _query_RequisitionTrip_Finished.Where(c => c.Requisition.PRG_Type == CurrentUser.PRG_Type);
            }

            var interCompanyRequisition_ExternalVehicleList = new List<InterCompanyRequisition_ExternalVehicle>();
            var requisitionTripList = new List<RequisitionTrip>();
            var requisitionTripList_finished = new List<RequisitionTrip_Finished>();

            //# StartingDate
            if (StartingDate != null)
            {
                var _StartingDate = StartingDate != null ? StartingDate : new DateTime();

                _query_InterCompanyRequisition_ExternalVehicle = _query_InterCompanyRequisition_ExternalVehicle.Where(c => c.CreatedAt > StartingDate);
                _query_RequisitionTrip = _query_RequisitionTrip.Where(c => c.OWN_MHT_DHT == "DHT" && c.FK_Vehicle != null && c.AssingedAt > StartingDate);
                _query_RequisitionTrip_Finished = _query_RequisitionTrip_Finished.Where(c => c.OWN_MHT_DHT == "DHT" && c.FK_Vehicle != null && c.AssingedAt > StartingDate);

                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", _StartingDate);
            }
            else
            {
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", DateTime.Today.Date);
            }

            //# EndingDate
            if (EndingDate != null)
            {
                var _EndingDate = EndingDate != null ? EndingDate.Value.AddDays(1) : new DateTime();

                _query_InterCompanyRequisition_ExternalVehicle = _query_InterCompanyRequisition_ExternalVehicle.Where(c => c.CreatedAt < _EndingDate);
                _query_RequisitionTrip = _query_RequisitionTrip.Where(c => c.OWN_MHT_DHT == "DHT" && c.FK_Vehicle != null && c.AssingedAt < _EndingDate);
                _query_RequisitionTrip_Finished = _query_RequisitionTrip_Finished.Where(c => c.OWN_MHT_DHT == "DHT" && c.FK_Vehicle != null && c.AssingedAt < _EndingDate);

                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", _EndingDate);
            }
            else
            {
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", DateTime.Today.AddDays(1).Date);
            }

            //# FK_Location_From
            if (FK_Location_From != null)
            {
                _query_RequisitionTrip = _query_RequisitionTrip.Where(m => m.Requisition.FK_Location_From == FK_Location_From);
                _query_RequisitionTrip_Finished = _query_RequisitionTrip_Finished.Where(m => m.Requisition.FK_Location_From == FK_Location_From);
                ViewBag.FromLocations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name", FK_Location_From);
            }
            else
            {
                ViewBag.FromLocations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name");
            }

            //# FK_Location_To
            if (FK_Location_To != null)
            {
                _query_RequisitionTrip = _query_RequisitionTrip.Where(m => m.Requisition.FK_Location_To == FK_Location_To);
                _query_RequisitionTrip_Finished = _query_RequisitionTrip_Finished.Where(m => m.Requisition.FK_Location_To == FK_Location_To);
                ViewBag.ToLocations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name", FK_Location_To);
            }
            else
            {
                ViewBag.ToLocations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name");
            }

            if (StartingDate != null || EndingDate != null)
            {
                interCompanyRequisition_ExternalVehicleList = interCompanyRequisition_ExternalVehicleList.OrderBy(m => m.CreatedAt).ToList();
                requisitionTripList = _query_RequisitionTrip.OrderByDescending(m => m.AssingedAt).ToList();
                requisitionTripList_finished = _query_RequisitionTrip_Finished.OrderByDescending(m => m.AssingedAt).ToList();
            }

            return View(new Tuple<List<InterCompanyRequisition_ExternalVehicle>, List<RequisitionTrip>, List<RequisitionTrip_Finished>>(interCompanyRequisition_ExternalVehicleList, requisitionTripList, requisitionTripList_finished));
        }
        public ActionResult RequisitionTrip_Pay(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var requisitionTrip = bll.db.RequisitionTrips.Find(id);
            if (requisitionTrip != null)
            {
                return View(requisitionTrip);
            }
            else
            {
                var requisitionTrip_Finished = bll.db.RequisitionTrip_Finished.Find(id);
                if (requisitionTrip_Finished != null)
                {
                    requisitionTrip = new RequisitionTrip();
                    requisitionTrip.PK_RequisitionTrip = requisitionTrip_Finished.PK_RequisitionTrip_Finished;
                    requisitionTrip.TrackingID = requisitionTrip_Finished.TrackingID;

                    return View(requisitionTrip);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
        [HttpPost]
        public ActionResult RequisitionTrip_Pay(FormCollection formCollection)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            try
            {
                var PK_RequisitionTrip = Convert.ToInt64(formCollection["PK_RequisitionTrip"]);
                var requisitionTrip = bll.db.RequisitionTrips.Where(m => m.PK_RequisitionTrip == PK_RequisitionTrip).FirstOrDefault();
                if (requisitionTrip != null)
                {
                    if (!string.IsNullOrEmpty(requisitionTrip.PaymentStatus))
                    {
                        CreateAlertMessage(AlertMessageType.Danger, "Danger", "Hired trip is already paid.");
                        return RedirectToAction("ExternalTripIndex");
                    }
                    requisitionTrip.PaymentStatus = "Paid";
                    requisitionTrip.PaidAt = DateTime.Now;
                    requisitionTrip.FK_PaidByUser = CurrentUser.PK_User;
                    bll.db.SaveChanges();
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Hired trip is successfully paid.");
                }
                else
                {
                    var requisitionTrip_Finished = bll.db.RequisitionTrip_Finished.Find(PK_RequisitionTrip);
                    if (requisitionTrip_Finished != null)
                    {
                        if (!string.IsNullOrEmpty(requisitionTrip.PaymentStatus))
                        {
                            CreateAlertMessage(AlertMessageType.Danger, "Danger", "Hired trip is already paid.");
                            return RedirectToAction("ExternalTripIndex");
                        }
                        requisitionTrip_Finished.PaymentStatus = "Paid";
                        requisitionTrip_Finished.PaidAt = DateTime.Now;
                        requisitionTrip_Finished.FK_PaidByUser = CurrentUser.PK_User;
                        bll.db.SaveChanges();
                        CreateAlertMessage(AlertMessageType.Success, "Success", "Hired trip is successfully paid.");
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                }
                return RedirectToAction("ExternalTripIndex");
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                return RedirectToAction("ExternalTripIndex");
            }
        }
        public ActionResult EntryCurrentDateGPCount()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            ViewBag.PRG_Type = CurrentUser.PRG_Type;
            ViewBag.IssueDate = String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-1).Date);

            var query = bll.db.Locations.Where(m => m.IsDeleted == false && (m.LocationType == "Factory" || m.LocationType == "Depo"));
            if (CurrentUser.PRG_Type == "ALL")
            {
                query = query.Where(m => m.PK_Location == null);
            }
            else
            {
                query = query.Where(m => m.PRG_Type == CurrentUser.PRG_Type || m.PRG_Type == "ALL");
            }
            var list = query.ToList();
            return View(list);
        }
        [HttpPost]
        public ActionResult EntryCurrentDateGPCount(FormCollection form)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var item_total = Convert.ToInt32(form["item_total"]);
            var PRG_Type = CurrentUser.PRG_Type;
            var IssueDate = Convert.ToDateTime(form["IssueDate"]);
            var now = DateTime.Now;
            if (bll.db.LocationWiseGPs.Where(m => m.PRG_Type == PRG_Type && m.IssueDate == IssueDate).Any())
            {
                var old_list = bll.db.LocationWiseGPs.Where(m => m.PRG_Type == PRG_Type && m.IssueDate == IssueDate).ToList();
                bll.db.LocationWiseGPs.RemoveRange(old_list);
                bll.db.SaveChanges();
            }
            for (int i = 1; i <= item_total; i++)
            {
                var FK_Location_ = form["FK_Location_" + i];
                var GPCount_ = form["GPCount_" + i];
                if (!string.IsNullOrEmpty(FK_Location_) && !string.IsNullOrEmpty(GPCount_))
                {
                    var locationWiseGP = new LocationWiseGP()
                    {
                        PRG_Type = PRG_Type,
                        IssueDate = IssueDate,
                        FK_Location = Guid.Parse(form["FK_Location_" + i]),
                        GPCount = Convert.ToInt64(form["GPCount_" + i]),
                        FK_AppUser_Creator = CurrentUser.PK_User,
                        CreatedAt = now
                    };
                    bll.db.LocationWiseGPs.Add(locationWiseGP);
                }
                bll.db.SaveChanges();
            }
            CreateAlertMessage(AlertMessageType.Success, "Success", "Depo VS GP data update for " + form["PRG_Type"] + " Date: " + IssueDate.ToShortDateString());
            return RedirectToAction("EntryCurrentDateGPCount");
        }

        //#Helper and Ajax Method
        public ActionResult RequisitionTrip_Cancel(Int64 id)
        {
            try
            {
                var pendingTrip = bll.db.RequisitionTrips.Where(m => m.IsDeleted != true && m.Requisition.IsDeleted != true && m.PK_RequisitionTrip == id).FirstOrDefault();

                var message = "";//pendingTrip

                if (pendingTrip.StatusText == InternalTripStatus.Assigned)
                /*&& (pendingTrip.Requisition.FK_Depo_From == pendingTrip.Vehicle.FK_DepoInOut && pendingTrip.Vehicle.LocationInOrOut == true)*/
                {
                    pendingTrip.FK_Vehicle = null;
                    pendingTrip.OWN_MHT_DHT = null;
                    pendingTrip.PRG_Type = null;
                    pendingTrip.FinalWantedAtDateTime = null;
                    pendingTrip.Driver_Staff_ID = null;
                    pendingTrip.Driver_Name = null;
                    pendingTrip.TotalAmount = null;
                    pendingTrip.StatusText = InternalTripStatus.Cancelled;
                    pendingTrip.FK_AppUser_Canceller = CurrentUser.PK_User;
                    pendingTrip.CancelledAt = DateTime.Now;

                    if (!string.IsNullOrEmpty(pendingTrip.Requisition.AppUser.Email))
                    {
                        try
                        {
                            var Mail_Subject = "Requisition " + pendingTrip.StatusText + " " + pendingTrip.TrackingID;
                            var Mail_Body = "Dear Concern, The below requisition is " + pendingTrip.StatusText + ".<br>";
                            Mail_Body = Mail_Body + "<b>Requisition Detail</b>" + "<br>" +
                                "Demand: " + pendingTrip.TrackingID + "<br>" +
                                "Requistion: " + pendingTrip.TrackingID + "<br>" +
                                "From: " + pendingTrip.Requisition.Location.Name + " " + pendingTrip.Requisition.StartingLocation + "<br>" +
                                "To: " + pendingTrip.Requisition.Location1.Name + " " + pendingTrip.Requisition.FinishingLocation + "<br>" +
                                "Cacelled At: " + pendingTrip.CancelledAt + "<br>";
                            SendMail_Single(pendingTrip.Requisition.AppUser.Email, Mail_Subject, Mail_Body);
                        }
                        catch (Exception e)
                        {
                        }
                    }

                    //# Child Trips
                    if (pendingTrip.IsParent == true)
                    {
                        pendingTrip.IsParent = null;

                        var pendingChildTripList = bll.db.RequisitionTrips.Where(m => m.FK_RequisitionTrip_Parent == pendingTrip.PK_RequisitionTrip).ToList();
                        foreach (var pendingChildTrip in pendingChildTripList)
                        {
                            pendingChildTrip.FK_RequisitionTrip_Parent = null;
                            pendingChildTrip.FK_Vehicle = null;
                            pendingChildTrip.OWN_MHT_DHT = null;
                            pendingChildTrip.PRG_Type = null;
                            pendingChildTrip.Driver_Staff_ID = null;
                            pendingChildTrip.Driver_Name = null;
                            pendingChildTrip.TotalAmount = null;
                            pendingChildTrip.TentativeFinishingDateTime = null;
                            pendingChildTrip.FK_AppUser_Assigner = null;
                            pendingChildTrip.StatusText = InternalTripStatus.Cancelled;
                            pendingChildTrip.FK_AppUser_Canceller = CurrentUser.PK_User;
                            pendingChildTrip.CancelledAt = DateTime.Now;

                            if (!string.IsNullOrEmpty(pendingChildTrip.Requisition.AppUser.Email))
                            {
                                try
                                {
                                    var Mail_Subject = "Requisition " + pendingChildTrip.StatusText + " " + pendingChildTrip.TrackingID;
                                    var Mail_Body = "Dear Concern, The below requisition is " + pendingChildTrip.StatusText + ".<br>";
                                    Mail_Body = Mail_Body + "<b>Requisition Detail</b>" + "<br>" +
                                        "Demand: " + pendingChildTrip.TrackingID + "<br>" +
                                        "Requistion: " + pendingChildTrip.TrackingID + "<br>" +
                                        "From: " + pendingChildTrip.Requisition.Location.Name + " " + pendingChildTrip.Requisition.StartingLocation + "<br>" +
                                        "To: " + pendingChildTrip.Requisition.Location1.Name + " " + pendingChildTrip.Requisition.FinishingLocation + "<br>" +
                                        "Cacelled At: " + pendingTrip.CancelledAt + "<br>";
                                    SendMail_Single(pendingChildTrip.Requisition.AppUser.Email, Mail_Subject, Mail_Body);
                                }
                                catch (Exception e)
                                {
                                }
                            }
                        }
                    }
                    bll.db.SaveChanges();

                    if (!bll.db.RequisitionTrips.Where(m => m.FK_Vehicle == pendingTrip.FK_Vehicle && m.StatusText == InternalTripStatus.Assigned).Any())
                    {
                        pendingTrip.Vehicle.FK_RequisitionTrip_CurrentAssigner = null;
                        bll.db.SaveChanges();
                    }
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Requisition cancelled successfully.");
                    return RedirectToAction("RequisitionTripIndex_Assigner");
                }
                else if (pendingTrip.StatusText != InternalTripStatus.Assigned)
                {
                    CreateAlertMessage(AlertMessageType.Danger, "Danger", "Requisition can not be cancelled. Current status: " + pendingTrip.StatusText);
                    return RedirectToAction("RequisitionTripIndex_Assigner");
                }
                else
                {
                    message = "Undefined Reasson : CancelTrip 1.1";
                    CreateAlertMessage(AlertMessageType.Danger, "Danger", message);
                    return RedirectToAction("RequisitionTripIndex_Assigner");
                }
            }
            catch (Exception e)
            {
                var errrorMessage = "";
                do { errrorMessage = "#" + errrorMessage + e.Message; if (e.InnerException != null) { e = e.InnerException; } else { break; } } while (true);
                //return errrorMessage;
                CreateAlertMessage(AlertMessageType.Warning, "Danger", errrorMessage);
                return RedirectToAction("RequisitionTripIndex_Assigner");
            }
        }
        public ActionResult RequisitionTrip_Start(Int64 id, string StartAutoOrManaul, Guid FK_AppUser_Start)
        {
            try
            {
                var pendingTrip = bll.db.RequisitionTrips.Where(m => m.IsDeleted != true && m.Requisition.IsDeleted != true && m.PK_RequisitionTrip == id).FirstOrDefault();
                //if (pendingTrip.IsNotifiedToDriver != true)
                //{
                //    pendingTrip.IsNotifiedToDriver = true;
                //    pendingTrip.NotifiedToDriverAt = DateTime.Now;
                //}
                //var currentTrip = bll.db.RequisitionTrip.Where(m => m.PK_RequisitionTrip == pendingTrip.Vehicle.FK_RequisitionTrip_Current).FirstOrDefault();

                var message = "";

                //-check no current trip and vehicle is inside of pending trip's starting location
                if ((pendingTrip.StatusText == InternalTripStatus.Assigned)
                    /*&& (pendingTrip.Requisition.FK_Depo_From == pendingTrip.Vehicle.FK_DepoInOut && pendingTrip.Vehicle.LocationInOrOut == true)*/)
                {
                    pendingTrip.StatusText = InternalTripStatus.Started;
                    pendingTrip.StartAutoOrManaul = StartAutoOrManaul;
                    pendingTrip.FK_LocationGate_Start = FK_AppUser_Start;
                    pendingTrip.StartedAt = DateTime.Now;
                    pendingTrip.Vehicle.FK_RequisitionTrip_Last = pendingTrip.PK_RequisitionTrip;
                    var locationToLocationMapping = bll.db.LocationToLocationMappings.Where(m => (m.FK_Location_1 == pendingTrip.Requisition.Location.PK_Location && m.FK_Location_2 == pendingTrip.Requisition.Location1.PK_Location)
                    || (m.FK_Location_2 == pendingTrip.Requisition.Location.PK_Location && m.FK_Location_1 == pendingTrip.Requisition.Location1.PK_Location)
                    ).FirstOrDefault();
                    if (locationToLocationMapping != null && locationToLocationMapping.StandardTravelTimeMinute != null)
                    {
                        pendingTrip.TentativeFinishingDateTime = (pendingTrip.StartedAt ?? DateTime.Now).AddMinutes(locationToLocationMapping.StandardTravelTimeMinute ?? 0);
                    }
                    // Update Vehicle Trip
                    //pendingTrip.Vehicle.FK_RequisitionTrip_Current = pendingTrip.Vehicle.FK_RequisitionTrip_Pending;
                    //pendingTrip.Vehicle.FK_RequisitionTrip_Pending = null;


                    // Update Driver Trip
                    //pendingTrip.AppUser4.FK_RequisitionTrip_Current = pendingTrip.AppUser4.FK_RequisitionTrip_Pending;
                    //pendingTrip.AppUser4.FK_RequisitionTrip_Pending = null;

                    //# Notify Assigner Mail
                    if (!string.IsNullOrEmpty(pendingTrip.AppUser.Email))
                    {
                        try
                        {
                            var Mail_Subject = "Requisition " + pendingTrip.StatusText + " " + pendingTrip.TrackingID;
                            var Mail_Body = "Dear Concern, The below requisition is " + pendingTrip.StatusText + ".<br>";
                            Mail_Body = Mail_Body + "<b>Requisition Detail</b>" + "<br>" +
                                "Demand: " + pendingTrip.TrackingID + "<br>" +
                                "Requistion: " + pendingTrip.TrackingID + "<br>" +
                                "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + " " +
                                pendingTrip.Vehicle.OWN_MHT_DHT + " " +
                                pendingTrip.Vehicle.VehicleType + "<br>" +

                                "Driver: " + (string.IsNullOrEmpty(pendingTrip.Driver_Staff_ID) ? "" : pendingTrip.Driver_Staff_ID) + " " +
                                (string.IsNullOrEmpty(pendingTrip.Driver_Name) ? "" : pendingTrip.Driver_Name) + " " +
                                (string.IsNullOrEmpty(pendingTrip.Driver_ContactNumber) ? "" : pendingTrip.Driver_ContactNumber) + "<br>" +

                                "Total Amount: " + (pendingTrip.TotalAmount != null ? "" : pendingTrip.TotalAmount + " TK") + " " + "<br>" +
                                "From: " + pendingTrip.Requisition.Location.Name + " " + pendingTrip.Requisition.StartingLocation + "<br>" +
                                "To: " + pendingTrip.Requisition.Location1.Name + " " + pendingTrip.Requisition.FinishingLocation + "<br>" +
                                "Started At: " + pendingTrip.StartedAt + "<br>";
                            SendMail_Single(pendingTrip.AppUser.Email, Mail_Subject, Mail_Body);
                        }
                        catch (Exception e)
                        {
                        }
                    }

                    //# Notify Raiser Mail
                    if (!string.IsNullOrEmpty(pendingTrip.Requisition.AppUser.Email))
                    {
                        try
                        {
                            var Mail_Subject = "Requisition " + pendingTrip.StatusText + " " + pendingTrip.TrackingID;
                            var Mail_Body = "Dear Concern, The below requisition is " + pendingTrip.StatusText + ".<br>";
                            Mail_Body = Mail_Body + "<b>Requisition Detail</b>" + "<br>" +
                                "Demand: " + pendingTrip.TrackingID + "<br>" +
                                "Requistion: " + pendingTrip.TrackingID + "<br>" +
                                "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + " " +
                                pendingTrip.Vehicle.OWN_MHT_DHT + " " +
                                pendingTrip.Vehicle.VehicleType + "<br>" +

                                "Driver: " + (string.IsNullOrEmpty(pendingTrip.Driver_Staff_ID) ? "" : pendingTrip.Driver_Staff_ID) + " " +
                                (string.IsNullOrEmpty(pendingTrip.Driver_Name) ? "" : pendingTrip.Driver_Name) + " " +
                                (string.IsNullOrEmpty(pendingTrip.Driver_ContactNumber) ? "" : pendingTrip.Driver_ContactNumber) + "<br>" +

                                "Total Amount: " + (pendingTrip.TotalAmount != null ? "" : pendingTrip.TotalAmount + " TK") + " " + "<br>" +
                                "From: " + pendingTrip.Requisition.Location.Name + " " + pendingTrip.Requisition.StartingLocation + "<br>" +
                                "To: " + pendingTrip.Requisition.Location1.Name + " " + pendingTrip.Requisition.FinishingLocation + "<br>" +
                                "Started At: " + pendingTrip.StartedAt + "<br>";
                            SendMail_Single(pendingTrip.Requisition.AppUser.Email, Mail_Subject, Mail_Body);
                        }
                        catch (Exception e)
                        {
                        }
                    }
                    bll.db.SaveChanges();
                    if (!bll.db.RequisitionTrips.Where(m => m.FK_Vehicle == pendingTrip.FK_Vehicle && m.StatusText == InternalTripStatus.Assigned).Any())
                    {
                        pendingTrip.Vehicle.FK_RequisitionTrip_CurrentAssigner = null;
                        bll.db.SaveChanges();
                    }
                    //return "YES";
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Requisition started successfully");
                    return RedirectToAction("RequisitionTripIndex_Assigner");
                }
                else if (pendingTrip.StatusText != InternalTripStatus.Assigned)
                {
                    CreateAlertMessage(AlertMessageType.Danger, "Danger", "Requisition can not be started. Current status: " + pendingTrip.StatusText);
                    return RedirectToAction("RequisitionTripIndex_Assigner");
                }
                else
                {
                    message = "Undefined Reasson : StartTrip 1.1";
                    //return message;
                    CreateAlertMessage(AlertMessageType.Danger, "Danger", message);
                    return RedirectToAction("RequisitionTripIndex_Assigner");
                }
            }
            catch (Exception e)
            {
                var errrorMessage = "";
                do { errrorMessage = "#" + errrorMessage + e.Message; if (e.InnerException != null) { e = e.InnerException; } else { break; } } while (true);
                //return errrorMessage;
                CreateAlertMessage(AlertMessageType.Warning, "Danger", errrorMessage);
                return RedirectToAction("RequisitionTripIndex_Assigner");
            }
        }
        public ActionResult RequisitionTrip_Start_Client(Int64 id, string StartAutoOrManaul, Guid FK_AppUser_Start)
        {
            try
            {
                var pendingTrip = bll.db.RequisitionTrips.Where(m => m.IsDeleted != true && m.Requisition.IsDeleted != true && m.PK_RequisitionTrip == id).FirstOrDefault();
                //if (pendingTrip.IsNotifiedToDriver != true)
                //{
                //    pendingTrip.IsNotifiedToDriver = true;
                //    pendingTrip.NotifiedToDriverAt = DateTime.Now;
                //}
                //var currentTrip = bll.db.RequisitionTrip.Where(m => m.PK_RequisitionTrip == pendingTrip.Vehicle.FK_RequisitionTrip_Current).FirstOrDefault();

                var message = "";

                //-check no current trip and vehicle is inside of pending trip's starting location
                if ((pendingTrip.StatusText == InternalTripStatus.Assigned)
                    /*&& (pendingTrip.Requisition.FK_Depo_From == pendingTrip.Vehicle.FK_DepoInOut && pendingTrip.Vehicle.LocationInOrOut == true)*/)
                {
                    pendingTrip.StatusText = InternalTripStatus.Started;
                    pendingTrip.StartAutoOrManaul = StartAutoOrManaul;
                    pendingTrip.FK_LocationGate_Start = FK_AppUser_Start;
                    pendingTrip.StartedAt = DateTime.Now;
                    pendingTrip.Vehicle.FK_RequisitionTrip_Last = pendingTrip.PK_RequisitionTrip;
                    var locationToLocationMapping = bll.db.LocationToLocationMappings.Where(m => (m.FK_Location_1 == pendingTrip.Requisition.Location.PK_Location && m.FK_Location_2 == pendingTrip.Requisition.Location1.PK_Location)
                    || (m.FK_Location_2 == pendingTrip.Requisition.Location.PK_Location && m.FK_Location_1 == pendingTrip.Requisition.Location1.PK_Location)
                    ).FirstOrDefault();
                    if (locationToLocationMapping != null && locationToLocationMapping.StandardTravelTimeMinute != null)
                    {
                        pendingTrip.TentativeFinishingDateTime = (pendingTrip.StartedAt ?? DateTime.Now).AddMinutes(locationToLocationMapping.StandardTravelTimeMinute ?? 0);
                    }
                    // Update Vehicle Trip
                    //pendingTrip.Vehicle.FK_RequisitionTrip_Current = pendingTrip.Vehicle.FK_RequisitionTrip_Pending;
                    //pendingTrip.Vehicle.FK_RequisitionTrip_Pending = null;


                    // Update Driver Trip
                    //pendingTrip.AppUser4.FK_RequisitionTrip_Current = pendingTrip.AppUser4.FK_RequisitionTrip_Pending;
                    //pendingTrip.AppUser4.FK_RequisitionTrip_Pending = null;

                    //# Notify Assigner Mail
                    if (!string.IsNullOrEmpty(pendingTrip.AppUser.Email))
                    {
                        try
                        {
                            var Mail_Subject = "Requisition " + pendingTrip.StatusText + " " + pendingTrip.TrackingID;
                            var Mail_Body = "Dear Concern, The below requisition is " + pendingTrip.StatusText + ".<br>";
                            Mail_Body = Mail_Body + "<b>Requisition Detail</b>" + "<br>" +
                                "Demand: " + pendingTrip.TrackingID + "<br>" +
                                "Requistion: " + pendingTrip.TrackingID + "<br>" +
                                "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + " " +
                                pendingTrip.Vehicle.OWN_MHT_DHT + " " +
                                pendingTrip.Vehicle.VehicleType + "<br>" +

                                "Driver: " + (string.IsNullOrEmpty(pendingTrip.Driver_Staff_ID) ? "" : pendingTrip.Driver_Staff_ID) + " " +
                                (string.IsNullOrEmpty(pendingTrip.Driver_Name) ? "" : pendingTrip.Driver_Name) + " " +
                                (string.IsNullOrEmpty(pendingTrip.Driver_ContactNumber) ? "" : pendingTrip.Driver_ContactNumber) + "<br>" +

                                "Total Amount: " + (pendingTrip.TotalAmount != null ? "" : pendingTrip.TotalAmount + " TK") + " " + "<br>" +
                                "From: " + pendingTrip.Requisition.Location.Name + " " + pendingTrip.Requisition.StartingLocation + "<br>" +
                                "To: " + pendingTrip.Requisition.Location1.Name + " " + pendingTrip.Requisition.FinishingLocation + "<br>" +
                                "Started At: " + pendingTrip.StartedAt + "<br>";
                            SendMail_Single(pendingTrip.AppUser.Email, Mail_Subject, Mail_Body);
                        }
                        catch (Exception e)
                        {
                        }
                    }

                    //# Notify Raiser Mail
                    if (!string.IsNullOrEmpty(pendingTrip.Requisition.AppUser.Email))
                    {
                        try
                        {
                            var Mail_Subject = "Requisition " + pendingTrip.StatusText + " " + pendingTrip.TrackingID;
                            var Mail_Body = "Dear Concern, The below requisition is " + pendingTrip.StatusText + ".<br>";
                            Mail_Body = Mail_Body + "<b>Requisition Detail</b>" + "<br>" +
                                "Demand: " + pendingTrip.TrackingID + "<br>" +
                                "Requistion: " + pendingTrip.TrackingID + "<br>" +
                                "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + " " +
                                pendingTrip.Vehicle.OWN_MHT_DHT + " " +
                                pendingTrip.Vehicle.VehicleType + "<br>" +

                                "Driver: " + (string.IsNullOrEmpty(pendingTrip.Driver_Staff_ID) ? "" : pendingTrip.Driver_Staff_ID) + " " +
                                (string.IsNullOrEmpty(pendingTrip.Driver_Name) ? "" : pendingTrip.Driver_Name) + " " +
                                (string.IsNullOrEmpty(pendingTrip.Driver_ContactNumber) ? "" : pendingTrip.Driver_ContactNumber) + "<br>" +

                                "Total Amount: " + (pendingTrip.TotalAmount != null ? "" : pendingTrip.TotalAmount + " TK") + " " + "<br>" +
                                "From: " + pendingTrip.Requisition.Location.Name + " " + pendingTrip.Requisition.StartingLocation + "<br>" +
                                "To: " + pendingTrip.Requisition.Location1.Name + " " + pendingTrip.Requisition.FinishingLocation + "<br>" +
                                "Started At: " + pendingTrip.StartedAt + "<br>";
                            SendMail_Single(pendingTrip.Requisition.AppUser.Email, Mail_Subject, Mail_Body);
                        }
                        catch (Exception e)
                        {
                        }
                    }
                    bll.db.SaveChanges();
                    if (!bll.db.RequisitionTrips.Where(m => m.FK_Vehicle == pendingTrip.FK_Vehicle && m.StatusText == InternalTripStatus.Assigned).Any())
                    {
                        pendingTrip.Vehicle.FK_RequisitionTrip_CurrentAssigner = null;
                        bll.db.SaveChanges();
                    }
                    //return "YES";
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Requisition started successfully");
                    return RedirectToAction("RequisitionTripIndex_Client");
                }
                else if (pendingTrip.StatusText != InternalTripStatus.Assigned)
                {
                    CreateAlertMessage(AlertMessageType.Danger, "Danger", "Requisition can not be started. Current status: " + pendingTrip.StatusText);
                    return RedirectToAction("RequisitionTripIndex_Client");
                }
                else
                {
                    message = "Undefined Reasson : StartTrip 1.2";
                    CreateAlertMessage(AlertMessageType.Danger, "Danger", message);
                    return RedirectToAction("RequisitionTripIndex_Client");
                }
            }
            catch (Exception e)
            {
                var errrorMessage = "";
                do { errrorMessage = "#" + errrorMessage + e.Message; if (e.InnerException != null) { e = e.InnerException; } else { break; } } while (true);
                //return errrorMessage;
                CreateAlertMessage(AlertMessageType.Warning, "Danger", errrorMessage);
                return RedirectToAction("RequisitionTripIndex_Client");
            }
        }
        public ActionResult RequisitionTrip_Finish(Int64 id, string FinishAutoOrManaul, Guid FK_AppUser_Finish)
        {
            try
            {
                var currentTrip = bll.db.RequisitionTrips.Where(m => m.IsDeleted != true && m.Requisition.IsDeleted != true && m.PK_RequisitionTrip == id).FirstOrDefault();

                var now = DateTime.Now;
                var message = "";

                if (currentTrip != null)
                {

                    if (currentTrip.StatusText == InternalTripStatus.Started /*&& (currentTrip.Requisition.FK_Depo_To == currentTrip.Vehicle.FK_DepoInOut && currentTrip.Vehicle.LocationInOrOut == true)*/)
                    {
                        currentTrip.StatusText = InternalTripStatus.Finished;
                        currentTrip.FinishAutoOrManaul = FinishAutoOrManaul;
                        currentTrip.FK_LocationGate_Finish = FK_AppUser_Finish;
                        currentTrip.FinishedAt = DateTime.Now;
                        if (currentTrip.Vehicle.FK_RequisitionTrip_Last == currentTrip.PK_RequisitionTrip)
                        {
                            currentTrip.Vehicle.FK_RequisitionTrip_Last = null;
                        }

                        // Update Vehicle Trip
                        //currentTrip.Vehicle.FK_RequisitionTrip_Current = null;

                        // Update Driver Trip
                        //currentTrip.AppUser4.FK_RequisitionTrip_Current = null;

                        //# Notify Assigner Mail
                        if (!string.IsNullOrEmpty(currentTrip.AppUser.Email))
                        {
                            try
                            {
                                var Mail_Subject = "Requisition " + currentTrip.StatusText + " " + currentTrip.TrackingID;
                                var Mail_Body = "Dear Concern, The below requisition is " + currentTrip.StatusText + ".<br>";
                                Mail_Body = Mail_Body + "<b>Requisition Detail</b>" + "<br>" +
                                    "Demand: " + currentTrip.TrackingID + "<br>" +
                                    "Requistion: " + currentTrip.TrackingID + "<br>" +
                                    "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + " " +
                                    currentTrip.Vehicle.OWN_MHT_DHT + " " +
                                    currentTrip.Vehicle.VehicleType + "<br>" +

                                    "Driver: " + (string.IsNullOrEmpty(currentTrip.Driver_Staff_ID) ? "" : currentTrip.Driver_Staff_ID) + " " +
                                    (string.IsNullOrEmpty(currentTrip.Driver_Name) ? "" : currentTrip.Driver_Name) + " " +
                                    (string.IsNullOrEmpty(currentTrip.Driver_ContactNumber) ? "" : currentTrip.Driver_ContactNumber) + "<br>" +

                                    "Total Amount: " + (currentTrip.TotalAmount != null ? "" : currentTrip.TotalAmount + " TK") + " " + "<br>" +
                                    "From: " + currentTrip.Requisition.Location.Name + " " + currentTrip.Requisition.StartingLocation + "<br>" +
                                    "To: " + currentTrip.Requisition.Location1.Name + " " + currentTrip.Requisition.FinishingLocation + "<br>" +
                                    "Finished At: " + currentTrip.FinishedAt + "<br>";
                                SendMail_Single(currentTrip.AppUser.Email, Mail_Subject, Mail_Body);
                            }
                            catch (Exception e)
                            {
                            }
                        }

                        //# Notify Raiser Mail
                        if (!string.IsNullOrEmpty(currentTrip.Requisition.AppUser.Email))
                        {
                            try
                            {
                                var Mail_Subject = "Requisition " + currentTrip.StatusText + " " + currentTrip.TrackingID;
                                var Mail_Body = "Dear Concern, The below requisition is " + currentTrip.StatusText + ".<br>";
                                Mail_Body = Mail_Body + "<b>Requisition Detail</b>" + "<br>" +
                                    "Demand: " + currentTrip.TrackingID + "<br>" +
                                    "Requistion: " + currentTrip.TrackingID + "<br>" +
                                    "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + " " +
                                    currentTrip.Vehicle.OWN_MHT_DHT + " " +
                                    currentTrip.Vehicle.VehicleType + "<br>" +

                                    "Driver: " + (string.IsNullOrEmpty(currentTrip.Driver_Staff_ID) ? "" : currentTrip.Driver_Staff_ID) + " " +
                                    (string.IsNullOrEmpty(currentTrip.Driver_Name) ? "" : currentTrip.Driver_Name) + " " +
                                    (string.IsNullOrEmpty(currentTrip.Driver_ContactNumber) ? "" : currentTrip.Driver_ContactNumber) + "<br>" +

                                    "Total Amount: " + (currentTrip.TotalAmount != null ? "" : currentTrip.TotalAmount + " TK") + " " + "<br>" +
                                    "From: " + currentTrip.Requisition.Location.Name + " " + currentTrip.Requisition.StartingLocation + "<br>" +
                                    "To: " + currentTrip.Requisition.Location1.Name + " " + currentTrip.Requisition.FinishingLocation + "<br>" +
                                    "Finished At: " + currentTrip.FinishedAt + "<br>";
                                SendMail_Single(currentTrip.Requisition.AppUser.Email, Mail_Subject, Mail_Body);
                            }
                            catch (Exception e)
                            {
                            }
                        }
                        bll.db.SaveChanges();

                        //#Check pending trip for loading
                        //var pendingTrip = bll.db.RequisitionTrips.Where(m => m.PK_RequisitionTrip == currentTrip.Vehicle.FK_RequisitionTrip_Pending).FirstOrDefault();
                        //if (pendingTrip != null)
                        //{
                        //    //-check no current trip and vehicle is inside of pending trip's starting location
                        //    if (pendingTrip.Requisition.FK_Depo_From == pendingTrip.Vehicle.FK_DepoInOut && pendingTrip.Vehicle.LocationInOrOut == true)
                        //    {
                        //        if (pendingTrip.Requisition.LoadedOrEmpty == true)
                        //        {
                        //            pendingTrip.StatusText = InternalTripStatus.StartedLoading;
                        //            pendingTrip.LoadingStartDateTime = DateTime.Now;
                        //            // Update Vehicle Trip
                        //            pendingTrip.Vehicle.FK_RequisitionTrip_Current = pendingTrip.Vehicle.FK_RequisitionTrip_Pending;
                        //            pendingTrip.Vehicle.FK_RequisitionTrip_Pending = null;

                        //            // Update Driver Trip
                        //            pendingTrip.AppUser4.FK_RequisitionTrip_Current = pendingTrip.AppUser4.FK_RequisitionTrip_Pending;
                        //            pendingTrip.AppUser4.FK_RequisitionTrip_Pending = null;

                        //            //# Notify Driver Firebase
                        //            //var vehicle = bll.db.Vehicles.Where(m => m.PK_Vehicle == currentTrip.FK_Vehicle).FirstOrDefault();
                        //            if (!string.IsNullOrEmpty(pendingTrip.Vehicle.FID))
                        //            {
                        //                var _FK_Depo_To = currentTrip.Requisition.VehicleSharingDemands.Where(n => n.IsHeadDemand == true).FirstOrDefault().FK_Depo_To;
                        //                //var title = "Driver: Your Vehicle Entered in Current Trip #" + currentTrip.Requisition.TrackingID + " Destination: " + bll.db.Locations.Where(m => m.PK_Location == _FK_Depo_To).FirstOrDefault().Name;
                        //                var title = "সম্মানিত চালক, গাড়ি আসন্ন যাত্রার (#" + currentTrip.Requisition.TrackingID + ")  লোডিং-এর সময় গণনা শুরু হয়েছে।";
                        //                message = "Dear Concern \n";
                        //                message = message + "<b>Requisition Detail</b>" + "\n"
                        //                    + "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + "\n"
                        //                    + "From: " + currentTrip.Requisition.Location.Name + "\n"
                        //                    + "To: " + currentTrip.Requisition.Location1.Name + "\n"
                        //                    + "At: " + currentTrip.Requisition.PossibleJourneyStartDateTime + "\n";
                        //                SendFCM_Notification_Single_New(pendingTrip.Vehicle.FID, title, message, currentTrip.PK_RequisitionTrip.ToString(), "RequisitionTrip");
                        //            }
                        //            //# Notify Assigner Mail
                        //            if (!string.IsNullOrEmpty(pendingTrip.AppUser.Email))
                        //            {
                        //                var Mail_Subject = "Assigner: Internal trip is started";
                        //                var Mail_Body = "Dear Concern<br>";
                        //                Mail_Body = Mail_Body + "<b>Requisition Detail</b>" + " <br>"
                        //                    + "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + "<br>"
                        //                    + "From: " + pendingTrip.Requisition.Location.Name + "<br>"
                        //                    + "To: " + pendingTrip.Requisition.Location1.Name + "<br>"
                        //                    + "At: " + pendingTrip.Requisition.PossibleJourneyStartDateTime + "<br>" + "\n";
                        //                SendMail_Single(pendingTrip.AppUser.Email, Mail_Subject, Mail_Body);
                        //            }
                        //            //# Notify Raiser Mail
                        //            foreach (var _demand in pendingTrip.Requisition.VehicleSharingDemands)
                        //            {
                        //                if (!string.IsNullOrEmpty(_demand.AppUser.Email))
                        //                {
                        //                    var Mail_Subject = "Raiser: Internal trip is started";
                        //                    var Mail_Body = "Dear Concern<br>";
                        //                    Mail_Body = Mail_Body + "<b>Requisition Detail</b>" + " <br>"
                        //                        + "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + "<br>"
                        //                        + "From: " + pendingTrip.Requisition.Location.Name + "<br>"
                        //                        + "To: " + pendingTrip.Requisition.Location1.Name + "<br>"
                        //                        + "At: " + pendingTrip.Requisition.PossibleJourneyStartDateTime + "<br>" + "\n";
                        //                    SendMail_Single(_demand.AppUser.Email, Mail_Subject, Mail_Body);
                        //                }
                        //            }
                        //            bll.db.SaveChanges();
                        //        }
                        //        else
                        //        {
                        //            pendingTrip.StatusText = InternalTripStatus.StartedEmptyTrip;
                        //            pendingTrip.LoadingStartDateTime = DateTime.Now;
                        //            pendingTrip.LoadingDoneDateTime = pendingTrip.LoadingStartDateTime;
                        //            // Update Vehicle Trip
                        //            pendingTrip.Vehicle.FK_RequisitionTrip_Current = pendingTrip.Vehicle.FK_RequisitionTrip_Pending;
                        //            pendingTrip.Vehicle.FK_RequisitionTrip_Pending = null;

                        //            // Update Driver Trip
                        //            pendingTrip.AppUser4.FK_RequisitionTrip_Current = pendingTrip.AppUser4.FK_RequisitionTrip_Pending;
                        //            pendingTrip.AppUser4.FK_RequisitionTrip_Pending = null;

                        //            //# Notify Driver Firebase
                        //            //var vehicle = bll.db.Vehicles.Where(m => m.PK_Vehicle == currentTrip.FK_Vehicle).FirstOrDefault();
                        //            if (!string.IsNullOrEmpty(pendingTrip.Vehicle.FID))
                        //            {
                        //                var _FK_Depo_To = currentTrip.Requisition.VehicleSharingDemands.Where(n => n.IsHeadDemand == true).FirstOrDefault().FK_Depo_To;
                        //                //var title = "Driver: Your Vehicle Entered in Current Trip #" + currentTrip.Requisition.TrackingID + " Destination: " + bll.db.Locations.Where(m => m.PK_Location == _FK_Depo_To).FirstOrDefault().Name;
                        //                var title = "সম্মানিত চালক, গাড়ি আসন্ন খালি যাত্রার #" + pendingTrip.Requisition.TrackingID + " শুরুর স্থানে (" + bll.db.Locations.Where(m => m.PK_Location == _FK_Depo_To).FirstOrDefault().Name + ") প্রবেশ করেছে, যাত্রার সময় গণনা শুরু হয়েছে।";
                        //                message = "Dear Concern \n";
                        //                message = message + "<b>Requisition Detail</b>" + "\n"
                        //                    + "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + "\n"
                        //                    + "From: " + currentTrip.Requisition.Location.Name + "\n"
                        //                    + "To: " + currentTrip.Requisition.Location1.Name + "\n"
                        //                    + "At: " + currentTrip.Requisition.PossibleJourneyStartDateTime + "\n";
                        //                SendFCM_Notification_Single_New(pendingTrip.Vehicle.FID, title, message, currentTrip.PK_RequisitionTrip.ToString(), "RequisitionTrip");
                        //            }
                        //            //# Notify Assigner Mail
                        //            if (!string.IsNullOrEmpty(pendingTrip.AppUser.Email))
                        //            {
                        //                var Mail_Subject = "Assigner: Internal empty trip started";
                        //                var Mail_Body = "Dear Concern<br>";
                        //                Mail_Body = Mail_Body + "<b>Requisition Detail</b>" + " <br>"
                        //                    + "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + "<br>"
                        //                    + "From: " + pendingTrip.Requisition.Location.Name + "<br>"
                        //                    + "To: " + pendingTrip.Requisition.Location1.Name + "<br>"
                        //                    + "At: " + pendingTrip.Requisition.PossibleJourneyStartDateTime + "<br>" + "\n";
                        //                SendMail_Single(pendingTrip.AppUser.Email, Mail_Subject, Mail_Body);
                        //            }
                        //            //# Notify Raiser Mail
                        //            foreach (var _demand in pendingTrip.Requisition.VehicleSharingDemands)
                        //            {
                        //                if (!string.IsNullOrEmpty(_demand.AppUser.Email))
                        //                {
                        //                    var Mail_Subject = "Raiser: Internal empty trip started";
                        //                    var Mail_Body = "Dear Concern<br>";
                        //                    Mail_Body = Mail_Body + "<b>Requisition Detail</b>" + " <br>"
                        //                        + "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + "<br>"
                        //                        + "From: " + pendingTrip.Requisition.Location.Name + "<br>"
                        //                        + "To: " + pendingTrip.Requisition.Location1.Name + "<br>"
                        //                        + "At: " + pendingTrip.Requisition.PossibleJourneyStartDateTime + "<br>" + "\n";
                        //                    SendMail_Single(_demand.AppUser.Email, Mail_Subject, Mail_Body);
                        //                }
                        //            }
                        //            bll.db.SaveChanges();
                        //        }
                        //    }
                        //}
                        //return "YES";
                        CreateAlertMessage(AlertMessageType.Success, "Success", "Requisition finished successfully");
                        return RedirectToAction("RequisitionTripIndex_Assigner");
                    }
                    else if (currentTrip.StatusText != InternalTripStatus.Started)
                    {
                        CreateAlertMessage(AlertMessageType.Danger, "Danger", "Requisition can not be finished. Current status: " + currentTrip.StatusText);
                        return RedirectToAction("RequisitionTripIndex_Assigner");
                    }
                    else
                    {
                        message = "Undefined Reasson : FinishTrip 1.1";
                        //return message;
                        CreateAlertMessage(AlertMessageType.Danger, "Danger", message);
                        return RedirectToAction("RequisitionTripIndex_Assigner");
                    }
                }
                else
                {
                    message = "যাত্রার তথ্য খুজে পাওয়া যায়নি।";
                    //return message;
                    CreateAlertMessage(AlertMessageType.Danger, "Danger", message);
                    return RedirectToAction("RequisitionTripIndex_Assigner");
                }
            }
            catch (Exception e)
            {
                var errrorMessage = "";
                do { errrorMessage = "#" + errrorMessage + e.Message; if (e.InnerException != null) { e = e.InnerException; } else { break; } } while (true);
                //return errrorMessage;
                CreateAlertMessage(AlertMessageType.Warning, "Danger", errrorMessage);
                return RedirectToAction("RequisitionTripIndex_Assigner");
            }
        }
        public ActionResult RequisitionTrip_Finish_Client(Int64 id, string FinishAutoOrManaul, Guid FK_AppUser_Finish)
        {
            try
            {
                var currentTrip = bll.db.RequisitionTrips.Where(m => m.IsDeleted != true && m.Requisition.IsDeleted != true && m.PK_RequisitionTrip == id).FirstOrDefault();

                var now = DateTime.Now;
                var message = "";

                if (currentTrip != null)
                {

                    if (currentTrip.StatusText == InternalTripStatus.Started /*&& (currentTrip.Requisition.FK_Depo_To == currentTrip.Vehicle.FK_DepoInOut && currentTrip.Vehicle.LocationInOrOut == true)*/)
                    {
                        currentTrip.StatusText = InternalTripStatus.Finished;
                        currentTrip.FinishAutoOrManaul = FinishAutoOrManaul;
                        currentTrip.FK_LocationGate_Finish = FK_AppUser_Finish;
                        currentTrip.FinishedAt = DateTime.Now;
                        if (currentTrip.Vehicle.FK_RequisitionTrip_Last == currentTrip.PK_RequisitionTrip)
                        {
                            currentTrip.Vehicle.FK_RequisitionTrip_Last = null;
                        }

                        // Update Vehicle Trip
                        //currentTrip.Vehicle.FK_RequisitionTrip_Current = null;

                        // Update Driver Trip
                        //currentTrip.AppUser4.FK_RequisitionTrip_Current = null;

                        //# Notify Assigner Mail
                        if (!string.IsNullOrEmpty(currentTrip.AppUser.Email))
                        {
                            try
                            {
                                var Mail_Subject = "Requisition " + currentTrip.StatusText + " " + currentTrip.TrackingID;
                                var Mail_Body = "Dear Concern, The below requisition is " + currentTrip.StatusText + ".<br>";
                                Mail_Body = Mail_Body + "<b>Requisition Detail</b>" + "<br>" +
                                    "Demand: " + currentTrip.TrackingID + "<br>" +
                                    "Requistion: " + currentTrip.TrackingID + "<br>" +
                                    "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + " " +
                                    currentTrip.Vehicle.OWN_MHT_DHT + " " +
                                    currentTrip.Vehicle.VehicleType + "<br>" +

                                    "Driver: " + (string.IsNullOrEmpty(currentTrip.Driver_Staff_ID) ? "" : currentTrip.Driver_Staff_ID) + " " +
                                    (string.IsNullOrEmpty(currentTrip.Driver_Name) ? "" : currentTrip.Driver_Name) + " " +
                                    (string.IsNullOrEmpty(currentTrip.Driver_ContactNumber) ? "" : currentTrip.Driver_ContactNumber) + "<br>" +

                                    "Total Amount: " + (currentTrip.TotalAmount != null ? "" : currentTrip.TotalAmount + " TK") + " " + "<br>" +
                                    "From: " + currentTrip.Requisition.Location.Name + " " + currentTrip.Requisition.StartingLocation + "<br>" +
                                    "To: " + currentTrip.Requisition.Location1.Name + " " + currentTrip.Requisition.FinishingLocation + "<br>" +
                                    "Finished At: " + currentTrip.FinishedAt + "<br>";
                                SendMail_Single(currentTrip.AppUser.Email, Mail_Subject, Mail_Body);
                            }
                            catch (Exception e)
                            {
                            }
                        }

                        //# Notify Raiser Mail
                        if (!string.IsNullOrEmpty(currentTrip.Requisition.AppUser.Email))
                        {
                            try
                            {
                                var Mail_Subject = "Requisition " + currentTrip.StatusText + " " + currentTrip.TrackingID;
                                var Mail_Body = "Dear Concern, The below requisition is " + currentTrip.StatusText + ".<br>";
                                Mail_Body = Mail_Body + "<b>Requisition Detail</b>" + "<br>" +
                                    "Demand: " + currentTrip.TrackingID + "<br>" +
                                    "Requistion: " + currentTrip.TrackingID + "<br>" +
                                    "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + " " +
                                    currentTrip.Vehicle.OWN_MHT_DHT + " " +
                                    currentTrip.Vehicle.VehicleType + "<br>" +

                                    "Driver: " + (string.IsNullOrEmpty(currentTrip.Driver_Staff_ID) ? "" : currentTrip.Driver_Staff_ID) + " " +
                                    (string.IsNullOrEmpty(currentTrip.Driver_Name) ? "" : currentTrip.Driver_Name) + " " +
                                    (string.IsNullOrEmpty(currentTrip.Driver_ContactNumber) ? "" : currentTrip.Driver_ContactNumber) + "<br>" +

                                    "Total Amount: " + (currentTrip.TotalAmount != null ? "" : currentTrip.TotalAmount + " TK") + " " + "<br>" +
                                    "From: " + currentTrip.Requisition.Location.Name + " " + currentTrip.Requisition.StartingLocation + "<br>" +
                                    "To: " + currentTrip.Requisition.Location1.Name + " " + currentTrip.Requisition.FinishingLocation + "<br>" +
                                    "Finished At: " + currentTrip.FinishedAt + "<br>";
                                SendMail_Single(currentTrip.Requisition.AppUser.Email, Mail_Subject, Mail_Body);
                            }
                            catch (Exception e)
                            {
                            }
                        }
                        bll.db.SaveChanges();

                        //#Check pending trip for loading
                        //var pendingTrip = bll.db.RequisitionTrips.Where(m => m.PK_RequisitionTrip == currentTrip.Vehicle.FK_RequisitionTrip_Pending).FirstOrDefault();
                        //if (pendingTrip != null)
                        //{
                        //    //-check no current trip and vehicle is inside of pending trip's starting location
                        //    if (pendingTrip.Requisition.FK_Depo_From == pendingTrip.Vehicle.FK_DepoInOut && pendingTrip.Vehicle.LocationInOrOut == true)
                        //    {
                        //        if (pendingTrip.Requisition.LoadedOrEmpty == true)
                        //        {
                        //            pendingTrip.StatusText = InternalTripStatus.StartedLoading;
                        //            pendingTrip.LoadingStartDateTime = DateTime.Now;
                        //            // Update Vehicle Trip
                        //            pendingTrip.Vehicle.FK_RequisitionTrip_Current = pendingTrip.Vehicle.FK_RequisitionTrip_Pending;
                        //            pendingTrip.Vehicle.FK_RequisitionTrip_Pending = null;

                        //            // Update Driver Trip
                        //            pendingTrip.AppUser4.FK_RequisitionTrip_Current = pendingTrip.AppUser4.FK_RequisitionTrip_Pending;
                        //            pendingTrip.AppUser4.FK_RequisitionTrip_Pending = null;

                        //            //# Notify Driver Firebase
                        //            //var vehicle = bll.db.Vehicles.Where(m => m.PK_Vehicle == currentTrip.FK_Vehicle).FirstOrDefault();
                        //            if (!string.IsNullOrEmpty(pendingTrip.Vehicle.FID))
                        //            {
                        //                var _FK_Depo_To = currentTrip.Requisition.VehicleSharingDemands.Where(n => n.IsHeadDemand == true).FirstOrDefault().FK_Depo_To;
                        //                //var title = "Driver: Your Vehicle Entered in Current Trip #" + currentTrip.Requisition.TrackingID + " Destination: " + bll.db.Locations.Where(m => m.PK_Location == _FK_Depo_To).FirstOrDefault().Name;
                        //                var title = "সম্মানিত চালক, গাড়ি আসন্ন যাত্রার (#" + currentTrip.Requisition.TrackingID + ")  লোডিং-এর সময় গণনা শুরু হয়েছে।";
                        //                message = "Dear Concern \n";
                        //                message = message + "<b>Requisition Detail</b>" + "\n"
                        //                    + "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + "\n"
                        //                    + "From: " + currentTrip.Requisition.Location.Name + "\n"
                        //                    + "To: " + currentTrip.Requisition.Location1.Name + "\n"
                        //                    + "At: " + currentTrip.Requisition.PossibleJourneyStartDateTime + "\n";
                        //                SendFCM_Notification_Single_New(pendingTrip.Vehicle.FID, title, message, currentTrip.PK_RequisitionTrip.ToString(), "RequisitionTrip");
                        //            }
                        //            //# Notify Assigner Mail
                        //            if (!string.IsNullOrEmpty(pendingTrip.AppUser.Email))
                        //            {
                        //                var Mail_Subject = "Assigner: Internal trip is started";
                        //                var Mail_Body = "Dear Concern<br>";
                        //                Mail_Body = Mail_Body + "<b>Requisition Detail</b>" + " <br>"
                        //                    + "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + "<br>"
                        //                    + "From: " + pendingTrip.Requisition.Location.Name + "<br>"
                        //                    + "To: " + pendingTrip.Requisition.Location1.Name + "<br>"
                        //                    + "At: " + pendingTrip.Requisition.PossibleJourneyStartDateTime + "<br>" + "\n";
                        //                SendMail_Single(pendingTrip.AppUser.Email, Mail_Subject, Mail_Body);
                        //            }
                        //            //# Notify Raiser Mail
                        //            foreach (var _demand in pendingTrip.Requisition.VehicleSharingDemands)
                        //            {
                        //                if (!string.IsNullOrEmpty(_demand.AppUser.Email))
                        //                {
                        //                    var Mail_Subject = "Raiser: Internal trip is started";
                        //                    var Mail_Body = "Dear Concern<br>";
                        //                    Mail_Body = Mail_Body + "<b>Requisition Detail</b>" + " <br>"
                        //                        + "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + "<br>"
                        //                        + "From: " + pendingTrip.Requisition.Location.Name + "<br>"
                        //                        + "To: " + pendingTrip.Requisition.Location1.Name + "<br>"
                        //                        + "At: " + pendingTrip.Requisition.PossibleJourneyStartDateTime + "<br>" + "\n";
                        //                    SendMail_Single(_demand.AppUser.Email, Mail_Subject, Mail_Body);
                        //                }
                        //            }
                        //            bll.db.SaveChanges();
                        //        }
                        //        else
                        //        {
                        //            pendingTrip.StatusText = InternalTripStatus.StartedEmptyTrip;
                        //            pendingTrip.LoadingStartDateTime = DateTime.Now;
                        //            pendingTrip.LoadingDoneDateTime = pendingTrip.LoadingStartDateTime;
                        //            // Update Vehicle Trip
                        //            pendingTrip.Vehicle.FK_RequisitionTrip_Current = pendingTrip.Vehicle.FK_RequisitionTrip_Pending;
                        //            pendingTrip.Vehicle.FK_RequisitionTrip_Pending = null;

                        //            // Update Driver Trip
                        //            pendingTrip.AppUser4.FK_RequisitionTrip_Current = pendingTrip.AppUser4.FK_RequisitionTrip_Pending;
                        //            pendingTrip.AppUser4.FK_RequisitionTrip_Pending = null;

                        //            //# Notify Driver Firebase
                        //            //var vehicle = bll.db.Vehicles.Where(m => m.PK_Vehicle == currentTrip.FK_Vehicle).FirstOrDefault();
                        //            if (!string.IsNullOrEmpty(pendingTrip.Vehicle.FID))
                        //            {
                        //                var _FK_Depo_To = currentTrip.Requisition.VehicleSharingDemands.Where(n => n.IsHeadDemand == true).FirstOrDefault().FK_Depo_To;
                        //                //var title = "Driver: Your Vehicle Entered in Current Trip #" + currentTrip.Requisition.TrackingID + " Destination: " + bll.db.Locations.Where(m => m.PK_Location == _FK_Depo_To).FirstOrDefault().Name;
                        //                var title = "সম্মানিত চালক, গাড়ি আসন্ন খালি যাত্রার #" + pendingTrip.Requisition.TrackingID + " শুরুর স্থানে (" + bll.db.Locations.Where(m => m.PK_Location == _FK_Depo_To).FirstOrDefault().Name + ") প্রবেশ করেছে, যাত্রার সময় গণনা শুরু হয়েছে।";
                        //                message = "Dear Concern \n";
                        //                message = message + "<b>Requisition Detail</b>" + "\n"
                        //                    + "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + "\n"
                        //                    + "From: " + currentTrip.Requisition.Location.Name + "\n"
                        //                    + "To: " + currentTrip.Requisition.Location1.Name + "\n"
                        //                    + "At: " + currentTrip.Requisition.PossibleJourneyStartDateTime + "\n";
                        //                SendFCM_Notification_Single_New(pendingTrip.Vehicle.FID, title, message, currentTrip.PK_RequisitionTrip.ToString(), "RequisitionTrip");
                        //            }
                        //            //# Notify Assigner Mail
                        //            if (!string.IsNullOrEmpty(pendingTrip.AppUser.Email))
                        //            {
                        //                var Mail_Subject = "Assigner: Internal empty trip started";
                        //                var Mail_Body = "Dear Concern<br>";
                        //                Mail_Body = Mail_Body + "<b>Requisition Detail</b>" + " <br>"
                        //                    + "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + "<br>"
                        //                    + "From: " + pendingTrip.Requisition.Location.Name + "<br>"
                        //                    + "To: " + pendingTrip.Requisition.Location1.Name + "<br>"
                        //                    + "At: " + pendingTrip.Requisition.PossibleJourneyStartDateTime + "<br>" + "\n";
                        //                SendMail_Single(pendingTrip.AppUser.Email, Mail_Subject, Mail_Body);
                        //            }
                        //            //# Notify Raiser Mail
                        //            foreach (var _demand in pendingTrip.Requisition.VehicleSharingDemands)
                        //            {
                        //                if (!string.IsNullOrEmpty(_demand.AppUser.Email))
                        //                {
                        //                    var Mail_Subject = "Raiser: Internal empty trip started";
                        //                    var Mail_Body = "Dear Concern<br>";
                        //                    Mail_Body = Mail_Body + "<b>Requisition Detail</b>" + " <br>"
                        //                        + "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + "<br>"
                        //                        + "From: " + pendingTrip.Requisition.Location.Name + "<br>"
                        //                        + "To: " + pendingTrip.Requisition.Location1.Name + "<br>"
                        //                        + "At: " + pendingTrip.Requisition.PossibleJourneyStartDateTime + "<br>" + "\n";
                        //                    SendMail_Single(_demand.AppUser.Email, Mail_Subject, Mail_Body);
                        //                }
                        //            }
                        //            bll.db.SaveChanges();
                        //        }
                        //    }
                        //}
                        //return "YES";
                        CreateAlertMessage(AlertMessageType.Success, "Success", "Requisition finished successfully");
                        return RedirectToAction("RequisitionTripIndex_Client");
                    }
                    else if (currentTrip.StatusText != InternalTripStatus.Started)
                    {
                        CreateAlertMessage(AlertMessageType.Danger, "Danger", "Requisition can not be finished. Current status: " + currentTrip.StatusText);
                        return RedirectToAction("RequisitionTripIndex_Client");
                    }
                    else
                    {
                        message = "Undefined Reasson : FinishTrip 1.2";
                        //return message;
                        CreateAlertMessage(AlertMessageType.Danger, "Danger", message);
                        return RedirectToAction("RequisitionTripIndex_Client");
                    }
                }
                else
                {
                    message = "যাত্রার তথ্য খুজে পাওয়া যায়নি।";
                    //return message;
                    CreateAlertMessage(AlertMessageType.Danger, "Danger", message);
                    return RedirectToAction("RequisitionTripIndex_Client");
                }
            }
            catch (Exception e)
            {
                var errrorMessage = "";
                do { errrorMessage = "#" + errrorMessage + e.Message; if (e.InnerException != null) { e = e.InnerException; } else { break; } } while (true);
                //return errrorMessage;
                CreateAlertMessage(AlertMessageType.Warning, "Danger", errrorMessage);
                return RedirectToAction("RequisitionTripIndex_Client");
            }
        }
        public void RequisitionTrip_StartMulti(Guid FK_Vehicle, Guid FK_Location, Guid FK_AppUser)
        {
            var now = DateTime.Now;
            var pendingTripList = bll.db.RequisitionTrips.Where(m => m.IsDeleted != true && m.Requisition.IsDeleted != true && m.FK_Vehicle == FK_Vehicle && m.StatusText == InternalTripStatus.Assigned
            && m.Requisition.FK_Location_From == FK_Location && m.FinalWantedAtDateTime < now).ToList();
            foreach (var pendingTrip in pendingTripList)
            {
                pendingTrip.StatusText = InternalTripStatus.Started;
                pendingTrip.StartAutoOrManaul = "Auto";
                pendingTrip.FK_LocationGate_Start = FK_AppUser;
                pendingTrip.StartedAt = DateTime.Now;
                pendingTrip.Vehicle.FK_RequisitionTrip_Last = pendingTrip.PK_RequisitionTrip;

                var locationToLocationMapping = bll.db.LocationToLocationMappings.Where(m => (m.FK_Location_1 == pendingTrip.Requisition.Location.PK_Location && m.FK_Location_2 == pendingTrip.Requisition.Location1.PK_Location)
                        || (m.FK_Location_2 == pendingTrip.Requisition.Location.PK_Location && m.FK_Location_1 == pendingTrip.Requisition.Location1.PK_Location)
                        ).FirstOrDefault();
                if (locationToLocationMapping != null && locationToLocationMapping.StandardTravelTimeMinute != null)
                {
                    pendingTrip.TentativeFinishingDateTime = (pendingTrip.StartedAt ?? DateTime.Now).AddMinutes(locationToLocationMapping.StandardTravelTimeMinute ?? 0);
                }

                //# Notify Assigner Mail
                if (!string.IsNullOrEmpty(pendingTrip.AppUser.Email))
                {
                    try
                    {
                        var Mail_Subject = "Requisition " + pendingTrip.StatusText + " " + pendingTrip.TrackingID;
                        var Mail_Body = "Dear Concern, The below requisition is " + pendingTrip.StatusText + ".<br>";
                        Mail_Body = Mail_Body + "<b>Requisition Detail</b>" + "<br>" +
                            "Demand: " + pendingTrip.TrackingID + "<br>" +
                            "Requistion: " + pendingTrip.TrackingID + "<br>" +
                            "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + " " +
                            pendingTrip.Vehicle.OWN_MHT_DHT + " " +
                            pendingTrip.Vehicle.VehicleType + "<br>" +

                            "Driver: " + (string.IsNullOrEmpty(pendingTrip.Driver_Staff_ID) ? "" : pendingTrip.Driver_Staff_ID) + " " +
                            (string.IsNullOrEmpty(pendingTrip.Driver_Name) ? "" : pendingTrip.Driver_Name) + " " +
                            (string.IsNullOrEmpty(pendingTrip.Driver_ContactNumber) ? "" : pendingTrip.Driver_ContactNumber) + "<br>" +

                            "Total Amount: " + (pendingTrip.TotalAmount != null ? "" : pendingTrip.TotalAmount + " TK") + " " + "<br>" +
                            "From: " + pendingTrip.Requisition.Location.Name + " " + pendingTrip.Requisition.StartingLocation + "<br>" +
                            "To: " + pendingTrip.Requisition.Location1.Name + " " + pendingTrip.Requisition.FinishingLocation + "<br>" +
                            "Started At: " + pendingTrip.StartedAt + "<br>";
                        SendMail_Single(pendingTrip.AppUser.Email, Mail_Subject, Mail_Body);
                    }
                    catch (Exception e)
                    {
                    }
                }

                //# Notify Raiser Mail
                if (!string.IsNullOrEmpty(pendingTrip.Requisition.AppUser.Email))
                {
                    try
                    {
                        var Mail_Subject = "Requisition " + pendingTrip.StatusText + " " + pendingTrip.TrackingID;
                        var Mail_Body = "Dear Concern, The below requisition is " + pendingTrip.StatusText + ".<br>";
                        Mail_Body = Mail_Body + "<b>Requisition Detail</b>" + "<br>" +
                            "Demand: " + pendingTrip.TrackingID + "<br>" +
                            "Requistion: " + pendingTrip.TrackingID + "<br>" +
                            "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + " " +
                            pendingTrip.Vehicle.OWN_MHT_DHT + " " +
                            pendingTrip.Vehicle.VehicleType + "<br>" +

                            "Driver: " + (string.IsNullOrEmpty(pendingTrip.Driver_Staff_ID) ? "" : pendingTrip.Driver_Staff_ID) + " " +
                            (string.IsNullOrEmpty(pendingTrip.Driver_Name) ? "" : pendingTrip.Driver_Name) + " " +
                            (string.IsNullOrEmpty(pendingTrip.Driver_ContactNumber) ? "" : pendingTrip.Driver_ContactNumber) + "<br>" +

                            "Total Amount: " + (pendingTrip.TotalAmount != null ? "" : pendingTrip.TotalAmount + " TK") + " " + "<br>" +
                            "From: " + pendingTrip.Requisition.Location.Name + " " + pendingTrip.Requisition.StartingLocation + "<br>" +
                            "To: " + pendingTrip.Requisition.Location1.Name + " " + pendingTrip.Requisition.FinishingLocation + "<br>" +
                            "Started At: " + pendingTrip.StartedAt + "<br>";
                        SendMail_Single(pendingTrip.Requisition.AppUser.Email, Mail_Subject, Mail_Body);
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
            bll.db.SaveChanges();

            if (pendingTripList.Count != 0)
            {
                var _pendingTrip = pendingTripList.FirstOrDefault();
                if (!bll.db.RequisitionTrips.Where(m => m.FK_Vehicle == _pendingTrip.FK_Vehicle && m.StatusText == InternalTripStatus.Assigned).Any())
                {
                    _pendingTrip.Vehicle.FK_RequisitionTrip_CurrentAssigner = null;
                    bll.db.SaveChanges();
                }
            }
        }
        public void RequisitionTrip_FinishMulti(Guid FK_Vehicle, Guid FK_Location, Guid FK_AppUser_Gate)
        {
            var enteredLocation = bll.db.Locations.Where(m => m.PK_Location == FK_Location).FirstOrDefault();
            var currentTripList = bll.db.RequisitionTrips.Where(m => m.IsDeleted != true
            && m.Requisition.IsDeleted != true
            && m.FK_Vehicle == FK_Vehicle
            //&& m.Requisition.FK_Location_To == FK_Location //# restrict location check as if all running(Statustext=Started) trip can be selected
            && m.StatusText == InternalTripStatus.Started).ToList();
            foreach (var currentTrip in currentTripList)
            {
                currentTrip.StatusText = InternalTripStatus.Finished;
                currentTrip.FinishAutoOrManaul = "Auto";
                currentTrip.FK_LocationGate_Finish = FK_AppUser_Gate;
                currentTrip.FinishedAt = DateTime.Now;
                if (currentTrip.Vehicle.FK_RequisitionTrip_Last == currentTrip.PK_RequisitionTrip)
                {
                    currentTrip.Vehicle.FK_RequisitionTrip_Last = null;
                }

                //# Notify Assigner Mail
                if (!string.IsNullOrEmpty(currentTrip.AppUser.Email))
                {
                    try
                    {
                        var Mail_Subject = "Requisition " + currentTrip.StatusText + " " + currentTrip.TrackingID;
                        var Mail_Body = "Dear Concern, The below requisition is " + currentTrip.StatusText + ".<br>";
                        Mail_Body = Mail_Body + "<b>Requisition Detail</b>" + "<br>" +
                            "Demand: " + currentTrip.TrackingID + "<br>" +
                            "Requistion: " + currentTrip.TrackingID + "<br>" +
                            "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + " " +
                            currentTrip.Vehicle.OWN_MHT_DHT + " " +
                            currentTrip.Vehicle.VehicleType + "<br>" +

                            "Driver: " + (string.IsNullOrEmpty(currentTrip.Driver_Staff_ID) ? "" : currentTrip.Driver_Staff_ID) + " " +
                            (string.IsNullOrEmpty(currentTrip.Driver_Name) ? "" : currentTrip.Driver_Name) + " " +
                            (string.IsNullOrEmpty(currentTrip.Driver_ContactNumber) ? "" : currentTrip.Driver_ContactNumber) + "<br>" +

                            "Total Amount: " + (currentTrip.TotalAmount != null ? "" : currentTrip.TotalAmount + " TK") + " " + "<br>" +
                            "From: " + currentTrip.Requisition.Location.Name + " " + currentTrip.Requisition.StartingLocation + "<br>" +
                            "To: " + currentTrip.Requisition.Location1.Name + " " + currentTrip.Requisition.FinishingLocation + "<br>" +
                            "Finished At: " + currentTrip.FinishedAt + "<br>";
                        SendMail_Single(currentTrip.AppUser.Email, Mail_Subject, Mail_Body);
                    }
                    catch (Exception e)
                    {
                    }
                }

                //# Notify Raiser Mail
                if (!string.IsNullOrEmpty(currentTrip.Requisition.AppUser.Email))
                {
                    try
                    {
                        var Mail_Subject = "Requisition " + currentTrip.StatusText + " " + currentTrip.TrackingID;
                        var Mail_Body = "Dear Concern, The below requisition is " + currentTrip.StatusText + ".<br>";
                        Mail_Body = Mail_Body + "<b>Requisition Detail</b>" + "<br>" +
                            "Demand: " + currentTrip.TrackingID + "<br>" +
                            "Requistion: " + currentTrip.TrackingID + "<br>" +
                            "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + " " +
                            currentTrip.Vehicle.OWN_MHT_DHT + " " +
                            currentTrip.Vehicle.VehicleType + "<br>" +

                            "Driver: " + (string.IsNullOrEmpty(currentTrip.Driver_Staff_ID) ? "" : currentTrip.Driver_Staff_ID) + " " +
                            (string.IsNullOrEmpty(currentTrip.Driver_Name) ? "" : currentTrip.Driver_Name) + " " +
                            (string.IsNullOrEmpty(currentTrip.Driver_ContactNumber) ? "" : currentTrip.Driver_ContactNumber) + "<br>" +

                            "Total Amount: " + (currentTrip.TotalAmount != null ? "" : currentTrip.TotalAmount + " TK") + " " + "<br>" +
                            "From: " + currentTrip.Requisition.Location.Name + " " + currentTrip.Requisition.StartingLocation + "<br>" +
                            "To: " + currentTrip.Requisition.Location1.Name + " " + currentTrip.Requisition.FinishingLocation + "<br>" +
                            "Finished At: " + currentTrip.FinishedAt + "<br>";
                        SendMail_Single(currentTrip.Requisition.AppUser.Email, Mail_Subject, Mail_Body);
                    }
                    catch (Exception e)
                    {
                    }
                }
            }

            bll.db.SaveChanges();
        }

        public JsonResult GetFreeDriverDetail(string UniqueIDNumber)
        {
            var appUser = bll.db.AppUsers.Where(m => m.UniqueIDNumber == UniqueIDNumber).Select(m => new
            {
                m.PK_User,
                m.PRG_Type,
                m.FullName,
                m.ContactNumber,
                m.Depo.Name,
                m.AppUserType,
                //m.FK_RequisitionTrip_Pending
            }).FirstOrDefault();
            if (appUser != null)
            {
                return Json(appUser, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("NotFound", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetVehicles_Inside_Toward(Guid FK_Location)
        {
            //var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            var list_inside = bll.db.Vehicles.Where(m => m.IsDeleted != true && (m.OWN_MHT_DHT == "OWN" || m.OWN_MHT_DHT == "MHT" || m.OWN_MHT_DHT == "DHT")
            && m.LocationInOrOut == true && m.FK_LocationInOut == FK_Location && (true/*m.FK_RequisitionTrip_CurrentAssigner == null || m.FK_RequisitionTrip_CurrentAssigner == CurrentUser.PK_User*/)
            ).Select(m =>
             new
             {
                 m.PK_Vehicle,
                 m.RegistrationNumber,
                 m.OWN_MHT_DHT,
                 ContactNumber = (m.Internal_VehicleContactNumber != null ? m.Internal_VehicleContactNumber : "") + (m.MHT_DHT_DriverContactNumber != null ? m.MHT_DHT_DriverContactNumber : "")
             }
            ).ToList();

            var _dateTimeLowerLimit = DateTime.Now.AddHours(-24);
            var list_toward_query =
            (
                from trip in bll.db.RequisitionTrips
                    .Where(m =>
                        m.IsDeleted != true &&
                        m.Requisition.IsDeleted != true &&
                        m.Requisition.FK_Location_To == FK_Location &&
                        m.StatusText == InternalTripStatus.Started &&
                        m.StartedAt > _dateTimeLowerLimit
                    //m.FK_Vehicle != null
                    )

                join vehicle in bll.db.Vehicles
                    .Where(m =>
                        m.IsDeleted != true &&
                        //(m.OWN_MHT_DHT == "OWN" || m.OWN_MHT_DHT == "MHT" || m.OWN_MHT_DHT == "DHT") &&
                        m.LocationInOrOut == false && (m.FK_RequisitionTrip_CurrentAssigner == null /*|| m.FK_RequisitionTrip_CurrentAssigner == CurrentUser.PK_User*/)
                    )
                on trip.FK_Vehicle equals vehicle.PK_Vehicle

                group trip by trip.Vehicle into g_trip
                select new
                {
                    g_trip.Key.RegistrationNumber,
                    g_trip.Key.OWN_MHT_DHT,
                    ContactNumber = (g_trip.Key.Internal_VehicleContactNumber != null ? g_trip.Key.Internal_VehicleContactNumber : "") + (g_trip.Key.MHT_DHT_DriverContactNumber != null ? g_trip.Key.MHT_DHT_DriverContactNumber : ""),
                    PossibleJourneyFinishDateTime = g_trip.OrderByDescending(m => m.FinalWantedAtDateTime).Select(m => m.FinalWantedAtDateTime).FirstOrDefault()
                }
            ).AsQueryable();
            var list_toward = list_toward_query.ToList();

            return Json(new { list_inside, list_toward }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllVehicleOutSide(string VehicleRegNum)
        {
            //var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            var list = bll.db.Vehicles.Where(m => m.IsDeleted != true && (m.OWN_MHT_DHT == "OWN" || m.OWN_MHT_DHT == "MHT" || m.OWN_MHT_DHT == "DHT") && (true/*m.FK_RequisitionTrip_CurrentAssigner == null || m.FK_RequisitionTrip_CurrentAssigner == CurrentUser.PK_User*/) && m.RegistrationNumber.Contains(VehicleRegNum)).Select(m =>
               new
               {
                   m.PK_Vehicle,
                   m.RegistrationNumber,
                   m.OWN_MHT_DHT,
                   ContactNumber = (m.Internal_VehicleContactNumber != null ? m.Internal_VehicleContactNumber : "") + (m.MHT_DHT_DriverContactNumber != null ? m.MHT_DHT_DriverContactNumber : "")
               }
            ).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetRequisitionTripDetail(string TrackingID)
        {
            var query = bll.db.RequisitionTrips.Where(m => m.IsDeleted != true && m.Requisition.IsDeleted != true && m.TrackingID == TrackingID);
            if (CurrentUser.PRG_Type != "ALL")
            {
                query = query.Where(m => m.Requisition.PRG_Type == CurrentUser.PRG_Type);
            }
            var data = query.Select(m => new
            {
                m.PK_RequisitionTrip,
                m.TrackingID,
                From = m.Requisition.Location.Name + " " + m.Requisition.StartingLocation,
                To = m.Requisition.Location1.Name + " " + m.Requisition.FinishingLocation,
                RequisitionVehicleType = m.Requisition.RequisitionVehicleType.Title_English,
                m.WantedCount,
                m.StatusText,
            }).FirstOrDefault();
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("NotFound", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetRequisitionTrips(DateTime? StartingDate, DateTime? EndingDate, String TrackingId, Guid? FK_AppUser_Client, Guid? FK_Location_From, Guid? FK_Location_To, long? PK_RequisitionTrip)
        {
            var list = new List<Models.RequisitionTrip>();
            //var starting_limit = DateTime.Now.Date.AddHours(7);
            var query = bll.db.RequisitionTrips.AsQueryable().Where(m => m.IsDeleted != true && m.Requisition.IsDeleted != true && m.StatusText == InternalTripStatus.Created && m.IsForwarded != true);

            if (PK_RequisitionTrip != null)
            {
                query.Where(m => m.PK_RequisitionTrip != PK_RequisitionTrip);
            }

            if (CurrentUser.PRG_Type != "ALL")
            {
                if (FK_Location_From == null)
                {
                    query = query.Where(c => c.Requisition.PRG_Type == CurrentUser.PRG_Type);
                }
                else
                {
                    query = query.Where(c => c.Requisition.PRG_Type == CurrentUser.PRG_Type || c.PRG_Type_ForwaredTo == CurrentUser.PRG_Type);
                }
            }

            if (StartingDate != null)
            {
                var _StartingDate = StartingDate != null ? StartingDate : new DateTime();
                query = query.Where(m => m.Requisition.PossibleJourneyStartDateTime > _StartingDate);
            }

            if (EndingDate != null)
            {
                var _EndingDate = EndingDate != null ? EndingDate : new DateTime();
                query = query.Where(m => m.Requisition.PossibleJourneyStartDateTime < _EndingDate);
            }

            if (!string.IsNullOrEmpty(TrackingId))
            {
                query = query.Where(m => m.TrackingID.Contains(TrackingId));
            }

            if (FK_AppUser_Client != null)
            {
                query = query.Where(m => m.Requisition.FK_AppUser_Client == FK_AppUser_Client);
            }

            if (FK_Location_From != null)
            {
                query = query.Where(m => (m.Requisition.FK_Location_From == FK_Location_From || (m.IsForwarded == true && m.FK_Location_ForwardedTo == FK_Location_From)));
            }

            if (FK_Location_To != null)
            {
                query = query.Where(m => m.Requisition.FK_Location_To == FK_Location_To);
            }

            if (StartingDate != null || EndingDate != null || FK_AppUser_Client != null || FK_Location_From != null || FK_Location_To != null)
            {
                list = query.OrderBy(m => m.Requisition.PossibleJourneyStartDateTime).ToList();
            }

            if (list.Count > 0)
            {

                var data = list.Select(item => new
                {
                    item.PK_RequisitionTrip,
                    item.TrackingID,
                    item.Requisition.PRG_Type,
                    Raiser = item.Requisition.AppUser.FullName,
                    From = item.Requisition.Location.Name + " " + item.Requisition.StartingLocation,
                    To = item.Requisition.Location1.Name + " " + item.Requisition.FinishingLocation,
                    RequisitionVehicleType = item.Requisition.RequisitionVehicleType.Title_English,
                    item.WantedCount,
                    item.Requisition.PossibleJourneyStartDateTime,
                    item.StatusText,
                    item.IsForwarded
                });
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("NotFound", JsonRequestBehavior.AllowGet);
            }

        }




        //# services
        public string TryShift_RequisitionTrip_To_RequisitionTripFinished()
        {
            var guid = Guid.NewGuid();
            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "TryShift_RequisitionTrip_To_RequisitionTripFinished-Start-" + guid,
                      CallingTime = DateTime.Now,
                      UserDefinedMessage = ""
                  }
                  );
            bll.db.SaveChanges();
            var res = "";
            try
            {
                var starting_limit = DateTime.Now.Date.AddDays(-7);

                //# Parent
                var finishedParentList = bll.db.RequisitionTrips.AsQueryable().Where(m => m.FinalWantedAtDateTime < starting_limit /*&& m.StatusText == InternalTripStatus.Finished*/ && m.IsParent == true).ToList();
                Int64 parentCount = 0;
                foreach (var parent in finishedParentList)
                {
                    if (parent.Vehicle.FK_RequisitionTrip_Last == parent.PK_RequisitionTrip)
                    {
                        parent.Vehicle.FK_RequisitionTrip_Last = null;
                    }
                    var childreen = bll.db.RequisitionTrips.Where(m => m.IsParent != true && m.FK_RequisitionTrip_Parent == parent.PK_RequisitionTrip).ToList();
                    if (true/*childreen.Count() == childreen.Where(m => m.StatusText == InternalTripStatus.Finished).Count()*/)
                    {
                        RequisitionTrip_Finished requisitionTrip_Finished_parent = convert_RequisitionTrip_To_RequisitionTripFinished(parent);
                        bll.db.RequisitionTrip_Finished.Add(requisitionTrip_Finished_parent);
                        foreach (var child in childreen)
                        {
                            if (child.Vehicle != null && child.Vehicle.FK_RequisitionTrip_Last == child.PK_RequisitionTrip)
                            {
                                child.Vehicle.FK_RequisitionTrip_Last = null;
                            }
                            RequisitionTrip_Finished requisitionTrip_Finished_child = convert_RequisitionTrip_To_RequisitionTripFinished(child);
                            bll.db.RequisitionTrip_Finished.Add(requisitionTrip_Finished_child);
                            bll.db.RequisitionTrips.Remove(child);
                        }
                        bll.db.RequisitionTrips.Remove(parent);
                        bll.db.SaveChanges();
                        parentCount++;
                    }
                }
                res = res + "finishedParentList : " + parentCount + @"/" + finishedParentList.Count;
            }
            catch (Exception e)
            {
                bll.db.AppErrorLogs.Add(
                      new AppErrorLog()
                      {
                          ErrorMessage = e.Message,
                          ErrorTime = DateTime.Now,
                          UserDefinedMessage = "Requisition/TryShift_RequisitionTrip_To_RequisitionTripFinished"
                      }
                    );
                bll.db.SaveChanges();
            }
            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "TryShift_RequisitionTrip_To_RequisitionTripFinished-End-" + guid,
                      CallingTime = DateTime.Now,
                      UserDefinedMessage = ""
                  }
                  );
            bll.db.SaveChanges();
            return res;
        }
        public RequisitionTrip_Finished convert_RequisitionTrip_To_RequisitionTripFinished(RequisitionTrip requisitionTrip)
        {
            RequisitionTrip_Finished requisitionTrip_Finished = new RequisitionTrip_Finished();

            requisitionTrip_Finished.PK_RequisitionTrip_Finished = requisitionTrip.PK_RequisitionTrip;
            requisitionTrip_Finished.IsDeleted = requisitionTrip.IsDeleted;
            requisitionTrip_Finished.TrackingID = requisitionTrip.TrackingID;
            requisitionTrip_Finished.FK_Requisition = requisitionTrip.FK_Requisition;
            requisitionTrip_Finished.WantedCount = requisitionTrip.WantedCount;
            requisitionTrip_Finished.FK_Vehicle = requisitionTrip.FK_Vehicle;
            requisitionTrip_Finished.OWN_MHT_DHT = requisitionTrip.OWN_MHT_DHT;
            requisitionTrip_Finished.Driver_Staff_ID = requisitionTrip.Driver_Staff_ID;
            requisitionTrip_Finished.Driver_Name = requisitionTrip.Driver_Name;
            requisitionTrip_Finished.Driver_ContactNumber = requisitionTrip.Driver_ContactNumber;
            requisitionTrip_Finished.TotalAmount = requisitionTrip.TotalAmount;
            requisitionTrip_Finished.CommissionAmount = requisitionTrip.CommissionAmount;
            requisitionTrip_Finished.AdvanceAmount = requisitionTrip.AdvanceAmount;
            requisitionTrip_Finished.PendingAmount = requisitionTrip.PendingAmount;
            requisitionTrip_Finished.FK_TransportAgency = requisitionTrip.FK_TransportAgency;
            requisitionTrip_Finished.FK_AppUser_Assigner = requisitionTrip.FK_AppUser_Assigner;
            requisitionTrip_Finished.FinalWantedAtDateTime = requisitionTrip.FinalWantedAtDateTime;
            requisitionTrip_Finished.AssingedAt = requisitionTrip.AssingedAt;
            requisitionTrip_Finished.TentativeFinishingDateTime = requisitionTrip.TentativeFinishingDateTime;
            requisitionTrip_Finished.FK_AppUser_Canceller = requisitionTrip.FK_AppUser_Canceller;
            requisitionTrip_Finished.CancelledAt = requisitionTrip.CancelledAt;
            requisitionTrip_Finished.StatusText = requisitionTrip.StatusText;
            requisitionTrip_Finished.StartedAt = requisitionTrip.StartedAt;
            requisitionTrip_Finished.FK_AppUser_Start = requisitionTrip.FK_AppUser_Start;
            requisitionTrip_Finished.FK_LocationGate_Start = requisitionTrip.FK_LocationGate_Start;
            requisitionTrip_Finished.StartAutoOrManaul = requisitionTrip.StartAutoOrManaul;
            requisitionTrip_Finished.FinishedAt = requisitionTrip.FinishedAt;
            requisitionTrip_Finished.FK_AppUser_Finish = requisitionTrip.FK_AppUser_Finish;
            requisitionTrip_Finished.FK_LocationGate_Finish = requisitionTrip.FK_LocationGate_Finish;
            requisitionTrip_Finished.FinishAutoOrManaul = requisitionTrip.FinishAutoOrManaul;
            requisitionTrip_Finished.IsParent = requisitionTrip.IsParent;
            requisitionTrip_Finished.FK_RequisitionTrip_Finished_Parent = requisitionTrip.FK_RequisitionTrip_Parent;
            requisitionTrip_Finished.IsForwarded = requisitionTrip.IsForwarded;
            requisitionTrip_Finished.FK_Location_ForwardedTo = requisitionTrip.FK_Location_ForwardedTo;
            requisitionTrip_Finished.PRG_Type_ForwaredTo = requisitionTrip.PRG_Type_ForwaredTo;
            requisitionTrip_Finished.ForwardedAt = requisitionTrip.ForwardedAt;
            requisitionTrip_Finished.FK_AppUser_ForwardedBy = requisitionTrip.FK_AppUser_ForwardedBy;
            requisitionTrip_Finished.OracleDB_IsPushed = requisitionTrip.OracleDB_IsPushed;
            requisitionTrip_Finished.OracleDB_PushedAt = requisitionTrip.OracleDB_PushedAt;
            requisitionTrip_Finished.OracleDB_IsPulled = requisitionTrip.OracleDB_IsPulled;
            requisitionTrip_Finished.OracleDB_PulledAt = requisitionTrip.OracleDB_PulledAt;
            requisitionTrip_Finished.OracleDB_GPNumber = requisitionTrip.OracleDB_GPNumber;
            requisitionTrip_Finished.OracleDB_GPNumberUpdatedAt = requisitionTrip.OracleDB_GPNumberUpdatedAt;
            requisitionTrip_Finished.FK_ParkingInOut_Before = requisitionTrip.FK_ParkingInOut_Before;
            requisitionTrip_Finished.FK_ParkingInOut_After = requisitionTrip.FK_ParkingInOut_After;
            requisitionTrip_Finished.ManualParkingEntryTime = requisitionTrip.ManualParkingEntryTime;
            requisitionTrip_Finished.AssigningNote = requisitionTrip.AssigningNote;
            requisitionTrip_Finished.IsGatePassUsed = requisitionTrip.IsGatePassUsed;
            requisitionTrip_Finished.FK_VehicleInOutManual_Before = requisitionTrip.FK_VehicleInOutManual_Before;
            requisitionTrip_Finished.FK_VehicleInOutManual_After = requisitionTrip.FK_VehicleInOutManual_After;
            requisitionTrip_Finished.PRG_Type = requisitionTrip.PRG_Type;

            return requisitionTrip_Finished;
        }

        public JsonResult RequisitionVsGP_GetDataForBI(string PRG_Type, string ReportDate)
        {
            var _StartingLimit = Convert.ToDateTime(ReportDate);
            var _EndingLimit = _StartingLimit.AddDays(1);
            var list = bll.db.Requisitions.Where(m => m.IsDeleted != true && m.PRG_Type == PRG_Type && m.PossibleJourneyStartDateTime > _StartingLimit && m.PossibleJourneyStartDateTime < _EndingLimit).ToList();

            var res = list.OrderBy(m => m.Location.Name).GroupBy(m => m.Location).AsQueryable().Select(item => new RequisitionVsGPViewModel()
            {
                LocationId = item.FirstOrDefault().Location.PK_Location,
                Location = item.FirstOrDefault().Location.Name,
                Wanted = item.Sum(m => m.WantedCount),
                Accepted = item.Sum(m => m.AcceptedCount),
                Assigned_Total = item.SelectMany(m => m.RequisitionTrips).Where(m => m.FK_Vehicle != null).Sum(m => m.WantedCount),
                Assigned_OWN = item.SelectMany(m => m.RequisitionTrips).Where(m => m.FK_Vehicle != null && m.OWN_MHT_DHT == "OWN").Sum(m => m.WantedCount),
                Assigned_MHT = item.SelectMany(m => m.RequisitionTrips).Where(m => m.FK_Vehicle != null && m.OWN_MHT_DHT == "MHT").Sum(m => m.WantedCount),
                Assigned_DHT = item.SelectMany(m => m.RequisitionTrips).Where(m => m.FK_Vehicle != null && m.OWN_MHT_DHT == "DHT").Sum(m => m.WantedCount),
            }).ToList();
            foreach (var item in res)
            {
                item.GPCount = bll.db.LocationWiseGPs.Where(m => m.PRG_Type == PRG_Type && m.FK_Location == item.LocationId && m.IssueDate == _StartingLimit).Any() ? bll.db.LocationWiseGPs.Where(m => m.PRG_Type == PRG_Type && m.FK_Location == item.LocationId && m.IssueDate == _StartingLimit).Select(m => m.GPCount).FirstOrDefault() : 0;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public string TrySych_RequisitionTrip_With_PRAN_OracleDB()
        {
            var guid = Guid.NewGuid();
            var now = DateTime.Now;
            var sqlCommandList = new List<string>();

            //# PRAN Oracle Push:Start
            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "TrySych_RequisitionTrip_With_PRAN_OracleDB-Push-Start-" + guid,
                      CallingTime = DateTime.Now,
                      UserDefinedMessage = ""
                  }
                  );
            bll.db.SaveChanges();
            var finishing_limit = DateTime.Now;//# to cancel trip for wrong entry
            var starting_limit = DateTime.Now.AddDays(-3);
            try
            {
                //# get list
                var this_list = bll.db.RequisitionTrips.Where(m =>
                m.IsDeleted != true
                && m.Requisition.PRG_Type == "PRAN"
                && m.FinalWantedAtDateTime > starting_limit
                && m.AssingedAt < finishing_limit
                && m.Requisition.OrganizationCode != null && m.Requisition.OrganizationName != null
                && (m.IsParent == true || m.FK_RequisitionTrip_Parent == null)
                && m.FK_Vehicle != null
                && m.OracleDB_IsPushed == null
                ).Select(m => m).ToList();

                foreach (var requisitionTrip in this_list)
                {
                    /*
                    insert into T_REQE_PRAN (REQE_ID,REQE_TRID,REQE_DAT,VEH_NUM,VEH_ID,IDATE,ORG_CODE,ORG_NAME) values ('451921','R 325769-4',to_date('2021-06-30 11:24:00', 'yyyy-mm-dd hh24:mi:ss'),'DHAKA METRO-AU-11-1571','1115711',to_date('2021-06-30 11:24:24', 'yyyy-mm-dd hh24:mi:ss'),'221', 'PFG-MAT-102')
                     */
                    var ERP_Id = requisitionTrip.Vehicle.ERP_Id != null && requisitionTrip.Requisition.PRG_Type == requisitionTrip.Vehicle.Depo.PRG_Type ? requisitionTrip.Vehicle.ERP_Id : 55555;
                    var sqlCommand = "insert into T_REQE_PRAN (REQE_ID,REQE_TRID,REQE_DAT,VEH_NUM,VEH_ID,IDATE,ORG_CODE,ORG_NAME) values (" +
                                    "'" + requisitionTrip.PK_RequisitionTrip + "'," +
                                    "'" + requisitionTrip.TrackingID + "'," +
                                    "to_date('" + ((DateTime)requisitionTrip.FinalWantedAtDateTime).ToString("yyyy-MM-dd HH:mm:ss") + "', 'yyyy-mm-dd hh24:mi:ss')," +
                                    "'" + requisitionTrip.Vehicle.RegistrationNumber + "'," +
                                    "'" + ERP_Id + "'," +
                                    "to_date('" + now.ToString("yyyy-MM-dd HH:mm:ss") + "', 'yyyy-mm-dd hh24:mi:ss')," +
                                    "'" + requisitionTrip.Requisition.OrganizationCode + "', " +
                                    "'" + requisitionTrip.Requisition.OrganizationName + "'" +
                                    ")";
                    sqlCommandList.Add(sqlCommand);
                }

                var res = OracleRequisitionTrip_PRAN_DBHelper.DbSaveChanges(sqlCommandList);
                if (res == true)
                {
                    foreach (var item in this_list)
                    {
                        item.OracleDB_IsPushed = true;
                        item.OracleDB_PushedAt = now;
                    }
                    bll.db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                bll.db.AppErrorLogs.Add(
                      new AppErrorLog()
                      {
                          ErrorMessage = e.Message,
                          ErrorTime = DateTime.Now,
                          UserDefinedMessage = "Requisition/TrySych_RequisitionTrip_With_PRAN_OracleDB-Push"
                      }
                    );
                bll.db.SaveChanges();
            }
            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "TrySych_RequisitionTrip_With_PRAN_OracleDB-Push-End-" + guid,
                      CallingTime = DateTime.Now,
                      UserDefinedMessage = ""
                  }
                  );
            bll.db.SaveChanges();
            //# PRAN Oracle Push:End


            //# PRAN Oracle Pull:Start
            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "TrySych_RequisitionTrip_With_PRAN_OracleDB-Pull-Start-" + guid,
                      CallingTime = DateTime.Now,
                      UserDefinedMessage = ""
                  }
                  );
            bll.db.SaveChanges();
            try
            {
                var query = "SELECT * FROM T_REQE_PRAN where REQE_DAT >= to_date('" + starting_limit.ToString("yyyy-MM-dd HH:mm:ss") + "', 'YYYY-MM-DD HH24:MI:SS') and REQE_GPNO is not NULL and IS_PULLED = 'N'";
                DataTable res = OracleRequisitionTrip_PRAN_DBHelper.ExecuteSelectCommand(query);
                foreach (DataRow row in res.Rows)
                {
                    var REQE_ID = Convert.ToInt64(row["REQE_ID"].ToString());
                    var requisitionTrip = bll.db.RequisitionTrips.Where(m => m.PK_RequisitionTrip == REQE_ID).FirstOrDefault();
                    if (requisitionTrip != null)
                    {
                        var REQE_GPNO = row["REQE_GPNO"].ToString();
                        if (!string.IsNullOrEmpty(REQE_GPNO))
                        {
                            requisitionTrip.OracleDB_GPNumber = REQE_GPNO;
                        }
                        var REQE_UPDAT = row["REQE_UPDAT"].ToString();
                        if (!string.IsNullOrEmpty(REQE_UPDAT))
                        {
                            requisitionTrip.OracleDB_GPNumberUpdatedAt = Convert.ToDateTime(REQE_UPDAT);
                        }
                        var sqlCommand = "update T_REQE_PRAN set IS_PULLED = 'Y' where REQE_ID = '" + REQE_ID + "'";
                        sqlCommandList.Add(sqlCommand);
                    }
                }
                bll.db.SaveChanges();
                OracleRequisitionTrip_PRAN_DBHelper.DbSaveChanges(sqlCommandList);
            }
            catch (Exception e)
            {
                bll.db.AppErrorLogs.Add(
                      new AppErrorLog()
                      {
                          ErrorMessage = e.Message,
                          ErrorTime = DateTime.Now,
                          UserDefinedMessage = "Requisition/TrySych_RequisitionTrip_With_PRAN_OracleDB-Pull"
                      }
                    );
                bll.db.SaveChanges();
            }

            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "TrySych_RequisitionTrip_With_PRAN_OracleDB-Pull-End-" + guid,
                      CallingTime = DateTime.Now,
                      UserDefinedMessage = ""
                  }
                  );
            bll.db.SaveChanges();
            //# PRAN Oracle Pull:End
            return "Done";
        }

        public string TrySych_RequisitionTrip_With_RFL_OracleDB()
        {
            var guid = Guid.NewGuid();
            var now = DateTime.Now;
            var sqlCommandList = new List<string>();

            //# PRAN Oracle Push:Start
            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "TrySych_RequisitionTrip_With_RFL_OracleDB_Test-Push-Start-" + guid,
                      CallingTime = DateTime.Now,
                      UserDefinedMessage = ""
                  }
                  );
            bll.db.SaveChanges();
            var finishing_limit = DateTime.Now;//# to cancel trip for wrong entry
            var starting_limit = DateTime.Now.AddDays(-3);
            try
            {
                //# get list
                var this_list = bll.db.RequisitionTrips.Where(m =>
                m.IsDeleted != true
                && m.Requisition.PRG_Type == "RFL"
                && m.FinalWantedAtDateTime > starting_limit
                && m.AssingedAt < finishing_limit
                && m.Requisition.OrganizationCode != null && m.Requisition.OrganizationName != null
                && (m.IsParent == true || m.FK_RequisitionTrip_Parent == null)
                && m.FK_Vehicle != null
                && m.OracleDB_IsPushed == null
                ).AsQueryable().Select(m => m).ToList();

                foreach (var requisitionTrip in this_list)
                {
                    /*
                    insert into T_REQE_PRAN (REQE_ID,REQE_TRID,REQE_DAT,VEH_NUM,VEH_ID,IDATE,ORG_CODE,ORG_NAME) values ('451921','R 325769-4',to_date('2021-06-30 11:24:00', 'yyyy-mm-dd hh24:mi:ss'),'DHAKA METRO-AU-11-1571','1115711',to_date('2021-06-30 11:24:24', 'yyyy-mm-dd hh24:mi:ss'),'221', 'PFG-MAT-102')
                     */
                    var ERP_Id = requisitionTrip.Vehicle.ERP_Id != null && requisitionTrip.Requisition.PRG_Type == requisitionTrip.Vehicle.Depo.PRG_Type ? requisitionTrip.Vehicle.ERP_Id : 55555;
                    var sqlCommand = "insert into T_REQE_RFL (REQE_ID,REQE_TRID,REQE_DAT,VEH_NUM,VEH_ID,IDATE,ORG_CODE,ORG_NAME) values (" +
                                    "'" + requisitionTrip.PK_RequisitionTrip + "'," +
                                    "'" + requisitionTrip.TrackingID + "'," +
                                    "to_date('" + ((DateTime)requisitionTrip.FinalWantedAtDateTime).ToString("yyyy-MM-dd HH:mm:ss") + "', 'yyyy-mm-dd hh24:mi:ss')," +
                                    "'" + requisitionTrip.Vehicle.RegistrationNumber + "'," +
                                    "'" + ERP_Id + "'," +
                                    "to_date('" + now.ToString("yyyy-MM-dd HH:mm:ss") + "', 'yyyy-mm-dd hh24:mi:ss')," +
                                    "'" + requisitionTrip.Requisition.OrganizationCode + "', " +
                                    "'" + requisitionTrip.Requisition.OrganizationName + "'" +
                                    ")";
                    sqlCommandList.Add(sqlCommand);
                }

                var res = OracleRequisitionTrip_RFL_DBHelper.DbSaveChanges(sqlCommandList);
                if (res == true)
                {
                    foreach (var item in this_list)
                    {
                        item.OracleDB_IsPushed = true;
                        item.OracleDB_PushedAt = now;
                    }
                    bll.db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                bll.db.AppErrorLogs.Add(
                      new AppErrorLog()
                      {
                          ErrorMessage = e.Message,
                          ErrorTime = DateTime.Now,
                          UserDefinedMessage = "Requisition/TrySych_RequisitionTrip_With_RFL_OracleDB_Test-Push"
                      }
                    );
                bll.db.SaveChanges();
            }
            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "TrySych_RequisitionTrip_With_RFL_OracleDB_Test-Push-End-" + guid,
                      CallingTime = DateTime.Now,
                      UserDefinedMessage = ""
                  }
                  );
            bll.db.SaveChanges();
            //# PRAN Oracle Push:End


            //# PRAN Oracle Pull:Start
            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "TrySych_RequisitionTrip_With_RFL_OracleDB_Test-Pull-Start-" + guid,
                      CallingTime = DateTime.Now,
                      UserDefinedMessage = ""
                  }
                  );
            bll.db.SaveChanges();
            try
            {
                var query = "SELECT * FROM T_REQE_RFL where REQE_DAT >= to_date('" + starting_limit.ToString("yyyy-MM-dd HH:mm:ss") + "', 'YYYY-MM-DD HH24:MI:SS') and REQE_GPNO is not NULL and IS_PULLED = 'N'";
                DataTable res = OracleRequisitionTrip_RFL_DBHelper.ExecuteSelectCommand(query);
                foreach (DataRow row in res.Rows)
                {
                    var REQE_ID = Convert.ToInt64(row["REQE_ID"].ToString());
                    var requisitionTrip = bll.db.RequisitionTrips.Where(m => m.PK_RequisitionTrip == REQE_ID).FirstOrDefault();
                    if (requisitionTrip != null)
                    {
                        var REQE_GPNO = row["REQE_GPNO"].ToString();
                        if (!string.IsNullOrEmpty(REQE_GPNO))
                        {
                            requisitionTrip.OracleDB_GPNumber = REQE_GPNO;
                        }
                        var REQE_UPDAT = row["REQE_UPDAT"].ToString();
                        if (!string.IsNullOrEmpty(REQE_UPDAT))
                        {
                            requisitionTrip.OracleDB_GPNumberUpdatedAt = Convert.ToDateTime(REQE_UPDAT);
                        }
                        var sqlCommand = "update T_REQE_RFL set IS_PULLED = 'Y' where REQE_ID = '" + REQE_ID + "'";
                        sqlCommandList.Add(sqlCommand);
                    }
                }
                bll.db.SaveChanges();
                OracleRequisitionTrip_PRAN_DBHelper.DbSaveChanges(sqlCommandList);
            }
            catch (Exception e)
            {
                bll.db.AppErrorLogs.Add(
                      new AppErrorLog()
                      {
                          ErrorMessage = e.Message,
                          ErrorTime = DateTime.Now,
                          UserDefinedMessage = "Requisition/TrySych_RequisitionTrip_With_RFL_OracleDB_Test-Pull"
                      }
                    );
                bll.db.SaveChanges();
            }

            bll.db.ServiceCalls.Add(
                  new ServiceCall()
                  {
                      CallingMessage = "TrySych_RequisitionTrip_With_RFL_OracleDB_Test-Pull-End-" + guid,
                      CallingTime = DateTime.Now,
                      UserDefinedMessage = ""
                  }
                  );
            bll.db.SaveChanges();
            //# PRAN Oracle Pull:End
            return "Done";
        }


        #region Requisition Alert Mail
        public string TrySendRequisitionReportEmail()
        {
            try
            {
                var _email = "automation@mis.prangroup.com";
                var _epass = "aaaaAAAA0000";

                SmtpClient sc = new SmtpClient("mail.mis.prangroup.com");
                //SmtpClient sc = new SmtpClient("172.17.4.106");
                sc.EnableSsl = false;
                sc.Credentials = new NetworkCredential(_email, _epass);
                sc.Port = 25;

                var Mail_Subject = "3rd EyE: Daily Vehicle Sharing Report";
                var Mail_ToList = new List<string>() {
                    "dist100@prangroup.com", "piptpt7@pip.prangroup.com", "pipdist20@pip.prangroup.com","piptpt8@pip.prangroup.com","dist15@hip.prangroup.com","desh12@prangroup.com", //PRAN
                    "dist121@prangroup.com", "dist14@rflgroupbd.com", "rfl148@rflgroupbd.com", //RFL
                    "dist135@rflgroupbd.com", "dist235@rflgroupbd.com", "dist133@rflgroupbd.com", "dist33@rflgroupbd.com","dist48@rflgroupbd.com","pipdist145@pip.prangroup.com","rfltpt1@pip.prangroup.com","dist370@rflgroupbd.com", //RFL
                };
                var Mail_CcList = new List<string>() { "automation17@mis.prangroup.com", "mis3@prangroup.com" };
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
                string url = "";
#if DEBUG
                url = ConfigurationManager.AppSettings["DEBUG_DOMAIN"];
#else
url = ConfigurationManager.AppSettings["LIVE_DOMAIN"];
#endif
                url = url + @"Requisition/RequisitionReportEmailBodyGenerator";
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
        public ActionResult RequisitionReportEmailBodyGenerator()
        {
            var starting_limit = DateTime.Now.AddDays(-1);
            var finishing_limit = DateTime.Now;
            //var fromDate = DateTime.ParseExact("2019-04-01", "yyyy-MM-dd", CultureInfo.InvariantCulture);
            //var toDate = DateTime.ParseExact("2019-05-01", "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var requisionlist = bll.db.IndividualRequisitions.Where(m => (m.AppUser.PRG_Type == "PRAN" || m.AppUser.PRG_Type == "RFL") &&
            m.PossibleJourneyStartDateTime >= starting_limit && m.PossibleJourneyStartDateTime < finishing_limit).ToList();

            ViewBag.InstantRequisition_PRAN = (from req in bll.db.InstantRequisitions.Where(m => m.CreatedAt >= starting_limit && m.CreatedAt < finishing_limit)
                                               join user in bll.db.AppUsers.Where(m => m.PRG_Type == "PRAN") on req.FK_RequisitionAgent equals user.PK_User
                                               select req).Count();
            ViewBag.InstantRequisition_RFL = (from req in bll.db.InstantRequisitions.Where(m => m.CreatedAt >= starting_limit && m.CreatedAt < finishing_limit)
                                              join user in bll.db.AppUsers.Where(m => m.PRG_Type == "RFL") on req.FK_RequisitionAgent equals user.PK_User
                                              select req).Count();


            ViewBag.OutSide_PRAN = (from trip in bll.db.VehicleTrips.Where(m => m.CreatedAt >= starting_limit && m.CreatedAt < finishing_limit)
                                    join user in bll.db.AppUsers.Where(m => m.PRG_Type == "PRAN") on trip.FK_CreatedByUser equals user.PK_User
                                    select trip).Count();
            ViewBag.OutSide_RFL = (from trip in bll.db.VehicleTrips.Where(m => m.CreatedAt >= starting_limit && m.CreatedAt < finishing_limit)
                                   join user in bll.db.AppUsers.Where(m => m.PRG_Type == "RFL") on trip.FK_CreatedByUser equals user.PK_User
                                   select trip).Count();
            return View(requisionlist);
        }
        #endregion

    }

    public class RequisitionVsGPViewModel
    {
        public Guid LocationId { get; set; }
        public string Location { get; set; }
        public double? Wanted { get; set; }
        public double? Accepted { get; set; }
        public double? Assigned_Total { get; set; }
        public double? Assigned_OWN { get; set; }
        public double? Assigned_MHT { get; set; }
        public double? Assigned_DHT { get; set; }
        public long? GPCount { get; set; }
    }

}