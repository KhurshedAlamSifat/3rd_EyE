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
    public class ContructualAgentController : BaseController
    {
        BLL_ContructualAgent bll = new BLL_ContructualAgent();

        public ActionResult Index()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            List<VM_AppUser> list;
            list = bll.GetAllViewModels();
            return View(list);
        }
        public void ExportRequisitionAgentList()
        {
            List<VM_AppUser> list;
            list = bll.GetAllViewModels();
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=Agent_List.xls");
            Response.AddHeader("Content-Type", "application/vnd.ms-excel");

            //# Add Header Row
            Response.Output.Write("Full Name" + "\t");
            Response.Output.Write("Contructual Company" + "\t");
            Response.Output.Write("Agent Type" + "\t");
            Response.Output.Write("Contact Number" + "\t");
            Response.Output.Write("Email" + "\t");
            Response.Output.Write("PermissionAdd" + "\t");
            Response.Output.Write("Permission Eidt" + "\t");
            Response.Output.Write("Permission View" + "\t");
            Response.Output.Write("Permission Delete" + "\t");
            Response.Output.Write("Permission Admin" + "\t");
            Response.Output.Write("Contact Address" + "\t");
            Response.Output.WriteLine();

            foreach (var item in list)
            {
                Response.Output.Write(item.Model.FullName + "\t");
                if (item.Model.ContructualRequisitionCompany != null)
                {
                    Response.Output.Write(item.Model.ContructualRequisitionCompany.Name + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                Response.Output.Write(item.Model.AppUserType + "\t");
                Response.Output.Write(item.Model.ContactNumber + "\t");
                Response.Output.Write(item.Model.Email + "\t");

                if (item.Model.PermissionAdd == true)
                {
                    Response.Output.Write("Yes" + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Model.PermissionEdit == true)
                {
                    Response.Output.Write("Yes" + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Model.PermissionView == true)
                {
                    Response.Output.Write("Yes" + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Model.PermissionDelete == true)
                {
                    Response.Output.Write("Yes" + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Model.PermissionAdmin == true)
                {
                    Response.Output.Write("Yes" + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                Response.Output.Write(item.Model.ContactAddress + "\t");
                Response.Output.WriteLine();
            }
            Response.End();
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
                var model = bll.db.AppUsers.Find(id);
                if (model != null)
                {
                    var viewModel = bll.ConvertToViewModel(model);
                    return View(viewModel);
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
            ViewBag.model = new AppUser();
            ViewBag.ContructualCompanies = new SelectList(bll.db.ContructualRequisitionCompanies.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_ContructualRequisitionCompany", "Name");

            return View();
        }
        [HttpPost]
        public ActionResult Create(AppUser model)
        {
            string modelValidator = bll.IsValidModel_ToCreate(model);
            if (modelValidator == ValidationStatus.OK)
            {
                try
                {
                    var db_model = bll.FilterToDBModel(model);
                    bll.db.AppUsers.Add(db_model);

                    bll.db.SaveChanges();
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Agent is successfully added.");
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
            ViewBag.ContructualCompanies = new SelectList(bll.db.ContructualRequisitionCompanies.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_ContructualRequisitionCompany", "Name", model.FK_ContructualRequisitionCompany);

            return View();
        }
        [HttpPost]
        public ActionResult CreateContructualCompany_ByModal(ContructualRequisitionCompany model)

        {
            model.Name = model.Name.Trim().ToUpper();
            model.RegistrationNumber = string.IsNullOrEmpty(model.RegistrationNumber) ? null : model.RegistrationNumber.Trim().ToUpper();
            model.ContactNumber = string.IsNullOrEmpty(model.ContactNumber) ? null : model.ContactNumber.Trim();
            model.ContactAddress = string.IsNullOrEmpty(model.ContactAddress) ? null : model.ContactAddress.Trim();
            string modelValidator = "";
            if (bll.db.ContructualRequisitionCompanies.Where(m => m.Name == model.Name).Any())
            {
                modelValidator += "This name is already used by another contructual company. Please, use another name.";
            }
            if (modelValidator == "")
            {
                modelValidator = ValidationStatus.OK;
            }
            if (modelValidator == ValidationStatus.OK)
            {
                try
                {
                    model.PK_ContructualRequisitionCompany = Guid.NewGuid();
                    model.IsDeleted = false;
                    bll.db.ContructualRequisitionCompanies.Add(model);
                    bll.db.SaveChanges();
                    //CreateAlertMessage(AlertMessageType.Success, "Success", "Tansport company is successfully added.");
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
            return RedirectToAction("Create");
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
                var model = bll.db.AppUsers.Find(id);
                if (model != null)
                {
                    ViewBag.model = model;
                    ViewBag.ContructualCompanies = new SelectList(bll.db.ContructualRequisitionCompanies.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_ContructualRequisitionCompany", "Name", model.FK_ContructualRequisitionCompany);


                    return View();
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
        [HttpPost]
        public ActionResult Edit(AppUser model)
        {
            string modelValidator = bll.IsValidModel_ToEdit(model);
            if (modelValidator == ValidationStatus.OK)
            {
                try
                {
                    var db_model = bll.FilterToDBModel(model);
                    bll.db.SaveChanges();
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Agent is successfully edited.");
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
            ViewBag.ContructualCompanies = new SelectList(bll.db.ContructualRequisitionCompanies.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_ContructualRequisitionCompany", "Name", model.FK_ContructualRequisitionCompany);

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
                var model = bll.db.AppUsers.Find(id);
                if (model != null)
                {
                    try
                    {
                        model.IsDeleted = true;
                        bll.db.SaveChanges();
                        CreateAlertMessage(AlertMessageType.Success, "Success", "Agent is successfully deleted.");
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