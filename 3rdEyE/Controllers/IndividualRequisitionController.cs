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

namespace _3rdEyE.Controllers
{
    public class IndividualRequisitionController : BaseController
    {
        BLL_IndividualRequisition bll = new BLL_IndividualRequisition();
        //Dictionary<string, string> VehicleTypesDict = new Dictionary<string, string> { { "Ambulance", "Ambulance" }, { "Bus", "Bus" }, { "Cargo Truck", "Cargo Truck" }, { "Cargo Truck - Open", "Cargo Truck - Open" }, { "Cargo VAN", "Cargo VAN" }, { "Open VAN", "Open VAN" }, { "Concrete Mixer", "Concrete Mixer" }, { "Covered Van", "Covered Van" }, { "Delivery Van", "Delivery Van" }, { "Milk Tanker", "Milk Tanker" }, { "Mini Bus", "Mini Bus" }, { "Mini Truck", "Mini Truck" }, { "Mobile Crance", "Mobile Crance" }, { "Motor Car", "Motor Car" }, { "OMNI Bus", "OMNI Bus" }, { "Pickup", "Pickup" }, { "Refrigerator Van", "Refrigerator Van" }, { "Tank Lorry", "Tank Lorry" }, { "Tipper", "Tipper" }, { "Trailers", "Trailers" }, { "Water Tanker", "Water Tanker" } };
        //Dictionary<double, string> VehicleCapacityDict = new Dictionary<double, string> { { 0.8, "0.8 ton" }, { 1, "1 ton" }, { 1.5, "1.5 tons" }, { 2, "2 tons" }, { 3, "3 tons" }, { 5, "5 tons" }, { 7, "7 tons" }, { 12, "12 tons" }, { 20, "20 tons" } };

        public ActionResult IndexBy_Client()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var lastTime = DateTime.Now.AddDays(-7).Date;
            List<ViewModels.VM_IndividualRequisition> list;
            list = bll.db.IndividualRequisitions.AsEnumerable().Where(m => m.IsDeleted != true && m.FK_RequisitionAgent == CurrentUser.PK_User && m.CreatedAt >= lastTime).OrderByDescending(m => m.CreatedAt).Select(c => bll.ConvertToViewModel(c)).ToList();
            return View(list);
        }

