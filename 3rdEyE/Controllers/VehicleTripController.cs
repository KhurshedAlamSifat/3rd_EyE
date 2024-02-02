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

    //# THIS IS OBSOLETE MODULE
    public class VehicleTripController : BaseController
    {
        BLL_VehicleTrip bll = new BLL_VehicleTrip();
        public ActionResult Index(DateTime? StartingDate, DateTime? EndingDate)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            if (StartingDate != null && EndingDate != null)
            {
                var _StartingDate = StartingDate != null ? StartingDate : new DateTime();
                var _EndingDate = EndingDate != null ? EndingDate : new DateTime();
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", _StartingDate);
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", _EndingDate);
                var list = bll.db.VehicleTrips.AsEnumerable().Where(c => c.IsDeleted == false && c.CreatedAt >= StartingDate && c.CreatedAt <= EndingDate.Value.AddDays(1)).Select(c => bll.ConvertToViewModel(c)).ToList();
                return View(list);
            }
            else
            {
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", DateTime.Today.Date);
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd h:mm tt}", DateTime.Today.AddDays(1).Date);
                var list = new List<ViewModels.VM_VehicleTrip>();
                return View(list);
            }
        }
        public ActionResult VehicleTripDashBoard()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            return View();
        }
        public JsonResult GetVehicleTripDashBoardData()
        {
            DateTime now = DateTime.Now;
            var lastMonthFirstDay = new DateTime(now.Year, now.Month - 1, 1);
            var lastMonthLastDay = lastMonthFirstDay.AddMonths(1).AddDays(-1);
            object DepoWiseLastMonthVehicleTripExpense;
            /*
            Delete  from ReadyReport where ReportName = 'DepoWiseLastMonthVehicleTripExpense';
            
            insert into ReadyReport(BaseModule,ReportName,PRG_Type,[Key],Value1,Value2)
            select 'VehicleTrip/VehicleTripDashBoard','DepoWiseLastMonthVehicleTripExpense', 'PRAN' ,Depo.Name, sum(VehicleTrip.CarriageOutword) as TotalExpense, count(VehicleTrip.CarriageOutword) as TotalCount  from VehicleTrip
            join Depo on Depo.PK_Depo = VehicleTrip.FK_Depo_From
            where VehicleTrip.CreatedAt >= '2020-08-01' and VehicleTrip.CreatedAt < '2020-09-01'
            and Depo.PRG_Type = 'PRAN'
            group by Depo.PRG_Type,Depo.Name;

            insert into ReadyReport(BaseModule,ReportName,PRG_Type,[Key],Value1,Value2)
            select 'VehicleTrip/VehicleTripDashBoard','DepoWiseLastMonthVehicleTripExpense', 'RFL' ,Depo.Name, sum(VehicleTrip.CarriageOutword) as TotalExpense, count(VehicleTrip.CarriageOutword) as TotalCount  from VehicleTrip
            join Depo on Depo.PK_Depo = VehicleTrip.FK_Depo_From
            where VehicleTrip.CreatedAt >= '2020-08-01' and VehicleTrip.CreatedAt < '2020-09-01'
            and Depo.PRG_Type = 'RFL'
            group by Depo.PRG_Type,Depo.Name;
             */
            //if (CurrentUser.PRG_Type == "ALL")
            //{
            //    DepoWiseLastMonthVehicleTripExpense = bll.db.ReadyReports.Where(m => m.ReportName == "DepoWiseLastMonthVehicleTripExpense").Select(m => new
            //    {
            //        DepoName = m.Key,
            //        DepoExpense = m.Value1,
            //        NumberOfHiring = m.Value2
            //    }).OrderByDescending(m => m.DepoExpense).ToList();
            //}
            //else
            //{
            //    DepoWiseLastMonthVehicleTripExpense = bll.db.ReadyReports.Where(m => m.ReportName == "DepoWiseLastMonthVehicleTripExpense" && m.PRG_Type == CurrentUser.PRG_Type).Select(m => new
            //    {
            //        DepoName = m.Key,
            //        DepoExpense = m.Value1,
            //        NumberOfHiring = m.Value2
            //    }).OrderByDescending(m => m.DepoExpense).ToList();
            //}
            if (CurrentUser.PRG_Type == "ALL")
            {
                DepoWiseLastMonthVehicleTripExpense = bll.db.VehicleTrips.Where(m => m.CreatedAt > lastMonthFirstDay && m.CreatedAt < lastMonthLastDay).GroupBy(m => m.Depo).Select(m => new
                {
                    DepoName = m.Key.Name,
                    DepoExpense = m.Sum(n => n.CarriageOutword),
                    NumberOfHiring = m.Count()
                }
                ).OrderByDescending(m => m.DepoExpense).ToList();
            }
            else
            {
                DepoWiseLastMonthVehicleTripExpense = bll.db.VehicleTrips.Where(m => m.CreatedAt > lastMonthFirstDay && m.CreatedAt < lastMonthLastDay && m.Depo.PRG_Type == CurrentUser.PRG_Type).GroupBy(m => m.Depo).Select(m => new
                {
                    DepoName = m.Key.Name,
                    DepoExpense = m.Sum(n => n.CarriageOutword),
                    NumberOfHiring = m.Count()
                }
                ).OrderByDescending(m => m.DepoExpense).ToList();
            }
            object Most3HiredVehiclesForTrip;
            /*
            Delete  from ReadyReport where ReportName = 'Most3HiredVehiclesForTrip';
            
            insert into ReadyReport(BaseModule,ReportName,PRG_Type,[Key],Value1,Value_varchar)
            select top 3 'VehicleTrip/VehicleTripDashBoard','Most3HiredVehiclesForTrip', 'PRAN', Vehicle.RegistrationNumber, count(Vehicle.RegistrationNumber) as TotalCount, Depo.Name  from VehicleTrip
            join Depo on Depo.PK_Depo = VehicleTrip.FK_Depo_From
            join Vehicle on VehicleTrip.FK_Vehicle = Vehicle.PK_Vehicle
            where VehicleTrip.CreatedAt >= '2020-06-01' and VehicleTrip.CreatedAt < '2020-09-01'
            and Depo.PRG_Type = 'PRAN'
            group by Depo.PRG_Type,Vehicle.RegistrationNumber,Depo.Name
            order by count(Vehicle.RegistrationNumber) desc;

            insert into ReadyReport(BaseModule,ReportName,PRG_Type,[Key],Value1,Value_varchar)
            select top 3 'VehicleTrip/VehicleTripDashBoard','Most3HiredVehiclesForTrip', 'RFL', Vehicle.RegistrationNumber, count(Vehicle.RegistrationNumber) as TotalCount, Depo.Name  from VehicleTrip
            join Depo on Depo.PK_Depo = VehicleTrip.FK_Depo_From
            join Vehicle on VehicleTrip.FK_Vehicle = Vehicle.PK_Vehicle
            where VehicleTrip.CreatedAt >= '2020-06-01' and VehicleTrip.CreatedAt < '2020-09-01'
            and Depo.PRG_Type = 'RFL'
            group by Depo.PRG_Type,Vehicle.RegistrationNumber,Depo.Name
            order by count(Vehicle.RegistrationNumber) desc;

             */
            //if (CurrentUser.PRG_Type == "ALL")
            //{
            //    Most3HiredVehiclesForTrip = bll.db.ReadyReports.Where(m => m.ReportName == "Most3HiredVehiclesForTrip").Select(m => new
            //    {
            //        RegistrationNumber = m.Key,
            //        DepoName = m.Value_varchar,
            //        HiredCount = m.Value1
            //    }).OrderByDescending(m => m.HiredCount).ToList();
            //}
            //else
            //{
            //    Most3HiredVehiclesForTrip = bll.db.ReadyReports.Where(m => m.ReportName == "Most3HiredVehiclesForTrip" && m.PRG_Type == CurrentUser.PRG_Type).Select(m => new
            //    {
            //        RegistrationNumber = m.Key,
            //        DepoName = m.Value_varchar,
            //        HiredCount = m.Value1
            //    }).OrderByDescending(m => m.HiredCount).ToList();
            //}

            if (CurrentUser.PRG_Type == "ALL")
            {
                Most3HiredVehiclesForTrip = bll.db.VehicleTrips.Where(m => m.CreatedAt > lastMonthFirstDay && m.CreatedAt < lastMonthLastDay).GroupBy(m => new { m.Depo, m.Vehicle }).Select(m => new
                {
                    DepoName = m.Key.Depo.Name,
                    RegistrationNumber = m.Key.Vehicle.RegistrationNumber,
                    HiredCount = m.Count()
                }).OrderByDescending(m => m.HiredCount).Take(3).ToList();
            }
            else
            {
                Most3HiredVehiclesForTrip = bll.db.VehicleTrips.Where(m => m.CreatedAt > lastMonthFirstDay && m.CreatedAt < lastMonthLastDay && m.Vehicle.Depo.PRG_Type == CurrentUser.PRG_Type).GroupBy(m => new { m.Depo, m.Vehicle }).Select(m => new
                {
                    DepoName = m.Key.Depo.Name,
                    RegistrationNumber = m.Key.Vehicle.RegistrationNumber,
                    HiredCount = m.Count()
                }).OrderByDescending(m => m.HiredCount).Take(3).ToList();
            }
            return Json(new { DepoWiseLastMonthVehicleTripExpense, Most3HiredVehiclesForTrip }, JsonRequestBehavior.AllowGet);

        }
        #region //OLD
        public ActionResult View_DHT(Guid id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var model = bll.db.VehicleTrips.Find(id);
                if (model != null)
                {
                    var viewModel = bll.ConvertToViewModel(model);
                    return View(viewModel);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
        //public ActionResult Create_DHT()
        //{
        //    if (CommonClass.IsInvalidAccess())
        //    {
        //        return Redirect("/Access/Login");
        //    }
        //    ViewBag.model = new VehicleTrip();
        //    ViewBag.Vehicles = new SelectList(bll.db.Vehicles.Where(m => m.IsDeleted == false && m.OWN_MHT_DHT == "DHT").OrderBy(m => m.RegistrationNumber), "PK_Vehicle", "RegistrationNumber");
        //    var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
        //    ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && m.PK_Depo != _invalidDepoPK).OrderBy(m => m.Name), "PK_Depo", "Name");
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult Create_DHT(VehicleTrip model)
        //{
        //    if (CommonClass.IsInvalidAccess())
        //    {
        //        return Redirect("/Access/Login");
        //    }
        //    //string modelValidator = bll.IsValidModel_ToCreate(model);
        //    string modelValidator = ValidationStatus.OK;
        //    if (modelValidator == ValidationStatus.OK)
        //    {
        //        try
        //        {
        //            var db_model = bll.FilterToDBModel_DHT(model);
        //            bll.db.VehicleTrips.Add(db_model);
        //            bll.db.SaveChanges();
        //            CreateAlertMessage(AlertMessageType.Success, "Success", "Vehicle Trip is successfully added.");
        //            return RedirectToAction("Index");
        //        }
        //        catch (Exception exception)
        //        {
        //            CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
        //        }
        //    }
        //    else
        //    {
        //        CreateAlertMessage(AlertMessageType.Danger, "Validation Failure", modelValidator);
        //    }
        //    ViewBag.Vehicles = new SelectList(bll.db.Vehicles.Where(m => m.IsDeleted == false && m.OWN_MHT_DHT == "DHT").OrderBy(m => m.RegistrationNumber), "PK_Vehicle", "RegistrationNumber", model.FK_Vehicle);
        //    var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
        //    ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && m.PK_Depo != _invalidDepoPK).OrderBy(m => m.Name), "PK_Depo", "Name");

        //    return View(model);
        //}

        public ActionResult Edit_DHT(Guid? id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var model = bll.db.VehicleTrips.Find(id);
                if (model != null)
                {
                    ViewBag.Vehicles = new SelectList(bll.db.Vehicles.Where(m => m.IsDeleted == false && m.OWN_MHT_DHT == "DHT").OrderBy(m => m.RegistrationNumber), "PK_Vehicle", "RegistrationNumber", model.FK_Vehicle);
                    var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
                    ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && m.PK_Depo != _invalidDepoPK).OrderBy(m => m.Name), "PK_Depo", "Name");

                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
        [HttpPost]
        public ActionResult Edit_DHT(VehicleTrip model)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            //string modelValidator = bll.IsValidModel_ToEdit(model);
            string modelValidator = ValidationStatus.OK;
            if (modelValidator == ValidationStatus.OK)
            {
                try
                {
                    var db_model = bll.FilterToDBModel_DHT(model);
                    bll.db.SaveChanges();
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Vehicle Trip is successfully edited.");
                    return RedirectToAction("Index");
                }
                catch (Exception exception)
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                }
            }
            else
            {
                CreateAlertMessage(AlertMessageType.Danger, "Validation Failure", modelValidator);
            }
            ViewBag.Vehicles = new SelectList(bll.db.Vehicles.Where(m => m.IsDeleted == false && m.OWN_MHT_DHT == "DHT").OrderBy(m => m.RegistrationNumber), "PK_Vehicle", "RegistrationNumber", model.FK_Vehicle);
            var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
            ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && m.PK_Depo != _invalidDepoPK).OrderBy(m => m.Name), "PK_Depo", "Name");

            return View(model);
        }
        #endregion // OLd















        #region //OLD
        public ActionResult View(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var model = bll.db.VehicleTrips.Find(id);
                if (model != null)
                {
                    var viewModel = bll.ConvertToViewModel(model);
                    return View(viewModel);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
        public ActionResult Create()
        {
            ViewBag.model = new VehicleTrip();
            ViewBag.Vehicles = new SelectList(bll.db.Vehicles.Where(m => m.IsDeleted == false).OrderBy(m => m.RegistrationNumber), "PK_Vehicle", "RegistrationNumber");
            var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
            ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && m.PK_Depo != _invalidDepoPK).OrderBy(m => m.Name), "PK_Depo", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult Create(VehicleTrip model)
        {
            //string modelValidator = bll.IsValidModel_ToCreate(model);
            string modelValidator = ValidationStatus.OK;
            if (modelValidator == ValidationStatus.OK)
            {
                try
                {
                    var db_model = bll.FilterToDBModel_Common(model);
                    bll.db.VehicleTrips.Add(db_model);
                    bll.db.SaveChanges();
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Vehicle Trip is successfully added.");
                    return RedirectToAction("Index");
                }
                catch (Exception exception)
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                }
            }
            else
            {
                CreateAlertMessage(AlertMessageType.Danger, "Validation Failure", modelValidator);
            }
            ViewBag.Vehicles = new SelectList(bll.db.Vehicles.Where(m => m.IsDeleted == false).OrderBy(m => m.RegistrationNumber), "PK_Vehicle", "RegistrationNumber", model.FK_Vehicle);
            var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
            ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && m.PK_Depo != _invalidDepoPK).OrderBy(m => m.Name), "PK_Depo", "Name");

            return View(model);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var model = bll.db.VehicleTrips.Find(id);
                if (model != null)
                {
                    ViewBag.Vehicles = new SelectList(bll.db.Vehicles.Where(m => m.IsDeleted == false).OrderBy(m => m.RegistrationNumber), "PK_Vehicle", "RegistrationNumber", model.FK_Vehicle);
                    var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
                    ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && m.PK_Depo != _invalidDepoPK).OrderBy(m => m.Name), "PK_Depo", "Name");

                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
        [HttpPost]
        public ActionResult Edit(VehicleTrip model)
        {
            //string modelValidator = bll.IsValidModel_ToEdit(model);
            string modelValidator = ValidationStatus.OK;
            if (modelValidator == ValidationStatus.OK)
            {
                try
                {
                    var db_model = bll.FilterToDBModel_Common(model);
                    bll.db.SaveChanges();
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Vehicle Trip is successfully edited.");
                    return RedirectToAction("Index");
                }
                catch (Exception exception)
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                }
            }
            else
            {
                CreateAlertMessage(AlertMessageType.Danger, "Validation Failure", modelValidator);
            }
            ViewBag.Vehicles = new SelectList(bll.db.Vehicles.Where(m => m.IsDeleted == false).OrderBy(m => m.RegistrationNumber), "PK_Vehicle", "RegistrationNumber", model.FK_Vehicle);
            var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
            ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && m.PK_Depo != _invalidDepoPK).OrderBy(m => m.Name), "PK_Depo", "Name");

            return View(model);
        }
        #endregion // OLd


        public ActionResult Delete(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var model = bll.db.VehicleTrips.Find(id);
                if (model != null)
                {
                    try
                    {
                        model.IsDeleted = true;
                        bll.db.SaveChanges();
                        CreateAlertMessage(AlertMessageType.Success, "Success", "Vehicle Trip is successfully deleted.");
                        return RedirectToAction("Index");
                    }
                    catch (Exception exception)
                    {
                        CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                        return RedirectToAction("Index");
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