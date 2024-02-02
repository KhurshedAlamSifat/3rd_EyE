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
    public class DriverController : BaseController
    {

        BLL_Driver bll = new BLL_Driver();

        public ActionResult Index()
        {
            var list = bll.GetAllViewModels();
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
                var model = bll.db.Drivers.Find(id);
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

        public ActionResult Create()
        {
            ViewBag.model = new Driver();
            var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
            ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && (!m.Category.Contains("Physical"))).OrderBy(m => m.Name), "PK_Depo", "Name");

            return View();
        }
        [HttpPost]
        public ActionResult Create(Driver model)
        {
            string modelValidator = bll.IsValidModel_ToCreate(model);
            if (modelValidator == ValidationStatus.OK)
            {
                try
                {
                    var db_model = bll.FilterToDBModel(model);
                    bll.db.Drivers.Add(db_model);
                    bll.db.SaveChanges();
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Driver is successfully added.");
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
        public ActionResult Add_ByModal(Driver model)
        {
            string modelValidator = bll.IsValidModel_ToCreate(model);
            if (modelValidator == ValidationStatus.OK)
            {
                try
                {
                    var db_model = bll.FilterToDBModel(model);
                    bll.db.Drivers.Add(db_model);
                    bll.db.SaveChanges();
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Driver is successfully added.");
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
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var model = bll.db.Drivers.Find(id);
                if (model != null)
                {
                    ViewBag.model = model;
                    var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
                    ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && (!m.Category.Contains("Physical")) && m.PK_Depo != _invalidDepoPK).OrderBy(m => m.Name), "PK_Depo", "Name", model.FK_Depo);


                    ViewBag.TestDateTime = model.LisenceRenewalDate.ToString();

                    return View();
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
        [HttpPost]
        public ActionResult Edit(Driver model)
        {
            string modelValidator = bll.IsValidModel_ToEdit(model);
            if (modelValidator == ValidationStatus.OK)
            {
                try
                {
                    var db_model = bll.FilterToDBModel(model);
                    bll.db.SaveChanges();
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Driver is successfully edited.");
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
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var model = bll.db.Drivers.Find(id);
                if (model != null)
                {
                    try
                    {
                        model.IsDeleted = true;
                        bll.db.SaveChanges();
                        CreateAlertMessage(AlertMessageType.Success, "Success", "Driver is successfully deleted.");
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