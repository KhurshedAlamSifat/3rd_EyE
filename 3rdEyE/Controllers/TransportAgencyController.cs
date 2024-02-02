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
using _3rdEyE.BLLs;
using _3rdEyE.ViewModels;
using Microsoft.Reporting.WebForms;

namespace _3rdEyE.Controllers
{
    public class TransportAgencyController : BaseController
    {

        public ActionResult Index()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var list = bll.db.TransportAgencies.AsEnumerable().Where(c => c.IsDeleted == false).ToList();
            return View(list);
        }
        //public ActionResult View(Guid id)
        //{
        //    if (CommonClass.IsInvalidAccess())
        //    {
        //        return Redirect("/Access/Login");
        //    }
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    else
        //    {
        //        var model = bll.db.AppUsers.Find(id);
        //        if (model != null)
        //        {
        //            return View(model);
        //        }
        //        else
        //        {
        //            return HttpNotFound();
        //        }
        //    }
        //}

        public ActionResult Create()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            ViewBag.Districts = new SelectList(bll.db.Districts.OrderBy(m => m.Name), "Name", "Name");

            return View(new TransportAgency());
        }
        [HttpPost]
        public ActionResult Create(TransportAgency model)
        {

            //# Validation
            string modelValidator = "";
            if (bll.db.TransportAgencies.Where(m => m.Name == model.Name && m.DistrictName == model.DistrictName).Any())
            {
                modelValidator = modelValidator + " Transport Agency aleary exist.";
            }


            if (modelValidator == "")
            {
                try
                {
                    model.CreatedAt = DateTime.Now;
                    model.FK_CreatedByUser = CurrentUser.PK_User;
                    bll.db.TransportAgencies.Add(model);

                    bll.db.SaveChanges();
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Tarnsport Agency is successfully added.");
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
            ViewBag.Districts = new SelectList(bll.db.Districts.OrderBy(m => m.Name), "Name", "Name", model.DistrictName);
            return View(model);
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
                var model = bll.db.TransportAgencies.Find(id);
                if (model != null)
                {
                    ViewBag.model = model;
                    var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
                    ViewBag.Districts = new SelectList(bll.db.Districts.OrderBy(m => m.Name), "Name", "Name", model.DistrictName);
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
        [HttpPost]
        public ActionResult Edit(TransportAgency model)
        {

            //# Validation
            string modelValidator = "";
            if (bll.db.TransportAgencies.Where(m => m.PK_TransportAgency != model.PK_TransportAgency && m.Name == model.Name && m.DistrictName == model.DistrictName).Any())
            {
                modelValidator = modelValidator + " Transport Agency aleary exist.";
            }


            if (modelValidator == "")
            {
                try
                {
                    model.UpdatedAt = DateTime.Now;
                    model.FK_UpdatedByUser = CurrentUser.PK_User;
                    
                    bll.db.Entry(model).State = EntityState.Modified;
                    bll.db.SaveChanges();
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Tarnsport Agency is successfully added.");
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
            ViewBag.Districts = new SelectList(bll.db.Districts.OrderBy(m => m.Name), "Name", "Name", model.DistrictName);
            return View(model);
        }

        public ActionResult Delete(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var model = bll.db.TransportAgencies.Find(id);
                if (model != null)
                {
                    try
                    {
                        model.IsDeleted = true;
                        bll.db.SaveChanges();
                        CreateAlertMessage(AlertMessageType.Success, "Success", "Tarnsport Agency is successfully deleted.");
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