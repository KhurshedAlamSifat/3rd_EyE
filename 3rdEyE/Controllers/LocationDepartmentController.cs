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
    public class LocationDepartmentController : BaseController
    {
        Dictionary<string, string> PRG_TypesDict = new Dictionary<string, string> { { "PRAN", "PRAN" }, { "RFL", "RFL" }, { "CS", "CS" } };
        public ActionResult Index()
        {
            var list = bll.db.LocationDepartments.ToList();
            return View(list);
        }

        public ActionResult Create()
        {
            ViewBag.PRG_TypesDict = new SelectList(PRG_TypesDict, "Key", "Value");
            ViewBag.Locations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted != true).OrderBy(m => m.Name), "PK_Location", "Name");
            return View(new LocationDepartment());
        }

        [HttpPost]
        public ActionResult Create(LocationDepartment model)
        {
            try
            {
                bll.db.LocationDepartments.Add(model);
                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "Location Department is successfully added.");
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                ViewBag.PRG_TypesDict = new SelectList(PRG_TypesDict, "Key", "Value", model.PRG_Type);
                ViewBag.Locations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted != true).OrderBy(m => m.Name), "PK_Location", "Name", model.FK_Location);
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
                var model = bll.db.LocationDepartments.Find(id);
                if (model != null)
                {
                    ViewBag.PRG_TypesDict = new SelectList(PRG_TypesDict, "Key", "Value", model.PRG_Type);
                    ViewBag.Locations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted != true).OrderBy(m => m.Name), "PK_Location", "Name", model.FK_Location);
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        public ActionResult Edit(LocationDepartment model)
        {
            try
            {
                var db_model = bll.db.LocationDepartments.Where(m => m.PK_LocationDepartment == model.PK_LocationDepartment).FirstOrDefault();
                db_model.DepartmentCode = model.DepartmentCode;
                db_model.DepartmentName = model.DepartmentName;
                db_model.FK_Location = model.FK_Location;
                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "Location Department is successfully edited.");
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                return RedirectToAction("Edit", new { id = model.PK_LocationDepartment });
            }
        }
        public JsonResult GetLocationDepartment_FK_Location(Guid FK_Location)
        {
            var list = bll.db.LocationDepartments.Where(m => m.FK_Location == FK_Location && m.PRG_Type == CurrentUser.PRG_Type)
                .Select(m =>
                    new
                    {
                        m.PK_LocationDepartment,
                        m.DepartmentName,
                        m.DepartmentCode,
                        m.FK_Location
                    }
                ).OrderBy(m => m.DepartmentCode).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetLocationInfoWithDepartment_FK_Location(Guid FK_Location)
        {
            var location = bll.db.Locations.Where(m => m.PK_Location == FK_Location).Select(m => new { m.PK_Location, m.PRG_Type, m.Name, m.LocationType }).FirstOrDefault();
            var locationDepartments = bll.db.LocationDepartments.Where(m => m.FK_Location == FK_Location && m.PRG_Type == CurrentUser.PRG_Type)
                .Select(m =>
                    new
                    {
                        m.PK_LocationDepartment,
                        m.DepartmentName,
                        m.DepartmentCode,
                        m.FK_Location
                    }
                ).OrderBy(m => m.DepartmentCode).ToList();
            return Json(new { location, locationDepartments }, JsonRequestBehavior.AllowGet);
        }
    }
}