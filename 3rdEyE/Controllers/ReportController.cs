using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _3rdEyE.BLL;
using _3rdEyE.Models;
using _3rdEyE.ManagingTools;
using _3rdEyE.ViewModels;
using Microsoft.Reporting.WebForms;
using System.Data.Entity;
using System.Diagnostics;

namespace _3rdEyE.Controllers
{
    public class ReportController : BaseController
    {
        Dictionary<string, string> OWN_MHT_DHT_Dict = new Dictionary<string, string> { { "OWN", "OWN" }, { "MHT", "MHT" }, { "DHT", "DHT" } };


        #region //All Vehicles
        public ActionResult AllVehicles(String FK_Depo, String FK_Company, string OWN_MHT_DHT, String FK_DepoGroup, String RegistrationNumber)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            List<ViewModels.VM_Vehicle> list = new List<ViewModels.VM_Vehicle>();
            var query = bll.db.Vehicles.AsEnumerable().Where(c => c.IsDeleted == false);

            //# FK_Depo
            ViewBag.Depoes = new SelectList(bll.db.Depoes.AsEnumerable().Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Depo", "Name", FK_Depo);
            if (FK_Depo != null)
            {
                var _FK_Depo = Guid.Parse(FK_Depo);
                query = query.Where(m => m.FK_Depo == _FK_Depo);
            }

            //# FK_Company
            ViewBag.UserCompanies = new SelectList(bll.db.Companies.AsEnumerable().Where(m => m.IsDeleted == false && m.GroupOfCompany.IsPranRFLGroup == true && m.IsUserCompany == true).OrderBy(m => m.Name), "PK_Company", "Name", FK_Company);
            if (FK_Company != null)
            {
                var _FK_Company = Guid.Parse(FK_Company);
                query = query.Where(m => m.FK_Company == _FK_Company);
            }

            //# OWN_MHT_DHT
            ViewBag.OWN_MHT_DHT = new SelectList(OWN_MHT_DHT_Dict.AsEnumerable(), "Key", "Value", OWN_MHT_DHT);
            if (OWN_MHT_DHT != null)
            {
                query = query.Where(m => m.OWN_MHT_DHT == OWN_MHT_DHT);
            }

            //# FK_DepoGroup
            ViewBag.DepoGroups = new SelectList(bll.db.DepoGroups.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_DepoGroup", "Name", FK_DepoGroup);
            if (FK_DepoGroup != null)
            {
                var _FK_DepoGroup = Guid.Parse(FK_DepoGroup);
                query = query.Where(m => m.FK_DepoGroup == _FK_DepoGroup);
            }

            //# RegistrationNumber
            ViewBag.RegistrationNumber = RegistrationNumber;
            if (RegistrationNumber != null)
            {
                query = query.Where(m => m.RegistrationNumber.Contains(RegistrationNumber));
            }

            if (FK_Depo != null || FK_Company != null || OWN_MHT_DHT != null || FK_DepoGroup != null || RegistrationNumber != null)
            {
                list = query.Select(m => ConvertToViewModelVehicle(m)).ToList();
            }

            return View(list);
        }

        public ActionResult AllVehicles2(String FK_Depo, String FK_Company, string OWN_MHT_DHT, String FK_DepoGroup, String RegistrationNumber)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            List<ViewModels.VM_Vehicle> list = new List<ViewModels.VM_Vehicle>();
            var query = bll.db.Vehicles.AsEnumerable().Where(c => c.IsDeleted == false);

            //# FK_Depo
            ViewBag.Depoes = new SelectList(bll.db.Depoes.AsEnumerable().Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Depo", "Name", FK_Depo);
            if (FK_Depo != null)
            {
                var _FK_Depo = Guid.Parse(FK_Depo);
                query = query.Where(m => m.FK_Depo == _FK_Depo);
            }

            //# FK_Company
            ViewBag.UserCompanies = new SelectList(bll.db.Companies.AsEnumerable().Where(m => m.IsDeleted == false && m.GroupOfCompany.IsPranRFLGroup == true && m.IsUserCompany == true).OrderBy(m => m.Name), "PK_Company", "Name", FK_Company);
            if (FK_Company != null)
            {
                var _FK_Company = Guid.Parse(FK_Company);
                query = query.Where(m => m.FK_Company == _FK_Company);
            }

            //# OWN_MHT_DHT
            ViewBag.OWN_MHT_DHT = new SelectList(OWN_MHT_DHT_Dict.AsEnumerable(), "Key", "Value", OWN_MHT_DHT);
            if (OWN_MHT_DHT != null)
            {
                query = query.Where(m => m.OWN_MHT_DHT == OWN_MHT_DHT);
            }

            //# FK_DepoGroup
            ViewBag.DepoGroups = new SelectList(bll.db.DepoGroups.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_DepoGroup", "Name", FK_DepoGroup);
            if (FK_DepoGroup != null)
            {
                var _FK_DepoGroup = Guid.Parse(FK_DepoGroup);
                query = query.Where(m => m.FK_DepoGroup == _FK_DepoGroup);
            }

            //# RegistrationNumber
            ViewBag.RegistrationNumber = RegistrationNumber;
            if (RegistrationNumber != null)
            {
                query = query.Where(m => m.RegistrationNumber.Contains(RegistrationNumber));
            }

            if (FK_Depo != null || FK_Company != null || OWN_MHT_DHT != null || FK_DepoGroup != null || RegistrationNumber != null)
            {
                list = query.Select(m => ConvertToViewModelVehicle(m)).ToList();
            }

            return View(list);
        }
        public VM_Vehicle ConvertToViewModelVehicle(Vehicle model)
        {
            var viewModel = new VM_Vehicle();
            viewModel.Model = model;

            //# Only view property
            viewModel.IsDeleted_Text = model.IsDeleted == true ? "Deleted" : "Undeleted";
            viewModel.RegisrationDate_Text = model.RegisrationDate == null ? "" : CommonClass.ConvertToDateString(model.RegisrationDate);
            viewModel.Internal_PurchaseDate_Text = model.Internal_PurchaseDate == null ? "" : CommonClass.ConvertToDateString(model.Internal_PurchaseDate);
            viewModel.MHT_AgreementFrom_Text = model.MHT_AgreementFrom == null ? "" : CommonClass.ConvertToDateString(model.MHT_AgreementFrom);
            viewModel.MHT_AgreementTo_Text = model.MHT_AgreementTo == null ? "" : CommonClass.ConvertToDateString(model.MHT_AgreementTo);
            viewModel.NumberPlate_IsDigital_Text = model.NumberPlate_IsDigital == true ? "Digital" : "Analogue";
            return viewModel;
        }

        public void ExportVehicleList(String FK_Depo, String FK_Company, string OWN_MHT_DHT, String FK_DepoGroup, String RegistrationNumber)
        {
            List<ViewModels.VM_Vehicle> list = new List<ViewModels.VM_Vehicle>();
            var query = bll.db.Vehicles.AsEnumerable().Where(c => c.IsDeleted == false);

            //# FK_Depo
            ViewBag.Depoes = new SelectList(bll.db.Depoes.AsEnumerable().Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_Depo", "Name", FK_Depo);
            if (FK_Depo != null)
            {
                var _FK_Depo = Guid.Parse(FK_Depo);
                query = query.Where(m => m.FK_Depo == _FK_Depo);
            }

            //# FK_Company
            ViewBag.UserCompanies = new SelectList(bll.db.Companies.AsEnumerable().Where(m => m.IsDeleted == false && m.GroupOfCompany.IsPranRFLGroup == true && m.IsUserCompany == true).OrderBy(m => m.Name), "PK_Company", "Name", FK_Company);
            if (FK_Company != null)
            {
                var _FK_Company = Guid.Parse(FK_Company);
                query = query.Where(m => m.FK_Company == _FK_Company);
            }

            //# OWN_MHT_DHT
            ViewBag.OWN_MHT_DHT = new SelectList(OWN_MHT_DHT_Dict.AsEnumerable(), "Key", "Value", OWN_MHT_DHT);
            if (OWN_MHT_DHT != null)
            {
                query = query.Where(m => m.OWN_MHT_DHT == OWN_MHT_DHT);
            }

            //# FK_DepoGroup
            ViewBag.DepoGroups = new SelectList(bll.db.DepoGroups.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_DepoGroup", "Name", FK_DepoGroup);
            if (FK_DepoGroup != null)
            {
                var _FK_DepoGroup = Guid.Parse(FK_DepoGroup);
                query = query.Where(m => m.FK_DepoGroup == _FK_DepoGroup);
            }

            //# RegistrationNumber
            ViewBag.RegistrationNumber = RegistrationNumber;
            if (RegistrationNumber != null)
            {
                query = query.Where(m => m.RegistrationNumber.Contains(RegistrationNumber));
            }

            if (FK_Depo != null || FK_Company != null || OWN_MHT_DHT != null || FK_DepoGroup != null || RegistrationNumber != null)
            {
                list = query.Select(m => ConvertToViewModelVehicle(m)).ToList();
            }

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=Vehicle_List.xls");
            Response.AddHeader("Content-Type", "application/vnd.ms-excel");

            //# Add Header Row
            Response.Output.Write("Registration No" + "\t");
            Response.Output.Write("OWN/MHT/DHT" + "\t");
            Response.Output.Write("User Company" + "\t");
            Response.Output.Write("User Group" + "\t");
            Response.Output.Write("User Group Category" + "\t");
            Response.Output.Write("Sub User Group" + "\t");
            Response.Output.Write("Brand" + "\t");
            Response.Output.Write("Model" + "\t");
            Response.Output.Write("Registration Date" + "\t");
            Response.Output.Write("Chassis No" + "\t");
            Response.Output.Write("Engine No" + "\t");
            Response.Output.Write("Engine CC" + "\t");
            Response.Output.Write("Vehicle Type" + "\t");
            Response.Output.Write("Fuel Type" + "\t");
            Response.Output.Write("GPS IMEI Number" + "\t");
            Response.Output.Write("GPS Mobile Number" + "\t");
            Response.Output.Write("Vehicle Contact Number" + "\t");
            Response.Output.Write("Capacity(Tons)" + "\t");
            Response.Output.Write("Weight(KG)" + "\t");
            Response.Output.Write("Digital Number Plate" + "\t");
            Response.Output.Write("Price" + "\t");
            Response.Output.Write("Purchase Date" + "\t");
            Response.Output.Write("Purchasing Company" + "\t");
            Response.Output.Write("Financing Company" + "\t");
            Response.Output.Write("KPL Empty" + "\t");
            Response.Output.Write("KPL Loaded" + "\t");
            Response.Output.Write("KPL Loaded Plastic" + "\t");
            Response.Output.Write("KPL InterCity" + "\t");
            Response.Output.Write("KPL InterCHT" + "\t");
            Response.Output.Write("KPL Hill" + "\t");
            Response.Output.Write("KPL OnlyMover" + "\t");
            Response.Output.Write("KPL Loaded 8 To 12 Tons" + "\t");
            Response.Output.Write("KPL Loaded 12 To 25 Tons" + "\t");
            Response.Output.Write("KPL Loaded 8 To 12 Tons Out" + "\t");
            Response.Output.Write("KPL Loaded 12 To 25 Tons Out" + "\t");
            Response.Output.Write("MHT/DHT Vehicle Size" + "\t");
            Response.Output.Write("MHT/DHT Driver Name" + "\t");
            Response.Output.Write("MHT/DHT Driver Licenese Number" + "\t");
            Response.Output.Write("MHT/DHT Driver License Type" + "\t");
            Response.Output.Write("MHT/DHT Driver Contact Number" + "\t");
            Response.Output.Write("MHT/DHT Driver Father Name" + "\t");
            Response.Output.Write("MHT/DHT Driver Village" + "\t");
            Response.Output.Write("MHT/DHT Driver PostOfiice" + "\t");
            Response.Output.Write("MHT/DHT Driver Thana" + "\t");
            Response.Output.Write("MHT/DHT Driver District" + "\t");
            Response.Output.Write("MHT/DHT Driver NID" + "\t");
            Response.Output.Write("MHT/DHT Driver Address Salary" + "\t");
            Response.Output.Write("MHT/DHT Owner Name" + "\t");
            Response.Output.Write("MHT/DHT Owner Father Name" + "\t");
            Response.Output.Write("MHT/DHT Owner Village" + "\t");
            Response.Output.Write("MHT/DHT Owner PostOffice" + "\t");
            Response.Output.Write("MHT/DHT Owner Thana" + "\t");
            Response.Output.Write("MHT/DHT Owner District" + "\t");
            Response.Output.Write("MHT/DHT Owner ContactNumber" + "\t");
            Response.Output.Write("MHT/DHT Owner OwnerNID" + "\t");
            Response.Output.Write("MHT/DHT Owner AgreementFrom" + "\t");
            Response.Output.Write("MHT/DHT Owner AgreementTo" + "\t");
            Response.Output.Write("MHT/DHT Owner MonthlyRentRate" + "\t");

            Response.Output.WriteLine();
            foreach (var item in list)
            {
                Response.Output.Write(item.Model.RegistrationNumber + "\t");
                Response.Output.Write(item.Model.OWN_MHT_DHT + "\t");
                if (item.Model.Company != null)
                {
                    Response.Output.Write(item.Model.Company.Name + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Model.Depo != null)
                {
                    Response.Output.Write(item.Model.Depo.Name + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Model.Depo != null)
                {
                    Response.Output.Write(item.Model.Depo.Category + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Model.DepoGroup != null)
                {
                    Response.Output.Write(item.Model.DepoGroup.Name + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }

                if (item.Model.VehicleModel != null)
                {
                    Response.Output.Write(item.Model.VehicleModel.VehicleBrand.Name + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Model.VehicleModel != null)
                {
                    Response.Output.Write(item.Model.VehicleModel.Title + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                Response.Output.Write(item.RegisrationDate_Text + "\t");
                Response.Output.Write(item.Model.ChassisNumber + "\t");
                Response.Output.Write(item.Model.EngineNumber + "\t");

                if (item.Model.EngineCC != null)
                {
                    Response.Output.Write(item.Model.EngineCC + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                Response.Output.Write(item.Model.VehicleType + "\t");
                Response.Output.Write(item.Model.FuelType + "\t");
                Response.Output.Write(item.Model.GpsIMEINumber + "\t");
                Response.Output.Write(item.Model.GpsMobileNumber + "\t");
                Response.Output.Write(item.Model.Internal_VehicleContactNumber + "\t");

                if (item.Model.CapacityTon != null)
                {
                    Response.Output.Write(item.Model.CapacityTon + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Model.WeightKG != null)
                {
                    Response.Output.Write(item.Model.WeightKG + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                Response.Output.Write(item.NumberPlate_IsDigital_Text + "\t");
                if (item.Model.Internal_VehiclePrice != null)
                {
                    Response.Output.Write(item.Model.Internal_VehiclePrice + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Model.Internal_PurchaseDate != null)
                {
                    Response.Output.Write(item.Internal_PurchaseDate_Text + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Model.Company1 != null)
                {
                    Response.Output.Write(item.Model.Company1.Name + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Model.FinancingCompany != null)
                {
                    Response.Output.Write(item.Model.FinancingCompany.Name + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }

                if (item.Model.Internal_KPL_Empty != null)
                {
                    Response.Output.Write(item.Model.Internal_KPL_Empty + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Model.Internal_KPL_Loaded != null)
                {
                    Response.Output.Write(item.Model.Internal_KPL_Loaded + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Model.Internal_KPL_Loaded_Plastic != null)
                {
                    Response.Output.Write(item.Model.Internal_KPL_Loaded_Plastic + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Model.Internal_KPL_InterCity != null)
                {
                    Response.Output.Write(item.Model.Internal_KPL_InterCity + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Model.Internal_KPL_InterCHT != null)
                {
                    Response.Output.Write(item.Model.Internal_KPL_InterCHT + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Model.Internal_KPL_Hill != null)
                {
                    Response.Output.Write(item.Model.Internal_KPL_Hill + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Model.Internal_KPL_OnlyMover != null)
                {
                    Response.Output.Write(item.Model.Internal_KPL_OnlyMover + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Model.Internal_KPL_Loaded_8_To_12_Tons != null)
                {
                    Response.Output.Write(item.Model.Internal_KPL_Loaded_8_To_12_Tons + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Model.Internal_KPL_Loaded_12_To_25_Tons != null)
                {
                    Response.Output.Write(item.Model.Internal_KPL_Loaded_12_To_25_Tons + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Model.Internal_KPL_Loaded_8_To_12_Tons_Out != null)
                {
                    Response.Output.Write(item.Model.Internal_KPL_Loaded_8_To_12_Tons_Out + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                if (item.Model.Internal_KPL_Loaded_12_To_25_Tons_Out != null)
                {
                    Response.Output.Write(item.Model.Internal_KPL_Loaded_12_To_25_Tons_Out + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }

                if (item.Model.MHT_DHT_VehicleSize != null)
                {
                    Response.Output.Write(item.Model.MHT_DHT_VehicleSize + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                Response.Output.Write(item.Model.MHT_DHT_DriverName + "\t");
                Response.Output.Write(item.Model.MHT_DHT_DriverLiceneseNumber + "\t");
                Response.Output.Write(item.Model.MHT_DHT_DriverLicenseType + "\t");
                Response.Output.Write(item.Model.MHT_DHT_DriverContactNumber + "\t");
                Response.Output.Write(item.Model.MHT_DHT_DriverFatherName + "\t");
                Response.Output.Write(item.Model.MHT_DHT_DriverAddressVillage + "\t");
                Response.Output.Write(item.Model.MHT_DHT_DriverAddressPostOfiice + "\t");
                Response.Output.Write(item.Model.MHT_DHT_DriverAddressThana + "\t");
                Response.Output.Write(item.Model.MHT_DHT_DriverAddressDistrict + "\t");
                Response.Output.Write(item.Model.MHT_DHT_DriverNID + "\t");
                if (item.Model.MHT_DHT_DriverSalary != null)
                {
                    Response.Output.Write(item.Model.MHT_DHT_DriverSalary + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                Response.Output.Write(item.Model.MHT_DHT_OwnerName + "\t");
                Response.Output.Write(item.Model.MHT_DHT_OwnerFatherName + "\t");
                Response.Output.Write(item.Model.MHT_DHT_OwnerAddressVillage + "\t");
                Response.Output.Write(item.Model.MHT_DHT_OwnerAddressPostOffice + "\t");
                Response.Output.Write(item.Model.MHT_DHT_OwnerAddressThana + "\t");
                Response.Output.Write(item.Model.MHT_DHT_OwnerAddressDistrict + "\t");
                Response.Output.Write(item.Model.MHT_DHT_OwnerContactNumber + "\t");
                Response.Output.Write(item.Model.MHT_DHT_OwnerNID + "\t");
                Response.Output.Write(item.MHT_AgreementFrom_Text + "\t");
                Response.Output.Write(item.MHT_AgreementTo_Text + "\t");
                if (item.Model.MHT_MonthlyRentRate != null)
                {
                    Response.Output.Write(item.Model.MHT_MonthlyRentRate + "\t");
                }
                else
                {
                    Response.Output.Write("\t");
                }
                Response.Output.WriteLine();

            }
            Response.End();
        }
        #endregion

        #region //CURENT Sattus
        public ActionResult VehicleCurrentStatusReport()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            ViewBag.Companies = new SelectList(bll.db.Companies.Where(vbm => vbm.IsDeleted == false).OrderBy(m => m.Name), "PK_Company", "Name");
            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && (!m.Category.Contains("Physical")) && accessibleDepoes.Contains(m.PK_Depo)).OrderBy(m => m.Name), "PK_Depo", "Name");

            return View();
        }
        public JsonResult GetVehicleCurrentStatusReport(string FK_Company, string FK_Depo)
        {
            List<Dictionary<string, string>> dictioneryList = new List<Dictionary<string, string>>();

            List<Dictionary<string, string>> finalDictioneryList = new List<Dictionary<string, string>>();
            DataTable dataTable = new DataTable();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter adpt = new SqlDataAdapter();
            cmd.Connection = (SqlConnection)bll.db.Database.Connection;

            cmd.CommandText = "EXEC Report_GetVehicleLastUpdate_withNearMapLocation '" + CurrentUser.PK_User + "'";
            adpt.SelectCommand = cmd;
            dataTable.Reset();
            adpt.Fill(dataTable);
            dictioneryList.AddRange(GetTableRows(dataTable));
            var query = dictioneryList.Where(m => 1 == 1);
            if (FK_Company != "")
            {
                query = query.Where(m => m["FK_Company"] == FK_Company);
            }
            if (FK_Depo != "")
            {
                query = query.Where(m => m["FK_Depo"] == FK_Depo);
            }

            finalDictioneryList = query.ToList();
            return Json(finalDictioneryList, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region //Halt Report
        public ActionResult HaltReport()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            ViewBag.Depoes = new SelectList(bll.db.Depoes.AsEnumerable().Where(c => !c.Category.Contains("Physical") && c.IsDeleted != true && accessibleDepoes.Contains(c.PK_Depo)).OrderBy(m => m.Name).Select(m => new { m.PK_Depo, Name = m.Name }), "PK_Depo", "Name");
            return View();
        }
        public ActionResult ShowMapWithMarker(string latitude, string longitude)
        {
            ViewBag.latitude = latitude;
            ViewBag.longitude = longitude;
            return View();
        }

        public ActionResult ShowMapWith2Markers(string latitude1, string longitude1, string latitude2, string longitude2)
        {
            ViewBag.latitude1 = latitude1;
            ViewBag.longitude1 = longitude1;
            ViewBag.latitude2 = latitude2;
            ViewBag.longitude2 = longitude2;
            return View();
        }

        public JsonResult GetVehicleHaltTime(string FK_Depo, DateTime StartingDate, DateTime EndingDate, string RegistrationNumber, Int32 MinimumMinute)
        {
            DateTime _StartingDate;
            List<Dictionary<string, string>> dictioneryList = new List<Dictionary<string, string>>();
            List<Dictionary<string, string>> _dictioneryList = new List<Dictionary<string, string>>();
            DataTable dataTable = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = int.MaxValue;
            SqlDataAdapter adpt = new SqlDataAdapter();
            cmd.Connection = (SqlConnection)bll.db.Database.Connection;
            string query = "";

            var vehicles_query = bll.db.Vehicles.Where(m => m.IsDeleted == false && m.OWN_MHT_DHT == "OWN" && m.Depo.IsDeleted == false).AsQueryable();
            //# FK_Depo
            if (!string.IsNullOrEmpty(FK_Depo))
            {
                var _FK_Depo = Guid.Parse(FK_Depo);
                vehicles_query = vehicles_query.Where(m => m.FK_Depo == _FK_Depo);
            }
            else
            {
                var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
                vehicles_query = vehicles_query.Where(m => accessibleDepoes.Contains(m.FK_Depo));
            }

            //# RegistrationNumber
            if (!string.IsNullOrEmpty(RegistrationNumber))
            {
                vehicles_query = vehicles_query.Where(m => m.VehicleTrackingInformation.Vehicle.RegistrationNumber.Contains(RegistrationNumber));
            }

            var vehicles = vehicles_query.ToList();

            if (vehicles.Count() > 0)
            {
                foreach (var vehicle in vehicles)
                {
                    _StartingDate = StartingDate;
                    _dictioneryList = new List<Dictionary<string, string>>();
                    while (_StartingDate <= EndingDate)
                    {
                        query = "EXEC Report_GetVehicleHaltTime '" + CurrentUser.PK_User + "', '" + vehicle.PK_Vehicle + "', '" + _StartingDate.ToString() + "', '" + _StartingDate.AddDays(1).ToString() + "', '" + MinimumMinute + "'";
                        cmd.CommandText = query;
                        adpt.SelectCommand = cmd;
                        dataTable.Reset();
                        adpt.Fill(dataTable);
                        _dictioneryList.AddRange(GetTableRows(dataTable));
                        _dictioneryList.Add(new Dictionary<string, string>() { { "_rowType", "NewDate_end" }, { "DateString", _StartingDate.ToString("MMMM dd yyyy") } });
                        if ((EndingDate - _StartingDate).TotalDays > 1)
                        {
                            _StartingDate = _StartingDate.AddDays(1);
                        }
                        else if ((EndingDate - _StartingDate).TotalDays < 1 && _StartingDate != EndingDate)
                        {
                            _StartingDate = EndingDate;
                        }
                        else //_StartingDate == EndingDate;
                        {
                            break;
                        }
                    }
                    var _FK = vehicle.PK_Vehicle;
                    var vehicleRegNumber = bll.db.Vehicles.Where(m => m.PK_Vehicle == _FK).Select(m => m.RegistrationNumber).FirstOrDefault();
                    dictioneryList.Add(new Dictionary<string, string>() { { "_rowType", "NewVehicle_start" }, { "_rowSpan", (_dictioneryList.Count + 1).ToString() }, { "NewVehicle", vehicleRegNumber } });

                    dictioneryList.AddRange(_dictioneryList);
                    dictioneryList.Add(new Dictionary<string, string>() { { "_rowType", "NewVehicle_end" }, { "StartingDate", StartingDate.ToString("MMMM dd yyyy") }, { "EndingDate", EndingDate.ToString("MMMM dd yyyy") } });
                }
            }
            var json_data = Json(dictioneryList, JsonRequestBehavior.AllowGet); json_data.MaxJsonLength = int.MaxValue; return json_data;
        }
        public JsonResult GetVehicleHaltTime_ReadyReport(string FK_Depo, DateTime StartingDate, DateTime EndingDate, string RegistrationNumber, Int32 MinimumMinute)
        {
            DateTime _StartingDate;
            List<Dictionary<string, string>> dictioneryList = new List<Dictionary<string, string>>();
            List<Dictionary<string, string>> _dictioneryList = new List<Dictionary<string, string>>();
            DataTable dataTable = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = int.MaxValue;
            SqlDataAdapter adpt = new SqlDataAdapter();
            cmd.Connection = (SqlConnection)bll.db.Database.Connection;
            string query = "";

            var vehicles_query = bll.db.Vehicles.Where(m => m.IsDeleted == false && m.OWN_MHT_DHT == "OWN" && m.Depo.IsDeleted == false).AsQueryable();
            //# FK_Depo
            if (!string.IsNullOrEmpty(FK_Depo))
            {
                var _FK_Depo = Guid.Parse(FK_Depo);
                vehicles_query = vehicles_query.Where(m => m.FK_Depo == _FK_Depo);
            }
            else
            {
                var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
                vehicles_query = vehicles_query.Where(m => accessibleDepoes.Contains(m.FK_Depo));
            }

            //# RegistrationNumber
            if (!string.IsNullOrEmpty(RegistrationNumber))
            {
                vehicles_query = vehicles_query.Where(m => m.VehicleTrackingInformation.Vehicle.RegistrationNumber.Contains(RegistrationNumber));
            }

            var vehicles = vehicles_query.ToList();

            if (vehicles.Count() > 0)
            {
                foreach (var vehicle in vehicles)
                {
                    _StartingDate = StartingDate;
                    _dictioneryList = new List<Dictionary<string, string>>();
                    while (_StartingDate <= EndingDate)
                    {
                        query = "EXEC Report_GetVehicleHaltTime_ReadyReport '" + vehicle.PK_Vehicle + "', '" + _StartingDate.ToString() + "', '" + _StartingDate.AddDays(1).ToString() + "', '" + MinimumMinute + "'";
                        cmd.CommandText = query;
                        adpt.SelectCommand = cmd;
                        dataTable.Reset();
                        adpt.Fill(dataTable);
                        _dictioneryList.AddRange(GetTableRows(dataTable));
                        _dictioneryList.Add(new Dictionary<string, string>() { { "_rowType", "NewDate_end" }, { "DateString", _StartingDate.ToString("MMMM dd yyyy") } });
                        _StartingDate = _StartingDate.AddDays(1);
                    }
                    var _FK = vehicle.PK_Vehicle;
                    var vehicleRegNumber = bll.db.Vehicles.Where(m => m.PK_Vehicle == _FK).Select(m => m.RegistrationNumber).FirstOrDefault();
                    dictioneryList.Add(new Dictionary<string, string>() { { "_rowType", "NewVehicle_start" }, { "_rowSpan", (_dictioneryList.Count + 1).ToString() }, { "NewVehicle", vehicleRegNumber } });

                    dictioneryList.AddRange(_dictioneryList);
                    dictioneryList.Add(new Dictionary<string, string>() { { "_rowType", "NewVehicle_end" }, { "StartingDate", StartingDate.ToString("MMMM dd yyyy") }, { "EndingDate", EndingDate.ToString("MMMM dd yyyy") } });
                }
            }
            var json_data = Json(dictioneryList, JsonRequestBehavior.AllowGet); json_data.MaxJsonLength = int.MaxValue; return json_data;
        }

        public ActionResult HaltReportDetail()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            var vehicles = (from v in bll.db.Vehicles.Where(m => m.OWN_MHT_DHT == "OWN" && accessibleDepoes.Contains(m.FK_Depo))
                            select v).OrderBy(m => m.RegistrationNumber).ToList();
            ViewBag.Vehicles = new SelectList(vehicles.OrderBy(m => m.RegistrationNumber), "PK_Vehicle", "RegistrationNumber");
            return View();
        }
        #endregion

        #region //Halt Report
        public ActionResult RunAndHaltReport()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            ViewBag.Depoes = new SelectList(bll.db.Depoes.AsEnumerable().Where(c => !c.Category.Contains("Physical") && c.IsDeleted != true && accessibleDepoes.Contains(c.PK_Depo)).OrderBy(m => m.Name).Select(m => new { m.PK_Depo, Name = m.Name }), "PK_Depo", "Name");
            return View();
        }
        [HttpPost]
        public JsonResult GetVehicleHaltAndRunReportData(string FK_Depo, DateTime StartingDate, DateTime EndingDate, string RegistrationNumber)
        {
            DateTime _StartingDate;
            List<Dictionary<string, string>> dictioneryList = new List<Dictionary<string, string>>();
            List<Dictionary<string, string>> _dictioneryList = new List<Dictionary<string, string>>();
            DataTable dataTable = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = int.MaxValue;
            SqlDataAdapter adpt = new SqlDataAdapter();
            cmd.Connection = (SqlConnection)bll.db.Database.Connection;
            string query = "";

            var vehicles_query = bll.db.Vehicles.Where(m => m.IsDeleted == false && m.OWN_MHT_DHT == "OWN" && m.Depo.IsDeleted == false).AsQueryable();
            //# FK_Depo
            if (!string.IsNullOrEmpty(FK_Depo))
            {
                var _FK_Depo = Guid.Parse(FK_Depo);
                vehicles_query = vehicles_query.Where(m => m.FK_Depo == _FK_Depo);
            }
            else
            {
                var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
                vehicles_query = vehicles_query.Where(m => accessibleDepoes.Contains(m.FK_Depo));
            }

            //# RegistrationNumber
            if (!string.IsNullOrEmpty(RegistrationNumber))
            {
                vehicles_query = vehicles_query.Where(m => m.VehicleTrackingInformation.Vehicle.RegistrationNumber.Contains(RegistrationNumber));
            }

            var vehicles = vehicles_query.ToList();

            if (vehicles.Count() > 0)
            {
                foreach (var vehicle in vehicles)
                {
                    _StartingDate = StartingDate;
                    _dictioneryList = new List<Dictionary<string, string>>();
                    while (_StartingDate <= EndingDate)
                    {
                        query = "EXEC Report_GetVehicleRunAndHaltReport '" + CurrentUser.PK_User + "', '" + vehicle.PK_Vehicle + "', '" + _StartingDate.ToString() + "', '" + _StartingDate.AddDays(1).ToString() + "'";
                        cmd.CommandText = query;
                        adpt.SelectCommand = cmd;
                        dataTable.Reset();
                        adpt.Fill(dataTable);
                        _dictioneryList.AddRange(GetTableRows(dataTable));
                        _dictioneryList.Add(new Dictionary<string, string>() { { "_rowType", "NewDate_end" }, { "DateString", _StartingDate.ToString("MMMM dd yyyy") } });
                        if ((EndingDate - _StartingDate).TotalDays > 1)
                        {
                            _StartingDate = _StartingDate.AddDays(1);
                        }
                        else if ((EndingDate - _StartingDate).TotalDays < 1 && _StartingDate != EndingDate)
                        {
                            _StartingDate = EndingDate;
                        }
                        else //_StartingDate == EndingDate;
                        {
                            break;
                        }
                    }
                    var _FK = vehicle.PK_Vehicle;
                    var vehicleRegNumber = bll.db.Vehicles.Where(m => m.PK_Vehicle == _FK).Select(m => m.RegistrationNumber).FirstOrDefault();
                    dictioneryList.Add(new Dictionary<string, string>() { { "_rowType", "NewVehicle_start" }, { "_rowSpan", (_dictioneryList.Count + 1).ToString() }, { "NewVehicle", vehicleRegNumber } });

                    dictioneryList.AddRange(_dictioneryList);
                    dictioneryList.Add(new Dictionary<string, string>() { { "_rowType", "NewVehicle_end" }, { "StartingDate", StartingDate.ToString("MMMM dd yyyy") }, { "EndingDate", EndingDate.ToString("MMMM dd yyyy") } });
                }
            }
            var json_data = Json(dictioneryList, JsonRequestBehavior.AllowGet); json_data.MaxJsonLength = int.MaxValue; return json_data;
        }
        #endregion

        #region //ConsolidatedReport
        public ActionResult ConsolidatedReport()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            ViewBag.Depoes = new SelectList(bll.db.Depoes.AsEnumerable().Where(c => !c.Category.Contains("Physical") && c.IsDeleted != true && accessibleDepoes.Contains(c.PK_Depo)).OrderBy(m => m.Name).Select(m => new { m.PK_Depo, Name = m.Name + " [" + m.Vehicles.Count + " vehicles]" }), "PK_Depo", "Name");
            return View();
        }
        public ActionResult ConsolidatedReport_()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.Depo.PRG_Type != null && m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true && m.Depo.Vehicles.Where(n => n.IsDeleted != true && n.GpsIMEINumber != null).Count() > 0).Select(m => new { m.FK_Depo, m.Depo.Name, VehicleCount = m.Depo.Vehicles.Where(n => n.IsDeleted != true && n.GpsIMEINumber != null).Count() }).ToList();
            List<SelectListItem> Vehicles = new List<SelectListItem>();
            Vehicles.Add(new SelectListItem() { Value = "all_vehicles", Text = "All Vehicles" });
            foreach (var item in accessibleDepoes)
            {
                Vehicles.Add(new SelectListItem() { Value = "FK_Depo=" + item.FK_Depo, Text = "All of " + item.Name + " (" + item.VehicleCount + " vehicles)" });
            }
            Vehicles.AddRange(bll.db.Vehicles.AsEnumerable().Where(m => m.IsDeleted != true && m.GpsIMEINumber != null && accessibleDepoes.Select(n => n.FK_Depo).Contains(m.FK_Depo)).OrderBy(m => m.RegistrationNumber).Select(m => new SelectListItem { Value = m.PK_Vehicle.ToString(), Text = m.RegistrationNumber }));
            ViewBag.Vehicles = new SelectList(Vehicles, "Value", "Text");
            return View();
        }
        public JsonResult GetConsolidatedReport(string FK_Depo, DateTime StartingDate, DateTime EndingDate, string RegistrationNumber)
        {
            DateTime _StartingDate;
            List<Dictionary<string, string>> dictioneryList = new List<Dictionary<string, string>>();
            List<Dictionary<string, string>> _dictioneryList = new List<Dictionary<string, string>>();

            DataTable dataTable = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = int.MaxValue;
            SqlDataAdapter adpt = new SqlDataAdapter();
            cmd.Connection = (SqlConnection)bll.db.Database.Connection;
            string query = "";

            var vehicles_query = bll.db.Vehicles.Where(m => m.IsDeleted == false && m.OWN_MHT_DHT == "OWN" && m.Depo.IsDeleted == false).AsQueryable();
            //# FK_Depo
            if (!string.IsNullOrEmpty(FK_Depo))
            {
                var _FK_Depo = Guid.Parse(FK_Depo);
                vehicles_query = vehicles_query.Where(m => m.FK_Depo == _FK_Depo);
            }

            //# RegistrationNumber
            if (!string.IsNullOrEmpty(RegistrationNumber))
            {
                vehicles_query = vehicles_query.Where(m => m.VehicleTrackingInformation.Vehicle.RegistrationNumber.Contains(RegistrationNumber));
            }

            var vehicles = vehicles_query.ToList();

            if (vehicles.Count() > 0)
            {
                foreach (var vehicle in vehicles)
                {
                    _StartingDate = StartingDate;
                    _dictioneryList = new List<Dictionary<string, string>>();

                    while (_StartingDate <= EndingDate)
                    {
                        query = "EXEC Report_GetVehicleConsolidatedReport '" + CurrentUser.PK_User + "', '" + vehicle.PK_Vehicle + "', '" + _StartingDate.ToString() + "'";
                        cmd.CommandText = query;
                        adpt.SelectCommand = cmd;
                        dataTable.Reset();
                        adpt.Fill(dataTable);
                        _dictioneryList.AddRange(GetTableRows(dataTable));
                        _StartingDate = _StartingDate.AddDays(1);
                    }
                    var _FK = vehicle.PK_Vehicle;
                    var vehicleRegNumber = bll.db.Vehicles.Where(m => m.PK_Vehicle == _FK).Select(m => m.RegistrationNumber).FirstOrDefault();
                    var vehicleMobileNumber = bll.db.Vehicles.Where(m => m.PK_Vehicle == _FK).Select(m => m.Internal_VehicleContactNumber).FirstOrDefault();
                    dictioneryList.Add(new Dictionary<string, string>() { { "_rowType", "NewVehicle_start" }, { "_rowSpan", ((_dictioneryList.Where(m => m["_rowType"] == "data_consolidated")).Count() + 2).ToString() }, { "NewVehicle", vehicleRegNumber }, { "NewVehicle_mobileNumber", vehicleMobileNumber } });

                    dictioneryList.AddRange(_dictioneryList);
                    dictioneryList.Add(new Dictionary<string, string>() { { "_rowType", "NewVehicle_end" }, { "StartingDate", StartingDate.ToString("MMMM dd yyyy") }, { "EndingDate", EndingDate.ToString("MMMM dd yyyy") } });

                }
            }
            var json_data = Json(dictioneryList, JsonRequestBehavior.AllowGet); json_data.MaxJsonLength = int.MaxValue; return json_data;
        }
        public JsonResult GetConsolidatedReport_ReadyReport(string FK_Depo, DateTime StartingDate, DateTime EndingDate, string RegistrationNumber)
        {
            DateTime _StartingDate;
            List<Dictionary<string, string>> dictioneryList = new List<Dictionary<string, string>>();
            List<Dictionary<string, string>> _dictioneryList = new List<Dictionary<string, string>>();
            DataTable dataTable = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = int.MaxValue;
            SqlDataAdapter adpt = new SqlDataAdapter();
            cmd.Connection = (SqlConnection)bll.db.Database.Connection;
            string query = "";

            var vehicles_query = bll.db.Vehicles.Where(m => m.IsDeleted == false && m.OWN_MHT_DHT == "OWN" && m.Depo.IsDeleted == false).AsQueryable();
            //# FK_Depo
            if (!string.IsNullOrEmpty(FK_Depo))
            {
                var _FK_Depo = Guid.Parse(FK_Depo);
                vehicles_query = vehicles_query.Where(m => m.FK_Depo == _FK_Depo);
            }

            //# RegistrationNumber
            if (!string.IsNullOrEmpty(RegistrationNumber))
            {
                vehicles_query = vehicles_query.Where(m => m.VehicleTrackingInformation.Vehicle.RegistrationNumber.Contains(RegistrationNumber));
            }

            var vehicles = vehicles_query.ToList();

            if (vehicles.Count() > 0)
            {
                foreach (var vehicle in vehicles)
                {
                    _StartingDate = StartingDate;
                    _dictioneryList = new List<Dictionary<string, string>>();
                    while (_StartingDate <= EndingDate)
                    {
                        query = "EXEC Report_GetVehicleConsolidatedReport_ReadyReport '" + vehicle.PK_Vehicle + "', '" + _StartingDate.ToString() + "'";
                        cmd.CommandText = query;
                        adpt.SelectCommand = cmd;
                        dataTable.Reset();
                        adpt.Fill(dataTable);
                        _dictioneryList.AddRange(GetTableRows(dataTable));
                        _dictioneryList.Add(new Dictionary<string, string>() { { "_rowType", "NewDate_end" }, { "DateString", _StartingDate.ToString("MMMM dd yyyy") } });
                        _StartingDate = _StartingDate.AddDays(1);
                    }
                    var _FK = vehicle.PK_Vehicle;
                    var vehicleRegNumber = bll.db.Vehicles.Where(m => m.PK_Vehicle == _FK).Select(m => m.RegistrationNumber).FirstOrDefault();
                    dictioneryList.Add(new Dictionary<string, string>() { { "_rowType", "NewVehicle_start" }, { "_rowSpan", ((_dictioneryList.Where(m => m["_rowType"] == "data_consolidated")).Count() + 2).ToString() }, { "NewVehicle", vehicleRegNumber } });

                    dictioneryList.AddRange(_dictioneryList);
                    dictioneryList.Add(new Dictionary<string, string>() { { "_rowType", "NewVehicle_end" }, { "StartingDate", StartingDate.ToString("MMMM dd yyyy") }, { "EndingDate", EndingDate.ToString("MMMM dd yyyy") } });
                }
            }
            var json_data = Json(dictioneryList, JsonRequestBehavior.AllowGet); json_data.MaxJsonLength = int.MaxValue; return json_data;
        }
        #endregion


        #region //Vehicle History
        public ActionResult VehicleHistory()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            var vehicles = (from v in bll.db.Vehicles.Where(m => m.OWN_MHT_DHT == "OWN" && accessibleDepoes.Contains(m.FK_Depo))
                            select v).OrderBy(m => m.RegistrationNumber).ToList();
            ViewBag.Vehicles = new SelectList(vehicles.OrderBy(m => m.RegistrationNumber), "PK_Vehicle", "RegistrationNumber");
            return View();
        }
        public JsonResult GetVehicleHistory(string FK_Vehicle, DateTime StartingDate, DateTime EndingDate, Int32 TimeLap)
        {
            List<Dictionary<string, string>> dictioneryList = new List<Dictionary<string, string>>();
            DataTable dataTable = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = int.MaxValue;
            SqlDataAdapter adpt = new SqlDataAdapter();
            cmd.Connection = (SqlConnection)bll.db.Database.Connection;
            string query = "";
            query = "EXEC Report_GetVehicleHistory '" + CurrentUser.PK_User + "', '" + FK_Vehicle + "', '" + StartingDate.ToString() + "', '" + EndingDate.ToString() + "', " + TimeLap;
            cmd.CommandText = query;
            adpt.SelectCommand = cmd;
            dataTable.Reset();
            adpt.Fill(dataTable);
            dictioneryList.AddRange(GetTableRows(dataTable));

            decimal trackingDistanceInKiloMeter = 0;
            if (dictioneryList.Count > 1)
            {
                var startingMileage = dictioneryList.Where(m => m["Mileage"] != "").Select(m => m["Mileage"]).FirstOrDefault();
                var endingMileage = dictioneryList.Where(m => m["Mileage"] != "").Select(m => m["Mileage"]).LastOrDefault();
                if (!string.IsNullOrEmpty(startingMileage) && !string.IsNullOrEmpty(endingMileage) && startingMileage != endingMileage)
                {
                    trackingDistanceInKiloMeter = Math.Round((Convert.ToDecimal(endingMileage) - Convert.ToDecimal(startingMileage)) / 1000, 2);
                }
            }
            var json_data = Json(new { trackingDistanceInKiloMeter, list = dictioneryList }, JsonRequestBehavior.AllowGet); json_data.MaxJsonLength = int.MaxValue; return json_data;
        }
        public ActionResult VehicleHistory2()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            var vehicles = (from v in bll.db.Vehicles.Where(m => m.OWN_MHT_DHT == "OWN" && accessibleDepoes.Contains(m.FK_Depo))
                            select v).OrderBy(m => m.RegistrationNumber).ToList();
            ViewBag.Vehicles = new SelectList(vehicles.OrderBy(m => m.RegistrationNumber), "PK_Vehicle", "RegistrationNumber");
            return View();
        }
        #endregion

        #region //Vehicle Depo InOur
        public ActionResult VehicleInOutHistory()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            var vehicles = (from v in bll.db.Vehicles.Where(m => m.OWN_MHT_DHT == "OWN" && accessibleDepoes.Contains(m.FK_Depo))
                            select v).OrderBy(m => m.RegistrationNumber).ToList();
            ViewBag.Vehicles = new SelectList(vehicles.OrderBy(m => m.RegistrationNumber), "PK_Vehicle", "RegistrationNumber");
            return View();
        }
        public JsonResult GetVehicleInOutHistory(string FK_Vehicle, DateTime StartingDate, DateTime EndingDate)
        {
            var query = "EXEC Report_GetVehicleInOutHistoryDetail '" + CurrentUser.PK_User + "', '" + FK_Vehicle + "', '" + StartingDate.ToString() + "', '" + EndingDate.ToString() + "'";
            var _list = bll.db.Report_VehicleInOutHistoryDetail.SqlQuery(query).ToList();
            var vm_list = new List<VM_VehicleInOut>();

            if (_list.Count == 0)
            {
                return Json(vm_list, JsonRequestBehavior.AllowGet);
            }
            var isIn = true;

            var gap = new VM_VehicleInOut();
            if (_list.FirstOrDefault().FK_Depo != null)
            {
                isIn = true;
                gap.In = true;
                gap.StartDateTime = StartingDate;
                gap.StartDateTimeString = CommonClass.ConvertToDateTimeString(StartingDate);
                var _FK_Depo = _list.FirstOrDefault().FK_Depo;
                gap.DepoName = bll.db.Depoes.Where(m => m.PK_Depo == _FK_Depo).FirstOrDefault().Name;
                gap.Latitude = _list.FirstOrDefault().Latitude;
                gap.Longitude = _list.FirstOrDefault().Longitude;
            }
            else
            {
                isIn = false;
                gap.In = false;
                gap.StartDateTime = StartingDate;
                gap.StartDateTimeString = CommonClass.ConvertToDateTimeString(StartingDate);
                gap.Latitude = _list.FirstOrDefault().Latitude;
                gap.Longitude = _list.FirstOrDefault().Longitude;
            }

            foreach (var item in _list)
            {
                if (isIn == false && item.FK_Depo != null)
                {
                    isIn = true;

                    gap.EndDateTime = item.UpdateTime;
                    gap.EndDateTimeString = CommonClass.ConvertToDateTimeString(item.UpdateTime);
                    gap.GapTime = gap.EndDateTime - gap.StartDateTime;
                    gap.GapTimeString = (gap.GapTime.Days > 0 ? gap.GapTime.Days + " Day(s) " : "") + (gap.GapTime.Hours > 0 ? gap.GapTime.Hours + " Hour(s) " : "") + (gap.GapTime.Minutes > 0 ? gap.GapTime.Minutes + " Min(s) " : "");
                    vm_list.Add(gap);

                    gap = new VM_VehicleInOut();
                    gap.In = true;
                    gap.StartDateTime = item.UpdateTime;
                    gap.StartDateTimeString = CommonClass.ConvertToDateTimeString(item.UpdateTime);
                    gap.DepoName = bll.db.Depoes.Where(m => m.PK_Depo == item.FK_Depo).FirstOrDefault().Name;
                    gap.Latitude = item.Latitude;
                    gap.Longitude = item.Longitude;
                }
                else if (isIn == true && item.FK_Depo == null)
                {
                    isIn = false;

                    gap.EndDateTime = item.UpdateTime;
                    gap.EndDateTimeString = CommonClass.ConvertToDateTimeString(item.UpdateTime);
                    gap.GapTime = gap.EndDateTime - gap.StartDateTime;
                    gap.GapTimeString = (gap.GapTime.Days > 0 ? gap.GapTime.Days + " Day(s) " : "") + (gap.GapTime.Hours > 0 ? gap.GapTime.Hours + " Hour(s) " : "") + (gap.GapTime.Minutes > 0 ? gap.GapTime.Minutes + " Min(s) " : "");
                    vm_list.Add(gap);

                    gap = new VM_VehicleInOut();
                    gap.In = false;
                    gap.StartDateTime = item.UpdateTime;
                    gap.StartDateTimeString = CommonClass.ConvertToDateTimeString(item.UpdateTime);
                    gap.Latitude = item.Latitude;
                    gap.Longitude = item.Longitude;
                }
            }

            gap.EndDateTime = EndingDate;
            gap.EndDateTimeString = CommonClass.ConvertToDateTimeString(EndingDate);
            gap.GapTime = gap.EndDateTime - gap.StartDateTime;
            gap.GapTimeString = (gap.GapTime.Days > 0 ? gap.GapTime.Days + " Day(s) " : "") + (gap.GapTime.Hours > 0 ? gap.GapTime.Hours + " Hour(s) " : "") + (gap.GapTime.Minutes > 0 ? gap.GapTime.Minutes + " Min(s) " : "");
            vm_list.Add(gap);

            //# Remove unaccepted gap
            var minTimeSpan = new TimeSpan(0, 10, 0);//# 10 min
            for (int i = 0; i < vm_list.Count; i++)
            {
                var currentItem = vm_list[i];
                if (vm_list[i].GapTime < minTimeSpan)
                {
                    if (i != 0 && i != (vm_list.Count - 1))//# Not he first/last item
                    {
                        var previousItem = vm_list[i - 1];
                        var nextItem = vm_list[i + 1];
                        previousItem.EndDateTime = nextItem.EndDateTime;
                        previousItem.EndDateTimeString = CommonClass.ConvertToDateTimeString(nextItem.EndDateTime);
                        previousItem.GapTime = nextItem.EndDateTime - previousItem.StartDateTime;
                        previousItem.GapTimeString = (previousItem.GapTime.Days > 0 ? previousItem.GapTime.Days + " Day(s) " : "") + (previousItem.GapTime.Hours > 0 ? previousItem.GapTime.Hours + " Hour(s) " : "") + (previousItem.GapTime.Minutes > 0 ? previousItem.GapTime.Minutes + " Min(s) " : "");
                        vm_list.Remove(currentItem);
                        vm_list.Remove(nextItem);
                    }
                }
            }

            //# total in time
            TimeSpan totalInDepoTime = new TimeSpan(0, 0, 0);
            foreach (var item in vm_list.Where(m => m.In == true).ToList())
            {
                totalInDepoTime = totalInDepoTime.Add(item.GapTime);
            }
            vm_list.Add(new VM_VehicleInOut()
            {
                In = true,
                GapTimeString = (totalInDepoTime.Days > 0 ? totalInDepoTime.Days + " Day(s) " : "") + (totalInDepoTime.Hours > 0 ? totalInDepoTime.Hours + " Hour(s) " : "") + (totalInDepoTime.Minutes > 0 ? totalInDepoTime.Minutes + " Min(s) " : "")
            });

            //# total out time
            TimeSpan totalOutDepoTime = new TimeSpan(0, 0, 0);
            foreach (var item in vm_list.Where(m => m.In == false).ToList())
            {
                totalOutDepoTime = totalOutDepoTime.Add(item.GapTime);
            }
            vm_list.Add(new VM_VehicleInOut()
            {
                In = true,
                GapTimeString = (totalOutDepoTime.Days > 0 ? totalOutDepoTime.Days + " Day(s) " : "") + (totalOutDepoTime.Hours > 0 ? totalOutDepoTime.Hours + " Hour(s) " : "") + (totalOutDepoTime.Minutes > 0 ? totalOutDepoTime.Minutes + " Min(s) " : "")
            });
            return Json(vm_list, JsonRequestBehavior.AllowGet);
        }
        public class VM_VehicleInOut
        {
            public bool In { get; set; }
            public DateTime StartDateTime { get; set; }
            public string StartDateTimeString { get; set; }
            public DateTime EndDateTime { get; set; }
            public string EndDateTimeString { get; set; }
            public TimeSpan GapTime { get; set; }
            public string GapTimeString { get; set; }
            public string DepoName { get; set; }
            public Nullable<double> Latitude { get; set; }
            public Nullable<double> Longitude { get; set; }
        }
        #endregion

        #region //Vehicle History Detail
        public ActionResult VehicleHistoryDetail()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            var vehicles = (from v in bll.db.Vehicles.Where(m => m.OWN_MHT_DHT == "OWN" && accessibleDepoes.Contains(m.FK_Depo))
                            select v).OrderBy(m => m.RegistrationNumber).ToList();
            ViewBag.Vehicles = new SelectList(vehicles.OrderBy(m => m.RegistrationNumber), "PK_Vehicle", "RegistrationNumber");
            return View();
        }
        public JsonResult GetVehicleHistoryDetail(string FK_Vehicle, DateTime StartingDate, DateTime EndingDate)
        {
            List<Dictionary<string, string>> dictioneryList = new List<Dictionary<string, string>>();
            DataTable dataTable = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = int.MaxValue;
            SqlDataAdapter adpt = new SqlDataAdapter();
            cmd.Connection = (SqlConnection)bll.db.Database.Connection;
            string query = "";
            query = "EXEC Report_GetVehicleHistoryDetail '" + CurrentUser.PK_User + "', '" + FK_Vehicle + "', '" + StartingDate.ToString() + "', '" + EndingDate.ToString() + "'";
            cmd.CommandText = query;
            adpt.SelectCommand = cmd;
            dataTable.Reset();
            adpt.Fill(dataTable);
            dictioneryList.AddRange(GetTableRows(dataTable));
            var json_data = Json(dictioneryList, JsonRequestBehavior.AllowGet); json_data.MaxJsonLength = int.MaxValue; return json_data;
        }
        #endregion

        #region //Temperature Report
        public ActionResult TemperatureHistory()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.Depo.PRG_Type != null && m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true && m.Depo.Vehicles.Where(n => n.IsDeleted != true && n.GpsIMEINumber != null && n.Internal_ShowTemperature == true).Count() > 0).Select(m => new { m.FK_Depo, m.Depo.Name, VehicleCount = m.Depo.Vehicles.Where(n => n.IsDeleted != true && n.GpsIMEINumber != null && n.Internal_ShowTemperature == true).Count() }).ToList();
            List<SelectListItem> Vehicles = new List<SelectListItem>();
            //Vehicles.Add(new SelectListItem() { Value = "all_vehicles", Text = "All Vehicles" });
            foreach (var item in accessibleDepoes)
            {
                Vehicles.Add(new SelectListItem() { Value = "FK_Depo=" + item.FK_Depo, Text = "All of " + item.Name + " (" + item.VehicleCount + " vehicles)" });
            }
            Vehicles.AddRange(bll.db.Vehicles.AsEnumerable().Where(m => m.IsDeleted != true && m.GpsIMEINumber != null && m.Internal_ShowTemperature == true && accessibleDepoes.Select(n => n.FK_Depo).Contains(m.FK_Depo)).OrderBy(m => m.RegistrationNumber).Select(m => new SelectListItem { Value = m.PK_Vehicle.ToString(), Text = m.RegistrationNumber }));
            ViewBag.Vehicles = new SelectList(Vehicles, "Value", "Text");
            return View();
        }
        public JsonResult GetTemperatureHistory(string FK_Vehicles, DateTime StartingDate, DateTime EndingDate, Int32 IntervalMinute)
        {
            List<Dictionary<string, string>> dictioneryList = new List<Dictionary<string, string>>();
            DataTable dataTable = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = int.MaxValue;
            SqlDataAdapter adpt = new SqlDataAdapter();
            cmd.Connection = (SqlConnection)bll.db.Database.Connection;
            string query = "";
            query = "EXEC Report_GetTemperatureHistory '";
            if (CurrentUser.UniqueIDNumber == "217700")
            {
                query = "EXEC Report_GetTemperatureHistory_RE '";
            }
            if (FK_Vehicles.Contains("all_vehicles"))
            {
                var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
                var PK_Vehicles = bll.db.Vehicles.Where(m => m.IsDeleted != true && m.Internal_ShowTemperature == true && accessibleDepoes.Contains(m.FK_Depo)).Select(m => m.PK_Vehicle).ToList();
                foreach (var PK in PK_Vehicles)
                {
                    query = query + CurrentUser.PK_User + "', '" + PK + "', '" + StartingDate.ToString() + "', '" + EndingDate.ToString() + "', '" + IntervalMinute + "'";

                    cmd.CommandText = query;
                    adpt.SelectCommand = cmd;
                    dataTable.Reset();
                    adpt.Fill(dataTable);
                    dictioneryList.AddRange(GetTableRows(dataTable));

                }
            }
            else if (FK_Vehicles.Contains("FK_Depo="))
            {
                var FK_DepoVehicle_list = FK_Vehicles.Split(',');
                if (FK_DepoVehicle_list.Count() > 0)
                {
                    foreach (var FK_DepoVehicle in FK_DepoVehicle_list)
                    {
                        if (FK_DepoVehicle == "")
                        {
                            break;
                        }
                        else if (FK_DepoVehicle.Contains("FK_Depo="))
                        {
                            Guid FK_Depo = Guid.Parse(FK_DepoVehicle.Replace("FK_Depo=", ""));
                            var FK_Vehicle_list = bll.db.Vehicles.Where(m => m.IsDeleted != true && m.Internal_ShowTemperature == true && m.FK_Depo == FK_Depo).Select(m => m.PK_Vehicle).ToList();
                            foreach (var FK_Vehicle in FK_Vehicle_list)
                            {
                                query = query + CurrentUser.PK_User + "', '" + FK_Vehicle + "', '" + StartingDate.ToString() + "', '" + EndingDate.ToString() + "', '" + IntervalMinute + "'";

                                cmd.CommandText = query;
                                adpt.SelectCommand = cmd;
                                dataTable.Reset();
                                adpt.Fill(dataTable);
                                dictioneryList.AddRange(GetTableRows(dataTable));
                            }
                        }
                        else
                        {
                            query = query + CurrentUser.PK_User + "', '" + FK_DepoVehicle + "', '" + StartingDate.ToString() + "', '" + EndingDate.ToString() + "', '" + IntervalMinute + "'";

                            cmd.CommandText = query;
                            adpt.SelectCommand = cmd;
                            dataTable.Reset();
                            adpt.Fill(dataTable);
                            dictioneryList.AddRange(GetTableRows(dataTable));
                        }
                    }
                }
            }
            else
            {
                var FK_Vehicle_list = FK_Vehicles.Split(',');
                if (FK_Vehicle_list.Count() > 0)
                {
                    foreach (var FK_Vehicle in FK_Vehicle_list)
                    {
                        if (FK_Vehicle == "")
                        {
                            break;
                        }
                        query = query + CurrentUser.PK_User + "', '" + FK_Vehicle + "', '" + StartingDate.ToString() + "', '" + EndingDate.ToString() + "', '" + IntervalMinute + "'";

                        cmd.CommandText = query;
                        adpt.SelectCommand = cmd;
                        dataTable.Reset();
                        adpt.Fill(dataTable);
                        dictioneryList.AddRange(GetTableRows(dataTable));
                    }
                }
            }
            var json_data = Json(dictioneryList, JsonRequestBehavior.AllowGet); json_data.MaxJsonLength = int.MaxValue; return json_data;
        }
        #endregion


    }
}