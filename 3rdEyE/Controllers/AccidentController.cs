using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _3rdEyE.Models;
using _3rdEyE.ViewModels;
using _3rdEyE.BLL;
using _3rdEyE.ManagingTools;
using System.IO;
using System.Collections.Specialized;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace _3rdEyE.Controllers
{
    public class AccidentController : BaseController
    {
        BLL_Accident bll = new BLL_Accident();
        public Dictionary<string, string> DamageTypes = new Dictionary<string, string> { { "Minor", "Minor" }, { "Major", "Major" }, { "Fatal", "Fatal" } };
        //# Accident
        public ActionResult AccidentIndex(String TrackingID, DateTime? StartingDate, DateTime? EndingDate)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var list = new List<Accident>();
            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            var query = bll.db.Accidents.AsEnumerable().Where(c => c.IsDeleted == false).Where(m => accessibleDepoes.Contains(m.Vehicle.FK_Depo));
            if (!string.IsNullOrEmpty(TrackingID))
            {
                query = query.Where(m => m.TrackingID != null && m.TrackingID.Contains(TrackingID));
                ViewBag.TrackingID = TrackingID;
            }
            if (StartingDate != null)
            {
                query = query.Where(m => m.OccuranceDate >= StartingDate);
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd}", StartingDate);
            }
            else
            {
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-15));
            }
            if (EndingDate != null)
            {
                var _EndingDateString = String.Format("{0:yyyy-MM-dd}", EndingDate);
                var _EndingDate = Convert.ToDateTime(_EndingDateString).AddDays(1);
                query = query.Where(m => m.OccuranceDate <= _EndingDate);
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd}", EndingDate);
            }
            else
            {
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
            }
            if (!string.IsNullOrEmpty(TrackingID) || StartingDate != null || EndingDate != null)
            {
                list = query.ToList();
            }
            return View(list);
        }
        public void AccidentExcelExport(String TrackingID, DateTime? StartingDate, DateTime? EndingDate)
        {
            var list = new List<VM_Accident>();
            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            var query = bll.db.Accidents.AsEnumerable().Where(c => c.IsDeleted == false).Where(m => accessibleDepoes.Contains(m.Vehicle.FK_Depo));
            if (!string.IsNullOrEmpty(TrackingID))
            {
                query = query.Where(m => m.TrackingID.Contains(TrackingID));
                ViewBag.TrackingID = TrackingID;
            }
            if (StartingDate != null)
            {
                query = query.Where(m => m.OccuranceDate >= StartingDate);
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd}", StartingDate);
            }
            else
            {
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-15));
            }
            if (EndingDate != null)
            {
                var _EndingDateString = String.Format("{0:yyyy-MM-dd}", EndingDate);
                var _EndingDate = Convert.ToDateTime(_EndingDateString).AddDays(1);
                query = query.Where(m => m.OccuranceDate <= _EndingDate);
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd}", _EndingDate);
            }
            else
            {
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
            }
            if (!string.IsNullOrEmpty(TrackingID) || StartingDate != null || EndingDate != null)
            {
                list = query.Select(m => bll.ConvertToViewModel(m)).ToList();
            }
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=Accident_List.xls");
            Response.AddHeader("Content-Type", "application/vnd.ms-excel");

            //# Add Header Row
            Response.Output.Write("Tracking ID" + "\t");
            Response.Output.Write("Vehicle" + "\t");
            Response.Output.Write("Driver ID" + "\t");
            Response.Output.Write("Driver Name" + "\t");

            Response.Output.Write("Accident Date" + "\t");//OccuranceDate
            Response.Output.Write("Location-District" + "\t");
            Response.Output.Write("Location-Upazila/Thana" + "\t");
            Response.Output.Write("Location Detail" + "\t");//AccidentLocationDetail
            Response.Output.Write("Current Location" + "\t");//CurrentVehicleLocation

            Response.Output.Write("Damage Type" + "\t");
            Response.Output.Write("DescriptionDuty" + "\t");
            Response.Output.Write("Description Accident" + "\t");
            Response.Output.Write("Primary Cause" + "\t");

            Response.Output.Write("Man Loss Count" + "\t");
            Response.Output.Write("Damage Product Cost" + "\t");
            Response.Output.Write("Damage Vehicle Detail" + "\t");
            Response.Output.Write("Other Damage" + "\t");
            Response.Output.Write("Following Depo" + "\t");//FK_DepoFollowUp

            Response.Output.Write("Assigned Staff ID" + "\t");//ActionTakenStaffID
            Response.Output.Write("Assigned Staff Name" + "\t");//ActionTakenStaffName
            Response.Output.Write("Note" + "\t");
            Response.Output.Write("Status" + "\t");//Status_text

            Response.Output.Write("SettlementNote" + "\t");
            Response.Output.Write("FK_SettledByUser" + "\t");
            Response.Output.Write("SettledAt" + "\t");

            Response.Output.WriteLine();
            foreach (var item in list)
            {
                //Response.Output.Write("Tracking ID" + "\t");
                //Response.Output.Write("Vehicle" + "\t");
                //Response.Output.Write("Driver ID" + "\t");
                //Response.Output.Write("Driver Name" + "\t");
                if (item.Model.TrackingID != null)
                {
                    Response.Output.Write(item.Model.TrackingID + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Model.FK_Vehicle != null)
                {
                    Response.Output.Write(item.Model.Vehicle.RegistrationNumber + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Model.AccusedDriverStaffID != null)
                {
                    Response.Output.Write(item.Model.AccusedDriverStaffID + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Model.AccusedDriverName != null)
                {
                    Response.Output.Write(item.Model.AccusedDriverName + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }

                //Response.Output.Write("Accident Date" + "\t");//OccuranceDate
                //Response.Output.Write("Location-District" + "\t");
                //Response.Output.Write("Location-Upazila/Thana" + "\t");
                //Response.Output.Write("Location Detail" + "\t");//AccidentLocationDetail
                //Response.Output.Write("Current Location" + "\t");//CurrentVehicleLocation
                if (item.Model.OccuranceDate != null)
                {
                    Response.Output.Write(item.Model.OccuranceDate + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Model.District != null)
                {
                    Response.Output.Write(item.Model.District.Name + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Model.Upazila != null)
                {
                    Response.Output.Write(item.Model.Upazila.Name + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Model.AccidentLocationDetail != null)
                {
                    Response.Output.Write(item.Model.AccidentLocationDetail.Replace(System.Environment.NewLine, "") + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Model.CurrentVehicleLocation != null)
                {
                    Response.Output.Write(item.Model.CurrentVehicleLocation.Replace(System.Environment.NewLine, "") + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }

                //Response.Output.Write("Damage Type" + "\t");
                //Response.Output.Write("Description Duty" + "\t");
                //Response.Output.Write("Description Accident" + "\t");
                //Response.Output.Write("Primary Cause" + "\t");
                if (item.Model.DamageType != null)
                {
                    Response.Output.Write(item.Model.DamageType + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Model.DescriptionDuty != null)
                {
                    Response.Output.Write(item.Model.DescriptionDuty.Replace(System.Environment.NewLine, "") + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Model.DescriptionAccident != null)
                {
                    Response.Output.Write(item.Model.DescriptionAccident.Replace(System.Environment.NewLine, "") + "\t");
                    //Response.Output.Write("\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Model.PrimaryCause != null)
                {
                    Response.Output.Write(item.Model.PrimaryCause + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }

                //Response.Output.Write("Man Loss Count" + "\t");
                //Response.Output.Write("Damage Product Cost" + "\t");
                //Response.Output.Write("Damage Vehicle Detail" + "\t");
                //Response.Output.Write("Other Damage" + "\t");
                //Response.Output.Write("Following Depo" + "\t");//FK_DepoFollowUp
                if (item.Model.ManLossCount != null)
                {
                    Response.Output.Write(item.Model.ManLossCount + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Model.DamageProductCost != null)
                {
                    Response.Output.Write(item.Model.DamageProductCost + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Model.DamageVehicleDetail != null)
                {
                    Response.Output.Write(item.Model.DamageVehicleDetail.Replace(System.Environment.NewLine, "") + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Model.OtherDamage != null)
                {
                    Response.Output.Write(item.Model.OtherDamage.Replace(System.Environment.NewLine, "") + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Model.FK_DepoFollowUp != null)
                {
                    Response.Output.Write(item.Model.Depo.Name + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }

                //Response.Output.Write("Assigned Staff ID" + "\t");//ActionTakenStaffID
                //Response.Output.Write("Assigned Staff ID" + "\t");//ActionTakenStaffName
                //Response.Output.Write("Note" + "\t");
                //Response.Output.Write("Status" + "\t");//Status_text
                if (item.Model.ActionTakenStaffID != null)
                {
                    Response.Output.Write(item.Model.ActionTakenStaffID + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Model.ActionTakenStaffName != null)
                {
                    Response.Output.Write(item.Model.ActionTakenStaffName + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Model.Note != null)
                {
                    Response.Output.Write(item.Model.Note.Replace(System.Environment.NewLine, "") + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Model.Status != null)
                {
                    Response.Output.Write(item.Status_Text + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }

                //Response.Output.Write("SettlementNote" + "\t");
                //Response.Output.Write("SettlementAmount" + "\t");
                //Response.Output.Write("FK_SettledByUser" + "\t");
                //Response.Output.Write("SettledAt" + "\t");
                if (item.Model.SettlementNote != null)
                {
                    Response.Output.Write(item.Model.SettlementNote.Replace(System.Environment.NewLine, "") + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Model.FK_SettledByUser != null)
                {
                    Response.Output.Write(item.Model.AppUser1.FullName + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Model.SettledAt != null)
                {
                    Response.Output.Write(item.Model.SettledAt + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }

                //if (item.Model.X != null)
                //{
                //    Response.Output.Write(item.Model.X + "\t");
                //}
                //else
                //{
                //    Response.Output.Write("\t");
                //}
                Response.Output.WriteLine();

            }
            Response.End();
        }
        public ActionResult AccidentCreate(Guid? FK_Vehicle)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = new Accident();
            ViewBag.model = model;
            if (FK_Vehicle != null)
            {
                var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
                ViewBag.Vehicles = new SelectList(bll.db.Vehicles.Where(c => c.IsDeleted == false && c.OWN_MHT_DHT == "OWN" && accessibleDepoes.Contains(c.FK_Depo)).OrderBy(m => m.RegistrationNumber), "PK_Vehicle", "RegistrationNumber", FK_Vehicle);
            }
            else
            {
                var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
                ViewBag.Vehicles = new SelectList(bll.db.Vehicles.Where(c => c.IsDeleted == false && c.OWN_MHT_DHT == "OWN" && accessibleDepoes.Contains(c.FK_Depo)).OrderBy(m => m.RegistrationNumber), "PK_Vehicle", "RegistrationNumber");
            }
            ViewBag.Districts = new SelectList(bll.db.Districts, "PK_District", "Name");
            ViewBag.DamageTypes = new SelectList(DamageTypes, "Key", "Value");
            ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted != true && (!m.Category.Contains("Physical"))).OrderBy(m => m.Name), "PK_Depo", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult AccidentCreate(FormCollection formCollection, List<HttpPostedFileBase> ImageFiles)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }

            try
            {

                var model = new Accident();
                model.IsDeleted = false;
                model.CreatedAt = DateTime.Now;
                model.FK_CreatedByUser = CommonClass.GetCurrentUser().PK_User;
                model.FK_Vehicle = Guid.Parse(formCollection["FK_Vehicle"]);
                if (formCollection["AccusedDriverStaffID"] != "")
                {
                    model.AccusedDriverStaffID = formCollection["AccusedDriverStaffID"];
                }
                else
                {
                    model.AccusedDriverStaffID = null;
                }
                if (formCollection["AccusedDriverName"] != "")
                {
                    model.AccusedDriverName = formCollection["AccusedDriverName"];
                }
                else
                {
                    model.AccusedDriverName = null;
                }
                if (formCollection["OccuranceDate"] != "")
                {
                    model.OccuranceDate = Convert.ToDateTime(formCollection["OccuranceDate"]);
                }
                if (formCollection["FK_District"] != "")
                {
                    model.FK_District = Convert.ToInt64(formCollection["FK_District"]);
                }
                else
                {
                    model.FK_District = null;
                }
                if (formCollection["FK_Upazila"] != null && formCollection["FK_Upazila"] != "")
                {
                    model.FK_Upazila = Convert.ToInt64(formCollection["FK_Upazila"]);
                }
                else
                {
                    model.FK_Upazila = null;
                }
                if (formCollection["AccidentLocationDetail"] != "")
                {
                    model.AccidentLocationDetail = formCollection["AccidentLocationDetail"];
                }
                else
                {
                    model.AccidentLocationDetail = null;
                }
                if (formCollection["CurrentVehicleLocation"] != "")
                {
                    model.CurrentVehicleLocation = formCollection["CurrentVehicleLocation"];
                }
                else
                {
                    model.CurrentVehicleLocation = null;
                }
                if (formCollection["DamageType"] != "")
                {
                    model.DamageType = formCollection["DamageType"];
                }
                else
                {
                    model.DamageType = null;
                }
                if (formCollection["DescriptionDuty"] != "")
                {
                    model.DescriptionDuty = formCollection["DescriptionDuty"];
                }
                else
                {
                    model.DescriptionDuty = null;
                }
                if (formCollection["DescriptionAccident"] != "")
                {
                    model.DescriptionAccident = formCollection["DescriptionAccident"];
                }
                else
                {
                    model.DescriptionAccident = null;
                }
                if (formCollection["PrimaryCause"] != "")
                {
                    model.PrimaryCause = formCollection["PrimaryCause"];
                }
                else
                {
                    model.PrimaryCause = null;
                }
                if (formCollection["ManLossCount"] != "")
                {
                    model.ManLossCount = Convert.ToInt32(formCollection["ManLossCount"]);
                }
                else
                {
                    model.ManLossCount = null;
                }
                if (formCollection["DamageProductCost"] != "")
                {
                    model.DamageProductCost = Convert.ToInt64(formCollection["DamageProductCost"]);
                }
                else
                {
                    model.DamageProductCost = null;
                }
                if (formCollection["DamageVehicleDetail"] != "")
                {
                    model.DamageVehicleDetail = formCollection["DamageVehicleDetail"];
                }
                else
                {
                    model.DamageVehicleDetail = null;
                }
                if (formCollection["OtherDamage"] != "")
                {
                    model.OtherDamage = formCollection["OtherDamage"];
                }
                else
                {
                    model.OtherDamage = null;
                }
                if (formCollection["FK_DepoFollowUp"] != "")
                {
                    model.FK_DepoFollowUp = Guid.Parse(formCollection["FK_DepoFollowUp"]);
                }
                else
                {
                    model.FK_DepoFollowUp = null;
                }
                if (formCollection["ActionTakenStaffID"] != "")
                {
                    model.ActionTakenStaffID = formCollection["ActionTakenStaffID"];
                }
                else
                {
                    model.AccusedDriverStaffID = null;
                }
                if (formCollection["ActionTakenStaffName"] != "")
                {
                    model.ActionTakenStaffName = formCollection["ActionTakenStaffName"];
                }
                else
                {
                    model.ActionTakenStaffName = null;
                }
                if (formCollection["Note"] != "")
                {
                    model.Note = formCollection["Note"];
                }
                else
                {
                    model.Note = null;
                }
                model.Status = 0;
                bll.db.Accidents.Add(model);
                bll.db.SaveChanges();
                var _RegistrationNumber = bll.db.Vehicles.Where(m => m.PK_Vehicle == model.FK_Vehicle).FirstOrDefault().RegistrationNumber;
                model.TrackingID = _RegistrationNumber.Split('-')[2] + _RegistrationNumber.Split('-')[3] + "-" + model.PK_Accident;
                //# create folder
                string virtualFolderPath = CommonClass.ImageDirectory + "Vehicles/" + model.FK_Vehicle + "/" + "Accident" + "/";
                string physicalFolderPath = Path.Combine(Server.MapPath(virtualFolderPath));
                if (!Directory.Exists(physicalFolderPath))
                {
                    Directory.CreateDirectory(physicalFolderPath);
                }

                int totalDocument = Convert.ToInt32(formCollection["rowCount"]);
                for (int i = 0; i < totalDocument; i++)
                {
                    var subModel = new AccidentDocument();

                    subModel.IsDeleted = false;

                    subModel.CreatedAt = DateTime.Now;
                    subModel.FK_CreatedByUser = CommonClass.GetCurrentUser().PK_User;

                    subModel.FK_Accident = model.PK_Accident;
                    subModel.Title = formCollection["Title_" + i];
                    subModel.IdentitficaitonKey = formCollection["IdentitficaitonKey_" + i];
                    subModel.IdentitficaitonValue = formCollection["IdentitficaitonValue_" + i];

                    string virtualFilePath = virtualFolderPath + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace(":", "-") + " " + i + " " + "." + ImageFiles[i].FileName.Split('.').Last();
                    ImageFiles[i].SaveAs(Path.Combine(Server.MapPath(virtualFilePath)));

                    subModel.ImageLocation = virtualFilePath;

                    bll.db.AccidentDocuments.Add(subModel);
                }
                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "Accident entry is successfully added.");
                return RedirectToAction("AccidentIndex");
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                return RedirectToAction("AccidentIndex");
            }
        }

        public ActionResult AccidentEdit(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.Accidents.Where(m => m.PK_Accident == id && m.IsDeleted == false).FirstOrDefault();
            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            ViewBag.Vehicles = new SelectList(bll.db.Vehicles.Where(c => c.IsDeleted == false && c.OWN_MHT_DHT == "OWN" && accessibleDepoes.Contains(c.FK_Depo)).OrderBy(m => m.RegistrationNumber), "PK_Vehicle", "RegistrationNumber", model.FK_Vehicle);
            ViewBag.Districts = new SelectList(bll.db.Districts, "PK_District", "Name", model.FK_District);
            ViewBag.Upazilas = new SelectList(bll.db.Upazilas.Where(m => m.FK_District == model.FK_District), "PK_Upazila", "Name", model.FK_Upazila);
            ViewBag.DamageTypes = new SelectList(DamageTypes, "Key", "Value", model.DamageType);
            ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted != true && (!m.Category.Contains("Physical"))).OrderBy(m => m.Name), "PK_Depo", "Name", model.FK_DepoFollowUp);
            return View(model);
        }

        [HttpPost]
        public ActionResult AccidentEdit(FormCollection formCollection, List<HttpPostedFileBase> ImageFiles)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }

            try
            {
                var id = Convert.ToInt64(formCollection["PK_Accident"]);
                var model = bll.db.Accidents.Where(m => m.PK_Accident == id && m.IsDeleted == false).FirstOrDefault();
                model.UpdatedAt = DateTime.Now;
                model.FK_UpdatedByUser = CommonClass.GetCurrentUser().PK_User;

                model.FK_Vehicle = Guid.Parse(formCollection["FK_Vehicle"]);
                if (formCollection["AccusedDriverStaffID"] != "")
                {
                    model.AccusedDriverStaffID = formCollection["AccusedDriverStaffID"];
                }
                else
                {
                    model.AccusedDriverStaffID = null;
                }
                if (formCollection["AccusedDriverName"] != "")
                {
                    model.AccusedDriverName = formCollection["AccusedDriverName"];
                }
                else
                {
                    model.AccusedDriverName = null;
                }
                if (formCollection["OccuranceDate"] != "")
                {
                    model.OccuranceDate = Convert.ToDateTime(formCollection["OccuranceDate"]);
                }
                if (formCollection["FK_District"] != "")
                {
                    model.FK_District = Convert.ToInt64(formCollection["FK_District"]);
                }
                else
                {
                    model.FK_District = null;
                }
                if (formCollection["FK_Upazila"] != null && formCollection["FK_Upazila"] != "")
                {
                    model.FK_Upazila = Convert.ToInt64(formCollection["FK_Upazila"]);
                }
                else
                {
                    model.FK_Upazila = null;
                }
                if (formCollection["AccidentLocationDetail"] != "")
                {
                    model.AccidentLocationDetail = formCollection["AccidentLocationDetail"];
                }
                else
                {
                    model.AccidentLocationDetail = null;
                }
                if (formCollection["CurrentVehicleLocation"] != "")
                {
                    model.CurrentVehicleLocation = formCollection["CurrentVehicleLocation"];
                }
                else
                {
                    model.CurrentVehicleLocation = null;
                }
                if (formCollection["DamageType"] != "")
                {
                    model.DamageType = formCollection["DamageType"];
                }
                else
                {
                    model.DamageType = null;
                }
                if (formCollection["DescriptionDuty"] != "")
                {
                    model.DescriptionDuty = formCollection["DescriptionDuty"];
                }
                else
                {
                    model.DescriptionDuty = null;
                }
                if (formCollection["DescriptionAccident"] != "")
                {
                    model.DescriptionAccident = formCollection["DescriptionAccident"];
                }
                else
                {
                    model.DescriptionAccident = null;
                }
                if (formCollection["PrimaryCause"] != "")
                {
                    model.PrimaryCause = formCollection["PrimaryCause"];
                }
                else
                {
                    model.PrimaryCause = null;
                }
                if (formCollection["ManLossCount"] != "")
                {
                    model.ManLossCount = Convert.ToInt32(formCollection["ManLossCount"]);
                }
                else
                {
                    model.ManLossCount = null;
                }
                if (formCollection["DamageProductCost"] != "")
                {
                    model.DamageProductCost = Convert.ToInt64(formCollection["DamageProductCost"]);
                }
                else
                {
                    model.DamageProductCost = null;
                }
                if (formCollection["DamageVehicleDetail"] != "")
                {
                    model.DamageVehicleDetail = formCollection["DamageVehicleDetail"];
                }
                else
                {
                    model.DamageVehicleDetail = null;
                }
                if (formCollection["OtherDamage"] != "")
                {
                    model.OtherDamage = formCollection["OtherDamage"];
                }
                else
                {
                    model.OtherDamage = null;
                }
                if (formCollection["FK_DepoFollowUp"] != "")
                {
                    model.FK_DepoFollowUp = Guid.Parse(formCollection["FK_DepoFollowUp"]);
                }
                else
                {
                    model.FK_DepoFollowUp = null;
                }
                if (formCollection["ActionTakenStaffID"] != "")
                {
                    model.ActionTakenStaffID = formCollection["ActionTakenStaffID"];
                }
                else
                {
                    model.AccusedDriverStaffID = null;
                }
                if (formCollection["ActionTakenStaffName"] != "")
                {
                    model.ActionTakenStaffName = formCollection["ActionTakenStaffName"];
                }
                else
                {
                    model.ActionTakenStaffName = null;
                }
                if (formCollection["Note"] != "")
                {
                    model.Note = formCollection["Note"];
                }
                else
                {
                    model.Note = null;
                }
                bll.db.SaveChanges();

                //# create folder
                string virtualFolderPath = CommonClass.ImageDirectory + "Vehicles/" + model.FK_Vehicle + "/" + "Accident" + "/";
                string physicalFolderPath = Path.Combine(Server.MapPath(virtualFolderPath));
                if (!Directory.Exists(physicalFolderPath))
                {
                    Directory.CreateDirectory(physicalFolderPath);
                }

                int totalDocument = Convert.ToInt32(formCollection["rowCount"]);
                for (int i = 0; i < totalDocument; i++)
                {
                    var subModel = new AccidentDocument();

                    subModel.IsDeleted = false;

                    subModel.CreatedAt = DateTime.Now;
                    subModel.FK_CreatedByUser = CommonClass.GetCurrentUser().PK_User;

                    subModel.FK_Accident = model.PK_Accident;
                    subModel.Title = formCollection["Title_" + i];
                    subModel.IdentitficaitonKey = formCollection["IdentitficaitonKey_" + i];
                    subModel.IdentitficaitonValue = formCollection["IdentitficaitonValue_" + i];

                    string virtualFilePath = virtualFolderPath + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace(":", "-") + " " + i + " " + "." + ImageFiles[i].FileName.Split('.').Last();
                    ImageFiles[i].SaveAs(Path.Combine(Server.MapPath(virtualFilePath)));

                    subModel.ImageLocation = virtualFilePath;

                    bll.db.AccidentDocuments.Add(subModel);
                }
                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "Accident is successfully updated.");
                return RedirectToAction("AccidentIndex");
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
            }
            return RedirectToAction("AccidentIndex");
        }

        public ActionResult AccidentView(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.Accidents.Where(m => m.PK_Accident == id && m.IsDeleted == false).FirstOrDefault();
            if (model != null)
            {
                var viewModel = bll.ConvertToViewModel(model);
                var list = bll.db.AccidentDocuments.Where(m => m.FK_Accident == model.PK_Accident && m.IsDeleted == false).ToList();
                viewModel.Model.AccidentDocuments = list;
                return View(viewModel);
            }
            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult AccidentSettle(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.Accidents.Where(m => m.PK_Accident == id && m.IsDeleted == false).FirstOrDefault();
            if (model != null)
            {
                return View(model);
            }
            else
            {
                return HttpNotFound();
            }
        }
        [HttpPost]
        public ActionResult AccidentSettle(FormCollection formCollection, List<HttpPostedFileBase> ImageFiles)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }

            try
            {
                var PK_Accident = Convert.ToInt64(formCollection["PK_Accident"]);
                var accident = bll.db.Accidents.Where(m => m.PK_Accident == PK_Accident && m.IsDeleted == false).FirstOrDefault();
                if (accident.Status == 1)
                {
                    CreateAlertMessage(AlertMessageType.Danger, "Danger", "Accident is already close.");
                    return RedirectToAction("AccidentSettle", new { id = PK_Accident });
                }
                if (!string.IsNullOrEmpty(formCollection["OthersCost"]))
                {
                    accident.OthersCost = Convert.ToInt64(formCollection["OthersCost"]);
                }
                if (!string.IsNullOrEmpty(formCollection["OthersNote"]))
                {
                    accident.OthersNote = formCollection["OthersNote"];
                }
                if (!string.IsNullOrEmpty(formCollection["CompensationTaken"]))
                {
                    accident.CompensationTaken = Convert.ToInt64(formCollection["CompensationTaken"]);
                }
                if (!string.IsNullOrEmpty(formCollection["CompensationTakenFrom"]))
                {
                    accident.CompensationTakenFrom = formCollection["CompensationTakenFrom"];
                }
                if (!string.IsNullOrEmpty(formCollection["CompensationTakenStaffID"]))
                {
                    accident.CompensationTakenStaffID = formCollection["CompensationTakenStaffID"];
                }
                if (!string.IsNullOrEmpty(formCollection["CompensationTakenStaffName"]))
                {
                    accident.CompensationTakenStaffName = formCollection["CompensationTakenStaffName"];
                }
                if (!string.IsNullOrEmpty(formCollection["DeductionTakenFromDriver"]))
                {
                    accident.DeductionTakenFromDriver = Convert.ToInt64(formCollection["DeductionTakenFromDriver"]);
                }
                if (!string.IsNullOrEmpty(formCollection["SettlementNote"]))
                {
                    accident.SettlementNote = formCollection["SettlementNote"];
                }
                accident.SettledAt = DateTime.Now;
                accident.FK_SettledByUser = CurrentUser.PK_User;
                accident.Status = 1;
                bll.db.SaveChanges();
                //# create folder
                string virtualFolderPath = CommonClass.ImageDirectory + "Vehicles/" + accident.FK_Vehicle + "/" + "Accident" + "/";
                string physicalFolderPath = Path.Combine(Server.MapPath(virtualFolderPath));
                if (!Directory.Exists(physicalFolderPath))
                {
                    Directory.CreateDirectory(physicalFolderPath);
                }

                int totalDocument = Convert.ToInt32(formCollection["rowCount"]);
                for (int i = 0; i < totalDocument; i++)
                {
                    var subModel = new AccidentDocument();

                    subModel.IsDeleted = false;

                    subModel.CreatedAt = DateTime.Now;
                    subModel.FK_CreatedByUser = CommonClass.GetCurrentUser().PK_User;

                    subModel.FK_Accident = accident.PK_Accident;
                    subModel.Title = formCollection["Title_" + i];
                    subModel.IdentitficaitonKey = formCollection["IdentitficaitonKey_" + i];
                    subModel.IdentitficaitonValue = formCollection["IdentitficaitonValue_" + i];

                    string virtualFilePath = virtualFolderPath + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace(":", "-") + " " + i + " " + "." + ImageFiles[i].FileName.Split('.').Last();
                    ImageFiles[i].SaveAs(Path.Combine(Server.MapPath(virtualFilePath)));

                    subModel.ImageLocation = virtualFilePath;

                    bll.db.AccidentDocuments.Add(subModel);
                }
                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "Police case is successfully added.");
                return RedirectToAction("AccidentIndex");
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                return RedirectToAction("AccidentIndex");
            }
        }

        //# AccidentExpense
        public ActionResult ExpenceCreate(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.Accidents.Where(m => m.PK_Accident == id && m.IsDeleted == false).FirstOrDefault();
            if (model != null)
            {
                return View(model);
            }
            else
            {
                return HttpNotFound();
            }
        }
        [HttpPost]
        public ActionResult ExpenceCreate(FormCollection formCollection, List<HttpPostedFileBase> ImageFiles)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }

            try
            {
                var PK_Accident = Convert.ToInt64(formCollection["PK_Accident"]);
                var accident = bll.db.Accidents.Where(m => m.PK_Accident == PK_Accident && m.IsDeleted == false).FirstOrDefault();
                if (accident.Status == 1)
                {
                    CreateAlertMessage(AlertMessageType.Danger, "Danger", "Accident is already close.");
                }
                var model = new AccidentExpense();
                model.IsDeleted = false;
                model.CreatedAt = DateTime.Now;
                model.FK_CreatedByUser = CommonClass.GetCurrentUser().PK_User;
                model.FK_Accident = Convert.ToInt64(formCollection["PK_Accident"]);
                var _oldExpensesCount = accident.AccidentExpenses.Count();
                if (_oldExpensesCount.ToString().Count() < 2)
                {
                    model.TrackingID = accident.TrackingID + "-" + "0" + accident.AccidentExpenses.Count();
                }
                else
                {
                    model.TrackingID = accident.TrackingID + "-" + accident.AccidentExpenses.Count();
                }

                if (formCollection["OccuranceDate"] != "")
                {
                    model.OccuranceDate = Convert.ToDateTime(formCollection["OccuranceDate"]);
                }
                if (formCollection["PaidAmount"] != "")
                {
                    model.PaidAmount = Convert.ToInt64(formCollection["PaidAmount"]);
                }
                else
                {
                    model.PaidAmount = null;
                }
                if (formCollection["Purpose"] != "")
                {
                    model.Purpose = formCollection["Purpose"];
                }
                else
                {
                    model.Purpose = null;
                }
                if (formCollection["InternalTakerStaffID"] != "")
                {
                    model.InternalTakerStaffID = formCollection["InternalTakerStaffID"];
                }
                else
                {
                    model.InternalTakerStaffID = null;
                }
                if (formCollection["InternalTakerStaffName"] != "")
                {
                    model.InternalTakerStaffName = formCollection["InternalTakerStaffName"];
                }
                else
                {
                    model.InternalTakerStaffName = null;
                }
                if (formCollection["ExternalTakerName"] != "")
                {
                    model.ExternalTakerName = formCollection["ExternalTakerName"];
                }
                else
                {
                    model.ExternalTakerName = null;
                }
                if (formCollection["ExternalTakerContactNumber"] != "")
                {
                    model.ExternalTakerContactNumber = formCollection["ExternalTakerContactNumber"];
                }
                else
                {
                    model.ExternalTakerContactNumber = null;
                }
                if (formCollection["ExternalTakerContactAddress"] != "")
                {
                    model.ExternalTakerContactAddress = formCollection["ExternalTakerContactAddress"];
                }
                else
                {
                    model.ExternalTakerContactAddress = null;
                }
                model.Status = 0;
                bll.db.AccidentExpenses.Add(model);
                bll.db.SaveChanges();
                //# create folder
                string virtualFolderPath = CommonClass.ImageDirectory + "Vehicles/" + accident.FK_Vehicle + "/" + "Accident" + "/";
                string physicalFolderPath = Path.Combine(Server.MapPath(virtualFolderPath));
                if (!Directory.Exists(physicalFolderPath))
                {
                    Directory.CreateDirectory(physicalFolderPath);
                }

                int totalDocument = Convert.ToInt32(formCollection["rowCount"]);
                for (int i = 0; i < totalDocument; i++)
                {
                    var subModel = new AccidentDocument();

                    subModel.IsDeleted = false;

                    subModel.CreatedAt = DateTime.Now;
                    subModel.FK_CreatedByUser = CommonClass.GetCurrentUser().PK_User;

                    subModel.FK_Accident = accident.PK_Accident;
                    subModel.Title = formCollection["Title_" + i];
                    subModel.IdentitficaitonKey = formCollection["IdentitficaitonKey_" + i];
                    subModel.IdentitficaitonValue = formCollection["IdentitficaitonValue_" + i];

                    string virtualFilePath = virtualFolderPath + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace(":", "-") + " " + i + " " + "." + ImageFiles[i].FileName.Split('.').Last();
                    ImageFiles[i].SaveAs(Path.Combine(Server.MapPath(virtualFilePath)));

                    subModel.ImageLocation = virtualFilePath;

                    bll.db.AccidentDocuments.Add(subModel);
                }
                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "Expense of accident is successfully added.");
                return RedirectToAction("AccidentExpenseIndex");
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                return RedirectToAction("AccidentExpenseIndex");
            }
        }

        public ActionResult AccidentExpenseIndex(String TrackingID, DateTime? StartingDate, DateTime? EndingDate)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var list = new List<AccidentExpense>();
            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            var query = bll.db.AccidentExpenses.AsEnumerable().Where(c => c.IsDeleted == false).Where(m => accessibleDepoes.Contains(m.Accident.Vehicle.FK_Depo));
            if (!string.IsNullOrEmpty(TrackingID))
            {
                query = query.Where(m => m.TrackingID.Contains(TrackingID));
                ViewBag.TrackingID = TrackingID;
            }
            if (StartingDate != null)
            {
                query = query.Where(m => m.OccuranceDate >= StartingDate);
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd}", StartingDate);
            }
            else
            {
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-15));
            }
            if (EndingDate != null)
            {
                var _EndingDateString = String.Format("{0:yyyy-MM-dd}", EndingDate);
                var _EndingDate = Convert.ToDateTime(_EndingDateString).AddDays(1);
                query = query.Where(m => m.OccuranceDate <= _EndingDate);
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd}", _EndingDate);
            }
            else
            {
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
            }
            if (!string.IsNullOrEmpty(TrackingID) || StartingDate != null || EndingDate != null)
            {
                list = query.ToList();
            }
            return View(list);
        }
        public ActionResult AccidentExpenseIndex_ForAccountant(String TrackingID, DateTime? StartingDate, DateTime? EndingDate)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var list = new List<AccidentExpense>();
            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            var query = bll.db.AccidentExpenses.AsEnumerable().Where(c => c.IsDeleted == false).Where(m => accessibleDepoes.Contains(m.Accident.Vehicle.FK_Depo));
            if (!string.IsNullOrEmpty(TrackingID))
            {
                query = query.Where(m => m.TrackingID.Contains(TrackingID));
                ViewBag.TrackingID = TrackingID;
            }
            if (StartingDate != null)
            {
                query = query.Where(m => m.OccuranceDate >= StartingDate);
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd}", StartingDate);
            }
            else
            {
                ViewBag.StartingDate = String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-15));
            }
            if (EndingDate != null)
            {
                var _EndingDateString = String.Format("{0:yyyy-MM-dd}", EndingDate);
                var _EndingDate = Convert.ToDateTime(_EndingDateString).AddDays(1);
                query = query.Where(m => m.OccuranceDate <= _EndingDate);
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd}", _EndingDate);
            }
            else
            {
                ViewBag.EndingDate = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
            }
            if (!string.IsNullOrEmpty(TrackingID) || StartingDate != null || EndingDate != null)
            {
                list = query.ToList();
            }
            return View(list);
        }
        public ActionResult ExpenceApprove(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.AccidentExpenses.Find(id);
            if (model != null)
            {
                return View(model);
            }
            else
            {
                return HttpNotFound();
            }

        }
        [HttpPost]
        public ActionResult ExpenceApprove(FormCollection formCollection)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }

            try
            {
                var PK_AccidentExpense = Convert.ToInt64(formCollection["PK_AccidentExpense"]);
                var model = bll.db.AccidentExpenses.Where(m => m.PK_AccidentExpense == PK_AccidentExpense).FirstOrDefault();
                if (model.Status == 1)
                {
                    CreateAlertMessage(AlertMessageType.Danger, "Danger", "AccidentExpense is already close.");
                }
                model.Status = 1;
                model.PaidAt = DateTime.Now;
                model.FK_PaidByUser = CurrentUser.PK_User;
                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "Police case is successfully added.");
                return RedirectToAction("AccidentExpenseIndex_ForAccountant");
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                return RedirectToAction("AccidentExpenseIndex_ForAccountant");
            }
        }


        public FileResult AccidentDocumentDownload(Int64 id)
        {
            var model = bll.db.AccidentDocuments.Find(id);
            byte[] fileBytes = System.IO.File.ReadAllBytes(Path.Combine(Server.MapPath(model.ImageLocation)));
            string fileName = model.ImageLocation.Split('/').Last();

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        public ActionResult AccidentDocumentDelete(Guid id)
        {
            var model = bll.db.AccidentDocuments.Find(id);
            var FK_Accident = model.FK_Accident;
            try
            {
                System.IO.File.Delete(Path.Combine(Server.MapPath(model.ImageLocation)));
                bll.db.AccidentDocuments.Remove(model);
                bll.db.SaveChanges();
            }
            catch (Exception e)
            {
                var errrorMessage = "";
                do
                {
                    errrorMessage = "#" + errrorMessage + e.Message;
                    if (e.InnerException != null)
                    {
                        e = e.InnerException;
                    }
                    else
                    {
                        break;
                    }
                } while (true);
            }
            return RedirectToAction("AccidentEdit", new { id = FK_Accident });
        }

    }
}