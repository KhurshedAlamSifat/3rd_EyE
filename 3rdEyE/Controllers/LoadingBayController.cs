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
    public class LoadingBayController : BaseController
    {
        Dictionary<string, string> PRG_TypesDict = new Dictionary<string, string> { { "PRAN", "PRAN" }, { "RFL", "RFL" }, { "CS", "CS" } };
        #region All
        public ActionResult Index(Guid? FK_Location, string PRG_Type)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var list = new List<LoadingBay>();

            var query = bll.db.LoadingBays.AsEnumerable().Where(c => c.IsDeleted != true);

            //PRG_Type
            if (PRG_Type != null)
            {
                query = query.Where(m => m.LocationBuilding.PRG_Type == PRG_Type);
            }
            ViewBag.PRG_TypesDict = new SelectList(PRG_TypesDict, "Key", "Value", PRG_Type);

            //FK_Location
            if (FK_Location != null)
            {
                query = query.Where(m => m.LocationBuilding.FK_Location == FK_Location);
            }
            ViewBag.LocationList = new SelectList(bll.db.Locations.Where(m => m.IsDeleted != true && (m.LocationType == "Factory" || m.LocationType == "Depo" || m.LocationType == "Office")).OrderBy(m => m.Name), "PK_Location", "Name", FK_Location);

            list = query.ToList();
            return View(list);
        }

        public ActionResult Create()
        {
            var model = new LoadingBay();

            ViewBag.LocationBuildings = new SelectList(bll.db.LocationBuildings.Where(m => m.IsDeleted != true && m.PRG_Type == CurrentUser.PRG_Type).OrderBy(m => m.Name), "PK_LocationBuilding", "Name", model.FK_LocationBuilding);

            return View(model);
        }
        [HttpPost]
        public ActionResult Create(LoadingBay model)
        {
            try
            {
                var db_model = new LoadingBay();
                db_model.IsDeleted = false;

                db_model.Name = model.Name;
                db_model.FK_LocationBuilding = model.FK_LocationBuilding;

                db_model.CreatedAt = DateTime.Now;
                db_model.FK_CreatedByUser = CurrentUser.PK_User;

                bll.db.LoadingBays.Add(db_model);
                bll.db.SaveChanges();

                CreateAlertMessage(AlertMessageType.Success, "Success", "LoadingBay is successfully created.");
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", "Location Building could not be created.");
                ViewBag.LocationBuildings = new SelectList(bll.db.LocationBuildings.Where(m => m.IsDeleted != true && m.PRG_Type == CurrentUser.PRG_Type).OrderBy(m => m.Name), "PK_LocationBuilding", "Name", model.FK_LocationBuilding);
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
                var model = bll.db.LoadingBays.Find(id);
                if (model != null)
                {
                    ViewBag.LocationBuildings = new SelectList(bll.db.LocationBuildings.Where(m => m.IsDeleted != true && m.PRG_Type == CurrentUser.PRG_Type).OrderBy(m => m.Name), "PK_LocationBuilding", "Name", model.FK_LocationBuilding);
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
        [HttpPost]
        public ActionResult Edit(LoadingBay model)
        {
            try
            {
                var db_model = bll.db.LoadingBays.Find(model.PK_LoadingBay);

                db_model.Name = model.Name;
                db_model.FK_LocationBuilding = model.FK_LocationBuilding;

                db_model.UpdatedAt = DateTime.Now;
                db_model.FK_UpdatedByUser = CurrentUser.PK_User;

                bll.db.Entry(db_model).State = EntityState.Modified;
                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "LoadingBay is successfully updated.");
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", "Location Building could not be updated.");
                ViewBag.LocationBuildings = new SelectList(bll.db.LocationBuildings.Where(m => m.IsDeleted != true && m.PRG_Type == CurrentUser.PRG_Type).OrderBy(m => m.Name), "PK_LocationBuilding", "Name", model.FK_LocationBuilding);
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
                var model = bll.db.LoadingBays.Find(id);
                if (model != null)
                {
                    try
                    {
                        model.IsDeleted = true;
                        model.DeletedAt = DateTime.Now;
                        model.FK_DeletedByUser = CurrentUser.PK_User;
                        bll.db.SaveChanges();
                        CreateAlertMessage(AlertMessageType.Success, "Success", "LoadingBay is successfully deleted.");
                        return RedirectToAction("Index");
                    }
                    catch (Exception exception)
                    {
                        CreateAlertMessage(AlertMessageType.Warning, "Warning", "LoadingBay could not be deleted.");
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        public JsonResult GetLoadingBayBy_FK_LocationBuilding(Int64 FK_LocationBuilding)
        {
            var list = bll.db.LoadingBays.Where(m => m.IsDeleted != true && m.FK_LocationBuilding == FK_LocationBuilding).Select(m => new { m.PK_LoadingBay, m.Name }).ToList();
        return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}