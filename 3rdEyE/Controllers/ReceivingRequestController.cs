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

namespace _3rdEyE.Controllers
{
    public class ReceivingRequestController : BaseController
    {
        List<string> ReceivingRequestStatus = new List<string> { "Open", "Close" };
        List<string> CarrerTypes = new List<string> { "Man", "CNG", "Manual Van", "Other" };
        public ActionResult Index(DateTime? StartingDate, DateTime? EndingDate, String TrackingId, string FK_Location, String Status)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var list = new List<Models.ReceivingRequest>();
            var now = DateTime.Now;
            var query = bll.db.ReceivingRequests.AsQueryable();
            if (StartingDate != null)
            {
                var _StartingDate = StartingDate != null ? StartingDate : new DateTime();
                query = query.Where(m => m.AssumedGateReceivingDateTime > _StartingDate);
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", _StartingDate);
            }
            else
            {
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", now.Date);
            }
            if (EndingDate != null)
            {
                var _EndingDate = EndingDate != null ? EndingDate : new DateTime();
                query = query.Where(m => m.AssumedGateReceivingDateTime < _EndingDate);
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
            if (!string.IsNullOrEmpty(FK_Location))
            {
                var _FK_Location = Guid.Parse(FK_Location);
                query = query.Where(m => m.LocationDepartment.FK_Location == _FK_Location);
                ViewBag.Locations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false && (m.LocationType == "Depo" || m.LocationType == "Factory" || m.LocationType == "Office" || m.LocationType == "Workshop")).OrderBy(m => m.Name), "PK_Location", "Name", FK_Location);
            }
            else
            {
                ViewBag.Locations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false && (m.LocationType == "Depo" || m.LocationType == "Factory" || m.LocationType == "Office" || m.LocationType == "Workshop")).OrderBy(m => m.Name), "PK_Location", "Name");
            }
            if (!string.IsNullOrEmpty(Status))
            {
                query = query.Where(m => m.Status == Status);
                ViewBag.Statuses = new SelectList(ReceivingRequestStatus, Status);
            }
            else
            {
                ViewBag.Statuses = new SelectList(ReceivingRequestStatus, Status);
            }
            if (StartingDate != null || EndingDate != null || (!string.IsNullOrEmpty(TrackingId)) || FK_Location != null || (!string.IsNullOrEmpty(Status)))
            {
                list = query.AsQueryable().ToList();
            }
            return View(list);
        }

        public ActionResult Index_Client(DateTime? StartingDate, DateTime? EndingDate, String TrackingId, string FK_LocationDepartment, String Status)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var list = new List<Models.ReceivingRequest>();
            var now = DateTime.Now;
            var query = bll.db.ReceivingRequests.AsQueryable();
            if (StartingDate != null)
            {
                var _StartingDate = StartingDate != null ? StartingDate : new DateTime();
                query = query.Where(m => m.AssumedGateReceivingDateTime > _StartingDate);
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", _StartingDate);
            }
            else
            {
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", now.Date);
            }
            if (EndingDate != null)
            {
                var _EndingDate = EndingDate != null ? EndingDate : new DateTime();
                query = query.Where(m => m.AssumedGateReceivingDateTime < _EndingDate);
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
            if (!string.IsNullOrEmpty(FK_LocationDepartment))
            {
                var _FK_LocationDepartment = Convert.ToInt64(FK_LocationDepartment);
                query = query.Where(m => m.FK_LocationDepartment == _FK_LocationDepartment);
                ViewBag.LocationDepartments = new SelectList(bll.db.LocationDepartments.OrderBy(m => m.DepartmentCode), "PK_LocationDepartment", "DepartmentCode", FK_LocationDepartment);
            }
            else
            {
                ViewBag.LocationDepartments = new SelectList(bll.db.LocationDepartments.OrderBy(m => m.DepartmentCode), "PK_LocationDepartment", "DepartmentCode");
            }
            if (!string.IsNullOrEmpty(Status))
            {
                query = query.Where(m => m.Status == Status);
                ViewBag.Statuses = new SelectList(ReceivingRequestStatus, Status);
            }
            else
            {
                ViewBag.Statuses = new SelectList(ReceivingRequestStatus, Status);
            }
            if (StartingDate != null || EndingDate != null || (!string.IsNullOrEmpty(TrackingId)) || FK_LocationDepartment != null || (!string.IsNullOrEmpty(Status)))
            {
                list = query.AsQueryable().ToList();
            }
            return View(list);
        }
        public ActionResult Index_Gate()
        {
            var StartingDate = DateTime.Now.AddDays(-1);
            var departmentList = bll.db.LocationDepartments.Where(m => m.FK_Location == CurrentUser.FK_Location).Select(m => m.PK_LocationDepartment).ToList();
            var list = bll.db.ReceivingRequests.Where(m => m.Status == "Open" && m.AssumedGateReceivingDateTime > StartingDate && departmentList.Contains(m.FK_LocationDepartment)).ToList();
            return View(list);
        }

        public ActionResult Create()
        {
            ViewBag.LocationDepartments = new SelectList(bll.db.LocationDepartments.Where(m => m.FK_Location == CurrentUser.FK_Location && m.PRG_Type == CurrentUser.PRG_Type).OrderBy(m => m.DepartmentCode), "PK_LocationDepartment", "DepartmentCode");
            ViewBag.CarrerTypes = new SelectList(CarrerTypes);
            return View(new ReceivingRequest());
        }
        [HttpPost]
        public ActionResult Create(ReceivingRequest model)
        {
            try
            {
                model.CreatedAt = DateTime.Now;
                model.FK_AppUser_Client = CurrentUser.PK_User;
                model.Status = "Open";

                if (!string.IsNullOrEmpty(model.Item1_Name))
                {
                    model.Item1_Status = "Pending";
                }
                if (!string.IsNullOrEmpty(model.Item2_Name))
                {
                    model.Item2_Status = "Pending";
                }
                if (!string.IsNullOrEmpty(model.Item3_Name))
                {
                    model.Item3_Status = "Pending";
                }
                if (!string.IsNullOrEmpty(model.Item4_Name))
                {
                    model.Item4_Status = "Pending";
                }
                if (!string.IsNullOrEmpty(model.Item5_Name))
                {
                    model.Item5_Status = "Pending";
                }
                bll.db.ReceivingRequests.Add(model);
                bll.db.SaveChanges();
                model.TrackingID = "RR-" + model.PK_ReceivingRequest;
                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "Receiving request is successfully added.");
                return RedirectToAction("Index_Client");
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                ViewBag.LocationDepartments = new SelectList(bll.db.LocationDepartments.Where(m => m.FK_Location == CurrentUser.FK_Location && m.PRG_Type == CurrentUser.PRG_Type).OrderBy(m => m.DepartmentCode), "PK_LocationDepartment", "DepartmentCode", model.FK_LocationDepartment);
                ViewBag.CarrerTypes = new SelectList(CarrerTypes, model.CarrerType);
                return View(model);
            }
        }

        public ActionResult ReceiveByGate(Int64 id)
        {
            var model = bll.db.ReceivingRequests.Find(id);
            if (model == null)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", "Request not found");
                return RedirectToAction("Index_Gate");
            }
            else if (model.Status != "Open")
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", "Request is not in proper status");
                return RedirectToAction("Index_Gate");
            }
            else
            {
                return View(model);
            }
        }
        [HttpPost]
        public ActionResult ReceiveByGate(ReceivingRequest model, FormCollection formCollection)
        {
            try
            {
                var db_model = bll.db.ReceivingRequests.Where(m => m.PK_ReceivingRequest == model.PK_ReceivingRequest).FirstOrDefault();
                if (db_model == null)
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", "Request not found");
                    return RedirectToAction("Index_Gate");
                }
                else if (db_model.Status != "Open")
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", "Request is not in proper status");
                    return RedirectToAction("Index_Gate");
                }
                else
                {

                    if (db_model.Item1_Status == "Pending" && formCollection["Item1_GateConfirmation"] == "Received")
                    {
                        db_model.Item1_GateReceivingDoccumentNumber = formCollection["DoccumentNumber"];
                        db_model.Item1_Status = "Received at gate";

                        db_model.FK_AppUser_ReceivingGate = CurrentUser.PK_User;
                        db_model.GateReceivingDateTime = DateTime.Now;
                    }

                    if (db_model.Item2_Status == "Pending" && formCollection["Item2_GateConfirmation"] == "Received")
                    {
                        db_model.Item2_GateReceivingDoccumentNumber = formCollection["DoccumentNumber"];
                        db_model.Item2_Status = "Received at gate";

                        db_model.FK_AppUser_ReceivingGate = CurrentUser.PK_User;
                        db_model.GateReceivingDateTime = DateTime.Now;
                    }

                    if (db_model.Item3_Status == "Pending" && formCollection["Item3_GateConfirmation"] == "Received")
                    {
                        db_model.Item3_GateReceivingDoccumentNumber = formCollection["DoccumentNumber"];
                        db_model.Item3_Status = "Received at gate";

                        db_model.FK_AppUser_ReceivingGate = CurrentUser.PK_User;
                        db_model.GateReceivingDateTime = DateTime.Now;
                    }

                    if (db_model.Item4_Status == "Pending" && formCollection["Item4_GateConfirmation"] == "Received")
                    {
                        db_model.Item4_GateReceivingDoccumentNumber = formCollection["DoccumentNumber"];
                        db_model.Item4_Status = "Received at gate";

                        db_model.FK_AppUser_ReceivingGate = CurrentUser.PK_User;
                        db_model.GateReceivingDateTime = DateTime.Now;
                    }

                    if (db_model.Item5_Status == "Pending" && formCollection["Item5_GateConfirmation"] == "Received")
                    {
                        db_model.Item5_GateReceivingDoccumentNumber = formCollection["DoccumentNumber"];
                        db_model.Item5_Status = "Received at gate";

                        db_model.FK_AppUser_ReceivingGate = CurrentUser.PK_User;
                        db_model.GateReceivingDateTime = DateTime.Now;
                    }

                    bll.db.SaveChanges();
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Request succesfully updated.");
                    return RedirectToAction("Index_Gate");
                }
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                return RedirectToAction("Index_Gate");
            }
        }

        public ActionResult ReceiveByClient(Int64 id)
        {
            var model = bll.db.ReceivingRequests.Find(id);
            if (model == null)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", "Request not found");
                return RedirectToAction("Index_Client");
            }
            else if (model.Status != "Open")
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", "Request is not in proper status");
                return RedirectToAction("Index_Client");
            }
            else
            {
                return View(model);
            }
        }
        [HttpPost]
        public ActionResult ReceiveByClient(ReceivingRequest model, FormCollection formCollection)
        {
            try
            {
                var db_model = bll.db.ReceivingRequests.Where(m => m.PK_ReceivingRequest == model.PK_ReceivingRequest).FirstOrDefault();
                if (db_model == null)
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", "Request not found");
                    return RedirectToAction("Index_Client");
                }
                else if (db_model.Status != "Open")
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", "Request is not in proper status");
                    return RedirectToAction("Index_Client");
                }
                else
                {

                    if (db_model.Item1_Status == "Received at gate" && !string.IsNullOrEmpty(formCollection["Item1_ClientConfirmation"]))
                    {
                        if (formCollection["Item1_ClientConfirmation"] == "Accept")
                        {
                            db_model.Item1_Status = "Accepted by client";
                        }
                        else if (formCollection["Item1_ClientConfirmation"] == "Reject")
                        {
                            db_model.Item1_Status = "Rejected by client";
                        }
                        db_model.Item1_Note_FinalReceival = formCollection["Item1_Note_FinalReceival"];
                    }

                    if (db_model.Item2_Status == "Received at gate" && !string.IsNullOrEmpty(formCollection["Item2_ClientConfirmation"]))
                    {
                        if (formCollection["Item2_ClientConfirmation"] == "Accept")
                        {
                            db_model.Item2_Status = "Accepted by client";
                        }
                        else if (formCollection["Item2_ClientConfirmation"] == "Reject")
                        {
                            db_model.Item2_Status = "Rejected by client";
                        }
                        db_model.Item2_Note_FinalReceival = formCollection["Item2_Note_FinalReceival"];
                    }

                    if (db_model.Item3_Status == "Received at gate" && !string.IsNullOrEmpty(formCollection["Item3_ClientConfirmation"]))
                    {
                        if (formCollection["Item3_ClientConfirmation"] == "Accept")
                        {
                            db_model.Item3_Status = "Accepted by client";
                        }
                        else if (formCollection["Item3_ClientConfirmation"] == "Reject")
                        {
                            db_model.Item3_Status = "Rejected by client";
                        }
                        db_model.Item3_Note_FinalReceival = formCollection["Item3_Note_FinalReceival"];
                    }

                    if (db_model.Item4_Status == "Received at gate" && !string.IsNullOrEmpty(formCollection["Item4_ClientConfirmation"]))
                    {
                        if (formCollection["Item4_ClientConfirmation"] == "Accept")
                        {
                            db_model.Item4_Status = "Accepted by client";
                        }
                        else if (formCollection["Item4_ClientConfirmation"] == "Reject")
                        {
                            db_model.Item4_Status = "Rejected by client";
                        }
                        db_model.Item4_Note_FinalReceival = formCollection["Item4_Note_FinalReceival"];
                    }

                    if (db_model.Item5_Status == "Received at gate" && !string.IsNullOrEmpty(formCollection["Item5_ClientConfirmation"]))
                    {
                        if (formCollection["Item5_ClientConfirmation"] == "Accept")
                        {
                            db_model.Item5_Status = "Accepted by client";
                        }
                        else if (formCollection["Item5_ClientConfirmation"] == "Reject")
                        {
                            db_model.Item5_Status = "Rejected by client";
                        }
                        db_model.Item5_Note_FinalReceival = formCollection["Item5_Note_FinalReceival"];
                    }

                    if (
                        (db_model.Item1_Status == null || db_model.Item1_Status == "Accepted by client") &&
                        (db_model.Item2_Status == null || db_model.Item2_Status == "Accepted by client") &&
                        (db_model.Item3_Status == null || db_model.Item3_Status == "Accepted by client") &&
                        (db_model.Item4_Status == null || db_model.Item4_Status == "Accepted by client") &&
                        (db_model.Item5_Status == null || db_model.Item5_Status == "Accepted by client")
                        )
                    {
                        db_model.Status = "Close";
                    }
                    bll.db.SaveChanges();
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Request succesfully updated.");
                    return RedirectToAction("Index_Client");
                }
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                return RedirectToAction("Index_Client");
            }
        }

        public ActionResult ConfirmGateOutForRejectedItem(Int64 PK_ReceivingRequest, int Item_id)
        {
            try
            {
                var db_model = bll.db.ReceivingRequests.Where(m => m.PK_ReceivingRequest == PK_ReceivingRequest).FirstOrDefault();
                if (Item_id == 1 && db_model.Item1_Status == "Rejected by client")
                {
                    db_model.Item1_Status = "Rejected by client & gate out";
                }
                if (Item_id == 2 && db_model.Item2_Status == "Rejected by client")
                {
                    db_model.Item2_Status = "Rejected by client & gate out";
                }
                if (Item_id == 3 && db_model.Item3_Status == "Rejected by client")
                {
                    db_model.Item3_Status = "Rejected by client & gate out";
                }
                if (Item_id == 4 && db_model.Item4_Status == "Rejected by client")
                {
                    db_model.Item4_Status = "Rejected by client & gate out";
                }
                if (Item_id == 5 && db_model.Item5_Status == "Rejected by client")
                {
                    db_model.Item5_Status = "Rejected by client & gate out";
                }

                if (
                        (db_model.Item1_Status == null || db_model.Item1_Status == "Accepted by client" || db_model.Item1_Status == "Rejected by client & gate out") &&
                        (db_model.Item2_Status == null || db_model.Item2_Status == "Accepted by client" || db_model.Item2_Status == "Rejected by client & gate out") &&
                        (db_model.Item3_Status == null || db_model.Item3_Status == "Accepted by client" || db_model.Item3_Status == "Rejected by client & gate out") &&
                        (db_model.Item4_Status == null || db_model.Item4_Status == "Accepted by client" || db_model.Item4_Status == "Rejected by client & gate out") &&
                        (db_model.Item5_Status == null || db_model.Item5_Status == "Accepted by client" || db_model.Item5_Status == "Rejected by client & gate out")
                        )
                {
                    db_model.Status = "Close";
                }
                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "Request succesfully updated.");
                return RedirectToAction("Index_Gate");
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                return RedirectToAction("Index_Gate");
            }
        }

    }
}