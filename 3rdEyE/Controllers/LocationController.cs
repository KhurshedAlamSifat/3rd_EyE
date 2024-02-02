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
    public class LocationController : BaseController
    {
        Dictionary<string, string> PRG_TypesDict = new Dictionary<string, string> { { "PRAN", "PRAN" }, { "RFL", "RFL" }, { "ALL", "ALL" } };
        Dictionary<string, string> LocationTypeDict = new Dictionary<string, string> {{"Depo","Depo"},
                                                                                      {"District","District"},
                                                                                      {"Factory","Factory"},
                                                                                      {"Office","Office"},
                                                                                      {"OS","OS"},
                                                                                      {"Workshop","Workshop"}};
        #region All
        public ActionResult Index(string LocationType)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var list = new List<Location>();

            var query = bll.db.Locations.AsEnumerable().Where(c => c.IsDeleted != true);

            //if (PRG_Type == null)
            //{
            //    query = query.Where(m => m.PRG_Type == null);
            //}
            //else if (PRG_Type != "all")
            //{
            //    query = query.Where(m => m.PRG_Type == PRG_Type);
            //}
            List<SelectListItem> PRG_TypeList = new List<SelectListItem>();
            PRG_TypeList.Add(new SelectListItem() { Value = "all", Text = "All" });
            PRG_TypeList.AddRange(PRG_TypesDict.AsEnumerable().Select(m => new SelectListItem { Value = m.Key, Text = m.Value }));

            if (LocationType == null)
            {
                query = query.Where(m => m.LocationType == null);
            }
            else if (LocationType != "all")
            {
                query = query.Where(m => m.LocationType == LocationType);
            }
            List<SelectListItem> Location_TypeList = new List<SelectListItem>();
            Location_TypeList.Add(new SelectListItem() { Value = "all", Text = "All" });
            Location_TypeList.AddRange(LocationTypeDict.AsEnumerable().Select(m => new SelectListItem { Value = m.Key, Text = m.Value }));
            ViewBag.Location_Type = new SelectList(Location_TypeList.OrderBy(m => m.Text), "Value", "Text", LocationType);

            if ((!string.IsNullOrEmpty(LocationType)))
            {
                list = query.ToList();
            }
            return View(list);
        }
        // GET: Location1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Location1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Location location)
        {
            location.PK_Location = Guid.NewGuid();  
            location.IsDeleted = false;
            location.CreatedAt = DateTime.Now;
            location.FK_CreatedByUser = CurrentUser.PK_User;
            
            bll.db.Locations.Add(location);
            bll.db.SaveChanges();
            return RedirectToAction("Index");

        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = bll.db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Location location)
        {
            if (ModelState.IsValid)
            {
                location.UpdatedAt = DateTime.Now;
                location.FK_UpdatedByUser= CurrentUser.PK_User;
                bll.db.Entry(location).State = EntityState.Modified;
                bll.db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(location);
        }
        public ActionResult View(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var model = bll.db.Locations.Find(id);
                if (model != null)
                {
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        public string IsValidModel_ToCreate(Location model)
        {
            string result = "";

            //# checks, name is unique
            if (bll.db.Locations.Where(c => c.Name.ToUpper() == model.Name.ToUpper().Trim()).Any())
            {
                result += "This Depo name is already used by another compnay. Please, use an unique name. ";

            }

            if (result == "")
            {
                result = ValidationStatus.OK;
            }
            return result;
        }
        public Location FilterToDBModel(Location model)
        {

            Location db_model;
            if (model.PK_Location.ToString() == "00000000-0000-0000-0000-000000000000")
            {
                db_model = new Location();
                db_model.PK_Location = Guid.NewGuid();
                db_model.IsDeleted = false;
                db_model.CreatedAt = DateTime.Now;
                db_model.FK_CreatedByUser = CommonClass.GetCurrentUser().PK_User;
            }
            else
            {
                db_model = bll.db.Locations.Find(model.PK_Location);
                db_model.UpdatedAt = DateTime.Now;
                db_model.FK_UpdatedByUser = CommonClass.GetCurrentUser().PK_User;
            }

            db_model.Name = model.Name.Trim();
            db_model.PRG_Type = model.PRG_Type;
            db_model.LocationType = model.LocationType;
            db_model.Latitude = !string.IsNullOrEmpty(model.Latitude) ? model.Latitude.Trim() : "";
            db_model.Longitude = !string.IsNullOrEmpty(model.Longitude) ? model.Longitude.Trim() : "";

            return db_model;
        }
        public string IsValidModel_ToEdit(Location model)
        {
            string result = "";

            //# checks, name is unique
            if (bll.db.Locations.Where(c => c.PK_Location != model.PK_Location && c.Name.ToUpper() == model.Name.ToUpper().Trim()).Any())
            {
                result += "This Depo name is already used by another compnay. Please, use an unique name. ";

            }

            if (result == "")
            {
                result = ValidationStatus.OK;
            }
            return result;
        }

        public ActionResult Delete(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var model = bll.db.Locations.Find(id);
                if (model != null)
                {
                    try
                    {
                        model.IsDeleted = true;
                        bll.db.SaveChanges();
                        CreateAlertMessage(AlertMessageType.Success, "Success", "Locationt is successfully deleted.");
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

        public JsonResult GetAppUserGate_ByLocation(Guid PK_Location)
        {
            var list = bll.db.AppUsers.AsEnumerable().Where(m => m.IsDeleted == false && (m.AppUserType == "Internal Gate Entry Device" || m.AppUserType == "Internal Parking Entry Device") && m.FK_Location == PK_Location).OrderBy(m => m.FullName)
                .Select(m =>
                new
                {
                    Value = m.PK_User.ToString(),
                    Text = m.FullName + " " + m.UniqueIDNumber
                })
                .ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion


    }
}