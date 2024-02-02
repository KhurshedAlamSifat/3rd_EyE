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
using System.IO;
using _3rdEyE.BLLs;

namespace _3rdEyE.Controllers
{
    public class ContructualRequisitionController : BaseController
    {
        BLL_ContructualRequisition bll = new BLL_ContructualRequisition();
        //Dictionary<string, string> VehicleTypesDict = new Dictionary<string, string> { { "Ambulance", "Ambulance" }, { "Bus", "Bus" }, { "Cargo Truck", "Cargo Truck" }, { "Cargo Truck - Open", "Cargo Truck - Open" }, { "Cargo VAN", "Cargo VAN" }, { "Open VAN", "Open VAN" }, { "Concrete Mixer", "Concrete Mixer" }, { "Covered Van", "Covered Van" }, { "Delivery Van", "Delivery Van" }, { "Milk Tanker", "Milk Tanker" }, { "Mini Bus", "Mini Bus" }, { "Mini Truck", "Mini Truck" }, { "Mobile Crance", "Mobile Crance" }, { "Motor Car", "Motor Car" }, { "OMNI Bus", "OMNI Bus" }, { "Pickup", "Pickup" }, { "Refrigerator Van", "Refrigerator Van" }, { "Tank Lorry", "Tank Lorry" }, { "Tipper", "Tipper" }, { "Trailers", "Trailers" }, { "Water Tanker", "Water Tanker" } };
        //Dictionary<double, string> VehicleCapacityDict = new Dictionary<double, string> { { 0.8, "0.8 Ton" }, { 1, "1 Ton" }, { 1.5, "1.5 Tons" }, { 2, "2 Tons" }, { 3, "3 Tons" }, { 5, "5 Tons" }, { 7, "7 Tons" }, { 12, "12 Tons" }, { 20, "20 Tons" } };

        public ActionResult ContructualRequisitionDetailList_ExternalAgent()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var list = bll.db.ContructualRequisitionDetails.Where(m => m.ContructualRequisition.FK_ContructualRequisitionCompany == CurrentUser.FK_ContructualRequisitionCompany && m.IsDeleted == false).ToList();
            return View(list);
        }

