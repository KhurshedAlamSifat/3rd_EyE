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
    public class TransportAgentController : BaseController
    {
        BLL_TransportAgent bll = new BLL_TransportAgent();

        public ActionResult Index()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            List<AppUser> list;
            list = bll.db.AppUsers.AsEnumerable().Where(c => c.IsDeleted == false && (c.AppUserType == "External Transport Agent")).ToList();
            return View(list);
        }
        public void ExportRequisitionAgentList()
        {
            List<AppUser> list;
            list = bll.db.AppUsers.AsEnumerable().Where(c => c.IsDeleted == false && (c.AppUserType == "External Transport Agent")).ToList();
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=Agent_List.xls");
            Response.AddHeader("Content-Type", "application/vnd.ms-excel");

            //# Add Header Row
            Response.Output.Write("Full Name" + "\t");
            Response.Output.Write("Transport Company" + "\t");
            Response.Output.Write("Agent Type" + "\t");
            Response.Output.Write("Contact Number" + "\t");
            Response.Output.Write("Email" + "\t");
            Response.Output.Write("Depo" + "\t");
            Response.Output.Write("PermissionAdd" + "\t");
            Response.Output.Write("Permission Eidt" + "\t");
            Response.Output.Write("Permission View" + "\t");
            Response.Output.Write("Permission Delete" + "\t");
            Response.Output.Write("Permission Admin" + "\t");
            Response.Output.Write("Contact Address" + "\t");
            Response.Output.WriteLine();

            foreach (var item in list)
            {
                Response.Output.Write(item.FullName + "\t");
                if (item.TransportCompany!= null)
                {
                    Response.Output.Write(item.TransportCompany.Name + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                Response.Output.Write(item.AppUserType + "\t");
                Response.Output.Write(item.ContactNumber + "\t");
                Response.Output.Write(item.Email + "\t");
                if (item.Depo != null)
                {
                    Response.Output.Write(item.Depo.Name + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }

                if (item.PermissionAdd == true)
                {
                    Response.Output.Write("Yes" + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.PermissionEdit == true)
                {
                    Response.Output.Write("Yes" + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.PermissionView == true)
                {
                    Response.Output.Write("Yes" + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.PermissionDelete == true)
                {
                    Response.Output.Write("Yes" + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.PermissionAdmin == true)
                {
                    Response.Output.Write("Yes" + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                Response.Output.Write(item.ContactAddress + "\t");
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
            var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
            ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(vbm => vbm.PK_Depo != _invalidDepoPK).OrderBy(m => m.Name), "PK_Depo", "Name");
            ViewBag.TransportCompanies = new SelectList(bll.db.TransportCompanies.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_TransportCompany", "Name");

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
            var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
            ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(vbm => vbm.PK_Depo != _invalidDepoPK).OrderBy(m => m.Name), "PK_Depo", "Name", model.FK_Depo);
            ViewBag.TransportCompanies = new SelectList(bll.db.TransportCompanies.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_TransportCompany", "Name", model.FK_TransportCompany);

            return View();
        }
        [HttpPost]
        public ActionResult CreateTransportCompany_ByModal(TransportCompany model)

        {
            model.Name = model.Name.Trim().ToUpper();
            model.RegistrationNumber = string.IsNullOrEmpty(model.RegistrationNumber) ? null : model.RegistrationNumber.Trim().ToUpper();
            model.ContactNumber = string.IsNullOrEmpty(model.ContactNumber) ? null : model.ContactNumber.Trim();
            model.ContactAddress = string.IsNullOrEmpty(model.ContactAddress) ? null : model.ContactAddress.Trim();
            model.OwnerName = string.IsNullOrEmpty(model.OwnerName) ? null : model.OwnerName.Trim();
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
                    model.PK_TransportCompany = Guid.NewGuid();
                    model.IsDeleted = false;
                    bll.db.TransportCompanies.Add(model);
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
                var model = bll.db.AppUsers.Find(id);
                if (model != null)
                {
                    ViewBag.model = model;
                    var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
                    ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(vbm => vbm.PK_Depo != _invalidDepoPK).OrderBy(m => m.Name), "PK_Depo", "Name", model.FK_Depo);
                    ViewBag.TransportCompanies = new SelectList(bll.db.TransportCompanies.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_TransportCompany", "Name", model.FK_TransportCompany);


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
            var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
            ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(vbm => vbm.PK_Depo != _invalidDepoPK).OrderBy(m => m.Name), "PK_Depo", "Name", model.FK_Depo);
            ViewBag.TransportCompanies = new SelectList(bll.db.TransportCompanies.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_TransportCompany", "Name", model.FK_TransportCompany);

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

        #region
        public ActionResult ManageRequisitionAgentProposableDepo()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            List<AppUser> list;
            list = bll.db.AppUsers.ToList();
            ViewBag.AppUsers = new SelectList(list.OrderByDescending(m => m.RowSerial).Where(m => m.AppUserType == "External Transport Agent").OrderBy(m => m.FullName), "PK_User", "ContactNumber");
            return View();
        }
        public JsonResult GetProposableDepoListOfRequisitionAgent(Guid FK_RequisitionAgent)
        {
            var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
            var list = (from depo in bll.db.Depoes.AsEnumerable()
                        where depo.PK_Depo != _invalidDepoPK
                        select new
                        {
                            FK_Depo = depo.PK_Depo,
                            DepoName = depo.Name,
                            IsProposable = bll.db.RequisitionAgentProposedDepoes.Where(m => m.FK_Depo == depo.PK_Depo && m.FK_RequisitionAgent == FK_RequisitionAgent && m.WillPropose == true).Any()
                        }
                        ).OrderBy(m => m.DepoName).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ManageRequisitionAgentProposableDepo(FormCollection form)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var FK_RequisitionAgent = Guid.Parse(form["FK_RequisitionAgent"]);
            var PermittedDepoes = form["PermittedDepoes"];
            try
            {

                var PermittedDepoe_FKs = PermittedDepoes.Split(',');
                var oldList = bll.db.RequisitionAgentProposedDepoes.Where(m => m.FK_RequisitionAgent == FK_RequisitionAgent).ToList();
                foreach (var item in oldList)
                {
                    //# Update existing permission
                    if (PermittedDepoe_FKs.Contains(item.FK_Depo.ToString()))
                    {
                        item.WillPropose = true;
                    }
                    else
                    {
                        item.WillPropose = false;
                    }
                }
                if (PermittedDepoe_FKs.Count() > 0)
                {

                    foreach (var FK in PermittedDepoe_FKs)
                    {
                        if (FK == "")
                        {
                            break;
                        }
                        //# Add new parmision
                        if (!oldList.Where(m => m.FK_Depo.ToString() == FK).Any())
                        {
                            bll.db.RequisitionAgentProposedDepoes.Add(
                                    new RequisitionAgentProposedDepo()
                                    {
                                        FK_RequisitionAgent = FK_RequisitionAgent,
                                        FK_Depo = Guid.Parse(FK),
                                        WillPropose = true
                                    });
                        }
                    }
                }


                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "Permission is successfully updated.");
                return RedirectToAction("ManageRequisitionAgentProposableDepo");
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                return RedirectToAction("ManageRequisitionAgentProposableDepo");
            }
        }
        #endregion
    }
}