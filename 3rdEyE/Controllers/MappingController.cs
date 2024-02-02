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
    public class MappingController : BaseController
    {
        public ActionResult Index()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var list = bll.db.Mappings.ToList();
            return View(list);
        }

        public ActionResult Create()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            ViewBag.MappingKeys = new SelectList(bll.db.Mappings.Select(m => m.MappingKey).Distinct());
            return View(new Mapping());
        }

        [HttpPost]
        public ActionResult Create(Mapping model)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            try
            {
                if (bll.db.Mappings.Where(m => m.MappingKey == model.MappingKey && m.IndependentKeyValue == model.IndependentKeyValue && m.DependentKeyValue == model.DependentKeyValue).Any())
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", "Mapping already exists.");
                    return RedirectToAction("Index");
                }
                else
                {
                    model.IndependentKeyName = bll.db.Mappings.Where(m => m.MappingKey == model.MappingKey).FirstOrDefault().IndependentKeyName;
                    model.DependentKeyName = bll.db.Mappings.Where(m => m.MappingKey == model.MappingKey).FirstOrDefault().DependentKeyName;

                    model.FK_CreatedByUser = CurrentUser.PK_User;
                    model.CreatedAt = DateTime.Now;

                    bll.db.Mappings.Add(model);
                    bll.db.SaveChanges();
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Mapping is successfully added.");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                ViewBag.MappingKeys = new SelectList(bll.db.Mappings.Select(m => m.MappingKey).Distinct(), model.MappingKey);
                return View(model);
            }
        }

        public ActionResult Edit(Int64 id)
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
                var model = bll.db.Mappings.Find(id);
                if (model != null)
                {
                    ViewBag.MappingKeys = new SelectList(bll.db.Mappings.Select(m => m.MappingKey).Distinct(), model.MappingKey);
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        public ActionResult Edit(Mapping model)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            try
            {
                var db_model = bll.db.Mappings.Where(m => m.PK_Mapping == model.PK_Mapping).FirstOrDefault();
                db_model.IndependentKeyName = bll.db.Mappings.Where(m => m.MappingKey == model.MappingKey).FirstOrDefault().IndependentKeyName;
                db_model.DependentKeyName = bll.db.Mappings.Where(m => m.MappingKey == model.MappingKey).FirstOrDefault().DependentKeyName;
                db_model.IndependentKeyValue = model.IndependentKeyValue;
                db_model.DependentKeyValue = model.DependentKeyValue;

                model.FK_UpdatedByUser = CurrentUser.PK_User;
                model.UpdatedAt = DateTime.Now;

                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "Mapping is successfully edited.");
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                return RedirectToAction("Edit", new { id = model.PK_Mapping });
            }
        }

        public ActionResult Delete(Int64 id)
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
                var model = bll.db.Mappings.Find(id);
                if (model != null)
                {
                    bll.db.Mappings.Remove(model);
                    bll.db.SaveChanges();
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Mapping is successfully deleted.");
                    return RedirectToAction("Index");
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
    }
}