using _3rdEyE.BLL;
using _3rdEyE.BLLs;
using _3rdEyE.ManagingTools;
using _3rdEyE.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace _3rdEyE.Controllers
{
    public class VehicleSharingController : BaseController
    {
        static class InternalTripStatus
        {
            public const string Assigned = "Assigned";
            public const string EnteredStartingLocation = "Entered Starting Location";
            public const string StartedLoading = "Started Loading";
            public const string FinishedLoading = "Finished Loading";
            public const string StartedEmptyTrip = "Started Empty Trip";
            public const string CreatedBill = "Created Bill";
            public const string ApprovedBill = "Approved Bill";
            public const string PaidBill = "Paid Bill";
            public const string LeftStartingLoaction = "Left Starting Loaction";
            public const string EnteredFinishingLocation = "Entered Finishing Location";
            public const string StartedUnloading = "Started Unloading";
            public const string FinishedUnloading = "Finished Unloading";
            public const string FinishedEmptyTrip = "Finished Empty Trip";
        }

        static class VehicleSharingBiddingStatus
        {
            public const string Created = "Created";
            public const string Bidded = "Bidded";
            public const string Approved = "Approved";
            public const string CancelledByClient = "Cancelled By Client";
            public const string CancelledByApprover = "Cancelled By Approver";
        }

        static class ExternalTripStatus
        {
            public const string Assigned = "Assigned";
            public const string EnteredStartingLocation = "Entered Starting Location";
            public const string LeftStartingLoaction = "Left Starting Loaction";
            public const string EnteredFinishingLocation = "Entered Finishing Location";
            public const string LeftFinishingLocation = "Left Finishing Location";
        }
        static class ExternalTripPaymentStatus
        {
            public const string ApprovedBill = "Approved Bill";
            public const string PaidBill = "Paid Bill";
        }
        public string NotifyExternal()
        {

            //-Notify Driver Firebase
            var driver = bll.db.AppUsers.Where(m => m.UniqueIDNumber == "ta2").FirstOrDefault();
            if (!string.IsNullOrEmpty(driver.FID))
            {
                var title = "Test Bidding Notify #";
                var message = "Dear Concern \n";
                message = message + "Trip Detail" + "\n";
                SendFCM_Notification_Single_New(driver.FID, title, message, "35", "external");
            }
            return "external-sent";
        }
        public string NotifyInternal()
        {

            //-Notify Driver Firebase
            var driver = bll.db.AppUsers.Where(m => m.UniqueIDNumber == "d001").FirstOrDefault();
            if (!string.IsNullOrEmpty(driver.FID))
            {
                var title = "Test Trip Notify #";
                var message = "Dear Concern \n";
                message = message + "Trip Detail" + "\n";
                SendFCM_Notification_Single_New(driver.FID, title, message, "33", "VehicleSharingInternalTrip");
            }
            return "VehicleSharingInternalTrip-sent";
        }

        //# Demand
        public ActionResult DemandIndexBy_Client(DateTime? StartingDate, DateTime? EndingDate)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            ViewBag.DisplayMessage = bll.db.DisplayMessages.Where(m => m.PK_DisplayMessage == "InterCompanyRequisition/IndexBy_Client").Select(m => m.Message).FirstOrDefault();
            if (StartingDate != null && EndingDate != null)
            {
                var _StartingDate = StartingDate != null ? StartingDate : new DateTime();
                var _EndingDate = EndingDate != null ? EndingDate : new DateTime();
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", _StartingDate);
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", _EndingDate);
                var list = bll.db.VehicleSharingDemands.AsEnumerable()/*.Where(m => m.IsDeleted != true && m.FK_AppUser_Client == CurrentUser.PK_User && m.CreatedAt >= _StartingDate && m.CreatedAt <= _EndingDate)*/.OrderByDescending(m => m.CreatedAt).ToList();
                return View(list);
            }
            else
            {
                var now = DateTime.Now;
                var today7 = DateTime.Now.Date.AddHours(7);
                if (now > today7)
                {
                    StartingDate = today7;
                    EndingDate = today7.AddDays(1);
                }
                else
                {
                    StartingDate = today7.AddDays(-1);
                    EndingDate = today7;
                }
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", StartingDate);
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", EndingDate);
                var list = new List<VehicleSharingDemand>();
                return View(list);
            }
        }

        public ActionResult DemandIndexBy_Approver(DateTime? StartingDate, DateTime? EndingDate, Guid? FK_AppUser_Client, Guid? FK_Depo_From, Guid? FK_Depo_To)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var list = new List<VehicleSharingDemand>();
            var now = DateTime.Now;
            var today7 = DateTime.Now.Date.AddHours(7);
            var query = bll.db.VehicleSharingDemands.AsEnumerable().Where(m => m.IsDeleted != true);
            if (StartingDate != null)
            {
                var _StartingDate = StartingDate != null ? StartingDate : new DateTime();
                //query = query.Where(m => m.CreatedAt >= _StartingDate);
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", _StartingDate);
            }
            else
            {
                if (now > today7)
                {
                    ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", today7);
                }
                else
                {
                    ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", today7.AddDays(-1));
                }
            }
            if (EndingDate != null)
            {
                var _EndingDate = EndingDate != null ? EndingDate : new DateTime();
                //query = query.Where(m => m.CreatedAt <= _EndingDate);
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", _EndingDate);
            }
            else
            {
                if (now > today7)
                {
                    ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", today7.AddDays(1));
                }
                else
                {
                    ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", today7);
                }
            }
            if (FK_AppUser_Client != null)
            {
                //query = query.Where(m => m.FK_AppUser_Client == FK_AppUser_Client);
                var _cliesnts = bll.db.AppUsers.Where(m => m.IsDeleted != true && m.AppUserSubType.Contains("RAISER")).Select(m => new { m.PK_User, Text = m.FullName + ":" + m.Depo.Name }).ToList();
                ViewBag.Clients = new SelectList(_cliesnts, "PK_User", "Text", FK_AppUser_Client);
            }
            else
            {
                var _cliesnts = bll.db.AppUsers.Where(m => m.IsDeleted != true && m.AppUserSubType.Contains("RAISER")).Select(m => new { m.PK_User, Text = m.FullName + ":" + m.Depo.Name }).ToList();
                ViewBag.Clients = new SelectList(_cliesnts, "PK_User", "Text");
            }
            if (FK_Depo_From != null)
            {
                //query = query.Where(m => m.FK_Depo_From == FK_Depo_From);
                ViewBag.FromLocations = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted != true), "PK_Depo", "Name", FK_Depo_From);
            }
            else
            {
                ViewBag.FromLocations = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted != true), "PK_Depo", "Name");
            }
            if (FK_Depo_To != null)
            {
                //query = query.Where(m => m.FK_Depo_To == FK_Depo_To);
                ViewBag.ToLocations = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted != true), "PK_Depo", "Name", FK_Depo_To);
            }
            else
            {
                ViewBag.ToLocations = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted != true), "PK_Depo", "Name");
            }
            if (StartingDate != null || EndingDate != null || FK_AppUser_Client != null || FK_Depo_From != null || FK_Depo_To != null)
            {
                list = query.OrderByDescending(m => m.CreatedAt).ToList();
            }
            return View(list);
        }
        public ActionResult DemandIndexBy_Assigner(DateTime? StartingDate, DateTime? EndingDate, Guid? FK_AppUser_Client, Guid? FK_Depo_From, Guid? FK_Depo_To)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var list = new List<VehicleSharingDemand>();
            var now = DateTime.Now;
            var today7 = DateTime.Now.Date.AddHours(7);
            var query = bll.db.VehicleSharingDemands.AsEnumerable().Where(m => m.IsDeleted != true);
            if (StartingDate != null)
            {
                var _StartingDate = StartingDate != null ? StartingDate : new DateTime();
                //query = query.Where(m => m.CreatedAt >= _StartingDate);
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", _StartingDate);
            }
            else
            {
                if (now > today7)
                {
                    ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", today7);
                }
                else
                {
                    ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", today7.AddDays(-1));
                }
            }
            if (EndingDate != null)
            {
                var _EndingDate = EndingDate != null ? EndingDate : new DateTime();
                //query = query.Where(m => m.CreatedAt <= _EndingDate);
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", _EndingDate);
            }
            else
            {
                if (now > today7)
                {
                    ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", today7.AddDays(1));
                }
                else
                {
                    ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", today7);
                }
            }
            if (FK_AppUser_Client != null)
            {
                //query = query.Where(m => m.FK_AppUser_Client == FK_AppUser_Client);
                var _cliesnts = bll.db.AppUsers.Where(m => m.IsDeleted != true && m.AppUserSubType.Contains("RAISER")).Select(m => new { m.PK_User, Text = m.FullName + ":" + m.Depo.Name }).ToList();
                ViewBag.Clients = new SelectList(_cliesnts, "PK_User", "Text", FK_AppUser_Client);
            }
            else
            {
                var _cliesnts = bll.db.AppUsers.Where(m => m.IsDeleted != true && m.AppUserSubType.Contains("RAISER")).Select(m => new { m.PK_User, Text = m.FullName + ":" + m.Depo.Name }).ToList();
                ViewBag.Clients = new SelectList(_cliesnts, "PK_User", "Text");
            }
            if (FK_Depo_From != null)
            {
                //query = query.Where(m => m.FK_Depo_From == FK_Depo_From);
                ViewBag.FromLocations = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted != true), "PK_Depo", "Name", FK_Depo_From);
            }
            else
            {
                ViewBag.FromLocations = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted != true), "PK_Depo", "Name");
            }
            if (FK_Depo_To != null)
            {
                //query = query.Where(m => m.FK_Depo_To == FK_Depo_To);
                ViewBag.ToLocations = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted != true), "PK_Depo", "Name", FK_Depo_To);
            }
            else
            {
                ViewBag.ToLocations = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted != true), "PK_Depo", "Name");
            }
            if (StartingDate != null || EndingDate != null || FK_AppUser_Client != null || FK_Depo_From != null || FK_Depo_To != null)
            {
                list = query.OrderByDescending(m => m.CreatedAt).ToList();
            }
            return View(list);
        }

        public ActionResult DemandCreate()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }

            //var userSubType = "-";
            //if (CurrentUser.AppUserSubType != null && CurrentUser.AppUserSubType.Contains(" RAISER"))
            //{
            //    userSubType = CurrentUser.AppUserSubType.Replace(" RAISER", " APPROVER");
            //}
            //else if (CurrentUser.AppUserSubType != null && CurrentUser.AppUserSubType.Contains(" APPROVER"))
            //{
            //    userSubType = CurrentUser.AppUserSubType;
            //}
            //var _approversPKs = bll.db.RequisitionAgentProposedDepoes.Where(m => m.WillPropose == true && m.FK_Depo == CurrentUser.FK_Depo).Select(m => m.FK_RequisitionAgent).ToList();
            //var Approvers = bll.db.AppUsers.Where(m => (m.FK_Depo == CurrentUser.FK_Depo || _approversPKs.Contains(m.PK_User)) && m.AppUserSubType == userSubType).Select(m => new { key = m.PK_User, value = m.Depo.Name + ":" + m.AppUserSubType.Replace(" APPROVER", "") + ":" + m.FullName }).ToList();
            //ViewBag.Approvers = new SelectList(Approvers, "key", "value");
            ViewBag.RequisitionVehicleTypes = new SelectList(bll.db.RequisitionVehicleTypes.Where(m => m.IsDeleted != true), "PK_RequisitionVehicleType", "Title_English");
            var locations = bll.db.Depoes.Where(m => m.IsDeleted != true ).Select(m => new { PK_Depo = m.PK_Depo, Name = m.Code + " " + m.Name });
            ViewBag.Locations1 = new SelectList(locations.OrderBy(m => m.Name), "PK_Depo", "Name");
            ViewBag.Locations2 = new SelectList(locations.OrderBy(m => m.Name), "PK_Depo", "Name");
            var _today = DateTime.Now.Date;
            ViewBag.AlreadyOpenedCount = bll.db.VehicleSharingDemands.Where(m => m.FK_AppUser_Client == CurrentUser.PK_User && m.CreatedAt > _today).Count();
            return View();
        }
        [HttpPost]
        public ActionResult DemandCreate(FormCollection form)
        {
            var model = new VehicleSharingDemand();
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            try
            {
                model.IsDeleted = false;

                model.CreatedAt = DateTime.Now;
                model.FK_AppUser_Client = CurrentUser.PK_User;
                model.FK_ReferenceDepo = CurrentUser.FK_Depo;

                if (!string.IsNullOrEmpty(form["FK_Depo_From"]))
                {
                    model.FK_Depo_From = Guid.Parse(form["FK_Depo_From"]);
                }
                if (!string.IsNullOrEmpty(form["StartingLocation"]))
                {
                    model.StartingLocation = form["StartingLocation"];
                }

                if (!string.IsNullOrEmpty(form["FK_Depo_To"]))
                {
                    model.FK_Depo_To = Guid.Parse(form["FK_Depo_To"]);
                }
                if (!string.IsNullOrEmpty(form["FinishingLocation"]))
                {
                    model.FinishingLocation = form["FinishingLocation"];
                }

                if (!string.IsNullOrEmpty(form["DistanceGoogle"]))
                {
                    model.DistanceGoogle = Convert.ToDouble(form["DistanceGoogle"]);
                }
                model.FK_RequisitionVehicleType = Convert.ToInt32(form["FK_RequisitionVehicleType"]);
                //var _VehicleType = form["VehicleType"].Split('#');
                //var _Layer1 = _VehicleType[0];
                //var _Layer2 = _VehicleType[1];
                //var _Layer3 = _VehicleType[2];
                //var VehicleType = bll.db.RequisitionVehicleTypes.Where(m => m.Layer1 == _Layer1 && m.Layer2 == _Layer2 && m.Layer3 == _Layer3).FirstOrDefault();
                //model.VehicleTypeLayer1 = VehicleType.Layer1;
                //model.VehicleTypeLayer1_english = VehicleType.Layer1_english;
                //model.VehicleTypeLayer1_bangla = VehicleType.Layer1_bangla;
                //model.VehicleTypeLayer2 = VehicleType.Layer2;
                //model.VehicleTypeLayer2_english = VehicleType.Layer2_english;
                //model.VehicleTypeLayer2_bangla = VehicleType.Layer2_bangla;
                //model.VehicleTypeLayer3 = VehicleType.Layer3;
                //model.VehicleTypeLayer3_english = VehicleType.Layer3_english;
                //model.VehicleTypeLayer3_bangla = VehicleType.Layer3_bangla;
                model.WantedCount = Convert.ToDouble(form["WantedCount"]);
                model.PossibleJourneyStartDateTime = DateTime.ParseExact(form["PossibleJourneyStartDateTime"], "yyyy-MM-dd h:mm tt", CultureInfo.InvariantCulture);
                model.ClientNote = form["ClientNote"];
                model.Status = 0;
                model.LoadedOrEmpty = true;

                bll.db.VehicleSharingDemands.Add(model);
                bll.db.SaveChanges();
                model.TrackingID = "D" + model.PK_VehicleSharingDemand.ToString();
                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "Vehicle Demand is successfully added.");

                //var userSubType = CurrentUser.AppUserSubType.Replace("_RAISER", "_APPROVER");
                //var Approvers = bll.db.AppUsers.Where(m => (m.FK_Depo == CurrentUser.FK_Depo || bll.db.RequisitionAgentProposedDepoes.Where(n => n.FK_RequisitionAgent == m.PK_User && n.WillPropose == true).Select(n => n.FK_Depo).ToList().Contains(CurrentUser.FK_Depo)) && m.AppUserSubType == userSubType).Select(m => new { key = m.PK_User, value = m.Depo.Name + ":" + m.AppUserSubType.Replace("_APPROVER", "") + ":" + m.FullName }).ToList();
                //ViewBag.Approvers = new SelectList(Approvers, "key", "value", model.FK_AppUser_Approver);


                ViewBag.RequisitionVehicleTypes = new SelectList(bll.db.RequisitionVehicleTypes.Where(m => m.IsDeleted != true), "PK_RequisitionVehicleType", "Title_English", model.FK_RequisitionVehicleType);
                var locations = bll.db.Depoes.Where(m => m.IsDeleted != true && m.Code != null).Select(m => new { PK_Depo = m.PK_Depo, Name = m.Code + " " + m.Name });
                ViewBag.Locations1 = new SelectList(locations.OrderBy(m => m.Name), "PK_Depo", "Name", model.FK_Depo_From);
                ViewBag.Locations2 = new SelectList(locations.OrderBy(m => m.Name), "PK_Depo", "Name", model.FK_Depo_To);
                var _today = DateTime.Now.Date;
                ViewBag.AlreadyOpenedCount = bll.db.VehicleSharingDemands.Where(m => m.FK_AppUser_Client == CurrentUser.PK_User && m.CreatedAt > _today).Count();
                return View(model);
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
            }
            //var _userSubType = CurrentUser.AppUserSubType.Replace("_RAISER", "_APPROVER");
            //var _Approvers = bll.db.AppUsers.Where(m => (m.FK_Depo == CurrentUser.FK_Depo || bll.db.RequisitionAgentProposedDepoes.Where(n => n.FK_RequisitionAgent == m.PK_User && n.WillPropose == true).Select(n => n.FK_Depo).ToList().Contains(CurrentUser.FK_Depo)) && m.AppUserSubType == _userSubType).Select(m => new { key = m.PK_User, value = m.Depo.Name + ":" + m.AppUserSubType.Replace("_APPROVER", "") + ":" + m.FullName }).ToList();
            //ViewBag.Approvers = new SelectList(_Approvers, "key", "value", model.FK_AppUser_Approver);

            ViewBag.RequisitionVehicleTypes = new SelectList(bll.db.RequisitionVehicleTypes.Where(m => m.IsDeleted != true), "PK_RequisitionVehicleType", "Title_English", model.FK_RequisitionVehicleType);
            ViewBag.Locations1 = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Depo", "Name", model.FK_Depo_From);
            ViewBag.Locations2 = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Depo", "Name", model.FK_Depo_To);
            var __today = DateTime.Now.Date;
            ViewBag.AlreadyOpenedCount = bll.db.VehicleSharingDemands.Where(m => m.FK_AppUser_Client == CurrentUser.PK_User && m.CreatedAt > __today).Count();
            return View(model);
        }

        public ActionResult DemandEdit(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.VehicleSharingDemands.AsEnumerable().Where(m => m.PK_VehicleSharingDemand == id).FirstOrDefault();
            //var userSubType = CurrentUser.AppUserSubType.Replace("_RAISER", "_APPROVER");
            //var Approvers = bll.db.AppUsers.Where(m => (m.FK_Depo == CurrentUser.FK_Depo || bll.db.RequisitionAgentProposedDepoes.Where(n => n.FK_RequisitionAgent == m.PK_User && n.WillPropose == true).Select(n => n.FK_Depo).ToList().Contains(CurrentUser.FK_Depo)) && m.AppUserSubType == userSubType).Select(m => new { key = m.PK_User, value = m.Depo.Name + ":" + m.AppUserSubType.Replace("_APPROVER", "") + ":" + m.FullName }).ToList();
            //ViewBag.Approvers = new SelectList(Approvers, "key", "value", model.FK_AppUser_Approver);
            ViewBag.RequisitionVehicleTypes = new SelectList(bll.db.RequisitionVehicleTypes.Where(m => m.IsDeleted != true), "PK_RequisitionVehicleType", "Title_English", model.FK_RequisitionVehicleType);
            ViewBag.Locations1 = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Depo", "Name", model.FK_Depo_From);
            ViewBag.Locations2 = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Depo", "Name", model.FK_Depo_To);
            return View(model);
        }
        [HttpPost]
        public ActionResult DemandEdit(FormCollection form)
        {
            var _pk = Convert.ToInt64(form["PK_VehicleSharingDemand"]);
            var model = bll.db.VehicleSharingDemands.Where(m => m.PK_VehicleSharingDemand == _pk).FirstOrDefault();
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            try
            {
                if (model.Status == 0)
                {
                    model.UpdatedAt = DateTime.Now;
                    if (!string.IsNullOrEmpty(form["FK_Depo_From"]))
                    {
                        model.FK_Depo_From = Guid.Parse(form["FK_Depo_From"]);
                    }
                    if (!string.IsNullOrEmpty(form["FK_Depo_To"]))
                    {
                        model.FK_Depo_To = Guid.Parse(form["FK_Depo_To"]);
                    }
                    model.FK_RequisitionVehicleType = Convert.ToInt32(form["FK_RequisitionVehicleType"]);
                    model.WantedCount = Convert.ToDouble(form["WantedCount"]);
                    model.PossibleJourneyStartDateTime = DateTime.ParseExact(form["PossibleJourneyStartDateTime"], "yyyy-MM-dd h:mm tt", CultureInfo.InvariantCulture);
                    model.ClientNote = form["ClientNote"];
                    model.Status = 0;
                    bll.db.SaveChanges();

                    CreateAlertMessage(AlertMessageType.Success, "Success", "Vehicle Demand is successfully added.");
                    return RedirectToAction("DemandIndexBy_Client");
                }
                else
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", "Vehicle Sharing could not be modified anymore.");
                    return RedirectToAction("DemandIndexBy_Client");
                }
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
            }
            //var userSubType = CurrentUser.AppUserSubType.Replace("_RAISER", "_APPROVER");
            //var Approvers = bll.db.AppUsers.Where(m => (m.FK_Depo == CurrentUser.FK_Depo || bll.db.RequisitionAgentProposedDepoes.Where(n => n.FK_RequisitionAgent == m.PK_User && n.WillPropose == true).Select(n => n.FK_Depo).ToList().Contains(CurrentUser.FK_Depo)) && m.AppUserSubType == userSubType).Select(m => new { key = m.PK_User, value = m.Depo.Name + ":" + m.AppUserSubType.Replace("_APPROVER", "") + ":" + m.FullName }).ToList();
            //ViewBag.Approvers = new SelectList(Approvers, "key", "value", model.FK_AppUser_Approver);
            ViewBag.RequisitionVehicleTypes = new SelectList(bll.db.RequisitionVehicleTypes.Where(m => m.IsDeleted != true), "PK_RequisitionVehicleType", "Title_English", model.FK_RequisitionVehicleType);
            ViewBag.Locations1 = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Depo", "Name", model.FK_Depo_From);
            ViewBag.Locations2 = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Depo", "Name", model.FK_Depo_To);
            return View(model);
        }

        public ActionResult DemandView(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.VehicleSharingDemands.AsEnumerable().Where(m => m.PK_VehicleSharingDemand == id).FirstOrDefault();
            return View(model);
        }

        public ActionResult DemandApprove(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.VehicleSharingDemands.AsEnumerable().Where(m => m.PK_VehicleSharingDemand == id).FirstOrDefault();
            return View(model);
        }
        [HttpPost]
        public ActionResult DemandApprove(FormCollection form)
        {
            var _pk = Convert.ToInt64(form["PK_VehicleSharingDemand"]);
            var model = bll.db.VehicleSharingDemands.Where(m => m.PK_VehicleSharingDemand == _pk).FirstOrDefault();
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            try
            {
                if (model.Status == 0)
                {
                    model.VerifiedAt = DateTime.Now;
                    model.FK_AppUser_Approver = CurrentUser.PK_User;

                    model.Status = Convert.ToInt32(form["Status"]);
                    model.AcceptedCount = Convert.ToDouble(form["AcceptedCount"]);
                    model.ApproverNote = form["ApproverNote"];
                    bll.db.SaveChanges();
                    //-Notify Requisitor Mail
                    if (!string.IsNullOrEmpty(model.AppUser.Email))
                    {
                        var Mail_Subject = "Requisitior: Requisition " + (model.Status == 1 ? "Accepted" : "Rejected");
                        var Mail_Body = "Dear Concern<br>";
                        Mail_Body = Mail_Body + "Your requisition is " + (model.Status == 1 ? "approved" : "rejected") + " <br>"
                            + "From: " + (model.Depo != null ? model.Depo.Name : "") + "<br>"
                            + "To: " + (model.Depo1 != null ? model.Depo1.Name : "") + "<br>"
                            + "Wanted quantity: " + model.WantedCount + "<br>"
                            + "On: " + CommonClass.ConvertToDateTimeString(model.PossibleJourneyStartDateTime) + "\n";
                        //SendMail_Single(model.AppUser.Email, Mail_Subject, Mail_Body);
                    }
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Vehicle Sharing is successfully Verified.");
                    return RedirectToAction("DemandIndexBy_Approver");
                }
                else
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", "Vehicle Sharing could not be modified anymore.");
                    return RedirectToAction("DemandIndexBy_Approver");
                }
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult DemandBulkApprove(FormCollection form)
        {
            var redirectUrl = Request.UrlReferrer.ToString();
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            try
            {
                var AcceptedCounts = form["AcceptedCounts"].Split(',');
                var PK_VehicleSharingDemands = form["PK_VehicleSharingDemands"].Split(',');
                for (int i = 0; i < PK_VehicleSharingDemands.Length; i++)
                {
                    if (string.IsNullOrEmpty(PK_VehicleSharingDemands[i]))
                    {
                        break;
                    }
                    var _pk = Convert.ToInt64(PK_VehicleSharingDemands[i]);
                    VehicleSharingDemand model = bll.db.VehicleSharingDemands.Where(m => m.PK_VehicleSharingDemand == _pk).FirstOrDefault();
                    if (model.Status == 0)
                    {
                        model.VerifiedAt = DateTime.Now;
                        model.FK_AppUser_Approver = CurrentUser.PK_User;

                        model.Status = 1;
                        model.AcceptedCount = Convert.ToDouble(AcceptedCounts[i]);
                        //model.ApproverNote = form["ApproverNote"];
                        bll.db.SaveChanges();
                        //-Notify Requisitor Mail
                        if (!string.IsNullOrEmpty(model.AppUser.Email))
                        {
                            var Mail_Subject = "Requisition " + (model.Status == 1 ? "Accepted" : "Rejected");
                            var Mail_Body = "Dear Concern<br>";
                            Mail_Body = Mail_Body + "Your requisition is " + (model.Status == 1 ? "approved" : "rejected") + " <br>"
                                + "From: " + (model.Depo != null ? model.Depo.Name : "") + "<br>"
                                + "To: " + (model.Depo1 != null ? model.Depo1.Name : "") + "<br>"
                                + "Wanted quantity: " + model.WantedCount + "<br>"
                                + "On: " + CommonClass.ConvertToDateTimeString(model.PossibleJourneyStartDateTime) + "\n";
                            //SendMail_Single(model.AppUser.Email, Mail_Subject, Mail_Body);
                        }
                    }
                    else
                    {
                        CreateAlertMessage(AlertMessageType.Warning, "Warning", "Vehicle Sharing could not be modified anymore.");
                    }
                }
                CreateAlertMessage(AlertMessageType.Success, "Success", PK_VehicleSharingDemands.Length + " Vehicle Sharings is successfully Verified.");
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
            }
            return Redirect(redirectUrl);
        }

        //# Sharing
        public ActionResult SharingIndexBy_Assigner(DateTime? StartingDate, DateTime? EndingDate)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            ViewBag.DisplayMessage = bll.db.DisplayMessages.Where(m => m.PK_DisplayMessage == "InterCompanyRequisition/IndexBy_Client").Select(m => m.Message).FirstOrDefault();
            if (StartingDate != null && EndingDate != null)
            {
                var _StartingDate = StartingDate != null ? StartingDate : new DateTime();
                var _EndingDate = EndingDate != null ? EndingDate : new DateTime();
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", _StartingDate);
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", _EndingDate);
                //var list = bll.db.VehicleSharings.AsEnumerable().Where(m => m.IsDeleted != true && m.FK_AppUser_Assigner == CurrentUser.PK_User && m.CreatedAt >= _StartingDate && m.CreatedAt <= _EndingDate).OrderByDescending(m => m.CreatedAt).ToList();
                var list = bll.db.VehicleSharings.ToList();
                return View(list);
            }
            else
            {
                var now = DateTime.Now;
                var today7 = DateTime.Now.Date.AddHours(7);
                if (now > today7)
                {
                    StartingDate = today7;
                    EndingDate = today7.AddDays(1);
                }
                else
                {
                    StartingDate = today7.AddDays(-1);
                    EndingDate = today7;
                }
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", StartingDate);
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", EndingDate);
                var list = new List<VehicleSharing>();
                return View(list);
            }
        }

        public ActionResult VehicleSharingCreate(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.VehicleSharingDemands.AsEnumerable().Where(m => m.PK_VehicleSharingDemand == id).FirstOrDefault();
            return View(model);
        }
        [HttpPost]
        public ActionResult VehicleSharingCreate(FormCollection form)
        {
            var _pk = Convert.ToInt64(form["PK_VehicleSharingDemand"]);
            VehicleSharingDemand demand = bll.db.VehicleSharingDemands.Where(m => m.PK_VehicleSharingDemand == _pk).FirstOrDefault();
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            try
            {
                if (demand.Status == 1)
                {
                    //# create sharing
                    var sharing = new VehicleSharing();
                    sharing.CreatedAt = DateTime.Now;
                    sharing.FK_AppUser_Assigner = CurrentUser.PK_User;
                    sharing.Status = 0;
                    sharing.LoadedOrEmpty = demand.LoadedOrEmpty;
                    sharing.WantedCount = demand.WantedCount;
                    sharing.PossibleJourneyStartDateTime = demand.PossibleJourneyStartDateTime;
                    sharing.AcceptedCount = demand.AcceptedCount;
                    sharing.FK_AppUser_Client = demand.FK_AppUser_Client;
                    sharing.FK_ReferenceDepo = demand.FK_ReferenceDepo;
                    sharing.FK_AppUser_Approver = demand.FK_AppUser_Approver;
                    sharing.FK_Depo_From = demand.FK_Depo_From;
                    sharing.StartingLocation = demand.StartingLocation;
                    sharing.FK_Depo_To = demand.FK_Depo_To;
                    sharing.FinishingLocation = demand.FinishingLocation;
                    sharing.DistanceGoogle = demand.DistanceGoogle;
                    sharing.FK_RequisitionVehicleType = demand.FK_RequisitionVehicleType;
                    sharing.VehicleType = demand.VehicleType;
                    sharing.VehicleCapacity = demand.VehicleCapacity;

                    //sharing.TrackingID = demand.TrackingID.Replace('D', 'S');
                    var TrackingPattern = demand.Depo.Code + "-" + demand.Depo1.Code + "-"
                        + DateTime.Now.Month.ToString("00") + "-"
                        + DateTime.Now.ToString("yy") + "-";
                    var existingCount = bll.db.VehicleSharings.Where(m => m.TrackingID.Contains(TrackingPattern)).Count();
                    sharing.TrackingID = TrackingPattern + ((existingCount + 1).ToString("00000"));



                    bll.db.VehicleSharings.Add(sharing);
                    bll.db.SaveChanges();

                    demand.Status = 2;
                    demand.IsHeadDemand = true;
                    demand.SharedAt = sharing.CreatedAt;
                    demand.FK_VehicleSharing = sharing.PK_VehicleSharing;
                    bll.db.SaveChanges();

                    //#Inernal Trip
                    var newInternalTripInfo = form["newInternalTripInfo"];
                    if (newInternalTripInfo != "")
                    {
                        var InternalVehicles = newInternalTripInfo.Split('#').ToList().Select(m => new { FK_Vehicle = m.Split('*')[0], FK_Driver = m.Split('*')[1] }).ToList();
                        var unassignedVehicles = "";
                        if (InternalVehicles.Count() > 0)
                        {
                            foreach (var InternalVehicle in InternalVehicles)
                            {
                                if (InternalVehicle.FK_Vehicle == "" || InternalVehicle.FK_Driver == "")
                                {
                                    continue;
                                }
                                var _pk_vheicle = Guid.Parse(InternalVehicle.FK_Vehicle);
                                var _vehicle = bll.db.Vehicles.Where(m => m.PK_Vehicle == _pk_vheicle).FirstOrDefault();
                                var _pk_driver = Guid.Parse(InternalVehicle.FK_Driver);
                                var _driver = bll.db.AppUsers.Where(m => m.PK_User == _pk_driver).FirstOrDefault();
                                if ((_driver.FK_VehicleSharingInternalTrip_Pending == null) &&
                                    ((_vehicle.FK_VehicleSharingInternalTrip_Pending == null && _vehicle.FK_VehicleSharingInternalTrip_Current == null)
                                    || (_vehicle.LocationInOrOut == true && _vehicle.FK_LocationInOut == demand.FK_Depo_From && _vehicle.FK_VehicleSharingInternalTrip_Pending == null && _vehicle.FK_VehicleSharingInternalTrip_Current == null))
                                    || (demand.FK_Depo_From == _vehicle.VehicleSharingInternalTrip1.VehicleSharing.FK_Depo_To && _vehicle.FK_VehicleSharingInternalTrip_Pending == null)
                                    )
                                {
                                    demand.Status = 3;
                                    var internalTrip = new VehicleSharingInternalTrip();
                                    internalTrip.AssingedAt = DateTime.Now;
                                    internalTrip.FK_VehicleSharing = sharing.PK_VehicleSharing;
                                    internalTrip.FK_Vehicle = Guid.Parse(InternalVehicle.FK_Vehicle);
                                    internalTrip.FK_AppUser_Driver = Guid.Parse(InternalVehicle.FK_Driver);
                                    internalTrip.FK_AppUser_Assigner = CurrentUser.PK_User;
                                    internalTrip.IsDeleted = false;
                                    internalTrip.IsTest = true;
                                    internalTrip.AssingedAt = DateTime.Now;
                                    internalTrip.StatusText = InternalTripStatus.Assigned;
                                    if (_vehicle.LocationInOrOut == true && _vehicle.FK_LocationInOut == demand.FK_Depo_From && _vehicle.FK_VehicleSharingInternalTrip_Pending == null && _vehicle.FK_VehicleSharingInternalTrip_Current == null)
                                    {
                                        internalTrip.StatusText = InternalTripStatus.EnteredStartingLocation;
                                    }

                                    bll.db.VehicleSharingInternalTrips.Add(internalTrip);
                                    bll.db.SaveChanges();
                                    _vehicle.FK_VehicleSharingInternalTrip_Pending = internalTrip.PK_VehicleSharingInternalTrip;
                                    _driver.FK_VehicleSharingInternalTrip_Pending = internalTrip.PK_VehicleSharingInternalTrip;
                                    bll.db.SaveChanges();

                                    //-Notify Driver Firebase
                                    var vehicle = bll.db.Vehicles.Where(m => m.PK_Vehicle == internalTrip.FK_Vehicle).FirstOrDefault();
                                    if (!string.IsNullOrEmpty(vehicle.FID))
                                    {
                                        //var title = "Driver: New Internal Trip #" + bll.db.VehicleSharings.Where(m => m.PK_VehicleSharing == internalTrip.FK_VehicleSharing).FirstOrDefault().TrackingID + " Created";
                                        var title = "সম্মনিত চালক, নতুন যাত্রার (#" + bll.db.VehicleSharings.Where(m => m.PK_VehicleSharing == internalTrip.FK_VehicleSharing).FirstOrDefault().TrackingID + ") তথ্য দেখুন।";
                                        var message = "Dear Concern \n";
                                        message = message + "Trip Detail" + "\n"
                                            + "Vehicle: " + internalTrip.Vehicle.RegistrationNumber + "\n"
                                            + "From: " + internalTrip.VehicleSharing.Depo.Name + "\n"
                                            + "To: " + internalTrip.VehicleSharing.Depo1.Name + "\n"
                                            + "On: " + internalTrip.VehicleSharing.PossibleJourneyStartDateTime + "\n";
                                        SendFCM_Notification_Single_New(vehicle.FID, title, message, internalTrip.PK_VehicleSharingInternalTrip.ToString(), "VehicleSharingInternalTrip");
                                    }

                                    //-Notify Requisitor Mail
                                    foreach (var _demand in sharing.VehicleSharingDemands)
                                    {
                                        if (!string.IsNullOrEmpty(_demand.AppUser.Email))
                                        {
                                            var Mail_Subject = "Requisitior: New Internal Trip Created";
                                            var Mail_Body = "Dear Concern<br>";
                                            Mail_Body = Mail_Body + "Trip Detail" + " <br>"
                                                + "Vehicle: " + internalTrip.Vehicle.RegistrationNumber + "<br>"
                                                + "From: " + internalTrip.VehicleSharing.Depo.Name + "<br>"
                                                + "To: " + internalTrip.VehicleSharing.Depo1.Name + "<br>"
                                                + "On: " + internalTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
                                            //SendMail_Single(_demand.AppUser.Email, Mail_Subject, Mail_Body);
                                        }
                                    }

                                }
                                else
                                {
                                    unassignedVehicles = unassignedVehicles + _vehicle.RegistrationNumber + ",";
                                }
                            }
                        }
                    }

                    //#External Trip
                    var IsExternalAdvertised = form["IsExternalAdvertised"];
                    if (IsExternalAdvertised != null)
                    {
                        sharing.IsExternalAdvertised = true;
                        sharing.ExternalWantedCount = Convert.ToDouble(form["ExternalWantedCount"]);
                        sharing.KeepBidOpenUntil = DateTime.ParseExact(form["KeepBidOpenUntil"], "yyyy-MM-dd h:mm tt", CultureInfo.InvariantCulture);
                        var transportAgents = bll.db.VehicleSharingAgentMappings.Where(m => m.FK_AppUser_InternalAgent == CurrentUser.PK_User).Select(m => m.AppUser).ToList();
                        foreach (var ta in transportAgents)
                        {
                            var bid = new VehicleSharingBidding()
                            {
                                FK_VehicleSharing = sharing.PK_VehicleSharing,
                                FK_RequisitionAgent_Bidder = ta.PK_User,
                                StatusText = VehicleSharingBiddingStatus.Created,
                                CreatedAt = DateTime.Now,
                                ManagableQuantity = 0
                            };
                            bll.db.VehicleSharingBiddings.Add(bid);
                            bll.db.SaveChanges();

                            //-Notify External Transport Agent Firebase
                            if (!string.IsNullOrEmpty(ta.FID))
                            {
                                //var title = "External Transport Agent: New Demand #" + bll.db.VehicleSharings.Where(m => m.PK_VehicleSharing == demand.FK_VehicleSharing).FirstOrDefault().TrackingID + " of Vehicle";
                                var title = "সম্মনিত এজেন্ট, : নতুন ডিমান্ড #" + bll.db.VehicleSharings.Where(m => m.PK_VehicleSharing == demand.FK_VehicleSharing).FirstOrDefault().TrackingID + " দেখুন।";
                                var message = "Dear Concern \n";
                                message = message + "Trip Detail" + "\n"
                                    + "Vehicle Type: " + demand.RequisitionVehicleType.Title_English + "\n"
                                    + "From: " + demand.Depo.Name + "\n"
                                    + "To: " + demand.Depo1.Name + "\n"
                                    + "On: " + demand.PossibleJourneyStartDateTime + "\n";
                                SendFCM_Notification_Single_New(ta.FID, title, message, bid.PK_VehicleSharingBidding.ToString(), "external");
                            }
                        }
                    }
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Vehicle Sharing #" + sharing.TrackingID + " is successfully created");
                    return RedirectToAction("ManageSharing", new { id = sharing.PK_VehicleSharing });
                }
                else
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", "Vehicle Sharing could not be modified anymore.");
                    return RedirectToAction("DemandIndexBy_Approver");
                }
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
            }
            return RedirectToAction("DemandIndexBy_Approver");
        }

        public ActionResult ManageSharing(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.VehicleSharings.AsEnumerable().Where(m => m.PK_VehicleSharing == id).FirstOrDefault();
            var stillRequiredFromExternal = model.ExternalWantedCount - model.VehicleSharingInternalTrips.Count() - model.VehicleSharingBiddings.Where(m => m.StatusText == VehicleSharingBiddingStatus.Approved).Select(m => m.ApprovedQuantity).Sum();
            if (stillRequiredFromExternal == null)
            {
                stillRequiredFromExternal = 0;
            }
            ViewBag.stillRequiredFromExternal = stillRequiredFromExternal;
            return View(model);
        }
        [HttpPost]
        public ActionResult ManageSharing(FormCollection form)
        {


            var _pk = Convert.ToInt64(form["PK_VehicleSharing"]);
            VehicleSharing sharing = bll.db.VehicleSharings.Where(m => m.PK_VehicleSharing == _pk).FirstOrDefault();
            var demand = sharing.VehicleSharingDemands.Where(m => m.IsHeadDemand == true).FirstOrDefault();
            sharing.UpdatedAt = DateTime.Now;
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            try
            {
                if (sharing.Status == 0)
                {
                    //# Demand
                    var newDemandInfo = form["newDemandInfo"];
                    if (newDemandInfo != "")
                    {
                        var unassignedDemands = "";
                        var newDemands = newDemandInfo.Split(',').ToList().Select(m => new { PK_VehicleSharingDemand = m.Split('*')[0], TrackingID = m.Split('*')[1] }).ToList();
                        if (newDemands.Count() > 0)
                        {
                            foreach (var newDemand in newDemands)
                            {
                                var _PK_VehicleSharingDemand = Convert.ToInt64(newDemand.PK_VehicleSharingDemand);
                                var _vehicleSharingDemand = bll.db.VehicleSharingDemands.Where(m => m.PK_VehicleSharingDemand == _PK_VehicleSharingDemand).FirstOrDefault();
                                if ((_vehicleSharingDemand.Status == 1))
                                {
                                    if (sharing.VehicleSharingInternalTrips.Any())
                                    {
                                        _vehicleSharingDemand.Status = 3;
                                    }
                                    else if (sharing.VehicleSharingBiddings.SelectMany(m => m.VehicleSharingExternalTrips).Any())
                                    {
                                        _vehicleSharingDemand.Status = 3;
                                    }
                                    else
                                    {
                                        _vehicleSharingDemand.Status = 2;
                                    }
                                    _vehicleSharingDemand.FK_VehicleSharing = sharing.PK_VehicleSharing;
                                    _vehicleSharingDemand.SharedAt = DateTime.Now;
                                    bll.db.SaveChanges();

                                    //-Notify Requisitor Mail
                                    var _driverMail = bll.db.AppUsers.Where(m => m.PK_User == _vehicleSharingDemand.FK_AppUser_Client).FirstOrDefault().Email;
                                    if (!string.IsNullOrEmpty(_driverMail))
                                    {
                                        var Mail_Subject = "Raiser: Vehicle Sharing Is Linked With Your Demand";
                                        var Mail_Body = "Dear Concern<br>";
                                        Mail_Body = Mail_Body + "Sharing Detail" + " <br>"
                                            + "Demand Tracking ID: " + _vehicleSharingDemand.TrackingID + "<br>"
                                            + "Sharing Tracking ID: " + sharing.TrackingID + "<br>" + "\n"
                                            + " <a href='http://localhost:49798/VehicleSharing/VehicleSharingDetail?PK_VehicleSharing=" + sharing.PK_VehicleSharing + "'>Show Detail</a> " + "<br>" + "\n"; ;
                                        //SendMail_Single(_driverMail, Mail_Subject, Mail_Body);
                                    }
                                }
                                else
                                {
                                    unassignedDemands = unassignedDemands + _vehicleSharingDemand.TrackingID + ",";
                                }
                            }
                        }
                    }

                    //#Inernal Trip
                    var newInternalTripInfo = form["newInternalTripInfo"];
                    if (newInternalTripInfo != "")
                    {
                        var unassignedVehicles = "";
                        var InternalVehicles = newInternalTripInfo.Split('#').ToList().Select(m => new { FK_Vehicle = m.Split('*')[0], FK_Driver = m.Split('*')[1] }).ToList();
                        if (InternalVehicles.Count() > 0)
                        {
                            foreach (var InternalVehicle in InternalVehicles)
                            {
                                if (InternalVehicle.FK_Vehicle == "" || InternalVehicle.FK_Driver == "")
                                {
                                    continue;
                                }
                                var _pk_vheicle = Guid.Parse(InternalVehicle.FK_Vehicle);
                                var _vehicle = bll.db.Vehicles.Where(m => m.PK_Vehicle == _pk_vheicle).FirstOrDefault();
                                var _pk_driver = Guid.Parse(InternalVehicle.FK_Driver);
                                var _driver = bll.db.AppUsers.Where(m => m.PK_User == _pk_driver).FirstOrDefault();
                                if ((_driver.FK_VehicleSharingInternalTrip_Pending == null) &&
                                    (
                                    (_vehicle.FK_VehicleSharingInternalTrip_Pending == null && _vehicle.FK_VehicleSharingInternalTrip_Current == null)
                                    || (_vehicle.LocationInOrOut == true && _vehicle.FK_LocationInOut == demand.FK_Depo_From && _vehicle.FK_VehicleSharingInternalTrip_Pending == null && _vehicle.FK_VehicleSharingInternalTrip_Current == null))
                                    || (demand.FK_Depo_From == _vehicle.VehicleSharingInternalTrip1.VehicleSharing.FK_Depo_To && _vehicle.FK_VehicleSharingInternalTrip_Pending == null)
                                    )
                                {
                                    var internalTrip = new VehicleSharingInternalTrip();
                                    internalTrip.AssingedAt = DateTime.Now;
                                    internalTrip.FK_VehicleSharing = sharing.PK_VehicleSharing;
                                    internalTrip.FK_Vehicle = Guid.Parse(InternalVehicle.FK_Vehicle);
                                    internalTrip.FK_AppUser_Driver = Guid.Parse(InternalVehicle.FK_Driver);
                                    internalTrip.FK_AppUser_Assigner = CurrentUser.PK_User;
                                    internalTrip.IsDeleted = false;
                                    internalTrip.IsTest = true;
                                    internalTrip.AssingedAt = DateTime.Now;
                                    internalTrip.StatusText = InternalTripStatus.Assigned;
                                    if (_vehicle.LocationInOrOut == true && _vehicle.FK_LocationInOut == demand.FK_Depo_From && _vehicle.FK_VehicleSharingInternalTrip_Pending == null && _vehicle.FK_VehicleSharingInternalTrip_Current == null)
                                    {
                                        internalTrip.StatusText = InternalTripStatus.EnteredStartingLocation;
                                    }
                                    bll.db.VehicleSharingInternalTrips.Add(internalTrip);
                                    bll.db.SaveChanges();
                                    _vehicle.FK_VehicleSharingInternalTrip_Pending = internalTrip.PK_VehicleSharingInternalTrip;
                                    _driver.FK_VehicleSharingInternalTrip_Pending = internalTrip.PK_VehicleSharingInternalTrip;
                                    bll.db.SaveChanges();

                                    //-Notify Driver Firebase
                                    var vehicle = bll.db.Vehicles.Where(m => m.PK_Vehicle == internalTrip.FK_Vehicle).FirstOrDefault();
                                    if (!string.IsNullOrEmpty(vehicle.FID))
                                    {
                                        //var title = "Driver: New Internal Trip #" + bll.db.VehicleSharings.Where(m => m.PK_VehicleSharing == internalTrip.FK_VehicleSharing).FirstOrDefault().TrackingID + " Created";
                                        var title = "সম্মনিত চালক, নতুন যাত্রার (#" + bll.db.VehicleSharings.Where(m => m.PK_VehicleSharing == internalTrip.FK_VehicleSharing).FirstOrDefault().TrackingID + ") তথ্য দেখুন।";
                                        var message = "Dear Concern \n";
                                        message = message + "Trip Detail" + "\n"
                                            + "Vehicle: " + internalTrip.Vehicle.RegistrationNumber + "\n"
                                            + "From: " + internalTrip.VehicleSharing.Depo.Name + "\n"
                                            + "To: " + internalTrip.VehicleSharing.Depo1.Name + "\n"
                                            + "On: " + internalTrip.VehicleSharing.PossibleJourneyStartDateTime + "\n";
                                        SendFCM_Notification_Single_New(vehicle.FID, title, message, internalTrip.PK_VehicleSharingInternalTrip.ToString(), "VehicleSharingInternalTrip");
                                    }

                                    //-Notify Requisitor Mail
                                    foreach (var _demand in sharing.VehicleSharingDemands)
                                    {
                                        _demand.Status = 3;
                                        if (!string.IsNullOrEmpty(_demand.AppUser.Email))
                                        {
                                            var Mail_Subject = "Requisitior: New Internal Trip #" + internalTrip.VehicleSharing.TrackingID + " Created";
                                            var Mail_Body = "Dear Concern<br>";
                                            Mail_Body = Mail_Body + "Trip Detail" + " <br>"
                                                + "Vehicle: " + internalTrip.Vehicle.RegistrationNumber + "<br>"
                                                + "From: " + internalTrip.VehicleSharing.Depo.Name + "<br>"
                                                + "To: " + internalTrip.VehicleSharing.Depo1.Name + "<br>"
                                                + "On: " + internalTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
                                            //SendMail_Single(_demand.AppUser.Email, Mail_Subject, Mail_Body);
                                        }
                                    }
                                    bll.db.SaveChanges();
                                }
                                else
                                {
                                    unassignedVehicles = unassignedVehicles + _vehicle.RegistrationNumber + ",";
                                }
                            }
                        }
                    }

                    //#External Trip
                    var IsExternalAdvertised = form["IsExternalAdvertised"];
                    if (IsExternalAdvertised != null)
                    {
                        sharing.IsExternalAdvertised = true;
                        sharing.ExternalWantedCount = Convert.ToDouble(form["ExternalWantedCount"]);
                        sharing.KeepBidOpenUntil = DateTime.ParseExact(form["KeepBidOpenUntil"], "yyyy-MM-dd h:mm tt", CultureInfo.InvariantCulture);
                        var transportAgents = bll.db.VehicleSharingAgentMappings.Where(m => m.FK_AppUser_InternalAgent == CurrentUser.PK_User).Select(m => m.AppUser).ToList();
                        foreach (var ta in transportAgents)
                        {
                            var bid = new VehicleSharingBidding()
                            {
                                FK_VehicleSharing = sharing.PK_VehicleSharing,
                                FK_RequisitionAgent_Bidder = ta.PK_User,
                                StatusText = VehicleSharingBiddingStatus.Created,
                                CreatedAt = DateTime.Now,
                                ManagableQuantity = 0
                            };
                            bll.db.VehicleSharingBiddings.Add(bid);
                            bll.db.SaveChanges();

                            //-Notify External Transport Agent Firebase
                            if (!string.IsNullOrEmpty(ta.FID))
                            {
                                //var title = "External Transport Agent: New Demand #" + demand.VehicleSharing.TrackingID + " of Vehicle";
                                var title = "সম্মনিত এজেন্ট, : নতুন ডিমান্ড #" + sharing.TrackingID + " দেখুন।";
                                var message = "Dear Concern \n";
                                message = message + "Trip Detail" + "\n"
                                    + "Vehicle Type: " + sharing.RequisitionVehicleType.Title_English + "\n"
                                    + "From: " + sharing.Depo.Name + " " + sharing.StartingLocation + "\n"
                                    + "To: " + sharing.Depo1.Name + " " + sharing.FinishingLocation + "\n"
                                    + "On: " + sharing.PossibleJourneyStartDateTime + "\n";
                                SendFCM_Notification_Single_New(ta.FID, title, message, bid.PK_VehicleSharingBidding.ToString(), "external");
                            }
                        }
                    }
                    else
                    {
                        var ExternalWantedCountNew = Convert.ToInt32(form["ExternalWantedCountNew"]);
                        sharing.ExternalWantedCount = ExternalWantedCountNew;
                        bll.db.SaveChanges();
                        var stillRequiredFromExternal = sharing.ExternalWantedCount - sharing.VehicleSharingBiddings.Where(m => m.StatusText == VehicleSharingBiddingStatus.Approved).Select(m => m.ApprovedQuantity).Sum();

                        foreach (var bid in sharing.VehicleSharingBiddings.Where(m => m.StatusText == VehicleSharingBiddingStatus.Bidded).OrderBy(m => m.PricePerQuantity).ThenBy(m => m.BiddedAt))
                        {
                            var vehicleSharingBidding_ApprovedQuantity = form["vehicleSharingBidding_ApprovedQuantity_" + bid.PK_VehicleSharingBidding];
                            if (vehicleSharingBidding_ApprovedQuantity != "" && vehicleSharingBidding_ApprovedQuantity != "0")
                            {
                                bid.StatusText = VehicleSharingBiddingStatus.Approved;
                                bid.ApprovedQuantity = Convert.ToInt32(form["vehicleSharingBidding_ApprovedQuantity_" + bid.PK_VehicleSharingBidding]);
                                bid.ApprovalNote = form["vehicleSharingBidding_ApprovalNote_" + bid.PK_VehicleSharingBidding];

                                stillRequiredFromExternal = stillRequiredFromExternal - bid.ApprovedQuantity;

                                if (stillRequiredFromExternal > 0 && bid.ManagableQuantity > bid.ApprovedQuantity)
                                {
                                    var skipped = stillRequiredFromExternal < bid.ManagableQuantity ? stillRequiredFromExternal - bid.ApprovedQuantity : bid.ManagableQuantity - bid.ApprovedQuantity;
                                    if (skipped > 0)
                                    {
                                        //bid.ApprovalMessage = "Required : " + stillRequiredFromExternal + ", Managable : " + bid.ManagableQuantity + ", Taken : " + bid.ApprovedQuantity + ", Skipped: " + skipped;
                                        bid.ApprovalMessage = "Managable/Taken/Skipped: " + bid.ManagableQuantity + "/" + bid.ApprovedQuantity + "/" + skipped;
                                    }
                                }

                                var minTime = DateTime.Now.AddMinutes(-1);
                                var lessPricedBid = sharing.VehicleSharingBiddings.Where(m => m.PK_VehicleSharingBidding != bid.PK_VehicleSharingBidding && m.StatusText == VehicleSharingBiddingStatus.Bidded && m.PricePerQuantity < bid.PricePerQuantity && m.BiddedAt < minTime).OrderBy(m => m.PricePerQuantity).FirstOrDefault();
                                if (lessPricedBid != null)
                                {
                                    bid.FK_VehicleSharingBidding_LessPriced = lessPricedBid.PK_VehicleSharingBidding;
                                }

                                //-Notify External Transport Agent Firebase
                                var ta = bll.db.AppUsers.Where(m => m.PK_User == bid.FK_RequisitionAgent_Bidder).FirstOrDefault();
                                if (!string.IsNullOrEmpty(ta.FID))
                                {
                                    //var title = "External Transport Agent: Bidding Approved #" + demand.VehicleSharing.TrackingID;
                                    var title = "সম্মনিত এজেন্ট, : বিড #" + sharing.TrackingID + " গ্রহণ হয়েছে।";

                                    var message = "Dear Concern \n";
                                    message = message + "Trip Detail" + "\n"
                                        + "Sharing Id: " + sharing.TrackingID + "\n"
                                        + "Vehicle Type: " + sharing.RequisitionVehicleType.Title_English + "\n"
                                        + "From: " + sharing.Depo.Name + " " + sharing.StartingLocation + "\n"
                                        + "To: " + sharing.Depo1.Name + " " + sharing.FinishingLocation + "\n"
                                        + "On: " + sharing.PossibleJourneyStartDateTime + "\n";
                                    SendFCM_Notification_Single_New(ta.FID, title, message, bid.PK_VehicleSharingBidding.ToString(), "external");
                                }

                                //-Notify Requisitor Mail
                                foreach (var _demand in sharing.VehicleSharingDemands)
                                {
                                    if (!string.IsNullOrEmpty(_demand.AppUser.Email))
                                    {
                                        var Mail_Subject = "Requisitior: New External Trip Created #" + sharing.TrackingID;
                                        var Mail_Body = "Dear Concern<br>";
                                        Mail_Body = Mail_Body + "Trip Detail" + " <br>"
                                            + "Transport Agent: " + ta.FullName + "\n" + ta.ContactNumber + "<br>"
                                            + "From: " + _demand.Depo.Name + "<br>"
                                            + "To: " + _demand.Depo1.Name + "<br>"
                                            + "On: " + _demand.PossibleJourneyStartDateTime + "<br>" + "\n";
                                        //SendMail_Single(_demand.AppUser.Email, Mail_Subject, Mail_Body);
                                    }
                                }
                            }
                        }
                        bll.db.SaveChanges();
                    }

                    //# Finalize Sharing
                    var IsLockedSharing = form["IsLockedSharing"];
                    if (IsLockedSharing != null)
                    {
                        sharing.Status = 1;
                        foreach (var bid in sharing.VehicleSharingBiddings.Where(m => m.StatusText == VehicleSharingBiddingStatus.Bidded))
                        {
                            bid.StatusText = VehicleSharingBiddingStatus.CancelledByApprover;
                            //-Notify External Transport Agent Firebase
                            var ta = bll.db.AppUsers.Where(m => m.PK_User == bid.FK_RequisitionAgent_Bidder).FirstOrDefault();
                            if (!string.IsNullOrEmpty(ta.FID))
                            {
                                //var title = "External Transport Agent: Bidding Cancelled #" + demand.VehicleSharing.TrackingID;
                                var title = "সম্মনিত এজেন্ট, : বিড #" + sharing.TrackingID + " গ্রহণ করতে ব্যার্থ হয়েছি। \n পরবর্তীতে বিড করার অনুরোধ রইল।";
                                var message = "Dear Concern \n";
                                message = message + "Trip Detail" + "\n"
                                    + "Sharing Id: " + sharing.TrackingID + "\n"
                                    + "Vehicle Type: " + sharing.RequisitionVehicleType.Title_English + "\n"
                                    + "From: " + sharing.Depo.Name + " " + sharing.StartingLocation + "\n"
                                    + "To: " + sharing.Depo1.Name + " " + sharing.FinishingLocation + "\n"
                                    + "On: " + sharing.PossibleJourneyStartDateTime + "\n";
                                SendFCM_Notification_Single_New(ta.FID, title, message, bid.PK_VehicleSharingBidding.ToString(), "external");
                            }
                        }
                        bll.db.SaveChanges();
                    }
                    CreateAlertMessage(AlertMessageType.Success, "Warning", "Vehicle Sharing is successfully modified.");
                    return RedirectToAction("ManageSharing", new { id = sharing.PK_VehicleSharing });
                }
                else
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", "Vehicle Sharing could not be modified anymore.");
                    return RedirectToAction("ManageSharing", new { id = sharing.PK_VehicleSharing });
                }
            }
            catch (Exception e)
            {
                CreateAlertMessage(AlertMessageType.Danger, "Warning", "Internal Error Ocuured.");
                return RedirectToAction("ManageSharing", new { id = sharing.PK_VehicleSharing });
            }
        }

        public ActionResult ViewSharing(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.VehicleSharings.AsEnumerable().Where(m => m.PK_VehicleSharing == id).FirstOrDefault();
            return View(model);
        }

        //# VehicleSharingInternalTrip
        //#Documentation
        /* 
         * Assigner = AppUser
         * Driver = AppUser4
         * BillCreator = AppUser2
         * BillApprover = AppUser1
         * BillPayer = AppUser3
         */
        public ActionResult VehicleSharingInternalTripIndex(DateTime? StartingDate, DateTime? EndingDate)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            ViewBag.DisplayMessage = bll.db.DisplayMessages.Where(m => m.PK_DisplayMessage == "InterCompanyRequisition/IndexBy_Client").Select(m => m.Message).FirstOrDefault();
            if (StartingDate != null && EndingDate != null)
            {
                var _StartingDate = StartingDate != null ? StartingDate : new DateTime();
                var _EndingDate = EndingDate != null ? EndingDate : new DateTime();
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", _StartingDate);
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", _EndingDate);
                var list = bll.db.VehicleSharingInternalTrips.AsEnumerable()/*.Where(m => m.IsDeleted != true && m.FK_AppUser_Assigner == CurrentUser.PK_User && m.AssingedAt >= _StartingDate && m.AssingedAt <= _EndingDate)*/.OrderByDescending(m => m.AssingedAt).ToList();
                return View(list);
            }
            else
            {
                var now = DateTime.Now;
                var today7 = DateTime.Now.Date.AddHours(7);
                if (now > today7)
                {
                    StartingDate = today7;
                    EndingDate = today7.AddDays(1);
                }
                else
                {
                    StartingDate = today7.AddDays(-1);
                    EndingDate = today7;
                }
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", StartingDate);
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", EndingDate);
                var list = new List<VehicleSharingInternalTrip>();
                return View(list);
            }
        }

        public ActionResult VehicleSharingInternalTripIndex_TPTView(DateTime? StartingDate, DateTime? EndingDate)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            ViewBag.DisplayMessage = bll.db.DisplayMessages.Where(m => m.PK_DisplayMessage == "InterCompanyRequisition/IndexBy_Client").Select(m => m.Message).FirstOrDefault();
            if (StartingDate != null && EndingDate != null)
            {
                var _StartingDate = StartingDate != null ? StartingDate : new DateTime();
                var _EndingDate = EndingDate != null ? EndingDate : new DateTime();
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", _StartingDate);
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", _EndingDate);
                var list = bll.db.VehicleSharingInternalTrips.AsEnumerable()/*.Where(m => m.IsDeleted != true && m.FK_AppUser_Assigner == CurrentUser.PK_User && m.AssingedAt >= _StartingDate && m.AssingedAt <= _EndingDate)*/.OrderByDescending(m => m.AssingedAt).ToList();
                return View(list);
            }
            else
            {
                var now = DateTime.Now;
                var today7 = DateTime.Now.Date.AddHours(7);
                if (now > today7)
                {
                    StartingDate = today7;
                    EndingDate = today7.AddDays(1);
                }
                else
                {
                    StartingDate = today7.AddDays(-1);
                    EndingDate = today7;
                }
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", StartingDate);
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", EndingDate);
                var list = new List<VehicleSharingInternalTrip>();
                return View(list);
            }
        }

        public ActionResult VehicleSharingInternalTripView(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.VehicleSharingInternalTrips.AsEnumerable().Where(m => m.PK_VehicleSharingInternalTrip == id).FirstOrDefault();
            return View(model);
        }

        public ActionResult VehicleSharingInternalTripCreateBill(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.VehicleSharingInternalTrips.AsEnumerable().Where(m => m.PK_VehicleSharingInternalTrip == id).FirstOrDefault();
            var from_to = model.VehicleSharing.VehicleSharingDemands.Where(m => m.IsHeadDemand == true).Select(m => new { m.FK_Depo_From, m.FK_Depo_To }).FirstOrDefault();
            RouteChart routeChart = bll.db.RouteCharts.Where(m => (m.FK_Depo1 == from_to.FK_Depo_From && m.FK_Depo2 == from_to.FK_Depo_To) || (m.FK_Depo1 == from_to.FK_Depo_To && m.FK_Depo2 == from_to.FK_Depo_From)).FirstOrDefault();
            if (routeChart == null)
            {
                routeChart = new RouteChart();
                routeChart.Distance = 0;
                routeChart.DriversMoney = 0;
                routeChart.HelpersMoney = 0;
            }
            ViewBag.RouteChart = routeChart;
            return View(model);
        }
        [HttpPost]
        public ActionResult VehicleSharingInternalTripCreateBill(VehicleSharingInternalTrip model)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            try
            {

                var db_model = bll.db.VehicleSharingInternalTrips.Where(m => m.PK_VehicleSharingInternalTrip == model.PK_VehicleSharingInternalTrip && m.IsDeleted != true).FirstOrDefault();
                if (db_model == null)
                {
                    return HttpNotFound();
                }
                db_model.StatusText = InternalTripStatus.CreatedBill;
                db_model.BillCreatedAt = DateTime.Now;
                db_model.FK_AppUser_BillCreator = CurrentUser.PK_User;

                db_model.DistanceGoogle = model.DistanceGoogle != null ? model.DistanceGoogle : 0;
                db_model.DistanceRouteChart = model.DistanceRouteChart != null ? model.DistanceRouteChart : 0;
                db_model.DistanceManual = model.DistanceManual != null ? model.DistanceRouteChart : 0;

                db_model.Distance_Loaded = model.Distance_Loaded != null ? model.Distance_Loaded : 0;
                db_model.KPL_Loaded = model.KPL_Loaded != null ? model.KPL_Loaded : 0;

                db_model.Distance_Loaded_Plastic = model.Distance_Loaded_Plastic != null ? model.Distance_Loaded_Plastic : 0;
                db_model.KPL_Loaded_Plastic = model.KPL_Loaded_Plastic != null ? model.KPL_Loaded_Plastic : 0;

                db_model.Distance_Empty = model.Distance_Empty != null ? model.Distance_Empty : 0;
                db_model.KPL_Empty = model.KPL_Empty != null ? model.KPL_Empty : 0;

                db_model.Distance_InterCity = model.Distance_InterCity != null ? model.Distance_InterCity : 0;
                db_model.KPL_InterCity = model.KPL_InterCity != null ? model.KPL_InterCity : 0;

                db_model.Distance_InterCHT = model.Distance_InterCHT != null ? model.Distance_InterCHT : 0;
                db_model.KPL_InterCHT = model.KPL_InterCHT != null ? model.KPL_InterCHT : 0;

                db_model.Distance_Hill = model.Distance_Hill != null ? model.Distance_Hill : 0;
                db_model.KPL_Hill = model.KPL_Hill != null ? model.KPL_Hill : 0;

                db_model.Distance_OnlyMover = model.Distance_OnlyMover != null ? model.Distance_OnlyMover : 0;
                db_model.KPL_OnlyMover = model.KPL_OnlyMover != null ? model.KPL_OnlyMover : 0;

                db_model.Distance_Loaded_8_To_12_Tons = model.Distance_Loaded_8_To_12_Tons != null ? model.Distance_Loaded_8_To_12_Tons : 0;
                db_model.KPL_Loaded_8_To_12_Tons = model.KPL_Loaded_8_To_12_Tons != null ? model.KPL_Loaded_8_To_12_Tons : 0;

                db_model.Distance_Loaded_12_To_25_Tons = model.Distance_Loaded_12_To_25_Tons != null ? model.Distance_Loaded_12_To_25_Tons : 0;
                db_model.KPL_Loaded_12_To_25_Tons = model.KPL_Loaded_12_To_25_Tons != null ? model.KPL_Loaded_12_To_25_Tons : 0;

                db_model.DistanceTrip = model.DistanceTrip != null ? model.DistanceTrip : 0;
                db_model.FuelConsumedLitre = model.FuelConsumedLitre != null ? model.FuelConsumedLitre : 0;
                db_model.FuelPricePerLitre = model.FuelPricePerLitre != null ? model.FuelPricePerLitre : 0;
                db_model.FuelExpence = model.FuelExpence != null ? model.FuelExpence : 0;

                db_model.FuelExpenceGivenCashOrOil = model.FuelExpenceGivenCashOrOil;

                db_model.DriversMoney = model.DriversMoney != null ? model.DriversMoney : 0;
                db_model.HelpersMoney = model.HelpersMoney != null ? model.HelpersMoney : 0;
                db_model.BridgeTollFerryCharge = model.BridgeTollFerryCharge != null ? model.BridgeTollFerryCharge : 0;
                db_model.LoadingCost = model.LoadingCost != null ? model.LoadingCost : 0;
                db_model.UnloadingCost = model.UnloadingCost != null ? model.UnloadingCost : 0;
                db_model.LaborCharge = model.LaborCharge != null ? model.LaborCharge : 0;
                db_model.EntertainmentCCharge = model.EntertainmentCCharge != null ? model.EntertainmentCCharge : 0;
                db_model.ParkingCharge = model.ParkingCharge != null ? model.ParkingCharge : 0;
                db_model.EntertainmentACharge = model.EntertainmentACharge != null ? model.EntertainmentACharge : 0;
                db_model.RepairCharge = model.RepairCharge != null ? model.RepairCharge : 0;
                db_model.OverStayCharge = model.OverStayCharge != null ? model.OverStayCharge : 0;
                db_model.OpenBodyCharge = model.OpenBodyCharge != null ? model.OpenBodyCharge : 0;
                db_model.DemurrageCharge = model.DemurrageCharge != null ? model.DemurrageCharge : 0;
                db_model.TotalExpense = model.TotalExpense != null ? model.TotalExpense : 0;

                bll.db.SaveChanges();

                //-Notify Driver Firebase
                var vehicle = bll.db.Vehicles.Where(m => m.PK_Vehicle == db_model.FK_Vehicle).FirstOrDefault();
                if (!string.IsNullOrEmpty(vehicle.FID))
                {
                    //var title = "Driver: Trip #" + db_model.VehicleSharing.TrackingID + " bill is paid";
                    var title = "সম্মনিত চালক, যাত্রার (#" + db_model.VehicleSharing.TrackingID + ") বিল দেওয়া হয়েছে।";
                    var message = "Dear Concern \n";
                    message = message + "Trip Detail" + "\n"
                        + "Vehicle: " + db_model.Vehicle.RegistrationNumber + "\n"
                        + "From: " + db_model.VehicleSharing.Depo.Name + " " + db_model.VehicleSharing.StartingLocation + "\n"
                        + "To: " + db_model.VehicleSharing.Depo1.Name + " " + db_model.VehicleSharing.FinishingLocation + "\n"
                        + "On: " + db_model.VehicleSharing.PossibleJourneyStartDateTime + "\n";
                    SendFCM_Notification_Single_New(vehicle.FID, title, message, db_model.PK_VehicleSharingInternalTrip.ToString(), "VehicleSharingInternalTrip");
                }
                CreateAlertMessage(AlertMessageType.Success, "Success", "Paid.");
                return RedirectToAction("VehicleSharingInternalTripIndex");
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                return RedirectToAction("VehicleSharingInternalTripIndex");
            }
        }

        public ActionResult VehicleSharingInternalTripPrintBill(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.VehicleSharingInternalTrips.Find(id);
            if (model != null)
            {
                return View(model);
            }
            else
            {
                return HttpNotFound();
            }

        }
        public JsonResult VehicleSharingInternalTripUpdatePrintCopy(Int64 id)
        {
            var model = bll.db.VehicleSharingInternalTrips.Find(id);
            if (model.PrintCopy == null || model.PrintCopy == 0)
            {
                model.PrintCopy = 1;
            }
            else
            {
                model.PrintCopy = model.PrintCopy + 1;
            }
            bll.db.SaveChanges();
            return Json(new { model.PrintCopy }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VehicleSharingInternalTripApproveBill(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.VehicleSharingInternalTrips.Where(m => m.PK_VehicleSharingInternalTrip == id).FirstOrDefault();
            if (model != null)
            {
                return View(model);
            }
            else
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", "Internal trip not found.");
                return RedirectToAction("VehicleSharingInternalTripIndex");
            }
        }
        [HttpPost]
        public ActionResult VehicleSharingInternalTripApproveBill(VehicleSharingInternalTrip model)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var db_model = bll.db.VehicleSharingInternalTrips.Where(m => m.PK_VehicleSharingInternalTrip == model.PK_VehicleSharingInternalTrip).FirstOrDefault();
            if (db_model != null)
            {
                if (db_model.StatusText == InternalTripStatus.CreatedBill)
                {
                    db_model.StatusText = InternalTripStatus.ApprovedBill;
                    db_model.BillApprovedAt = DateTime.Now;
                    db_model.FK_AppUser_BillApprover = CurrentUser.PK_User;
                    bll.db.SaveChanges();
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Internal trip bill is successfully approved.");
                }
                else
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", "Internal trip bill is already approved.");
                }
                return RedirectToAction("VehicleSharingInternalTripIndex");
            }
            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult VehicleSharingInternalTripPayBill(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.VehicleSharingInternalTrips.Where(m => m.PK_VehicleSharingInternalTrip == id).FirstOrDefault();
            if (model != null)
            {
                return View(model);
            }
            else
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", "Internal trip not found.");
                return RedirectToAction("VehicleSharingInternalTripIndex");
            }
        }
        [HttpPost]
        public ActionResult VehicleSharingInternalTripPayBill(VehicleSharingInternalTrip model)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var db_model = bll.db.VehicleSharingInternalTrips.Where(m => m.PK_VehicleSharingInternalTrip == model.PK_VehicleSharingInternalTrip).FirstOrDefault();
            if (db_model != null)
            {
                if (db_model.StatusText == InternalTripStatus.ApprovedBill)
                {
                    db_model.StatusText = InternalTripStatus.PaidBill;
                    db_model.BillPaidAt = DateTime.Now;
                    db_model.FK_AppUser_BillPayer = CurrentUser.PK_User;
                    bll.db.SaveChanges();
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Internal trip bill is successfully paid.");
                }
                else
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", "Internal trip bill is already paid.");
                }
                return RedirectToAction("VehicleSharingInternalTripIndex");
            }
            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult VehicleSharingInternalTripCreateBillAdjustment(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var internalTrip = bll.db.VehicleSharingInternalTrips.Where(m => m.PK_VehicleSharingInternalTrip == id && m.IsDeleted != true).FirstOrDefault();
            if (internalTrip.AdjustmentStatusText != null)
            {
                CreateAlertMessage(AlertMessageType.Danger, "Danger", "Already adjustment is created for this bill.");
                return RedirectToAction("VehicleSharingInternalTripIndex");
            }
            var model = new VehicleSharingInternalTripAdjustment();
            model.VehicleSharingInternalTrip = internalTrip;
            return View(model);
        }
        [HttpPost]
        public ActionResult VehicleSharingInternalTripCreateBillAdjustment(VehicleSharingInternalTripAdjustment model)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            try
            {

                var internalTrip = bll.db.VehicleSharingInternalTrips.Where(m => m.PK_VehicleSharingInternalTrip == model.PK_VehicleSharingInternalTripAdjustment && m.IsDeleted != true).FirstOrDefault();
                if (internalTrip.AdjustmentStatusText != null)
                {
                    CreateAlertMessage(AlertMessageType.Danger, "Danger", "Already adjustment is created for this bill.");
                    return RedirectToAction("VehicleSharingInternalTripIndex");
                }
                internalTrip.AdjustmentStatusText = "Created Adjustment";
                model.VehicleSharingInternalTrip = internalTrip;

                model.IsDeleted = false;
                model.BillAdjustmentCreatedAt = DateTime.Now;
                model.FK_AppUser_BillAdjustmentCreator = CurrentUser.PK_User;

                //# Adjustment-Given
                model.Distance_Empty2 = model.Distance_Empty2 != null ? model.Distance_Empty2 : 0;
                model.Distance_Loaded2 = model.Distance_Loaded2 != null ? model.Distance_Loaded2 : 0;
                model.Distance_Loaded_Plastic2 = model.Distance_Loaded_Plastic2 != null ? model.Distance_Loaded_Plastic2 : 0;
                model.Distance_Hill2 = model.Distance_Hill2 != null ? model.Distance_Hill2 : 0;
                model.Distance_OnlyMover2 = model.Distance_OnlyMover2 != null ? model.Distance_OnlyMover2 : 0;
                model.Distance_Loaded_8_To_12_Tons2 = model.Distance_Loaded_8_To_12_Tons2 != null ? model.Distance_Loaded_8_To_12_Tons2 : 0;
                model.Distance_Loaded_12_To_25_Tons2 = model.Distance_Loaded_12_To_25_Tons2 != null ? model.Distance_Loaded_12_To_25_Tons2 : 0;
                model.FuelConsumedLitre2 = model.FuelConsumedLitre2 != null ? model.FuelConsumedLitre2 : 0;
                model.FuelExpence2 = model.FuelExpence2 != null ? model.FuelExpence2 : 0;

                model.BridgeTollFerryCharge2 = model.BridgeTollFerryCharge2 != null ? model.BridgeTollFerryCharge2 : 0;
                model.EntertainmentCCharge2 = model.EntertainmentCCharge2 != null ? model.EntertainmentCCharge2 : 0;
                model.ParkingCharge2 = model.ParkingCharge2 != null ? model.ParkingCharge2 : 0;
                model.EntertainmentACharge2 = model.EntertainmentACharge2 != null ? model.EntertainmentACharge2 : 0;
                model.RepairCharge2 = model.RepairCharge2 != null ? model.RepairCharge2 : 0;
                model.OverStayCharge2 = model.OverStayCharge2 != null ? model.OverStayCharge2 : 0;
                model.OpenBodyCharge2 = model.OpenBodyCharge2 != null ? model.OpenBodyCharge2 : 0;
                model.DemurrageCharge2 = model.DemurrageCharge2 != null ? model.DemurrageCharge2 : 0;

                model.TotalExpense2 = model.TotalExpense2 != null ? model.TotalExpense2 : 0;

                //# Adjustment-Taken
                model.Distance_Empty3 = model.Distance_Empty3 != null ? model.Distance_Empty3 : 0;
                model.Distance_Loaded3 = model.Distance_Loaded3 != null ? model.Distance_Loaded3 : 0;
                model.Distance_Loaded_Plastic3 = model.Distance_Loaded_Plastic3 != null ? model.Distance_Loaded_Plastic3 : 0;
                model.Distance_Hill3 = model.Distance_Hill3 != null ? model.Distance_Hill3 : 0;
                model.Distance_OnlyMover3 = model.Distance_OnlyMover3 != null ? model.Distance_OnlyMover3 : 0;
                model.Distance_Loaded_8_To_12_Tons3 = model.Distance_Loaded_8_To_12_Tons3 != null ? model.Distance_Loaded_8_To_12_Tons3 : 0;
                model.Distance_Loaded_12_To_25_Tons3 = model.Distance_Loaded_12_To_25_Tons3 != null ? model.Distance_Loaded_12_To_25_Tons3 : 0;
                model.FuelConsumedLitre3 = model.FuelConsumedLitre3 != null ? model.FuelConsumedLitre3 : 0;
                model.FuelExpence3 = model.FuelExpence3 != null ? model.FuelExpence3 : 0;

                model.BridgeTollFerryCharge3 = model.BridgeTollFerryCharge3 != null ? model.BridgeTollFerryCharge3 : 0;
                model.EntertainmentCCharge3 = model.EntertainmentCCharge3 != null ? model.EntertainmentCCharge3 : 0;
                model.ParkingCharge3 = model.ParkingCharge3 != null ? model.ParkingCharge3 : 0;
                model.EntertainmentACharge3 = model.EntertainmentACharge3 != null ? model.EntertainmentACharge3 : 0;
                model.RepairCharge3 = model.RepairCharge3 != null ? model.RepairCharge3 : 0;
                model.OverStayCharge3 = model.OverStayCharge3 != null ? model.OverStayCharge3 : 0;
                model.OpenBodyCharge3 = model.OpenBodyCharge3 != null ? model.OpenBodyCharge3 : 0;
                model.DemurrageCharge3 = model.DemurrageCharge3 != null ? model.DemurrageCharge3 : 0;

                model.TotalExpense3 = model.TotalExpense3 != null ? model.TotalExpense3 : 0;

                bll.db.VehicleSharingInternalTripAdjustments.Add(model);
                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "Bill is successfully adjusted");
                return RedirectToAction("VehicleSharingInternalTripIndex");
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                return RedirectToAction("VehicleSharingInternalTripIndex");
            }
        }

        public ActionResult VehicleSharingInternalTripPayBillAdjustment(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.VehicleSharingInternalTripAdjustments.Where(m => m.PK_VehicleSharingInternalTripAdjustment == id).FirstOrDefault();
            if (model != null)
            {
                return View(model);
            }
            else
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", "Internal trip bill adjustment not found.");
                return RedirectToAction("VehicleSharingInternalTripIndex");
            }
        }
        [HttpPost]
        public ActionResult VehicleSharingInternalTripPayBillAdjustment(VehicleSharingInternalTripAdjustment model)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var db_model = bll.db.VehicleSharingInternalTripAdjustments.Where(m => m.PK_VehicleSharingInternalTripAdjustment == model.PK_VehicleSharingInternalTripAdjustment).FirstOrDefault();
            if (db_model != null)
            {
                if (db_model.VehicleSharingInternalTrip.AdjustmentStatusText == "Created Adjustment")
                {
                    db_model.VehicleSharingInternalTrip.AdjustmentStatusText = "Paid Adjustment";
                    db_model.BillAdjustmentPaidAt = DateTime.Now;
                    db_model.FK_AppUser_BillAdjustmentPaidBy = CurrentUser.PK_User;
                    bll.db.SaveChanges();
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Internal trip bill adjustment is successfully paid.");
                }
                else
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", "Internal trip bill adjustment is already paid.");
                }
                return RedirectToAction("VehicleSharingInternalTripIndex");
            }
            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult _VehicleSharingInternalTripChangeStatus(int id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.VehicleSharingInternalTrips.Where(m => m.PK_VehicleSharingInternalTrip == id).FirstOrDefault();

            var Statuses = new Dictionary<string, string> {
                //{ "Assigned","Assigned"},
                //{ "Entered Starting Location","Entered Starting Location"},
                { "Started Loading","Started Loading"},
                { "Finished Loading","Finished Loading"},
                //{ "Started Empty Trip","Started Empty Trip"},
                //{ "Created Bill","Created Bill"},
                //{ "Paid Bill","Paid Bill"},
                //{ "Left Starting Loaction","Left Starting Loaction"},
                //{ "Entered Finishing Location","Entered Finishing Location"},
                { "Started Unloading","Started Unloading"},
                { "Finished Unloading","Finished Unloading"},
                //{ "Finished Empty Trip","Finished Empty Trip"},
            };
            ViewBag.Statuses = new SelectList(Statuses, "Key", "Value", model.StatusText);
            return View(model);
        }
        [HttpPost]
        public ActionResult _VehicleSharingInternalTripChangeStatus(FormCollection form)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var PK_VehicleSharingInternalTrip = Convert.ToInt32(form["PK_VehicleSharingInternalTrip"]);
            var StatusText = form["StatusText"];
            var res = "";
            if (StatusText == "Started Loading")
            {
                //res = VehicleSharingInternalTrip_StartLoading(PK_VehicleSharingInternalTrip);
            }
            else if (StatusText == "Finished Loading")
            {
                //res = VehicleSharingInternalTrip_FinishLoading(PK_VehicleSharingInternalTrip);
            }
            else if (StatusText == "Started Unloading")
            {
                //res = VehicleSharingInternalTrip_StartUnloading(PK_VehicleSharingInternalTrip);
            }
            else if (StatusText == "Finished Unloading")
            {
                //res = VehicleSharingInternalTrip_FinishUnloading(PK_VehicleSharingInternalTrip);
            }
            if (res == "YES")
            {
                //CreateAlertMessage(AlertMessageType.Success, "Success", "Status update successfull.");
            }
            else
            {
                CreateAlertMessage(AlertMessageType.Danger, "Danger", res);
            }
            return RedirectToAction("VehicleSharingInternalTripChangeStatus", new { id = Convert.ToUInt64(form["PK_VehicleSharingInternalTrip"]) });
        }

        public ActionResult VehicleSharingInternalTrip_StartLoading(Int64 id)
        {
            try
            {
                var pendingTrip = bll.db.VehicleSharingInternalTrips.Where(m => m.PK_VehicleSharingInternalTrip == id).FirstOrDefault();
                if (pendingTrip.IsNotifiedToDriver != true)
                {
                    pendingTrip.IsNotifiedToDriver = true;
                    pendingTrip.NotifiedToDriverAt = DateTime.Now;
                }
                var currentTrip = bll.db.VehicleSharingInternalTrips.Where(m => m.PK_VehicleSharingInternalTrip == pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Current).FirstOrDefault();

                var message = "";

                if (currentTrip == null)
                {
                    //-check no current trip and vehicle is inside of pending trip's starting location
                    if ((pendingTrip.StatusText == InternalTripStatus.Assigned || pendingTrip.StatusText == InternalTripStatus.EnteredStartingLocation)
                        && (pendingTrip.VehicleSharing.FK_Depo_From == pendingTrip.Vehicle.FK_LocationInOut && pendingTrip.Vehicle.LocationInOrOut == true))
                    {
                        if (pendingTrip.VehicleSharing.LoadedOrEmpty == true)
                        {
                            pendingTrip.StatusText = InternalTripStatus.StartedLoading;
                            pendingTrip.LoadingStartDateTime = DateTime.Now;
                            // Update Vehicle Trip
                            pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Current = pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Pending;
                            pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Pending = null;

                            // Update Driver Trip
                            pendingTrip.AppUser4.FK_VehicleSharingInternalTrip_Current = pendingTrip.AppUser4.FK_VehicleSharingInternalTrip_Pending;
                            pendingTrip.AppUser4.FK_VehicleSharingInternalTrip_Pending = null;

                            //-Notify Assigner Mail
                            if (!string.IsNullOrEmpty(pendingTrip.AppUser.Email))
                            {
                                var Mail_Subject = "Assigner: Internal trip's loading is started";
                                var Mail_Body = "Dear Concern<br>";
                                Mail_Body = Mail_Body + "Trip Detail" + " <br>"
                                    + "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + "<br>"
                                    + "From: " + pendingTrip.VehicleSharing.Depo.Name + "<br>"
                                    + "To: " + pendingTrip.VehicleSharing.Depo1.Name + "<br>"
                                    + "On: " + pendingTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
                                SendMail_Single(pendingTrip.AppUser.Email, Mail_Subject, Mail_Body);
                            }
                            //-Notify Requisitor Mail
                            foreach (var _demand in pendingTrip.VehicleSharing.VehicleSharingDemands)
                            {
                                if (!string.IsNullOrEmpty(_demand.AppUser.Email))
                                {
                                    var Mail_Subject = "Requisitior: Internal trip's loading is started";
                                    var Mail_Body = "Dear Concern<br>";
                                    Mail_Body = Mail_Body + "Trip Detail" + " <br>"
                                        + "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + "<br>"
                                        + "From: " + pendingTrip.VehicleSharing.Depo.Name + "<br>"
                                        + "To: " + pendingTrip.VehicleSharing.Depo1.Name + "<br>"
                                        + "On: " + pendingTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
                                    SendMail_Single(_demand.AppUser.Email, Mail_Subject, Mail_Body);
                                }
                            }
                            bll.db.SaveChanges();
                            //return "YES";
                            CreateAlertMessage(AlertMessageType.Success, "Success", "Loading started successfully");
                            return RedirectToAction("VehicleSharingInternalTripIndex_TPTView");
                        }
                        else
                        {
                            pendingTrip.StatusText = InternalTripStatus.StartedEmptyTrip;
                            pendingTrip.LoadingStartDateTime = DateTime.Now;
                            pendingTrip.LoadingDoneDateTime = pendingTrip.LoadingStartDateTime;
                            // Update Vehicle Trip
                            pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Current = pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Pending;
                            pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Pending = null;

                            // Update Driver Trip
                            pendingTrip.AppUser4.FK_VehicleSharingInternalTrip_Current = pendingTrip.AppUser4.FK_VehicleSharingInternalTrip_Pending;
                            pendingTrip.AppUser4.FK_VehicleSharingInternalTrip_Pending = null;

                            //-Notify Assigner Mail
                            if (!string.IsNullOrEmpty(pendingTrip.AppUser.Email))
                            {
                                var Mail_Subject = "Assigner: Internal empty trip started";
                                var Mail_Body = "Dear Concern<br>";
                                Mail_Body = Mail_Body + "Trip Detail" + " <br>"
                                    + "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + "<br>"
                                    + "From: " + pendingTrip.VehicleSharing.Depo.Name + "<br>"
                                    + "To: " + pendingTrip.VehicleSharing.Depo1.Name + "<br>"
                                    + "On: " + pendingTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
                                SendMail_Single(pendingTrip.AppUser.Email, Mail_Subject, Mail_Body);
                            }
                            //-Notify Requisitor Mail
                            foreach (var _demand in pendingTrip.VehicleSharing.VehicleSharingDemands)
                            {
                                if (!string.IsNullOrEmpty(_demand.AppUser.Email))
                                {
                                    var Mail_Subject = "Requisitior: Internal empty trip started";
                                    var Mail_Body = "Dear Concern<br>";
                                    Mail_Body = Mail_Body + "Trip Detail" + " <br>"
                                        + "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + "<br>"
                                        + "From: " + pendingTrip.VehicleSharing.Depo.Name + "<br>"
                                        + "To: " + pendingTrip.VehicleSharing.Depo1.Name + "<br>"
                                        + "On: " + pendingTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
                                    SendMail_Single(_demand.AppUser.Email, Mail_Subject, Mail_Body);
                                }
                            }
                            bll.db.SaveChanges();
                            //return "YES";
                            CreateAlertMessage(AlertMessageType.Success, "Success", "Loading started successfully");
                            return RedirectToAction("VehicleSharingInternalTripIndex_TPTView");
                        }
                    }
                    else if (pendingTrip.VehicleSharing.FK_Depo_From != pendingTrip.Vehicle.FK_LocationInOut || pendingTrip.Vehicle.LocationInOrOut != true)
                    {
                        message = "গাড়ির পরবর্তী যাত্রার (#" + pendingTrip.VehicleSharing.TrackingID + ") শুরুর স্থানে (" + pendingTrip.VehicleSharing.Depo.Name + ") প্রবেশ করুন।";
                        //return message;
                        CreateAlertMessage(AlertMessageType.Danger, "Danger", message);
                        return RedirectToAction("VehicleSharingInternalTripIndex_TPTView");
                    }
                    else if (currentTrip.StatusText != InternalTripStatus.Assigned && currentTrip.StatusText != InternalTripStatus.EnteredStartingLocation)
                    {
                        message = "চলমান যাত্রার (#" + currentTrip.VehicleSharing.TrackingID + ") অবস্থা সামঞ্জস্যপুর্ন নয়, লোড শুরু করা যাচ্ছে না।";
                        //return message;
                        CreateAlertMessage(AlertMessageType.Danger, "Danger", message);
                        return RedirectToAction("VehicleSharingInternalTripIndex_TPTView");
                    }
                    else
                    {
                        message = "Undefined Reasson : StartLoading 1.1";
                        //return message;
                        CreateAlertMessage(AlertMessageType.Danger, "Danger", message);
                        return RedirectToAction("VehicleSharingInternalTripIndex_TPTView");
                    }
                }
                else
                {
                    //-check current trip already CLOSED and vehicle is inside of pending trip from location
                    if ((currentTrip.StatusText == InternalTripStatus.FinishedUnloading || currentTrip.StatusText == InternalTripStatus.FinishedEmptyTrip) && (pendingTrip.VehicleSharing.FK_Depo_From == pendingTrip.Vehicle.FK_LocationInOut && pendingTrip.Vehicle.LocationInOrOut == true))//Trip closed
                    {
                        if (pendingTrip.VehicleSharing.LoadedOrEmpty == true)
                        {
                            pendingTrip.StatusText = InternalTripStatus.StartedLoading;
                            pendingTrip.LoadingStartDateTime = DateTime.Now;
                            // Update Vehicle Trip
                            pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Current = pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Pending;
                            pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Pending = null;

                            // Update Driver Trip
                            pendingTrip.AppUser4.FK_VehicleSharingInternalTrip_Current = pendingTrip.AppUser4.FK_VehicleSharingInternalTrip_Pending;
                            pendingTrip.AppUser4.FK_VehicleSharingInternalTrip_Pending = null;

                            //-Notify Assigner Mail
                            if (!string.IsNullOrEmpty(pendingTrip.AppUser.Email))
                            {
                                var Mail_Subject = "Assigner: Internal trip's loading is started";
                                var Mail_Body = "Dear Concern<br>";
                                Mail_Body = Mail_Body + "Trip Detail" + " <br>"
                                    + "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + "<br>"
                                    + "From: " + pendingTrip.VehicleSharing.Depo.Name + "<br>"
                                    + "To: " + pendingTrip.VehicleSharing.Depo1.Name + "<br>"
                                    + "On: " + pendingTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
                                SendMail_Single(pendingTrip.AppUser.Email, Mail_Subject, Mail_Body);
                            }
                            //-Notify Requisitor Mail
                            foreach (var _demand in pendingTrip.VehicleSharing.VehicleSharingDemands)
                            {
                                if (!string.IsNullOrEmpty(_demand.AppUser.Email))
                                {
                                    var Mail_Subject = "Requisitior: Internal trip's loading is started";
                                    var Mail_Body = "Dear Concern<br>";
                                    Mail_Body = Mail_Body + "Trip Detail" + " <br>"
                                        + "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + "<br>"
                                        + "From: " + pendingTrip.VehicleSharing.Depo.Name + "<br>"
                                        + "To: " + pendingTrip.VehicleSharing.Depo1.Name + "<br>"
                                        + "On: " + pendingTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
                                    SendMail_Single(_demand.AppUser.Email, Mail_Subject, Mail_Body);
                                }
                            }
                            bll.db.SaveChanges();
                            //return "YES";
                            CreateAlertMessage(AlertMessageType.Success, "Success", "Loading started successfully");
                            return RedirectToAction("VehicleSharingInternalTripIndex_TPTView");
                        }
                        else
                        {
                            pendingTrip.StatusText = InternalTripStatus.StartedEmptyTrip;
                            pendingTrip.LoadingStartDateTime = DateTime.Now;
                            pendingTrip.LoadingDoneDateTime = pendingTrip.LoadingStartDateTime;
                            // Update Vehicle Trip
                            pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Current = pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Pending;
                            pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Pending = null;

                            // Update Driver Trip
                            pendingTrip.AppUser4.FK_VehicleSharingInternalTrip_Current = pendingTrip.AppUser4.FK_VehicleSharingInternalTrip_Pending;
                            pendingTrip.AppUser4.FK_VehicleSharingInternalTrip_Pending = null;

                            //-Notify Assigner Mail
                            if (!string.IsNullOrEmpty(pendingTrip.AppUser.Email))
                            {
                                var Mail_Subject = "Assigner: Internal empty trip started";
                                var Mail_Body = "Dear Concern<br>";
                                Mail_Body = Mail_Body + "Trip Detail" + " <br>"
                                    + "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + "<br>"
                                    + "From: " + pendingTrip.VehicleSharing.Depo.Name + "<br>"
                                    + "To: " + pendingTrip.VehicleSharing.Depo1.Name + "<br>"
                                    + "On: " + pendingTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
                                SendMail_Single(pendingTrip.AppUser.Email, Mail_Subject, Mail_Body);
                            }
                            //-Notify Requisitor Mail
                            foreach (var _demand in pendingTrip.VehicleSharing.VehicleSharingDemands)
                            {
                                if (!string.IsNullOrEmpty(_demand.AppUser.Email))
                                {
                                    var Mail_Subject = "Requisitior: Internal empty trip started";
                                    var Mail_Body = "Dear Concern<br>";
                                    Mail_Body = Mail_Body + "Trip Detail" + " <br>"
                                        + "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + "<br>"
                                        + "From: " + pendingTrip.VehicleSharing.Depo.Name + "<br>"
                                        + "To: " + pendingTrip.VehicleSharing.Depo1.Name + "<br>"
                                        + "On: " + pendingTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
                                    SendMail_Single(_demand.AppUser.Email, Mail_Subject, Mail_Body);
                                }
                            }
                            bll.db.SaveChanges();
                            //return "YES";
                            CreateAlertMessage(AlertMessageType.Success, "Success", "Loading started successfully");
                            return RedirectToAction("VehicleSharingInternalTripIndex_TPTView");
                        }
                    }
                    else if (pendingTrip.VehicleSharing.FK_Depo_From != pendingTrip.Vehicle.FK_LocationInOut || pendingTrip.Vehicle.LocationInOrOut != true)
                    {
                        message = "গাড়ির পরবর্তী যাত্রার (#" + pendingTrip.VehicleSharing.TrackingID + ") শুরুর স্থানে (" + pendingTrip.VehicleSharing.Depo.Name + ") প্রবেশ করুন।";
                        //return message;
                        CreateAlertMessage(AlertMessageType.Danger, "Danger", message);
                        return RedirectToAction("VehicleSharingInternalTripIndex_TPTView");
                    }
                    else if (currentTrip.StatusText != InternalTripStatus.FinishedUnloading && currentTrip.StatusText != InternalTripStatus.FinishedEmptyTrip)
                    {
                        if (currentTrip.VehicleSharing.LoadedOrEmpty == true)
                        {
                            message = "চলমান যাত্রার (#" + currentTrip.VehicleSharing.TrackingID + ") মালামাল আনলোড শেষ করুন।";
                            //return message;
                            CreateAlertMessage(AlertMessageType.Danger, "Danger", message);
                            return RedirectToAction("VehicleSharingInternalTripIndex_TPTView");
                        }
                        else
                        {
                            //# Vehicle with empty trip already enterd in it's Destination but emptyTrip.StatusText != FinishedEmptyTrip 
                            message = "Undefined Reasson : StartLoading 2.1";
                            //return message;
                            CreateAlertMessage(AlertMessageType.Danger, "Danger", message);
                            return RedirectToAction("VehicleSharingInternalTripIndex_TPTView");
                        }
                    }
                    else
                    {
                        message = "Undefined Reasson : StartLoading 2.2";
                        //return message;
                        CreateAlertMessage(AlertMessageType.Danger, "Danger", message);
                        return RedirectToAction("VehicleSharingInternalTripIndex_TPTView");
                    }
                }
            }
            catch (Exception e)
            {
                var errrorMessage = "";
                do { errrorMessage = "#" + errrorMessage + e.Message; if (e.InnerException != null) { e = e.InnerException; } else { break; } } while (true);
                //return errrorMessage;
                CreateAlertMessage(AlertMessageType.Warning, "Danger", errrorMessage);
                return RedirectToAction("VehicleSharingInternalTripIndex_TPTView");
            }
        }
        public ActionResult VehicleSharingInternalTrip_FinishLoading(Int64 id)
        {
            try
            {
                var currentTrip = bll.db.VehicleSharingInternalTrips.Where(m => m.PK_VehicleSharingInternalTrip == id).FirstOrDefault();

                var message = "";

                if (currentTrip != null)
                {
                    if (currentTrip.StatusText == InternalTripStatus.StartedLoading
                        && (currentTrip.VehicleSharing.FK_Depo_From == currentTrip.Vehicle.FK_LocationInOut && currentTrip.Vehicle.LocationInOrOut == true)
                        )
                    {
                        currentTrip.StatusText = InternalTripStatus.FinishedLoading;
                        currentTrip.LoadingDoneDateTime = DateTime.Now;
                        //-Notify Assigner Mail
                        if (!string.IsNullOrEmpty(currentTrip.AppUser.Email))
                        {
                            var Mail_Subject = "Assigner: Internal trip's loading is completed";
                            var Mail_Body = "Dear Concern<br>";
                            Mail_Body = Mail_Body + "Trip Detail" + " <br>"
                                + "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + "<br>"
                                + "From: " + currentTrip.VehicleSharing.Depo.Name + "<br>"
                                + "To: " + currentTrip.VehicleSharing.Depo1.Name + "<br>"
                                + "On: " + currentTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
                            SendMail_Single(currentTrip.AppUser.Email, Mail_Subject, Mail_Body);
                        }
                        //-Notify Requisitor Mail
                        foreach (var _demand in currentTrip.VehicleSharing.VehicleSharingDemands)
                        {
                            if (!string.IsNullOrEmpty(_demand.AppUser.Email))
                            {
                                var Mail_Subject = "Requisitior: Internal trip's loading is completed";
                                var Mail_Body = "Dear Concern<br>";
                                Mail_Body = Mail_Body + "Trip Detail" + " <br>"
                                    + "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + "<br>"
                                    + "From: " + currentTrip.VehicleSharing.Depo.Name + "<br>"
                                    + "To: " + currentTrip.VehicleSharing.Depo1.Name + "<br>"
                                    + "On: " + currentTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
                                SendMail_Single(_demand.AppUser.Email, Mail_Subject, Mail_Body);
                            }
                        }
                        bll.db.SaveChanges();
                        //return "YES";
                        CreateAlertMessage(AlertMessageType.Success, "Success", "Loading finished successfully");
                        return RedirectToAction("VehicleSharingInternalTripIndex_TPTView");
                    }
                    else if (currentTrip.VehicleSharing.FK_Depo_From != currentTrip.Vehicle.FK_LocationInOut || currentTrip.Vehicle.LocationInOrOut != true)
                    {
                        message = "চলমান যাত্রার (#" + currentTrip.VehicleSharing.TrackingID + ") শুরুর স্থানে (" + currentTrip.VehicleSharing.Depo.Name + ") প্রবেশ করুন।";
                        //return message;
                        CreateAlertMessage(AlertMessageType.Danger, "Danger", message);
                        return RedirectToAction("VehicleSharingInternalTripIndex_TPTView");
                    }
                    else if (currentTrip.StatusText != InternalTripStatus.StartedLoading)
                    {
                        message = "চলমান যাত্রার (#" + currentTrip.VehicleSharing.TrackingID + ") মালামাল লোড শুরু করুন।";
                        //return message;
                        CreateAlertMessage(AlertMessageType.Danger, "Danger", message);
                        return RedirectToAction("VehicleSharingInternalTripIndex_TPTView");
                    }
                    else
                    {
                        message = "Undefined Reasson : FinishLoading 1.1";
                        //return message;
                        CreateAlertMessage(AlertMessageType.Danger, "Danger", message);
                        return RedirectToAction("VehicleSharingInternalTripIndex_TPTView");
                    }
                }
                else
                {
                    message = "যাত্রার তথ্য খুজে পাওয়া যায়নি।";
                    //return message;
                    CreateAlertMessage(AlertMessageType.Danger, "Danger", message);
                    return RedirectToAction("VehicleSharingInternalTripIndex_TPTView");
                }
            }
            catch (Exception e)
            {
                var errrorMessage = "";
                do { errrorMessage = "#" + errrorMessage + e.Message; if (e.InnerException != null) { e = e.InnerException; } else { break; } } while (true);
                //return errrorMessage;
                CreateAlertMessage(AlertMessageType.Warning, "Danger", errrorMessage);
                return RedirectToAction("VehicleSharingInternalTripIndex_TPTView");
            }
        }
        public ActionResult VehicleSharingInternalTrip_StartUnloading(Int64 id)
        {
            try
            {
                var currentTrip = bll.db.VehicleSharingInternalTrips.Where(m => m.PK_VehicleSharingInternalTrip == id).FirstOrDefault();

                var now = DateTime.Now;
                var message = "";

                if (currentTrip != null)
                {
                    if (currentTrip.StatusText == InternalTripStatus.EnteredFinishingLocation && (currentTrip.VehicleSharing.FK_Depo_To == currentTrip.Vehicle.FK_LocationInOut && currentTrip.Vehicle.LocationInOrOut == true))
                    {
                        currentTrip.StatusText = InternalTripStatus.StartedUnloading;
                        currentTrip.UnloadingStartDateTime = DateTime.Now;
                        //-Notify Assigner Mail
                        if (!string.IsNullOrEmpty(currentTrip.AppUser.Email))
                        {
                            var Mail_Subject = "Assigner: Internal trip's unloading is started";
                            var Mail_Body = "Dear Concern<br>";
                            Mail_Body = Mail_Body + "Trip Detail" + " <br>"
                                + "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + "<br>"
                                + "From: " + currentTrip.VehicleSharing.Depo.Name + "<br>"
                                + "To: " + currentTrip.VehicleSharing.Depo1.Name + "<br>"
                                + "On: " + currentTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
                            SendMail_Single(currentTrip.AppUser.Email, Mail_Subject, Mail_Body);
                        }
                        //-Notify Requisitor Mail
                        foreach (var _demand in currentTrip.VehicleSharing.VehicleSharingDemands)
                        {
                            if (!string.IsNullOrEmpty(_demand.AppUser.Email))
                            {
                                var Mail_Subject = "Requisitior: Internal trip's unloading is started";
                                var Mail_Body = "Dear Concern<br>";
                                Mail_Body = Mail_Body + "Trip Detail" + " <br>"
                                    + "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + "<br>"
                                    + "From: " + currentTrip.VehicleSharing.Depo.Name + "<br>"
                                    + "To: " + currentTrip.VehicleSharing.Depo1.Name + "<br>"
                                    + "On: " + currentTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
                                SendMail_Single(_demand.AppUser.Email, Mail_Subject, Mail_Body);
                            }
                        }
                        bll.db.SaveChanges();
                        //return "YES";
                        CreateAlertMessage(AlertMessageType.Success, "Success", "Unoading started successfully");
                        return RedirectToAction("VehicleSharingInternalTripIndex_TPTView");
                    }
                    else if (currentTrip.VehicleSharing.FK_Depo_To != currentTrip.Vehicle.FK_LocationInOut || currentTrip.Vehicle.LocationInOrOut != true)
                    {
                        message = "চলমান যাত্রার (#" + currentTrip.VehicleSharing.TrackingID + ") শেষের স্থানে (" + currentTrip.VehicleSharing.Depo1.Name + ") প্রবেশ করুন।";
                        //return message;
                        CreateAlertMessage(AlertMessageType.Danger, "Danger", message);
                        return RedirectToAction("VehicleSharingInternalTripIndex_TPTView");
                    }
                    else if (currentTrip.StatusText != InternalTripStatus.EnteredFinishingLocation)
                    {
                        message = "চলমান যাত্রার (#" + currentTrip.VehicleSharing.TrackingID + ") অবস্থা সামঞ্জস্যপুর্ন নয়,আনলোড শুরু করা যাচ্ছে না।";
                        //return message;
                        CreateAlertMessage(AlertMessageType.Danger, "Danger", message);
                        return RedirectToAction("VehicleSharingInternalTripIndex_TPTView");
                    }
                    else
                    {
                        message = "Undefined Reasson : StartUnloading 1.1";
                        //return message;
                        CreateAlertMessage(AlertMessageType.Danger, "Danger", message);
                        return RedirectToAction("VehicleSharingInternalTripIndex_TPTView");
                    }
                }
                else
                {
                    message = "যাত্রার তথ্য খুজে পাওয়া যায়নি।";
                    //return message;
                    CreateAlertMessage(AlertMessageType.Danger, "Danger", message);
                    return RedirectToAction("VehicleSharingInternalTripIndex_TPTView");
                }
            }
            catch (Exception e)
            {
                var errrorMessage = "";
                do { errrorMessage = "#" + errrorMessage + e.Message; if (e.InnerException != null) { e = e.InnerException; } else { break; } } while (true);
                //return errrorMessage;
                CreateAlertMessage(AlertMessageType.Warning, "Danger", errrorMessage);
                return RedirectToAction("VehicleSharingInternalTripIndex_TPTView");
            }
        }
        public ActionResult VehicleSharingInternalTrip_FinishUnloading(Int64 id)
        {
            try
            {
                var currentTrip = bll.db.VehicleSharingInternalTrips.Where(m => m.PK_VehicleSharingInternalTrip == id).FirstOrDefault();

                var now = DateTime.Now;
                var message = "";

                if (currentTrip != null)
                {

                    if (currentTrip.StatusText == InternalTripStatus.StartedUnloading && (currentTrip.VehicleSharing.FK_Depo_To == currentTrip.Vehicle.FK_LocationInOut && currentTrip.Vehicle.LocationInOrOut == true))
                    {
                        currentTrip.StatusText = InternalTripStatus.FinishedUnloading;
                        currentTrip.UnloadingDoneDateTime = DateTime.Now;

                        // Update Vehicle Trip
                        currentTrip.Vehicle.FK_VehicleSharingInternalTrip_Current = null;

                        // Update Driver Trip
                        currentTrip.AppUser4.FK_VehicleSharingInternalTrip_Current = null;

                        //-Notify Assigner Mail
                        if (!string.IsNullOrEmpty(currentTrip.AppUser.Email))
                        {
                            var Mail_Subject = "Assigner: Internal trip's unloading is started";
                            var Mail_Body = "Dear Concern<br>";
                            Mail_Body = Mail_Body + "Trip Detail" + " <br>"
                                + "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + "<br>"
                                + "From: " + currentTrip.VehicleSharing.Depo.Name + "<br>"
                                + "To: " + currentTrip.VehicleSharing.Depo1.Name + "<br>"
                                + "On: " + currentTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
                            SendMail_Single(currentTrip.AppUser.Email, Mail_Subject, Mail_Body);
                        }
                        //-Notify Requisitor Mail
                        foreach (var _demand in currentTrip.VehicleSharing.VehicleSharingDemands)
                        {
                            if (!string.IsNullOrEmpty(_demand.AppUser.Email))
                            {
                                var Mail_Subject = "Requisitior: Internal trip's unloading is started";
                                var Mail_Body = "Dear Concern<br>";
                                Mail_Body = Mail_Body + "Trip Detail" + " <br>"
                                    + "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + "<br>"
                                    + "From: " + currentTrip.VehicleSharing.Depo.Name + "<br>"
                                    + "To: " + currentTrip.VehicleSharing.Depo1.Name + "<br>"
                                    + "On: " + currentTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
                                SendMail_Single(_demand.AppUser.Email, Mail_Subject, Mail_Body);
                            }
                        }
                        bll.db.SaveChanges();

                        //#Check pending trip for loading
                        var pendingTrip = bll.db.VehicleSharingInternalTrips.Where(m => m.PK_VehicleSharingInternalTrip == currentTrip.Vehicle.FK_VehicleSharingInternalTrip_Pending).FirstOrDefault();
                        if (pendingTrip != null)
                        {
                            //-check no current trip and vehicle is inside of pending trip's starting location
                            if (pendingTrip.VehicleSharing.FK_Depo_From == pendingTrip.Vehicle.FK_LocationInOut && pendingTrip.Vehicle.LocationInOrOut == true)
                            {
                                if (pendingTrip.VehicleSharing.LoadedOrEmpty == true)
                                {
                                    pendingTrip.StatusText = InternalTripStatus.StartedLoading;
                                    pendingTrip.LoadingStartDateTime = DateTime.Now;
                                    // Update Vehicle Trip
                                    pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Current = pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Pending;
                                    pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Pending = null;

                                    // Update Driver Trip
                                    pendingTrip.AppUser4.FK_VehicleSharingInternalTrip_Current = pendingTrip.AppUser4.FK_VehicleSharingInternalTrip_Pending;
                                    pendingTrip.AppUser4.FK_VehicleSharingInternalTrip_Pending = null;

                                    //-Notify Driver Firebase
                                    //var vehicle = bll.db.Vehicles.Where(m => m.PK_Vehicle == currentTrip.FK_Vehicle).FirstOrDefault();
                                    if (!string.IsNullOrEmpty(pendingTrip.Vehicle.FID))
                                    {
                                        var _FK_Depo_To = currentTrip.VehicleSharing.VehicleSharingDemands.Where(n => n.IsHeadDemand == true).FirstOrDefault().FK_Depo_To;
                                        //var title = "Driver: Your Vehicle Entered in Current Trip #" + currentTrip.VehicleSharing.TrackingID + " Destination: " + bll.db.Depoes.Where(m => m.PK_Depo == _FK_Depo_To).FirstOrDefault().Name;
                                        var title = "সম্মানিত চালক, গাড়ি আসন্ন যাত্রার (#" + currentTrip.VehicleSharing.TrackingID + ")  লোডিং-এর সময় গণনা শুরু হয়েছে।";
                                        message = "Dear Concern \n";
                                        message = message + "Trip Detail" + "\n"
                                            + "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + "\n"
                                            + "From: " + currentTrip.VehicleSharing.Depo.Name + "\n"
                                            + "To: " + currentTrip.VehicleSharing.Depo1.Name + "\n"
                                            + "On: " + currentTrip.VehicleSharing.PossibleJourneyStartDateTime + "\n";
                                        SendFCM_Notification_Single_New(pendingTrip.Vehicle.FID, title, message, currentTrip.PK_VehicleSharingInternalTrip.ToString(), "VehicleSharingInternalTrip");
                                    }
                                    //-Notify Assigner Mail
                                    if (!string.IsNullOrEmpty(pendingTrip.AppUser.Email))
                                    {
                                        var Mail_Subject = "Assigner: Internal trip's loading is started";
                                        var Mail_Body = "Dear Concern<br>";
                                        Mail_Body = Mail_Body + "Trip Detail" + " <br>"
                                            + "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + "<br>"
                                            + "From: " + pendingTrip.VehicleSharing.Depo.Name + "<br>"
                                            + "To: " + pendingTrip.VehicleSharing.Depo1.Name + "<br>"
                                            + "On: " + pendingTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
                                        SendMail_Single(pendingTrip.AppUser.Email, Mail_Subject, Mail_Body);
                                    }
                                    //-Notify Requisitor Mail
                                    foreach (var _demand in pendingTrip.VehicleSharing.VehicleSharingDemands)
                                    {
                                        if (!string.IsNullOrEmpty(_demand.AppUser.Email))
                                        {
                                            var Mail_Subject = "Requisitior: Internal trip's loading is started";
                                            var Mail_Body = "Dear Concern<br>";
                                            Mail_Body = Mail_Body + "Trip Detail" + " <br>"
                                                + "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + "<br>"
                                                + "From: " + pendingTrip.VehicleSharing.Depo.Name + "<br>"
                                                + "To: " + pendingTrip.VehicleSharing.Depo1.Name + "<br>"
                                                + "On: " + pendingTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
                                            SendMail_Single(_demand.AppUser.Email, Mail_Subject, Mail_Body);
                                        }
                                    }
                                    bll.db.SaveChanges();
                                }
                                else
                                {
                                    pendingTrip.StatusText = InternalTripStatus.StartedEmptyTrip;
                                    pendingTrip.LoadingStartDateTime = DateTime.Now;
                                    pendingTrip.LoadingDoneDateTime = pendingTrip.LoadingStartDateTime;
                                    // Update Vehicle Trip
                                    pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Current = pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Pending;
                                    pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Pending = null;

                                    // Update Driver Trip
                                    pendingTrip.AppUser4.FK_VehicleSharingInternalTrip_Current = pendingTrip.AppUser4.FK_VehicleSharingInternalTrip_Pending;
                                    pendingTrip.AppUser4.FK_VehicleSharingInternalTrip_Pending = null;

                                    //-Notify Driver Firebase
                                    //var vehicle = bll.db.Vehicles.Where(m => m.PK_Vehicle == currentTrip.FK_Vehicle).FirstOrDefault();
                                    if (!string.IsNullOrEmpty(pendingTrip.Vehicle.FID))
                                    {
                                        var _FK_Depo_To = currentTrip.VehicleSharing.VehicleSharingDemands.Where(n => n.IsHeadDemand == true).FirstOrDefault().FK_Depo_To;
                                        //var title = "Driver: Your Vehicle Entered in Current Trip #" + currentTrip.VehicleSharing.TrackingID + " Destination: " + bll.db.Depoes.Where(m => m.PK_Depo == _FK_Depo_To).FirstOrDefault().Name;
                                        var title = "সম্মানিত চালক, গাড়ি আসন্ন খালি যাত্রার #" + pendingTrip.VehicleSharing.TrackingID + " শুরুর স্থানে (" + bll.db.Depoes.Where(m => m.PK_Depo == _FK_Depo_To).FirstOrDefault().Name + ") প্রবেশ করেছে, যাত্রার সময় গণনা শুরু হয়েছে।";
                                        message = "Dear Concern \n";
                                        message = message + "Trip Detail" + "\n"
                                            + "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + "\n"
                                            + "From: " + currentTrip.VehicleSharing.Depo.Name + "\n"
                                            + "To: " + currentTrip.VehicleSharing.Depo1.Name + "\n"
                                            + "On: " + currentTrip.VehicleSharing.PossibleJourneyStartDateTime + "\n";
                                        SendFCM_Notification_Single_New(pendingTrip.Vehicle.FID, title, message, currentTrip.PK_VehicleSharingInternalTrip.ToString(), "VehicleSharingInternalTrip");
                                    }
                                    //-Notify Assigner Mail
                                    if (!string.IsNullOrEmpty(pendingTrip.AppUser.Email))
                                    {
                                        var Mail_Subject = "Assigner: Internal empty trip started";
                                        var Mail_Body = "Dear Concern<br>";
                                        Mail_Body = Mail_Body + "Trip Detail" + " <br>"
                                            + "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + "<br>"
                                            + "From: " + pendingTrip.VehicleSharing.Depo.Name + "<br>"
                                            + "To: " + pendingTrip.VehicleSharing.Depo1.Name + "<br>"
                                            + "On: " + pendingTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
                                        SendMail_Single(pendingTrip.AppUser.Email, Mail_Subject, Mail_Body);
                                    }
                                    //-Notify Requisitor Mail
                                    foreach (var _demand in pendingTrip.VehicleSharing.VehicleSharingDemands)
                                    {
                                        if (!string.IsNullOrEmpty(_demand.AppUser.Email))
                                        {
                                            var Mail_Subject = "Requisitior: Internal empty trip started";
                                            var Mail_Body = "Dear Concern<br>";
                                            Mail_Body = Mail_Body + "Trip Detail" + " <br>"
                                                + "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + "<br>"
                                                + "From: " + pendingTrip.VehicleSharing.Depo.Name + "<br>"
                                                + "To: " + pendingTrip.VehicleSharing.Depo1.Name + "<br>"
                                                + "On: " + pendingTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
                                            SendMail_Single(_demand.AppUser.Email, Mail_Subject, Mail_Body);
                                        }
                                    }
                                    bll.db.SaveChanges();
                                }
                            }
                        }
                        //return "YES";
                        CreateAlertMessage(AlertMessageType.Success, "Success", "Unoading finished successfully");
                        return RedirectToAction("VehicleSharingInternalTripIndex_TPTView");
                    }
                    else if (currentTrip.VehicleSharing.FK_Depo_To != currentTrip.Vehicle.FK_LocationInOut || currentTrip.Vehicle.LocationInOrOut != true)
                    {
                        message = "চলমান যাত্রার (#" + currentTrip.VehicleSharing.TrackingID + ") শেষের স্থানে (" + currentTrip.VehicleSharing.Depo1.Name + ") প্রবেশ করুন।";
                        //return message;
                        CreateAlertMessage(AlertMessageType.Danger, "Danger", message);
                        return RedirectToAction("VehicleSharingInternalTripIndex_TPTView");
                    }
                    else if (currentTrip.StatusText != InternalTripStatus.StartedUnloading)
                    {
                        message = "চলমান যাত্রার (#" + currentTrip.VehicleSharing.TrackingID + ") মালামাল আনলোড শুরু করুন।";
                        //return message;
                        CreateAlertMessage(AlertMessageType.Danger, "Danger", message);
                        return RedirectToAction("VehicleSharingInternalTripIndex_TPTView");
                    }
                    else
                    {
                        message = "Undefined Reasson : FinishUnloading 1.1";
                        //return message;
                        CreateAlertMessage(AlertMessageType.Danger, "Danger", message);
                        return RedirectToAction("VehicleSharingInternalTripIndex_TPTView");
                    }
                }
                else
                {
                    message = "যাত্রার তথ্য খুজে পাওয়া যায়নি।";
                    //return message;
                    CreateAlertMessage(AlertMessageType.Danger, "Danger", message);
                    return RedirectToAction("VehicleSharingInternalTripIndex_TPTView");
                }
            }
            catch (Exception e)
            {
                var errrorMessage = "";
                do { errrorMessage = "#" + errrorMessage + e.Message; if (e.InnerException != null) { e = e.InnerException; } else { break; } } while (true);
                //return errrorMessage;
                CreateAlertMessage(AlertMessageType.Warning, "Danger", errrorMessage);
                return RedirectToAction("VehicleSharingInternalTripIndex_TPTView");
            }
        }

        //# Internal Empty Trip
        public ActionResult EmptyTripCreate()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }

            //var userSubType = "-";
            //if (CurrentUser.AppUserSubType != null && CurrentUser.AppUserSubType.Contains(" RAISER"))
            //{
            //    userSubType = CurrentUser.AppUserSubType.Replace(" RAISER", " APPROVER");
            //}
            //else if (CurrentUser.AppUserSubType != null && CurrentUser.AppUserSubType.Contains(" APPROVER"))
            //{
            //    userSubType = CurrentUser.AppUserSubType;
            //}
            //var _approversPKs = bll.db.RequisitionAgentProposedDepoes.Where(m => m.WillPropose == true && m.FK_Depo == CurrentUser.FK_Depo).Select(m => m.FK_RequisitionAgent).ToList();
            //var Approvers = bll.db.AppUsers.Where(m => (m.FK_Depo == CurrentUser.FK_Depo || _approversPKs.Contains(m.PK_User)) && m.AppUserSubType == userSubType).Select(m => new { key = m.PK_User, value = m.Depo.Name + ":" + m.AppUserSubType.Replace(" APPROVER", "") + ":" + m.FullName }).ToList();
            //ViewBag.Approvers = new SelectList(Approvers, "key", "value");
            ViewBag.RequisitionVehicleTypes = new SelectList(bll.db.RequisitionVehicleTypes.Where(m => m.IsDeleted != true), "PK_RequisitionVehicleType", "Title_English");
            var locations = bll.db.Depoes.Where(m => m.IsDeleted != true && m.Category == "Physical Location").Select(m => new { PK_Depo = m.PK_Depo, Name = m.Code + " " + m.Name });
            ViewBag.Locations1 = new SelectList(locations, "PK_Depo", "Name");
            ViewBag.Locations2 = new SelectList(locations, "PK_Depo", "Name");
            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            ViewBag.Vehicles = new SelectList(bll.db.Vehicles.Where(m => m.OWN_MHT_DHT == "OWN" && accessibleDepoes.Contains(m.FK_Depo)).Select(m => new { m.PK_Vehicle, m.RegistrationNumber }), "PK_Vehicle", "RegistrationNumber");
            ViewBag.Drivers = new SelectList(bll.db.AppUsers.Where(m => m.AppUserType == "Internal Driver").Select(m => new { m.PK_User, m.UniqueIDNumber }), "PK_User", "UniqueIDNumber");
            var _today = DateTime.Now.Date;
            ViewBag.AlreadyOpenedCount = bll.db.VehicleSharingDemands.Where(m => m.FK_AppUser_Client == CurrentUser.PK_User && m.CreatedAt > _today).Count();
            return View();
        }
        [HttpPost]
        public ActionResult EmptyTripCreate(FormCollection form)
        {
            var demand = new VehicleSharingDemand();
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            try
            {
                using (var dbContextTransaction = bll.db.Database.BeginTransaction())
                {
                    demand = new VehicleSharingDemand();
                    demand.IsDeleted = false;

                    demand.CreatedAt = DateTime.Now;
                    demand.FK_AppUser_Client = CurrentUser.PK_User;
                    demand.FK_ReferenceDepo = CurrentUser.FK_Depo;

                    if (!string.IsNullOrEmpty(form["FK_Depo_From"]))
                    {
                        demand.FK_Depo_From = Guid.Parse(form["FK_Depo_From"]);
                    }
                    if (!string.IsNullOrEmpty(form["FK_Depo_To"]))
                    {
                        demand.FK_Depo_To = Guid.Parse(form["FK_Depo_To"]);
                    }
                    if (!string.IsNullOrEmpty(form["DistanceGoogle"]))
                    {
                        demand.DistanceGoogle = Convert.ToDouble(form["DistanceGoogle"]);
                    }
                    demand.FK_RequisitionVehicleType = Convert.ToInt32(form["FK_RequisitionVehicleType"]);
                    demand.WantedCount = 1;
                    demand.AcceptedCount = 1;
                    demand.PossibleJourneyStartDateTime = DateTime.ParseExact(form["PossibleJourneyStartDateTime"], "yyyy-MM-dd h:mm tt", CultureInfo.InvariantCulture);
                    demand.ClientNote = form["ClientNote"];
                    demand.Status = 0;
                    demand.LoadedOrEmpty = false;

                    bll.db.VehicleSharingDemands.Add(demand);
                    bll.db.SaveChanges();
                    demand.TrackingID = "ED" + demand.PK_VehicleSharingDemand.ToString();
                    bll.db.SaveChanges();

                    //# create sharing
                    var sharing = new VehicleSharing();
                    sharing.CreatedAt = DateTime.Now;
                    sharing.FK_AppUser_Assigner = CurrentUser.PK_User;
                    sharing.Status = 0;
                    sharing.LoadedOrEmpty = demand.LoadedOrEmpty;
                    sharing.WantedCount = demand.WantedCount;
                    sharing.PossibleJourneyStartDateTime = demand.PossibleJourneyStartDateTime;
                    sharing.AcceptedCount = demand.WantedCount;
                    sharing.FK_AppUser_Client = demand.FK_AppUser_Client;
                    sharing.FK_ReferenceDepo = demand.FK_ReferenceDepo;
                    sharing.FK_AppUser_Approver = demand.FK_AppUser_Approver;
                    sharing.FK_Depo_From = demand.FK_Depo_From;
                    sharing.StartingLocation = demand.StartingLocation;
                    sharing.FK_Depo_To = demand.FK_Depo_To;
                    sharing.FinishingLocation = demand.FinishingLocation;
                    sharing.DistanceGoogle = demand.DistanceGoogle;
                    sharing.FK_RequisitionVehicleType = demand.FK_RequisitionVehicleType;
                    sharing.VehicleType = demand.VehicleType;
                    sharing.VehicleCapacity = demand.VehicleCapacity;

                    //sharing.TrackingID = demand.TrackingID.Replace('D', 'S');
                    var TrackingPattern = bll.db.Depoes.Where(m => m.PK_Depo == demand.FK_Depo_From).FirstOrDefault().Code + "-" + bll.db.Depoes.Where(m => m.PK_Depo == demand.FK_Depo_To).FirstOrDefault().Code + "-"
                        + DateTime.Now.Month.ToString("00") + "-"
                        + DateTime.Now.ToString("yy") + "-";
                    var existingCount = bll.db.VehicleSharings.Where(m => m.TrackingID.Contains(TrackingPattern)).Count();
                    sharing.TrackingID = "E-" + TrackingPattern + ((existingCount + 1).ToString("00000"));

                    bll.db.VehicleSharings.Add(sharing);
                    bll.db.SaveChanges();

                    demand.Status = 2;
                    demand.IsHeadDemand = true;
                    demand.SharedAt = sharing.CreatedAt;
                    demand.FK_VehicleSharing = sharing.PK_VehicleSharing;
                    bll.db.SaveChanges();


                    //#Create Empty trip
                    var FK_Vheicle = Guid.Parse(form["FK_Vehicle"]);
                    var _vehicle = bll.db.Vehicles.Where(m => m.PK_Vehicle == FK_Vheicle).FirstOrDefault();
                    var FK_Driver = Guid.Parse(form["FK_Driver"]);
                    var _driver = bll.db.AppUsers.Where(m => m.PK_User == FK_Driver).FirstOrDefault();
                    if ((_driver.FK_VehicleSharingInternalTrip_Pending == null) &&
                        ((_vehicle.FK_VehicleSharingInternalTrip_Pending == null && _vehicle.FK_VehicleSharingInternalTrip_Current == null)
                        || (_vehicle.LocationInOrOut == true && _vehicle.FK_LocationInOut == demand.FK_Depo_From && _vehicle.FK_VehicleSharingInternalTrip_Pending == null && _vehicle.FK_VehicleSharingInternalTrip_Current == null))
                        || (demand.FK_Depo_From == _vehicle.VehicleSharingInternalTrip1.VehicleSharing.FK_Depo_To && _vehicle.FK_VehicleSharingInternalTrip_Pending == null)
                        )
                    {
                        var internalTrip = new VehicleSharingInternalTrip();
                        internalTrip.AssingedAt = DateTime.Now;
                        internalTrip.FK_VehicleSharing = sharing.PK_VehicleSharing;
                        internalTrip.FK_Vehicle = FK_Vheicle;
                        internalTrip.FK_AppUser_Driver = FK_Driver;
                        internalTrip.FK_AppUser_Assigner = CurrentUser.PK_User;
                        internalTrip.IsDeleted = false;
                        internalTrip.IsTest = true;
                        //internalTrip.PossibleJourneyFinishDateTime = DateTime.ParseExact(InternalVehicle.PossiblleReachingTime, "yyyy-MM-dd hh:mm tt", CultureInfo.InvariantCulture); ;
                        internalTrip.AssingedAt = DateTime.Now;
                        internalTrip.StatusText = InternalTripStatus.Assigned;
                        if (_vehicle.LocationInOrOut == true && _vehicle.FK_LocationInOut == demand.FK_Depo_From && _vehicle.FK_VehicleSharingInternalTrip_Pending == null && _vehicle.FK_VehicleSharingInternalTrip_Current == null)
                        {
                            internalTrip.StatusText = InternalTripStatus.EnteredStartingLocation;
                        }

                        bll.db.VehicleSharingInternalTrips.Add(internalTrip);
                        bll.db.SaveChanges();
                        _vehicle.FK_VehicleSharingInternalTrip_Pending = internalTrip.PK_VehicleSharingInternalTrip;
                        _driver.FK_VehicleSharingInternalTrip_Pending = internalTrip.PK_VehicleSharingInternalTrip;
                        bll.db.SaveChanges();
                        dbContextTransaction.Commit();

                        //-Notify Driver Firebase
                        var vehicle = bll.db.Vehicles.Where(m => m.PK_Vehicle == internalTrip.FK_Vehicle).FirstOrDefault();
                        if (!string.IsNullOrEmpty(vehicle.FID))
                        {
                            //var title = "Driver: New Internal Trip #" + bll.db.VehicleSharings.Where(m => m.PK_VehicleSharing == internalTrip.FK_VehicleSharing).FirstOrDefault().TrackingID + " Created";
                            var title = "সম্মনিত চালক, নতুন খালি যাত্রার (#" + bll.db.VehicleSharings.Where(m => m.PK_VehicleSharing == internalTrip.FK_VehicleSharing).FirstOrDefault().TrackingID + ") তথ্য দেখুন।";
                            var message = "Dear Concern \n";
                            message = message + "Trip Detail" + "\n"
                                + "Vehicle: " + internalTrip.Vehicle.RegistrationNumber + "\n"
                                + "From: " + internalTrip.VehicleSharing.Depo.Name + "\n"
                                + "To: " + internalTrip.VehicleSharing.Depo1.Name + "\n"
                                + "On: " + internalTrip.VehicleSharing.PossibleJourneyStartDateTime + "\n";
                            SendFCM_Notification_Single_New(vehicle.FID, title, message, internalTrip.PK_VehicleSharingInternalTrip.ToString(), "VehicleSharingInternalTrip");
                        }
                        CreateAlertMessage(AlertMessageType.Success, "Success", "Vehicle internal empty trip is successfully added.");
                    }
                    else
                    {
                        dbContextTransaction.Rollback();
                        CreateAlertMessage(AlertMessageType.Information, "Information", "Vehicle internal empty trip is could not be created for reasson(s).");
                    }

                }
            }
            catch (Exception exception)
            {
                ViewBag.RequisitionVehicleTypes = new SelectList(bll.db.RequisitionVehicleTypes.Where(m => m.IsDeleted != true), "PK_RequisitionVehicleType", "Title_English", demand.FK_RequisitionVehicleType);
                ViewBag.Locations1 = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Depo", "Name", demand.FK_Depo_From);
                ViewBag.Locations2 = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Depo", "Name", demand.FK_Depo_To);
                var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
                ViewBag.Vehicles = new SelectList(bll.db.Vehicles.Where(m => m.OWN_MHT_DHT == "OWN" && accessibleDepoes.Contains(m.FK_Depo)).Select(m => new { m.PK_Vehicle, m.RegistrationNumber }), "PK_Vehicle", "RegistrationNumber", Guid.Parse(form["PK_Vehicle"]));
                var _today = DateTime.Now.Date;
                ViewBag.AlreadyOpenedCount = bll.db.VehicleSharingDemands.Where(m => m.FK_AppUser_Client == CurrentUser.PK_User && m.CreatedAt > _today).Count();
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                return View(demand);
            }
            return RedirectToAction("VehicleSharingInternalTripIndex");
        }

        //# External Trip
        //#Documentation
        /* 
         * Assigner = AppUser
         * BillApprover = AppUser1
         * BillPayer = AppUser2
         */
        public ActionResult VehicleSharingApprovedBiddingIndex(DateTime? StartingDate, DateTime? EndingDate)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var list = new List<VehicleSharingBidding>();
            var now = DateTime.Now;
            var today7 = DateTime.Now.Date.AddHours(7);
            var query = bll.db.VehicleSharingBiddings.Where(m => m.StatusText == VehicleSharingBiddingStatus.Approved).AsEnumerable()/*.Where(m => m.Status == 1)*/;
            if (StartingDate != null)
            {
                var _StartingDate = StartingDate != null ? StartingDate : new DateTime();
                //query = query.Where(m => m.CreatedAt >= _StartingDate);
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", _StartingDate);
            }
            else
            {
                if (now > today7)
                {
                    ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", today7);
                }
                else
                {
                    ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", today7.AddDays(-1));
                }
            }
            if (EndingDate != null)
            {
                var _EndingDate = EndingDate != null ? EndingDate : new DateTime();
                //query = query.Where(m => m.CreatedAt <= _EndingDate);
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", _EndingDate);
            }
            else
            {
                if (now > today7)
                {
                    ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", today7.AddDays(1));
                }
                else
                {
                    ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", today7);
                }
            }
            if (StartingDate != null || EndingDate != null)
            {
                list = query.OrderByDescending(m => m.VerifiedAt).ToList();
            }
            return View(list);
        }

        public ActionResult VehicleSharingExternalTripView(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.VehicleSharingExternalTrips.AsEnumerable().Where(m => m.PK_VehicleSharingExternalTrip == id).FirstOrDefault();
            return View(model);
        }

        public ActionResult VehicleSharingBiddingAssign(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.VehicleSharingBiddings.Where(m => m.PK_VehicleSharingBidding == id).FirstOrDefault();
            if (model != null)
            {
                return View(model);
            }
            else
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", "Bidding not found.");
                return RedirectToAction("VehicleSharingApprovedBiddingIndex");
            }
        }
        [HttpPost]
        public ActionResult VehicleSharingBiddingAssign(FormCollection formCollection)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var PK_VehicleSharingBidding = Convert.ToInt64(formCollection["PK_VehicleSharingBidding"]);
            var FK_Vehicle = Guid.Parse(formCollection["FK_Vehicle"]);
            var bidding = bll.db.VehicleSharingBiddings.Where(m => m.PK_VehicleSharingBidding == PK_VehicleSharingBidding).FirstOrDefault();
            //check exceeds count
            if (bidding.ApprovedQuantity == bidding.VehicleSharingExternalTrips.Count())
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", "Already assigned approved qauntity of vehicle.");
                return RedirectToAction("VehicleSharingApprovedBiddingIndex");
            }
            //check already added this vehicle
            else if (bidding.VehicleSharingExternalTrips.Where(m => m.FK_Vehicle == FK_Vehicle).Any())
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", "This vehicle already assigned.");
                return RedirectToAction("VehicleSharingApprovedBiddingIndex");
            }
            else
            {
                var vehicle = bll.db.Vehicles.Where(m => m.PK_Vehicle == FK_Vehicle).FirstOrDefault();
                var externalTrip = new VehicleSharingExternalTrip();
                externalTrip.FK_Vehicle = FK_Vehicle;
                externalTrip.MHT_DHT_DriverName = vehicle.MHT_DHT_DriverName;
                externalTrip.MHT_DHT_DriverContactNumber = vehicle.MHT_DHT_DriverContactNumber;
                externalTrip.MHT_DHT_DriverLiceneseNumber = vehicle.MHT_DHT_DriverLiceneseNumber;
                externalTrip.FK_VehicleSharingBidding = PK_VehicleSharingBidding;
                externalTrip.FK_AppUser_Assigner = CurrentUser.PK_User;
                externalTrip.AssingedAt = DateTime.Now;
                externalTrip.StatusText = ExternalTripStatus.Assigned;
                bll.db.VehicleSharingExternalTrips.Add(externalTrip);
                bll.db.SaveChanges();
                vehicle.FK_VehicleSharingExternalTrip_Current = externalTrip.PK_VehicleSharingExternalTrip;
                //-Notify Requisitor Mail
                foreach (var _demand in bidding.VehicleSharing.VehicleSharingDemands)
                {
                    _demand.Status = 3;
                    if (!string.IsNullOrEmpty(_demand.AppUser.Email))
                    {
                        var Mail_Subject = "Requisitior: New External Trip #" + bidding.VehicleSharing.TrackingID + " Created";
                        var Mail_Body = "Dear Concern<br>";
                        Mail_Body = Mail_Body + "Trip Detail" + " <br>"
                            + "Vehicle: " + vehicle.RegistrationNumber + "<br>"
                            + "From: " + externalTrip.VehicleSharingBidding.VehicleSharing.Depo.Name + "<br>"
                            + "To: " + externalTrip.VehicleSharingBidding.VehicleSharing.Depo1.Name + "<br>"
                            + "On: " + externalTrip.VehicleSharingBidding.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
                        //SendMail_Single(_demand.AppUser.Email, Mail_Subject, Mail_Body);
                    }
                }

                //- Check from temporary vehicles
                var temporaryVehicle = bll.db.TemporaryVehicles.Where(m => m.RegistrationNumber == vehicle.RegistrationNumber).FirstOrDefault();
                if (temporaryVehicle != null)
                {
                    externalTrip.StatusText = ExternalTripStatus.EnteredStartingLocation;
                    externalTrip.EnteredStartingLocationDateTime = temporaryVehicle.IssueDateTime;
                    bll.db.TemporaryVehicles.Remove(temporaryVehicle);
                }
                bll.db.SaveChanges();

                if ((bidding.ApprovedQuantity - bidding.VehicleSharingExternalTrips.Count()) > 0)
                {
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Successfully assigned external vheicle.");
                    return RedirectToAction("VehicleSharingBiddingAssign", new { id = PK_VehicleSharingBidding });
                }
                else
                {
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Successfully assigned external vheicle.");
                    return RedirectToAction("VehicleSharingApprovedBiddingIndex");
                }
            }
        }

        public ActionResult VehicleSharingExternalTripIndex(DateTime? StartingDate, DateTime? EndingDate)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            ViewBag.DisplayMessage = bll.db.DisplayMessages.Where(m => m.PK_DisplayMessage == "InterCompanyRequisition/IndexBy_Client").Select(m => m.Message).FirstOrDefault();
            if (StartingDate != null && EndingDate != null)
            {
                var _StartingDate = StartingDate != null ? StartingDate : new DateTime();
                var _EndingDate = EndingDate != null ? EndingDate : new DateTime();
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", _StartingDate);
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", _EndingDate);
                var list = bll.db.VehicleSharingExternalTrips.AsEnumerable()/*.Where(m => m.IsDeleted != true && m.FK_AppUser_Assigner == CurrentUser.PK_User && m.AssingedAt >= _StartingDate && m.AssingedAt <= _EndingDate)*/.OrderByDescending(m => m.AssingedAt).ToList();
                return View(list);
            }
            else
            {
                var now = DateTime.Now;
                var today7 = DateTime.Now.Date.AddHours(7);
                if (now > today7)
                {
                    StartingDate = today7;
                    EndingDate = today7.AddDays(1);
                }
                else
                {
                    StartingDate = today7.AddDays(-1);
                    EndingDate = today7;
                }
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", StartingDate);
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", EndingDate);
                var list = new List<VehicleSharingExternalTrip>();
                return View(list);
            }
        }

        public ActionResult VehicleSharingExternalTripPrintBill(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.VehicleSharingExternalTrips.Find(id);
            if (model != null)
            {
                return View(model);
            }
            else
            {
                return HttpNotFound();
            }

        }
        public JsonResult VehicleSharingExternalTripUpdatePrintCopy(Int64 id)
        {
            var model = bll.db.VehicleSharingExternalTrips.Find(id);
            if (model.PrintCopy == null || model.PrintCopy == 0)
            {
                model.PrintCopy = 1;
            }
            else
            {
                model.PrintCopy = model.PrintCopy + 1;
            }
            bll.db.SaveChanges();
            return Json(new { model.PrintCopy }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VehicleSharingExternalTripApproveBill(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.VehicleSharingExternalTrips.Where(m => m.PK_VehicleSharingExternalTrip == id).FirstOrDefault();
            if (model != null)
            {
                return View(model);
            }
            else
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", "Internal trip not found.");
                return RedirectToAction("VehicleSharingInternalTripIndex");
            }
        }
        [HttpPost]
        public ActionResult VehicleSharingExternalTripApproveBill(VehicleSharingExternalTrip model)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var db_model = bll.db.VehicleSharingExternalTrips.Where(m => m.PK_VehicleSharingExternalTrip == model.PK_VehicleSharingExternalTrip).FirstOrDefault();
            if (db_model != null)
            {
                if (db_model.PaymentStatusText == null)
                {
                    db_model.PaymentStatusText = ExternalTripPaymentStatus.ApprovedBill;
                    db_model.BillApprovedAt = DateTime.Now;
                    db_model.FK_AppUser_BillApprover = CurrentUser.PK_User;
                    bll.db.SaveChanges();
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Internal trip bill is successfully approved.");
                }
                else
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", "Internal trip bill is already approved.");
                }
                return RedirectToAction("VehicleSharingExternalTripIndex");
            }
            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult VehicleSharingExternalTripPayBill(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.VehicleSharingExternalTrips.Where(m => m.PK_VehicleSharingExternalTrip == id).FirstOrDefault();
            if (model != null)
            {
                return View(model);
            }
            else
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", "Internal trip not found.");
                return RedirectToAction("VehicleSharingInternalTripIndex");
            }
        }
        [HttpPost]
        public ActionResult VehicleSharingExternalTripPayBill(VehicleSharingExternalTrip model)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var db_model = bll.db.VehicleSharingExternalTrips.Where(m => m.PK_VehicleSharingExternalTrip == model.PK_VehicleSharingExternalTrip).FirstOrDefault();
            if (db_model != null)
            {
                if (db_model.PaymentStatusText == ExternalTripPaymentStatus.ApprovedBill)
                {
                    db_model.PaymentStatusText = ExternalTripPaymentStatus.PaidBill;
                    db_model.BillPaidAt = DateTime.Now;
                    db_model.FK_AppUser_BillPaidBy = CurrentUser.PK_User;
                    bll.db.SaveChanges();
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Internal trip bill is successfully paid.");
                }
                else
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", "Internal trip bill is already paid.");
                }
                return RedirectToAction("VehicleSharingExternalTripIndex");
            }
            else
            {
                return HttpNotFound();
            }
        }


        //# Gate In Out
        //public ActionResult VehicleEnterDepo(Guid? id)
        //{
        //    if (CommonClass.IsInvalidAccess())
        //    {
        //        return Redirect("/Access/Login");
        //    }
        //    var vehicle = new Vehicle();
        //    if (id != null)
        //    {
        //        vehicle = bll.db.Vehicles.Where(m => m.PK_Vehicle == id).FirstOrDefault();
        //    }
        //    ViewBag.OwnVehicles = new SelectList(bll.db.Vehicles.Where(m => m.IsDeleted == false), "PK_Vehicle", "RegistrationNumber", vehicle.PK_Vehicle);
        //    ViewBag.Locations = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false), "PK_Depo", "Name", vehicle.FK_DepoInOut);
        //    //var InOrOut = new Dictionary<bool, string> { { false, "Out" }, { true, "In" } };
        //    var InOrOut = new Dictionary<bool, string> { { false, "Out" } };
        //    ViewBag.InOrOut = new SelectList(InOrOut, "Key", "Value", vehicle.DepoInOrOut);
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult _VehicleEnterDepo(FormCollection form)
        //{
        //    if (CommonClass.IsInvalidAccess())
        //    {
        //        return Redirect("/Access/Login");
        //    }
        //    var model = new VehicleInOutManual();
        //    //model.DevelopersNote = "test";
        //    model.FK_Vehicle = Guid.Parse(form["FK_Vehicle"]);
        //    model.FK_Depo = Guid.Parse(form["FK_Depo"]);
        //    model.InOrOut = bool.Parse(form["InOut"]);
        //    model.CreatedAt = DateTime.Parse(form["CreatedAt"]);
        //    model.FK_CreatedByUser = CurrentUser.PK_User;
        //    bll.db.VehicleInOutManuals.Add(model);

        //    var _fk_vehicle = Guid.Parse(form["FK_Vehicle"]);
        //    var vehicle = bll.db.Vehicles.Where(m => m.PK_Vehicle == _fk_vehicle).FirstOrDefault();
        //    vehicle.FK_DepoInOut = model.FK_Depo;
        //    vehicle.DepoInOutTime = model.CreatedAt;
        //    vehicle.DepoInOrOut = model.InOrOut;
        //    bll.db.SaveChanges();

        //    if (model.InOrOut == true)
        //    {
        //        var currentTrip = bll.db.VehicleSharingInternalTrips.Where(m => m.PK_VehicleSharingInternalTrip == vehicle.FK_VehicleSharingInternalTrip_Current).FirstOrDefault();
        //        //# Entered with a Current Trip 
        //        if (currentTrip != null && currentTrip.StatusText == InternalTripStatus.LeftStartingLoaction && currentTrip.VehicleSharing.Depo1.PK_Depo == model.FK_Depo)
        //        {
        //            if (currentTrip.VehicleSharing.LoadedOrEmpty == true)
        //            {
        //                currentTrip.StatusText = InternalTripStatus.StartedUnloading;
        //                currentTrip.EnteredFinishingLocationDateTime = DateTime.Now;
        //                currentTrip.UnloadingStartDateTime = DateTime.Now;
        //                //-Notify Driver Firebase
        //                //var vehicle = bll.db.Vehicles.Where(m => m.PK_Vehicle == currentTrip.FK_Vehicle).FirstOrDefault();
        //                if (!string.IsNullOrEmpty(vehicle.FID))
        //                {
        //                    var _FK_Depo_To = currentTrip.VehicleSharing.VehicleSharingDemands.Where(n => n.IsHeadDemand == true).FirstOrDefault().FK_Depo_To;
        //                    //var title = "Driver: Your Vehicle Entered in Current Trip #" + currentTrip.VehicleSharing.TrackingID + " Destination: " + bll.db.Depoes.Where(m => m.PK_Depo == _FK_Depo_To).FirstOrDefault().Name;
        //                    var title = "সম্মনিত চালক, গাড়ি চলমান যাত্রার (#" + currentTrip.VehicleSharing.TrackingID + ") গন্তব্যে (" + bll.db.Depoes.Where(m => m.PK_Depo == _FK_Depo_To).FirstOrDefault().Name + ") পৌছিয়েছে, আনলোডিং-এর সময় গণনা শুরু হয়েছে।";
        //                    var message = "Dear Concern \n";
        //                    message = message + "Trip Detail" + "\n"
        //                        + "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + "\n"
        //                        + "From: " + currentTrip.VehicleSharing.Depo.Name + "\n"
        //                        + "To: " + currentTrip.VehicleSharing.Depo1.Name + "\n"
        //                        + "On: " + currentTrip.VehicleSharing.PossibleJourneyStartDateTime + "\n";
        //                    SendFCM_Notification_Single_New(vehicle.FID, title, message, currentTrip.PK_VehicleSharingInternalTrip.ToString(), "VehicleSharingInternalTrip");
        //                }
        //                //-Notify Assigner Mail
        //                if (!string.IsNullOrEmpty(currentTrip.AppUser1.Email))
        //                {
        //                    var Mail_Subject = "Assigner: Internal trip's unloading is started";
        //                    var Mail_Body = "Dear Concern<br>";
        //                    Mail_Body = Mail_Body + "Trip Detail" + " <br>"
        //                        + "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + "<br>"
        //                        + "From: " + currentTrip.VehicleSharing.Depo.Name + "<br>"
        //                        + "To: " + currentTrip.VehicleSharing.Depo1.Name + "<br>"
        //                        + "On: " + currentTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
        //                    SendMail_Single(currentTrip.AppUser.Email, Mail_Subject, Mail_Body);
        //                }
        //                //-Notify Requisitor Mail
        //                foreach (var _demand in currentTrip.VehicleSharing.VehicleSharingDemands)
        //                {
        //                    if (!string.IsNullOrEmpty(_demand.AppUser.Email))
        //                    {
        //                        var Mail_Subject = "Requisitior: Internal trip's unloading is started";
        //                        var Mail_Body = "Dear Concern<br>";
        //                        Mail_Body = Mail_Body + "Trip Detail" + " <br>"
        //                            + "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + "<br>"
        //                            + "From: " + currentTrip.VehicleSharing.Depo.Name + "<br>"
        //                            + "To: " + currentTrip.VehicleSharing.Depo1.Name + "<br>"
        //                            + "On: " + currentTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
        //                        SendMail_Single(_demand.AppUser.Email, Mail_Subject, Mail_Body);
        //                    }
        //                }
        //                bll.db.SaveChanges();
        //            }
        //            else
        //            {
        //                currentTrip.StatusText = InternalTripStatus.FinishedEmptyTrip;
        //                currentTrip.EnteredFinishingLocationDateTime = DateTime.Now;
        //                currentTrip.UnloadingStartDateTime = DateTime.Now;
        //                currentTrip.UnloadingDoneDateTime = DateTime.Now;



        //                // Update Vehicle Trip
        //                currentTrip.Vehicle.FK_VehicleSharingInternalTrip_Current = null;

        //                // Update Driver Trip
        //                currentTrip.AppUser.FK_VehicleSharingInternalTrip_Current = null;

        //                //-Notify Driver Firebase
        //                //var vehicle = bll.db.Vehicles.Where(m => m.PK_Vehicle == currentTrip.FK_Vehicle).FirstOrDefault();
        //                if (!string.IsNullOrEmpty(vehicle.FID))
        //                {
        //                    var _FK_Depo_To = currentTrip.VehicleSharing.VehicleSharingDemands.Where(n => n.IsHeadDemand == true).FirstOrDefault().FK_Depo_To;
        //                    //var title = "Driver: Your Vehicle Entered in Current Trip #" + currentTrip.VehicleSharing.TrackingID + " Destination: " + bll.db.Depoes.Where(m => m.PK_Depo == _FK_Depo_To).FirstOrDefault().Name;
        //                    var title = "সম্মনিত চালক, গাড়ি চলমান খালি যাত্রার (#" + currentTrip.VehicleSharing.TrackingID + ") গন্তব্যে (" + bll.db.Depoes.Where(m => m.PK_Depo == _FK_Depo_To).FirstOrDefault().Name + ") পৌছিয়েছে।";
        //                    var message = "Dear Concern \n";
        //                    message = message + "Trip Detail" + "\n"
        //                        + "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + "\n"
        //                        + "From: " + currentTrip.VehicleSharing.Depo.Name + "\n"
        //                        + "To: " + currentTrip.VehicleSharing.Depo1.Name + "\n"
        //                        + "On: " + currentTrip.VehicleSharing.PossibleJourneyStartDateTime + "\n";
        //                    SendFCM_Notification_Single_New(vehicle.FID, title, message, currentTrip.PK_VehicleSharingInternalTrip.ToString(), "VehicleSharingInternalTrip");
        //                }
        //                //-Notify Assigner Mail
        //                if (!string.IsNullOrEmpty(currentTrip.AppUser1.Email))
        //                {
        //                    var Mail_Subject = "Assigner: Internal empty trip completed";
        //                    var Mail_Body = "Dear Concern<br>";
        //                    Mail_Body = Mail_Body + "Trip Detail" + " <br>"
        //                        + "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + "<br>"
        //                        + "From: " + currentTrip.VehicleSharing.Depo.Name + "<br>"
        //                        + "To: " + currentTrip.VehicleSharing.Depo1.Name + "<br>"
        //                        + "On: " + currentTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
        //                    SendMail_Single(currentTrip.AppUser.Email, Mail_Subject, Mail_Body);
        //                }
        //                //-Notify Requisitor Mail
        //                foreach (var _demand in currentTrip.VehicleSharing.VehicleSharingDemands)
        //                {
        //                    if (!string.IsNullOrEmpty(_demand.AppUser.Email))
        //                    {
        //                        var Mail_Subject = "Assigner: Internal empty trip completed";
        //                        var Mail_Body = "Dear Concern<br>";
        //                        Mail_Body = Mail_Body + "Trip Detail" + " <br>"
        //                            + "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + "<br>"
        //                            + "From: " + currentTrip.VehicleSharing.Depo.Name + "<br>"
        //                            + "To: " + currentTrip.VehicleSharing.Depo1.Name + "<br>"
        //                            + "On: " + currentTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
        //                        SendMail_Single(_demand.AppUser.Email, Mail_Subject, Mail_Body);
        //                    }
        //                }
        //                bll.db.SaveChanges();
        //            }
        //        }
        //        var pendingTrip = bll.db.VehicleSharingInternalTrips.Where(m => m.PK_VehicleSharingInternalTrip == vehicle.FK_VehicleSharingInternalTrip_Pending).FirstOrDefault();
        //        if (pendingTrip != null && pendingTrip.StatusText == InternalTripStatus.Assigned && pendingTrip.VehicleSharing.Depo.PK_Depo == model.FK_Depo)
        //        {
        //            if (currentTrip == null || (currentTrip != null && currentTrip.VehicleSharing.LoadedOrEmpty == false))
        //            {
        //                if (pendingTrip.VehicleSharing.LoadedOrEmpty == true)
        //                {
        //                    pendingTrip.StatusText = InternalTripStatus.StartedLoading;
        //                    pendingTrip.LoadingStartDateTime = DateTime.Now;

        //                    // Update Vehicle Trip
        //                    pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Current = pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Pending;
        //                    pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Pending = null;

        //                    // Update Driver Trip
        //                    pendingTrip.AppUser.FK_VehicleSharingInternalTrip_Current = pendingTrip.AppUser.FK_VehicleSharingInternalTrip_Pending;
        //                    pendingTrip.AppUser.FK_VehicleSharingInternalTrip_Pending = null;

        //                    //-Notify Driver Firebase
        //                    if (!string.IsNullOrEmpty(vehicle.FID))
        //                    {
        //                        var _FK_Depo_From = pendingTrip.VehicleSharing.VehicleSharingDemands.Where(n => n.IsHeadDemand == true).FirstOrDefault().FK_Depo_From;
        //                        //var title = "Driver: Your Vehicle Entered in Pending Trip #" + pendingTrip.VehicleSharing.TrackingID + " Strating Location: " + bll.db.Depoes.Where(m => m.PK_Depo == _FK_Depo_From).FirstOrDefault().Name;
        //                        var title = "সম্মনিত চালক, গাড়ি আসন্ন যাত্রার #" + pendingTrip.VehicleSharing.TrackingID + " শুরুর স্থানে (" + bll.db.Depoes.Where(m => m.PK_Depo == _FK_Depo_From).FirstOrDefault().Name + ") প্রবেশ করেছে, লোডিং-এর সময় গণনা শুরু হয়েছে।";
        //                        var message = "Dear Concern \n";
        //                        message = message + "Trip Detail" + "\n"
        //                            + "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + "\n"
        //                            + "From: " + pendingTrip.VehicleSharing.Depo.Name + "\n"
        //                            + "To: " + pendingTrip.VehicleSharing.Depo1.Name + "\n"
        //                            + "On: " + pendingTrip.VehicleSharing.PossibleJourneyStartDateTime + "\n";
        //                        SendFCM_Notification_Single_New(vehicle.FID, title, message, pendingTrip.PK_VehicleSharingInternalTrip.ToString(), "VehicleSharingInternalTrip");
        //                    }
        //                    //-Notify Assigner Mail
        //                    if (!string.IsNullOrEmpty(pendingTrip.AppUser1.Email))
        //                    {
        //                        var Mail_Subject = "Assigner: Internal trip's loading is started";
        //                        var Mail_Body = "Dear Concern<br>";
        //                        Mail_Body = Mail_Body + "Trip Detail" + " <br>"
        //                            + "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + "<br>"
        //                            + "From: " + pendingTrip.VehicleSharing.Depo.Name + "<br>"
        //                            + "To: " + pendingTrip.VehicleSharing.Depo1.Name + "<br>"
        //                            + "On: " + pendingTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
        //                        SendMail_Single(pendingTrip.AppUser.Email, Mail_Subject, Mail_Body);
        //                    }
        //                    //-Notify Requisitor Mail
        //                    foreach (var _demand in pendingTrip.VehicleSharing.VehicleSharingDemands)
        //                    {
        //                        if (!string.IsNullOrEmpty(_demand.AppUser.Email))
        //                        {
        //                            var Mail_Subject = "Requisitior: Internal trip's loading is started";
        //                            var Mail_Body = "Dear Concern<br>";
        //                            Mail_Body = Mail_Body + "Trip Detail" + " <br>"
        //                                + "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + "<br>"
        //                                + "From: " + pendingTrip.VehicleSharing.Depo.Name + "<br>"
        //                                + "To: " + pendingTrip.VehicleSharing.Depo1.Name + "<br>"
        //                                + "On: " + pendingTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
        //                            SendMail_Single(_demand.AppUser.Email, Mail_Subject, Mail_Body);
        //                        }
        //                    }
        //                    bll.db.SaveChanges();
        //                }
        //                else
        //                {
        //                    pendingTrip.StatusText = InternalTripStatus.StartedEmptyTrip;
        //                    pendingTrip.LoadingStartDateTime = DateTime.Now;
        //                    pendingTrip.LoadingDoneDateTime = pendingTrip.LoadingStartDateTime;

        //                    // Update Vehicle Trip
        //                    pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Current = pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Pending;
        //                    pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Pending = null;

        //                    // Update Driver Trip
        //                    pendingTrip.AppUser.FK_VehicleSharingInternalTrip_Current = pendingTrip.AppUser.FK_VehicleSharingInternalTrip_Pending;
        //                    pendingTrip.AppUser.FK_VehicleSharingInternalTrip_Pending = null;

        //                    //-Notify Driver Firebase
        //                    if (!string.IsNullOrEmpty(vehicle.FID))
        //                    {
        //                        var _FK_Depo_From = pendingTrip.VehicleSharing.VehicleSharingDemands.Where(n => n.IsHeadDemand == true).FirstOrDefault().FK_Depo_From;
        //                        //var title = "Driver: Your Vehicle Entered in Pending Trip #" + pendingTrip.VehicleSharing.TrackingID + " Strating Location: " + bll.db.Depoes.Where(m => m.PK_Depo == _FK_Depo_From).FirstOrDefault().Name;
        //                        var title = "সম্মনিত চালক, গাড়ি আসন্ন খালি যাত্রার #" + pendingTrip.VehicleSharing.TrackingID + " শুরুর স্থানে (" + bll.db.Depoes.Where(m => m.PK_Depo == _FK_Depo_From).FirstOrDefault().Name + ") প্রবেশ করেছে, যাত্রার সময় গণনা শুরু হয়েছে।";
        //                        var message = "Dear Concern \n";
        //                        message = message + "Trip Detail" + "\n"
        //                            + "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + "\n"
        //                            + "From: " + pendingTrip.VehicleSharing.Depo.Name + "\n"
        //                            + "To: " + pendingTrip.VehicleSharing.Depo1.Name + "\n"
        //                            + "On: " + pendingTrip.VehicleSharing.PossibleJourneyStartDateTime + "\n";
        //                        SendFCM_Notification_Single_New(vehicle.FID, title, message, pendingTrip.PK_VehicleSharingInternalTrip.ToString(), "VehicleSharingInternalTrip");
        //                    }
        //                    //-Notify Assigner Mail
        //                    if (!string.IsNullOrEmpty(pendingTrip.AppUser1.Email))
        //                    {
        //                        var Mail_Subject = "Assigner: Internal empty trip started";
        //                        var Mail_Body = "Dear Concern<br>";
        //                        Mail_Body = Mail_Body + "Trip Detail" + " <br>"
        //                            + "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + "<br>"
        //                            + "From: " + pendingTrip.VehicleSharing.Depo.Name + "<br>"
        //                            + "To: " + pendingTrip.VehicleSharing.Depo1.Name + "<br>"
        //                            + "On: " + pendingTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
        //                        SendMail_Single(pendingTrip.AppUser.Email, Mail_Subject, Mail_Body);
        //                    }
        //                    //-Notify Requisitor Mail
        //                    foreach (var _demand in pendingTrip.VehicleSharing.VehicleSharingDemands)
        //                    {
        //                        if (!string.IsNullOrEmpty(_demand.AppUser.Email))
        //                        {
        //                            var Mail_Subject = "Requisitior: Internal empty trip started";
        //                            var Mail_Body = "Dear Concern<br>";
        //                            Mail_Body = Mail_Body + "Trip Detail" + " <br>"
        //                                + "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + "<br>"
        //                                + "From: " + pendingTrip.VehicleSharing.Depo.Name + "<br>"
        //                                + "To: " + pendingTrip.VehicleSharing.Depo1.Name + "<br>"
        //                                + "On: " + pendingTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
        //                            SendMail_Single(_demand.AppUser.Email, Mail_Subject, Mail_Body);
        //                        }
        //                    }
        //                    bll.db.SaveChanges();
        //                }
        //            }
        //            else
        //            {
        //                pendingTrip.StatusText = InternalTripStatus.EnteredStartingLocation;
        //                bll.db.SaveChanges();
        //            }
        //        }
        //    }
        //    if (model.InOrOut == false)
        //    {
        //        var currentTrip = bll.db.VehicleSharingInternalTrips.Where(m => m.PK_VehicleSharingInternalTrip == vehicle.FK_VehicleSharingInternalTrip_Current).FirstOrDefault();
        //        if (currentTrip != null && currentTrip.StatusText == InternalTripStatus.PaidBill && currentTrip.VehicleSharing.Depo.PK_Depo == model.FK_Depo)
        //        {
        //            currentTrip.StatusText = InternalTripStatus.LeftStartingLoaction;
        //            currentTrip.LeftStartingLoactionDateTime = DateTime.Now;

        //            var from_to = currentTrip.VehicleSharing.VehicleSharingDemands.Where(n => n.IsHeadDemand == true).Select(m => new { m.FK_Depo_From, m.FK_Depo_To }).FirstOrDefault();
        //            var approxTimeHour = bll.db.RouteCharts.Where(m => (m.FK_Depo1 == from_to.FK_Depo_From && m.FK_Depo2 == from_to.FK_Depo_To) || (m.FK_Depo1 == from_to.FK_Depo_To && m.FK_Depo2 == from_to.FK_Depo_From)).FirstOrDefault().ApproxTimeHour;
        //            currentTrip.PossibleJourneyFinishDateTime = DateTime.Now.AddHours(approxTimeHour);

        //            //-Notify Driver Firebase
        //            //var vehicle = bll.db.Vehicles.Where(m => m.PK_Vehicle == currentTrip.FK_Vehicle).FirstOrDefault();
        //            if (!string.IsNullOrEmpty(vehicle.FID))
        //            {
        //                var _FK_Depo_From = currentTrip.VehicleSharing.VehicleSharingDemands.Where(n => n.IsHeadDemand == true).FirstOrDefault().FK_Depo_From;
        //                //var title = "Driver: Your Vehicle Left Current Trip #" + currentTrip.VehicleSharing.TrackingID + " Staring Location: " + bll.db.Depoes.Where(m => m.PK_Depo == _FK_Depo_From).FirstOrDefault().Name;
        //                var title = "সম্মনিত চালক, গাড়ি চলমান যাত্রার (#" + currentTrip.VehicleSharing.TrackingID + ") শুরুর স্থান (" + bll.db.Depoes.Where(m => m.PK_Depo == _FK_Depo_From).FirstOrDefault().Name + ") ত্যাগ করেছে।";
        //                var message = "Dear Concern \n";
        //                message = message + "Trip Detail" + "\n"
        //                    + "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + "\n"
        //                    + "From: " + currentTrip.VehicleSharing.Depo.Name + "\n"
        //                    + "To: " + currentTrip.VehicleSharing.Depo1.Name + "\n"
        //                    + "On: " + currentTrip.VehicleSharing.PossibleJourneyStartDateTime + "\n";
        //                SendFCM_Notification_Single_New(vehicle.FID, title, message, currentTrip.PK_VehicleSharingInternalTrip.ToString(), "VehicleSharingInternalTrip");
        //            }
        //            //-Notify Assigner Mail
        //            if (!string.IsNullOrEmpty(currentTrip.AppUser1.Email))
        //            {
        //                var Mail_Subject = "Assigner: Internal trip's vehicle left starting location";
        //                var Mail_Body = "Dear Concern<br>";
        //                Mail_Body = Mail_Body + "Trip Detail" + " <br>"
        //                    + "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + "<br>"
        //                    + "From: " + currentTrip.VehicleSharing.Depo.Name + "<br>"
        //                    + "To: " + currentTrip.VehicleSharing.Depo1.Name + "<br>"
        //                    + "On: " + currentTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
        //                SendMail_Single(currentTrip.AppUser.Email, Mail_Subject, Mail_Body);
        //            }
        //            //-Notify Requisitor Mail
        //            foreach (var _demand in currentTrip.VehicleSharing.VehicleSharingDemands)
        //            {
        //                if (!string.IsNullOrEmpty(_demand.AppUser.Email))
        //                {
        //                    var Mail_Subject = "Requisitior: Internal trip's vehicle left starting location";
        //                    var Mail_Body = "Dear Concern<br>";
        //                    Mail_Body = Mail_Body + "Trip Detail" + " <br>"
        //                        + "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + "<br>"
        //                        + "From: " + currentTrip.VehicleSharing.Depo.Name + "<br>"
        //                        + "To: " + currentTrip.VehicleSharing.Depo1.Name + "<br>"
        //                        + "On: " + currentTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
        //                    SendMail_Single(_demand.AppUser.Email, Mail_Subject, Mail_Body);
        //                }
        //            }
        //            bll.db.SaveChanges();
        //        }
        //    }

        //    CreateAlertMessage(AlertMessageType.Success, "Success", "successfully added.");
        //    ViewBag.OwnVehicles = new SelectList(bll.db.Vehicles.Where(m => m.IsDeleted == false && m.OWN_MHT_DHT == "OWN"), "PK_Vehicle", "RegistrationNumber", model.FK_Vehicle);
        //    ViewBag.Locations = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false), "PK_Depo", "Name", model.FK_Depo);
        //    var InOrOut = new Dictionary<bool, string> { { false, "Out" }, { true, "In" } };
        //    ViewBag.InOrOut = new SelectList(InOrOut, "Key", "Value", model.InOrOut);
        //    return RedirectToAction("VehicleEnterDepo", new { id = form["FK_Vehicle"] });
        //}

        //[HttpPost]
        //public ActionResult VehicleEnterDepo(FormCollection form)
        //{
        //    if (CommonClass.IsInvalidAccess())
        //    {
        //        return Redirect("/Access/Login");
        //    }
        //    var FK_Vehicle = Guid.Parse(form["FK_Vehicle"]);
        //    var FK_Depo = Guid.Parse(form["FK_Depo"]);
        //    var InOut = bool.Parse(form["InOut"]);

        //    if (InOut == true)
        //    {
        //        return Gate_In(FK_Vehicle, FK_Depo);
        //    }
        //    else
        //    {
        //        return Gate_Out(FK_Vehicle, FK_Depo);
        //    }
        //}

        //public ActionResult Gate_In(Guid FK_Vehicle, Guid FK_Depo)
        //{
        //    if (CommonClass.IsInvalidAccess())
        //    {
        //        return Redirect("/Access/Login");
        //    }
        //    var model = new VehicleInOutManual();
        //    //model.DevelopersNote = "test";
        //    model.FK_Vehicle = FK_Vehicle;
        //    model.FK_Depo = FK_Depo;
        //    model.InOrOut = true;
        //    model.CreatedAt = DateTime.Now;
        //    model.FK_CreatedByUser = CurrentUser.PK_User;
        //    bll.db.VehicleInOutManuals.Add(model);

        //    var vehicle = bll.db.Vehicles.Where(m => m.PK_Vehicle == FK_Vehicle).FirstOrDefault();
        //    var depo = bll.db.Depoes.Where(m => m.PK_Depo == FK_Depo).FirstOrDefault();
        //    vehicle.FK_DepoInOut = model.FK_Depo;
        //    vehicle.DepoInOutTime = model.CreatedAt;
        //    vehicle.DepoInOrOut = model.InOrOut;
        //    bll.db.SaveChanges();

        //    if (vehicle.OWN_MHT_DHT == "OWN")
        //    {
        //        Gate_OWN_In(vehicle, depo);
        //    }
        //    else if (vehicle.OWN_MHT_DHT != "OWN")
        //    {
        //        Gate_Hired_In(vehicle, depo);
        //    }
        //    CreateAlertMessage(AlertMessageType.Success, "Success", "successfully Gate In entry given.");
        //    return RedirectToAction("VehicleEnterDepo", new { id = FK_Vehicle });
        //}

        //public ActionResult Gate_Out(Guid FK_Vehicle, Guid FK_Depo)
        //{
        //    if (CommonClass.IsInvalidAccess())
        //    {
        //        return Redirect("/Access/Login");
        //    }
        //    var model = new VehicleInOutManual();
        //    //model.DevelopersNote = "test";
        //    model.FK_Vehicle = FK_Vehicle;
        //    model.FK_Depo = FK_Depo;
        //    model.InOrOut = false;
        //    model.CreatedAt = DateTime.Now;
        //    model.FK_CreatedByUser = CurrentUser.PK_User;
        //    bll.db.VehicleInOutManuals.Add(model);

        //    var vehicle = bll.db.Vehicles.Where(m => m.PK_Vehicle == FK_Vehicle).FirstOrDefault();
        //    var depo = bll.db.Depoes.Where(m => m.PK_Depo == FK_Depo).FirstOrDefault();
        //    vehicle.FK_DepoInOut = model.FK_Depo;
        //    vehicle.DepoInOutTime = model.CreatedAt;
        //    vehicle.DepoInOrOut = model.InOrOut;
        //    bll.db.SaveChanges();

        //    if (vehicle.OWN_MHT_DHT == "OWN")
        //    {
        //        Gate_OWN_Out(vehicle, depo);
        //    }
        //    else if (vehicle.OWN_MHT_DHT != "OWN")
        //    {
        //        Gate_Hired_Out(vehicle, depo);
        //    }
        //    CreateAlertMessage(AlertMessageType.Success, "Success", "successfully Gate Out entry given.");
        //    return RedirectToAction("VehicleEnterDepo", new { id = FK_Vehicle });
        //}

        //public void Gate_OWN_In(Vehicle vehicle, Depo depo)
        //{
        //    var currentTrip = bll.db.VehicleSharingInternalTrips.Where(m => m.PK_VehicleSharingInternalTrip == vehicle.FK_VehicleSharingInternalTrip_Current).FirstOrDefault();
        //    //# Entered with a Current Trip 
        //    if (currentTrip != null && currentTrip.StatusText == InternalTripStatus.LeftStartingLoaction && currentTrip.VehicleSharing.Depo1.PK_Depo == depo.PK_Depo)
        //    {
        //        if (currentTrip.VehicleSharing.LoadedOrEmpty == true)
        //        {
        //            currentTrip.StatusText = InternalTripStatus.StartedUnloading;
        //            currentTrip.EnteredFinishingLocationDateTime = DateTime.Now;
        //            currentTrip.UnloadingStartDateTime = DateTime.Now;
        //            //-Notify Driver Firebase
        //            //var vehicle = bll.db.Vehicles.Where(m => m.PK_Vehicle == currentTrip.FK_Vehicle).FirstOrDefault();
        //            if (!string.IsNullOrEmpty(vehicle.FID))
        //            {
        //                var _FK_Depo_To = currentTrip.VehicleSharing.VehicleSharingDemands.Where(n => n.IsHeadDemand == true).FirstOrDefault().FK_Depo_To;
        //                //var title = "Driver: Your Vehicle Entered in Current Trip #" + currentTrip.VehicleSharing.TrackingID + " Destination: " + bll.db.Depoes.Where(m => m.PK_Depo == _FK_Depo_To).FirstOrDefault().Name;
        //                var title = "সম্মনিত চালক, গাড়ি চলমান যাত্রার (#" + currentTrip.VehicleSharing.TrackingID + ") গন্তব্যে (" + bll.db.Depoes.Where(m => m.PK_Depo == _FK_Depo_To).FirstOrDefault().Name + ") পৌছিয়েছে, আনলোডিং-এর সময় গণনা শুরু হয়েছে।";
        //                var message = "Dear Concern \n";
        //                message = message + "Trip Detail" + "\n"
        //                    + "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + "\n"
        //                    + "From: " + currentTrip.VehicleSharing.Depo.Name + "\n"
        //                    + "To: " + currentTrip.VehicleSharing.Depo1.Name + "\n"
        //                    + "On: " + currentTrip.VehicleSharing.PossibleJourneyStartDateTime + "\n";
        //                SendFCM_Notification_Single_New(vehicle.FID, title, message, currentTrip.PK_VehicleSharingInternalTrip.ToString(), "VehicleSharingInternalTrip");
        //            }
        //            //-Notify Assigner Mail
        //            if (!string.IsNullOrEmpty(currentTrip.AppUser1.Email))
        //            {
        //                var Mail_Subject = "Assigner: Internal trip's unloading is started";
        //                var Mail_Body = "Dear Concern<br>";
        //                Mail_Body = Mail_Body + "Trip Detail" + " <br>"
        //                    + "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + "<br>"
        //                    + "From: " + currentTrip.VehicleSharing.Depo.Name + "<br>"
        //                    + "To: " + currentTrip.VehicleSharing.Depo1.Name + "<br>"
        //                    + "On: " + currentTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
        //                SendMail_Single(currentTrip.AppUser.Email, Mail_Subject, Mail_Body);
        //            }
        //            //-Notify Requisitor Mail
        //            foreach (var _demand in currentTrip.VehicleSharing.VehicleSharingDemands)
        //            {
        //                if (!string.IsNullOrEmpty(_demand.AppUser.Email))
        //                {
        //                    var Mail_Subject = "Requisitior: Internal trip's unloading is started";
        //                    var Mail_Body = "Dear Concern<br>";
        //                    Mail_Body = Mail_Body + "Trip Detail" + " <br>"
        //                        + "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + "<br>"
        //                        + "From: " + currentTrip.VehicleSharing.Depo.Name + "<br>"
        //                        + "To: " + currentTrip.VehicleSharing.Depo1.Name + "<br>"
        //                        + "On: " + currentTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
        //                    SendMail_Single(_demand.AppUser.Email, Mail_Subject, Mail_Body);
        //                }
        //            }
        //            bll.db.SaveChanges();
        //        }
        //        else
        //        {
        //            currentTrip.StatusText = InternalTripStatus.FinishedEmptyTrip;
        //            currentTrip.EnteredFinishingLocationDateTime = DateTime.Now;
        //            currentTrip.UnloadingStartDateTime = DateTime.Now;
        //            currentTrip.UnloadingDoneDateTime = DateTime.Now;



        //            // Update Vehicle Trip
        //            currentTrip.Vehicle.FK_VehicleSharingInternalTrip_Current = null;

        //            // Update Driver Trip
        //            currentTrip.AppUser.FK_VehicleSharingInternalTrip_Current = null;

        //            //-Notify Driver Firebase
        //            //var vehicle = bll.db.Vehicles.Where(m => m.PK_Vehicle == currentTrip.FK_Vehicle).FirstOrDefault();
        //            if (!string.IsNullOrEmpty(vehicle.FID))
        //            {
        //                var _FK_Depo_To = currentTrip.VehicleSharing.VehicleSharingDemands.Where(n => n.IsHeadDemand == true).FirstOrDefault().FK_Depo_To;
        //                //var title = "Driver: Your Vehicle Entered in Current Trip #" + currentTrip.VehicleSharing.TrackingID + " Destination: " + bll.db.Depoes.Where(m => m.PK_Depo == _FK_Depo_To).FirstOrDefault().Name;
        //                var title = "সম্মনিত চালক, গাড়ি চলমান খালি যাত্রার (#" + currentTrip.VehicleSharing.TrackingID + ") গন্তব্যে (" + bll.db.Depoes.Where(m => m.PK_Depo == _FK_Depo_To).FirstOrDefault().Name + ") পৌছিয়েছে।";
        //                var message = "Dear Concern \n";
        //                message = message + "Trip Detail" + "\n"
        //                    + "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + "\n"
        //                    + "From: " + currentTrip.VehicleSharing.Depo.Name + "\n"
        //                    + "To: " + currentTrip.VehicleSharing.Depo1.Name + "\n"
        //                    + "On: " + currentTrip.VehicleSharing.PossibleJourneyStartDateTime + "\n";
        //                SendFCM_Notification_Single_New(vehicle.FID, title, message, currentTrip.PK_VehicleSharingInternalTrip.ToString(), "VehicleSharingInternalTrip");
        //            }
        //            //-Notify Assigner Mail
        //            if (!string.IsNullOrEmpty(currentTrip.AppUser1.Email))
        //            {
        //                var Mail_Subject = "Assigner: Internal empty trip completed";
        //                var Mail_Body = "Dear Concern<br>";
        //                Mail_Body = Mail_Body + "Trip Detail" + " <br>"
        //                    + "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + "<br>"
        //                    + "From: " + currentTrip.VehicleSharing.Depo.Name + "<br>"
        //                    + "To: " + currentTrip.VehicleSharing.Depo1.Name + "<br>"
        //                    + "On: " + currentTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
        //                SendMail_Single(currentTrip.AppUser.Email, Mail_Subject, Mail_Body);
        //            }
        //            //-Notify Requisitor Mail
        //            foreach (var _demand in currentTrip.VehicleSharing.VehicleSharingDemands)
        //            {
        //                if (!string.IsNullOrEmpty(_demand.AppUser.Email))
        //                {
        //                    var Mail_Subject = "Assigner: Internal empty trip completed";
        //                    var Mail_Body = "Dear Concern<br>";
        //                    Mail_Body = Mail_Body + "Trip Detail" + " <br>"
        //                        + "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + "<br>"
        //                        + "From: " + currentTrip.VehicleSharing.Depo.Name + "<br>"
        //                        + "To: " + currentTrip.VehicleSharing.Depo1.Name + "<br>"
        //                        + "On: " + currentTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
        //                    SendMail_Single(_demand.AppUser.Email, Mail_Subject, Mail_Body);
        //                }
        //            }
        //            bll.db.SaveChanges();
        //        }
        //    }
        //    var pendingTrip = bll.db.VehicleSharingInternalTrips.Where(m => m.PK_VehicleSharingInternalTrip == vehicle.FK_VehicleSharingInternalTrip_Pending).FirstOrDefault();
        //    if (pendingTrip != null && pendingTrip.StatusText == InternalTripStatus.Assigned && pendingTrip.VehicleSharing.Depo.PK_Depo == depo.PK_Depo)
        //    {
        //        if (currentTrip == null || (currentTrip != null && currentTrip.VehicleSharing.LoadedOrEmpty == false))
        //        {
        //            if (pendingTrip.VehicleSharing.LoadedOrEmpty == true)
        //            {
        //                pendingTrip.StatusText = InternalTripStatus.StartedLoading;
        //                pendingTrip.LoadingStartDateTime = DateTime.Now;

        //                // Update Vehicle Trip
        //                pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Current = pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Pending;
        //                pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Pending = null;

        //                // Update Driver Trip
        //                pendingTrip.AppUser.FK_VehicleSharingInternalTrip_Current = pendingTrip.AppUser.FK_VehicleSharingInternalTrip_Pending;
        //                pendingTrip.AppUser.FK_VehicleSharingInternalTrip_Pending = null;

        //                //-Notify Driver Firebase
        //                if (!string.IsNullOrEmpty(vehicle.FID))
        //                {
        //                    var _FK_Depo_From = pendingTrip.VehicleSharing.VehicleSharingDemands.Where(n => n.IsHeadDemand == true).FirstOrDefault().FK_Depo_From;
        //                    //var title = "Driver: Your Vehicle Entered in Pending Trip #" + pendingTrip.VehicleSharing.TrackingID + " Strating Location: " + bll.db.Depoes.Where(m => m.PK_Depo == _FK_Depo_From).FirstOrDefault().Name;
        //                    var title = "সম্মনিত চালক, গাড়ি আসন্ন যাত্রার #" + pendingTrip.VehicleSharing.TrackingID + " শুরুর স্থানে (" + bll.db.Depoes.Where(m => m.PK_Depo == _FK_Depo_From).FirstOrDefault().Name + ") প্রবেশ করেছে, লোডিং-এর সময় গণনা শুরু হয়েছে।";
        //                    var message = "Dear Concern \n";
        //                    message = message + "Trip Detail" + "\n"
        //                        + "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + "\n"
        //                        + "From: " + pendingTrip.VehicleSharing.Depo.Name + "\n"
        //                        + "To: " + pendingTrip.VehicleSharing.Depo1.Name + "\n"
        //                        + "On: " + pendingTrip.VehicleSharing.PossibleJourneyStartDateTime + "\n";
        //                    SendFCM_Notification_Single_New(vehicle.FID, title, message, pendingTrip.PK_VehicleSharingInternalTrip.ToString(), "VehicleSharingInternalTrip");
        //                }
        //                //-Notify Assigner Mail
        //                if (!string.IsNullOrEmpty(pendingTrip.AppUser1.Email))
        //                {
        //                    var Mail_Subject = "Assigner: Internal trip's loading is started";
        //                    var Mail_Body = "Dear Concern<br>";
        //                    Mail_Body = Mail_Body + "Trip Detail" + " <br>"
        //                        + "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + "<br>"
        //                        + "From: " + pendingTrip.VehicleSharing.Depo.Name + "<br>"
        //                        + "To: " + pendingTrip.VehicleSharing.Depo1.Name + "<br>"
        //                        + "On: " + pendingTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
        //                    SendMail_Single(pendingTrip.AppUser.Email, Mail_Subject, Mail_Body);
        //                }
        //                //-Notify Requisitor Mail
        //                foreach (var _demand in pendingTrip.VehicleSharing.VehicleSharingDemands)
        //                {
        //                    if (!string.IsNullOrEmpty(_demand.AppUser.Email))
        //                    {
        //                        var Mail_Subject = "Requisitior: Internal trip's loading is started";
        //                        var Mail_Body = "Dear Concern<br>";
        //                        Mail_Body = Mail_Body + "Trip Detail" + " <br>"
        //                            + "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + "<br>"
        //                            + "From: " + pendingTrip.VehicleSharing.Depo.Name + "<br>"
        //                            + "To: " + pendingTrip.VehicleSharing.Depo1.Name + "<br>"
        //                            + "On: " + pendingTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
        //                        SendMail_Single(_demand.AppUser.Email, Mail_Subject, Mail_Body);
        //                    }
        //                }
        //                bll.db.SaveChanges();
        //            }
        //            else
        //            {
        //                pendingTrip.StatusText = InternalTripStatus.StartedEmptyTrip;
        //                pendingTrip.LoadingStartDateTime = DateTime.Now;
        //                pendingTrip.LoadingDoneDateTime = pendingTrip.LoadingStartDateTime;

        //                // Update Vehicle Trip
        //                pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Current = pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Pending;
        //                pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Pending = null;

        //                // Update Driver Trip
        //                pendingTrip.AppUser.FK_VehicleSharingInternalTrip_Current = pendingTrip.AppUser.FK_VehicleSharingInternalTrip_Pending;
        //                pendingTrip.AppUser.FK_VehicleSharingInternalTrip_Pending = null;

        //                //-Notify Driver Firebase
        //                if (!string.IsNullOrEmpty(vehicle.FID))
        //                {
        //                    var _FK_Depo_From = pendingTrip.VehicleSharing.VehicleSharingDemands.Where(n => n.IsHeadDemand == true).FirstOrDefault().FK_Depo_From;
        //                    //var title = "Driver: Your Vehicle Entered in Pending Trip #" + pendingTrip.VehicleSharing.TrackingID + " Strating Location: " + bll.db.Depoes.Where(m => m.PK_Depo == _FK_Depo_From).FirstOrDefault().Name;
        //                    var title = "সম্মনিত চালক, গাড়ি আসন্ন খালি যাত্রার #" + pendingTrip.VehicleSharing.TrackingID + " শুরুর স্থানে (" + bll.db.Depoes.Where(m => m.PK_Depo == _FK_Depo_From).FirstOrDefault().Name + ") প্রবেশ করেছে, যাত্রার সময় গণনা শুরু হয়েছে।";
        //                    var message = "Dear Concern \n";
        //                    message = message + "Trip Detail" + "\n"
        //                        + "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + "\n"
        //                        + "From: " + pendingTrip.VehicleSharing.Depo.Name + "\n"
        //                        + "To: " + pendingTrip.VehicleSharing.Depo1.Name + "\n"
        //                        + "On: " + pendingTrip.VehicleSharing.PossibleJourneyStartDateTime + "\n";
        //                    SendFCM_Notification_Single_New(vehicle.FID, title, message, pendingTrip.PK_VehicleSharingInternalTrip.ToString(), "VehicleSharingInternalTrip");
        //                }
        //                //-Notify Assigner Mail
        //                if (!string.IsNullOrEmpty(pendingTrip.AppUser1.Email))
        //                {
        //                    var Mail_Subject = "Assigner: Internal empty trip started";
        //                    var Mail_Body = "Dear Concern<br>";
        //                    Mail_Body = Mail_Body + "Trip Detail" + " <br>"
        //                        + "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + "<br>"
        //                        + "From: " + pendingTrip.VehicleSharing.Depo.Name + "<br>"
        //                        + "To: " + pendingTrip.VehicleSharing.Depo1.Name + "<br>"
        //                        + "On: " + pendingTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
        //                    SendMail_Single(pendingTrip.AppUser.Email, Mail_Subject, Mail_Body);
        //                }
        //                //-Notify Requisitor Mail
        //                foreach (var _demand in pendingTrip.VehicleSharing.VehicleSharingDemands)
        //                {
        //                    if (!string.IsNullOrEmpty(_demand.AppUser.Email))
        //                    {
        //                        var Mail_Subject = "Requisitior: Internal empty trip started";
        //                        var Mail_Body = "Dear Concern<br>";
        //                        Mail_Body = Mail_Body + "Trip Detail" + " <br>"
        //                            + "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + "<br>"
        //                            + "From: " + pendingTrip.VehicleSharing.Depo.Name + "<br>"
        //                            + "To: " + pendingTrip.VehicleSharing.Depo1.Name + "<br>"
        //                            + "On: " + pendingTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
        //                        SendMail_Single(_demand.AppUser.Email, Mail_Subject, Mail_Body);
        //                    }
        //                }
        //                bll.db.SaveChanges();
        //            }
        //        }
        //        else
        //        {
        //            pendingTrip.StatusText = InternalTripStatus.EnteredStartingLocation;
        //            bll.db.SaveChanges();
        //        }
        //    }
        //}
        //public void Gate_OWN_Out(Vehicle vehicle, Depo depo)
        //{
        //    var currentTrip = bll.db.VehicleSharingInternalTrips.Where(m => m.PK_VehicleSharingInternalTrip == vehicle.FK_VehicleSharingInternalTrip_Current).FirstOrDefault();
        //    if (currentTrip != null && currentTrip.StatusText == InternalTripStatus.PaidBill && currentTrip.VehicleSharing.Depo.PK_Depo == depo.PK_Depo)
        //    {
        //        currentTrip.StatusText = InternalTripStatus.LeftStartingLoaction;
        //        currentTrip.LeftStartingLoactionDateTime = DateTime.Now;

        //        var from_to = currentTrip.VehicleSharing.VehicleSharingDemands.Where(n => n.IsHeadDemand == true).Select(m => new { m.FK_Depo_From, m.FK_Depo_To }).FirstOrDefault();
        //        var approxTimeHour = bll.db.RouteCharts.Where(m => (m.FK_Depo1 == from_to.FK_Depo_From && m.FK_Depo2 == from_to.FK_Depo_To) || (m.FK_Depo1 == from_to.FK_Depo_To && m.FK_Depo2 == from_to.FK_Depo_From)).FirstOrDefault().ApproxTimeHour;
        //        currentTrip.PossibleJourneyFinishDateTime = DateTime.Now.AddHours(approxTimeHour);

        //        //-Notify Driver Firebase
        //        //var vehicle = bll.db.Vehicles.Where(m => m.PK_Vehicle == currentTrip.FK_Vehicle).FirstOrDefault();
        //        if (!string.IsNullOrEmpty(vehicle.FID))
        //        {
        //            var _FK_Depo_From = currentTrip.VehicleSharing.VehicleSharingDemands.Where(n => n.IsHeadDemand == true).FirstOrDefault().FK_Depo_From;
        //            //var title = "Driver: Your Vehicle Left Current Trip #" + currentTrip.VehicleSharing.TrackingID + " Staring Location: " + bll.db.Depoes.Where(m => m.PK_Depo == _FK_Depo_From).FirstOrDefault().Name;
        //            var title = "সম্মনিত চালক, গাড়ি চলমান যাত্রার (#" + currentTrip.VehicleSharing.TrackingID + ") শুরুর স্থান (" + bll.db.Depoes.Where(m => m.PK_Depo == _FK_Depo_From).FirstOrDefault().Name + ") ত্যাগ করেছে।";
        //            var message = "Dear Concern \n";
        //            message = message + "Trip Detail" + "\n"
        //                + "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + "\n"
        //                + "From: " + currentTrip.VehicleSharing.Depo.Name + "\n"
        //                + "To: " + currentTrip.VehicleSharing.Depo1.Name + "\n"
        //                + "On: " + currentTrip.VehicleSharing.PossibleJourneyStartDateTime + "\n";
        //            SendFCM_Notification_Single_New(vehicle.FID, title, message, currentTrip.PK_VehicleSharingInternalTrip.ToString(), "VehicleSharingInternalTrip");
        //        }
        //        //-Notify Assigner Mail
        //        if (!string.IsNullOrEmpty(currentTrip.AppUser1.Email))
        //        {
        //            var Mail_Subject = "Assigner: Internal trip's vehicle left starting location";
        //            var Mail_Body = "Dear Concern<br>";
        //            Mail_Body = Mail_Body + "Trip Detail" + " <br>"
        //                + "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + "<br>"
        //                + "From: " + currentTrip.VehicleSharing.Depo.Name + "<br>"
        //                + "To: " + currentTrip.VehicleSharing.Depo1.Name + "<br>"
        //                + "On: " + currentTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
        //            SendMail_Single(currentTrip.AppUser.Email, Mail_Subject, Mail_Body);
        //        }
        //        //-Notify Requisitor Mail
        //        foreach (var _demand in currentTrip.VehicleSharing.VehicleSharingDemands)
        //        {
        //            if (!string.IsNullOrEmpty(_demand.AppUser.Email))
        //            {
        //                var Mail_Subject = "Requisitior: Internal trip's vehicle left starting location";
        //                var Mail_Body = "Dear Concern<br>";
        //                Mail_Body = Mail_Body + "Trip Detail" + " <br>"
        //                    + "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + "<br>"
        //                    + "From: " + currentTrip.VehicleSharing.Depo.Name + "<br>"
        //                    + "To: " + currentTrip.VehicleSharing.Depo1.Name + "<br>"
        //                    + "On: " + currentTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
        //                SendMail_Single(_demand.AppUser.Email, Mail_Subject, Mail_Body);
        //            }
        //        }
        //        bll.db.SaveChanges();
        //    }
        //}
        //public void Gate_Hired_In(Vehicle vehicle, Depo depo)
        //{
        //    var currentTrip = bll.db.VehicleSharingExternalTrips.Where(m => m.PK_VehicleSharingExternalTrip == vehicle.FK_VehicleSharingExternalTrip_Current).FirstOrDefault();
        //    //# Entered with a Current Trip 
        //    if (currentTrip != null && /*currentTrip.StatusText == InternalTripStatus.LeftStartingLoaction &&*/ currentTrip.VehicleSharingBidding.VehicleSharing.FK_Depo_From == depo.PK_Depo)
        //    {
        //        currentTrip.StatusText = ExternalTripStatus.EnteredStartingLocation;
        //        currentTrip.EnteredStartingLocationDateTime = DateTime.Now;

        //        //-Notify Assigner Mail
        //        if (!string.IsNullOrEmpty(currentTrip.AppUser.Email))
        //        {
        //            var Mail_Subject = "Assigner: External trip's entered in starting location";
        //            var Mail_Body = "Dear Concern<br>";
        //            Mail_Body = Mail_Body + "Trip Detail" + " <br>"
        //                + "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + "<br>"
        //                + "From: " + currentTrip.VehicleSharingBidding.VehicleSharing.Depo.Name + "<br>"
        //                + "To: " + currentTrip.VehicleSharingBidding.VehicleSharing.Depo1.Name + "<br>"
        //                + "On: " + currentTrip.VehicleSharingBidding.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
        //            SendMail_Single(currentTrip.AppUser.Email, Mail_Subject, Mail_Body);
        //        }
        //        //-Notify Requisitor Mail
        //        foreach (var _demand in currentTrip.VehicleSharingBidding.VehicleSharing.VehicleSharingDemands)
        //        {
        //            if (!string.IsNullOrEmpty(_demand.AppUser.Email))
        //            {
        //                var Mail_Subject = "Requisitior: External trip's entered in starting location";
        //                var Mail_Body = "Dear Concern<br>";
        //                Mail_Body = Mail_Body + "Trip Detail" + " <br>"
        //                    + "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + "<br>"
        //                    + "From: " + currentTrip.VehicleSharingBidding.VehicleSharing.Depo.Name + "<br>"
        //                    + "To: " + currentTrip.VehicleSharingBidding.VehicleSharing.Depo1.Name + "<br>"
        //                    + "On: " + currentTrip.VehicleSharingBidding.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
        //                SendMail_Single(_demand.AppUser.Email, Mail_Subject, Mail_Body);
        //            }
        //        }
        //        bll.db.SaveChanges();
        //    }
        //    else if (currentTrip != null && /*currentTrip.StatusText == InternalTripStatus.LeftStartingLoaction &&*/ currentTrip.VehicleSharingBidding.VehicleSharing.FK_Depo_To == depo.PK_Depo)
        //    {
        //        currentTrip.StatusText = ExternalTripStatus.EnteredFinishingLocation;
        //        currentTrip.EnteredFinishingLocationDateTime = DateTime.Now;

        //        //-Notify Assigner Mail
        //        if (!string.IsNullOrEmpty(currentTrip.AppUser.Email))
        //        {
        //            var Mail_Subject = "Assigner: External trip's entered in finishing location";
        //            var Mail_Body = "Dear Concern<br>";
        //            Mail_Body = Mail_Body + "Trip Detail" + " <br>"
        //                + "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + "<br>"
        //                + "From: " + currentTrip.VehicleSharingBidding.VehicleSharing.Depo.Name + "<br>"
        //                + "To: " + currentTrip.VehicleSharingBidding.VehicleSharing.Depo1.Name + "<br>"
        //                + "On: " + currentTrip.VehicleSharingBidding.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
        //            SendMail_Single(currentTrip.AppUser.Email, Mail_Subject, Mail_Body);
        //        }
        //        //-Notify Requisitor Mail
        //        foreach (var _demand in currentTrip.VehicleSharingBidding.VehicleSharing.VehicleSharingDemands)
        //        {
        //            if (!string.IsNullOrEmpty(_demand.AppUser.Email))
        //            {
        //                var Mail_Subject = "Requisitior: External trip's entered in finishing location";
        //                var Mail_Body = "Dear Concern<br>";
        //                Mail_Body = Mail_Body + "Trip Detail" + " <br>"
        //                    + "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + "<br>"
        //                    + "From: " + currentTrip.VehicleSharingBidding.VehicleSharing.Depo.Name + "<br>"
        //                    + "To: " + currentTrip.VehicleSharingBidding.VehicleSharing.Depo1.Name + "<br>"
        //                    + "On: " + currentTrip.VehicleSharingBidding.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
        //                SendMail_Single(_demand.AppUser.Email, Mail_Subject, Mail_Body);
        //            }
        //        }
        //        bll.db.SaveChanges();
        //    }

        //}
        //public void Gate_Hired_Out(Vehicle vehicle, Depo depo)
        //{
        //    var currentTrip = bll.db.VehicleSharingExternalTrips.Where(m => m.PK_VehicleSharingExternalTrip == vehicle.FK_VehicleSharingExternalTrip_Current).FirstOrDefault();
        //    //# Entered with a Current Trip 
        //    if (currentTrip != null && /*currentTrip.StatusText == InternalTripStatus.LeftStartingLoaction &&*/ currentTrip.VehicleSharingBidding.VehicleSharing.FK_Depo_From == depo.PK_Depo)
        //    {
        //        currentTrip.StatusText = ExternalTripStatus.LeftStartingLoaction;
        //        currentTrip.LeftStartingLoactionDateTime = DateTime.Now;

        //        //-Notify Assigner Mail
        //        if (!string.IsNullOrEmpty(currentTrip.AppUser.Email))
        //        {
        //            var Mail_Subject = "Assigner: External trip's entered in starting location";
        //            var Mail_Body = "Dear Concern<br>";
        //            Mail_Body = Mail_Body + "Trip Detail" + " <br>"
        //                + "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + "<br>"
        //                + "From: " + currentTrip.VehicleSharingBidding.VehicleSharing.Depo.Name + "<br>"
        //                + "To: " + currentTrip.VehicleSharingBidding.VehicleSharing.Depo1.Name + "<br>"
        //                + "On: " + currentTrip.VehicleSharingBidding.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
        //            SendMail_Single(currentTrip.AppUser.Email, Mail_Subject, Mail_Body);
        //        }
        //        //-Notify Requisitor Mail
        //        foreach (var _demand in currentTrip.VehicleSharingBidding.VehicleSharing.VehicleSharingDemands)
        //        {
        //            if (!string.IsNullOrEmpty(_demand.AppUser.Email))
        //            {
        //                var Mail_Subject = "Requisitior: External trip's entered in starting location";
        //                var Mail_Body = "Dear Concern<br>";
        //                Mail_Body = Mail_Body + "Trip Detail" + " <br>"
        //                    + "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + "<br>"
        //                    + "From: " + currentTrip.VehicleSharingBidding.VehicleSharing.Depo.Name + "<br>"
        //                    + "To: " + currentTrip.VehicleSharingBidding.VehicleSharing.Depo1.Name + "<br>"
        //                    + "On: " + currentTrip.VehicleSharingBidding.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
        //                SendMail_Single(_demand.AppUser.Email, Mail_Subject, Mail_Body);
        //            }
        //        }
        //        bll.db.SaveChanges();
        //    }
        //    else if (currentTrip != null && /*currentTrip.StatusText == InternalTripStatus.LeftStartingLoaction &&*/ currentTrip.VehicleSharingBidding.VehicleSharing.FK_Depo_To == depo.PK_Depo)
        //    {
        //        vehicle.FK_VehicleSharingExternalTrip_Current = null;
        //        currentTrip.StatusText = ExternalTripStatus.LeftFinishingLocation;
        //        currentTrip.LeftFinishingLocationDateTime = DateTime.Now;

        //        //-Notify Assigner Mail
        //        if (!string.IsNullOrEmpty(currentTrip.AppUser.Email))
        //        {
        //            var Mail_Subject = "Assigner: External trip's entered in starting location";
        //            var Mail_Body = "Dear Concern<br>";
        //            Mail_Body = Mail_Body + "Trip Detail" + " <br>"
        //                + "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + "<br>"
        //                + "From: " + currentTrip.VehicleSharingBidding.VehicleSharing.Depo.Name + "<br>"
        //                + "To: " + currentTrip.VehicleSharingBidding.VehicleSharing.Depo1.Name + "<br>"
        //                + "On: " + currentTrip.VehicleSharingBidding.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
        //            SendMail_Single(currentTrip.AppUser.Email, Mail_Subject, Mail_Body);
        //        }
        //        //-Notify Requisitor Mail
        //        foreach (var _demand in currentTrip.VehicleSharingBidding.VehicleSharing.VehicleSharingDemands)
        //        {
        //            if (!string.IsNullOrEmpty(_demand.AppUser.Email))
        //            {
        //                var Mail_Subject = "Requisitior: External trip's entered in starting location";
        //                var Mail_Body = "Dear Concern<br>";
        //                Mail_Body = Mail_Body + "Trip Detail" + " <br>"
        //                    + "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + "<br>"
        //                    + "From: " + currentTrip.VehicleSharingBidding.VehicleSharing.Depo.Name + "<br>"
        //                    + "To: " + currentTrip.VehicleSharingBidding.VehicleSharing.Depo1.Name + "<br>"
        //                    + "On: " + currentTrip.VehicleSharingBidding.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
        //                SendMail_Single(_demand.AppUser.Email, Mail_Subject, Mail_Body);
        //            }
        //        }
        //        bll.db.SaveChanges();
        //    }
        //}
        //public ActionResult VehicleEnterDepoReport(Guid? FK_Depo, DateTime? StartingDate, DateTime? EndingDate)
        //{
        //    if (CommonClass.IsInvalidAccess())
        //    {
        //        return Redirect("/Access/Login");
        //    }
        //    if (FK_Depo != null && StartingDate != null && EndingDate != null)
        //    {
        //        var _StartingDate = StartingDate != null ? StartingDate : new DateTime();
        //        var _EndingDate = EndingDate != null ? EndingDate : new DateTime();
        //        ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", _StartingDate);
        //        ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", _EndingDate);
        //        var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
        //        ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && accessibleDepoes.Contains(m.PK_Depo)).OrderBy(m => m.Name), "PK_Depo", "Name", FK_Depo);
        //        var list = bll.db.VehicleInOutManuals.AsEnumerable().Where(m => m.Vehicle.FK_Depo == FK_Depo && m.CreatedAt >= _StartingDate && m.CreatedAt <= _EndingDate).OrderBy(m => m.Vehicle.RegistrationNumber).OrderBy(m => m.CreatedAt).ToList();
        //        return View(list);
        //    }
        //    else
        //    {
        //        ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", DateTime.Today.Date);
        //        ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", DateTime.Today.AddDays(1).Date);
        //        var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
        //        ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && accessibleDepoes.Contains(m.PK_Depo)).OrderBy(m => m.Name), "PK_Depo", "Name");
        //        var list = new List<VehicleInOutManual>();
        //        return View(list);
        //    }
        //}


        //# Ajax Methods
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
                m.FK_VehicleSharingInternalTrip_Pending
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
        public JsonResult GetInternalVehicles_Inside_Toward(Guid FK_Depo_From)
        {
            //var headDemand = bll.db.VehicleSharingDemands.Where(m => m.PK_VehicleSharingDemand == PK_VehicleSharingDemand).FirstOrDefault();
            //var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            var list_inside = bll.db.Vehicles.Where(m => m.OWN_MHT_DHT == "OWN" /*&& accessibleDepoes.Contains(m.FK_Depo)*/
            && m.LocationInOrOut == true /*&& m.FK_LocationInOut == FK_Depo_From */&& m.FK_VehicleSharingInternalTrip_Pending == null && m.FK_VehicleSharingInternalTrip_Current == null
            ).Select(m =>
             new
             {
                 m.PK_Vehicle,
                 m.RegistrationNumber,
                 ContactNumber = (m.Internal_VehicleContactNumber != null ? m.Internal_VehicleContactNumber : "") + (m.MHT_DHT_DriverContactNumber != null ? m.MHT_DHT_DriverContactNumber : "")
             }
            ).Take(10).ToList();

            var list_toward = bll.db.Vehicles.Where(m => m.OWN_MHT_DHT == "OWN" /*&& accessibleDepoes.Contains(m.FK_Depo)*/
            && FK_Depo_From == m.VehicleSharingInternalTrip1.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().FK_Depo_To && m.FK_VehicleSharingInternalTrip_Pending == null).Select(m =>
                   new
                   {
                       m.PK_Vehicle,
                       m.RegistrationNumber,
                       ContactNumber = (m.Internal_VehicleContactNumber != null ? m.Internal_VehicleContactNumber : "") + (m.MHT_DHT_DriverContactNumber != null ? m.MHT_DHT_DriverContactNumber : ""),
                       m.VehicleSharingInternalTrip1.PossibleJourneyFinishDateTime,
                   }
            ).ToList();
            return Json(new { list_inside, list_toward }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetInternalVehicles_Outside(string InternalVehicleRegNum)
        {
            //var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            var list = bll.db.Vehicles.Where(m => (m.OWN_MHT_DHT == "OWN" && m.FK_VehicleSharingInternalTrip_Pending == null /*&& accessibleDepoes.Contains(m.FK_Depo)*/) && m.RegistrationNumber.Contains(InternalVehicleRegNum)).Select(m =>
             new
             {
                 m.PK_Vehicle,
                 m.RegistrationNumber,
                 ContactNumber = (m.Internal_VehicleContactNumber != null ? m.Internal_VehicleContactNumber : "") + (m.MHT_DHT_DriverContactNumber != null ? m.MHT_DHT_DriverContactNumber : "")
             }
            ).Take(10).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetExternalVehicles(string ExternalVehicleRegNum)
        {
            var list = bll.db.Vehicles.Where(m => ((m.OWN_MHT_DHT == "DHT" || m.OWN_MHT_DHT == "MHT") && m.IsDeleted != true) && m.RegistrationNumber.Contains(ExternalVehicleRegNum)).Select(m =>
             new
             {
                 m.PK_Vehicle,
                 m.RegistrationNumber,
                 ContactNumber = (m.Internal_VehicleContactNumber != null ? m.Internal_VehicleContactNumber : "") + (m.MHT_DHT_DriverContactNumber != null ? m.MHT_DHT_DriverContactNumber : "")
             }
            ).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ApprovedDemandTrackingID(string ApprovedDemandTrackingID)
        {
            var demand = bll.db.VehicleSharingDemands.Where(m => m.TrackingID == ApprovedDemandTrackingID && m.Status == 1)
                .Select(item => new
                {
                    PK_VehicleSharingDemand = item.PK_VehicleSharingDemand,
                    TrackingID = item.TrackingID,
                    From = item.Depo.Name,
                    To = item.Depo1.Name,
                    VehicleType = item.RequisitionVehicleType.Title_English,
                    WantedCount = item.WantedCount,
                    PossibleJourneyStartDateTime = item.PossibleJourneyStartDateTime,
                }).FirstOrDefault();
            return Json(new { demand }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBiddedBid(Int64 FK_VehicleSharing)
        {
            var demand = bll.db.VehicleSharingBiddings.Where(m => m.FK_VehicleSharing == FK_VehicleSharing && m.StatusText == VehicleSharingBiddingStatus.Bidded).OrderBy(m => m.PricePerQuantity).ThenBy(m => m.BiddedAt)
                .Select(item => new
                {
                    item.PK_VehicleSharingBidding,
                    bidder = item.AppUser.FullName + " " + item.AppUser.ContactNumber,
                    item.ManagableQuantity,
                    item.PricePerQuantity,
                    item.StatusText
                }).ToList();
            return Json(demand, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDepoLocations(Guid FK_Depo_From, Guid FK_Depo_To)
        {
            var Depo_From = bll.db.Depoes.Where(m => m.PK_Depo == FK_Depo_From).Select(m => new
            {
                m.PK_Depo,
                m.Name,
                m.Latitude,
                m.Longitude
            }).FirstOrDefault();
            var Depo_To = bll.db.Depoes.Where(m => m.PK_Depo == FK_Depo_To).Select(m => new
            {
                m.PK_Depo,
                m.Name,
                m.Latitude,
                m.Longitude
            }).FirstOrDefault();
            return Json(new { Depo_From, Depo_To }, JsonRequestBehavior.AllowGet);
        }

    }
}