        public ActionResult ContructualRequisitionDetailEntryList_ApproverView()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var surpervisedContructualCompanies = CurrentUser.AppUserSurpervisedContructualCompanies.Select(cc => cc.FK_ContructualRequisitionCompany).ToList();
            var list = bll.db.ContructualRequisitionDetailEntries.Where(m => m.Status == 0 && surpervisedContructualCompanies.Contains(m.ContructualRequisitionDetail.ContructualRequisition.FK_ContructualRequisitionCompany)).ToList();
            return View(list);
        }

        public ActionResult Create()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            ViewBag.VehicleTypeLayersDict = bll.db.RequisitionVehicleTypes.Select(m => new { Key = m.Layer1 + "#" + m.Layer2 + "#" + m.Layer3, Value = m.Layer1_english + " : " + m.Layer2_english + " : " + m.Layer3_english }).ToDictionary(m => m.Key, m => m.Value);
            //ViewBag.VehicleTypesDict = VehicleTypesDict.ToDictionary(m => m.Key, m => m.Value);
            //ViewBag.VehicleCapacityDict = VehicleCapacityDict;
            ViewBag.ContructualRequisitionCompanies = new SelectList(bll.db.ContructualRequisitionCompanies.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_ContructualRequisitionCompany", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection form)
        {
            ContructualRequisition model = new ContructualRequisition();
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            try
            {
                model.PK_ContructualRequisition = Guid.NewGuid();

                model.FK_RequisitionAgent = CurrentUser.PK_User;

                model.FK_ContructualRequisitionCompany = Guid.Parse(form["FK_ContructualRequisitionCompany"]);
                model.ContructAcitivatingDate = Convert.ToDateTime(form["ContructAcitivatingDate"]);
                model.ContructDeactivatingDate = Convert.ToDateTime(form["ContructDeactivatingDate"]);
                model.REF_FK_Depo = CurrentUser.Depo.PK_Depo;
                model.CreatedAt = DateTime.Now;

                var contructualRequisitionDetailList = new List<ContructualRequisitionDetail>();
                var rowCount = Convert.ToInt32(form["rowCount"]);
                for (int i = 1; i <= rowCount; i++)
                {
                    if (!string.IsNullOrEmpty(form["VehicleTypeLayer_" + i])
                    && !string.IsNullOrEmpty(form["StartingLocation_" + i])
                    && !string.IsNullOrEmpty(form["FinishingLocation_" + i])
                    && !string.IsNullOrEmpty(form["PricePerVehicle_" + i]))
                    {
                        var contructualRequisitionDetail = new ContructualRequisitionDetail();
                        contructualRequisitionDetail.PK_ContructualRequisitionDetail = Guid.NewGuid();
                        contructualRequisitionDetail.FK_ContructualRequisition = model.PK_ContructualRequisition;
                        contructualRequisitionDetail.IsDeleted = false;
                        var VehicleTypeLayer = form["VehicleTypeLayer_" + i];
                        contructualRequisitionDetail.VehicleTypeLayer1 = VehicleTypeLayer.Split('#')[0];
                        contructualRequisitionDetail.VehicleTypeLayer1_english = bll.db.RequisitionVehicleTypes.Where(m => m.Layer1 == contructualRequisitionDetail.VehicleTypeLayer1).Select(m => m.Layer1_english).FirstOrDefault();
                        contructualRequisitionDetail.VehicleTypeLayer1_bangla = bll.db.RequisitionVehicleTypes.Where(m => m.Layer1 == contructualRequisitionDetail.VehicleTypeLayer1).Select(m => m.Layer1_bangla).FirstOrDefault();

                        contructualRequisitionDetail.VehicleTypeLayer2 = VehicleTypeLayer.Split('#')[1];
                        contructualRequisitionDetail.VehicleTypeLayer2_english = bll.db.RequisitionVehicleTypes.Where(m => m.Layer2 == contructualRequisitionDetail.VehicleTypeLayer2).Select(m => m.Layer2_english).FirstOrDefault();
                        contructualRequisitionDetail.VehicleTypeLayer2_bangla = bll.db.RequisitionVehicleTypes.Where(m => m.Layer2 == contructualRequisitionDetail.VehicleTypeLayer2).Select(m => m.Layer2_bangla).FirstOrDefault();

                        contructualRequisitionDetail.VehicleTypeLayer3 = VehicleTypeLayer.Split('#')[2];
                        contructualRequisitionDetail.VehicleTypeLayer3_english = bll.db.RequisitionVehicleTypes.Where(m => m.Layer3 == contructualRequisitionDetail.VehicleTypeLayer3).Select(m => m.Layer3_english).FirstOrDefault();
                        contructualRequisitionDetail.VehicleTypeLayer3_bangla = bll.db.RequisitionVehicleTypes.Where(m => m.Layer3 == contructualRequisitionDetail.VehicleTypeLayer3).Select(m => m.Layer3_bangla).FirstOrDefault();

                        contructualRequisitionDetail.StartingLocation = form["StartingLocation_" + i];
                        contructualRequisitionDetail.FinishingLocation = form["FinishingLocation_" + i];
                        contructualRequisitionDetail.PricePerVehicle = Convert.ToInt64(form["PricePerVehicle_" + i]);
                        contructualRequisitionDetailList.Add(contructualRequisitionDetail);
                    }
                }


                bll.db.ContructualRequisitions.Add(model);
                bll.db.ContructualRequisitionDetails.AddRange(contructualRequisitionDetailList);

                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "Contructual Requisition is successfully added.");
                return RedirectToAction("Create");
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
            }
            ViewBag.VehicleTypeLayersDict = new SelectList(bll.db.RequisitionVehicleTypes.Select(m => new { Key = m.Layer1 + "#" + m.Layer2 + "#" + m.Layer3, Value = m.Layer1_english + " : " + m.Layer2_english + " : " + m.Layer3_english }).OrderBy(m => m.Value), "Key", "Value");
            ViewBag.ContructualRequisitionCompanies = new SelectList(bll.db.ContructualRequisitionCompanies.Where(m => m.IsDeleted == false), "PK_ContructualRequisitionCompany", "Name", model.FK_ContructualRequisitionCompany);
            return View(model);
        }

        [HttpPost]
        public ActionResult Create_ContructualRequisitionDetailEntry_ByModal(ContructualRequisitionDetailEntry model)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            try
            {
                var db_model = new ContructualRequisitionDetailEntry();
                db_model.PK_ContructualRequisitionDetailEntry = Guid.NewGuid();
                db_model.FK_ContructualRequisitionDetail = model.FK_ContructualRequisitionDetail;
                db_model.VehicleCount = model.VehicleCount;
                db_model.ExecutionDate = model.ExecutionDate;
                db_model.Status = 0;
                db_model.FK_AppUser_AppliedBy = CurrentUser.PK_User;
                db_model.AppliedAt = DateTime.Now;
                bll.db.ContructualRequisitionDetailEntries.Add(model);

                //# notification for concerned internal agent
                List<Guid> FK_FirebaseAppUser = new List<Guid>();
                var notificatoinList = new List<RequisitionAgentNotification>();
                if (CurrentUser.ContructualRequisitionCompany != null)
                {
                    var contractualCompanyInternalAgents = bll.db.AppUsers.Where(m => m.IsDeleted == false && m.FK_Depo == CurrentUser.FK_Depo && m.PK_User != CurrentUser.PK_User && (m.AppUserType == "Internal Transport Agent" || m.AppUserType == "External Transport Agent")).ToList();
                    var contructualRequisitionDetail = bll.db.ContructualRequisitionDetails.Where(m => m.PK_ContructualRequisitionDetail == db_model.FK_ContructualRequisitionDetail).FirstOrDefault();
                    var NumbersToSMS = "";
                    foreach (var agent in contractualCompanyInternalAgents)
                    {
                        var notification = new RequisitionAgentNotification()
                        {
                            FK_RequisitionAgent = agent.PK_User,
                            Status = 0,
                            Title = "Cotractual vehicle request by- " + CurrentUser.FullName,
                            ViewLink = "/ContructualRequisition/Approve_ContructualRequisitionDetailEntry?PK_ContructualRequisitionDetailEntry=" + db_model.PK_ContructualRequisitionDetailEntry,
                            SubTitle = "Agent- " + CurrentUser.FullName + " " + "Contractual company-" + CurrentUser.ContructualRequisitionCompany.Name + ". needs " + db_model.VehicleCount + " " + contructualRequisitionDetail.VehicleTypeLayer1_english + " : " + contructualRequisitionDetail.VehicleTypeLayer2_english + " : " + contructualRequisitionDetail.VehicleTypeLayer3_english + ".",
                            CreatedAt = DateTime.Now,
                            Category = RequistionNotificationCategory.Contractual_Requisition_Entry_Created
                        };
                        notificatoinList.Add(notification);
                        if (!string.IsNullOrEmpty(agent.ContactNumber))
                        {
                            NumbersToSMS = NumbersToSMS + agent.ContactNumber + ",";
                        }
                    }
                    if (contractualCompanyInternalAgents.Count > 0)
                    {
                        FK_FirebaseAppUser.AddRange(contractualCompanyInternalAgents.Select(m => m.PK_User).ToList());
                    }

                    //# SMS
                    if (!string.IsNullOrEmpty(NumbersToSMS) && NumbersToSMS.Last() == ',')
                    {
                        NumbersToSMS = NumbersToSMS.Substring(0, NumbersToSMS.Length - 1);
                    }
                    string messageBody = "Agent- " + CurrentUser.FullName + " " + "Contractual company-" + CurrentUser.ContructualRequisitionCompany.Name + ". needs " + db_model.VehicleCount + " " + contructualRequisitionDetail.VehicleTypeLayer1_english + " : " + contructualRequisitionDetail.VehicleTypeLayer2_english + " : " + contructualRequisitionDetail.VehicleTypeLayer3_english + ".";
                    //var sms_response = SendSMS(NumbersToSMS, messageBody);

                    //# Firebase notifier
                    var Title = "Cotractual vehicle request by- " + CurrentUser.FullName;
                    var SubTitle = "Agent- " + CurrentUser.FullName + " " + "Contractual company-" + CurrentUser.ContructualRequisitionCompany.Name + ". needs " + db_model.VehicleCount + " " + contructualRequisitionDetail.VehicleTypeLayer1_english + " : " + contructualRequisitionDetail.VehicleTypeLayer2_english + " : " + contructualRequisitionDetail.VehicleTypeLayer3_english + ".";
                    var Category = RequistionNotificationCategory.Individual_Requisition_Created;
                    var fcm_response = SendFCM_Notification_Multiple(FK_FirebaseAppUser, Category, Title, SubTitle);
                }
                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "Contructual Requisition Entry Request is successfully added.");
                return RedirectToAction("View_ContructualRequisitionDetail", new { PK_ContructualRequisitionDetail = model.FK_ContructualRequisitionDetail });
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                return RedirectToAction("IndexBy_ExternalAgent");
            }
        }
        public ActionResult Approve_ContructualRequisitionDetailEntry(Guid PK_ContructualRequisitionDetailEntry)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            if (PK_ContructualRequisitionDetailEntry == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var model = bll.db.ContructualRequisitionDetailEntries.Where(m => m.PK_ContructualRequisitionDetailEntry == PK_ContructualRequisitionDetailEntry).FirstOrDefault();
                if (model != null)
                {
                    try
                    {
                        model.Status = 1;
                        model.AppliedAt = DateTime.Now;
                        model.FK_AppUser_ApprovedBy = CurrentUser.PK_User;

                        //# Notification
                        var notification = new RequisitionAgentNotification()
                        {
                            FK_RequisitionAgent = model.FK_AppUser_AppliedBy,
                            Status = 0,
                            Title = "Approved Contructual Requsition Request by- " + CurrentUser.FullName,
                            ViewLink = "/ContructualRequisition/ContructualRequisitionDetailList_ExternalAgent?PK_ContructualRequisitionDetailEntry=" + PK_ContructualRequisitionDetailEntry,
                            SubTitle = "Agent- " + CurrentUser.FullName + " " + "Depot-" + CurrentUser.Depo.Name + " Approved Contructual Requsition Request",
                            CreatedAt = DateTime.Now,
                            Category = RequistionNotificationCategory.Contractual_Requisition_Entry_Approved
                        };

                        var _agent = bll.db.AppUsers.Where(m => m.PK_User == model.FK_AppUser_AppliedBy).FirstOrDefault();
                        //# SMS
                        if (!string.IsNullOrEmpty(_agent.ContactNumber))
                        {
                            string messageBody = "Agent- " + CurrentUser.FullName + " approved contractual requsition" + model.VehicleCount + " " + model.ContructualRequisitionDetail.VehicleTypeLayer1_english + " : " + model.ContructualRequisitionDetail.VehicleTypeLayer2_english + " : " + model.ContructualRequisitionDetail.VehicleTypeLayer3_english + ". " + "\ncall for detail:" + CurrentUser.ContactNumber;
                            //var sms_response = SendSMS(_agent.ContactNumber, messageBody);
                        }

                        //# Firebase notifier
                        var Title = "Contractual Requisition is approved";
                        var SubTitle = "Agent- " + CurrentUser.FullName + " " + "Depot-" + CurrentUser.Depo.Name + " approved contractual requsition" + model.VehicleCount + " " + model.ContructualRequisitionDetail.VehicleTypeLayer1_english + " : " + model.ContructualRequisitionDetail.VehicleTypeLayer2_english + " : " + model.ContructualRequisitionDetail.VehicleTypeLayer3_english + "." + "\ncall for detail:" + CurrentUser.ContactNumber;
                        var Category = RequistionNotificationCategory.Contractual_Requisition_Entry_Approved;
                        var fcm_response = SendFCM_Notification_Single(_agent.PK_User, Category, Title, SubTitle);

                        CreateAlertMessage(AlertMessageType.Success, "Success", "Contractutal Requisition Request Approved.");
                        bll.db.SaveChanges();
                        return RedirectToAction("ContructualRequisitionDetailEntryList_ApproverView");
                    }
                    catch (Exception exception)
                    {
                        CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                        return RedirectToAction("ContructualRequisitionDetailList_ExternalAgent", new { PK_ContructualRequisitionDetailEntry = PK_ContructualRequisitionDetailEntry });
                    }
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        public ActionResult CreateContructualRequisitionCompany_ByModal(ContructualRequisitionCompany model)

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
                    //CreateAlertMessage(AlertMessageType.Success, "Success", "Contructual company is successfully added.");
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
        public ActionResult View_ContructualRequisitionDetail(Guid PK_ContructualRequisitionDetail)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            if (PK_ContructualRequisitionDetail == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var model = bll.db.ContructualRequisitionDetails.Find(PK_ContructualRequisitionDetail);
                if (model != null)
                {
                    var ContructualRequisitionDetailEntries = bll.db.ContructualRequisitionDetailEntries.Where(m => m.FK_ContructualRequisitionDetail == model.PK_ContructualRequisitionDetail).OrderByDescending(m => m.RowSerial).ToList();
                    ViewBag.ContructualRequisitionDetailEntries = ContructualRequisitionDetailEntries;
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
        public ActionResult Delete_ContructualRequisitionDetail(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var model = bll.db.ContructualRequisitionDetails.Find(id);
                if (model != null)
                {
                    try
                    {
                        model.IsDeleted = true;
                        bll.db.SaveChanges();
                        CreateAlertMessage(AlertMessageType.Success, "Success", "Contruct detail is successfully deleted.");
                        return RedirectToAction("ContructualRequisitionDetailList_ExternalAgent");
                    }
                    catch (Exception exception)
                    {
                        CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                        return RedirectToAction("ContructualRequisitionDetailList_ExternalAgent");
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