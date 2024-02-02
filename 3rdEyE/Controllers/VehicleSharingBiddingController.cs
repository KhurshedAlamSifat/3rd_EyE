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
using _3rdEyE.ViewModels;

namespace _3rdEyE.Controllers
{
    public class VehicleSharingBiddingController : BaseController
    {
        static class VehicleSharingBiddingStatus
        {
            public const string Created = "Created";
            public const string Bidded = "Bidded";
            public const string Approved = "Approved";
            public const string CancelledByClient = "Cancelled By Client";
            public const string CancelledByApprover = "Cancelled By Approver";
        }
        public ActionResult Index_Bidder(DateTime? StartingDate, DateTime? EndingDate)
        {
            //if (CommonClass.IsInvalidAccess())
            //{
            //    return Redirect("/Access/Login");
            //}
            //var lastTime = DateTime.Now.AddDays(-7).Date;
            //var list = bll.db.VehicleSharingBiddings.AsEnumerable().Where(m => m.FK_RequisitionAgent_Bidder == CurrentUser.PK_User && m.CreatedAt >= lastTime && m.StatusText != VehicleSharingBiddingStatus.CancelledByClient).OrderByDescending(m => m.VehicleSharing.CreatedAt).ToList();
            //return View(list);
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
                var list = bll.db.VehicleSharingBiddings.AsEnumerable().Where(m => m.FK_RequisitionAgent_Bidder == CurrentUser.PK_User && m.CreatedAt >= _StartingDate && m.CreatedAt <= _EndingDate).OrderByDescending(m => m.CreatedAt).ToList();
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
                var list = new List<VehicleSharingBidding>();
                return View(list);
            }
        }

