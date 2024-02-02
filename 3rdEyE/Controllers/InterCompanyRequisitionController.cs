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
    public class InterCompanyRequisitionController : BaseController
    {
        BLL_InterCompanyRequisition bll = new BLL_InterCompanyRequisition();
        //Dictionary<string, string> VehicleTypesDict = new Dictionary<string, string> { { "Ambulance", "Ambulance" }, { "Bus", "Bus" }, { "Cargo Truck", "Cargo Truck" }, { "Cargo Truck - Open", "Cargo Truck - Open" }, { "Cargo VAN", "Cargo VAN" }, { "Open VAN", "Open VAN" }, { "Concrete Mixer", "Concrete Mixer" }, { "Covered Van", "Covered Van" }, { "Delivery Van", "Delivery Van" }, { "Milk Tanker", "Milk Tanker" }, { "Mini Bus", "Mini Bus" }, { "Mini Truck", "Mini Truck" }, { "Mobile Crance", "Mobile Crance" }, { "Motor Car", "Motor Car" }, { "OMNI Bus", "OMNI Bus" }, { "Pickup", "Pickup" }, { "Refrigerator Van", "Refrigerator Van" }, { "Tank Lorry", "Tank Lorry" }, { "Tipper", "Tipper" }, { "Trailers", "Trailers" }, { "Water Tanker", "Water Tanker" } };
        //Dictionary<double, string> VehicleCapacityDict = new Dictionary<double, string> { { 0.8, "0.8 ton" }, { 1, "1 ton" }, { 1.5, "1.5 tons" }, { 2, "2 tons" }, { 3, "3 tons" }, { 5, "5 tons" }, { 7, "7 tons" }, { 12, "12 tons" }, { 20, "20 tons" } };

        public ActionResult InterCompanyRequisitionDashBoard()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            return View();
        }
        public JsonResult GetInterCompanyRequisitionDashBoardData()
        {
            var today = DateTime.Now.Date.AddHours(7);
            var yseterday = DateTime.Now.AddDays(-1).Date.AddHours(7);

            var YesterDayDetailData = (bll.db.InterCompanyRequisitions.Where(m => m.CreatedAt > yseterday && m.CreatedAt < today).GroupBy(m => m.AppUser.Depo.Name).Select(m => new
            {
                DepoName = m.Key,
                WantedCount = m.Select(n => n.WantedCount).Sum(),
                AcceptedCount = m.Where(n => n.AcceptedCount != null).Any() ? m.Select(n => n.AcceptedCount).Sum() : 0,
                AssignedInternal = m.SelectMany(n => n.InterCompanyRequisition_InternalVehicle).Count(),
                AssignedExternal = m.SelectMany(n => n.InterCompanyRequisition_ExternalVehicle).Count() + m.SelectMany(n => n.InterCompanyRequisition_ExternalTroller).Count()
            })).ToList();
            var YesterDaySummuryData = new
            {
                AcceptedCount = YesterDayDetailData.Select(m => m.AcceptedCount).Sum(),
                AssignedInternal = YesterDayDetailData.Select(m => m.AssignedInternal).Sum(),
                AssignedExternal = YesterDayDetailData.Select(m => m.AssignedExternal).Sum()
            };
            var lastMonthFirstDay = new DateTime();
            if (today.Month != 1)
            {
                lastMonthFirstDay = new DateTime(today.Year, today.Month - 1, 1);
            }
            else // new year's January month
            {
                lastMonthFirstDay = new DateTime(today.Year - 1, 12, 1);
            }

            var lastMonthLastDay = lastMonthFirstDay.AddMonths(1);
            //var lastMonthDetailData = (bll.db.InterCompanyRequisitions.Where(m => m.CreatedAt > lastMonthFirstDay && m.CreatedAt < lastMonthLastDay).GroupBy(m => m.AppUser.Depo.Name).Select(m => new
            //{
            //    DepoName = m.Key,
            //    WantedCount = m.Select(n => n.WantedCount).Sum(),
            //    AcceptedCount = m.Where(n => n.AcceptedCount != null).Any() ? m.Select(n => n.AcceptedCount).Sum() : 0,
            //    AssignedInternal = m.SelectMany(n => n.InterCompanyRequisition_InternalVehicle).Count(),
            //    AssignedExternal = m.SelectMany(n => n.InterCompanyRequisition_ExternalVehicle).Count() + m.SelectMany(n => n.InterCompanyRequisition_ExternalTroller).Count()
            //})).ToList();
            var LastMonthDetailData = (bll.db.InterCompanyRequisitions.GroupBy(m => m.AppUser.Depo.Name).Select(m => new
            {
                DepoName = m.Key,
                WantedCount = m.Select(n => n.WantedCount).Sum(),
                AcceptedCount = m.Where(n => n.AcceptedCount != null).Any() ? m.Select(n => n.AcceptedCount).Sum() : 0,
                AssignedInternal = m.SelectMany(n => n.InterCompanyRequisition_InternalVehicle).Count(),
                AssignedExternal = m.SelectMany(n => n.InterCompanyRequisition_ExternalVehicle).Count() + m.SelectMany(n => n.InterCompanyRequisition_ExternalTroller).Count()
            })).ToList();
            var LastMonthSummuryData = new
            {
                AcceptedCount = LastMonthDetailData.Select(m => m.AcceptedCount).Sum(),
                AssignedInternal = LastMonthDetailData.Select(m => m.AssignedInternal).Sum(),
                AssignedExternal = LastMonthDetailData.Select(m => m.AssignedExternal).Sum()
            };
            return Json(
                new
                {
                    YesterDayDetailData,
                    YesterDaySummuryData,
                    LastMonthSummuryData
                }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InterCompanyRequisitionDashBoard_ForMgt()
        {
            return View();
        }
        public JsonResult GetInterCompanyRequisitionDashBoardData_ForMgt()
        {
            var today = DateTime.Now.Date.AddHours(7);
            var yseterday = DateTime.Now.AddDays(-1).Date.AddHours(7);

            var YesterDayDetailData = (bll.db.InterCompanyRequisitions.Where(m => m.CreatedAt > yseterday && m.CreatedAt < today).GroupBy(m => m.AppUser.Depo.Name).Select(m => new
            {
                DepoName = m.Key,
                WantedCount = m.Select(n => n.WantedCount).Sum(),
                AcceptedCount = m.Where(n => n.AcceptedCount != null).Any() ? m.Select(n => n.AcceptedCount).Sum() : 0,
                AssignedInternal = m.SelectMany(n => n.InterCompanyRequisition_InternalVehicle).Count(),
                AssignedExternal = m.SelectMany(n => n.InterCompanyRequisition_ExternalVehicle).Count() + m.SelectMany(n => n.InterCompanyRequisition_ExternalTroller).Count()
            })).ToList();
            return Json(
                new
                {
                    YesterDayDetailData
                }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IndexBy_Client(DateTime? StartingDate, DateTime? EndingDate)
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
                var list = bll.db.InterCompanyRequisitions.AsEnumerable().Where(m => m.IsDeleted != true && m.FK_AppUser_Client == CurrentUser.PK_User && m.CreatedAt >= _StartingDate && m.CreatedAt <= _EndingDate).OrderByDescending(m => m.CreatedAt).Select(c => bll.ConvertToViewModel(c)).ToList();
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
                var list = new List<ViewModels.VM_InterCompanyRequisition>();
                return View(list);
            }
        }
        public ActionResult IndexBy_Approver(DateTime? StartingDate, DateTime? EndingDate, Guid? FK_AppUser_Client, Guid? FK_FromLocation, Guid? FK_ToLocation)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var list = new List<ViewModels.VM_InterCompanyRequisition>();
            var now = DateTime.Now;
            var today7 = DateTime.Now.Date.AddHours(7);
            var query = bll.db.InterCompanyRequisitions.AsEnumerable().Where(m => m.IsDeleted != true);
            if (StartingDate != null)
            {
                var _StartingDate = StartingDate != null ? StartingDate : new DateTime();
                query = query.Where(m => m.CreatedAt >= _StartingDate);
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
                query = query.Where(m => m.CreatedAt <= _EndingDate);
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
                query = query.Where(m => m.FK_AppUser_Client == FK_AppUser_Client);
                var _cliesnts = bll.db.AppUsers.Where(m => m.IsDeleted != true && m.AppUserSubType.Contains("RAISER")).Select(m => new { m.PK_User, Text = m.FullName + ":" + m.Depo.Name }).ToList();
                ViewBag.Clients = new SelectList(_cliesnts, "PK_User", "Text", FK_AppUser_Client);
            }
            else
            {
                var _cliesnts = bll.db.AppUsers.Where(m => m.IsDeleted != true && m.AppUserSubType.Contains("RAISER")).Select(m => new { m.PK_User, Text = m.FullName + ":" + m.Depo.Name }).ToList();
                ViewBag.Clients = new SelectList(_cliesnts, "PK_User", "Text");
            }
            if (FK_FromLocation != null)
            {
                query = query.Where(m => m.FK_InterCompanyRequisitionLocation_From == FK_FromLocation);
                ViewBag.FromLocations = new SelectList(bll.db.InterCompanyRequisitionLocations.Where(m => m.IsDeleted != true), "PK_InterCompanyRequisitionLocation", "Name", FK_FromLocation);
            }
            else
            {
                ViewBag.FromLocations = new SelectList(bll.db.InterCompanyRequisitionLocations.Where(m => m.IsDeleted != true), "PK_InterCompanyRequisitionLocation", "Name");
            }
            if (FK_ToLocation != null)
            {
                query = query.Where(m => m.FK_InterCompanyRequisitionLocation_To == FK_ToLocation);
                ViewBag.ToLocations = new SelectList(bll.db.InterCompanyRequisitionLocations.Where(m => m.IsDeleted != true), "PK_InterCompanyRequisitionLocation", "Name", FK_ToLocation);
            }
            else
            {
                ViewBag.ToLocations = new SelectList(bll.db.InterCompanyRequisitionLocations.Where(m => m.IsDeleted != true), "PK_InterCompanyRequisitionLocation", "Name");
            }
            if (StartingDate != null || EndingDate != null || FK_AppUser_Client != null || FK_FromLocation != null || FK_ToLocation != null)
            {
                list = query.OrderByDescending(m => m.CreatedAt).Select(m => bll.ConvertToViewModel(m)).ToList();
            }
            return View(list);
        }
        public ActionResult _ExternalTripIndex(DateTime? StartingDate, DateTime? EndingDate)
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
                var list = bll.db.InterCompanyRequisition_ExternalVehicle.AsEnumerable().Where(c => c.CreatedAt >= StartingDate && c.CreatedAt <= EndingDate.Value.AddDays(1)).OrderByDescending(m => m.CreatedAt).ToList();
                return View(list);
            }
            else
            {
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", DateTime.Today.Date);
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", DateTime.Today.AddDays(1).Date);
                var list = new List<InterCompanyRequisition_ExternalVehicle>();
                return View(list);
            }
        }
        public ActionResult Create()
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
            //var _approversPKs = bll.db.RequisitionAgentProposedDepoes.Where(m=>m.WillPropose == true && m.FK_Depo == CurrentUser.FK_Depo).Select(m=>m.FK_RequisitionAgent).ToList();
            //var Approvers = bll.db.AppUsers.Where(m => (m.FK_Depo == CurrentUser.FK_Depo || _approversPKs.Contains(m.PK_User)) && m.AppUserSubType == userSubType).Select(m => new { key = m.PK_User, value = m.Depo.Name + ":" + m.AppUserSubType.Replace(" APPROVER", "") + ":" + m.FullName }).ToList();
            //ViewBag.Approvers = new SelectList(Approvers, "key", "value");
            ViewBag.RequisitionVehicleTypes = new SelectList(bll.db.RequisitionVehicleTypes.Where(m => m.IsDeleted != true), "PK_RequisitionVehicleType", "Title_English");
            ViewBag.Locations1 = new SelectList(bll.db.InterCompanyRequisitionLocations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_InterCompanyRequisitionLocation", "Name");
            ViewBag.Locations2 = new SelectList(bll.db.InterCompanyRequisitionLocations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_InterCompanyRequisitionLocation", "Name");
            var _today = DateTime.Now.Date;
            ViewBag.AlreadyOpenedCount = bll.db.InterCompanyRequisitions.Where(m => m.FK_AppUser_Client == CurrentUser.PK_User && m.CreatedAt > _today).Count();
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection form)
        {
            InterCompanyRequisition model = new InterCompanyRequisition();
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            try
            {
                model = new InterCompanyRequisition();
                model.PK_InterCompanyRequisition = Guid.NewGuid();
                model.IsDeleted = false;

                model.CreatedAt = DateTime.Now;
                model.FK_AppUser_Client = CurrentUser.PK_User;
                model.FK_ReferenceDepo = CurrentUser.FK_Depo;
                //model.FK_AppUser_Approver = Guid.Parse(form["FK_AppUser_Approver"]);

                if (!string.IsNullOrEmpty(form["FK_InterCompanyRequisitionLocation_From"]))
                {
                    model.FK_InterCompanyRequisitionLocation_From = Guid.Parse(form["FK_InterCompanyRequisitionLocation_From"]);
                }
                model.StartingLocation = form["StartingLocation"];
                if (!string.IsNullOrEmpty(form["FK_InterCompanyRequisitionLocation_To"]))
                {
                    model.FK_InterCompanyRequisitionLocation_To = Guid.Parse(form["FK_InterCompanyRequisitionLocation_To"]);
                }
                model.FinishingLocation = form["FinishingLocation"];
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
                model.WantedCount = Convert.ToInt32(form["WantedCount"]);
                model.PossibleJourneyStartDateTime = DateTime.ParseExact(form["PossibleJourneyStartDateTime"], "yyyy-MM-dd h:mm tt", CultureInfo.InvariantCulture);
                model.ClientNote = form["ClientNote"];
                model.Status = 0;

                bll.db.InterCompanyRequisitions.Add(model);
                bll.db.SaveChanges();

                CreateAlertMessage(AlertMessageType.Success, "Success", "Inter Company Requisition is successfully added.");

                //var userSubType = CurrentUser.AppUserSubType.Replace("_RAISER", "_APPROVER");
                //var Approvers = bll.db.AppUsers.Where(m => (m.FK_Depo == CurrentUser.FK_Depo || bll.db.RequisitionAgentProposedDepoes.Where(n => n.FK_RequisitionAgent == m.PK_User && n.WillPropose == true).Select(n => n.FK_Depo).ToList().Contains(CurrentUser.FK_Depo)) && m.AppUserSubType == userSubType).Select(m => new { key = m.PK_User, value = m.Depo.Name + ":" + m.AppUserSubType.Replace("_APPROVER", "") + ":" + m.FullName }).ToList();
                //ViewBag.Approvers = new SelectList(Approvers, "key", "value", model.FK_AppUser_Approver);


                ViewBag.RequisitionVehicleTypes = new SelectList(bll.db.RequisitionVehicleTypes.Where(m => m.IsDeleted != true), "PK_RequisitionVehicleType", "Title_English", model.FK_RequisitionVehicleType);
                ViewBag.Locations1 = new SelectList(bll.db.InterCompanyRequisitionLocations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_InterCompanyRequisitionLocation", "Name", model.FK_InterCompanyRequisitionLocation_From);
                ViewBag.Locations2 = new SelectList(bll.db.InterCompanyRequisitionLocations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_InterCompanyRequisitionLocation", "Name", model.FK_InterCompanyRequisitionLocation_To);
                var _today = DateTime.Now.Date;
                ViewBag.AlreadyOpenedCount = bll.db.InterCompanyRequisitions.Where(m => m.FK_AppUser_Client == CurrentUser.PK_User && m.CreatedAt > _today).Count();
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
            ViewBag.Locations1 = new SelectList(bll.db.InterCompanyRequisitionLocations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_InterCompanyRequisitionLocation", "Name", model.FK_InterCompanyRequisitionLocation_From);
            ViewBag.Locations2 = new SelectList(bll.db.InterCompanyRequisitionLocations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_InterCompanyRequisitionLocation", "Name", model.FK_InterCompanyRequisitionLocation_To);
            var __today = DateTime.Now.Date;
            ViewBag.AlreadyOpenedCount = bll.db.InterCompanyRequisitions.Where(m => m.FK_AppUser_Client == CurrentUser.PK_User && m.CreatedAt > __today).Count();
            return View(model);
        }

        public ActionResult Edit(Guid id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.InterCompanyRequisitions.AsEnumerable().Where(m => m.PK_InterCompanyRequisition == id).FirstOrDefault();
            //var userSubType = CurrentUser.AppUserSubType.Replace("_RAISER", "_APPROVER");
            //var Approvers = bll.db.AppUsers.Where(m => (m.FK_Depo == CurrentUser.FK_Depo || bll.db.RequisitionAgentProposedDepoes.Where(n => n.FK_RequisitionAgent == m.PK_User && n.WillPropose == true).Select(n => n.FK_Depo).ToList().Contains(CurrentUser.FK_Depo)) && m.AppUserSubType == userSubType).Select(m => new { key = m.PK_User, value = m.Depo.Name + ":" + m.AppUserSubType.Replace("_APPROVER", "") + ":" + m.FullName }).ToList();
            //ViewBag.Approvers = new SelectList(Approvers, "key", "value", model.FK_AppUser_Approver);
            ViewBag.RequisitionVehicleTypes = new SelectList(bll.db.RequisitionVehicleTypes.Where(m => m.IsDeleted != true), "PK_RequisitionVehicleType", "Title_English", model.FK_RequisitionVehicleType);
            ViewBag.Locations1 = new SelectList(bll.db.InterCompanyRequisitionLocations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_InterCompanyRequisitionLocation", "Name", model.FK_InterCompanyRequisitionLocation_From);
            ViewBag.Locations2 = new SelectList(bll.db.InterCompanyRequisitionLocations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_InterCompanyRequisitionLocation", "Name", model.FK_InterCompanyRequisitionLocation_To);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(FormCollection form)
        {
            var _pk = Guid.Parse(form["PK_InterCompanyRequisition"]);
            InterCompanyRequisition model = bll.db.InterCompanyRequisitions.Where(m => m.PK_InterCompanyRequisition == _pk).FirstOrDefault();
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            try
            {
                if (model.Status == 0)
                {
                    model.UpdatedAt = DateTime.Now;

                    if (!string.IsNullOrEmpty(form["FK_InterCompanyRequisitionLocation_From"]))
                    {
                        model.FK_InterCompanyRequisitionLocation_From = Guid.Parse(form["FK_InterCompanyRequisitionLocation_From"]);
                    }
                    model.StartingLocation = form["StartingLocation"];
                    if (!string.IsNullOrEmpty(form["FK_InterCompanyRequisitionLocation_To"]))
                    {
                        model.FK_InterCompanyRequisitionLocation_To = Guid.Parse(form["FK_InterCompanyRequisitionLocation_To"]);
                    }
                    model.FinishingLocation = form["FinishingLocation"];
                    model.FK_RequisitionVehicleType = Convert.ToInt32(form["FK_RequisitionVehicleType"]);
                    model.WantedCount = Convert.ToInt32(form["WantedCount"]);
                    model.PossibleJourneyStartDateTime = DateTime.ParseExact(form["PossibleJourneyStartDateTime"], "yyyy-MM-dd h:mm tt", CultureInfo.InvariantCulture);
                    model.ClientNote = form["ClientNote"];
                    model.Status = 0;
                    bll.db.SaveChanges();

                    CreateAlertMessage(AlertMessageType.Success, "Success", "Inter Company Requisition is successfully added.");
                    return RedirectToAction("IndexBy_Client");
                }
                else
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", "Inter Company Requisition could not be modified anymore.");
                    return RedirectToAction("IndexBy_Client");
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
            ViewBag.Locations1 = new SelectList(bll.db.InterCompanyRequisitionLocations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_InterCompanyRequisitionLocation", "Name", model.FK_InterCompanyRequisitionLocation_From);
            ViewBag.Locations2 = new SelectList(bll.db.InterCompanyRequisitionLocations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_InterCompanyRequisitionLocation", "Name", model.FK_InterCompanyRequisitionLocation_To);
            return View(model);
        }

        public ActionResult View(Guid id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.InterCompanyRequisitions.AsEnumerable().Where(m => m.PK_InterCompanyRequisition == id).Select(m => bll.ConvertToViewModel(m)).FirstOrDefault();
            return View(model);
        }

        public ActionResult Approve(Guid id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.InterCompanyRequisitions.AsEnumerable().Where(m => m.PK_InterCompanyRequisition == id).FirstOrDefault();
            return View(model);
        }
        [HttpPost]
        public ActionResult Approve(FormCollection form)
        {
            var _pk = Guid.Parse(form["PK_InterCompanyRequisition"]);
            InterCompanyRequisition model = bll.db.InterCompanyRequisitions.Where(m => m.PK_InterCompanyRequisition == _pk).FirstOrDefault();
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
                    model.AcceptedCount = Convert.ToInt32(form["AcceptedCount"]);
                    model.ApproverNote = form["ApproverNote"];
                    bll.db.SaveChanges();
                    //#Mail
                    if (!string.IsNullOrEmpty(model.AppUser.Email))
                    {
                        var Mail_Subject = "Requisition " + (model.Status == 1 ? "Accepted" : "Rejected");
                        var Mail_Body = "Dear Concern<br>";
                        Mail_Body = Mail_Body + "Your requisition is " + (model.Status == 1 ? "approved" : "rejected") + " <br>"
                            + "From: " + (model.InterCompanyRequisitionLocation != null ? model.InterCompanyRequisitionLocation.Name : "") + " " + model.StartingLocation + "<br>"
                            + "To: " + (model.InterCompanyRequisitionLocation1 != null ? model.InterCompanyRequisitionLocation1.Name : "") + " " + model.FinishingLocation + "<br>"
                            + "Wanted quantity: " + model.WantedCount + "<br>"
                            + "On: " + CommonClass.ConvertToDateTimeString(model.PossibleJourneyStartDateTime) + "\n";
                        SendMail_Single(model.AppUser.Email, Mail_Subject, Mail_Body);
                    }

                    //#SMS
                    if (!string.IsNullOrEmpty(model.AppUser.ContactNumber))
                    {
                        var message = "Requisition " + (model.Status == 1 ? "Accepted" : "Rejected") + "\n"
                            + "From: " + (model.InterCompanyRequisitionLocation != null ? model.InterCompanyRequisitionLocation.Name : "") + " " + model.StartingLocation + "\n"
                            + "To: " + (model.InterCompanyRequisitionLocation1 != null ? model.InterCompanyRequisitionLocation1.Name : "") + " " + model.FinishingLocation + "\n"
                            + "Wanted quantity: " + model.WantedCount + "\n"
                            + "Accepted quantity: " + model.AcceptedCount + "\n"
                            + "On: " + CommonClass.ConvertToDateTimeString(model.PossibleJourneyStartDateTime) + "\n";
                        SendSMS(model.AppUser.ContactNumber, message);
                    }
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Inter Company Requisition is successfully Verified.");
                    return RedirectToAction("IndexBy_Approver");
                }
                else
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", "Inter Company Requisition could not be modified anymore.");
                    return RedirectToAction("IndexBy_Approver");
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
                var PK_InterCompanyRequisitions = form["PK_InterCompanyRequisitions"].Split(',');
                for (int i = 0; i < PK_InterCompanyRequisitions.Length; i++)
                {
                    if (string.IsNullOrEmpty(PK_InterCompanyRequisitions[i]))
                    {
                        break;
                    }
                    var _pk = Guid.Parse(PK_InterCompanyRequisitions[i]);
                    InterCompanyRequisition model = bll.db.InterCompanyRequisitions.Where(m => m.PK_InterCompanyRequisition == _pk).FirstOrDefault();
                    if (model.Status == 0)
                    {
                        model.VerifiedAt = DateTime.Now;
                        model.FK_AppUser_Approver = CurrentUser.PK_User;

                        model.Status = 1;
                        model.AcceptedCount = Convert.ToInt32(AcceptedCounts[i]);
                        //model.ApproverNote = form["ApproverNote"];
                        bll.db.SaveChanges();
                        //#Mail
                        if (!string.IsNullOrEmpty(model.AppUser.Email))
                        {
                            var Mail_Subject = "Requisition " + (model.Status == 1 ? "Accepted" : "Rejected");
                            var Mail_Body = "Dear Concern<br>";
                            Mail_Body = Mail_Body + "Your requisition is " + (model.Status == 1 ? "approved" : "rejected") + " <br>"
                                + "From: " + (model.InterCompanyRequisitionLocation != null ? model.InterCompanyRequisitionLocation.Name : "") + " " + model.StartingLocation + "<br>"
                                + "To: " + (model.InterCompanyRequisitionLocation1 != null ? model.InterCompanyRequisitionLocation1.Name : "") + " " + model.FinishingLocation + "<br>"
                                + "Wanted quantity: " + model.WantedCount + "<br>"
                                + "On: " + CommonClass.ConvertToDateTimeString(model.PossibleJourneyStartDateTime) + "\n";
                            SendMail_Single(model.AppUser.Email, Mail_Subject, Mail_Body);
                        }

                        //#SMS
                        if (!string.IsNullOrEmpty(model.AppUser.ContactNumber))
                        {
                            var message = "Requisition " + (model.Status == 1 ? "Accepted" : "Rejected") + "\n"
                                + "From: " + (model.InterCompanyRequisitionLocation != null ? model.InterCompanyRequisitionLocation.Name : "") + " " + model.StartingLocation + "\n"
                                + "To: " + (model.InterCompanyRequisitionLocation1 != null ? model.InterCompanyRequisitionLocation1.Name : "") + " " + model.FinishingLocation + "\n"
                                + "Wanted quantity: " + model.WantedCount + "\n"
                                + "Accepted quantity: " + model.AcceptedCount + "\n"
                                + "On: " + CommonClass.ConvertToDateTimeString(model.PossibleJourneyStartDateTime) + "\n";
                            SendSMS(model.AppUser.ContactNumber, message);
                        }
                    }
                    else
                    {
                        CreateAlertMessage(AlertMessageType.Warning, "Warning", "Inter Company Requisition could not be modified anymore.");
                    }
                }
                CreateAlertMessage(AlertMessageType.Success, "Success", PK_InterCompanyRequisitions.Length + " Inter Company Requisitions is successfully Verified.");
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
            }
            return Redirect(redirectUrl);
        }

        public ActionResult AssignVehicle(Guid id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.InterCompanyRequisitions.AsEnumerable().Where(m => m.PK_InterCompanyRequisition == id).FirstOrDefault();
            return View(model);
        }
        [HttpPost]
        public ActionResult AssignVehicle(FormCollection form)
        {
            var _pk = Guid.Parse(form["PK_InterCompanyRequisition"]);
            InterCompanyRequisition model = bll.db.InterCompanyRequisitions.Where(m => m.PK_InterCompanyRequisition == _pk).FirstOrDefault();
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            try
            {
                if (model.Status == 1 || model.Status == 2)
                {
                    //# InterCompanyRequisition_InternalVehicle
                    var InternalVehiclePKs = form["InternalVehiclePKs"];
                    var InternalVehicle_FKs = InternalVehiclePKs.Split(',');
                    var internalOldVehicle = bll.db.InterCompanyRequisition_InternalVehicle.Where(m => m.FK_InterCompanyRequisition == _pk).ToList();
                    foreach (var item in internalOldVehicle)
                    {
                        if (!InternalVehicle_FKs.Contains(item.FK_Vehicle.ToString()))
                        {
                            bll.db.InterCompanyRequisition_InternalVehicle.Remove(item);
                        }
                        else
                        {
                            item.Note = form["internalVehicleNote_" + item.FK_Vehicle.ToString().Replace("-", "")];
                        }
                    }
                    if (InternalVehicle_FKs.Count() > 0)
                    {
                        foreach (var FK in InternalVehicle_FKs)
                        {
                            if (FK == "")
                            {
                                break;
                            }
                            if (!internalOldVehicle.Where(m => m.FK_Vehicle.ToString() == FK).Any())
                            {
                                var internalVehicle = new InterCompanyRequisition_InternalVehicle();
                                internalVehicle.PK_InterCompanyRequisition_InternalVehicle = Guid.NewGuid();
                                internalVehicle.CreatedAt = DateTime.Now;
                                internalVehicle.FK_Vehicle = Guid.Parse(FK);
                                internalVehicle.FK_InterCompanyRequisition = _pk;
                                internalVehicle.Note = form["internalVehicleNote_" + FK.Replace("-", "")];
                                bll.db.InterCompanyRequisition_InternalVehicle.Add(internalVehicle);
                            }
                        }
                    }

                    //# InterCompanyRequisition_ExternalVehicle
                    var ExternalVehiclePKs = form["ExternalVehiclePKs"];
                    var ExternalVehicle_FKs = ExternalVehiclePKs.Split(',');
                    var externalOldVehicle = bll.db.InterCompanyRequisition_ExternalVehicle.Where(m => m.FK_InterCompanyRequisition == _pk).ToList();
                    foreach (var item in externalOldVehicle)
                    {
                        if (!ExternalVehicle_FKs.Contains(item.FK_Vehicle.ToString()))
                        {
                            bll.db.InterCompanyRequisition_ExternalVehicle.Remove(item);
                        }
                        else
                        {
                            item.Note = form["externalVehicleNote_" + item.FK_Vehicle.ToString().Replace("-", "")];
                            var hiredAmount = form["externalVehicleHiredAmount_" + item.FK_Vehicle.ToString().Replace("-", "")];
                            if (hiredAmount != "")
                            {
                                item.HiredAmount = long.Parse(hiredAmount);
                            }
                        }
                    }
                    if (ExternalVehicle_FKs.Count() > 0)
                    {
                        foreach (var FK in ExternalVehicle_FKs)
                        {
                            if (FK == "")
                            {
                                break;
                            }
                            if (!externalOldVehicle.Where(m => m.FK_Vehicle.ToString() == FK).Any())
                            {
                                var externalVehicle = new InterCompanyRequisition_ExternalVehicle();
                                externalVehicle.PK_InterCompanyRequisition_ExternalVehicle = Guid.NewGuid();
                                externalVehicle.CreatedAt = DateTime.Now;
                                externalVehicle.FK_Vehicle = Guid.Parse(FK);
                                externalVehicle.FK_InterCompanyRequisition = _pk;
                                externalVehicle.Note = form["externalVehicleNote_" + FK.Replace("-", "")];
                                var hiredAmount = form["externalVehicleHiredAmount_" + FK.Replace("-", "")];
                                if (hiredAmount != "")
                                {
                                    externalVehicle.HiredAmount = long.Parse(hiredAmount);
                                }
                                bll.db.InterCompanyRequisition_ExternalVehicle.Add(externalVehicle);
                            }
                        }
                    }

                    //# InterCompanyRequisition_ExternalTroller
                    var externalTrollerCount = Convert.ToInt32(form["externalTrollerCount"]);
                    var ExternalTroller_FKs = new Dictionary<string, string>();
                    var externalOldTroller = bll.db.InterCompanyRequisition_ExternalTroller.Where(m => m.FK_InterCompanyRequisition == _pk).ToList();
                    if (externalTrollerCount > 0)
                    {
                        for (int i = 0; i < externalTrollerCount; i++)
                        {
                            ExternalTroller_FKs.Add(i.ToString(), form["TorllerNumber_" + i]);
                        }
                    }
                    var externalTrollerOld = bll.db.InterCompanyRequisition_ExternalTroller.Where(m => m.FK_InterCompanyRequisition == _pk).ToList();
                    foreach (var item in externalTrollerOld)
                    {
                        if (!ExternalTroller_FKs.Where(m => m.Value == item.TrollerNumber).Any())
                        {
                            bll.db.InterCompanyRequisition_ExternalTroller.Remove(item);
                        }
                        else
                        {
                            item.Note = form["externalTrollerNote_" + ExternalTroller_FKs.Where(m => m.Value == item.TrollerNumber).Select(m => m.Key).FirstOrDefault()];
                        }
                    }
                    if (ExternalTroller_FKs.Count() > 0)
                    {
                        foreach (var FK in ExternalTroller_FKs)
                        {
                            if (!externalOldTroller.Where(m => m.TrollerNumber == FK.Value).Any())
                            {
                                bll.db.InterCompanyRequisition_ExternalTroller.Add(
                                new InterCompanyRequisition_ExternalTroller()
                                {
                                    PK_InterCompanyRequisition_ExternalTroller = Guid.NewGuid(),
                                    CreatedAt = DateTime.Now,
                                    TrollerNumber = FK.Value,
                                    FK_InterCompanyRequisition = _pk,
                                    Note = form["externalTrollerNote_" + FK.Key]
                                });
                            }
                        }
                    }

                    bll.db.SaveChanges();
                    //#Mail
                    //if (!string.IsNullOrEmpty(model.AppUser.Email))
                    //{
                    //    var Mail_Subject = "Vehicle assigned for your requisiton";
                    //    var Mail_Body = "Dear Concern";
                    //    Mail_Body = Mail_Body + "<P> New vehicle assigned for your requisition.</p>"
                    //        + " <p><b>Requisition detail:</b>"
                    //        + " From: " + (model.InterCompanyRequisitionLocation != null ? model.InterCompanyRequisitionLocation.Name : "") + " " + model.StartingLocation
                    //        + " To: " + (model.InterCompanyRequisitionLocation1 != null ? model.InterCompanyRequisitionLocation1.Name : "") + " " + model.FinishingLocation
                    //        + " On: " + CommonClass.ConvertToDateTimeString(model.PossibleJourneyStartDateTime) + "</p>";
                    //    Mail_Body = Mail_Body + "<P>Below vehicles are assigned </P>";
                    //    var hiredVehicles = bll.db.InterCompanyRequisition_InternalVehicle.Where(m => m.FK_InterCompanyRequisition == model.PK_InterCompanyRequisition).OrderByDescending(m => m.CreatedAt).ToList();
                    //    foreach (var item in hiredVehicles)
                    //    {
                    //        var _vehicle = bll.db.Vehicles.Where(m => m.PK_Vehicle == item.FK_Vehicle).FirstOrDefault();
                    //        Mail_Body = Mail_Body + "<p>" + _vehicle.RegistrationNumber + (_vehicle.Internal_VehicleContactNumber != null ? " Contact no: " + _vehicle.Internal_VehicleContactNumber : "") + "</p>";
                    //    }
                    //    Mail_Body = Mail_Body + (model.HiredVehicleCount_External != 0 ? "<p>Externally hired " + model.HiredVehicleCount_External + " vehicle(s) </p>" : "");
                    //    var _pendingCount = model.WantedCount - (model.HiredVehicleCount_External + hiredVehicles.Count());
                    //    if (_pendingCount > 0)
                    //    {
                    //        Mail_Body = Mail_Body + "<p>Pending vehicle(s): " + _pendingCount + "</p>";
                    //    }
                    //    if (!string.IsNullOrEmpty(model.ApproverNote))
                    //    {
                    //        Mail_Body = Mail_Body + " <p><b>Approver Note:</b> " + model.ApproverNote + "</p>";
                    //    }
                    //    SendMail_Single(model.AppUser.Email, Mail_Subject, Mail_Body);
                    //}

                    //#SMS
                    //if (!string.IsNullOrEmpty(model.AppUser.ContactNumber))
                    //{
                    //    var message = "From: " + (model.InterCompanyRequisitionLocation != null ? model.InterCompanyRequisitionLocation.Name : "") + " " + model.StartingLocation + "\n"
                    //        + "To: " + (model.InterCompanyRequisitionLocation1 != null ? model.InterCompanyRequisitionLocation1.Name : "") + " " + model.FinishingLocation + "\n"
                    //        + "On: " + CommonClass.ConvertToDateTimeString(model.PossibleJourneyStartDateTime) + "\n";
                    //    var internalVehicles = bll.db.InterCompanyRequisition_InternalVehicle.Where(m => m.FK_InterCompanyRequisition == model.PK_InterCompanyRequisition).OrderByDescending(m => m.CreatedAt).ToList();
                    //    foreach (var item in internalVehicles)
                    //    {
                    //        var _vehicle = bll.db.Vehicles.Where(m => m.PK_Vehicle == item.FK_Vehicle).FirstOrDefault();
                    //        message = message + _vehicle.RegistrationNumber + (item.Note != null ? "-" + _vehicle.Internal_VehicleContactNumber : "") + " ";
                    //    }
                    //    var externalVehicles = bll.db.InterCompanyRequisition_ExternalVehicle.Where(m => m.FK_InterCompanyRequisition == model.PK_InterCompanyRequisition).OrderByDescending(m => m.CreatedAt).ToList();
                    //    foreach (var item in externalVehicles)
                    //    {
                    //        var _vehicle = bll.db.Vehicles.Where(m => m.PK_Vehicle == item.FK_Vehicle).FirstOrDefault();
                    //        message = message + _vehicle.RegistrationNumber + (item.Note != null ? "-" + _vehicle.Internal_VehicleContactNumber : "") + " ";
                    //    }

                    //    var _pendingCount = model.AcceptedCount - (internalVehicles.Count() + externalVehicles.Count());
                    //    if (_pendingCount > 0)
                    //    {
                    //        message = message + "Pending vehicle(s): " + _pendingCount + "";
                    //    }
                    //    SendSMS(model.AppUser.ContactNumber, message);
                    //}

                    CreateAlertMessage(AlertMessageType.Success, "Success", "Inter Company Requisition is successfully Assigned.");
                    return RedirectToAction("IndexBy_Approver");
                }
                else
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", "Inter Company Requisition could not be modified anymore.");
                    return RedirectToAction("IndexBy_Approver");
                }
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
            }
            return View(model);
        }

        //# AJAX Method
        public JsonResult GetInternalVehicles(string InternalVehicleRegNum)
        {
            var list = bll.db.Vehicles.Where(m => (m.OWN_MHT_DHT == "OWN" || m.OWN_MHT_DHT == "MHT") && m.RegistrationNumber.Contains(InternalVehicleRegNum)).Select(m =>
             new
             {
                 m.PK_Vehicle,
                 m.RegistrationNumber,
                 ContactNumber = (m.Internal_VehicleContactNumber != null ? m.Internal_VehicleContactNumber : "") + (m.MHT_DHT_DriverContactNumber != null ? m.MHT_DHT_DriverContactNumber : "")
             }
            ).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetExternalVehicles(string ExternalVehicleRegNum)
        {
            var list = bll.db.Vehicles.Where(m => (m.OWN_MHT_DHT == "DHT" || m.OWN_MHT_DHT == "MHT") && m.RegistrationNumber.Contains(ExternalVehicleRegNum)).Select(m =>
             new
             {
                 m.PK_Vehicle,
                 m.RegistrationNumber,
                 ContactNumber = m.MHT_DHT_DriverContactNumber != null ? m.MHT_DHT_DriverContactNumber : ""
             }
            ).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

    }
}