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

namespace _3rdEyE.Controllers
{
    public class MapLocationController : BaseController
    {
        #region All
        public ActionResult Index()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var list = bll.db.MapLocations.ToList();
            return View(list);
        }
        public ActionResult Create()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Create(MapLocation model)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            model.CreatedAt = DateTime.Now;
            model.FK_CreatedByUser = CurrentUser.PK_User;

            bll.db.MapLocations.Add(model);
            bll.db.SaveChanges();
            CreateAlertMessage(AlertMessageType.Success, "Success", "MapLocation is successfully added.");
            return RedirectToAction("Index");
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
            MapLocation model = bll.db.MapLocations.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(MapLocation model)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var db_model = bll.db.MapLocations.Find(model.PK_MapLoaction);

            db_model.Name = model.Name;
            db_model.Latitude = model.Latitude;
            db_model.Longitude = model.Longitude;

            db_model.UpdatedAt = DateTime.Now;
            db_model.FK_UpdatedByUser = CurrentUser.PK_User;
            bll.db.SaveChanges();
            CreateAlertMessage(AlertMessageType.Success, "Success", "MapLocation is successfully updated.");
            return RedirectToAction("Index");
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
                var model = bll.db.MapLocations.Find(id);
                if (model != null)
                {
                    try
                    {
                        bll.db.MapLocations.Remove(model);
                        bll.db.SaveChanges();
                        CreateAlertMessage(AlertMessageType.Success, "Success", "MapLocation is successfully deleted.");
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

        #endregion


    }
}