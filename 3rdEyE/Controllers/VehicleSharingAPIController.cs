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
    public class VehicleSharingAPIController : BaseAPIController
    {
        static class InternalTripStatus
        {
            public const string Assigned = "Assigned";
            public const string EnteredStartingLocation = "Entered Starting Location";
            public const string StartedLoading = "Started Loading";
            public const string FinishedLoading = "Finished Loading";
            public const string StartedEmptyTrip = "Started Empty Trip";
            public const string CreatedBill = "Created Bill";
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

        //# VehicleSharingBidding
        public JsonResult GetVehicleSharingBiddingList(Guid FK_RequisitionAgent_Bidder)
        {
            var minDate = DateTime.Now.AddDays(-7).Date;
            var list = db.VehicleSharingBiddings.Where(m => m.FK_RequisitionAgent_Bidder == FK_RequisitionAgent_Bidder /*&& m.CreatedAt > minDate*/).OrderByDescending(m => m.CreatedAt)
                .Select(
                m => new
                {
                    m.PK_VehicleSharingBidding,
                    RequisitionVehicleType_Title = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().RequisitionVehicleType.Title_English,
                    From = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo.Name,
                    To = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo1.Name,
                    PossibleJourneyStartDateTime = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().PossibleJourneyStartDateTime.ToString(),
                    m.VehicleSharing.KeepBidOpenUntil,
                    m.StatusText,
                    m.VehicleSharing.ExternalWantedCount
                }
                );
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVehicleSharingBiddingList_Pending(Guid FK_RequisitionAgent_Bidder)
        {
            try
            {
                var list = db.VehicleSharingBiddings.Where(m => m.FK_RequisitionAgent_Bidder == FK_RequisitionAgent_Bidder && m.StatusText == VehicleSharingBiddingStatus.Created)
                .Select(
                m => new
                {
                    m.PK_VehicleSharingBidding,
                    RequisitionVehicleType_Title = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().RequisitionVehicleType.Title_English,
                    From = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo.Name,
                    To = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo1.Name,
                    PossibleJourneyStartDateTime = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().PossibleJourneyStartDateTime.ToString(),
                    m.StatusText
                }
                );
                return Json(new { flag = "YES", data = list }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var errrorMessage = "";
                do { errrorMessage = "#" + errrorMessage + e.Message; if (e.InnerException != null) { e = e.InnerException; } else { break; } } while (true);
                return Json(new { flag = "ERROR", data = FK_RequisitionAgent_Bidder, message = errrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetVehicleSharingBiddingList_Bidded(Guid FK_RequisitionAgent_Bidder)
        {
            try
            {
                var list = db.VehicleSharingBiddings.Where(m => m.FK_RequisitionAgent_Bidder == FK_RequisitionAgent_Bidder && (m.StatusText == VehicleSharingBiddingStatus.Bidded || m.StatusText == VehicleSharingBiddingStatus.CancelledByClient || m.StatusText == VehicleSharingBiddingStatus.Approved || m.StatusText == VehicleSharingBiddingStatus.CancelledByApprover))
                .Select(
                m => new
                {
                    m.PK_VehicleSharingBidding,
                    RequisitionVehicleType_Title = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().RequisitionVehicleType.Title_English,
                    From = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo.Name,
                    To = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo1.Name,
                    PossibleJourneyStartDateTime = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().PossibleJourneyStartDateTime.ToString(),
                    m.StatusText
                }
                );
                return Json(new { flag = "YES", data = list }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var errrorMessage = "";
                do { errrorMessage = "#" + errrorMessage + e.Message; if (e.InnerException != null) { e = e.InnerException; } else { break; } } while (true);
                return Json(new { flag = "ERROR", data = FK_RequisitionAgent_Bidder, message = errrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetVehicleSharingBiddingDetail(Int64 PK_VehicleSharingBidding)
        {
            try
            {
                var data = db.VehicleSharingBiddings.Where(m => m.PK_VehicleSharingBidding == PK_VehicleSharingBidding)
                .Select(
                    m => new
                    {
                        m.PK_VehicleSharingBidding,
                        RequisitionVehicleType_Title = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().RequisitionVehicleType.Title_English,
                        From = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo.Name,
                        FromLatitude = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo.Latitude,
                        FromLongitude = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo.Longitude,
                        To = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo1.Name,
                        ToLatitude = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo1.Latitude,
                        ToLongitude = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo1.Longitude,
                        PossibleJourneyStartDateTime = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().PossibleJourneyStartDateTime.ToString(),
                        m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().DistanceGoogle,
                        m.VehicleSharing.ExternalWantedCount,
                        m.StatusText,
                        m.ManagableQuantity,
                        m.PricePerQuantity,
                        m.ApprovedQuantity,
                        AssignerFullName = m.VehicleSharing.AppUser.FullName,
                        AssignerContactNumber = m.VehicleSharing.AppUser.ContactNumber,
                    }
                ).FirstOrDefault();
                return Json(new { flag = "YES", data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var errrorMessage = "";
                do { errrorMessage = "#" + errrorMessage + e.Message; if (e.InnerException != null) { e = e.InnerException; } else { break; } } while (true);
                return Json(new { flag = "ERROR", data = PK_VehicleSharingBidding, message = errrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult BidVehicleSharingBidding(VehicleSharingBidding model)
        {
            try
            {
                var now = DateTime.Now;
                var bidding = db.VehicleSharingBiddings.Where(m => m.PK_VehicleSharingBidding == model.PK_VehicleSharingBidding).FirstOrDefault();
                var sharing = db.VehicleSharings.Where(m => m.PK_VehicleSharing == bidding.FK_VehicleSharing).FirstOrDefault();
                if (bidding != null && bidding.StatusText == VehicleSharingBiddingStatus.Created && sharing.LockedAt == null && sharing.KeepBidOpenUntil > now)
                {
                    bidding.StatusText = VehicleSharingBiddingStatus.Bidded;
                    bidding.PricePerQuantity = model.PricePerQuantity;
                    bidding.ManagableQuantity = model.ManagableQuantity;
                    bidding.BiddedAt = DateTime.Now;
                    db.SaveChanges();
                    //-Notify Assigner Mail
                    if (!string.IsNullOrEmpty(sharing.AppUser.Email))
                    {
                        var Mail_Subject = "Assigner: new bid for #" + bidding.VehicleSharing.TrackingID;
                        var Mail_Body = "Dear Concern<br>";
                        Mail_Body = Mail_Body + "Bidding Details" + " <br>"
                            + "Tracking ID: " + bidding.VehicleSharing.TrackingID + "<br>"
                            + "From: " + bidding.VehicleSharing.Depo.Name + "<br>"
                            + "To: " + bidding.VehicleSharing.Depo1.Name + "<br>"
                            + "On: " + bidding.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
                        SendMail_Single(bidding.VehicleSharing.AppUser.Email, Mail_Subject, Mail_Body);
                    }
                    db.SaveChanges();
                    return Json(new { flag = "YES", message = "সফল ভাবে বিড করা হয়েছে।" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { flag = "NO", message = "দুঃখিত, বিড করার সুযোগ অবর্তমান।" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { flag = "NO", message = "", error_message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CancelVehicleSharingBidding(Int64 PK_VehicleSharingBidding)
        {
            try
            {
                var db_model = db.VehicleSharingBiddings.Where(m => m.PK_VehicleSharingBidding == PK_VehicleSharingBidding).FirstOrDefault();
                if (db_model != null && (db_model.StatusText == VehicleSharingBiddingStatus.Created || db_model.StatusText == VehicleSharingBiddingStatus.Bidded))
                {
                    db_model.StatusText = VehicleSharingBiddingStatus.CancelledByClient;
                    db_model.VerifiedAt = DateTime.Now;
                    db.SaveChanges();
                    return Json(new { flag = "YES", message = "Cancellation successful" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { flag = "NO", message = "Bidding is closed for this sharing." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { flag = "NO", message = "", error_message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //# VehicleSharingInternalTrip

        public JsonResult GetVehiclesInternalTripList_Billed(Guid PK_Vehicle)
        {
            try
            {
                var minDate = DateTime.Now.AddDays(-7).Date;
                var list = db.VehicleSharingInternalTrips.Where(m => m.FK_Vehicle == PK_Vehicle && m.AssingedAt > minDate && m.StatusText == InternalTripStatus.CreatedBill).OrderByDescending(m => m.AssingedAt)
                .Select(
                    m => new
                    {
                        m.PK_VehicleSharingInternalTrip,
                        RequisitionVehicleType_Title = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().RequisitionVehicleType.Title_English,
                        From = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo.Name,
                        FromLatitude = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo.Latitude,
                        FromLongitude = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo.Longitude,
                        To = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo1.Name,
                        ToLatitude = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo1.Latitude,
                        ToLongitude = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo1.Longitude,
                        PossibleJourneyStartDateTime = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().PossibleJourneyStartDateTime.ToString(),
                        m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().DistanceGoogle,
                        m.StatusText,
                        m.IsNotifiedToDriver,
                        AssignerFullName = m.VehicleSharing.AppUser.FullName,
                        AssignerContactNumber = m.VehicleSharing.AppUser.ContactNumber,
                        m.AssingedAt,
                        m.Vehicle.RegistrationNumber,
                    }
                ).OrderByDescending(m => m.AssingedAt).ToList();
                return Json(new
                {
                    flag = "YES",
                    data = new
                    {
                        list
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var errrorMessage = "";
                do { errrorMessage = "#" + errrorMessage + e.Message; if (e.InnerException != null) { e = e.InnerException; } else { break; } } while (true);
                return Json(new { flag = "ERROR", data = PK_Vehicle, message = errrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetVehiclesInternalTripList_Unbilled(Guid PK_Vehicle)
        {
            try
            {
                //var minDate = DateTime.Now.AddDays(-7).Date;
                var list = db.VehicleSharingInternalTrips.Where(m => m.FK_Vehicle == PK_Vehicle && m.StatusText != InternalTripStatus.CreatedBill).OrderByDescending(m => m.AssingedAt)
                .Select(
                    m => new
                    {
                        m.PK_VehicleSharingInternalTrip,
                        RequisitionVehicleType_Title = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().RequisitionVehicleType.Title_English,
                        From = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo.Name,
                        FromLatitude = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo.Latitude,
                        FromLongitude = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo.Longitude,
                        To = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo1.Name,
                        ToLatitude = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo1.Latitude,
                        ToLongitude = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo1.Longitude,
                        PossibleJourneyStartDateTime = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().PossibleJourneyStartDateTime.ToString(),
                        m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().DistanceGoogle,
                        m.StatusText,
                        m.IsNotifiedToDriver,
                        AssignerFullName = m.VehicleSharing.AppUser.FullName,
                        AssignerContactNumber = m.VehicleSharing.AppUser.ContactNumber,
                        m.AssingedAt,
                        m.Vehicle.RegistrationNumber,
                    }
                ).OrderByDescending(m => m.AssingedAt).ToList();
                return Json(new
                {
                    flag = "YES",
                    data = new
                    {
                        list
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var errrorMessage = "";
                do { errrorMessage = "#" + errrorMessage + e.Message; if (e.InnerException != null) { e = e.InnerException; } else { break; } } while (true);
                return Json(new { flag = "ERROR", data = PK_Vehicle, message = errrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetVehiclesInternalTrips(Guid PK_Vehicle)
        {
            try
            {
                var vehicle = db.Vehicles.Where(m => m.PK_Vehicle == PK_Vehicle).FirstOrDefault();

                var VehicleSharingInternalTrip_Current = db.VehicleSharingInternalTrips.Where(m => m.PK_VehicleSharingInternalTrip == vehicle.FK_VehicleSharingInternalTrip_Current)
                .Select(
                    m => new
                    {
                        m.PK_VehicleSharingInternalTrip,
                        RequisitionVehicleType_Title = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().RequisitionVehicleType.Title_English,
                        From = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo.Name,
                        FromLatitude = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo.Latitude,
                        FromLongitude = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo.Longitude,
                        To = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo1.Name,
                        ToLatitude = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo1.Latitude,
                        ToLongitude = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo1.Longitude,
                        PossibleJourneyStartDateTime = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().PossibleJourneyStartDateTime.ToString(),
                        m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().DistanceGoogle,
                        m.StatusText,
                        m.IsNotifiedToDriver,
                        AssignerFullName = m.VehicleSharing.AppUser.FullName,
                        AssignerContactNumber = m.VehicleSharing.AppUser.ContactNumber,
                        m.AssingedAt,
                        m.Vehicle.RegistrationNumber,
                    }
                ).FirstOrDefault();
                var VehicleSharingInternalTrip_Pending = db.VehicleSharingInternalTrips.Where(m => m.PK_VehicleSharingInternalTrip == vehicle.FK_VehicleSharingInternalTrip_Pending)
                .Select(
                    m => new
                    {
                        m.PK_VehicleSharingInternalTrip,
                        RequisitionVehicleType_Title = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().RequisitionVehicleType.Title_English,
                        From = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo.Name,
                        FromLatitude = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo.Latitude,
                        FromLongitude = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo.Longitude,
                        To = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo1.Name,
                        ToLatitude = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo1.Latitude,
                        ToLongitude = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo1.Longitude,
                        PossibleJourneyStartDateTime = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().PossibleJourneyStartDateTime.ToString(),
                        m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().DistanceGoogle,
                        m.StatusText,
                        m.IsNotifiedToDriver,
                        AssignerFullName = m.VehicleSharing.AppUser.FullName,
                        AssignerContactNumber = m.VehicleSharing.AppUser.ContactNumber,
                        m.AssingedAt,
                        m.Vehicle.RegistrationNumber,
                    }
                ).FirstOrDefault();
                return Json(new
                {
                    flag = "YES",
                    data = new
                    {
                        VehicleSharingInternalTrip_Current,
                        VehicleSharingInternalTrip_Pending
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var errrorMessage = "";
                do { errrorMessage = "#" + errrorMessage + e.Message; if (e.InnerException != null) { e = e.InnerException; } else { break; } } while (true);
                return Json(new { flag = "ERROR", data = PK_Vehicle, message = errrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetVehicleSharingInternalTripDetail(Int64 PK_VehicleSharingInternalTrip)
        {
            try
            {
                var data = db.VehicleSharingInternalTrips.Where(m => m.PK_VehicleSharingInternalTrip == PK_VehicleSharingInternalTrip)
                .Select(
                    m => new
                    {
                        m.PK_VehicleSharingInternalTrip,
                        RequisitionVehicleType_Title = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().RequisitionVehicleType.Title_English,
                        From = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo.Name,
                        FromLatitude = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo.Latitude,
                        FromLongitude = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo.Longitude,
                        To = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo1.Name,
                        ToLatitude = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo1.Latitude,
                        ToLongitude = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().Depo1.Longitude,
                        PossibleJourneyStartDateTime = m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().PossibleJourneyStartDateTime.ToString(),
                        m.VehicleSharing.VehicleSharingDemands.Where(d => d.IsHeadDemand == true).FirstOrDefault().DistanceGoogle,
                        m.StatusText,
                        m.IsNotifiedToDriver,
                        AssignerFullName = m.VehicleSharing.AppUser.FullName,
                        AssignerContactNumber = m.VehicleSharing.AppUser.ContactNumber,
                        m.AssingedAt,
                        m.Vehicle.RegistrationNumber,
                    }
                ).FirstOrDefault();
                return Json(new { flag = "YES", data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var errrorMessage = "";
                do { errrorMessage = "#" + errrorMessage + e.Message; if (e.InnerException != null) { e = e.InnerException; } else { break; } } while (true);
                return Json(new { flag = "ERROR", data = PK_VehicleSharingInternalTrip, message = errrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult VehicleSharingInternalTrip_Notify(Int64 PK_VehicleSharingInternalTrip)
        {
            try
            {
                var internalTrip = db.VehicleSharingInternalTrips.Where(m => m.PK_VehicleSharingInternalTrip == PK_VehicleSharingInternalTrip && m.IsNotifiedToDriver != true).FirstOrDefault();
                if (internalTrip != null)
                {
                    internalTrip.IsNotifiedToDriver = true;
                    internalTrip.NotifiedToDriverAt = DateTime.Now;
                    //-Notify Assigner Mail
                    if (!string.IsNullOrEmpty(internalTrip.AppUser.Email))
                    {
                        var Mail_Subject = "Assigner: Trip is seen by driver";
                        var Mail_Body = "Dear Concern<br>";
                        Mail_Body = Mail_Body + "Trip Detail" + " <br>"
                            + "Vehicle: " + internalTrip.Vehicle.RegistrationNumber + "<br>"
                            + "From: " + internalTrip.VehicleSharing.Depo.Name + "<br>"
                            + "To: " + internalTrip.VehicleSharing.Depo1.Name + "<br>"
                            + "On: " + internalTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
                        SendMail_Single(internalTrip.AppUser.Email, Mail_Subject, Mail_Body);
                    }
                    db.SaveChanges();
                }
                return Json(new { flag = "YES", data = PK_VehicleSharingInternalTrip }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var errrorMessage = "";
                do { errrorMessage = "#" + errrorMessage + e.Message; if (e.InnerException != null) { e = e.InnerException; } else { break; } } while (true);
                return Json(new { flag = "ERROR", data = PK_VehicleSharingInternalTrip, message = errrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult VehicleSharingInternalTrip_StartLoading(Int64 PK_VehicleSharingInternalTrip)
        {
            try
            {
                var pendingTrip = db.VehicleSharingInternalTrips.Where(m => m.PK_VehicleSharingInternalTrip == PK_VehicleSharingInternalTrip).FirstOrDefault();
                if (pendingTrip.IsNotifiedToDriver != true)
                {
                    pendingTrip.IsNotifiedToDriver = true;
                    pendingTrip.NotifiedToDriverAt = DateTime.Now;
                }
                var currentTrip = db.VehicleSharingInternalTrips.Where(m => m.PK_VehicleSharingInternalTrip == pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Current).FirstOrDefault();

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
                            db.SaveChanges();
                            return Json(new { flag = "YES", data = PK_VehicleSharingInternalTrip }, JsonRequestBehavior.AllowGet);
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
                            db.SaveChanges();
                            return Json(new { flag = "YES", data = PK_VehicleSharingInternalTrip }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else if (pendingTrip.VehicleSharing.FK_Depo_From != pendingTrip.Vehicle.FK_LocationInOut || pendingTrip.Vehicle.LocationInOrOut != true)
                    {
                        message = "গাড়ির পরবর্তী যাত্রার (#" + pendingTrip.VehicleSharing.TrackingID + ") শুরুর স্থানে (" + pendingTrip.VehicleSharing.Depo.Name + ") প্রবেশ করুন।";
                        return Json(new { flag = "No", data = PK_VehicleSharingInternalTrip, message = message }, JsonRequestBehavior.AllowGet);
                    }
                    else if (currentTrip.StatusText != InternalTripStatus.Assigned && currentTrip.StatusText != InternalTripStatus.EnteredStartingLocation)
                    {
                        message = "চলমান যাত্রার (#" + currentTrip.VehicleSharing.TrackingID + ") অবস্থা সামঞ্জস্যপুর্ন নয়, লোড শুরু করা যাচ্ছে না।";
                        return Json(new { flag = "No", data = PK_VehicleSharingInternalTrip, message = message }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        message = "Undefined Reasson : StartLoading 1.1";
                        return Json(new { flag = "No", data = PK_VehicleSharingInternalTrip, message = message }, JsonRequestBehavior.AllowGet);
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
                            db.SaveChanges();
                            return Json(new { flag = "YES", data = PK_VehicleSharingInternalTrip }, JsonRequestBehavior.AllowGet);
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
                            db.SaveChanges();
                            return Json(new { flag = "YES", data = PK_VehicleSharingInternalTrip }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else if (pendingTrip.VehicleSharing.FK_Depo_From != pendingTrip.Vehicle.FK_LocationInOut || pendingTrip.Vehicle.LocationInOrOut != true)
                    {
                        message = "গাড়ির পরবর্তী যাত্রার (#" + pendingTrip.VehicleSharing.TrackingID + ") শুরুর স্থানে (" + pendingTrip.VehicleSharing.Depo.Name + ") প্রবেশ করুন।";
                        return Json(new { flag = "No", data = PK_VehicleSharingInternalTrip, message = message }, JsonRequestBehavior.AllowGet);
                    }
                    else if (currentTrip.StatusText != InternalTripStatus.FinishedUnloading && currentTrip.StatusText != InternalTripStatus.FinishedEmptyTrip)
                    {
                        if (currentTrip.VehicleSharing.LoadedOrEmpty == true)
                        {
                            message = "চলমান যাত্রার (#" + currentTrip.VehicleSharing.TrackingID + ") মালামাল আনলোড শেষ করুন।";
                            return Json(new { flag = "No", data = PK_VehicleSharingInternalTrip, message = message }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            //# Vehicle with empty trip already enterd in it's Destination but emptyTrip.StatusText != FinishedEmptyTrip 
                            message = "Undefined Reasson : StartLoading 2.1";
                            return Json(new { flag = "No", data = PK_VehicleSharingInternalTrip, message = message }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        message = "Undefined Reasson : StartLoading 2.2";
                        return Json(new { flag = "No", data = PK_VehicleSharingInternalTrip, message = message }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception e)
            {
                var errrorMessage = "";
                do { errrorMessage = "#" + errrorMessage + e.Message; if (e.InnerException != null) { e = e.InnerException; } else { break; } } while (true);
                return Json(new { flag = "ERROR", data = PK_VehicleSharingInternalTrip, message = errrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult VehicleSharingInternalTrip_FinishLoading(Int64 PK_VehicleSharingInternalTrip)
        {
            try
            {
                var currentTrip = db.VehicleSharingInternalTrips.Where(m => m.PK_VehicleSharingInternalTrip == PK_VehicleSharingInternalTrip).FirstOrDefault();

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
                        db.SaveChanges();
                        return Json(new { flag = "YES", data = PK_VehicleSharingInternalTrip }, JsonRequestBehavior.AllowGet);
                    }
                    else if (currentTrip.VehicleSharing.FK_Depo_From != currentTrip.Vehicle.FK_LocationInOut || currentTrip.Vehicle.LocationInOrOut != true)
                    {
                        message = "চলমান যাত্রার (#" + currentTrip.VehicleSharing.TrackingID + ") শুরুর স্থানে (" + currentTrip.VehicleSharing.Depo.Name + ") প্রবেশ করুন।";
                        return Json(new { flag = "No", data = PK_VehicleSharingInternalTrip, message = message }, JsonRequestBehavior.AllowGet);
                    }
                    else if (currentTrip.StatusText != InternalTripStatus.StartedLoading)
                    {
                        message = "চলমান যাত্রার (#" + currentTrip.VehicleSharing.TrackingID + ") মালামাল লোড শুরু করুন।";
                        return Json(new { flag = "No", data = PK_VehicleSharingInternalTrip, message = message }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        message = "Undefined Reasson : FinishLoading 1.1";
                        return Json(new { flag = "No", data = PK_VehicleSharingInternalTrip, message = message }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    message = "যাত্রার তথ্য খুজে পাওয়া যায়নি।";
                    return Json(new { flag = "No", data = PK_VehicleSharingInternalTrip, message = message }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                var errrorMessage = "";
                do { errrorMessage = "#" + errrorMessage + e.Message; if (e.InnerException != null) { e = e.InnerException; } else { break; } } while (true);
                return Json(new { flag = "ERROR", data = PK_VehicleSharingInternalTrip, message = errrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult VehicleSharingInternalTrip_StartUnloading(Int64 PK_VehicleSharingInternalTrip)
        {
            try
            {
                var currentTrip = db.VehicleSharingInternalTrips.Where(m => m.PK_VehicleSharingInternalTrip == PK_VehicleSharingInternalTrip).FirstOrDefault();

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
                        db.SaveChanges();
                        return Json(new { flag = "YES", data = PK_VehicleSharingInternalTrip }, JsonRequestBehavior.AllowGet);
                    }
                    else if (currentTrip.VehicleSharing.FK_Depo_To != currentTrip.Vehicle.FK_LocationInOut || currentTrip.Vehicle.LocationInOrOut != true)
                    {
                        message = "চলমান যাত্রার (#" + currentTrip.VehicleSharing.TrackingID + ") শেষের স্থানে (" + currentTrip.VehicleSharing.Depo1.Name + ") প্রবেশ করুন।";
                        return Json(new { flag = "No", data = PK_VehicleSharingInternalTrip, message = message }, JsonRequestBehavior.AllowGet);
                    }
                    else if (currentTrip.StatusText != InternalTripStatus.EnteredFinishingLocation)
                    {
                        message = "চলমান যাত্রার (#" + currentTrip.VehicleSharing.TrackingID + ") অবস্থা সামঞ্জস্যপুর্ন নয়,আনলোড শুরু করা যাচ্ছে না।";
                        return Json(new { flag = "No", data = PK_VehicleSharingInternalTrip, message = message }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        message = "Undefined Reasson : StartUnloading 1.1";
                        return Json(new { flag = "No", data = PK_VehicleSharingInternalTrip, message = message }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    message = "যাত্রার তথ্য খুজে পাওয়া যায়নি।";
                    return Json(new { flag = "No", data = PK_VehicleSharingInternalTrip, message = message }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                var errrorMessage = "";
                do { errrorMessage = "#" + errrorMessage + e.Message; if (e.InnerException != null) { e = e.InnerException; } else { break; } } while (true);
                return Json(new { flag = "ERROR", data = PK_VehicleSharingInternalTrip, message = errrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult VehicleSharingInternalTrip_FinishUnloading(Int64 PK_VehicleSharingInternalTrip)
        {
            try
            {
                var currentTrip = db.VehicleSharingInternalTrips.Where(m => m.PK_VehicleSharingInternalTrip == PK_VehicleSharingInternalTrip).FirstOrDefault();

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
                        db.SaveChanges();

                        //#Check pending trip for loading
                        var pendingTrip = db.VehicleSharingInternalTrips.Where(m => m.PK_VehicleSharingInternalTrip == currentTrip.Vehicle.FK_VehicleSharingInternalTrip_Pending).FirstOrDefault();
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
                                    //var vehicle = db.Vehicles.Where(m => m.PK_Vehicle == currentTrip.FK_Vehicle).FirstOrDefault();
                                    if (!string.IsNullOrEmpty(pendingTrip.Vehicle.FID))
                                    {
                                        var _FK_Depo_To = currentTrip.VehicleSharing.VehicleSharingDemands.Where(n => n.IsHeadDemand == true).FirstOrDefault().FK_Depo_To;
                                        //var title = "Driver: Your Vehicle Entered in Current Trip #" + currentTrip.VehicleSharing.TrackingID + " Destination: " + db.Depoes.Where(m => m.PK_Depo == _FK_Depo_To).FirstOrDefault().Name;
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
                                    db.SaveChanges();
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
                                    //var vehicle = db.Vehicles.Where(m => m.PK_Vehicle == currentTrip.FK_Vehicle).FirstOrDefault();
                                    if (!string.IsNullOrEmpty(pendingTrip.Vehicle.FID))
                                    {
                                        var _FK_Depo_To = currentTrip.VehicleSharing.VehicleSharingDemands.Where(n => n.IsHeadDemand == true).FirstOrDefault().FK_Depo_To;
                                        //var title = "Driver: Your Vehicle Entered in Current Trip #" + currentTrip.VehicleSharing.TrackingID + " Destination: " + db.Depoes.Where(m => m.PK_Depo == _FK_Depo_To).FirstOrDefault().Name;
                                        var title = "সম্মানিত চালক, গাড়ি আসন্ন খালি যাত্রার #" + pendingTrip.VehicleSharing.TrackingID + " শুরুর স্থানে (" + db.Depoes.Where(m => m.PK_Depo == _FK_Depo_To).FirstOrDefault().Name + ") প্রবেশ করেছে, যাত্রার সময় গণনা শুরু হয়েছে।";
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
                                    db.SaveChanges();
                                }
                            }
                        }
                        return Json(new { flag = "YES", data = PK_VehicleSharingInternalTrip }, JsonRequestBehavior.AllowGet);
                    }
                    else if (currentTrip.VehicleSharing.FK_Depo_To != currentTrip.Vehicle.FK_LocationInOut || currentTrip.Vehicle.LocationInOrOut != true)
                    {
                        message = "চলমান যাত্রার (#" + currentTrip.VehicleSharing.TrackingID + ") শেষের স্থানে (" + currentTrip.VehicleSharing.Depo1.Name + ") প্রবেশ করুন।";
                        return Json(new { flag = "No", data = PK_VehicleSharingInternalTrip, message = message }, JsonRequestBehavior.AllowGet);
                    }
                    else if (currentTrip.StatusText != InternalTripStatus.StartedUnloading)
                    {
                        message = "চলমান যাত্রার (#" + currentTrip.VehicleSharing.TrackingID + ") মালামাল আনলোড শুরু করুন।";
                        return Json(new { flag = "No", data = PK_VehicleSharingInternalTrip, message = message }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        message = "Undefined Reasson : FinishUnloading 1.1";
                        return Json(new { flag = "No", data = PK_VehicleSharingInternalTrip, message = message }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    message = "যাত্রার তথ্য খুজে পাওয়া যায়নি।";
                    return Json(new { flag = "No", data = PK_VehicleSharingInternalTrip, message = message }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                var errrorMessage = "";
                do { errrorMessage = "#" + errrorMessage + e.Message; if (e.InnerException != null) { e = e.InnerException; } else { break; } } while (true);
                return Json(new { flag = "ERROR", data = PK_VehicleSharingInternalTrip, message = errrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }






        public JsonResult VehicleSharingInternalTrip_Start(Int64 PK_VehicleSharingInternalTrip)
        {
            return Json("unused", JsonRequestBehavior.AllowGet);
            //try
            //{
            //    var pendingTrip = db.VehicleSharingInternalTrips.Where(m => m.PK_VehicleSharingInternalTrip == PK_VehicleSharingInternalTrip).FirstOrDefault();
            //    var currentTrip = db.VehicleSharingInternalTrips.Where(m => m.PK_VehicleSharingInternalTrip == pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Current).FirstOrDefault();

            //    var now = DateTime.Now;
            //    var _PossibleJourneyStartDateTime = pendingTrip.VehicleSharing.PossibleJourneyStartDateTime;
            //    var message = "";

            //    if (currentTrip == null)
            //    {
            //        if (_PossibleJourneyStartDateTime > now)
            //        {
            //            message = "নিচে দেওয়া সময়ে যাত্রা শুরু করুন \n " + _PossibleJourneyStartDateTime.ToString();
            //            return Json(new { flag = "No", data = PK_VehicleSharingInternalTrip, message = message }, JsonRequestBehavior.AllowGet);
            //        }
            //        //-check no current trip and vehicle enter through gate of pending trip from location
            //        if (pendingTrip.IsNotifiedToDriver == true && pendingTrip.Status == 1)
            //        {
            //            pendingTrip.Status = 2;
            //            // Update Vehicle Trip
            //            pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Current = pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Pending;
            //            pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Pending = null;

            //            // Update Driver Trip
            //            pendingTrip.AppUser.FK_VehicleSharingInternalTrip_Current = pendingTrip.AppUser.FK_VehicleSharingInternalTrip_Pending;
            //            pendingTrip.AppUser.FK_VehicleSharingInternalTrip_Pending = null;

            //            //-Notify Assigner Mail
            //            if (!string.IsNullOrEmpty(pendingTrip.AppUser1.Email))
            //            {
            //                var Mail_Subject = "Assigner: Trip is started";
            //                var Mail_Body = "Dear Concern<br>";
            //                Mail_Body = Mail_Body + "Trip Detail" + " <br>"
            //                    + "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + "<br>"
            //                    + "From: " + pendingTrip.VehicleSharing.Depo.Name + "<br>"
            //                    + "To: " + pendingTrip.VehicleSharing.Depo1.Name + "<br>"
            //                    + "On: " + pendingTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
            //                SendMail_Single(pendingTrip.AppUser.Email, Mail_Subject, Mail_Body);
            //            }
            //            //-Notify Requisitor Mail
            //            foreach (var _demand in pendingTrip.VehicleSharing.VehicleSharingDemands)
            //            {
            //                if (!string.IsNullOrEmpty(_demand.AppUser.Email))
            //                {
            //                    var Mail_Subject = "Requisitior: Trip is started";
            //                    var Mail_Body = "Dear Concern<br>";
            //                    Mail_Body = Mail_Body + "Trip Detail" + " <br>"
            //                        + "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + "<br>"
            //                        + "From: " + pendingTrip.VehicleSharing.Depo.Name + "<br>"
            //                        + "To: " + pendingTrip.VehicleSharing.Depo1.Name + "<br>"
            //                        + "On: " + pendingTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
            //                    SendMail_Single(_demand.AppUser.Email, Mail_Subject, Mail_Body);
            //                }
            //            }
            //            db.SaveChanges();
            //            return Json(new { flag = "YES", data = PK_VehicleSharingInternalTrip }, JsonRequestBehavior.AllowGet);
            //        }
            //        //-check no current trip and vehicle is inside of pending trip from location
            //        else if (pendingTrip.IsNotifiedToDriver == true && pendingTrip.VehicleSharing.FK_Depo_From == pendingTrip.Vehicle.FK_DepoInOut && pendingTrip.Vehicle.DepoInOrOut == true)
            //        {
            //            pendingTrip.Status = 2;
            //            // Update Vehicle Trip
            //            pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Current = pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Pending;
            //            pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Pending = null;

            //            // Update Driver Trip
            //            pendingTrip.AppUser.FK_VehicleSharingInternalTrip_Current = pendingTrip.AppUser.FK_VehicleSharingInternalTrip_Pending;
            //            pendingTrip.AppUser.FK_VehicleSharingInternalTrip_Pending = null;

            //            //-Notify Assigner Mail
            //            if (!string.IsNullOrEmpty(pendingTrip.AppUser1.Email))
            //            {
            //                var Mail_Subject = "Assigner: Trip is started";
            //                var Mail_Body = "Dear Concern<br>";
            //                Mail_Body = Mail_Body + "Trip Detail" + " <br>"
            //                    + "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + "<br>"
            //                    + "From: " + pendingTrip.VehicleSharing.Depo.Name + "<br>"
            //                    + "To: " + pendingTrip.VehicleSharing.Depo1.Name + "<br>"
            //                    + "On: " + pendingTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
            //                SendMail_Single(pendingTrip.AppUser.Email, Mail_Subject, Mail_Body);
            //            }
            //            //-Notify Requisitor Mail
            //            foreach (var _demand in pendingTrip.VehicleSharing.VehicleSharingDemands)
            //            {
            //                if (!string.IsNullOrEmpty(_demand.AppUser.Email))
            //                {
            //                    var Mail_Subject = "Requisitior: Trip is started";
            //                    var Mail_Body = "Dear Concern<br>";
            //                    Mail_Body = Mail_Body + "Trip Detail" + " <br>"
            //                        + "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + "<br>"
            //                        + "From: " + pendingTrip.VehicleSharing.Depo.Name + "<br>"
            //                        + "To: " + pendingTrip.VehicleSharing.Depo1.Name + "<br>"
            //                        + "On: " + pendingTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
            //                    SendMail_Single(_demand.AppUser.Email, Mail_Subject, Mail_Body);
            //                }
            //            }
            //            db.SaveChanges();
            //            return Json(new { flag = "YES", data = PK_VehicleSharingInternalTrip }, JsonRequestBehavior.AllowGet);
            //        }
            //        //message = "Vehicle not enterd in Pending trip #" + pendingTrip.VehicleSharing.TrackingID + " from location:" + pendingTrip.VehicleSharing.Depo.Name;
            //        message = "গাড়ির পরবর্তী যাত্রার (#" + pendingTrip.VehicleSharing.TrackingID + ") শুরুর স্থানে (" + pendingTrip.VehicleSharing.Depo.Name + ") প্রবেশ করুন।";
            //    }
            //    else
            //    {
            //        //-check current trip already CLOSED and vehicle enter through gate of pending trip from location
            //        if (currentTrip.Status >= 5 && pendingTrip.IsNotifiedToDriver == true && pendingTrip.Status == 1)
            //        {
            //            if (_PossibleJourneyStartDateTime > now)
            //            {
            //                message = "নিচে দেওয়া সময়ে যাত্রা শুরু করুন \n " + _PossibleJourneyStartDateTime.ToString();
            //                return Json(new { flag = "No", data = PK_VehicleSharingInternalTrip, message = message }, JsonRequestBehavior.AllowGet);
            //            }

            //            pendingTrip.Status = 2;
            //            // Update Vehicle Trip
            //            pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Current = pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Pending;
            //            pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Pending = null;

            //            // Update Driver Trip
            //            pendingTrip.AppUser.FK_VehicleSharingInternalTrip_Current = pendingTrip.AppUser.FK_VehicleSharingInternalTrip_Pending;
            //            pendingTrip.AppUser.FK_VehicleSharingInternalTrip_Pending = null;

            //            //-Notify Assigner Mail
            //            if (!string.IsNullOrEmpty(pendingTrip.AppUser1.Email))
            //            {
            //                var Mail_Subject = "Assigner: Trip is started";
            //                var Mail_Body = "Dear Concern<br>";
            //                Mail_Body = Mail_Body + "Trip Detail" + " <br>"
            //                    + "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + "<br>"
            //                    + "From: " + pendingTrip.VehicleSharing.Depo.Name + "<br>"
            //                    + "To: " + pendingTrip.VehicleSharing.Depo1.Name + "<br>"
            //                    + "On: " + pendingTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
            //                SendMail_Single(pendingTrip.AppUser.Email, Mail_Subject, Mail_Body);
            //            }
            //            //-Notify Requisitor Mail
            //            foreach (var _demand in pendingTrip.VehicleSharing.VehicleSharingDemands)
            //            {
            //                if (!string.IsNullOrEmpty(_demand.AppUser.Email))
            //                {
            //                    var Mail_Subject = "Requisitior: Trip is started";
            //                    var Mail_Body = "Dear Concern<br>";
            //                    Mail_Body = Mail_Body + "Trip Detail" + " <br>"
            //                        + "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + "<br>"
            //                        + "From: " + pendingTrip.VehicleSharing.Depo.Name + "<br>"
            //                        + "To: " + pendingTrip.VehicleSharing.Depo1.Name + "<br>"
            //                        + "On: " + pendingTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
            //                    SendMail_Single(_demand.AppUser.Email, Mail_Subject, Mail_Body);
            //                }
            //            }
            //            db.SaveChanges();
            //            return Json(new { flag = "YES", data = PK_VehicleSharingInternalTrip }, JsonRequestBehavior.AllowGet);
            //        }
            //        //-check current trip already CLOSED and vehicle is inside of pending trip from location
            //        else if (currentTrip.Status >= 5 && pendingTrip.IsNotifiedToDriver == true && pendingTrip.VehicleSharing.FK_Depo_From == pendingTrip.Vehicle.FK_DepoInOut && pendingTrip.Vehicle.DepoInOrOut == true)//Trip closed
            //        {
            //            if (_PossibleJourneyStartDateTime > now)
            //            {
            //                message = "নিচে দেওয়া সময়ে যাত্রা শুরু করুন \n " + _PossibleJourneyStartDateTime.ToString();
            //                return Json(new { flag = "No", data = PK_VehicleSharingInternalTrip, message = message }, JsonRequestBehavior.AllowGet);
            //            }

            //            pendingTrip.Status = 2;
            //            // Update Vehicle Trip
            //            pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Current = pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Pending;
            //            pendingTrip.Vehicle.FK_VehicleSharingInternalTrip_Pending = null;

            //            // Update Driver Trip
            //            pendingTrip.AppUser.FK_VehicleSharingInternalTrip_Current = pendingTrip.AppUser.FK_VehicleSharingInternalTrip_Pending;
            //            pendingTrip.AppUser.FK_VehicleSharingInternalTrip_Pending = null;

            //            //-Notify Assigner Mail
            //            if (!string.IsNullOrEmpty(pendingTrip.AppUser1.Email))
            //            {
            //                var Mail_Subject = "Assigner: Trip is started";
            //                var Mail_Body = "Dear Concern<br>";
            //                Mail_Body = Mail_Body + "Trip Detail" + " <br>"
            //                    + "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + "<br>"
            //                    + "From: " + pendingTrip.VehicleSharing.Depo.Name + "<br>"
            //                    + "To: " + pendingTrip.VehicleSharing.Depo1.Name + "<br>"
            //                    + "On: " + pendingTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
            //                SendMail_Single(pendingTrip.AppUser.Email, Mail_Subject, Mail_Body);
            //            }
            //            //-Notify Requisitor Mail
            //            foreach (var _demand in pendingTrip.VehicleSharing.VehicleSharingDemands)
            //            {
            //                if (!string.IsNullOrEmpty(_demand.AppUser.Email))
            //                {
            //                    var Mail_Subject = "Requisitior: Trip is started";
            //                    var Mail_Body = "Dear Concern<br>";
            //                    Mail_Body = Mail_Body + "Trip Detail" + " <br>"
            //                        + "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + "<br>"
            //                        + "From: " + pendingTrip.VehicleSharing.Depo.Name + "<br>"
            //                        + "To: " + pendingTrip.VehicleSharing.Depo1.Name + "<br>"
            //                        + "On: " + pendingTrip.VehicleSharing.PossibleJourneyStartDateTime + "<br>" + "\n";
            //                    SendMail_Single(_demand.AppUser.Email, Mail_Subject, Mail_Body);
            //                }
            //            }
            //            db.SaveChanges();
            //            return Json(new { flag = "YES", data = PK_VehicleSharingInternalTrip }, JsonRequestBehavior.AllowGet);
            //        }

            //        if (!(currentTrip.Status >= 5))
            //        {
            //            //message = "Current trip #" + currentTrip.VehicleSharing.TrackingID + " not unloaded yet.";
            //            message = "চলমান যাত্রার (#" + currentTrip.VehicleSharing.TrackingID + ") মালামাল আনলোড করুন।";
            //        }
            //        else
            //        {
            //            //message = "Vehicle not enterd in Pending trip #" + pendingTrip.VehicleSharing.TrackingID + " from location:" + pendingTrip.VehicleSharing.Depo.Name;
            //            message = "গাড়ির পরবর্তী যাত্রার (#" + pendingTrip.VehicleSharing.TrackingID + ") শুরুর স্থানে (" + pendingTrip.VehicleSharing.Depo.Name + ") প্রবেশ করুন।";
            //        }
            //    }
            //    return Json(new { flag = "No", data = PK_VehicleSharingInternalTrip, message = message }, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception e)
            //{
            //    var errrorMessage = "";
            //    do { errrorMessage = "#" + errrorMessage + e.Message; if (e.InnerException != null) { e = e.InnerException; } else { break; } } while (true);
            //    return Json(new { flag = "ERROR", data = PK_VehicleSharingInternalTrip, message = errrorMessage }, JsonRequestBehavior.AllowGet);
            //}
        }

        //# Gate In Out

        //public JsonResult VehicleInOut(Guid PK_Vehicle, string DepoCode, string InOut)
        //{
        //    try
        //    {
        //        bool InOutBool = true;//InOut == "In"
        //        if (InOut == "Out")
        //        {
        //            InOutBool = false;
        //        }
        //        var FK_Depo = db.Depoes.Where(m => m.Code == DepoCode).FirstOrDefault().PK_Depo;
        //        var model = new VehicleInOutManual();
        //        model.DevelopersNote = "called by API";
        //        model.FK_Vehicle = PK_Vehicle;
        //        model.FK_Depo = FK_Depo;
        //        model.InOrOut = InOutBool;
        //        model.CreatedAt = DateTime.Now;
        //        model.IssueDateTime = model.CreatedAt;
        //        model.FK_CreatedByUser = db.AppUsers.FirstOrDefault().PK_User;
        //        db.VehicleInOutManuals.Add(model);

        //        var vehicle = db.Vehicles.Where(m => m.PK_Vehicle == PK_Vehicle).FirstOrDefault();
        //        vehicle.FK_DepoInOut = model.FK_Depo;
        //        vehicle.DepoInOutTime = model.CreatedAt;
        //        vehicle.DepoInOrOut = model.InOrOut;
        //        db.SaveChanges();

        //        if (model.InOrOut == true)
        //        {
        //            var currentTrip = db.VehicleSharingInternalTrips.Where(m => m.PK_VehicleSharingInternalTrip == vehicle.FK_VehicleSharingInternalTrip_Current).FirstOrDefault();
        //            //# Entered with a Current Trip 
        //            if (currentTrip != null && currentTrip.StatusText == InternalTripStatus.LeftStartingLoaction && currentTrip.VehicleSharing.Depo1.PK_Depo == model.FK_Depo)
        //            {
        //                currentTrip.StatusText = InternalTripStatus.StartedUnloading;
        //                currentTrip.UnloadingStartDateTime = DateTime.Now;
        //                //-Notify Driver Firebase
        //                //var vehicle = db.Vehicles.Where(m => m.PK_Vehicle == currentTrip.FK_Vehicle).FirstOrDefault();
        //                if (!string.IsNullOrEmpty(vehicle.FID))
        //                {
        //                    var _FK_Depo_To = currentTrip.VehicleSharing.VehicleSharingDemands.Where(n => n.IsHeadDemand == true).FirstOrDefault().FK_Depo_To;
        //                    //var title = "Driver: Your Vehicle Entered in Current Trip #" + currentTrip.VehicleSharing.TrackingID + " Destination: " + db.Depoes.Where(m => m.PK_Depo == _FK_Depo_To).FirstOrDefault().Name;
        //                    var title = "সম্মানিত চালক, গাড়ি চলমান যাত্রার (#" + currentTrip.VehicleSharing.TrackingID + ") গন্তব্যে (" + db.Depoes.Where(m => m.PK_Depo == _FK_Depo_To).FirstOrDefault().Name + ") পৌছিয়েছে, আনলোডিং-এর সময় গণনা শুরু হয়েছে।";
        //                    var message = "Dear Concern \n";
        //                    message = message + "Trip Detail" + "\n"
        //                        + "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + "\n"
        //                        + "From: " + currentTrip.VehicleSharing.Depo.Name + "\n"
        //                        + "To: " + currentTrip.VehicleSharing.Depo1.Name + "\n"
        //                        + "On: " + currentTrip.VehicleSharing.PossibleJourneyStartDateTime + "\n";
        //                    SendFCM_Notification_Single_New(vehicle.FID, title, message, currentTrip.PK_VehicleSharingInternalTrip.ToString(), "VehicleSharingInternalTrip");
        //                }
        //                db.SaveChanges();
        //            }
        //            var pendingTrip = db.VehicleSharingInternalTrips.Where(m => m.PK_VehicleSharingInternalTrip == vehicle.FK_VehicleSharingInternalTrip_Pending).FirstOrDefault();
        //            if (pendingTrip != null && pendingTrip.StatusText == InternalTripStatus.Assigned && pendingTrip.VehicleSharing.Depo.PK_Depo == model.FK_Depo)
        //            {
        //                pendingTrip.StatusText = InternalTripStatus.EnteredStartingLocation;
        //                //-Notify Driver Firebase
        //                //var vehicle = db.Vehicles.Where(m => m.PK_Vehicle == currentTrip.FK_Vehicle).FirstOrDefault();
        //                if (!string.IsNullOrEmpty(vehicle.FID))
        //                {
        //                    var _FK_Depo_From = pendingTrip.VehicleSharing.VehicleSharingDemands.Where(n => n.IsHeadDemand == true).FirstOrDefault().FK_Depo_From;
        //                    //var title = "Driver: Your Vehicle Entered in Pending Trip #" + pendingTrip.VehicleSharing.TrackingID + " Strating Location: " + db.Depoes.Where(m => m.PK_Depo == _FK_Depo_From).FirstOrDefault().Name;
        //                    var title = "সম্মানিত চালক, গাড়ি আসন্ন যাত্রার #" + pendingTrip.VehicleSharing.TrackingID + " শুরুর স্থানে (" + db.Depoes.Where(m => m.PK_Depo == _FK_Depo_From).FirstOrDefault().Name + ") প্রবেশ করেছে।";
        //                    var message = "Dear Concern \n";
        //                    message = message + "Trip Detail" + "\n"
        //                        + "Vehicle: " + pendingTrip.Vehicle.RegistrationNumber + "\n"
        //                        + "From: " + pendingTrip.VehicleSharing.Depo.Name + "\n"
        //                        + "To: " + pendingTrip.VehicleSharing.Depo1.Name + "\n"
        //                        + "On: " + pendingTrip.VehicleSharing.PossibleJourneyStartDateTime + "\n";
        //                    SendFCM_Notification_Single_New(vehicle.FID, title, message, pendingTrip.PK_VehicleSharingInternalTrip.ToString(), "VehicleSharingInternalTrip");
        //                }
        //                db.SaveChanges();
        //            }
        //        }
        //        if (model.InOrOut == false)
        //        {
        //            var currentTrip = db.VehicleSharingInternalTrips.Where(m => m.PK_VehicleSharingInternalTrip == vehicle.FK_VehicleSharingInternalTrip_Current).FirstOrDefault();
        //            if (currentTrip != null && currentTrip.StatusText == InternalTripStatus.CreatedBill && currentTrip.VehicleSharing.Depo.PK_Depo == model.FK_Depo)
        //            {
        //                currentTrip.StatusText = InternalTripStatus.LeftStartingLoaction;

        //                //-Notify Driver Firebase
        //                //var vehicle = db.Vehicles.Where(m => m.PK_Vehicle == currentTrip.FK_Vehicle).FirstOrDefault();
        //                if (!string.IsNullOrEmpty(vehicle.FID))
        //                {
        //                    var _FK_Depo_From = currentTrip.VehicleSharing.VehicleSharingDemands.Where(n => n.IsHeadDemand == true).FirstOrDefault().FK_Depo_From;
        //                    //var title = "Driver: Your Vehicle Left Current Trip #" + currentTrip.VehicleSharing.TrackingID + " Staring Location: " + db.Depoes.Where(m => m.PK_Depo == _FK_Depo_From).FirstOrDefault().Name;
        //                    var title = "সম্মানিত চালক, গাড়ি চলমান যাত্রার (#" + currentTrip.VehicleSharing.TrackingID + ") শুরুর স্থান (" + db.Depoes.Where(m => m.PK_Depo == _FK_Depo_From).FirstOrDefault().Name + ") ত্যাগ করেছে।";
        //                    var message = "Dear Concern \n";
        //                    message = message + "Trip Detail" + "\n"
        //                        + "Vehicle: " + currentTrip.Vehicle.RegistrationNumber + "\n"
        //                        + "From: " + currentTrip.VehicleSharing.Depo.Name + "\n"
        //                        + "To: " + currentTrip.VehicleSharing.Depo1.Name + "\n"
        //                        + "On: " + currentTrip.VehicleSharing.PossibleJourneyStartDateTime + "\n";
        //                    SendFCM_Notification_Single_New(vehicle.FID, title, message, currentTrip.PK_VehicleSharingInternalTrip.ToString(), "VehicleSharingInternalTrip");
        //                }
        //                db.SaveChanges();
        //            }
        //        }
        //        return Json(new { flag = "YES", data = new { PK_Vehicle, DepoCode, InOut } }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception e)
        //    {
        //        var errrorMessage = "";
        //        do { errrorMessage = "#" + errrorMessage + e.Message; if (e.InnerException != null) { e = e.InnerException; } else { break; } } while (true);
        //        return Json(new { flag = "ERROR", data = new { PK_Vehicle, DepoCode, InOut }, message = errrorMessage }, JsonRequestBehavior.AllowGet);
        //    }
        //}
    }
}