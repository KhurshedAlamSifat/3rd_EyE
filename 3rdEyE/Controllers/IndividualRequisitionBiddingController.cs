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

namespace _3rdEyE.Controllers
{
    public class IndividualRequisitionBiddingController : BaseController
    {
        BLL_IndividualRequisitionBidding bll = new BLL_IndividualRequisitionBidding();

        public ActionResult Index_Bidder()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var lastTime = DateTime.Now.AddDays(-7).Date;
            var list = bll.db.IndividualRequisitionBiddings.AsEnumerable().Where(m => m.FK_RequisitionAgent_Bidder == CurrentUser.PK_User && m.IndividualRequisition.CreatedAt >= lastTime && m.Status != -1).OrderByDescending(m => m.IndividualRequisition.CreatedAt).Select(c => bll.ConvertToViewModel(c)).ToList();
            return View(list);
        }

        public ActionResult ViewForBidder_ToBid(Guid PK_IndividualRequisitionBidding)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            if (PK_IndividualRequisitionBidding == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var individualRequisitionBidding = bll.db.IndividualRequisitionBiddings.Find(PK_IndividualRequisitionBidding);
                if (individualRequisitionBidding != null)
                {
                    //var model = bll.ConvertToViewModel(individualRequisitionBidding);
                    return View(individualRequisitionBidding);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
        [HttpPost]
        public ActionResult ViewForBidder_ToBid(IndividualRequisitionBidding model)
        {
            try
            {
                var db_model = bll.db.IndividualRequisitionBiddings.Find(model.PK_IndividualRequisitionBidding);
                if (db_model.IndividualRequisition.Status != 0)
                {
                    CreateAlertMessage(AlertMessageType.Danger, "Unsuccess", "Sorry, the Requisition could not be bidded as the requisition is not open anymore");
                    return RedirectToAction("Index_Bidder");
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


                CreateAlertMessage(AlertMessageType.Success, "Success", "Individual Requisition is successfully bidded.");
                return RedirectToAction("Index_Bidder");
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
            }
            return View(model);
        }

        public ActionResult ViewForBidder_ToEditBid(Guid PK_IndividualRequisitionBidding)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            if (PK_IndividualRequisitionBidding == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var individualRequisitionBidding = bll.db.IndividualRequisitionBiddings.Find(PK_IndividualRequisitionBidding);
                if (individualRequisitionBidding != null)
                {
                    return View(individualRequisitionBidding);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
        [HttpPost]
        public ActionResult ViewForBidder_ToEditBid(IndividualRequisitionBidding model)
        {
            try
            {
                var db_model = bll.db.IndividualRequisitionBiddings.Find(model.PK_IndividualRequisitionBidding);
                if (db_model.IndividualRequisition.Status != 0)
                {
                    CreateAlertMessage(AlertMessageType.Danger, "Unsuccess", "Sorry, the Requisition could not be re-bidded as the requisition is not open anymore");
                    return RedirectToAction("Index_Bidder");
                }
                db_model.Status = 1;
                db_model.ManagableQuantity = model.ManagableQuantity;
                db_model.PricePerQuantity = model.PricePerQuantity;
                bll.db.RequisitionAgentNotifications.Add(new RequisitionAgentNotification()
                {
                    FK_RequisitionAgent = db_model.IndividualRequisition.AppUser.PK_User,
                    Status = 0,
                    Title = "Updated bid by -" + db_model.AppUser.FullName + " [" + db_model.AppUser.UniqueIDNumber + "]",
                    ViewLink = "/IndividualRequisition/ViewForClient_ToApprove?PK_IndividualRequisition=" + db_model.FK_IndividualRequisition,
                    SubTitle = "Updated bid by -" + db_model.AppUser.FullName + " [" + db_model.AppUser.UniqueIDNumber + "] " + db_model.ManagableQuantity + " " + db_model.IndividualRequisition.VehicleTypeLayer1_english + ":" + db_model.IndividualRequisition.VehicleTypeLayer2_english + ":" + db_model.IndividualRequisition.VehicleTypeLayer3_english + ", BDT " + db_model.PricePerQuantity + " each",
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
                string messageBody = "Updated bid by -" + db_model.AppUser.FullName + " [" + db_model.AppUser.UniqueIDNumber + "] " + db_model.ManagableQuantity + " " + db_model.IndividualRequisition.VehicleTypeLayer1_english + ":" + db_model.IndividualRequisition.VehicleTypeLayer2_english + ":" + db_model.IndividualRequisition.VehicleTypeLayer3_english + ", BDT " + db_model.PricePerQuantity + " each";
                //var sms_response = SendSMS(NumbersToSMS, messageBody);

                //# Firebase notifier
                var FK_FirebaseAppUser = db_model.IndividualRequisition.AppUser.PK_User;
                var Title = "Updated bid by -" + db_model.AppUser.FullName + " [" + db_model.AppUser.UniqueIDNumber + "]";
                var SubTitle = "Updated bid by -" + db_model.AppUser.FullName + " [" + db_model.AppUser.UniqueIDNumber + "] " + db_model.ManagableQuantity + " " + db_model.IndividualRequisition.VehicleTypeLayer1_english + ":" + db_model.IndividualRequisition.VehicleTypeLayer2_english + ":" + db_model.IndividualRequisition.VehicleTypeLayer3_english + ", BDT " + db_model.PricePerQuantity + " each";
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

        public ActionResult ViewForBidder_Approved(Guid PK_IndividualRequisitionBidding)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            if (PK_IndividualRequisitionBidding == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var individualRequisitionBidding = bll.db.IndividualRequisitionBiddings.Find(PK_IndividualRequisitionBidding);
                if (individualRequisitionBidding != null)
                {
                    var individualRequisitionBidding_viewModel = bll.ConvertToViewModel(individualRequisitionBidding);
                    //var individualRequisition = bll.db.IndividualRequisitions.Where(m => m.PK_IndividualRequisition == individualRequisitionBidding.FK_IndividualRequisition).FirstOrDefault();
                    //ViewBag.individualRequisition_viewModel = new BLL_IndividualRequisition().ConvertToViewModel(individualRequisition);
                    return View(individualRequisitionBidding_viewModel);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        public ActionResult ViewForBidder_Disapproved(Guid PK_IndividualRequisitionBidding)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            if (PK_IndividualRequisitionBidding == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var individualRequisitionBidding = bll.db.IndividualRequisitionBiddings.Find(PK_IndividualRequisitionBidding);
                if (individualRequisitionBidding != null)
                {
                    var individualRequisitionBidding_viewModel = bll.ConvertToViewModel(individualRequisitionBidding);
                    //var individualRequisition = bll.db.IndividualRequisitions.Where(m => m.PK_IndividualRequisition == individualRequisitionBidding.FK_IndividualRequisition).FirstOrDefault();
                    //ViewBag.individualRequisition_viewModel = new BLL_IndividualRequisition().ConvertToViewModel(individualRequisition);
                    return View(individualRequisitionBidding_viewModel);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        // GET: Drivers/Delete/5
        public ActionResult Cancel(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var model = bll.db.IndividualRequisitionBiddings.Find(id);
                if (model != null)
                {
                    try
                    {
                        var db_model = bll.db.IndividualRequisitionBiddings.Find(model.PK_IndividualRequisitionBidding);
                        if (db_model.Status != 1 && db_model.Status != 2)
                        {
                            CreateAlertMessage(AlertMessageType.Danger, "Unsuccess", "Sorry, the bidding could not be cancelled as the requisition is already closed");
                            return RedirectToAction("Index_Bidder");
                        }
                        model.Status = -1;
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
        }
    }
}