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
    public class LocationBuildingController : BaseController
    {
        Dictionary<string, string> PRG_TypesDict = new Dictionary<string, string> { { "PRAN", "PRAN" }, { "RFL", "RFL" }, { "CS", "CS" } };
        #region All
        public ActionResult Index(Guid? FK_Location, string PRG_Type)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var list = new List<LocationBuilding>();

            var query = bll.db.LocationBuildings.AsEnumerable().Where(c => c.IsDeleted != true);

            //PRG_Type
            if (PRG_Type != null)
            {
                query = query.Where(m => m.PRG_Type == PRG_Type);
            }
            ViewBag.PRG_TypesDict = new SelectList(PRG_TypesDict, "Key", "Value", PRG_Type);

            //FK_Location
            if (FK_Location != null)
            {
                query = query.Where(m => m.FK_Location == FK_Location);
            }
            ViewBag.LocationList = new SelectList(bll.db.Locations.Where(m => m.IsDeleted != true && (m.LocationType == "Factory" || m.LocationType == "Depo" || m.LocationType == "Office")).OrderBy(m => m.Name), "PK_Location", "Name", FK_Location);

            list = query.ToList();
            return View(list);
        }

        public ActionResult Create()
        {
            var model = new LocationBuilding();

            ViewBag.PRG_TypesDict = new SelectList(PRG_TypesDict, "Key", "Value", model.PRG_Type);
            ViewBag.Locations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted != true && (m.LocationType == "Factory" || m.LocationType == "Depo" || m.LocationType == "Office")).OrderBy(m => m.Name), "PK_Location", "Name", model.FK_Location);

            return View(model);
        }
        [HttpPost]
        public ActionResult Create(LocationBuilding model)
        {
            try
            {
                var db_model = new LocationBuilding();
                db_model.IsDeleted = false;

                db_model.Name = model.Name;
                db_model.GraceVehicleCount = model.GraceVehicleCount;
                db_model.PRG_Type = model.PRG_Type;
                db_model.FK_Location = model.FK_Location;

                db_model.CreatedAt = DateTime.Now;
                db_model.FK_CreatedByUser = CurrentUser.PK_User;

                bll.db.LocationBuildings.Add(db_model);
                bll.db.SaveChanges();

                CreateAlertMessage(AlertMessageType.Success, "Success", "LocationBuilding is successfully created.");
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", "Location Building could not be created.");
                ViewBag.PRG_TypesDict = new SelectList(PRG_TypesDict, "Key", "Value", model.PRG_Type);
                ViewBag.Locations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted != true && (m.LocationType == "Factory" || m.LocationType == "Depo" || m.LocationType == "Office")).OrderBy(m => m.Name), "PK_Location", "Name", model.FK_Location);
                return View(model);
            }
        }

        public ActionResult Edit(Int64 id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var model = bll.db.LocationBuildings.Find(id);
                if (model != null)
                {
                    ViewBag.PRG_TypesDict = new SelectList(PRG_TypesDict, "Key", "Value", model.PRG_Type);
                    ViewBag.Locations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted != true && (m.LocationType == "Factory" || m.LocationType == "Depo" || m.LocationType == "Office")).OrderBy(m => m.Name), "PK_Location", "Name", model.FK_Location);
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
        [HttpPost]
        public ActionResult Edit(LocationBuilding model)
        {
            try
            {
                var db_model = bll.db.LocationBuildings.Find(model.PK_LocationBuilding);

                db_model.Name = model.Name;
                db_model.GraceVehicleCount = model.GraceVehicleCount;
                db_model.FK_Location = model.FK_Location;

                db_model.UpdatedAt = DateTime.Now;
                db_model.FK_UpdatedByUser = CurrentUser.PK_User;

                bll.db.Entry(db_model).State = EntityState.Modified;
                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "LocationBuilding is successfully updated.");
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", "Location Building could not be updated.");
                ViewBag.PRG_TypesDict = new SelectList(PRG_TypesDict, "Key", "Value", model.PRG_Type);
                ViewBag.Locations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted != true && (m.LocationType == "Factory" || m.LocationType == "Depo" || m.LocationType == "Office")).OrderBy(m => m.Name), "PK_Location", "Name", model.FK_Location);
                return View(model);
            }
        }

        public ActionResult Delete(Int64 id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var model = bll.db.LocationBuildings.Find(id);
                if (model != null)
                {
                    try
                    {
                        model.IsDeleted = true;
                        model.DeletedAt = DateTime.Now;
                        model.FK_DeletedByUser = CurrentUser.PK_User;
                        bll.db.SaveChanges();
                        CreateAlertMessage(AlertMessageType.Success, "Success", "LocationBuilding is successfully deleted.");
                        return RedirectToAction("Index");
                    }
                    catch (Exception exception)
                    {
                        CreateAlertMessage(AlertMessageType.Warning, "Warning", "Location Building could not be deleted.");
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
        #endregion

        public JsonResult GetLocationBuilding_FK_Location(Guid FK_Location)
        {
            var list = bll.db.LocationBuildings.Where(m => m.FK_Location == FK_Location && m.IsDeleted == false)
                .Select(m =>
                    new
                    {
                        m.PK_LocationBuilding,
                        m.Name,
                    }
                ).OrderBy(m => m.Name).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}