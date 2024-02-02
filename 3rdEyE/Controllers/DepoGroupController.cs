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
    public class DepoGroupController : BaseController
    {
        BLL_DepoGroup bll = new BLL_DepoGroup();
        public ActionResult Index()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            var list = bll.db.DepoGroups.AsEnumerable().Where(m => accessibleDepoes.Contains(m.FK_Depo) && m.IsDeleted == false).Select(c => c).ToList();
            return View(list);
        }

        public ActionResult View(Guid id)
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
                var model = bll.db.DepoGroups.Find(id);
                if (model != null)
                {
                    var viewModel = model;

                    ViewBag.viewModel = viewModel;
                    return View();
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
            ViewBag.model = new DepoGroup();
            var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
            ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && (!m.Category.Contains("Physical"))).OrderBy(m => m.Name), "PK_Depo", "Name");

            return View();
        }

        [HttpPost]
        public ActionResult Create(DepoGroup model)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            string modelValidator = bll.IsValidModel_ToCreate(model);
            if (modelValidator == ValidationStatus.OK)
            {
                try
                {
                    var db_model = bll.FilterToDBModel(model);
                    bll.db.DepoGroups.Add(db_model);
                    bll.db.SaveChanges();
                    CreateAlertMessage(AlertMessageType.Success, "Success", "DepoGroup is successfully added.");
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
            ViewBag.model = model;
            var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
            ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && (!m.Category.Contains("Physical")) && m.PK_Depo != _invalidDepoPK).OrderBy(m => m.Name), "PK_Depo", "Name", model.FK_Depo);


            return View();
        }

        [HttpPost]
        public ActionResult Add_ByModal(FormCollection form)
        {
            var model = new DepoGroup();
            model.Name = form["Name"].Trim();
            model.FK_Depo = Guid.Parse(form["FK_Depo_2"]);
            string modelValidator = bll.IsValidModel_ToCreate(model);
            if (modelValidator == ValidationStatus.OK)
            {
                try
                {
                    var db_model = bll.FilterToDBModel(model);
                    bll.db.DepoGroups.Add(db_model);
                    bll.db.SaveChanges();
                    CreateAlertMessage(AlertMessageType.Success, "Success", "User Sub-Group is successfully added.");
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
            var url = Request.ServerVariables["HTTP_REFERER"];

            return Redirect(url);
        }


        public ActionResult Edit(Guid? id)
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
                var model = bll.db.DepoGroups.Find(id);
                if (model != null)
                {
                    ViewBag.model = model;
                    var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
                    ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && (!m.Category.Contains("Physical")) && m.PK_Depo != _invalidDepoPK).OrderBy(m => m.Name), "PK_Depo", "Name", model.FK_Depo);

                    return View();
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
        [HttpPost]
        public ActionResult Edit(DepoGroup model)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            string modelValidator = bll.IsValidModel_ToEdit(model);
            if (modelValidator == ValidationStatus.OK)
            {
                try
                {
                    var db_model = bll.FilterToDBModel(model);
                    bll.db.SaveChanges();
                    CreateAlertMessage(AlertMessageType.Success, "Success", "DepoGroup is successfully edited.");
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
            ViewBag.model = model;
            var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
            ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && (!m.Category.Contains("Physical")) && m.PK_Depo != _invalidDepoPK).OrderBy(m => m.Name), "PK_Depo", "Name", model.FK_Depo);


            return View();
        }

        public ActionResult Delete(Guid id)
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
                var model = bll.db.DepoGroups.Find(id);
                if (model != null)
                {
                    try
                    {
                        model.IsDeleted = true;
                        bll.db.SaveChanges();
                        CreateAlertMessage(AlertMessageType.Success, "Success", "DepoGroup is successfully deleted.");
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
        public JsonResult GetDepoGroupBy_FK_Depo(Guid FK_Depo)
        {
            var list = bll.db.DepoGroups.Where(m => m.FK_Depo == FK_Depo).Select(m => new
            {
                m.PK_DepoGroup,
                m.Name
            }).OrderBy(m => m.Name).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}