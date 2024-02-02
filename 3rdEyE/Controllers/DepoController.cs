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
    public class DepoController : BaseController
    {
        Dictionary<string, string> PRG_TypesDict = new Dictionary<string, string> { { "PRAN", "PRAN" }, { "RFL", "RFL" }, { "CS", "CS" } };
        Dictionary<string, string> Depo_CategoryDict = new Dictionary<string, string> { { "Depot", "Depot" }, { "Factory", "Factory" },
            { "Dealer", "Dealer" }, { "Tasty/Mithai", "Tasty/Mithai" },
            { "Store/Production", "Store/Production" }, { "Textile", "Textile" },
            { "Bread", "Bread" }, { "Dairy", "Dairy" }, { "GPT", "GPT" }, { "Showroom", "Showroom"},
            { "Others", "Others" } };
        BLL_Depo bll = new BLL_Depo();
        #region Common
        public ActionResult EditBorders(Guid id)
        {
            var model = bll.db.Depoes.Where(m => m.PK_Depo == id).FirstOrDefault();
            return View(model);
        }
        [HttpPost]
        public ActionResult EditBorders(FormCollection form)
        {
            try
            {
                var _Fk_Depo = form["FK_Depo"];
                var FK_Depo = Guid.Parse(_Fk_Depo);
                var oldBorders = bll.db.DepoBorders.Where(m => m.FK_Depo == FK_Depo).ToList();
                if (oldBorders.Count > 0)
                {
                    bll.db.DepoBorders.RemoveRange(oldBorders);
                }
                var newBorders = new List<DepoBorder>();
                var LatLongs_string = form["LatLongs"];
                var LatLongs_array = LatLongs_string.Split('#');
                for (int i = 0; i < LatLongs_array.Count(); i++)
                {
                    var lat_string = LatLongs_array[i].Split('*')[0];
                    var lng_string = LatLongs_array[i].Split('*')[1];
                    newBorders.Add(
                        new DepoBorder()
                        {
                            FK_Depo = FK_Depo,
                            Latitude = Convert.ToDouble(lat_string),
                            Longitude = Convert.ToDouble(lng_string)
                        }
                        );
                }
                bll.db.DepoBorders.AddRange(newBorders);
                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "Border Added");
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
            }
            return RedirectToAction("EditBorders", new { id = form["FK_Depo"] });
        }
        #endregion

        #region All
        public ActionResult Index(string PRG_Type, string Depo_Category)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var list = new List<Depo>();

            var query = bll.db.Depoes.AsEnumerable().Where(c => !c.Category.Contains("Physical") && c.IsDeleted != true);

            if (PRG_Type == null)
            {
                query = query.Where(m => m.PRG_Type == null);
            }
            else if (PRG_Type != "all")
            {
                query = query.Where(m => m.PRG_Type == PRG_Type);
            }
            List<SelectListItem> PRG_TypeList = new List<SelectListItem>();
            PRG_TypeList.Add(new SelectListItem() { Value = "all", Text = "All" });
            PRG_TypeList.AddRange(PRG_TypesDict.AsEnumerable().Select(m => new SelectListItem { Value = m.Key, Text = m.Value }));
            ViewBag.PRG_Type = new SelectList(PRG_TypeList.OrderBy(m => m.Text), "Value", "Text", PRG_Type);

            if (Depo_Category == null)
            {
                query = query.Where(m => m.Category == null);
            }
            else if (Depo_Category != "all")
            {
                query = query.Where(m => m.Category == Depo_Category);
            }
            List<SelectListItem> Depo_CategoryList = new List<SelectListItem>();
            Depo_CategoryList.Add(new SelectListItem() { Value = "all", Text = "All" });
            Depo_CategoryList.AddRange(Depo_CategoryDict.AsEnumerable().Select(m => new SelectListItem { Value = m.Key, Text = m.Value }));
            ViewBag.Depo_Category = new SelectList(Depo_CategoryList.OrderBy(m => m.Text), "Value", "Text", Depo_Category);

            if ((!string.IsNullOrEmpty(PRG_Type)) || (!string.IsNullOrEmpty(Depo_Category)))
            {
                list = query.ToList();
            }
            return View(list);
        }

        public ActionResult View(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var model = bll.db.Depoes.Find(id);
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

        public ActionResult Create()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            if (CurrentUser.PRG_Type == "ALL")
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", "Your PRG_Type is ALL. Only PRAN/RFL/CS user can creatre User Group");
                return Redirect("/Home/Index");
            }
            ViewBag.model = new Depo();
            ViewBag.PRG_TypesDict = new SelectList(PRG_TypesDict, "Key", "Value", CurrentUser.PRG_Type);
            ViewBag.Depo_CategoryDict = new SelectList(Depo_CategoryDict, "Key", "Value");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Depo model)
        {
            string modelValidator = bll.IsValidModel_ToCreate(model);

            if (modelValidator == ValidationStatus.OK)
            {
                try
                {
                    var db_model = new Depo();
                    db_model.PK_Depo = Guid.NewGuid();
                    db_model.IsDeleted = false;
                    db_model.Name = model.Name.Trim();
                    db_model.PRG_Type = CurrentUser.PRG_Type;
                    db_model.Category = model.Category;
                    db_model.Latitude = !string.IsNullOrEmpty(model.Latitude) ? model.Latitude.Trim() : "";
                    db_model.Longitude = !string.IsNullOrEmpty(model.Longitude) ? model.Longitude.Trim() : "";
                    db_model.CreatedAt = DateTime.Now;
                    db_model.FK_CreatedByUser = CurrentUser.PK_User;

                    bll.db.Depoes.Add(db_model);
                    bll.db.SaveChanges();
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Depot is successfully added.");
                    return RedirectToAction("Index");
                }
                catch (Exception exception)
                {
                    ViewBag.model = model;
                    ViewBag.PRG_TypesDict = new SelectList(PRG_TypesDict, "Key", "Value", model.PRG_Type);

                    CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                }
            }
            else
            {
                ViewBag.model = model;
                ViewBag.PRG_TypesDict = new SelectList(PRG_TypesDict, "Key", "Value", model.PRG_Type);
                ViewBag.Depo_CategoryDict = new SelectList(Depo_CategoryDict, "Key", "Value");
                CreateAlertMessage(AlertMessageType.Danger, "Validation Failure", modelValidator);
            }


            return View();
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var model = bll.db.Depoes.Find(id);
                if (model != null)
                {
                    ViewBag.model = model;
                    ViewBag.PRG_TypesDict = new SelectList(PRG_TypesDict, "Key", "Value", model.PRG_Type);
                    ViewBag.Depo_CategoryDict = new SelectList(Depo_CategoryDict, "Key", "Value", model.Category);
                    return View();
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        public ActionResult Edit(Depo model)
        {
            string modelValidator = bll.IsValidModel_ToEdit(model);
            if (modelValidator == ValidationStatus.OK)
            {
                try
                {
                    var db_model = bll.db.Depoes.Find(model.PK_Depo);
                    db_model.IsDeleted = false;
                    db_model.Name = model.Name.Trim();
                    //db_model.PRG_Type = CurrentUser.PRG_Type;
                    db_model.Category = model.Category;
                    db_model.Latitude = !string.IsNullOrEmpty(model.Latitude) ? model.Latitude.Trim() : "";
                    db_model.Longitude = !string.IsNullOrEmpty(model.Longitude) ? model.Longitude.Trim() : "";
                    db_model.UpdatedAt = DateTime.Now;
                    db_model.FK_UpdatedByUser = CurrentUser.PK_User;
                    bll.db.SaveChanges();

                    CreateAlertMessage(AlertMessageType.Success, "Success", "Depot is successfully edited.");
                    return RedirectToAction("Index");
                }
                catch (Exception exception)
                {
                    ViewBag.model = model;
                    ViewBag.PRG_TypesDict = new SelectList(PRG_TypesDict, "Key", "Value", model.PRG_Type);
                    ViewBag.Depo_CategoryDict = new SelectList(Depo_CategoryDict, "Key", "Value", model.Category);

                    CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                }
            }
            else
            {
                ViewBag.model = model;
                ViewBag.PRG_TypesDict = new SelectList(PRG_TypesDict, "Key", "Value", model.PRG_Type);
                ViewBag.Depo_CategoryDict = new SelectList(Depo_CategoryDict, "Key", "Value", model.Category);

                CreateAlertMessage(AlertMessageType.Danger, "Validation Failure", modelValidator);
            }
            ViewBag.model = model;


            return View();
        }

        public ActionResult Delete(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var model = bll.db.Depoes.Find(id);
                if (model != null)
                {
                    try
                    {
                        model.IsDeleted = true;
                        bll.db.SaveChanges();
                        CreateAlertMessage(AlertMessageType.Success, "Success", "Depot is successfully deleted.");
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

        public JsonResult GetAccessibleDepoByCategory(string Category)
        {
            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            var list = bll.db.Depoes.Where(m => m.IsDeleted == false && accessibleDepoes.Contains(m.PK_Depo) && m.Category == Category).Select(m => new { m.PK_Depo, m.Name }).OrderBy(m => m.Name).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDepo_ByPRG_Type(string PRG_Type)
        {
            var list = bll.db.Depoes.Where(m => m.IsDeleted == false && m.PRG_Type == PRG_Type).Select(m =>
                    new
                    {
                        Value = m.PK_Depo,
                        Text = m.Name
                    }
                ).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}