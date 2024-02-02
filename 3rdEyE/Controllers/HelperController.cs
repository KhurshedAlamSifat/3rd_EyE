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
    public class HelperController : BaseController
    {

        BLL_Helper bll = new BLL_Helper();

        // GET: Helpers
        public ActionResult Index()
        {
            var list = bll.GetAllViewModels();
            return View(list);
        }


        // GET: Helpers/View/5
        public ActionResult View(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var model = bll.db.Helpers.Find(id);
                if (model != null)
                {
                    var viewModel = bll.ConvertToViewModel(model);

                    ViewBag.viewModel = viewModel;
                    return View();
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        // GET: Helpers/Create
        public ActionResult Create()
        {
            ViewBag.model = new Helper();
            var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
            ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false  && m.PK_Depo != _invalidDepoPK).OrderBy(m => m.Name), "PK_Depo", "Name");
            
            return View();
        }

        // POST: Helpers/Create
        [HttpPost]
        public ActionResult Create(Helper model)
        {
            string modelValidator = bll.IsValidModel_ToCreate(model);
            if (modelValidator == ValidationStatus.OK)
            {
                try
                {
                    var db_model = bll.FilterToDBModel(model);
                    bll.db.Helpers.Add(db_model);
                    bll.db.SaveChanges();
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Helper is successfully added.");
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
            ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false  && m.PK_Depo != _invalidDepoPK).OrderBy(m => m.Name), "PK_Depo", "Name", model.FK_Depo);
            

            return View();
        }

        // GET: Helpers/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var model = bll.db.Helpers.Find(id);
                if (model != null)
                {
                    ViewBag.model = model;
                    ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false ).OrderBy(m => m.Name), "PK_Depo", "Name", model.FK_Depo);
                    
                    return View();
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        // POST: Helpers/Edit/5
        [HttpPost]
        public ActionResult Edit(Helper model)
        {
            string modelValidator = bll.IsValidModel_ToEdit(model);
            if (modelValidator == ValidationStatus.OK)
            {
                try
                {
                    var db_model = bll.FilterToDBModel(model);
                    bll.db.SaveChanges();
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Helper is successfully edited.");
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
            ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false  && m.PK_Depo != _invalidDepoPK).OrderBy(m => m.Name), "PK_Depo", "Name", model.FK_Depo);
            

            return View();
        }

        // GET: Helpers/Delete/5
        public ActionResult Delete(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var model = bll.db.Helpers.Find(id);
                if (model != null)
                {
                    try
                    {
                        model.IsDeleted = true;
                        bll.db.SaveChanges();
                        CreateAlertMessage(AlertMessageType.Success, "Success", "Helper is successfully deleted.");
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