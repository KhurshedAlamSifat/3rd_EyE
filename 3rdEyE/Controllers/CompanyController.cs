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
    public class CompanyController : BaseController
    {

        BLL_Company bll = new BLL_Company();

        // GET: Companies
        public ActionResult Index()
        {
            var list = bll.GetAllViewModels();
            return View(list);
        }


        // GET: Companies/View/5
        public ActionResult View(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var model = bll.db.Companies.Find(id);
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

        // GET: Companies/Create
        public ActionResult Create()
        {
            ViewBag.model = new Company();
            ViewBag.GroupOfCompanies = new SelectList(bll.db.GroupOfCompanies.Where(m => m.IsActive == true), "PK_GroupOfCompany", "Name");
            
            return View();
        }

        // POST: Companies/Create
        [HttpPost]
        public ActionResult Create(Company model)
        {
            string modelValidator = bll.IsValidModel_ToCreate(model);
            if (modelValidator == ValidationStatus.OK)
            {
                try
                {
                    var db_model = bll.FilterToDBModel(model);
                    bll.db.Companies.Add(db_model);
                    bll.db.SaveChanges();
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Company is successfully added.");
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
            
            ViewBag.GroupOfCompanies = new SelectList(bll.db.GroupOfCompanies.Where(goc => goc.IsActive == true), "PK_GroupOfCompany", "Name", model.FK_GroupOfCompany);
            return View();
        }

        // GET: Companies/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var model = bll.db.Companies.Find(id);
                if (model != null)
                {
                    ViewBag.model = model;
                    
                    ViewBag.GroupOfCompanies = new SelectList(bll.db.GroupOfCompanies.Where(goc => goc.IsActive == true), "PK_GroupOfCompany", "Name", model.FK_GroupOfCompany);
                    return View();
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(Company model)
        {
            string modelValidator = bll.IsValidModel_ToEdit(model);
            if (modelValidator == ValidationStatus.OK)
            {
                try
                {
                    var db_model = bll.FilterToDBModel(model);
                    bll.db.SaveChanges();
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Company is successfully edited.");
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
            
            ViewBag.GroupOfCompanies = new SelectList(bll.db.GroupOfCompanies.Where(goc => goc.IsActive == true), "PK_GroupOfCompany", "Name", model.FK_GroupOfCompany);
            return View();
        }

        // GET: Companies/Delete/5
        public ActionResult Delete(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var model = bll.db.Companies.Find(id);
                if (model != null)
                {
                    try
                    {
                        model.IsDeleted = true;
                        bll.db.SaveChanges();
                        CreateAlertMessage(AlertMessageType.Success, "Success", "Company is successfully deleted.");
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

        //# AJAX METHOD
        public JsonResult IsPranRFLGroup_ByGroupOfCompany_PK(Guid PK_GroupOfCompany)
        {
            var result = bll.db.GroupOfCompanies.Where(gop => gop.PK_GroupOfCompany == PK_GroupOfCompany && gop.IsPranRFLGroup == true).Any();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}