        public ActionResult ViewForBidder_ToBid(Int64 PK_VehicleSharingBidding)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var VehicleSharingBidding = bll.db.VehicleSharingBiddings.Find(PK_VehicleSharingBidding);
            if (VehicleSharingBidding != null)
            {
                //var model = bll.ConvertToViewModel(VehicleSharingBidding);
                return View(VehicleSharingBidding);
            }
            else
            {
                return HttpNotFound();
            }
        }
        [HttpPost]
        public ActionResult ViewForBidder_ToBid(VehicleSharingBidding model)
        {
            try
            {
                var db_model = bll.db.VehicleSharingBiddings.Find(model.PK_VehicleSharingBidding);
                //# todo check last hour
                if (db_model.VehicleSharing.Status != 0 || db_model.VehicleSharing.KeepBidOpenUntil < DateTime.Now)
                {
                    CreateAlertMessage(AlertMessageType.Danger, "Unsuccess", "Sorry, the Requisition could not be bidded as the requisition is not open anymore");
                    return RedirectToAction("Index_Bidder");
                }
                if (db_model.StatusText != VehicleSharingBiddingStatus.Created)
                {
                    CreateAlertMessage(AlertMessageType.Danger, "Unsuccess", "Sorry, the Requisition could not be bidded as the bidding satus is: " + db_model.StatusText);
                    return RedirectToAction("Index_Bidder");
                }
                db_model.StatusText = VehicleSharingBiddingStatus.Bidded;
                db_model.ManagableQuantity = model.ManagableQuantity;
                db_model.PricePerQuantity = model.PricePerQuantity;
                bll.db.RequisitionAgentNotifications.Add(new RequisitionAgentNotification()
                {
                    FK_RequisitionAgent = db_model.VehicleSharing.AppUser.PK_User,
                    Status = 0,
                    Title = "New bid by -" + db_model.AppUser.FullName + " [" + db_model.AppUser.UniqueIDNumber + "]",
                    ViewLink = "/VehicleSharing/ViewForClient_ToApprove?PK_VehicleSharing=" + db_model.FK_VehicleSharing,
                    SubTitle = "New bid by -" + db_model.AppUser.FullName + " [" + db_model.AppUser.UniqueIDNumber + "] " + db_model.ManagableQuantity + " " + db_model.VehicleSharing.VehicleType + ", BDT " + db_model.PricePerQuantity + " each",
                    CreatedAt = DateTime.Now,
                    Category = RequistionNotificationCategory.Individual_Requisition_Bid_Created
                });
                bll.db.SaveChanges();

                //# SMS
                var NumbersToSMS = db_model.VehicleSharing.AppUser.ContactNumber;
                if (!string.IsNullOrEmpty(NumbersToSMS) && NumbersToSMS.Last() == ',')
                {
                    NumbersToSMS = NumbersToSMS.Substring(0, NumbersToSMS.Length - 1);
                }
                string messageBody = "New bid by -" + db_model.AppUser.FullName + " [" + db_model.AppUser.UniqueIDNumber + "] " + db_model.ManagableQuantity + " " + db_model.VehicleSharing.VehicleType + ", BDT " + db_model.PricePerQuantity + " each";
                //var sms_response = SendSMS(NumbersToSMS, messageBody);

                //# Firebase notifier
                var FK_FirebaseAppUser = db_model.VehicleSharing.AppUser.PK_User;
                var Title = "New bid by -" + db_model.AppUser.FullName + " [" + db_model.AppUser.UniqueIDNumber + "]";
                var SubTitle = "New bid by -" + db_model.AppUser.FullName + " [" + db_model.AppUser.UniqueIDNumber + "] " + db_model.ManagableQuantity + " " + db_model.VehicleSharing.VehicleType + ", BDT " + db_model.PricePerQuantity + " each";
                var Category = RequistionNotificationCategory.Individual_Requisition_Bid_Created;
                var fcm_response = SendFCM_Notification_Single(FK_FirebaseAppUser, Category, Title, SubTitle);


                CreateAlertMessage(AlertMessageType.Success, "Success", "Individual Requisition is successfully bidded.");
                return RedirectToAction("Index_Bidder");
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
            }
            return View(model);
        }

        public ActionResult ViewForBidder_ToEditBid(Int64 PK_VehicleSharingBidding)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            if (PK_VehicleSharingBidding == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var VehicleSharingBidding = bll.db.VehicleSharingBiddings.Find(PK_VehicleSharingBidding);
                if (VehicleSharingBidding != null)
                {
                    return View(VehicleSharingBidding);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
        [HttpPost]
        public ActionResult ViewForBidder_ToEditBid(VehicleSharingBidding model)
        {
            try
            {
                var db_model = bll.db.VehicleSharingBiddings.Find(model.PK_VehicleSharingBidding);
                if (db_model.VehicleSharing.Status != 0)
                {
                    CreateAlertMessage(AlertMessageType.Danger, "Unsuccess", "Sorry, the Requisition could not be re-bidded as the requisition is not open anymore");
                    return RedirectToAction("Index_Bidder");
                }
                if (db_model.StatusText != VehicleSharingBiddingStatus.Bidded)
                {
                    CreateAlertMessage(AlertMessageType.Danger, "Unsuccess", "Sorry, the Requisition could not be bidded as the bidding satus is: " + db_model.StatusText);
                    return RedirectToAction("Index_Bidder");
                }
                db_model.StatusText = VehicleSharingBiddingStatus.Bidded;
                db_model.ManagableQuantity = model.ManagableQuantity;
                db_model.PricePerQuantity = model.PricePerQuantity;
                bll.db.RequisitionAgentNotifications.Add(new RequisitionAgentNotification()
                {
                    FK_RequisitionAgent = db_model.VehicleSharing.AppUser.PK_User,
                    Status = 0,
                    Title = "Updated bid by -" + db_model.AppUser.FullName + " [" + db_model.AppUser.UniqueIDNumber + "]",
                    ViewLink = "/VehicleSharing/ViewForClient_ToApprove?PK_VehicleSharing=" + db_model.FK_VehicleSharing,
                    SubTitle = "Updated bid by -" + db_model.AppUser.FullName + " [" + db_model.AppUser.UniqueIDNumber + "] " + db_model.ManagableQuantity + " " + db_model.VehicleSharing.VehicleType + ", BDT " + db_model.PricePerQuantity + " each",
                    CreatedAt = DateTime.Now,
                    Category = RequistionNotificationCategory.Individual_Requisition_Bid_Created
                });
                bll.db.SaveChanges();

                //# SMS
                var NumbersToSMS = db_model.VehicleSharing.AppUser.ContactNumber;
                if (!string.IsNullOrEmpty(NumbersToSMS) && NumbersToSMS.Last() == ',')
                {
                    NumbersToSMS = NumbersToSMS.Substring(0, NumbersToSMS.Length - 1);
                }
                string messageBody = "Updated bid by -" + db_model.AppUser.FullName + " [" + db_model.AppUser.UniqueIDNumber + "] " + db_model.ManagableQuantity + " " + db_model.VehicleSharing.VehicleType + ", BDT " + db_model.PricePerQuantity + " each";
                //var sms_response = SendSMS(NumbersToSMS, messageBody);

                //# Firebase notifier
                var FK_FirebaseAppUser = db_model.VehicleSharing.AppUser.PK_User;
                var Title = "Updated bid by -" + db_model.AppUser.FullName + " [" + db_model.AppUser.UniqueIDNumber + "]";
                var SubTitle = "Updated bid by -" + db_model.AppUser.FullName + " [" + db_model.AppUser.UniqueIDNumber + "] " + db_model.ManagableQuantity + " " + db_model.VehicleSharing.VehicleType + ", BDT " + db_model.PricePerQuantity + " each";
                var Category = RequistionNotificationCategory.Individual_Requisition_Bid_Created;
                var fcm_response = SendFCM_Notification_Single(FK_FirebaseAppUser, Category, Title, SubTitle);


                CreateAlertMessage(AlertMessageType.Success, "Success", "Individual Requisition Bid is successfully Updated.");
                return RedirectToAction("Index_Bidder");
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
            }
            return View(model);
        }

        public ActionResult ViewForBidder_Approved(Int64 PK_VehicleSharingBidding)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            if (PK_VehicleSharingBidding == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var VehicleSharingBidding = bll.db.VehicleSharingBiddings.Find(PK_VehicleSharingBidding);
                if (VehicleSharingBidding != null)
                {
                    return View(VehicleSharingBidding);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        public ActionResult ViewForBidder_Disapproved(Int64 PK_VehicleSharingBidding)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            if (PK_VehicleSharingBidding == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var VehicleSharingBidding = bll.db.VehicleSharingBiddings.Find(PK_VehicleSharingBidding);
                if (VehicleSharingBidding != null)
                {
                    return View(VehicleSharingBidding);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        // GET: Drivers/Delete/5
        public ActionResult CancelByClient(Int64 id)
        {
            var model = bll.db.VehicleSharingBiddings.Find(id);
            if (model != null)
            {
                try
                {
                    var db_model = bll.db.VehicleSharingBiddings.Find(model.PK_VehicleSharingBidding);
                    if (db_model.StatusText != VehicleSharingBiddingStatus.Created && db_model.StatusText != VehicleSharingBiddingStatus.Bidded)
                    {
                        CreateAlertMessage(AlertMessageType.Danger, "Unsuccess", "Sorry, the bidding could not be cancelled as the requisition status is: " + db_model.StatusText);
                        return RedirectToAction("Index_Bidder");
                    }
                    model.StatusText = VehicleSharingBiddingStatus.CancelledByClient;
                    bll.db.SaveChanges();
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Requisition is cancelled.");
                    return RedirectToAction("Index_Bidder");
                }
                catch (Exception exception)
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                    return RedirectToAction("Index_Bidder");
                }
            }
            else
            {
                return HttpNotFound();
            }

        }


        // Manage Methods
        public string IsValidModel_ToCreate(VehicleSharingBidding model)
        {
            string result = "";
            if (bll.db.VehicleSharingBiddings.Where(c => c.FK_VehicleSharing == model.FK_VehicleSharing && c.FK_RequisitionAgent_Bidder == model.FK_RequisitionAgent_Bidder).Any())
            {
                result += "This Individual Requisition is already bidded. Can not bid more than once.";
            }
            if (result == "")
            {
                result = ValidationStatus.OK;
            }
            return result;
        }

        public VehicleSharingBidding FilterToDBModel(VehicleSharingBidding model)
        {
            VehicleSharingBidding db_model;
            db_model = new VehicleSharingBidding();

            db_model.FK_VehicleSharing = model.FK_VehicleSharing;
            db_model.FK_RequisitionAgent_Bidder = model.FK_RequisitionAgent_Bidder;
            db_model.ManagableQuantity = model.ManagableQuantity;
            db_model.PricePerQuantity = model.PricePerQuantity;
            db_model.StatusText = model.StatusText;
            return db_model;
        }

    }
}