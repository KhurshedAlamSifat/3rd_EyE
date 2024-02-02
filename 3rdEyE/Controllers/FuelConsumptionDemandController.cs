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
using System.Data.SqlClient;

namespace _3rdEyE.Controllers
{
    public class FuelConsumptionDemandController : BaseController
    {
        Dictionary<string, string> PRG_TypesDict = new Dictionary<string, string> { { "PRAN", "PRAN" }, { "RFL", "RFL" }, { "CS", "CS" } };
        Dictionary<string, float> FuelTypeDisct = new Dictionary<string, float> { { "Disel", 80 } };
        public int DiselUnitPrice = 80;
        List<string> Statuses = new List<string> { "Created", "Rejected", "Given" };
        public ActionResult CreateDemand_WithTrip(Nullable<Int64> FK_RequisitionTrip)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            ViewBag.FuelPumps = new SelectList(bll.db.FuelPumps.Where(m => m.IsDeleted == false), "PK_FuelPump", "Name");
            var trip = bll.db.RequisitionTrips.Include("Requisition").Where(m => m.PK_RequisitionTrip == FK_RequisitionTrip).FirstOrDefault();
            ViewBag.RequisitionTrip = trip;
            ViewBag.Vehicles = new SelectList(bll.db.Vehicles.Where(m => m.IsDeleted == false && m.PK_Vehicle == trip.FK_Vehicle), "PK_Vehicle", "RegistrationNumber", trip.FK_Vehicle);
            ViewBag.Locations1 = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name", trip.Requisition.FK_Location_From);
            ViewBag.Locations2 = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name", trip.Requisition.FK_Location_To);
            return View();
        }
        [HttpPost]
        public ActionResult CreateDemand_WithTrip(FuelConsumptionDemand model)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            try
            {
                model.IsDeleted = false;
                model.PRG_Type = CurrentUser.PRG_Type;
                model.FuelType = "Disel";
                model.FuelUnitType = "Litre";
                model.Status = "Created";
                model.Vehicle_KPL = 0;
                model.RequiredQuantity_Auto = 0;
                model.FuelUnitPrice = 80;

                model.CreatedAt = DateTime.Now;
                model.FK_AppUser_Client = CurrentUser.PK_User;

                bll.db.FuelConsumptionDemands.Add(model); ;
                bll.db.SaveChanges();
                model.TrackingID = "FD " + model.PK_FuelConsumptionDemand;
                bll.db.SaveChanges();

                CreateAlertMessage(AlertMessageType.Success, "Success", "Demands are successfully added.");
                return RedirectToAction("Index_All");
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                return RedirectToAction("Index_All");
            }
        }

        public ActionResult CreateDemand_WithoutTrip()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            ViewBag.FuelPumps = new SelectList(bll.db.FuelPumps.Where(m => m.IsDeleted == false), "PK_FuelPump", "Name");
            ViewBag.Vehicles = new SelectList(bll.db.Vehicles.Where(m => m.IsDeleted == false && m.PK_Vehicle == null), "PK_Vehicle", "RegistrationNumber");
            ViewBag.Locations1 = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name");
            ViewBag.Locations2 = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name");
            return View();
        }

        public ActionResult Index_All(string PRG_Type, DateTime? StartingDate, DateTime? EndingDate, string FK_Location_From, string FK_Location_To, String TrackingId, String Status)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var list = new List<FuelConsumptionDemand>();
            var now = DateTime.Now;
            var query = bll.db.FuelConsumptionDemands.AsQueryable().Where(m => m.IsDeleted != true);

            //PRG_Type
            if (!string.IsNullOrEmpty(PRG_Type))
            {
                query = query.Where(m => m.PRG_Type == PRG_Type);
                ViewBag.PRG_TypesDict = new SelectList(PRG_TypesDict, "Key", "Value", PRG_Type);
            }
            else
            {
                ViewBag.PRG_TypesDict = new SelectList(PRG_TypesDict, "Key", "Value");
            }

            if (StartingDate != null)
            {
                var _StartingDate = StartingDate != null ? StartingDate : new DateTime();
                query = query.Where(m => m.RequiredAt > _StartingDate);
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", _StartingDate);
            }
            else
            {
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", now.Date);
            }

            if (EndingDate != null)
            {
                var _EndingDate = EndingDate != null ? EndingDate : new DateTime();
                query = query.Where(m => m.RequiredAt < _EndingDate);
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

            if (!string.IsNullOrEmpty(FK_Location_From))
            {
                var _FK_Location_From = Guid.Parse(FK_Location_From);
                query = query.Where(m => m.FK_LocationFrom == _FK_Location_From);
                ViewBag.FromLocations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name", FK_Location_From);
            }
            else
            {
                ViewBag.FromLocations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name");
            }

            if (!string.IsNullOrEmpty(FK_Location_To))
            {
                var _FK_Location_To = Guid.Parse(FK_Location_To);
                query = query.Where(m => m.FK_LocationTo == _FK_Location_To);
                ViewBag.ToLocations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name", FK_Location_To);
            }
            else
            {
                ViewBag.ToLocations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Location", "Name");
            }

            if (!string.IsNullOrEmpty(Status))
            {
                query = query.Where(m => m.Status == Status);
                ViewBag.Statuses = new SelectList(Statuses, Status);
            }
            else
            {
                ViewBag.Statuses = new SelectList(Statuses, Status);
            }
            if (StartingDate != null || EndingDate != null || (!string.IsNullOrEmpty(TrackingId)) || FK_Location_From != null || FK_Location_To != null || (!string.IsNullOrEmpty(Status)))
            {
                list = query.AsQueryable().ToList();
            }
            return View(list);
        }

        public ActionResult Index_Pump()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var StartingDateTime = DateTime.Now.AddDays(-1);
            var query = bll.db.FuelConsumptionDemands.AsQueryable().Where(m => m.IsDeleted != true && m.RequiredAt > StartingDateTime && m.Status == "Created");
            var list = query.ToList();
            return View(list);
        }
        public ActionResult FuelGiving(Int64 PK_FuelConsumptionDemand)
        {
            var model = bll.db.FuelConsumptionDemands.Find(PK_FuelConsumptionDemand);
            if (model == null)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", "Demand not found");
                return RedirectToAction("Index_Client");
            }
            else if (model.Status != "Created")
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", "Request is not in proper status");
                return RedirectToAction("Index_Pump");
            }
            else
            {
                ViewBag.DiselUnitPrice = DiselUnitPrice;
                return View(model);
            }
        }
        [HttpPost]
        public ActionResult FuelGiving(FuelConsumptionDemand model)
        {
            try
            {
                var db_model = bll.db.FuelConsumptionDemands.Where(m => m.PK_FuelConsumptionDemand == model.PK_FuelConsumptionDemand).FirstOrDefault();
                if (db_model == null)
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", "Demand not found");
                    return RedirectToAction("Index_Pump");
                }
                else if (db_model.Status != "Created")
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", "Demand is not in proper status");
                    return RedirectToAction("Index_Pump");
                }
                else
                {
                    db_model.GivenQuantity = model.GivenQuantity;
                    db_model.GivenQuantityPrice = db_model.GivenQuantity * DiselUnitPrice;
                    if (db_model.BusinessUnit_1 != null)
                    {
                        var BusinessUnit_1_GivenQuantityPrice = db_model.GivenQuantity / 100 * ((decimal)db_model.BusinessUnit_1_CarringPercentage) * DiselUnitPrice;
                        db_model.BusinessUnit_1_GivenQuantityPrice = BusinessUnit_1_GivenQuantityPrice;
                    }
                    if (db_model.BusinessUnit_2 != null)
                    {
                        var BusinessUnit_2_GivenQuantityPrice = db_model.GivenQuantity / 100 * ((decimal)db_model.BusinessUnit_2_CarringPercentage) * DiselUnitPrice;
                        db_model.BusinessUnit_2_GivenQuantityPrice = BusinessUnit_2_GivenQuantityPrice;
                    }

                    db_model.ExcessQuantity = db_model.RequiredQuantity_Manual - db_model.GivenQuantity;
                    db_model.ExcessQuantityPrice = db_model.ExcessQuantity * DiselUnitPrice;

                    db_model.Status = "Given";
                    db_model.FK_AppUser_FuelGivenBy = CurrentUser.PK_User;
                    db_model.FuelGivenAt = DateTime.Now;
                    bll.db.SaveChanges();
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Request succesfully updated.");
                    return RedirectToAction("Index_Pump");
                }
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                return RedirectToAction("Index_Pump");
            }
        }


        public ActionResult Index_Account()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var query = bll.db.FuelConsumptionDemands.AsQueryable().Where(m => m.IsDeleted != true && m.Status == "Given" && m.ExcessQuantityPrice != 0 && m.ExcessMoneyGivenAt == null);
            var list = query.ToList();
            return View(list);
        }
        public ActionResult ExcessQuantityPriceGiving(Int64 PK_FuelConsumptionDemand)
        {
            var model = bll.db.FuelConsumptionDemands.Find(PK_FuelConsumptionDemand);
            if (model == null)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", "Demand not found");
                return RedirectToAction("Index_Account");
            }
            else if (model.Status != "Given" || model.ExcessQuantityPrice == 0 || model.ExcessMoneyGivenAt != null)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", "Demand is not in proper status");
                return RedirectToAction("Index_Account");
            }
            else
            {
                model.ExcessMoneyGivenAt = DateTime.Now;
                model.FK_AppUser_FuelGivenBy = CurrentUser.PK_User;
                bll.db.SaveChanges();
                return RedirectToAction("Index_Account");
            }
        }
    }
}