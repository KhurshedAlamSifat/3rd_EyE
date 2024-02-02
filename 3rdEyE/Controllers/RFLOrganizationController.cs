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
    public class RFLOrganizationController : BaseController
    {
        public ActionResult Index()
        {
            var list = bll.db.RFLOrganizations.ToList();
            return View(list);
        }

        public ActionResult Create()
        {
            ViewBag.Locations = new SelectList(bll.db.Locations.Where(m => m.IsDeleted != true).OrderBy(m => m.Name), "PK_Location", "Name");
            return View(new RFLOrganization());
        }

        [HttpPost]
        public ActionResult Create(RFLOrganization model)
        {
            try
            {
                bll.db.RFLOrganizations.Add(model);
                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "RFL Organization is successfully added.");
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
                var model = bll.db.RFLOrganizations.Find(id);
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
        public ActionResult Edit(RFLOrganization model)
        {
            try
            {
                var db_model = bll.db.RFLOrganizations.Where(m => m.PK_RFLOrganization == model.PK_RFLOrganization).FirstOrDefault();
                db_model.OrganizationCode = model.OrganizationCode;
                db_model.OrganizationName = model.OrganizationName;
                db_model.FK_Location = model.FK_Location;
                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "RFL Organization is successfully edited.");
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                return RedirectToAction("Edit", new { id = model.PK_RFLOrganization });
            }
        }
        public JsonResult GetRFLOrganizationBy_FK_Location(Guid FK_Location)
        {
            var list = bll.db.RFLOrganizations.Where(m => m.FK_Location == FK_Location)
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