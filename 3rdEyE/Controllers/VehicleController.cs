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
using Microsoft.Reporting.WebForms;
using System.ComponentModel;

namespace _3rdEyE.Controllers
{
    public class VehicleController : BaseController
    {
        BLL_Vehicle bll = new BLL_Vehicle();

        Dictionary<string, string> VehicleTypesDict = new Dictionary<string, string> { { "Ambulance", "Ambulance" }, { "Bus", "Bus" }, { "Cargo Truck", "Cargo Truck" }, { "Cargo Truck - Open", "Cargo Truck - Open" }, { "Cargo VAN", "Cargo VAN" }, { "Open VAN", "Open VAN" }, { "Concrete Mixer", "Concrete Mixer" }, { "Covered Van", "Covered Van" }, { "Delivery Van", "Delivery Van" }, { "Milk Tanker", "Milk Tanker" }, { "Micro Bus", "Micro Bus" }, { "Mini Bus", "Mini Bus" }, { "Mini Truck", "Mini Truck" }, { "Mobile Crance", "Mobile Crance" }, { "Motor Car", "Motor Car" }, { "OMNI Bus", "OMNI Bus" }, { "Pickup", "Pickup" }, { "Refrigerator Van", "Refrigerator Van" }, { "Tank Lorry", "Tank Lorry" }, { "Tipper", "Tipper" }, { "Trailers", "Trailers" }, { "Water Tanker", "Water Tanker" }, { "Tractor", "Tractor" }, { "Motor Cycle", "Motor Cycle" } };
        Dictionary<string, string> FuelTypesDict = new Dictionary<string, string> { { "CNG", "CNG" }, { "Diesel", "Diesel" }, { "Octen", "Octen" }, { "Petrol", "Petrol" } };
        Dictionary<double, double> CapacityTonDict = new Dictionary<double, double> { { 0.8, 0.8 }, { 1, 1 }, { 1.5, 1.5 }, { 2, 2 }, { 3, 3 }, { 5, 5 }, { 7, 7 }, { 10, 10 }, { 12, 12 }, { 15, 15 }, { 20, 20 }, { 25, 25 } };
        Dictionary<bool, string> NumberPlate_StatusesDict = new Dictionary<bool, string> { { true, "Digital" }, { false, "Manual" } };
        Dictionary<bool, string> ShowTemperatureDict = new Dictionary<bool, string> { { false, "No" }, { true, "Yes" } };
        //Dictionary<string, string> GpsDeviceModelsDict = new Dictionary<string, string> { { "Meitrack T1", "Meitrack T1" }, { "Meitrack T366", "Meitrack T366" }, { "Zenda VT1", "Zenda VT1" } };
        Dictionary<string, string> MHT_DHT_DriverLicenseTypeDict = new Dictionary<string, string> { { "Paper", "Paper" }, { "Digital", "Digital" } };
        Dictionary<double, double> MHT_DHT_VehicleSizeDict = new Dictionary<double, double> { { 8, 8 }, { 9, 9 }, { 10, 10 }, { 11, 11 }, { 12, 12 }, { 13, 13 }, { 14, 14 }, { 15, 15 }, { 16, 16 }, { 17, 17 }, { 18, 18 }, { 19, 19 }, { 20, 20 }, { 21, 21 }, { 22, 22 }, { 23, 23 }, { 24, 24 }, { 25, 25 }, { 26, 26 }, { 27, 27 }, { 28, 28 }, { 29, 29 }, { 30, 30 }, { 31, 31 }, { 32, 32 }, { 33, 33 }, { 34, 34 }, { 35, 35 }, { 36, 36 }, { 37, 37 }, { 38, 38 }, { 39, 39 }, { 40, 40 } };
        Dictionary<string, string> OWN_MHT_DHT_Dict = new Dictionary<string, string> { { "OWN", "OWN" }, { "MHT", "MHT" }, { "DHT", "DHT" } };
        public ActionResult VehicleDashBoard()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            return View();
        }
        public JsonResult GetVehicleDashBoardData()
        {
            if (CurrentUser.PRG_Type == "ALL")
            {
                var pran = (from depo in bll.db.Depoes.Where(m => m.IsDeleted != true)
                            where depo.PRG_Type == "PRAN"
                            select new
                            {
                                depo.Name,
                                vehicleCount = depo.Vehicles.Where(m => m.IsDeleted != true && m.OWN_MHT_DHT == "OWN").Count()
                            }).ToList();
                var rfl = (from depo in bll.db.Depoes.Where(m => m.IsDeleted != true)
                           where depo.PRG_Type == "RFL"
                           select new
                           {
                               depo.Name,
                               vehicleCount = depo.Vehicles.Where(m => m.IsDeleted != true && m.OWN_MHT_DHT == "OWN").Count()
                           }).ToList();
                return Json(new { pran = pran, rfl = rfl }, JsonRequestBehavior.AllowGet);

            }
            else if (CurrentUser.PRG_Type == "PRAN")
            {
                var pran = (from depo in bll.db.Depoes.Where(m => m.IsDeleted != true)
                            where depo.PRG_Type == "PRAN"
                            select new
                            {
                                depo.Name,
                                vehicleCount = depo.Vehicles.Where(m => m.IsDeleted != true && m.OWN_MHT_DHT == "OWN").Count()
                            }).ToList();
                return Json(new { pran = pran }, JsonRequestBehavior.AllowGet);
            }
            else if (CurrentUser.PRG_Type == "RFL")
            {
                var rfl = (from depo in bll.db.Depoes.Where(m => m.IsDeleted != true)
                           where depo.PRG_Type == "RFL"
                           select new
                           {
                               depo.Name,
                               vehicleCount = depo.Vehicles.Where(m => m.IsDeleted != true && m.OWN_MHT_DHT == "OWN").Count()
                           }).ToList();
                return Json(new { rfl = rfl }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Index(String FK_Depo, String FK_Company, string OWN_MHT_DHT, String FK_DepoGroup, String RegistrationNumber)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            List<ViewModels.VM_Vehicle> list = new List<ViewModels.VM_Vehicle>();
            var query = bll.db.Vehicles.AsEnumerable().Where(c => c.IsDeleted == false);

            //# FK_Depo
            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            ViewBag.Depoes = new SelectList(bll.db.Depoes.AsEnumerable().Where(m => m.IsDeleted == false && accessibleDepoes.Contains(m.PK_Depo)).OrderBy(m => m.Name), "PK_Depo", "Name", FK_Depo);
            if (FK_Depo != null)
            {
                var _FK_Depo = Guid.Parse(FK_Depo);
                query = query.Where(m => m.FK_Depo == _FK_Depo);
            }
            else
            {
                accessibleDepoes.Add(Guid.Parse("00000000-0000-0000-0000-000000000000"));
                query = query.Where(m => accessibleDepoes.Contains(m.FK_Depo));
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
            ViewBag.DepoGroups = new SelectList(bll.db.DepoGroups.Where(m => m.IsDeleted == false && accessibleDepoes.Contains(m.FK_Depo)).OrderBy(m => m.Name), "PK_DepoGroup", "Name", FK_DepoGroup);
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
                list = query.Select(m => bll.ConvertToViewModel(m)).ToList();
            }

            return View(list);
        }

        public void ExportVehicleList(String FK_Depo, String FK_Company, string OWN_MHT_DHT, String FK_DepoGroup, String RegistrationNumber)
        {
            List<ViewModels.VM_Vehicle> list = new List<ViewModels.VM_Vehicle>();
            var query = bll.db.Vehicles.AsEnumerable().Where(c => c.IsDeleted == false);

            //# FK_Depo
            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            ViewBag.Depoes = new SelectList(bll.db.Depoes.AsEnumerable().Where(m => m.IsDeleted == false && accessibleDepoes.Contains(m.PK_Depo)).OrderBy(m => m.Name), "PK_Depo", "Name", FK_Depo);
            if (FK_Depo != null)
            {
                var _FK_Depo = Guid.Parse(FK_Depo);
                query = query.Where(m => m.FK_Depo == _FK_Depo);
            }
            else
            {
                accessibleDepoes.Add(Guid.Parse("00000000-0000-0000-0000-000000000000"));
                query = query.Where(m => accessibleDepoes.Contains(m.FK_Depo));
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
            ViewBag.DepoGroups = new SelectList(bll.db.DepoGroups.Where(m => m.IsDeleted == false && accessibleDepoes.Contains(m.FK_Depo)).OrderBy(m => m.Name), "PK_DepoGroup", "Name", FK_DepoGroup);
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
                list = query.Select(m => bll.ConvertToViewModel(m)).ToList();
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

        public ActionResult ReportIndex()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var list = bll.GetAllViewModels();
            return View(list);
        }

        #region //OWN
        public ActionResult Create_OWN()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }


            ViewBag.UserCompanies = new SelectList(bll.db.Companies.Where(m => m.IsDeleted == false && m.GroupOfCompany.IsPranRFLGroup == true && m.IsUserCompany == true).OrderBy(m => m.Name), "PK_Company", "Name");
            var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && accessibleDepoes.Contains(m.PK_Depo) && m.PK_Depo != _invalidDepoPK).OrderBy(m => m.Name), "PK_Depo", "Name");
            ViewBag.DepoGroups = new SelectList(bll.db.DepoGroups.Where(m => m.IsDeleted == false && m.PK_DepoGroup == null).OrderBy(m => m.Name), "PK_DepoGroup", "Name");
            ViewBag.VehicleBrands = new SelectList(bll.db.VehicleBrands.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Name), "PK_VehicleBrand", "Name");
            ViewBag.VehicleModels = new SelectList(bll.db.VehicleBrands.Where(vbm => vbm.PK_VehicleBrand == null).OrderBy(m => m.Name), "PK_VehicleBrand", "Name");
            ViewBag.VehicleTypesDict = new SelectList(VehicleTypesDict.OrderBy(m => m.Value), "Key", "Value");
            ViewBag.FuelTypesDict = new SelectList(FuelTypesDict, "Key", "Value");
            ViewBag.CapacityTonDict = new SelectList(CapacityTonDict, "Key", "Value");
            ViewBag.NumberPlate_StatusesDict = new SelectList(NumberPlate_StatusesDict, "Key", "Value");
            ViewBag.GPS_DeviceExisting = new SelectList(bll.db.GPS_DeviceExisting.Where(m => m.GpsIMEINumber == null).Select(m => new { Key = m.GpsIMEINumber, value = m.GpsIMEINumber + " : " + m.GpsDeviceModel }), "Key", "Value");
            ViewBag.ShowTemperatureDict = new SelectList(ShowTemperatureDict.OrderBy(m => m.Value), "Key", "Value");

            ViewBag.PurchaseCompanies = new SelectList(bll.db.Companies.Where(m => m.IsDeleted == false && m.IsPruchaseCompany == true).OrderBy(m => m.Name), "PK_Company", "Name");
            ViewBag.FinancingCompanies = new SelectList(bll.db.FinancingCompanies.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_FinancingCompany", "Name");
            return View();
        }
        /// <summary>
        /// For adding without any validation
        /// </summary>
        /// <returns></returns>
        public ActionResult Create_OWN_2()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }


            ViewBag.UserCompanies = new SelectList(bll.db.Companies.Where(m => m.IsDeleted == false && m.GroupOfCompany.IsPranRFLGroup == true && m.IsUserCompany == true).OrderBy(m => m.Name), "PK_Company", "Name");
            var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
            //var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false  /*&& accessibleDepoes.Contains(m.PK_Depo)*/ && m.PK_Depo != _invalidDepoPK).OrderBy(m => m.Name), "PK_Depo", "Name");
            ViewBag.DepoGroups = new SelectList(bll.db.DepoGroups.Where(m => m.IsDeleted == false && m.PK_DepoGroup == null).OrderBy(m => m.Name), "PK_DepoGroup", "Name");
            ViewBag.VehicleBrands = new SelectList(bll.db.VehicleBrands.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Name), "PK_VehicleBrand", "Name");
            ViewBag.VehicleModels = new SelectList(bll.db.VehicleBrands.Where(vbm => vbm.PK_VehicleBrand == null).OrderBy(m => m.Name), "PK_VehicleBrand", "Name");
            ViewBag.VehicleTypesDict = new SelectList(VehicleTypesDict.OrderBy(m => m.Value), "Key", "Value");
            ViewBag.FuelTypesDict = new SelectList(FuelTypesDict, "Key", "Value");
            ViewBag.CapacityTonDict = new SelectList(CapacityTonDict, "Key", "Value");
            ViewBag.NumberPlate_StatusesDict = new SelectList(NumberPlate_StatusesDict, "Key", "Value");
            ViewBag.GPS_DeviceExisting = new SelectList(bll.db.GPS_DeviceExisting.Where(m => m.GpsIMEINumber == null).Select(m => new { Key = m.GpsIMEINumber, value = m.GpsIMEINumber + " : " + m.GpsDeviceModel }), "Key", "Value");
            ViewBag.ShowTemperatureDict = new SelectList(ShowTemperatureDict.OrderBy(m => m.Value), "Key", "Value");

            ViewBag.PurchaseCompanies = new SelectList(bll.db.Companies.Where(m => m.IsDeleted == false && m.IsPruchaseCompany == true).OrderBy(m => m.Name), "PK_Company", "Name");
            ViewBag.FinancingCompanies = new SelectList(bll.db.FinancingCompanies.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_FinancingCompany", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult Create_OWN(Vehicle model, HttpPostedFileBase VehicleImage)
        {

            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }

            var temporaryVehile = bll.db.TemporaryVehicles.Where(m => m.RegistrationNumber == model.RegistrationNumber).FirstOrDefault();
            if (temporaryVehile != null)
            {
                CreateAlertMessage(AlertMessageType.Information, "Information", "This vehicle is added into temporary vehicle list. Please, search " + temporaryVehile.RegistrationNumber + " here and add from here.");
                return Redirect("/VehicleGateNew/TemporaryVehicleIndex");
            }

            string modelValidator = bll.IsValidModel_ToCreate_OWN(model);


            if (modelValidator == ValidationStatus.OK)
            {

                try
                {

                    var db_model = bll.FilterToDBModel_OWN(model);
                    if (VehicleImage != null)
                    {
                        string virtualFolderPath = CommonClass.ImageDirectory + "Vehicles/" + db_model.PK_Vehicle + "/";

                        //# create folder
                        string physicalFolderPath = Path.Combine(Server.MapPath(virtualFolderPath));
                        if (!Directory.Exists(physicalFolderPath))
                        {
                            Directory.CreateDirectory(physicalFolderPath);
                        }
                        string virtualFilePath = virtualFolderPath + "this" + "." + VehicleImage.FileName.Split('.').Last();
                        VehicleImage.SaveAs(Path.Combine(Server.MapPath(virtualFilePath)));

                        db_model.ImageLocation = virtualFilePath;
                    }
                    bll.db.Vehicles.Add(db_model);
                    bll.db.SaveChanges();
                    //#vehicle tracking information
                    if (!string.IsNullOrEmpty(model.GpsIMEINumber))
                    {
                        //update vehicle
                        var deviceExisting = bll.db.GPS_DeviceExisting.Where(m => m.GpsIMEINumber == model.GpsIMEINumber).FirstOrDefault();
                        db_model.GpsIMEINumber = deviceExisting.GpsIMEINumber;
                        db_model.GpsDeviceModel = deviceExisting.GpsDeviceModel;

                        //update vehicleTrackingInformation
                        var vehicleTrackingInformation = bll.db.VehicleTrackingInformations.Where(m => m.PK_Vehicle == db_model.PK_Vehicle).FirstOrDefault();
                        if (vehicleTrackingInformation == null)
                        {
                            vehicleTrackingInformation = new VehicleTrackingInformation()
                            {
                                PK_Vehicle = db_model.PK_Vehicle,
                                FK_AppUser_UpdatedBy = CurrentUser.PK_User,
                                UpdatedAt = DateTime.Now,
                                GpsDeviceModel = deviceExisting.GpsDeviceModel,
                                GpsIMEINumber = deviceExisting.GpsIMEINumber,
                                GpsMobileNumber = db_model.GpsMobileNumber,
                                Internal_ShowTemperature = db_model.Internal_ShowTemperature
                            };
                            bll.db.VehicleTrackingInformations.Add(vehicleTrackingInformation);
                        }
                        else
                        {
                            vehicleTrackingInformation.FK_AppUser_UpdatedBy = CurrentUser.PK_User;
                            vehicleTrackingInformation.UpdatedAt = DateTime.Now;
                            vehicleTrackingInformation.GpsDeviceModel = deviceExisting.GpsDeviceModel;
                            vehicleTrackingInformation.GpsIMEINumber = deviceExisting.GpsIMEINumber;
                            vehicleTrackingInformation.GpsMobileNumber = db_model.GpsMobileNumber;
                            vehicleTrackingInformation.Internal_ShowTemperature = db_model.Internal_ShowTemperature;
                        }
                        if (vehicleTrackingInformation.GpsDeviceModel == "Meitrack T1" || vehicleTrackingInformation.GpsDeviceModel == "Meitrack T366")
                        {
                            var vehicleTrackingVT1 = bll.db.VehicleTrackingVT1.Where(m => m.PK_Vehicle == vehicleTrackingInformation.PK_Vehicle).FirstOrDefault();
                            if (vehicleTrackingVT1 != null)
                            {
                                bll.db.VehicleTrackingVT1.Remove(vehicleTrackingVT1);
                            }
                        }
                        else if (vehicleTrackingInformation.GpsDeviceModel == "Zenda VT1")
                        {
                            var vehicleTracking = bll.db.VehicleTrackings.Where(m => m.PK_Vehicle == vehicleTrackingInformation.PK_Vehicle).FirstOrDefault();
                            if (vehicleTracking != null)
                            {
                                bll.db.VehicleTrackings.Remove(vehicleTracking);
                            }
                        }

                        //create log
                        var deviceChangeLog = new GPS_DeviceChangeLog();
                        deviceChangeLog.FK_AppUser_CreatedBy = CurrentUser.PK_User;
                        deviceChangeLog.CreatedAt = DateTime.Now;
                        deviceChangeLog.FK_Vehicle = db_model.PK_Vehicle;
                        deviceChangeLog.GpsDeviceModel = deviceExisting.GpsDeviceModel;
                        deviceChangeLog.GpsIMEINumber = deviceExisting.GpsIMEINumber;
                        deviceChangeLog.GpsMobileNumber = model.GpsMobileNumber;
                        bll.db.GPS_DeviceChangeLog.Add(deviceChangeLog);

                        bll.db.SaveChanges();
                    }
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Vehicle is successfully added.");
                    return RedirectToAction("Index");
                }
                catch (Exception exception)
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                }
            }
            else
            {
                string validators = "";
                if (modelValidator != ValidationStatus.OK)
                {
                    validators = validators + modelValidator;
                }
                CreateAlertMessage(AlertMessageType.Danger, "Validation Failure", validators);
            }
            ViewBag.UserCompanies = new SelectList(bll.db.Companies.Where(m => m.IsDeleted == false && m.GroupOfCompany.IsPranRFLGroup == true && m.IsUserCompany == true).OrderBy(m => m.Name), "PK_Company", "Name", model.FK_Company);
            var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && accessibleDepoes.Contains(m.PK_Depo) && m.PK_Depo != _invalidDepoPK).OrderBy(m => m.Name), "PK_Depo", "Name", model.FK_Depo);
            ViewBag.DepoGroups = new SelectList(bll.db.DepoGroups.Where(m => m.IsDeleted == false && m.FK_Depo == model.FK_Depo).OrderBy(m => m.Name), "PK_DepoGroup", "Name", model.FK_DepoGroup);
            if (model.FK_VehicleModel != null)
            {
                var vehicleModel = bll.db.VehicleModels.Find(model.FK_VehicleModel);
                ViewBag.VehicleBrands = new SelectList(bll.db.VehicleBrands.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Name), "PK_VehicleBrand", "Name", vehicleModel.FK_VehicleBrand);
                ViewBag.VehicleModels = new SelectList(bll.db.VehicleModels.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Title), "PK_VehicleModel", "Title", model.FK_VehicleModel);
            }
            else
            {
                ViewBag.VehicleBrands = new SelectList(bll.db.VehicleBrands.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Name), "PK_VehicleBrand", "Name");
                ViewBag.VehicleModels = new SelectList(bll.db.VehicleModels.Where(vbm => vbm.PK_VehicleModel == null).OrderBy(m => m.Title), "PK_VehicleModel", "Title");
            }
            ViewBag.VehicleTypesDict = new SelectList(VehicleTypesDict.OrderBy(m => m.Value), "Key", "Value", model.VehicleType);
            ViewBag.FuelTypesDict = new SelectList(FuelTypesDict, "Key", "Value", model.FuelType);
            ViewBag.CapacityTonDict = new SelectList(CapacityTonDict, "Key", "Value", model.CapacityTon);
            ViewBag.NumberPlate_StatusesDict = new SelectList(NumberPlate_StatusesDict, "Key", "Value", model.NumberPlate_IsDigital);
            ViewBag.GPS_DeviceExisting = new SelectList(bll.db.GPS_DeviceExisting.Where(m => m.GpsIMEINumber == model.GpsIMEINumber).Select(m => new { Key = m.GpsIMEINumber, value = m.GpsIMEINumber + " : " + m.GpsDeviceModel }), "Key", "Value", model.GpsIMEINumber);
            ViewBag.ShowTemperatureDict = new SelectList(ShowTemperatureDict.OrderBy(m => m.Value), "Key", "Value", model.Internal_ShowTemperature);
            ViewBag.PurchaseCompanies = new SelectList(bll.db.Companies.Where(m => m.IsDeleted == false && m.IsPruchaseCompany == true).OrderBy(m => m.Name), "PK_Company", "Name", model.Internal_FK_PurchasingCompany);
            ViewBag.FinancingCompanies = new SelectList(bll.db.FinancingCompanies.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_FinancingCompany", "Name", model.Internal_FK_FinancingCompany);
            return View(model);
        }



        public ActionResult Edit_OWN(Guid id)
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
                var model = bll.db.Vehicles.Find(id);
                if (model != null)
                {
                    ViewBag.model = model;
                    ViewBag.UserCompanies = new SelectList(bll.db.Companies.Where(m => m.IsDeleted == false && m.GroupOfCompany.IsPranRFLGroup == true && m.IsUserCompany == true).OrderBy(m => m.Name), "PK_Company", "Name", model.FK_Company);
                    var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
                    var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
                    ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && accessibleDepoes.Contains(m.PK_Depo) && m.PK_Depo != _invalidDepoPK).OrderBy(m => m.Name), "PK_Depo", "Name", model.FK_Depo);
                    ViewBag.DepoGroups = new SelectList(bll.db.DepoGroups.Where(m => m.IsDeleted == false && m.FK_Depo == model.FK_Depo).OrderBy(m => m.Name), "PK_DepoGroup", "Name", model.FK_DepoGroup);
                    if (model.FK_VehicleModel != null)
                    {
                        var vehicleModel = bll.db.VehicleModels.Find(model.FK_VehicleModel);
                        ViewBag.VehicleBrands = new SelectList(bll.db.VehicleBrands.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Name), "PK_VehicleBrand", "Name", vehicleModel.FK_VehicleBrand);
                        ViewBag.VehicleModels = new SelectList(bll.db.VehicleModels.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Title), "PK_VehicleModel", "Title", model.FK_VehicleModel);
                    }
                    else
                    {
                        ViewBag.VehicleBrands = new SelectList(bll.db.VehicleBrands.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Name), "PK_VehicleBrand", "Name");
                        ViewBag.VehicleModels = new SelectList(bll.db.VehicleBrands.Where(vbm => vbm.PK_VehicleBrand == null).OrderBy(m => m.Name), "PK_VehicleBrand", "Name");
                    }
                    ViewBag.VehicleTypesDict = new SelectList(VehicleTypesDict.OrderBy(m => m.Value), "Key", "Value", model.VehicleType);
                    ViewBag.FuelTypesDict = new SelectList(FuelTypesDict, "Key", "Value", model.FuelType);
                    ViewBag.CapacityTonDict = new SelectList(CapacityTonDict, "Key", "Value", model.CapacityTon);
                    ViewBag.NumberPlate_StatusesDict = new SelectList(NumberPlate_StatusesDict, "Key", "Value", model.NumberPlate_IsDigital);
                    ViewBag.GPS_DeviceExisting = new SelectList(bll.db.GPS_DeviceExisting.Where(m => m.GpsIMEINumber == model.GpsIMEINumber).Select(m => new { Key = m.GpsIMEINumber, value = m.GpsIMEINumber + " : " + m.GpsDeviceModel }), "Key", "Value", model.GpsIMEINumber);
                    ViewBag.ShowTemperatureDict = new SelectList(ShowTemperatureDict.OrderBy(m => m.Value), "Key", "Value", model.Internal_ShowTemperature);
                    ViewBag.PurchaseCompanies = new SelectList(bll.db.Companies.Where(m => m.IsDeleted == false && m.IsPruchaseCompany == true).OrderBy(m => m.Name), "PK_Company", "Name", model.Internal_FK_PurchasingCompany);
                    ViewBag.FinancingCompanies = new SelectList(bll.db.FinancingCompanies.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_FinancingCompany", "Name", model.Internal_FK_FinancingCompany);
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
        public ActionResult Edit_OWN_2(Guid id)
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
                var model = bll.db.Vehicles.Find(id);
                if (model != null)
                {
                    ViewBag.model = model;
                    ViewBag.UserCompanies = new SelectList(bll.db.Companies.Where(m => m.IsDeleted == false && m.GroupOfCompany.IsPranRFLGroup == true && m.IsUserCompany == true).OrderBy(m => m.Name), "PK_Company", "Name", model.FK_Company);
                    var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
                    //var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
                    ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false  /*&& accessibleDepoes.Contains(m.PK_Depo) */&& m.PK_Depo != _invalidDepoPK).OrderBy(m => m.Name), "PK_Depo", "Name", model.FK_Depo);
                    ViewBag.DepoGroups = new SelectList(bll.db.DepoGroups.Where(m => m.IsDeleted == false && m.FK_Depo == model.FK_Depo).OrderBy(m => m.Name), "PK_DepoGroup", "Name", model.FK_DepoGroup);
                    if (model.FK_VehicleModel != null)
                    {
                        var vehicleModel = bll.db.VehicleModels.Find(model.FK_VehicleModel);
                        ViewBag.VehicleBrands = new SelectList(bll.db.VehicleBrands.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Name), "PK_VehicleBrand", "Name", vehicleModel.FK_VehicleBrand);
                        ViewBag.VehicleModels = new SelectList(bll.db.VehicleModels.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Title), "PK_VehicleModel", "Title", model.FK_VehicleModel);
                    }
                    else
                    {
                        ViewBag.VehicleBrands = new SelectList(bll.db.VehicleBrands.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Name), "PK_VehicleBrand", "Name");
                        ViewBag.VehicleModels = new SelectList(bll.db.VehicleBrands.Where(vbm => vbm.PK_VehicleBrand == null).OrderBy(m => m.Name), "PK_VehicleBrand", "Name");
                    }
                    ViewBag.VehicleTypesDict = new SelectList(VehicleTypesDict.OrderBy(m => m.Value), "Key", "Value", model.VehicleType);
                    ViewBag.FuelTypesDict = new SelectList(FuelTypesDict, "Key", "Value", model.FuelType);
                    ViewBag.CapacityTonDict = new SelectList(CapacityTonDict, "Key", "Value", model.CapacityTon);
                    ViewBag.NumberPlate_StatusesDict = new SelectList(NumberPlate_StatusesDict, "Key", "Value", model.NumberPlate_IsDigital);
                    ViewBag.GPS_DeviceExisting = new SelectList(bll.db.GPS_DeviceExisting.Where(m => m.GpsIMEINumber == model.GpsIMEINumber).Select(m => new { Key = m.GpsIMEINumber, value = m.GpsIMEINumber + " : " + m.GpsDeviceModel }), "Key", "Value", model.GpsIMEINumber);
                    ViewBag.ShowTemperatureDict = new SelectList(ShowTemperatureDict.OrderBy(m => m.Value), "Key", "Value", model.Internal_ShowTemperature);
                    ViewBag.PurchaseCompanies = new SelectList(bll.db.Companies.Where(m => m.IsDeleted == false && m.IsPruchaseCompany == true).OrderBy(m => m.Name), "PK_Company", "Name", model.Internal_FK_PurchasingCompany);
                    ViewBag.FinancingCompanies = new SelectList(bll.db.FinancingCompanies.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_FinancingCompany", "Name", model.Internal_FK_FinancingCompany);
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
        [HttpPost]
        public ActionResult Edit_OWN(Vehicle model, HttpPostedFileBase VehicleImage)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }

            string modelValidator = bll.IsValidModel_ToEdit_OWN(model);

            if (modelValidator == ValidationStatus.OK)
            {

                try
                {
                    var db_model = bll.FilterToDBModel_OWN(model);

                    if (VehicleImage != null)
                    {

                        //# delete old file, model.ImageLocation is hidden in view page
                        if (System.IO.File.Exists(model.ImageLocation))
                        {
                            System.IO.File.Delete(model.ImageLocation);
                        }
                        string virtualFolderPath = CommonClass.ImageDirectory + "Vehicles/" + model.PK_Vehicle + "/";

                        //# create folder
                        string physicalFolderPath = Path.Combine(Server.MapPath(virtualFolderPath));
                        if (!Directory.Exists(physicalFolderPath))
                        {
                            Directory.CreateDirectory(physicalFolderPath);
                        }

                        string virtualFilePath = virtualFolderPath + "this" + "." + VehicleImage.FileName.Split('.').Last();
                        VehicleImage.SaveAs(Path.Combine(Server.MapPath(virtualFilePath)));

                        db_model.ImageLocation = virtualFilePath;
                    }
                    bll.db.SaveChanges();

                    //#vehicle tracking information
                    if (model.GpsIMEINumber != db_model.GpsIMEINumber)
                    {
                        if (!string.IsNullOrEmpty(model.GpsIMEINumber))
                        {
                            //update vehicle
                            var deviceExisting = bll.db.GPS_DeviceExisting.Where(m => m.GpsIMEINumber == model.GpsIMEINumber).FirstOrDefault();
                            db_model.GpsIMEINumber = deviceExisting.GpsIMEINumber;
                            db_model.GpsDeviceModel = deviceExisting.GpsDeviceModel;

                            //update vehicleTrackingInformation
                            var vehicleTrackingInformation = bll.db.VehicleTrackingInformations.Where(m => m.PK_Vehicle == db_model.PK_Vehicle).FirstOrDefault();
                            if (vehicleTrackingInformation == null)
                            {
                                vehicleTrackingInformation = new VehicleTrackingInformation()
                                {
                                    PK_Vehicle = db_model.PK_Vehicle,
                                    FK_AppUser_UpdatedBy = CurrentUser.PK_User,
                                    UpdatedAt = DateTime.Now,
                                    GpsDeviceModel = deviceExisting.GpsDeviceModel,
                                    GpsIMEINumber = deviceExisting.GpsIMEINumber,
                                    GpsMobileNumber = db_model.GpsMobileNumber,
                                    Internal_ShowTemperature = db_model.Internal_ShowTemperature
                                };
                                bll.db.VehicleTrackingInformations.Add(vehicleTrackingInformation);
                            }
                            else
                            {
                                vehicleTrackingInformation.FK_AppUser_UpdatedBy = CurrentUser.PK_User;
                                vehicleTrackingInformation.UpdatedAt = DateTime.Now;
                                vehicleTrackingInformation.GpsDeviceModel = deviceExisting.GpsDeviceModel;
                                vehicleTrackingInformation.GpsIMEINumber = deviceExisting.GpsIMEINumber;
                                vehicleTrackingInformation.GpsMobileNumber = db_model.GpsMobileNumber;
                                vehicleTrackingInformation.Internal_ShowTemperature = db_model.Internal_ShowTemperature;
                            }
                            if (vehicleTrackingInformation.GpsDeviceModel == "Meitrack T1" || vehicleTrackingInformation.GpsDeviceModel == "Meitrack T366")
                            {
                                var vehicleTrackingVT1 = bll.db.VehicleTrackingVT1.Where(m => m.PK_Vehicle == vehicleTrackingInformation.PK_Vehicle).FirstOrDefault();
                                if (vehicleTrackingVT1 != null)
                                {
                                    bll.db.VehicleTrackingVT1.Remove(vehicleTrackingVT1);
                                }
                            }
                            else if (vehicleTrackingInformation.GpsDeviceModel == "Zenda VT1")
                            {
                                var vehicleTracking = bll.db.VehicleTrackings.Where(m => m.PK_Vehicle == vehicleTrackingInformation.PK_Vehicle).FirstOrDefault();
                                if (vehicleTracking != null)
                                {
                                    bll.db.VehicleTrackings.Remove(vehicleTracking);
                                }
                            }

                            //create log
                            var deviceChangeLog = new GPS_DeviceChangeLog();
                            deviceChangeLog.FK_AppUser_CreatedBy = CurrentUser.PK_User;
                            deviceChangeLog.CreatedAt = DateTime.Now;
                            deviceChangeLog.FK_Vehicle = db_model.PK_Vehicle;
                            deviceChangeLog.GpsDeviceModel = deviceExisting.GpsDeviceModel;
                            deviceChangeLog.GpsIMEINumber = deviceExisting.GpsIMEINumber;
                            deviceChangeLog.GpsMobileNumber = model.GpsMobileNumber;
                            bll.db.GPS_DeviceChangeLog.Add(deviceChangeLog);

                            bll.db.SaveChanges();
                        }
                        else
                        {
                            //update vehicle
                            db_model.GpsIMEINumber = null;
                            db_model.GpsDeviceModel = null;

                            //update tracking Information
                            var vehicleTrackingInformation = bll.db.VehicleTrackingInformations.Where(m => m.PK_Vehicle == db_model.PK_Vehicle).FirstOrDefault();
                            if (vehicleTrackingInformation != null)
                            {
                                vehicleTrackingInformation.FK_AppUser_UpdatedBy = CurrentUser.PK_User;
                                vehicleTrackingInformation.UpdatedAt = DateTime.Now;
                                vehicleTrackingInformation.GpsDeviceModel = null;
                                vehicleTrackingInformation.GpsIMEINumber = null;
                                vehicleTrackingInformation.GpsMobileNumber = null;
                                vehicleTrackingInformation.Internal_ShowTemperature = null;
                                bll.db.SaveChanges();
                            }

                            //create log
                            var deviceChangeLog = new GPS_DeviceChangeLog();
                            deviceChangeLog.FK_AppUser_CreatedBy = CurrentUser.PK_User;
                            deviceChangeLog.CreatedAt = DateTime.Now;
                            deviceChangeLog.FK_Vehicle = db_model.PK_Vehicle;
                            deviceChangeLog.GpsDeviceModel = null;
                            deviceChangeLog.GpsIMEINumber = null;
                            deviceChangeLog.GpsMobileNumber = model.GpsMobileNumber;
                            bll.db.GPS_DeviceChangeLog.Add(deviceChangeLog);

                            bll.db.SaveChanges();
                        }
                    }

                    CreateAlertMessage(AlertMessageType.Success, "Success", "Vehicle is successfully edited.");
                    return RedirectToAction("Index");
                }
                catch (Exception exception)
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                }
            }
            else
            {
                string validators = "";
                if (modelValidator != ValidationStatus.OK)
                {
                    validators = validators + modelValidator;
                }
                CreateAlertMessage(AlertMessageType.Danger, "Validation Failure", validators);
            }
            ViewBag.model = model;
            ViewBag.UserCompanies = new SelectList(bll.db.Companies.Where(m => m.IsDeleted == false && m.GroupOfCompany.IsPranRFLGroup == true && m.IsUserCompany == true).OrderBy(m => m.Name), "PK_Company", "Name", model.FK_Company);
            var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && accessibleDepoes.Contains(m.PK_Depo) && m.PK_Depo != _invalidDepoPK).OrderBy(m => m.Name), "PK_Depo", "Name", model.FK_Depo);
            ViewBag.DepoGroups = new SelectList(bll.db.DepoGroups.Where(m => m.IsDeleted == false && m.FK_Depo == model.FK_Depo).OrderBy(m => m.Name), "PK_DepoGroup", "Name", model.FK_DepoGroup);
            if (model.FK_VehicleModel != null)
            {
                var vehicleModel = bll.db.VehicleModels.Find(model.FK_VehicleModel);
                ViewBag.VehicleBrands = new SelectList(bll.db.VehicleBrands.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Name), "PK_VehicleBrand", "Name", vehicleModel.FK_VehicleBrand);
                ViewBag.VehicleModels = new SelectList(bll.db.VehicleModels.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Title), "PK_VehicleModel", "Title", model.FK_VehicleModel);
            }
            else
            {
                ViewBag.VehicleBrands = new SelectList(bll.db.VehicleBrands.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Name), "PK_VehicleBrand", "Name");
                ViewBag.VehicleModels = new SelectList(bll.db.VehicleModels.Where(vbm => vbm.PK_VehicleModel == null).OrderBy(m => m.Title), "PK_VehicleModel", "Title");
            }
            ViewBag.VehicleTypesDict = new SelectList(VehicleTypesDict.OrderBy(m => m.Value), "Key", "Value", model.VehicleType);
            ViewBag.FuelTypesDict = new SelectList(FuelTypesDict, "Key", "Value", model.FuelType);
            ViewBag.CapacityTonDict = new SelectList(CapacityTonDict, "Key", "Value", model.CapacityTon);
            ViewBag.NumberPlate_StatusesDict = new SelectList(NumberPlate_StatusesDict, "Key", "Value", model.NumberPlate_IsDigital);
            ViewBag.GPS_DeviceExisting = new SelectList(bll.db.GPS_DeviceExisting.Where(m => m.GpsIMEINumber == model.GpsIMEINumber).Select(m => new { Key = m.GpsIMEINumber, value = m.GpsIMEINumber + " : " + m.GpsDeviceModel }), "Key", "Value", model.GpsIMEINumber);
            ViewBag.ShowTemperatureDict = new SelectList(ShowTemperatureDict.OrderBy(m => m.Value), "Key", "Value", model.Internal_ShowTemperature);
            ViewBag.PurchaseCompanies = new SelectList(bll.db.Companies.Where(m => m.IsDeleted == false && m.IsPruchaseCompany == true).OrderBy(m => m.Name), "PK_Company", "Name", model.Internal_FK_PurchasingCompany);
            ViewBag.FinancingCompanies = new SelectList(bll.db.FinancingCompanies.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_FinancingCompany", "Name", model.Internal_FK_FinancingCompany);
            return View(model);
        }

        public ActionResult View_OWN(Guid id)
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
                var model = bll.db.Vehicles.Find(id);
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

        public ActionResult IndexByAgreementManager()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var _list = bll.db.Vehicles.Where(m => m.OWN_MHT_DHT == "OWN").ToList();
            var list = _list.Select(m => bll.ConvertToViewModel(m)).ToList();
            return View(list);
        }
        public ActionResult SetAgreementMaturity_OWN(Guid id)
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
                var model = bll.db.Vehicles.Find(id);
                if (model != null)
                {
                    ViewBag.model = model;
                    ViewBag.UserCompanies = new SelectList(bll.db.Companies.Where(m => m.IsDeleted == false && m.GroupOfCompany.IsPranRFLGroup == true && m.IsUserCompany == true).OrderBy(m => m.Name), "PK_Company", "Name", model.FK_Company);
                    var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
                    var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
                    ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && accessibleDepoes.Contains(m.PK_Depo) && m.PK_Depo != _invalidDepoPK).OrderBy(m => m.Name), "PK_Depo", "Name", model.FK_Depo);
                    ViewBag.DepoGroups = new SelectList(bll.db.DepoGroups.Where(m => m.IsDeleted == false && m.FK_Depo == model.FK_Depo).OrderBy(m => m.Name), "PK_DepoGroup", "Name", model.FK_DepoGroup);
                    if (model.FK_VehicleModel != null)
                    {
                        var vehicleModel = bll.db.VehicleModels.Find(model.FK_VehicleModel);
                        ViewBag.VehicleBrands = new SelectList(bll.db.VehicleBrands.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Name), "PK_VehicleBrand", "Name", vehicleModel.FK_VehicleBrand);
                        ViewBag.VehicleModels = new SelectList(bll.db.VehicleModels.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Title), "PK_VehicleModel", "Title", model.FK_VehicleModel);
                    }
                    else
                    {
                        ViewBag.VehicleBrands = new SelectList(bll.db.VehicleBrands.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Name), "PK_VehicleBrand", "Name");
                        ViewBag.VehicleModels = new SelectList(bll.db.VehicleBrands.Where(vbm => vbm.PK_VehicleBrand == null).OrderBy(m => m.Name), "PK_VehicleBrand", "Name");
                    }
                    ViewBag.VehicleTypesDict = new SelectList(VehicleTypesDict.OrderBy(m => m.Value), "Key", "Value", model.VehicleType);
                    ViewBag.FuelTypesDict = new SelectList(FuelTypesDict, "Key", "Value", model.FuelType);
                    ViewBag.CapacityTonDict = new SelectList(CapacityTonDict, "Key", "Value", model.CapacityTon);
                    ViewBag.NumberPlate_StatusesDict = new SelectList(NumberPlate_StatusesDict, "Key", "Value", model.NumberPlate_IsDigital);
                    ViewBag.GPS_DeviceExisting = new SelectList(bll.db.GPS_DeviceExisting.Where(m => m.GpsIMEINumber == model.GpsIMEINumber).Select(m => new { Key = m.GpsIMEINumber, value = m.GpsIMEINumber + " : " + m.GpsDeviceModel }), "Key", "Value", model.GpsIMEINumber);
                    ViewBag.ShowTemperatureDict = new SelectList(ShowTemperatureDict.OrderBy(m => m.Value), "Key", "Value", model.Internal_ShowTemperature);
                    ViewBag.PurchaseCompanies = new SelectList(bll.db.Companies.Where(m => m.IsDeleted == false && m.IsPruchaseCompany == true).OrderBy(m => m.Name), "PK_Company", "Name", model.Internal_FK_PurchasingCompany);
                    ViewBag.FinancingCompanies = new SelectList(bll.db.FinancingCompanies.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_FinancingCompany", "Name", model.Internal_FK_FinancingCompany);
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
        [HttpPost]
        public ActionResult SetAgreementMaturity_OWN(Vehicle model)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var db_model = bll.db.Vehicles.Where(m => m.PK_Vehicle == model.PK_Vehicle).FirstOrDefault();
            try
            {
                db_model.Internal_FinancingAgrementNumber = model.Internal_FinancingAgrementNumber;
                db_model.Internal_FinancingAgrementNumberOfVehicle = model.Internal_FinancingAgrementNumberOfVehicle;
                db_model.Internal_FinancingAgrementMaturityDate = model.Internal_FinancingAgrementMaturityDate;
                db_model.Internal_FinancingAgrementGRN_MRR = model.Internal_FinancingAgrementGRN_MRR;
                db_model.Internal_FinancingAgrementGRN_MRR_Date = model.Internal_FinancingAgrementGRN_MRR_Date;
                db_model.Internal_FinancingAgrementIsMatured = model.Internal_FinancingAgrementIsMatured;
                if (db_model.Internal_FinancingAgrementUpdatedBy == null)
                {
                    db_model.Internal_FinancingAgrementUpdatedBy = CurrentUser.PK_User;
                }
                if (model.Internal_FinancingAgrementIsMatured == true && db_model.Internal_FinancingAgrementIsMatured == null)
                {
                    db_model.Internal_FinancingAgrementIsMatured = true;
                    db_model.Internal_FinancingAgrementMaturityUpdatedBy = CurrentUser.PK_User;
                }
                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "Vehicle is successfully edited.");
                return RedirectToAction("IndexByAgreementManager");
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                return View(db_model);
            }
        }
        public ActionResult SetKPL(Guid id)
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
                var model = bll.db.Vehicles.Find(id);
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
        [HttpPost]
        public ActionResult SetKPL(Vehicle model)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }


            try
            {
                var db_model = bll.db.Vehicles.Find(model.PK_Vehicle);
                if (model.Internal_KPL_Loaded != db_model.Internal_KPL_Loaded || model.Internal_KPL_Loaded_Plastic != db_model.Internal_KPL_Loaded_Plastic || model.Internal_KPL_Empty != db_model.Internal_KPL_Empty
                    || db_model.Internal_KPL_InterCity != db_model.Internal_KPL_InterCity || model.Internal_KPL_InterCHT != db_model.Internal_KPL_InterCHT || model.Internal_KPL_Hill != db_model.Internal_KPL_Hill ||
                    model.Internal_KPL_OnlyMover != db_model.Internal_KPL_OnlyMover || model.Internal_KPL_Loaded_8_To_12_Tons != db_model.Internal_KPL_Loaded_8_To_12_Tons || model.Internal_KPL_Loaded_12_To_25_Tons != db_model.Internal_KPL_Loaded_12_To_25_Tons
                    || model.Internal_KPL_Loaded_8_To_12_Tons_Out != db_model.Internal_KPL_Loaded_8_To_12_Tons_Out || model.Internal_KPL_Loaded_12_To_25_Tons_Out != db_model.Internal_KPL_Loaded_12_To_25_Tons_Out)
                {
                    db_model.Internal_KPL_Loaded = model.Internal_KPL_Loaded;
                    db_model.Internal_KPL_Loaded_Plastic = model.Internal_KPL_Loaded_Plastic;
                    db_model.Internal_KPL_Empty = model.Internal_KPL_Empty;
                    db_model.Internal_KPL_InterCity = model.Internal_KPL_InterCity;
                    db_model.Internal_KPL_InterCHT = model.Internal_KPL_InterCHT;
                    db_model.Internal_KPL_Hill = model.Internal_KPL_Hill;
                    db_model.Internal_KPL_OnlyMover = model.Internal_KPL_OnlyMover;
                    db_model.Internal_KPL_Loaded_8_To_12_Tons = model.Internal_KPL_Loaded_8_To_12_Tons;
                    db_model.Internal_KPL_Loaded_12_To_25_Tons = model.Internal_KPL_Loaded_12_To_25_Tons;
                    db_model.Internal_KPL_Loaded_8_To_12_Tons_Out = model.Internal_KPL_Loaded_8_To_12_Tons_Out;
                    db_model.Internal_KPL_Loaded_12_To_25_Tons_Out = model.Internal_KPL_Loaded_12_To_25_Tons_Out;

                    var kpl = new KPLChangeLog();
                    kpl.FK_Vehicle = model.PK_Vehicle;
                    kpl.FK_CreatedByUser = CurrentUser.PK_User;
                    kpl.CreatedAt = DateTime.Now;

                    kpl.KPL_Loaded = model.Internal_KPL_Loaded;
                    kpl.KPL_Loaded_Plastic = model.Internal_KPL_Loaded_Plastic;
                    kpl.KPL_Empty = model.Internal_KPL_Empty;
                    kpl.KPL_InterCity = model.Internal_KPL_InterCity;
                    kpl.KPL_InterCHT = model.Internal_KPL_InterCHT;
                    kpl.KPL_Hill = model.Internal_KPL_Hill;
                    kpl.KPL_OnlyMover = model.Internal_KPL_OnlyMover;
                    kpl.KPL_Loaded_8_To_12_Tons = model.Internal_KPL_Loaded_8_To_12_Tons;
                    kpl.KPL_Loaded_12_To_25_Tons = model.Internal_KPL_Loaded_12_To_25_Tons;
                    kpl.KPL_Loaded_8_To_12_Tons_Out = model.Internal_KPL_Loaded_8_To_12_Tons_Out;
                    kpl.KPL_Loaded_12_To_25_Tons_Out = model.Internal_KPL_Loaded_12_To_25_Tons_Out;

                    bll.db.KPLChangeLogs.Add(kpl);
                    bll.db.SaveChanges();
                }

                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "Vehicle is successfully edited.");
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
            }
            ViewBag.model = model;
            ViewBag.UserCompanies = new SelectList(bll.db.Companies.Where(m => m.IsDeleted == false && m.GroupOfCompany.IsPranRFLGroup == true && m.IsUserCompany == true).OrderBy(m => m.Name), "PK_Company", "Name", model.FK_Company);
            var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && accessibleDepoes.Contains(m.PK_Depo) && m.PK_Depo != _invalidDepoPK).OrderBy(m => m.Name), "PK_Depo", "Name", model.FK_Depo);
            ViewBag.DepoGroups = new SelectList(bll.db.DepoGroups.Where(m => m.IsDeleted == false && m.FK_Depo == model.FK_Depo).OrderBy(m => m.Name), "PK_DepoGroup", "Name", model.FK_DepoGroup);
            if (model.FK_VehicleModel != null)
            {
                var vehicleModel = bll.db.VehicleModels.Find(model.FK_VehicleModel);
                ViewBag.VehicleBrands = new SelectList(bll.db.VehicleBrands.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Name), "PK_VehicleBrand", "Name", vehicleModel.FK_VehicleBrand);
                ViewBag.VehicleModels = new SelectList(bll.db.VehicleModels.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Title), "PK_VehicleModel", "Title", model.FK_VehicleModel);
            }
            else
            {
                ViewBag.VehicleBrands = new SelectList(bll.db.VehicleBrands.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Name), "PK_VehicleBrand", "Name");
                ViewBag.VehicleModels = new SelectList(bll.db.VehicleModels.Where(vbm => vbm.PK_VehicleModel == null).OrderBy(m => m.Title), "PK_VehicleModel", "Title");
            }
            ViewBag.VehicleTypesDict = new SelectList(VehicleTypesDict.OrderBy(m => m.Value), "Key", "Value", model.VehicleType);
            ViewBag.FuelTypesDict = new SelectList(FuelTypesDict, "Key", "Value", model.FuelType);
            ViewBag.CapacityTonDict = new SelectList(CapacityTonDict, "Key", "Value", model.CapacityTon);
            ViewBag.NumberPlate_StatusesDict = new SelectList(NumberPlate_StatusesDict, "Key", "Value", model.NumberPlate_IsDigital);
            ViewBag.GPS_DeviceExisting = new SelectList(bll.db.GPS_DeviceExisting.Where(m => m.GpsIMEINumber == model.GpsIMEINumber).Select(m => new { Key = m.GpsIMEINumber, value = m.GpsIMEINumber + " : " + m.GpsDeviceModel }), "Key", "Value", model.GpsIMEINumber);
            ViewBag.ShowTemperatureDict = new SelectList(ShowTemperatureDict.OrderBy(m => m.Value), "Key", "Value", model.Internal_ShowTemperature);
            ViewBag.PurchaseCompanies = new SelectList(bll.db.Companies.Where(m => m.IsDeleted == false && m.IsPruchaseCompany == true).OrderBy(m => m.Name), "PK_Company", "Name", model.Internal_FK_PurchasingCompany);
            ViewBag.FinancingCompanies = new SelectList(bll.db.FinancingCompanies.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_FinancingCompany", "Name", model.Internal_FK_FinancingCompany);
            return View(model);
        }

        public ActionResult SetAdvertisingCompany(Guid id)
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
                var model = bll.db.Vehicles.Find(id);
                if (model != null)
                {
                    ViewBag.AdvertisingCompanies = new SelectList(bll.db.Companies.Where(m => m.IsDeleted == false && m.GroupOfCompany.IsPranRFLGroup == true && m.IsAdevertisingCompany == true).OrderBy(m => m.Name), "PK_Company", "Name", model.Internal_FK_AdvertisingCompany);
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
        [HttpPost]
        public ActionResult SetAdvertisingCompany(Vehicle model)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            try
            {
                var db_model = bll.db.Vehicles.Find(model.PK_Vehicle);
                if (model.Internal_FK_AdvertisingCompany != null)
                {
                    db_model.Internal_FK_AdvertisingCompany = model.Internal_FK_AdvertisingCompany;
                }
                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "Vehicle is successfully edited.");
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
            }
            return RedirectToAction("Index");
        }
        #endregion 

        #region //DHT
        public ActionResult Create_DHT()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }

            ViewBag.UserCompanies = new SelectList(bll.db.Companies.Where(m => m.IsDeleted == false && m.GroupOfCompany.IsPranRFLGroup == true && m.IsUserCompany == true).OrderBy(m => m.Name), "PK_Company", "Name");
            var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && accessibleDepoes.Contains(m.PK_Depo) && m.PK_Depo != _invalidDepoPK).OrderBy(m => m.Name), "PK_Depo", "Name");
            ViewBag.VehicleBrands = new SelectList(bll.db.VehicleBrands.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Name), "PK_VehicleBrand", "Name");
            ViewBag.VehicleModels = new SelectList(bll.db.VehicleBrands.Where(vbm => vbm.PK_VehicleBrand == null).OrderBy(m => m.Name), "PK_VehicleBrand", "Name");
            ViewBag.VehicleTypesDict = new SelectList(VehicleTypesDict, "Key", "Value");
            ViewBag.FuelTypesDict = new SelectList(FuelTypesDict, "Key", "Value");
            ViewBag.CapacityTonDict = new SelectList(CapacityTonDict, "Key", "Value");
            ViewBag.NumberPlate_StatusesDict = new SelectList(NumberPlate_StatusesDict, "Key", "Value");

            ViewBag.PurchaseCompanies = new SelectList(bll.db.Companies.Where(m => m.IsDeleted == false && m.IsPruchaseCompany == true).OrderBy(m => m.Name), "PK_Company", "Name");
            ViewBag.FinancingCompanies = new SelectList(bll.db.FinancingCompanies.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_FinancingCompany", "Name");
            ViewBag.MHT_DHT_DriverLicenseTypeDict = new SelectList(MHT_DHT_DriverLicenseTypeDict, "Key", "Value");
            ViewBag.MHT_DHT_VehicleSizeDict = new SelectList(MHT_DHT_VehicleSizeDict, "Key", "Value");

            return View();
        }
        [HttpPost]
        public ActionResult Create_DHT(Vehicle model, HttpPostedFileBase VehicleImage, HttpPostedFileBase MHT_DHT_DriverImage, HttpPostedFileBase MHT_DHT_DriverLicenseImage)
        {

            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }

            var temporaryVehile = bll.db.TemporaryVehicles.Where(m => m.RegistrationNumber == model.RegistrationNumber).FirstOrDefault();
            if (temporaryVehile != null)
            {
                CreateAlertMessage(AlertMessageType.Information, "Information", "This vehicle is added into temporary vehicle list. Please, search " + temporaryVehile.RegistrationNumber + " here and add from here.");
                return Redirect("/VehicleGateNew/TemporaryVehicleIndex");
            }
            string modelValidator = bll.IsValidModel_ToCreate_DHT(model);


            if (modelValidator == ValidationStatus.OK)
            {

                try
                {

                    var db_model = bll.FilterToDBModel_DHT(model);
                    if (VehicleImage != null)
                    {
                        string virtualFolderPath = CommonClass.ImageDirectory + "Vehicles/" + db_model.PK_Vehicle + "/";

                        //# create folder
                        string physicalFolderPath = Path.Combine(Server.MapPath(virtualFolderPath));
                        if (!Directory.Exists(physicalFolderPath))
                        {
                            Directory.CreateDirectory(physicalFolderPath);
                        }
                        string virtualFilePath = virtualFolderPath + "This_Vehicle" + "." + VehicleImage.FileName.Split('.').Last();
                        VehicleImage.SaveAs(Path.Combine(Server.MapPath(virtualFilePath)));

                        db_model.ImageLocation = virtualFilePath;
                    }

                    bll.db.Vehicles.Add(db_model);
                    if (!string.IsNullOrEmpty(model.MHT_DHT_DriverName))
                    {
                        var hiredVehicleDriver = new HiredVehicleDriver();
                        hiredVehicleDriver.PK_HiredVehicleDriver = Guid.NewGuid();
                        hiredVehicleDriver.CreatedAt = DateTime.Now;
                        hiredVehicleDriver.FK_CreatedByUser = CurrentUser.PK_User;
                        hiredVehicleDriver.IsDeleted = false;
                        hiredVehicleDriver.FK_Vehicle = db_model.PK_Vehicle;

                        hiredVehicleDriver.DriverName = string.IsNullOrEmpty(model.MHT_DHT_DriverName) ? null : model.MHT_DHT_DriverName.Trim().ToUpper();
                        hiredVehicleDriver.DriverLiceneseNumber = string.IsNullOrEmpty(model.MHT_DHT_DriverLiceneseNumber) ? null : model.MHT_DHT_DriverLiceneseNumber.Trim().ToUpper();
                        hiredVehicleDriver.DriverLicenseType = model.MHT_DHT_DriverLicenseType;
                        hiredVehicleDriver.DriverContactNumber = model.MHT_DHT_DriverContactNumber;
                        hiredVehicleDriver.DriverFatherName = string.IsNullOrEmpty(model.MHT_DHT_DriverFatherName) ? null : model.MHT_DHT_DriverFatherName.Trim().ToUpper();
                        hiredVehicleDriver.DriverAddressVillage = string.IsNullOrEmpty(model.MHT_DHT_DriverAddressVillage) ? null : model.MHT_DHT_DriverAddressVillage.Trim().ToUpper();
                        hiredVehicleDriver.DriverAddressPostOfiice = string.IsNullOrEmpty(model.MHT_DHT_DriverAddressPostOfiice) ? null : model.MHT_DHT_DriverAddressPostOfiice.Trim().ToUpper();
                        hiredVehicleDriver.DriverAddressThana = string.IsNullOrEmpty(model.MHT_DHT_DriverAddressThana) ? null : model.MHT_DHT_DriverAddressThana.Trim().ToUpper();
                        hiredVehicleDriver.DriverAddressDistrict = string.IsNullOrEmpty(model.MHT_DHT_DriverAddressDistrict) ? null : model.MHT_DHT_DriverAddressDistrict.Trim().ToUpper();
                        hiredVehicleDriver.DriverNID = string.IsNullOrEmpty(model.MHT_DHT_DriverNID) ? null : model.MHT_DHT_DriverNID.Trim().ToUpper();
                        hiredVehicleDriver.DriverSalary = model.MHT_DHT_DriverSalary;

                        string virtualFolderPath = CommonClass.ImageDirectory + "Vehicles/" + db_model.PK_Vehicle + "/HiredVehicleDriver/";
                        //# create folder
                        string physicalFolderPath = Path.Combine(Server.MapPath(virtualFolderPath));
                        if (!Directory.Exists(physicalFolderPath))
                        {
                            Directory.CreateDirectory(physicalFolderPath);
                        }

                        if (MHT_DHT_DriverImage != null)
                        {
                            string virtualFilePath = virtualFolderPath + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace(":", "-") + " DriverImage " + hiredVehicleDriver.PK_HiredVehicleDriver + "." + MHT_DHT_DriverImage.FileName.Split('.').Last();
                            MHT_DHT_DriverImage.SaveAs(Path.Combine(Server.MapPath(virtualFilePath)));
                            hiredVehicleDriver.DriverImageLocation = virtualFilePath;
                            model.MHT_DHT_DriverImageLocation = virtualFilePath;
                        }

                        if (MHT_DHT_DriverLicenseImage != null)
                        {
                            hiredVehicleDriver.DriverLiceneseNumber = db_model.MHT_DHT_DriverLiceneseNumber;
                            string virtualFilePath = virtualFolderPath + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace(":", "-") + " LicenseNumber " + hiredVehicleDriver.PK_HiredVehicleDriver + "." + MHT_DHT_DriverImage.FileName.Split('.').Last();
                            MHT_DHT_DriverLicenseImage.SaveAs(Path.Combine(Server.MapPath(virtualFilePath)));
                            hiredVehicleDriver.DriverLicenseImageLocation = virtualFilePath;
                            model.MHT_DHT_DriverLicenseImageLocation = virtualFilePath;
                        }
                        bll.db.HiredVehicleDrivers.Add(hiredVehicleDriver);
                    }
                    bll.db.SaveChanges();
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Vehicle is successfully added.");
                    return RedirectToAction("Index");
                }
                catch (Exception exception)
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                }
            }
            else
            {
                string validators = "";
                if (modelValidator != ValidationStatus.OK)
                {
                    validators = validators + modelValidator;
                }
                CreateAlertMessage(AlertMessageType.Danger, "Validation Failure", validators);
            }
            ViewBag.UserCompanies = new SelectList(bll.db.Companies.Where(m => m.IsDeleted == false && m.GroupOfCompany.IsPranRFLGroup == true && m.IsUserCompany == true).OrderBy(m => m.Name), "PK_Company", "Name", model.FK_Company);
            var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && accessibleDepoes.Contains(m.PK_Depo) && m.PK_Depo != _invalidDepoPK).OrderBy(m => m.Name), "PK_Depo", "Name", model.FK_Depo);

            if (model.FK_VehicleModel != null)
            {
                var vehicleModel = bll.db.VehicleModels.Find(model.FK_VehicleModel);
                ViewBag.VehicleBrands = new SelectList(bll.db.VehicleBrands.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Name), "PK_VehicleBrand", "Name", vehicleModel.FK_VehicleBrand);
                ViewBag.VehicleModels = new SelectList(bll.db.VehicleModels.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Title), "PK_VehicleModel", "Title", model.FK_VehicleModel);
            }
            else
            {
                ViewBag.VehicleBrands = new SelectList(bll.db.VehicleBrands.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Name), "PK_VehicleBrand", "Name");
                ViewBag.VehicleModels = new SelectList(bll.db.VehicleModels.Where(vbm => vbm.PK_VehicleModel == null).OrderBy(m => m.Title), "PK_VehicleModel", "Title");
            }
            ViewBag.VehicleTypesDict = new SelectList(VehicleTypesDict.OrderBy(m => m.Value), "Key", "Value", model.VehicleType);
            ViewBag.FuelTypesDict = new SelectList(FuelTypesDict, "Key", "Value", model.FuelType);
            ViewBag.CapacityTonDict = new SelectList(CapacityTonDict, "Key", "Value", model.CapacityTon);
            ViewBag.NumberPlate_StatusesDict = new SelectList(NumberPlate_StatusesDict, "Key", "Value", model.NumberPlate_IsDigital);
            ViewBag.PurchaseCompanies = new SelectList(bll.db.Companies.Where(m => m.IsDeleted == false && m.IsPruchaseCompany == true).OrderBy(m => m.Name), "PK_Company", "Name", model.Internal_FK_PurchasingCompany);
            ViewBag.FinancingCompanies = new SelectList(bll.db.FinancingCompanies.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_FinancingCompany", "Name", model.Internal_FK_FinancingCompany);
            ViewBag.MHT_DHT_DriverLicenseTypeDict = new SelectList(MHT_DHT_DriverLicenseTypeDict, "Key", "Value", model.MHT_DHT_DriverLicenseType);
            ViewBag.MHT_DHT_VehicleSizeDict = new SelectList(MHT_DHT_VehicleSizeDict, "Key", "Value", model.MHT_DHT_VehicleSize);

            return View(model);
        }

        public ActionResult Edit_DHT(Guid id)
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
                var model = bll.db.Vehicles.Find(id);
                if (model != null)
                {
                    ViewBag.model = model;
                    ViewBag.UserCompanies = new SelectList(bll.db.Companies.Where(m => m.IsDeleted == false && m.GroupOfCompany.IsPranRFLGroup == true && m.IsUserCompany == true).OrderBy(m => m.Name), "PK_Company", "Name", model.FK_Company);
                    var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
                    var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
                    ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && accessibleDepoes.Contains(m.PK_Depo) && m.PK_Depo != _invalidDepoPK).OrderBy(m => m.Name), "PK_Depo", "Name", model.FK_Depo);

                    if (model.FK_VehicleModel != null)
                    {
                        var vehicleModel = bll.db.VehicleModels.Find(model.FK_VehicleModel);
                        ViewBag.VehicleBrands = new SelectList(bll.db.VehicleBrands.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Name), "PK_VehicleBrand", "Name", vehicleModel.FK_VehicleBrand);
                        ViewBag.VehicleModels = new SelectList(bll.db.VehicleModels.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Title), "PK_VehicleModel", "Title", model.FK_VehicleModel);
                    }
                    else
                    {
                        ViewBag.VehicleBrands = new SelectList(bll.db.VehicleBrands.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Name), "PK_VehicleBrand", "Name");
                        ViewBag.VehicleModels = new SelectList(bll.db.VehicleBrands.Where(vbm => vbm.PK_VehicleBrand == null).OrderBy(m => m.Name), "PK_VehicleBrand", "Name");
                    }
                    ViewBag.VehicleTypesDict = new SelectList(VehicleTypesDict, "Key", "Value", model.VehicleType);
                    ViewBag.FuelTypesDict = new SelectList(FuelTypesDict, "Key", "Value", model.FuelType);
                    ViewBag.CapacityTonDict = new SelectList(CapacityTonDict, "Key", "Value", model.CapacityTon);
                    ViewBag.NumberPlate_StatusesDict = new SelectList(NumberPlate_StatusesDict, "Key", "Value", model.NumberPlate_IsDigital);

                    ViewBag.PurchaseCompanies = new SelectList(bll.db.Companies.Where(m => m.IsDeleted == false && m.IsPruchaseCompany == true).OrderBy(m => m.Name), "PK_Company", "Name", model.Internal_FK_PurchasingCompany);
                    ViewBag.FinancingCompanies = new SelectList(bll.db.FinancingCompanies.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_FinancingCompany", "Name", model.Internal_FK_FinancingCompany);
                    ViewBag.MHT_DHT_DriverLicenseTypeDict = new SelectList(MHT_DHT_DriverLicenseTypeDict, "Key", "Value", model.MHT_DHT_DriverLicenseType);
                    ViewBag.MHT_DHT_VehicleSizeDict = new SelectList(MHT_DHT_VehicleSizeDict, "Key", "Value", model.MHT_DHT_VehicleSize);

                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
        [HttpPost]
        public ActionResult Edit_DHT(Vehicle model, HttpPostedFileBase VehicleImage, HttpPostedFileBase MHT_DHT_DriverImage, HttpPostedFileBase MHT_DHT_DriverLicenseImage)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }

            string modelValidator = bll.IsValidModel_ToEdit_DHT(model);


            if (modelValidator == ValidationStatus.OK)
            {

                try
                {
                    var db_model = bll.FilterToDBModel_DHT(model);

                    if (VehicleImage != null)
                    {
                        string virtualFolderPath = CommonClass.ImageDirectory + "Vehicles/" + db_model.PK_Vehicle + "/";

                        //# create folder
                        string physicalFolderPath = Path.Combine(Server.MapPath(virtualFolderPath));
                        if (!Directory.Exists(physicalFolderPath))
                        {
                            Directory.CreateDirectory(physicalFolderPath);
                        }
                        string virtualFilePath = virtualFolderPath + "This_Vehicle" + "." + VehicleImage.FileName.Split('.').Last();
                        VehicleImage.SaveAs(Path.Combine(Server.MapPath(virtualFilePath)));

                        db_model.ImageLocation = virtualFilePath;
                    }

                    if (model.MHT_DHT_IsNewDriver == true)
                    {
                        var hiredVehicleDriver = new HiredVehicleDriver();
                        hiredVehicleDriver.PK_HiredVehicleDriver = Guid.NewGuid();
                        hiredVehicleDriver.CreatedAt = DateTime.Now;
                        hiredVehicleDriver.FK_CreatedByUser = CurrentUser.PK_User;
                        hiredVehicleDriver.IsDeleted = false;
                        hiredVehicleDriver.FK_Vehicle = db_model.PK_Vehicle;

                        hiredVehicleDriver.DriverName = string.IsNullOrEmpty(model.MHT_DHT_DriverName) ? null : model.MHT_DHT_DriverName.Trim().ToUpper();
                        hiredVehicleDriver.DriverLiceneseNumber = string.IsNullOrEmpty(model.MHT_DHT_DriverLiceneseNumber) ? null : model.MHT_DHT_DriverLiceneseNumber.Trim().ToUpper();
                        hiredVehicleDriver.DriverLicenseType = model.MHT_DHT_DriverLicenseType;
                        hiredVehicleDriver.DriverContactNumber = model.MHT_DHT_DriverContactNumber;
                        hiredVehicleDriver.DriverFatherName = string.IsNullOrEmpty(model.MHT_DHT_DriverFatherName) ? null : model.MHT_DHT_DriverFatherName.Trim().ToUpper();
                        hiredVehicleDriver.DriverAddressVillage = string.IsNullOrEmpty(model.MHT_DHT_DriverAddressVillage) ? null : model.MHT_DHT_DriverAddressVillage.Trim().ToUpper();
                        hiredVehicleDriver.DriverAddressPostOfiice = string.IsNullOrEmpty(model.MHT_DHT_DriverAddressPostOfiice) ? null : model.MHT_DHT_DriverAddressPostOfiice.Trim().ToUpper();
                        hiredVehicleDriver.DriverAddressThana = string.IsNullOrEmpty(model.MHT_DHT_DriverAddressThana) ? null : model.MHT_DHT_DriverAddressThana.Trim().ToUpper();
                        hiredVehicleDriver.DriverAddressDistrict = string.IsNullOrEmpty(model.MHT_DHT_DriverAddressDistrict) ? null : model.MHT_DHT_DriverAddressDistrict.Trim().ToUpper();
                        hiredVehicleDriver.DriverNID = string.IsNullOrEmpty(model.MHT_DHT_DriverNID) ? null : model.MHT_DHT_DriverNID.Trim().ToUpper();
                        hiredVehicleDriver.DriverSalary = model.MHT_DHT_DriverSalary;

                        string virtualFolderPath = CommonClass.ImageDirectory + "Vehicles/" + db_model.PK_Vehicle + "/HiredVehicleDriver/";
                        //# create folder
                        string physicalFolderPath = Path.Combine(Server.MapPath(virtualFolderPath));
                        if (!Directory.Exists(physicalFolderPath))
                        {
                            Directory.CreateDirectory(physicalFolderPath);
                        }

                        if (MHT_DHT_DriverImage != null)
                        {
                            string virtualFilePath = virtualFolderPath + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace(":", "-") + " DriverImage " + hiredVehicleDriver.PK_HiredVehicleDriver + "." + MHT_DHT_DriverImage.FileName.Split('.').Last();
                            MHT_DHT_DriverImage.SaveAs(Path.Combine(Server.MapPath(virtualFilePath)));
                            hiredVehicleDriver.DriverImageLocation = virtualFilePath;
                            model.MHT_DHT_DriverImageLocation = virtualFilePath;
                        }
                        else
                        {
                            model.MHT_DHT_DriverImageLocation = null;
                        }

                        if (MHT_DHT_DriverLicenseImage != null)
                        {
                            hiredVehicleDriver.DriverLiceneseNumber = db_model.MHT_DHT_DriverLiceneseNumber;
                            string virtualFilePath = virtualFolderPath + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace(":", "-") + " LicenseNumber " + hiredVehicleDriver.PK_HiredVehicleDriver + "." + MHT_DHT_DriverImage.FileName.Split('.').Last();
                            MHT_DHT_DriverLicenseImage.SaveAs(Path.Combine(Server.MapPath(virtualFilePath)));
                            hiredVehicleDriver.DriverLicenseImageLocation = virtualFilePath;
                            model.MHT_DHT_DriverLicenseImageLocation = virtualFilePath;
                        }
                        else
                        {
                            model.MHT_DHT_DriverLicenseImageLocation = null;
                        }

                        bll.db.HiredVehicleDrivers.Add(hiredVehicleDriver);
                    }
                    else if (model.MHT_DHT_IsNewDriver == false)
                    {
                        var hiredVehicleDriver = bll.db.HiredVehicleDrivers.Where(m => m.FK_Vehicle == model.PK_Vehicle).OrderByDescending(m => m.CreatedAt).FirstOrDefault();
                        hiredVehicleDriver.UpdatedAt = DateTime.Now;
                        hiredVehicleDriver.FK_UpdatedByUser = CurrentUser.PK_User;

                        hiredVehicleDriver.DriverName = string.IsNullOrEmpty(model.MHT_DHT_DriverName) ? null : model.MHT_DHT_DriverName.Trim().ToUpper();
                        hiredVehicleDriver.DriverLiceneseNumber = string.IsNullOrEmpty(model.MHT_DHT_DriverLiceneseNumber) ? null : model.MHT_DHT_DriverLiceneseNumber.Trim().ToUpper();
                        hiredVehicleDriver.DriverLicenseType = model.MHT_DHT_DriverLicenseType;
                        hiredVehicleDriver.DriverContactNumber = model.MHT_DHT_DriverContactNumber;
                        hiredVehicleDriver.DriverFatherName = string.IsNullOrEmpty(model.MHT_DHT_DriverFatherName) ? null : model.MHT_DHT_DriverFatherName.Trim().ToUpper();
                        hiredVehicleDriver.DriverAddressVillage = string.IsNullOrEmpty(model.MHT_DHT_DriverAddressVillage) ? null : model.MHT_DHT_DriverAddressVillage.Trim().ToUpper();
                        hiredVehicleDriver.DriverAddressPostOfiice = string.IsNullOrEmpty(model.MHT_DHT_DriverAddressPostOfiice) ? null : model.MHT_DHT_DriverAddressPostOfiice.Trim().ToUpper();
                        hiredVehicleDriver.DriverAddressThana = string.IsNullOrEmpty(model.MHT_DHT_DriverAddressThana) ? null : model.MHT_DHT_DriverAddressThana.Trim().ToUpper();
                        hiredVehicleDriver.DriverAddressDistrict = string.IsNullOrEmpty(model.MHT_DHT_DriverAddressDistrict) ? null : model.MHT_DHT_DriverAddressDistrict.Trim().ToUpper();
                        hiredVehicleDriver.DriverNID = string.IsNullOrEmpty(model.MHT_DHT_DriverNID) ? null : model.MHT_DHT_DriverNID.Trim().ToUpper();
                        hiredVehicleDriver.DriverSalary = model.MHT_DHT_DriverSalary;

                        string virtualFolderPath = CommonClass.ImageDirectory + "Vehicles/" + db_model.PK_Vehicle + "/HiredVehicleDriver/";
                        //# create folder
                        string physicalFolderPath = Path.Combine(Server.MapPath(virtualFolderPath));
                        if (!Directory.Exists(physicalFolderPath))
                        {
                            Directory.CreateDirectory(physicalFolderPath);
                        }

                        if (MHT_DHT_DriverImage != null)
                        {
                            string virtualFilePath = virtualFolderPath + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace(":", "-") + " DriverImage " + hiredVehicleDriver.PK_HiredVehicleDriver + "." + MHT_DHT_DriverImage.FileName.Split('.').Last();
                            MHT_DHT_DriverImage.SaveAs(Path.Combine(Server.MapPath(virtualFilePath)));
                            hiredVehicleDriver.DriverImageLocation = virtualFilePath;
                            model.MHT_DHT_DriverImageLocation = virtualFilePath;
                        }
                        else
                        {
                            //model.MHT_DHT_DriverImageLocation = null;
                        }

                        if (MHT_DHT_DriverLicenseImage != null)
                        {
                            hiredVehicleDriver.DriverLiceneseNumber = db_model.MHT_DHT_DriverLiceneseNumber;
                            string virtualFilePath = virtualFolderPath + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace(":", "-") + " LicenseNumber " + hiredVehicleDriver.PK_HiredVehicleDriver + "." + MHT_DHT_DriverImage.FileName.Split('.').Last();
                            MHT_DHT_DriverLicenseImage.SaveAs(Path.Combine(Server.MapPath(virtualFilePath)));
                            hiredVehicleDriver.DriverLicenseImageLocation = virtualFilePath;
                            model.MHT_DHT_DriverLicenseImageLocation = virtualFilePath;
                        }
                        else
                        {
                            //model.MHT_DHT_DriverLicenseImageLocation = null;
                        }
                    }

                    bll.db.SaveChanges();
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Vehicle is successfully edited.");
                    return RedirectToAction("Index");
                }
                catch (Exception exception)
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                }
            }
            else
            {
                string validators = "";
                if (modelValidator != ValidationStatus.OK)
                {
                    validators = validators + modelValidator;
                }
                CreateAlertMessage(AlertMessageType.Danger, "Validation Failure", validators);
            }
            ViewBag.model = model;
            ViewBag.UserCompanies = new SelectList(bll.db.Companies.Where(m => m.IsDeleted == false && m.GroupOfCompany.IsPranRFLGroup == true && m.IsUserCompany == true).OrderBy(m => m.Name), "PK_Company", "Name", model.FK_Company);
            var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && accessibleDepoes.Contains(m.PK_Depo) && m.PK_Depo != _invalidDepoPK).OrderBy(m => m.Name), "PK_Depo", "Name", model.FK_Depo);

            if (model.FK_VehicleModel != null)
            {
                var vehicleModel = bll.db.VehicleModels.Find(model.FK_VehicleModel);
                ViewBag.VehicleBrands = new SelectList(bll.db.VehicleBrands.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Name), "PK_VehicleBrand", "Name", vehicleModel.FK_VehicleBrand);
                ViewBag.VehicleModels = new SelectList(bll.db.VehicleModels.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Title), "PK_VehicleModel", "Title", model.FK_VehicleModel);
            }
            else
            {
                ViewBag.VehicleBrands = new SelectList(bll.db.VehicleBrands.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Name), "PK_VehicleBrand", "Name");
                ViewBag.VehicleModels = new SelectList(bll.db.VehicleModels.Where(vbm => vbm.PK_VehicleModel == null).OrderBy(m => m.Title), "PK_VehicleModel", "Title");
            }
            ViewBag.VehicleTypesDict = new SelectList(VehicleTypesDict.OrderBy(m => m.Value), "Key", "Value", model.VehicleType);
            ViewBag.FuelTypesDict = new SelectList(FuelTypesDict, "Key", "Value", model.FuelType);
            ViewBag.CapacityTonDict = new SelectList(CapacityTonDict, "Key", "Value", model.CapacityTon);
            ViewBag.NumberPlate_StatusesDict = new SelectList(NumberPlate_StatusesDict, "Key", "Value", model.NumberPlate_IsDigital);

            ViewBag.PurchaseCompanies = new SelectList(bll.db.Companies.Where(m => m.IsDeleted == false && m.IsPruchaseCompany == true).OrderBy(m => m.Name), "PK_Company", "Name", model.Internal_FK_PurchasingCompany);
            ViewBag.FinancingCompanies = new SelectList(bll.db.FinancingCompanies.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_FinancingCompany", "Name", model.Internal_FK_FinancingCompany);
            ViewBag.MHT_DHT_DriverLicenseTypeDict = new SelectList(MHT_DHT_DriverLicenseTypeDict, "Key", "Value", model.MHT_DHT_DriverLicenseType);
            ViewBag.MHT_DHT_VehicleSizeDict = new SelectList(MHT_DHT_VehicleSizeDict.OrderBy(m => m.Value), "Key", "Value", model.MHT_DHT_VehicleSize);

            return View(model);
        }

        public ActionResult AddToVehicle(Int64 id)
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
                var temporaryVehicle = bll.db.TemporaryVehicles.Where(m => m.PK_TemporaryVehicle == id).FirstOrDefault();
                var model = new Vehicle();
                model.RegistrationNumber = temporaryVehicle.RegistrationNumber;
                if (model != null)
                {
                    ViewBag.model = model;
                    ViewBag.UserCompanies = new SelectList(bll.db.Companies.Where(m => m.IsDeleted == false && m.GroupOfCompany.IsPranRFLGroup == true && m.IsUserCompany == true).OrderBy(m => m.Name), "PK_Company", "Name", model.FK_Company);
                    var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
                    var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
                    ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && accessibleDepoes.Contains(m.PK_Depo) && m.PK_Depo != _invalidDepoPK).OrderBy(m => m.Name), "PK_Depo", "Name", model.FK_Depo);

                    if (model.FK_VehicleModel != null)
                    {
                        var vehicleModel = bll.db.VehicleModels.Find(model.FK_VehicleModel);
                        ViewBag.VehicleBrands = new SelectList(bll.db.VehicleBrands.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Name), "PK_VehicleBrand", "Name", vehicleModel.FK_VehicleBrand);
                        ViewBag.VehicleModels = new SelectList(bll.db.VehicleModels.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Title), "PK_VehicleModel", "Title", model.FK_VehicleModel);
                    }
                    else
                    {
                        ViewBag.VehicleBrands = new SelectList(bll.db.VehicleBrands.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Name), "PK_VehicleBrand", "Name");
                        ViewBag.VehicleModels = new SelectList(bll.db.VehicleBrands.Where(vbm => vbm.PK_VehicleBrand == null).OrderBy(m => m.Name), "PK_VehicleBrand", "Name");
                    }
                    ViewBag.VehicleTypesDict = new SelectList(VehicleTypesDict, "Key", "Value", model.VehicleType);
                    ViewBag.FuelTypesDict = new SelectList(FuelTypesDict, "Key", "Value", model.FuelType);
                    ViewBag.CapacityTonDict = new SelectList(CapacityTonDict, "Key", "Value", model.CapacityTon);
                    ViewBag.NumberPlate_StatusesDict = new SelectList(NumberPlate_StatusesDict, "Key", "Value", model.NumberPlate_IsDigital);

                    ViewBag.PurchaseCompanies = new SelectList(bll.db.Companies.Where(m => m.IsDeleted == false && m.IsPruchaseCompany == true).OrderBy(m => m.Name), "PK_Company", "Name", model.Internal_FK_PurchasingCompany);
                    ViewBag.FinancingCompanies = new SelectList(bll.db.FinancingCompanies.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_FinancingCompany", "Name", model.Internal_FK_FinancingCompany);
                    ViewBag.MHT_DHT_DriverLicenseTypeDict = new SelectList(MHT_DHT_DriverLicenseTypeDict, "Key", "Value", model.MHT_DHT_DriverLicenseType);
                    ViewBag.MHT_DHT_VehicleSizeDict = new SelectList(MHT_DHT_VehicleSizeDict, "Key", "Value", model.MHT_DHT_VehicleSize);

                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
        [HttpPost]
        public ActionResult AddToVehicle(Vehicle model, HttpPostedFileBase VehicleImage, HttpPostedFileBase MHT_DHT_DriverImage, HttpPostedFileBase MHT_DHT_DriverLicenseImage)
        {

            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }

            string modelValidator = bll.IsValidModel_ToCreate_DHT(model);


            if (modelValidator == ValidationStatus.OK)
            {

                try
                {

                    var db_model = bll.FilterToDBModel_DHT(model);
                    if (VehicleImage != null)
                    {
                        string virtualFolderPath = CommonClass.ImageDirectory + "Vehicles/" + db_model.PK_Vehicle + "/";

                        //# create folder
                        string physicalFolderPath = Path.Combine(Server.MapPath(virtualFolderPath));
                        if (!Directory.Exists(physicalFolderPath))
                        {
                            Directory.CreateDirectory(physicalFolderPath);
                        }
                        string virtualFilePath = virtualFolderPath + "This_Vehicle" + "." + VehicleImage.FileName.Split('.').Last();
                        VehicleImage.SaveAs(Path.Combine(Server.MapPath(virtualFilePath)));

                        db_model.ImageLocation = virtualFilePath;
                    }

                    bll.db.Vehicles.Add(db_model);
                    if (!string.IsNullOrEmpty(model.MHT_DHT_DriverName))
                    {
                        var hiredVehicleDriver = new HiredVehicleDriver();
                        hiredVehicleDriver.PK_HiredVehicleDriver = Guid.NewGuid();
                        hiredVehicleDriver.CreatedAt = DateTime.Now;
                        hiredVehicleDriver.FK_CreatedByUser = CurrentUser.PK_User;
                        hiredVehicleDriver.IsDeleted = false;
                        hiredVehicleDriver.FK_Vehicle = db_model.PK_Vehicle;

                        hiredVehicleDriver.DriverName = string.IsNullOrEmpty(model.MHT_DHT_DriverName) ? null : model.MHT_DHT_DriverName.Trim().ToUpper();
                        hiredVehicleDriver.DriverLiceneseNumber = string.IsNullOrEmpty(model.MHT_DHT_DriverLiceneseNumber) ? null : model.MHT_DHT_DriverLiceneseNumber.Trim().ToUpper();
                        hiredVehicleDriver.DriverLicenseType = model.MHT_DHT_DriverLicenseType;
                        hiredVehicleDriver.DriverContactNumber = model.MHT_DHT_DriverContactNumber;
                        hiredVehicleDriver.DriverFatherName = string.IsNullOrEmpty(model.MHT_DHT_DriverFatherName) ? null : model.MHT_DHT_DriverFatherName.Trim().ToUpper();
                        hiredVehicleDriver.DriverAddressVillage = string.IsNullOrEmpty(model.MHT_DHT_DriverAddressVillage) ? null : model.MHT_DHT_DriverAddressVillage.Trim().ToUpper();
                        hiredVehicleDriver.DriverAddressPostOfiice = string.IsNullOrEmpty(model.MHT_DHT_DriverAddressPostOfiice) ? null : model.MHT_DHT_DriverAddressPostOfiice.Trim().ToUpper();
                        hiredVehicleDriver.DriverAddressThana = string.IsNullOrEmpty(model.MHT_DHT_DriverAddressThana) ? null : model.MHT_DHT_DriverAddressThana.Trim().ToUpper();
                        hiredVehicleDriver.DriverAddressDistrict = string.IsNullOrEmpty(model.MHT_DHT_DriverAddressDistrict) ? null : model.MHT_DHT_DriverAddressDistrict.Trim().ToUpper();
                        hiredVehicleDriver.DriverNID = string.IsNullOrEmpty(model.MHT_DHT_DriverNID) ? null : model.MHT_DHT_DriverNID.Trim().ToUpper();
                        hiredVehicleDriver.DriverSalary = model.MHT_DHT_DriverSalary;

                        string virtualFolderPath = CommonClass.ImageDirectory + "Vehicles/" + db_model.PK_Vehicle + "/HiredVehicleDriver/";
                        //# create folder
                        string physicalFolderPath = Path.Combine(Server.MapPath(virtualFolderPath));
                        if (!Directory.Exists(physicalFolderPath))
                        {
                            Directory.CreateDirectory(physicalFolderPath);
                        }

                        if (MHT_DHT_DriverImage != null)
                        {
                            string virtualFilePath = virtualFolderPath + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace(":", "-") + " DriverImage " + hiredVehicleDriver.PK_HiredVehicleDriver + "." + MHT_DHT_DriverImage.FileName.Split('.').Last();
                            MHT_DHT_DriverImage.SaveAs(Path.Combine(Server.MapPath(virtualFilePath)));
                            hiredVehicleDriver.DriverImageLocation = virtualFilePath;
                            model.MHT_DHT_DriverImageLocation = virtualFilePath;
                        }

                        if (MHT_DHT_DriverLicenseImage != null)
                        {
                            hiredVehicleDriver.DriverLiceneseNumber = db_model.MHT_DHT_DriverLiceneseNumber;
                            string virtualFilePath = virtualFolderPath + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace(":", "-") + " LicenseNumber " + hiredVehicleDriver.PK_HiredVehicleDriver + "." + MHT_DHT_DriverImage.FileName.Split('.').Last();
                            MHT_DHT_DriverLicenseImage.SaveAs(Path.Combine(Server.MapPath(virtualFilePath)));
                            hiredVehicleDriver.DriverLicenseImageLocation = virtualFilePath;
                            model.MHT_DHT_DriverLicenseImageLocation = virtualFilePath;
                        }
                        bll.db.HiredVehicleDrivers.Add(hiredVehicleDriver);
                    }
                    bll.db.SaveChanges();
                    var temporaryVehicle = bll.db.TemporaryVehicles.Where(m => m.RegistrationNumber == db_model.RegistrationNumber).FirstOrDefault();
                    var response = new VehicleGateNewAPIController().GateIn_Existing(db_model.PK_Vehicle, temporaryVehicle);
                    if (response == "OK")
                    {
                        CreateAlertMessage(AlertMessageType.Success, "Success", "Temporary Vehicle is successfully added to fixed vehicle.");
                        return Redirect("/VehicleGateNew/TemporaryVehicleIndex");
                    }
                    else
                    {
                        CreateAlertMessage(AlertMessageType.Warning, "Warning", "Temporary Vehicle is successfully added to fixed vehicle. Please, Confirm gate entry from mobile app.");
                        return Redirect("/VehicleGateNew/TemporaryVehicleIndex");
                    }
                    //new VehicleGateNewAPIController().GateIn_Existing(db_model.PK_Vehicle, temporaryVehicle);
                    //CreateAlertMessage(AlertMessageType.Success, "Success", "Temporary Vehicle is successfully added to fixed vehicle.");
                    //return Redirect("/VehicleGateNew/TemporaryVehicleIndex");
                }
                catch (Exception exception)
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                    return Redirect("/VehicleGateNew/TemporaryVehicleIndex");
                }
            }
            else
            {
                string validators = "";
                if (modelValidator != ValidationStatus.OK)
                {
                    validators = validators + modelValidator;
                }
                CreateAlertMessage(AlertMessageType.Danger, "Validation Failure", validators);
                return Redirect("/VehicleGateNew/TemporaryVehicleIndex");
            }

        }

        public ActionResult View_DHT(Guid id)
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
                var model = bll.db.Vehicles.Find(id);
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
        #endregion 

        #region //MHT
        public ActionResult Create_MHT()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }


            ViewBag.UserCompanies = new SelectList(bll.db.Companies.Where(m => m.IsDeleted == false && m.GroupOfCompany.IsPranRFLGroup == true && m.IsUserCompany == true).OrderBy(m => m.Name), "PK_Company", "Name");
            var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && accessibleDepoes.Contains(m.PK_Depo) && m.PK_Depo != _invalidDepoPK).OrderBy(m => m.Name), "PK_Depo", "Name");
            ViewBag.VehicleBrands = new SelectList(bll.db.VehicleBrands.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Name), "PK_VehicleBrand", "Name");
            ViewBag.VehicleModels = new SelectList(bll.db.VehicleBrands.Where(vbm => vbm.PK_VehicleBrand == null).OrderBy(m => m.Name), "PK_VehicleBrand", "Name");
            ViewBag.VehicleTypesDict = new SelectList(VehicleTypesDict.OrderBy(m => m.Value), "Key", "Value");
            ViewBag.FuelTypesDict = new SelectList(FuelTypesDict, "Key", "Value");
            ViewBag.CapacityTonDict = new SelectList(CapacityTonDict, "Key", "Value");
            ViewBag.NumberPlate_StatusesDict = new SelectList(NumberPlate_StatusesDict, "Key", "Value");

            ViewBag.PurchaseCompanies = new SelectList(bll.db.Companies.Where(m => m.IsDeleted == false && m.IsPruchaseCompany == true).OrderBy(m => m.Name), "PK_Company", "Name");
            ViewBag.FinancingCompanies = new SelectList(bll.db.FinancingCompanies.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_FinancingCompany", "Name");
            ViewBag.MHT_DHT_DriverLicenseTypeDict = new SelectList(MHT_DHT_DriverLicenseTypeDict, "Key", "Value");
            ViewBag.MHT_DHT_VehicleSizeDict = new SelectList(MHT_DHT_VehicleSizeDict, "Key", "Value");

            return View();
        }
        [HttpPost]
        public ActionResult Create_MHT(Vehicle model, HttpPostedFileBase VehicleImage, HttpPostedFileBase DeedImageInput, HttpPostedFileBase MHT_DHT_DriverImage, HttpPostedFileBase MHT_DHT_DriverLicenseImage)
        {

            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }

            var temporaryVehile = bll.db.TemporaryVehicles.Where(m => m.RegistrationNumber == model.RegistrationNumber).FirstOrDefault();
            if (temporaryVehile != null)
            {
                CreateAlertMessage(AlertMessageType.Information, "Information", "This vehicle is added into temporary vehicle list. Please, search " + temporaryVehile.RegistrationNumber + " here and add from here.");
                return Redirect("/VehicleGateNew/TemporaryVehicleIndex");
            }

            string modelValidator = bll.IsValidModel_ToCreate_MHT(model);


            if (modelValidator == ValidationStatus.OK)
            {

                try
                {

                    var db_model = bll.FilterToDBModel_MHT(model);
                    if (VehicleImage != null)
                    {
                        string virtualFolderPath = CommonClass.ImageDirectory + "Vehicles/" + db_model.PK_Vehicle + "/";

                        //# create folder
                        string physicalFolderPath = Path.Combine(Server.MapPath(virtualFolderPath));
                        if (!Directory.Exists(physicalFolderPath))
                        {
                            Directory.CreateDirectory(physicalFolderPath);
                        }
                        string virtualFilePath = virtualFolderPath + "This_Vehicle" + "." + VehicleImage.FileName.Split('.').Last();
                        VehicleImage.SaveAs(Path.Combine(Server.MapPath(virtualFilePath)));

                        db_model.ImageLocation = virtualFilePath;
                    }
                    bll.db.Vehicles.Add(db_model);
                    if (!string.IsNullOrEmpty(model.MHT_DHT_DriverName))
                    {
                        var hiredVehicleDriver = new HiredVehicleDriver();
                        hiredVehicleDriver.PK_HiredVehicleDriver = Guid.NewGuid();
                        hiredVehicleDriver.CreatedAt = DateTime.Now;
                        hiredVehicleDriver.FK_CreatedByUser = CurrentUser.PK_User;
                        hiredVehicleDriver.IsDeleted = false;
                        hiredVehicleDriver.FK_Vehicle = db_model.PK_Vehicle;

                        hiredVehicleDriver.DriverName = string.IsNullOrEmpty(model.MHT_DHT_DriverName) ? null : model.MHT_DHT_DriverName.Trim().ToUpper();
                        hiredVehicleDriver.DriverLiceneseNumber = string.IsNullOrEmpty(model.MHT_DHT_DriverLiceneseNumber) ? null : model.MHT_DHT_DriverLiceneseNumber.Trim().ToUpper();
                        hiredVehicleDriver.DriverLicenseType = model.MHT_DHT_DriverLicenseType;
                        hiredVehicleDriver.DriverContactNumber = model.MHT_DHT_DriverContactNumber;
                        hiredVehicleDriver.DriverFatherName = string.IsNullOrEmpty(model.MHT_DHT_DriverFatherName) ? null : model.MHT_DHT_DriverFatherName.Trim().ToUpper();
                        hiredVehicleDriver.DriverAddressVillage = string.IsNullOrEmpty(model.MHT_DHT_DriverAddressVillage) ? null : model.MHT_DHT_DriverAddressVillage.Trim().ToUpper();
                        hiredVehicleDriver.DriverAddressPostOfiice = string.IsNullOrEmpty(model.MHT_DHT_DriverAddressPostOfiice) ? null : model.MHT_DHT_DriverAddressPostOfiice.Trim().ToUpper();
                        hiredVehicleDriver.DriverAddressThana = string.IsNullOrEmpty(model.MHT_DHT_DriverAddressThana) ? null : model.MHT_DHT_DriverAddressThana.Trim().ToUpper();
                        hiredVehicleDriver.DriverAddressDistrict = string.IsNullOrEmpty(model.MHT_DHT_DriverAddressDistrict) ? null : model.MHT_DHT_DriverAddressDistrict.Trim().ToUpper();
                        hiredVehicleDriver.DriverNID = string.IsNullOrEmpty(model.MHT_DHT_DriverNID) ? null : model.MHT_DHT_DriverNID.Trim().ToUpper();
                        hiredVehicleDriver.DriverSalary = model.MHT_DHT_DriverSalary;

                        string virtualFolderPath = CommonClass.ImageDirectory + "Vehicles/" + db_model.PK_Vehicle + "/HiredVehicleDriver/";
                        //# create folder
                        string physicalFolderPath = Path.Combine(Server.MapPath(virtualFolderPath));
                        if (!Directory.Exists(physicalFolderPath))
                        {
                            Directory.CreateDirectory(physicalFolderPath);
                        }

                        if (MHT_DHT_DriverImage != null)
                        {
                            string virtualFilePath = virtualFolderPath + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace(":", "-") + " DriverImage " + hiredVehicleDriver.PK_HiredVehicleDriver + "." + MHT_DHT_DriverImage.FileName.Split('.').Last();
                            MHT_DHT_DriverImage.SaveAs(Path.Combine(Server.MapPath(virtualFilePath)));
                            hiredVehicleDriver.DriverImageLocation = virtualFilePath;
                            model.MHT_DHT_DriverImageLocation = virtualFilePath;
                        }

                        if (MHT_DHT_DriverLicenseImage != null)
                        {
                            hiredVehicleDriver.DriverLiceneseNumber = db_model.MHT_DHT_DriverLiceneseNumber;
                            string virtualFilePath = virtualFolderPath + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace(":", "-") + " LicenseNumber " + hiredVehicleDriver.PK_HiredVehicleDriver + "." + MHT_DHT_DriverImage.FileName.Split('.').Last();
                            MHT_DHT_DriverLicenseImage.SaveAs(Path.Combine(Server.MapPath(virtualFilePath)));
                            hiredVehicleDriver.DriverLicenseImageLocation = virtualFilePath;
                            model.MHT_DHT_DriverLicenseImageLocation = virtualFilePath;
                        }
                        bll.db.HiredVehicleDrivers.Add(hiredVehicleDriver);
                    }
                    bll.db.SaveChanges();
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Vehicle is successfully added.");
                    return RedirectToAction("Index");
                }
                catch (Exception exception)
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                }
            }
            else
            {
                string validators = "";
                if (modelValidator != ValidationStatus.OK)
                {
                    validators = validators + modelValidator;
                }
                CreateAlertMessage(AlertMessageType.Danger, "Validation Failure", validators);
            }
            ViewBag.UserCompanies = new SelectList(bll.db.Companies.Where(m => m.IsDeleted == false && m.GroupOfCompany.IsPranRFLGroup == true && m.IsUserCompany == true).OrderBy(m => m.Name), "PK_Company", "Name", model.FK_Company);
            var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && accessibleDepoes.Contains(m.PK_Depo) && m.PK_Depo != _invalidDepoPK).OrderBy(m => m.Name), "PK_Depo", "Name", model.FK_Depo);

            if (model.FK_VehicleModel != null)
            {
                var vehicleModel = bll.db.VehicleModels.Find(model.FK_VehicleModel);
                ViewBag.VehicleBrands = new SelectList(bll.db.VehicleBrands.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Name), "PK_VehicleBrand", "Name", vehicleModel.FK_VehicleBrand);
                ViewBag.VehicleModels = new SelectList(bll.db.VehicleModels.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Title), "PK_VehicleModel", "Title", model.FK_VehicleModel);
            }
            else
            {
                ViewBag.VehicleBrands = new SelectList(bll.db.VehicleBrands.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Name), "PK_VehicleBrand", "Name");
                ViewBag.VehicleModels = new SelectList(bll.db.VehicleModels.Where(vbm => vbm.PK_VehicleModel == null).OrderBy(m => m.Title), "PK_VehicleModel", "Title");
            }
            ViewBag.VehicleTypesDict = new SelectList(VehicleTypesDict.OrderBy(m => m.Value), "Key", "Value", model.VehicleType);
            ViewBag.FuelTypesDict = new SelectList(FuelTypesDict, "Key", "Value", model.FuelType);
            ViewBag.CapacityTonDict = new SelectList(CapacityTonDict, "Key", "Value", model.CapacityTon);
            ViewBag.NumberPlate_StatusesDict = new SelectList(NumberPlate_StatusesDict, "Key", "Value", model.NumberPlate_IsDigital);
            ViewBag.PurchaseCompanies = new SelectList(bll.db.Companies.Where(m => m.IsDeleted == false && m.IsPruchaseCompany == true).OrderBy(m => m.Name), "PK_Company", "Name", model.Internal_FK_PurchasingCompany);
            ViewBag.FinancingCompanies = new SelectList(bll.db.FinancingCompanies.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_FinancingCompany", "Name", model.Internal_FK_FinancingCompany);
            ViewBag.MHT_DHT_DriverLicenseTypeDict = new SelectList(MHT_DHT_DriverLicenseTypeDict, "Key", "Value", model.MHT_DHT_DriverLicenseType);
            ViewBag.MHT_DHT_VehicleSizeDict = new SelectList(MHT_DHT_VehicleSizeDict, "Key", "Value", model.MHT_DHT_VehicleSize);

            return View(model);
        }

        public ActionResult Edit_MHT(Guid id)
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
                var model = bll.db.Vehicles.Find(id);
                if (model != null)
                {
                    ViewBag.model = model;
                    ViewBag.UserCompanies = new SelectList(bll.db.Companies.Where(m => m.IsDeleted == false && m.GroupOfCompany.IsPranRFLGroup == true && m.IsUserCompany == true).OrderBy(m => m.Name), "PK_Company", "Name", model.FK_Company);
                    var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
                    var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
                    ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && accessibleDepoes.Contains(m.PK_Depo) && m.PK_Depo != _invalidDepoPK).OrderBy(m => m.Name), "PK_Depo", "Name", model.FK_Depo);

                    if (model.FK_VehicleModel != null)
                    {
                        var vehicleModel = bll.db.VehicleModels.Find(model.FK_VehicleModel);
                        ViewBag.VehicleBrands = new SelectList(bll.db.VehicleBrands.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Name), "PK_VehicleBrand", "Name", vehicleModel.FK_VehicleBrand);
                        ViewBag.VehicleModels = new SelectList(bll.db.VehicleModels.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Title), "PK_VehicleModel", "Title", model.FK_VehicleModel);
                    }
                    else
                    {
                        ViewBag.VehicleBrands = new SelectList(bll.db.VehicleBrands.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Name), "PK_VehicleBrand", "Name");
                        ViewBag.VehicleModels = new SelectList(bll.db.VehicleBrands.Where(vbm => vbm.PK_VehicleBrand == null), "PK_VehicleBrand", "Name");
                    }
                    ViewBag.VehicleTypesDict = new SelectList(VehicleTypesDict.OrderBy(m => m.Value), "Key", "Value", model.VehicleType);
                    ViewBag.FuelTypesDict = new SelectList(FuelTypesDict, "Key", "Value", model.FuelType);
                    ViewBag.CapacityTonDict = new SelectList(CapacityTonDict, "Key", "Value", model.CapacityTon);
                    ViewBag.NumberPlate_StatusesDict = new SelectList(NumberPlate_StatusesDict, "Key", "Value", model.NumberPlate_IsDigital);
                    ViewBag.PurchaseCompanies = new SelectList(bll.db.Companies.Where(m => m.IsDeleted == false && m.IsPruchaseCompany == true).OrderBy(m => m.Name), "PK_Company", "Name", model.Internal_FK_PurchasingCompany);
                    ViewBag.FinancingCompanies = new SelectList(bll.db.FinancingCompanies.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_FinancingCompany", "Name", model.Internal_FK_FinancingCompany);
                    ViewBag.MHT_DHT_DriverLicenseTypeDict = new SelectList(MHT_DHT_DriverLicenseTypeDict, "Key", "Value", model.MHT_DHT_DriverLicenseType);
                    ViewBag.MHT_DHT_VehicleSizeDict = new SelectList(MHT_DHT_VehicleSizeDict, "Key", "Value", model.MHT_DHT_VehicleSize);

                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
        [HttpPost]
        public ActionResult Edit_MHT(Vehicle model, HttpPostedFileBase VehicleImage, HttpPostedFileBase DeedImageInput, HttpPostedFileBase MHT_DHT_DriverImage, HttpPostedFileBase MHT_DHT_DriverLicenseImage)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }

            string modelValidator = bll.IsValidModel_ToEdit_MHT(model);


            if (modelValidator == ValidationStatus.OK)
            {

                try
                {
                    var db_model = bll.FilterToDBModel_MHT(model);
                    if (VehicleImage != null)
                    {
                        string virtualFolderPath = CommonClass.ImageDirectory + "Vehicles/" + db_model.PK_Vehicle + "/";

                        //# create folder
                        string physicalFolderPath = Path.Combine(Server.MapPath(virtualFolderPath));
                        if (!Directory.Exists(physicalFolderPath))
                        {
                            Directory.CreateDirectory(physicalFolderPath);
                        }
                        string virtualFilePath = virtualFolderPath + "This_Vehicle" + "." + VehicleImage.FileName.Split('.').Last();
                        VehicleImage.SaveAs(Path.Combine(Server.MapPath(virtualFilePath)));

                        db_model.ImageLocation = virtualFilePath;
                    }
                    if (DeedImageInput != null)
                    {
                        string virtualFolderPath = CommonClass.ImageDirectory + "Vehicles/" + db_model.PK_Vehicle + "/";

                        //# create folder
                        string physicalFolderPath = Path.Combine(Server.MapPath(virtualFolderPath));
                        if (!Directory.Exists(physicalFolderPath))
                        {
                            Directory.CreateDirectory(physicalFolderPath);
                        }
                        string virtualFilePath = virtualFolderPath + "MHT_Deed" + "." + DeedImageInput.FileName.Split('.').Last();
                        DeedImageInput.SaveAs(Path.Combine(Server.MapPath(virtualFilePath)));

                        db_model.MHT_DeedImageLocation = virtualFilePath;
                    }

                    if (model.MHT_DHT_IsNewDriver == true)
                    {
                        var hiredVehicleDriver = new HiredVehicleDriver();
                        hiredVehicleDriver.PK_HiredVehicleDriver = Guid.NewGuid();
                        hiredVehicleDriver.CreatedAt = DateTime.Now;
                        hiredVehicleDriver.FK_CreatedByUser = CurrentUser.PK_User;
                        hiredVehicleDriver.IsDeleted = false;
                        hiredVehicleDriver.FK_Vehicle = db_model.PK_Vehicle;

                        hiredVehicleDriver.DriverName = string.IsNullOrEmpty(model.MHT_DHT_DriverName) ? null : model.MHT_DHT_DriverName.Trim().ToUpper();
                        hiredVehicleDriver.DriverLiceneseNumber = string.IsNullOrEmpty(model.MHT_DHT_DriverLiceneseNumber) ? null : model.MHT_DHT_DriverLiceneseNumber.Trim().ToUpper();
                        hiredVehicleDriver.DriverLicenseType = model.MHT_DHT_DriverLicenseType;
                        hiredVehicleDriver.DriverContactNumber = model.MHT_DHT_DriverContactNumber;
                        hiredVehicleDriver.DriverFatherName = string.IsNullOrEmpty(model.MHT_DHT_DriverFatherName) ? null : model.MHT_DHT_DriverFatherName.Trim().ToUpper();
                        hiredVehicleDriver.DriverAddressVillage = string.IsNullOrEmpty(model.MHT_DHT_DriverAddressVillage) ? null : model.MHT_DHT_DriverAddressVillage.Trim().ToUpper();
                        hiredVehicleDriver.DriverAddressPostOfiice = string.IsNullOrEmpty(model.MHT_DHT_DriverAddressPostOfiice) ? null : model.MHT_DHT_DriverAddressPostOfiice.Trim().ToUpper();
                        hiredVehicleDriver.DriverAddressThana = string.IsNullOrEmpty(model.MHT_DHT_DriverAddressThana) ? null : model.MHT_DHT_DriverAddressThana.Trim().ToUpper();
                        hiredVehicleDriver.DriverAddressDistrict = string.IsNullOrEmpty(model.MHT_DHT_DriverAddressDistrict) ? null : model.MHT_DHT_DriverAddressDistrict.Trim().ToUpper();
                        hiredVehicleDriver.DriverNID = string.IsNullOrEmpty(model.MHT_DHT_DriverNID) ? null : model.MHT_DHT_DriverNID.Trim().ToUpper();
                        hiredVehicleDriver.DriverSalary = model.MHT_DHT_DriverSalary;

                        string virtualFolderPath = CommonClass.ImageDirectory + "Vehicles/" + db_model.PK_Vehicle + "/HiredVehicleDriver/";
                        //# create folder
                        string physicalFolderPath = Path.Combine(Server.MapPath(virtualFolderPath));
                        if (!Directory.Exists(physicalFolderPath))
                        {
                            Directory.CreateDirectory(physicalFolderPath);
                        }

                        if (MHT_DHT_DriverImage != null)
                        {
                            string virtualFilePath = virtualFolderPath + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace(":", "-") + " DriverImage " + hiredVehicleDriver.PK_HiredVehicleDriver + "." + MHT_DHT_DriverImage.FileName.Split('.').Last();
                            MHT_DHT_DriverImage.SaveAs(Path.Combine(Server.MapPath(virtualFilePath)));
                            hiredVehicleDriver.DriverImageLocation = virtualFilePath;
                            model.MHT_DHT_DriverImageLocation = virtualFilePath;
                        }
                        else
                        {
                            model.MHT_DHT_DriverImageLocation = null;
                        }

                        if (MHT_DHT_DriverLicenseImage != null)
                        {
                            hiredVehicleDriver.DriverLiceneseNumber = db_model.MHT_DHT_DriverLiceneseNumber;
                            string virtualFilePath = virtualFolderPath + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace(":", "-") + " LicenseNumber " + hiredVehicleDriver.PK_HiredVehicleDriver + "." + MHT_DHT_DriverImage.FileName.Split('.').Last();
                            MHT_DHT_DriverLicenseImage.SaveAs(Path.Combine(Server.MapPath(virtualFilePath)));
                            hiredVehicleDriver.DriverLicenseImageLocation = virtualFilePath;
                            model.MHT_DHT_DriverLicenseImageLocation = virtualFilePath;
                        }
                        else
                        {
                            model.MHT_DHT_DriverLicenseImageLocation = null;
                        }

                        bll.db.HiredVehicleDrivers.Add(hiredVehicleDriver);
                    }
                    else if (model.MHT_DHT_IsNewDriver == false)
                    {
                        var hiredVehicleDriver = bll.db.HiredVehicleDrivers.Where(m => m.FK_Vehicle == model.PK_Vehicle).OrderByDescending(m => m.CreatedAt).FirstOrDefault();
                        hiredVehicleDriver.UpdatedAt = DateTime.Now;
                        hiredVehicleDriver.FK_UpdatedByUser = CurrentUser.PK_User;

                        hiredVehicleDriver.DriverName = string.IsNullOrEmpty(model.MHT_DHT_DriverName) ? null : model.MHT_DHT_DriverName.Trim().ToUpper();
                        hiredVehicleDriver.DriverLiceneseNumber = string.IsNullOrEmpty(model.MHT_DHT_DriverLiceneseNumber) ? null : model.MHT_DHT_DriverLiceneseNumber.Trim().ToUpper();
                        hiredVehicleDriver.DriverLicenseType = model.MHT_DHT_DriverLicenseType;
                        hiredVehicleDriver.DriverContactNumber = model.MHT_DHT_DriverContactNumber;
                        hiredVehicleDriver.DriverFatherName = string.IsNullOrEmpty(model.MHT_DHT_DriverFatherName) ? null : model.MHT_DHT_DriverFatherName.Trim().ToUpper();
                        hiredVehicleDriver.DriverAddressVillage = string.IsNullOrEmpty(model.MHT_DHT_DriverAddressVillage) ? null : model.MHT_DHT_DriverAddressVillage.Trim().ToUpper();
                        hiredVehicleDriver.DriverAddressPostOfiice = string.IsNullOrEmpty(model.MHT_DHT_DriverAddressPostOfiice) ? null : model.MHT_DHT_DriverAddressPostOfiice.Trim().ToUpper();
                        hiredVehicleDriver.DriverAddressThana = string.IsNullOrEmpty(model.MHT_DHT_DriverAddressThana) ? null : model.MHT_DHT_DriverAddressThana.Trim().ToUpper();
                        hiredVehicleDriver.DriverAddressDistrict = string.IsNullOrEmpty(model.MHT_DHT_DriverAddressDistrict) ? null : model.MHT_DHT_DriverAddressDistrict.Trim().ToUpper();
                        hiredVehicleDriver.DriverNID = string.IsNullOrEmpty(model.MHT_DHT_DriverNID) ? null : model.MHT_DHT_DriverNID.Trim().ToUpper();
                        hiredVehicleDriver.DriverSalary = model.MHT_DHT_DriverSalary;

                        string virtualFolderPath = CommonClass.ImageDirectory + "Vehicles/" + db_model.PK_Vehicle + "/HiredVehicleDriver/";
                        //# create folder
                        string physicalFolderPath = Path.Combine(Server.MapPath(virtualFolderPath));
                        if (!Directory.Exists(physicalFolderPath))
                        {
                            Directory.CreateDirectory(physicalFolderPath);
                        }

                        if (MHT_DHT_DriverImage != null)
                        {
                            string virtualFilePath = virtualFolderPath + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace(":", "-") + " DriverImage " + hiredVehicleDriver.PK_HiredVehicleDriver + "." + MHT_DHT_DriverImage.FileName.Split('.').Last();
                            MHT_DHT_DriverImage.SaveAs(Path.Combine(Server.MapPath(virtualFilePath)));
                            hiredVehicleDriver.DriverImageLocation = virtualFilePath;
                            model.MHT_DHT_DriverImageLocation = virtualFilePath;
                        }
                        else
                        {
                            //model.MHT_DHT_DriverImageLocation = null;
                        }

                        if (MHT_DHT_DriverLicenseImage != null)
                        {
                            hiredVehicleDriver.DriverLiceneseNumber = db_model.MHT_DHT_DriverLiceneseNumber;
                            string virtualFilePath = virtualFolderPath + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace(":", "-") + " LicenseNumber " + hiredVehicleDriver.PK_HiredVehicleDriver + "." + MHT_DHT_DriverImage.FileName.Split('.').Last();
                            MHT_DHT_DriverLicenseImage.SaveAs(Path.Combine(Server.MapPath(virtualFilePath)));
                            hiredVehicleDriver.DriverLicenseImageLocation = virtualFilePath;
                            model.MHT_DHT_DriverLicenseImageLocation = virtualFilePath;
                        }
                        else
                        {
                            //model.MHT_DHT_DriverLicenseImageLocation = null;
                        }
                    }

                    bll.db.SaveChanges();
                    CreateAlertMessage(AlertMessageType.Success, "Success", "Vehicle is successfully edited.");
                    return RedirectToAction("Index");
                }
                catch (Exception exception)
                {
                    CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                }
            }
            else
            {
                string validators = "";
                if (modelValidator != ValidationStatus.OK)
                {
                    validators = validators + modelValidator;
                }
                CreateAlertMessage(AlertMessageType.Danger, "Validation Failure", validators);
            }
            ViewBag.model = model;
            ViewBag.UserCompanies = new SelectList(bll.db.Companies.Where(m => m.IsDeleted == false && m.GroupOfCompany.IsPranRFLGroup == true && m.IsUserCompany == true).OrderBy(m => m.Name), "PK_Company", "Name", model.FK_Company);
            var _invalidDepoPK = Guid.Parse("00000000-0000-0000-0000-000000000000");
            var accessibleDepoes = bll.db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            ViewBag.Depoes = new SelectList(bll.db.Depoes.Where(m => m.IsDeleted == false && accessibleDepoes.Contains(m.PK_Depo) && m.PK_Depo != _invalidDepoPK).OrderBy(m => m.Name), "PK_Depo", "Name", model.FK_Depo);

            if (model.FK_VehicleModel != null)
            {
                var vehicleModel = bll.db.VehicleModels.Find(model.FK_VehicleModel);
                ViewBag.VehicleBrands = new SelectList(bll.db.VehicleBrands.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Name), "PK_VehicleBrand", "Name", vehicleModel.FK_VehicleBrand);
                ViewBag.VehicleModels = new SelectList(bll.db.VehicleModels.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Title), "PK_VehicleModel", "Title", model.FK_VehicleModel);
            }
            else
            {
                ViewBag.VehicleBrands = new SelectList(bll.db.VehicleBrands.Where(vbm => vbm.IsActive == true).OrderBy(m => m.Name), "PK_VehicleBrand", "Name");
                ViewBag.VehicleModels = new SelectList(bll.db.VehicleModels.Where(vbm => vbm.PK_VehicleModel == null).OrderBy(m => m.Title), "PK_VehicleModel", "Title");
            }
            ViewBag.VehicleTypesDict = new SelectList(VehicleTypesDict.OrderBy(m => m.Value), "Key", "Value", model.VehicleType);
            ViewBag.FuelTypesDict = new SelectList(FuelTypesDict, "Key", "Value", model.FuelType);
            ViewBag.CapacityTonDict = new SelectList(CapacityTonDict, "Key", "Value", model.CapacityTon);
            ViewBag.NumberPlate_StatusesDict = new SelectList(NumberPlate_StatusesDict, "Key", "Value", model.NumberPlate_IsDigital);
            ViewBag.PurchaseCompanies = new SelectList(bll.db.Companies.Where(m => m.IsDeleted == false && m.IsPruchaseCompany == true).OrderBy(m => m.Name), "PK_Company", "Name", model.Internal_FK_PurchasingCompany);
            ViewBag.FinancingCompanies = new SelectList(bll.db.FinancingCompanies.Where(m => m.IsDeleted == false).OrderBy(m => m.Name), "PK_FinancingCompany", "Name", model.Internal_FK_FinancingCompany);
            ViewBag.MHT_DHT_DriverLicenseTypeDict = new SelectList(MHT_DHT_DriverLicenseTypeDict, "Key", "Value", model.MHT_DHT_DriverLicenseType);
            ViewBag.MHT_DHT_VehicleSizeDict = new SelectList(MHT_DHT_VehicleSizeDict, "Key", "Value", model.MHT_DHT_VehicleSize);

            return View(model);
        }

        public ActionResult View_MHT(Guid id)
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
                var model = bll.db.Vehicles.Find(id);
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
        #endregion 


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
                var model = bll.db.Vehicles.Find(id);
                if (model != null)
                {
                    try
                    {
                        model.IsDeleted = true;

                        model.DeletedAt = DateTime.Now;
                        model.FK_DeletedByUser = CommonClass.GetCurrentUser().PK_User;

                        bll.db.SaveChanges();
                        CreateAlertMessage(AlertMessageType.Success, "Success", "Vehicle is successfully deleted.");
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
        public JsonResult IsPranRFLGroup_ByCompany_PK(Guid PK_Company)
        {
            var _IsPranRFLGroup = (from com in bll.db.Companies
                                   where com.PK_Company == PK_Company
                                   join gop in bll.db.GroupOfCompanies on com.FK_GroupOfCompany equals gop.PK_GroupOfCompany
                                   select gop.IsPranRFLGroup).FirstOrDefault();
            var result = _IsPranRFLGroup == true ? true : false;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetVehicleModelsBy_PK_VehicleBrand(Guid PK_VehicleBrand)
        {
            var list = bll.db.VehicleModels.Where(m => m.IsActive == true && m.FK_VehicleBrand == PK_VehicleBrand).Select(m => new
            {
                m.PK_VehicleModel,
                m.Title
            }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetGPS_DeviceExisting(string searchText)
        {
            var list = bll.db.GPS_DeviceExisting.Where(m => m.GpsIMEINumber.EndsWith(searchText)).Select(m => new
            {
                id = m.GpsIMEINumber,
                text = m.GpsIMEINumber + " : " + m.GpsDeviceModel
            }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllVehicleByRegistrationNumber(string OWN_MHT_DHT, string PRG_Type, string VehicleRegNumSearch)
        {
            var query = bll.db.Vehicles.Where(m => m.IsDeleted == false);
            if (!string.IsNullOrEmpty(OWN_MHT_DHT) && OWN_MHT_DHT != "all")
            {
                query = query.Where(m => m.OWN_MHT_DHT == OWN_MHT_DHT);
            }

            if ((OWN_MHT_DHT == "OWN" || OWN_MHT_DHT == "MHT") && !string.IsNullOrEmpty(PRG_Type) && PRG_Type != "all")
            {
                query = query.Where(m => m.Depo.PRG_Type == PRG_Type);
            }

            var list = query.Where(m => m.RegistrationNumber.Contains(VehicleRegNumSearch)).Select(m => new
            {
                m.PK_Vehicle,
                m.RegistrationNumber
            }).OrderBy(m => m.RegistrationNumber).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllVehicleViewModels_Json()
        {
            var list = bll.db.Vehicles.AsEnumerable().Where(c => c.IsDeleted == false).Select(
                m => new
                {
                    RegistrationNumber = m.RegistrationNumber,
                    CompanyName = m.Company.Name,
                    DepoName = m.Depo.Name,
                    VehicleType = m.VehicleType == null ? "" : m.VehicleType
                }
                ).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetFinancingCompaniesBy_FK_GroupOfCompany(Guid? FK_GroupOfCompany)
        {
            var list = bll.db.Companies.AsEnumerable().Where(c => c.IsDeleted == false && c.FK_GroupOfCompany == FK_GroupOfCompany).Select(
                m => new
                {
                    m.PK_Company,
                    m.Name
                }
                ).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDetail(Guid id)
        {
            var model = bll.db.Vehicles.Find(id);
            if (model != null)
            {
                return Json(new
                {
                    model.Internal_FK_AdvertisingCompany,
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("NOT FOUND", JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult Q(string q)
        {
            var list = bll.db.Vehicles.Where(m => m.RegistrationNumber.Contains(q)).Select(m => new
            {
                m.RegistrationNumber,
                m.OWN_MHT_DHT,
                DepoName = m.Depo != null ? m.Depo.Name : "",
                m.Internal_VehicleContactNumber,
                HasTracking = m.VehicleTrackingInformation.VehicleTracking != null ? "Yes" : "No",
                Link = m.VehicleTrackingInformation.VehicleTracking != null ? "maps.google.com/maps?f=q&hl=en&q=" + m.VehicleTrackingInformation.VehicleTracking.Latitude + "," + m.VehicleTrackingInformation.VehicleTracking.Longitude + "&ie=UTF8&z=16&iwloc=addr&om=1" : ""
            }).OrderBy(m => m.RegistrationNumber).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        //# Sales Software Vehicle Transfer
        public JsonResult GetNewVehiclesInfo(string requestTime)
        {
            var list = bll.db.Vehicles.Where(m => m.IsAddedToSaleSoft != true).OrderBy(m => m.RowSerial).Take(100);
            foreach (var item in list)
            {
                item.SaleSoftRequestTime = requestTime;
            }
            bll.db.SaveChanges();
            var _list = list.Select(m => new
            {
                Id = m.RowSerial,
                m.RegistrationNumber,
                m.VehicleType,
                OWN_MHT_DHT = m.OWN_MHT_DHT == "DHT" ? "LHT" : m.OWN_MHT_DHT,
                MobileNumber = m.Internal_VehicleContactNumber != null ? m.Internal_VehicleContactNumber : "" + m.MHT_DHT_DriverContactNumber != null ? m.MHT_DHT_DriverContactNumber : ""
            });
            return Json(_list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ConfrimVehicleAdded(string requestTime)
        {
            var list = bll.db.Vehicles.Where(m => m.IsAddedToSaleSoft != true && m.SaleSoftRequestTime == requestTime).OrderBy(m => m.RowSerial).ToList();
            foreach (var item in list)
            {
                item.IsAddedToSaleSoft = true;
            }
            bll.db.SaveChanges();
            return Json(new { VehicleConfirmed = list.Count }, JsonRequestBehavior.AllowGet);
        }

        //# API
        /// <summary>
        /// API Created from Sujan vai to check vehicle entered on PIP
        /// </summary>
        /// <param name="PK_Vehicle"></param>
        /// <returns></returns>
        public JsonResult GetVehicleLastGateIn(Guid PK_Vehicle)
        {
            var vehicle = bll.db.Vehicles.Where(m => m.PK_Vehicle == PK_Vehicle).FirstOrDefault();
            if (vehicle != null)
            {
                return Json(new
                {
                    RegistrationNumber = vehicle.RegistrationNumber,
                    InOrOut = vehicle.LocationInOrOut == true ? "In" : "Out",
                    InOutTime = vehicle.LocationInOutTime.ToString(),
                    InLoacationName = vehicle.FK_LocationInOut != null ? bll.db.Locations.Where(m => m.PK_Location == vehicle.FK_LocationInOut).FirstOrDefault().Name : ""
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    RegistrationNumber = "",
                    InOrOut = "",
                    InOutTime = "",
                    InLoacationName = ""
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
