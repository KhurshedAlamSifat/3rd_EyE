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
    public class PranOrganizationController : BaseController
    {
        public ActionResult Index()
        {
            var list = bll.db.PranOrganizations.ToList();
            return View(list);
        }

        public ActionResult Create()
        {
            ViewBag.Locations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted != true).OrderBy(m => m.Name), "PK_Location", "Name");
            return View(new PranOrganization());
        }

        [HttpPost]
        public ActionResult Create(PranOrganization model)
        {
            try
            {
                bll.db.PranOrganizations.Add(model);
                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "Pran Organization is successfully added.");
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
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
                var model = bll.db.PranOrganizations.Find(id);
                if (model != null)
                {
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
        public ActionResult Edit(PranOrganization model)
        {
            try
            {
                var db_model = bll.db.PranOrganizations.Where(m => m.PK_PranOrganization == model.PK_PranOrganization).FirstOrDefault();
                db_model.OrganizationCode = model.OrganizationCode;
                db_model.OrganizationName = model.OrganizationName;
                db_model.FK_Location = model.FK_Location;
                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "Pran Organization is successfully edited.");
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                return RedirectToAction("Edit", new { id = model.PK_PranOrganization });
            }
        }
        public JsonResult GetPranOrganizationBy_FK_Location(Guid FK_Location)
        {
            var list = bll.db.PranOrganizations.Where(m => m.FK_Location == FK_Location)
                .Select(m =>
                    new
                    {
                        m.OrganizationCode,
                        m.OrganizationName,
                        m.LocationName,
                        m.FK_Location
                    }
                ).OrderBy(m => m.OrganizationCode).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}