        public ActionResult Create()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            ViewBag.VehicleTypesDict = new SelectList(bll.db.RequisitionVehicleTypes.Select(m => new { Key = m.Layer1 + "#" + m.Layer2 + "#" + m.Layer3, Value = m.Layer1_english + " : " + m.Layer2_english + " : " + m.Layer3_english }), "Key", "Value");
            var requisitionAgentProposedDepoes = bll.db.RequisitionAgentProposedDepoes.Where(m => m.FK_RequisitionAgent == CurrentUser.PK_User && m.WillPropose == true).Select(m => m.FK_Depo).ToList();
            var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
            ViewBag.OtherDepoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && m.PK_Depo != _invalidDepoPK && !(requisitionAgentProposedDepoes.Contains(m.PK_Depo))).OrderBy(m => m.Name), "PK_Depo", "Name");
            ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && m.PK_Depo != _invalidDepoPK && m.Latitude != null && m.Longitude != null).OrderBy(m => m.Name), "PK_Depo", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection form)
        {
            IndividualRequisition individualRequisition = new IndividualRequisition();
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            try
            {
                individualRequisition = new IndividualRequisition();
                individualRequisition.PK_IndividualRequisition = Guid.NewGuid();
                individualRequisition.IsDeleted = false;

                individualRequisition.CreatedAt = DateTime.Now;
                individualRequisition.FK_RequisitionAgent = CurrentUser.PK_User;

                if (!string.IsNullOrEmpty(form["FK_Depo_From"]))
                {
                    individualRequisition.FK_Depo_From = Guid.Parse(form["FK_Depo_From"]);
                    individualRequisition.Depo = bll.db.Depoes.Where(m => m.PK_Depo == individualRequisition.FK_Depo_From).FirstOrDefault();
                }
                else
                {
                    individualRequisition.StartingLocation = form["StartingLocation"];
                }
                if (!string.IsNullOrEmpty(form["FK_Depo_To"]))
                {
                    individualRequisition.FK_Depo_To = Guid.Parse(form["FK_Depo_To"]);
                    individualRequisition.Depo1 = bll.db.Depoes.Where(m => m.PK_Depo == individualRequisition.FK_Depo_To).FirstOrDefault();
                }
                else
                {
                    individualRequisition.FinishingLocation = form["FinishingLocation"];
                }
                var _VehicleType = form["VehicleType"].Split('#');
                var _Layer1 = _VehicleType[0];
                var _Layer2 = _VehicleType[1];
                var _Layer3 = _VehicleType[2];
                var VehicleType = bll.db.RequisitionVehicleTypes.Where(m => m.Layer1 == _Layer1 && m.Layer2 == _Layer2 && m.Layer3 == _Layer3).FirstOrDefault();
                individualRequisition.VehicleTypeLayer1 = VehicleType.Layer1;
                individualRequisition.VehicleTypeLayer1_english = VehicleType.Layer1_english;
                individualRequisition.VehicleTypeLayer1_bangla = VehicleType.Layer1_bangla;
                individualRequisition.VehicleTypeLayer2 = VehicleType.Layer2;
                individualRequisition.VehicleTypeLayer2_english = VehicleType.Layer2_english;
                individualRequisition.VehicleTypeLayer2_bangla = VehicleType.Layer2_bangla;
                individualRequisition.VehicleTypeLayer3 = VehicleType.Layer3;
                individualRequisition.VehicleTypeLayer3_english = VehicleType.Layer3_english;
                individualRequisition.VehicleTypeLayer3_bangla = VehicleType.Layer3_bangla;
                individualRequisition.WantedCount = Convert.ToInt32(form["WantedCount"]);
                individualRequisition.PossibleJourneyStartDateTime = DateTime.ParseExact(form["PossibleJourneyStartDateTime"], "yyyy-MM-dd h:mm tt", CultureInfo.InvariantCulture);
                individualRequisition.Status = 0;

                bll.db.IndividualRequisitions.Add(individualRequisition);
                //# notification for own depo's agent
                var notificatoinList = new List<RequisitionAgentNotification>();
                var biddingList = new List<IndividualRequisitionBidding>();
                List<Guid> FK_FirebaseAppUser = new List<Guid>();
                string NumbersToSMS = "";
                if (CurrentUser.FK_Depo != null)
                {
                    var ownDepotOtherAgent = bll.db.AppUsers.Where(m => m.IsDeleted == false && m.FK_Depo == CurrentUser.FK_Depo && m.PK_User != CurrentUser.PK_User && (m.AppUserType == "Internal Transport Agent" || m.AppUserType == "External Transport Agent")).ToList();
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
                            Category = RequistionNotificationCategory.Individual_Requisition_Created
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
                    var otherDeposAgents = bll.db.AppUsers.Where(m => m.IsDeleted == false && m.FK_Depo == depo.FK_Depo && m.PK_User != CurrentUser.PK_User && (m.AppUserType == "Internal Transport Agent" || m.AppUserType == "External Transport Agent")).ToList();
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

                //# notification for other depo's agents
                string FK_Depoes = form["FK_Depoes"];
                if (FK_Depoes != null)
                {
                    var FK_Depo_list = FK_Depoes.Split(',');
                    if (FK_Depo_list.Count() > 0)
                    {
                        foreach (var FK in FK_Depo_list)
                        {
                            if (FK == "")
                            {
                                break;
                            }
                            var _FK = Guid.Parse(FK);
                            var otherDeposAgents = bll.db.AppUsers.Where(m => m.IsDeleted == false && m.PK_User != CurrentUser.PK_User && m.FK_Depo == _FK && (m.AppUserType == "Internal Transport Agent" || m.AppUserType == "External Transport Agent")).ToList();
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
                    }
                }
                bll.db.IndividualRequisitionBiddings.AddRange(biddingList);
                bll.db.RequisitionAgentNotifications.AddRange(notificatoinList);
                bll.db.SaveChanges();

                //# SMS
                NumbersToSMS = form["NumbersToSMS"];
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

                CreateAlertMessage(AlertMessageType.Success, "Success", "Individual Requisition is successfully added.");
                return RedirectToAction("IndexBy_Client");
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
            }
            ViewBag.VehicleTypesDict = new SelectList(bll.db.RequisitionVehicleTypes.Select(m => new { Key = m.Layer1 + "#" + m.Layer2 + "#" + m.Layer3, Value = m.Layer1_english + " : " + m.Layer2_english + " : " + m.Layer3_english }), "Key", "Value", new { Key = individualRequisition.VehicleTypeLayer1 + "#" + individualRequisition.VehicleTypeLayer2 + "#" + individualRequisition.VehicleTypeLayer3 });
            var requisitionAgentProposedDepoes = bll.db.RequisitionAgentProposedDepoes.Where(m => m.FK_RequisitionAgent == CurrentUser.PK_User && m.WillPropose == true).Select(m => m.FK_Depo).ToList();
            var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
            ViewBag.OtherDepoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && m.PK_Depo != _invalidDepoPK && !(requisitionAgentProposedDepoes.Contains(m.PK_Depo))).OrderBy(m => m.Name), "PK_Depo", "Name");
            ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && m.PK_Depo != _invalidDepoPK).OrderBy(m => m.Name), "PK_Depo", "Name");
            return View(individualRequisition);
        }

        public ActionResult ViewForClient_ToApprove(Guid PK_IndividualRequisition)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            if (PK_IndividualRequisition == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var model = bll.db.IndividualRequisitions.Find(PK_IndividualRequisition);
                if (model != null)
                {
                    var vm_IndividualRequisition = bll.ConvertToViewModel(model);
                    //list of biddings

                    return View(vm_IndividualRequisition);

                }
                else
                {
                    return HttpNotFound();
                }
            }
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
        public ActionResult ViewForClient_ToApprove(FormCollection form)
        {

            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            try
            {
                Guid PK_IndividualRequisition = Guid.Parse(form["PK_IndividualRequisition"]);
                var individualRequisition = bll.db.IndividualRequisitions.Where(m => m.PK_IndividualRequisition == PK_IndividualRequisition).FirstOrDefault();
                individualRequisition.Status = 1;
                int i = 0;
                while (true)
                {
                    var bookingID = form["bookingID_" + i];
                    if (bookingID == null)
                    {
                        break;
                    }
                    else
                    {
                        var bookingQuantity = form["bookingQuantity_" + i];
                        if (bookingQuantity != "" && bookingQuantity != "0")
                        {
                            var _PK_IndividualRequisitionBidding = Guid.Parse(bookingID);
                            var bid = bll.db.IndividualRequisitionBiddings.Where(m => m.PK_IndividualRequisitionBidding == _PK_IndividualRequisitionBidding).FirstOrDefault();
                            bid.Status = 2;
                            bid.ApprovedQuantity = Convert.ToInt32(bookingQuantity);
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
                                var sms_response = SendSMS(_agent.ContactNumber, messageBody);
                            }

                            //# Firebase notifier
                            var Title = "Bidding is approved";
                            var SubTitle = "Agent- " + CurrentUser.FullName + " Approved " + bid.ApprovedQuantity + " " + individualRequisition.VehicleTypeLayer1_english + ":" + individualRequisition.VehicleTypeLayer2_english + ":" + individualRequisition.VehicleTypeLayer3_english + ". " + "\ncall for detail:" + CurrentUser.ContactNumber;
                            var Category = RequistionNotificationCategory.Individual_Requisition_Approved;
                            var fcm_response = SendFCM_Notification_Single(_agent.PK_User, Category, Title, SubTitle);
                        }
                    }
                    i++;
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
                CreateAlertMessage(AlertMessageType.Success, "Success", "Individual Requisition is successfully approved.");
                return Redirect("/IndividualRequisition/IndexBy_Client");
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                return Redirect("/Home/Index");
            }
        }

        public ActionResult ViewForClient_Approved(Guid PK_IndividualRequisition)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            if (PK_IndividualRequisition == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var model = bll.db.IndividualRequisitions.Find(PK_IndividualRequisition);
                if (model != null)
                {
                    var vm_IndividualRequisition = bll.ConvertToViewModel(model);
                    //list of biddings

                    return View(vm_IndividualRequisition);

                }
                else
                {
                    return HttpNotFound();
                }
            }
        }




        //#MakeBidderRating
        public ActionResult MakeBidderRating(FormCollection form)
        {
            var id = Guid.Parse(form["PK_IndividualRequisitionBidding"]);
            var model = bll.db.IndividualRequisitionBiddings.Where(m => m.PK_IndividualRequisitionBidding == id).FirstOrDefault();
            if (model != null)
            {
                try
                {
                    model.Status = 4;
                    model.BidderRating = Convert.ToInt32(form["BidderRating"]);
                    model.BidderRatingNote = form["BidderRatingNote"];
                    bll.db.SaveChanges();
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Rating Added.");
                    return RedirectToAction("IndexBy_Client");
                }
                catch (Exception exception)
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                    return RedirectToAction("IndexBy_Client");
                }
            }
            else
            {
                return HttpNotFound();
            }
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

        //#Excell Upload
        public ActionResult CreateWithExcelFile()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            return View();
        }
        [HttpPost]
        public ActionResult CreateWithExcelFile(HttpPostedFileBase excelFile)//excellFile
        {
            if ((excelFile.ContentLength != 0) && (excelFile.FileName.EndsWith("xls") || excelFile.FileName.EndsWith("xlsx")))
            {
                //string path = Server.MapPath("~/TempFile/" + Guid.NewGuid() + excelFile.FileName.Split('.').Last());
                string path = Server.MapPath("~/TempFile/" + excelFile.FileName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                excelFile.SaveAs(path);
                Microsoft.Office.Interop.Excel.Application application = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook workbook = application.Workbooks.Open(path);
                Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.ActiveSheet;
                Microsoft.Office.Interop.Excel.Range range = worksheet.UsedRange;
                //var validation = true;
                var errorMessage = "";
                for (int i = 2; i < range.Rows.Count + 1; i++)
                {


                    if (range.Cells[i, 1].Text == "" && range.Cells[i, 2].Text == "")
                    {
                        errorMessage = errorMessage + "Row no:" + i + " No From Depo/ From Location found" + "\n";
                    }
                    if (range.Cells[i, 1].Text == "" && range.Cells[i, 1].Text.Text == "")
                    {
                        string val = range.Cells[i, 1].Text;
                        var hasAny = bll.db.Depoes.Where(m => m.Name == val).Any();
                        if (!hasAny)
                        {
                            errorMessage = errorMessage + "Row no:" + i + " " + val + " No From Depo Found by this name" + "\n";
                        }
                    }

                    if (range.Cells[i, 3].Text == "" && range.Cells[i, 4].Text == "")
                    {
                        errorMessage = errorMessage + "Row no:" + i + " No To Depo/ To Location found" + "\n";
                    }
                    if (range.Cells[i, 3].Text == "" && range.Cells[i, 3].Text.Text == "")
                    {
                        string val = range.Cells[i, 3].Text;
                        var hasAny = bll.db.Depoes.Where(m => m.Name == val).Any();
                        if (!hasAny)
                        {
                            errorMessage = errorMessage + "Row no:" + i + " " + val + " No To Depo Found by this name" + "\n";
                        }
                    }

                    if (range.Cells[i, 5].Text == "" && range.Cells[i, 5].Text.Text == "")
                    {
                        errorMessage = errorMessage + "Row no:" + i + " No Vehicle Type Found" + "\n";
                    }
                    if (range.Cells[i, 5].Text == "" && range.Cells[i, 5].Text.Text == "")
                    {
                        string val = range.Cells[i, 5].Text;
                        string val_1 = val.Split('/')[0];
                        string val_2 = val.Split('/')[1];
                        string val_3 = val.Split('/')[2];
                        var hasAny = bll.db.RequisitionVehicleTypes.Where(m => m.Layer1_english == val_1 && m.Layer2_english == val_2 && m.Layer3_english == val_3).Any();

                        if (!hasAny)
                        {
                            errorMessage = errorMessage + "Row no:" + i + " No Valid Vehicle Type Found" + "\n";
                        }
                    }

                    if (range.Cells[i, 6].Text == "" && range.Cells[i, 6].Text.Text == "")
                    {
                        errorMessage = errorMessage + "Row no:" + i + " No Date-time Found" + "\n";
                    }
                    if (range.Cells[i, 6].Text == "" && range.Cells[i, 6].Text.Text == "")
                    {
                        string val = range.Cells[i, 6].Text;
                        try
                        {
                            DateTime _temp = DateTime.ParseExact(val, "dd-MM-yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                        }
                        catch (Exception)
                        {
                            errorMessage = errorMessage + "Row no:" + i + " Invalid datetime format" + "\n";
                        }
                    }

                    if (range.Cells[i, 7].Text == "" && range.Cells[i, 7].Text.Text == "")
                    {
                        errorMessage = errorMessage + "Row no:" + i + " No Wanted Count Found" + "\n";
                    }
                    if (range.Cells[i, 7].Text == "" && range.Cells[i, 7].Text.Text == "")
                    {
                        string val = range.Cells[i, 7].Text;
                        try
                        {
                            int _temp = Convert.ToInt32(val);
                        }
                        catch (Exception)
                        {
                            errorMessage = errorMessage + "Row no:" + i + " Invalid Wanted Count format" + "\n";
                        }
                    }
                }
                ViewBag.errorMessage = errorMessage;
                workbook.Close();
                application.Quit();
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            return View();
        }
    }
}