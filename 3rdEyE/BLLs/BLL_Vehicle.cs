using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _3rdEyE.Models;
using _3rdEyE.ManagingTools;
using _3rdEyE.ViewModels;


namespace _3rdEyE.BLL
{
    public class BLL_Vehicle : BaseBLL
    {
        public List<VM_Vehicle> GetAllViewModels()
        {
            var list = db.Vehicles.AsEnumerable().Where(c => c.IsDeleted == false).Select(c => ConvertToViewModel(c)).ToList();
            return list;
        }
        public List<VM_Vehicle> GetViewModelsBy_Depo(Guid? id)
        {
            var list = db.Vehicles.AsEnumerable().Where(c => c.IsDeleted == false && c.FK_Depo == id).Select(c => ConvertToViewModel(c)).ToList();
            return list;
        }
        public List<VM_Vehicle> GetViewModelsBy_AccessibleDepoesofCurrentUser()
        {
            var accessibleDepoes = db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            var list = db.Vehicles.AsEnumerable().Where(c => c.IsDeleted == false).Where(m => accessibleDepoes.Contains(m.FK_Depo)).Select(c => ConvertToViewModel(c)).ToList();
            return list;
        }


        #region //OWN
        public string IsValidModel_ToCreate_OWN(Vehicle model)
        {
            string result = "";
            if (db.Vehicles.Where(c => c.RegistrationNumber.ToUpper() == model.RegistrationNumber.Trim()).Any())
            {
                result += "This registration number is already used by another vehicle. Please, use an unique registration number. ";
            }
            if (!string.IsNullOrEmpty(model.EngineNumber) && db.Vehicles.Where(c => c.EngineNumber.ToUpper() == model.EngineNumber.Trim()).Any())
            {
                result += "This engine number is already used by another vehicle. Please, use an unique engine number. ";
            }
            if (!string.IsNullOrEmpty(model.ChassisNumber) && db.Vehicles.Where(c => c.ChassisNumber.ToUpper() == model.ChassisNumber.Trim()).Any())
            {
                result += "This chassis number is already used by another vehicle. Please, use an unique chassis number. ";
            }
            if (!string.IsNullOrEmpty(model.GpsIMEINumber) && db.Vehicles.Where(c => c.GpsIMEINumber.ToUpper() == model.GpsIMEINumber.Trim()).Any())
            {
                result += "This GPS IMEI number is already used by another vehicle. Please, use an unique GPS IMEI number. ";
            }
            if (!string.IsNullOrEmpty(model.GpsMobileNumber) && db.Vehicles.Where(c => c.GpsMobileNumber.ToUpper() == model.GpsIMEINumber.Trim()).Any())
            {
                result += "This GPS device Mobile No is already used by another vehicle. Please, use an unique GPS device mobile Number. ";
            }

            if (!string.IsNullOrEmpty(model.Internal_VehicleContactNumber))
            {
                bool isValid;
                string formatError;
                var outMobileNumber = InputValidatorAndFormatter.FormateMobileNumber_BD(model.Internal_VehicleContactNumber, out isValid, out formatError);
                if (isValid == false)
                {
                    result += "Vehicle Contact Number " + formatError;
                }
            }

            if (!string.IsNullOrEmpty(model.GpsMobileNumber))
            {
                bool isValid;
                string formatError;
                var outMobileNumber = InputValidatorAndFormatter.FormateMobileNumber_BD(model.GpsMobileNumber, out isValid, out formatError);
                if (isValid == false)
                {
                    result += "GPS Mobile Number number " + formatError;
                }
            }

            if (result == "")
            {
                result = ValidationStatus.OK;
            }
            return result;
        }
        public string IsValidModel_ToEdit_OWN(Vehicle model)
        {
            string result = "";

            //# checks, registraton number is unique
            if (db.Vehicles.Where(c => c.PK_Vehicle != model.PK_Vehicle && c.RegistrationNumber.ToUpper() == model.RegistrationNumber.Trim()).Any())
            {
                result += "This registration number is already used by another vehicle. Please, use an unique registration number. ";
            }
            if (!string.IsNullOrEmpty(model.EngineNumber) && db.Vehicles.Where(c => c.PK_Vehicle != model.PK_Vehicle && c.EngineNumber.ToUpper() == model.EngineNumber.Trim()).Any())
            {
                result += "This engine number is already used by another vehicle. Please, use an unique engine number. ";
            }
            if (!string.IsNullOrEmpty(model.ChassisNumber) && db.Vehicles.Where(c => c.PK_Vehicle != model.PK_Vehicle && c.ChassisNumber.ToUpper() == model.ChassisNumber.Trim()).Any())
            {
                result += "This chassis number is already used by another vehicle. Please, use an unique chassis number. ";
            }
            if (!string.IsNullOrEmpty(model.GpsIMEINumber) && db.Vehicles.Where(c => c.PK_Vehicle != model.PK_Vehicle && c.GpsIMEINumber.ToUpper() == model.GpsIMEINumber.Trim()).Any())
            {
                result += "This GPS IMEI number is already used by another vehicle. Please, use an unique GPS IMEI number. ";
            }
            if (!string.IsNullOrEmpty(model.GpsMobileNumber) && db.Vehicles.Where(c => c.PK_Vehicle != model.PK_Vehicle && c.GpsMobileNumber.ToUpper() == model.GpsMobileNumber.Trim()).Any())
            {
                result += "This GPS device Mobile No is already used by another vehicle. Please, use an unique GPS device mobile Number. ";
            }

            if (!string.IsNullOrEmpty(model.Internal_VehicleContactNumber))
            {
                bool isValid;
                string formatError;
                var outMobileNumber = InputValidatorAndFormatter.FormateMobileNumber_BD(model.Internal_VehicleContactNumber, out isValid, out formatError);
                if (isValid == false)
                {
                    result += "Vehicle Contact Number " + formatError;
                }
            }

            if (!string.IsNullOrEmpty(model.GpsMobileNumber))
            {
                bool isValid;
                string formatError;
                var outMobileNumber = InputValidatorAndFormatter.FormateMobileNumber_BD(model.GpsMobileNumber, out isValid, out formatError);
                if (isValid == false)
                {
                    result += "GPS IMEI number " + formatError;
                }
            }

            if (result == "")
            {
                result = ValidationStatus.OK;
            }
            return result;
        }
        public Vehicle FilterToDBModel_OWN(Vehicle model)
        {
            Vehicle db_model = FilterToDBModel_Common(model);
            bool isValid;
            string formatError;
            db_model.OWN_MHT_DHT = "OWN";
            if (!string.IsNullOrEmpty(model.Internal_VehicleContactNumber))
            {
                var outMobileNumber = InputValidatorAndFormatter.FormateMobileNumber_BD(model.Internal_VehicleContactNumber, out isValid, out formatError);
                if (isValid == true)
                {
                    db_model.Internal_VehicleContactNumber = outMobileNumber;
                }
            }
            if (!string.IsNullOrEmpty(model.GpsMobileNumber))
            {
                var outMobileNumber = InputValidatorAndFormatter.FormateMobileNumber_BD(model.GpsMobileNumber, out isValid, out formatError);
                if (isValid == true)
                {
                    db_model.GpsMobileNumber = outMobileNumber;
                }
            }

            db_model.FK_DepoGroup = model.FK_DepoGroup;

            return db_model;
        }
        #endregion

        #region //MHT
        public string IsValidModel_ToCreate_MHT(Vehicle model)
        {
            string result = "";
            if (db.Vehicles.Where(c => c.RegistrationNumber.ToUpper() == model.RegistrationNumber.Trim()).Any())
            {
                result += "This registration number is already used by another vehicle. Please, use an unique registration number. ";
            }
            if (!string.IsNullOrEmpty(model.EngineNumber) && db.Vehicles.Where(c => c.EngineNumber.ToUpper() == model.EngineNumber.Trim()).Any())
            {
                result += "This engine number is already used by another vehicle. Please, use an unique engine number. ";
            }
            if (!string.IsNullOrEmpty(model.ChassisNumber) && db.Vehicles.Where(c => c.ChassisNumber.ToUpper() == model.ChassisNumber.Trim()).Any())
            {
                result += "This chassis number is already used by another vehicle. Please, use an unique chassis number. ";
            }

            if (result == "")
            {
                result = ValidationStatus.OK;
            }
            return result;
        }
        public string IsValidModel_ToEdit_MHT(Vehicle model)
        {
            string result = "";

            //# checks, registraton number is unique
            if (db.Vehicles.Where(c => c.PK_Vehicle != model.PK_Vehicle && c.RegistrationNumber.ToUpper() == model.RegistrationNumber.Trim()).Any())
            {
                result += "This registration number is already used by another vehicle. Please, use an unique registration number. ";
            }
            if (!string.IsNullOrEmpty(model.EngineNumber) && db.Vehicles.Where(c => c.PK_Vehicle != model.PK_Vehicle && c.EngineNumber.ToUpper() == model.EngineNumber.Trim()).Any())
            {
                result += "This engine number is already used by another vehicle. Please, use an unique engine number. ";
            }
            if (!string.IsNullOrEmpty(model.ChassisNumber) && db.Vehicles.Where(c => c.PK_Vehicle != model.PK_Vehicle && c.ChassisNumber.ToUpper() == model.ChassisNumber.Trim()).Any())
            {
                result += "This chassis number is already used by another vehicle. Please, use an unique chassis number. ";
            }

            if (result == "")
            {
                result = ValidationStatus.OK;
            }
            return result;
        }
        public Vehicle FilterToDBModel_MHT(Vehicle model)
        {
            Vehicle db_model = FilterToDBModel_Common(model);
            db_model.OWN_MHT_DHT = "MHT";

            return db_model;
        }
        #endregion

        #region //DHT
        public string IsValidModel_ToCreate_DHT(Vehicle model)
        {
            string result = "";
            if (db.Vehicles.Where(c => c.RegistrationNumber.ToUpper() == model.RegistrationNumber.Trim()).Any())
            {
                result += "This registration number is already used by another vehicle. Please, use an unique registration number. ";
            }
            if (!string.IsNullOrEmpty(model.EngineNumber) && db.Vehicles.Where(c => c.EngineNumber.ToUpper() == model.EngineNumber.Trim()).Any())
            {
                result += "This engine number is already used by another vehicle. Please, use an unique engine number. ";
            }
            if (!string.IsNullOrEmpty(model.ChassisNumber) && db.Vehicles.Where(c => c.ChassisNumber.ToUpper() == model.ChassisNumber.Trim()).Any())
            {
                result += "This chassis number is already used by another vehicle. Please, use an unique chassis number. ";
            }

            if (result == "")
            {
                result = ValidationStatus.OK;
            }
            return result;
        }
        public string IsValidModel_ToEdit_DHT(Vehicle model)
        {
            string result = "";

            //# checks, registraton number is unique
            if (db.Vehicles.Where(c => c.PK_Vehicle != model.PK_Vehicle && c.RegistrationNumber.ToUpper() == model.RegistrationNumber.Trim()).Any())
            {
                result += "This registration number is already used by another vehicle. Please, use an unique registration number. ";
            }
            if (!string.IsNullOrEmpty(model.EngineNumber) && db.Vehicles.Where(c => c.PK_Vehicle != model.PK_Vehicle && c.EngineNumber.ToUpper() == model.EngineNumber.Trim()).Any())
            {
                result += "This engine number is already used by another vehicle. Please, use an unique engine number. ";
            }
            if (!string.IsNullOrEmpty(model.ChassisNumber) && db.Vehicles.Where(c => c.PK_Vehicle != model.PK_Vehicle && c.ChassisNumber.ToUpper() == model.ChassisNumber.Trim()).Any())
            {
                result += "This chassis number is already used by another vehicle. Please, use an unique chassis number. ";
            }

            if (result == "")
            {
                result = ValidationStatus.OK;
            }
            return result;
        }
        public Vehicle FilterToDBModel_DHT(Vehicle model)
        {
            Vehicle db_model = FilterToDBModel_Common(model);
            db_model.OWN_MHT_DHT = "DHT";

            return db_model;
        }
        #endregion

        public Vehicle FilterToDBModel_Common(Vehicle model)
        {
            Vehicle db_model;
            if (model.PK_Vehicle.ToString() == "00000000-0000-0000-0000-000000000000")
            {
                db_model = new Vehicle();
                db_model.PK_Vehicle = Guid.NewGuid();
                db_model.IsDeleted = false;

                db_model.CreatedAt = DateTime.Now;
                db_model.FK_CreatedByUser = CommonClass.GetCurrentUser().PK_User;
            }
            else
            {
                db_model = db.Vehicles.Find(model.PK_Vehicle);
                db_model.UpdatedAt = DateTime.Now;
                db_model.FK_UpdatedByUser = CommonClass.GetCurrentUser().PK_User;
            }
            db_model.ERP_Id = model.ERP_Id;
            db_model.RegistrationNumber = model.RegistrationNumber;
            db_model.PhysicalDocumnetLocation = model.PhysicalDocumnetLocation;
            db_model.FK_Company = model.FK_Company;
            db_model.FK_Depo = model.FK_Depo;
            db_model.FK_VehicleModel = model.FK_VehicleModel;
            db_model.RegisrationDate = model.RegisrationDate;
            db_model.ChassisNumber = string.IsNullOrEmpty(model.ChassisNumber) ? null : model.ChassisNumber.ToUpper();
            db_model.EngineNumber = string.IsNullOrEmpty(model.EngineNumber) ? null : model.EngineNumber.ToUpper();
            db_model.EngineCC = model.EngineCC;
            db_model.VehicleType = model.VehicleType;
            db_model.GpsMobileNumber = model.GpsMobileNumber;
            //db_model.GpsIMEINumber = string.IsNullOrEmpty(model.GpsIMEINumber) ? null : model.GpsIMEINumber.ToUpper();
            //db_model.GpsDeviceModel = model.GpsDeviceModel;
            db_model.CapacityTon = model.CapacityTon;
            db_model.WeightKG = model.WeightKG;
            db_model.CapacitySeat = model.CapacitySeat;
            db_model.NumberPlate_IsDigital = model.NumberPlate_IsDigital;
            db_model.FuelType = model.FuelType;

            db_model.Internal_VehiclePrice = model.Internal_VehiclePrice;
            db_model.Internal_PurchaseDate = model.Internal_PurchaseDate;
            db_model.Internal_FK_PurchasingCompany = model.Internal_FK_PurchasingCompany;
            db_model.Internal_ShowTemperature = model.Internal_ShowTemperature;

            db_model.Internal_VehicleContactNumber = (!string.IsNullOrEmpty(model.Internal_VehicleContactNumber)) ? model.Internal_VehicleContactNumber.Replace(" ", "").Replace("+", "").Replace("-", "") : null;
            db_model.Internal_VehicleContactNumber = (!string.IsNullOrEmpty(db_model.Internal_VehicleContactNumber) && !db_model.Internal_VehicleContactNumber.StartsWith("0")) ? "0" + db_model.Internal_VehicleContactNumber : db_model.Internal_VehicleContactNumber;

            //db_model.Internal_FinancingAgrementNumber = model.Internal_FinancingAgrementNumber;
            //db_model.Internal_FinancingAgrementNumberOfVehicle = model.Internal_FinancingAgrementNumberOfVehicle;
            //db_model.Internal_FinancingAgrementMaturityDate = model.Internal_FinancingAgrementMaturityDate;
            //db_model.Internal_FinancingAgrementGRN_MRR = model.Internal_FinancingAgrementGRN_MRR;
            //db_model.Internal_FinancingAgrementGRN_MRR_Date = model.Internal_FinancingAgrementGRN_MRR_Date;
            //db_model.Internal_FinancingAgrementIsMatured = model.Internal_FinancingAgrementIsMatured;
            db_model.Internal_FK_FinancingCompany = model.Internal_FK_FinancingCompany;

            db_model.MHT_DHT_VehicleSize = model.MHT_DHT_VehicleSize;
            db_model.MHT_DHT_DriverName = string.IsNullOrEmpty(model.MHT_DHT_DriverName) ? null : model.MHT_DHT_DriverName.Trim().ToUpper();
            db_model.MHT_DHT_DriverLiceneseNumber = string.IsNullOrEmpty(model.MHT_DHT_DriverLiceneseNumber) ? null : model.MHT_DHT_DriverLiceneseNumber.Trim().ToUpper();
            db_model.MHT_DHT_DriverLicenseType = model.MHT_DHT_DriverLicenseType;

            db_model.MHT_DHT_DriverContactNumber = (!string.IsNullOrEmpty(model.MHT_DHT_DriverContactNumber)) ? model.MHT_DHT_DriverContactNumber.Replace(" ", "").Replace("+", "").Replace("-", "") : null;
            db_model.MHT_DHT_DriverContactNumber = (!string.IsNullOrEmpty(db_model.MHT_DHT_DriverContactNumber) && !db_model.MHT_DHT_DriverContactNumber.StartsWith("0")) ? "0" + db_model.MHT_DHT_DriverContactNumber : db_model.MHT_DHT_DriverContactNumber;


            db_model.MHT_DHT_DriverFatherName = string.IsNullOrEmpty(model.MHT_DHT_DriverFatherName) ? null : model.MHT_DHT_DriverFatherName.Trim().ToUpper();
            db_model.MHT_DHT_DriverAddressVillage = string.IsNullOrEmpty(model.MHT_DHT_DriverAddressVillage) ? null : model.MHT_DHT_DriverAddressVillage.Trim().ToUpper();
            db_model.MHT_DHT_DriverAddressPostOfiice = string.IsNullOrEmpty(model.MHT_DHT_DriverAddressPostOfiice) ? null : model.MHT_DHT_DriverAddressPostOfiice.Trim().ToUpper();
            db_model.MHT_DHT_DriverAddressThana = string.IsNullOrEmpty(model.MHT_DHT_DriverAddressThana) ? null : model.MHT_DHT_DriverAddressThana.Trim().ToUpper();
            db_model.MHT_DHT_DriverAddressDistrict = string.IsNullOrEmpty(model.MHT_DHT_DriverAddressDistrict) ? null : model.MHT_DHT_DriverAddressDistrict.Trim().ToUpper();
            db_model.MHT_DHT_DriverNID = string.IsNullOrEmpty(model.MHT_DHT_DriverNID) ? null : model.MHT_DHT_DriverNID.Trim().ToUpper();
            db_model.MHT_DHT_DriverSalary = model.MHT_DHT_DriverSalary;

            db_model.MHT_DHT_OwnerName = string.IsNullOrEmpty(model.MHT_DHT_OwnerName) ? null : model.MHT_DHT_OwnerName.Trim().ToUpper();
            db_model.MHT_DHT_OwnerFatherName = string.IsNullOrEmpty(model.MHT_DHT_OwnerFatherName) ? null : model.MHT_DHT_OwnerFatherName.Trim().ToUpper();
            db_model.MHT_DHT_OwnerAddressVillage = string.IsNullOrEmpty(model.MHT_DHT_OwnerAddressVillage) ? null : model.MHT_DHT_OwnerAddressVillage.Trim().ToUpper();
            db_model.MHT_DHT_OwnerAddressPostOffice = string.IsNullOrEmpty(model.MHT_DHT_OwnerAddressPostOffice) ? null : model.MHT_DHT_OwnerAddressPostOffice.Trim().ToUpper();
            db_model.MHT_DHT_OwnerAddressThana = string.IsNullOrEmpty(model.MHT_DHT_OwnerAddressThana) ? null : model.MHT_DHT_OwnerAddressThana.Trim().ToUpper();
            db_model.MHT_DHT_OwnerAddressDistrict = string.IsNullOrEmpty(model.MHT_DHT_OwnerAddressDistrict) ? null : model.MHT_DHT_OwnerAddressDistrict.Trim().ToUpper();
            db_model.MHT_DHT_OwnerContactNumber = (!string.IsNullOrEmpty(model.MHT_DHT_OwnerContactNumber)) ? model.MHT_DHT_OwnerContactNumber.Replace(" ", "").Replace("+", "").Replace("-", "") : null;
            db_model.MHT_DHT_OwnerContactNumber = (!string.IsNullOrEmpty(db_model.MHT_DHT_OwnerContactNumber) && !db_model.MHT_DHT_OwnerContactNumber.StartsWith("0")) ? "0" + db_model.MHT_DHT_OwnerContactNumber : db_model.MHT_DHT_OwnerContactNumber;
            db_model.MHT_DHT_OwnerNID = string.IsNullOrEmpty(model.MHT_DHT_OwnerNID) ? null : model.MHT_DHT_OwnerNID.Trim().ToUpper();

            db_model.MHT_AgreementFrom = model.MHT_AgreementFrom;
            db_model.MHT_AgreementTo = model.MHT_AgreementTo;
            db_model.MHT_MonthlyRentRate = model.MHT_MonthlyRentRate;

            return db_model;
        }

        public VM_Vehicle ConvertToViewModel(Vehicle model)
        {
            var viewModel = new VM_Vehicle();
            viewModel.Model = model;

            //# Only view property
            viewModel.IsDeleted_Text = model.IsDeleted == true ? "Deleted" : "Undeleted";
            viewModel.RegisrationDate_Text = model.RegisrationDate == null ? "" : CommonClass.ConvertToDateString(model.RegisrationDate);
            viewModel.Internal_PurchaseDate_Text = model.Internal_PurchaseDate == null ? "" : CommonClass.ConvertToDateString(model.Internal_PurchaseDate);
            viewModel.Internal_ShowTemperature_Text = model.Internal_ShowTemperature == true ? "Yes" : "No";
            viewModel.MHT_AgreementFrom_Text = model.MHT_AgreementFrom == null ? "" : CommonClass.ConvertToDateString(model.MHT_AgreementFrom);
            viewModel.MHT_AgreementTo_Text = model.MHT_AgreementTo == null ? "" : CommonClass.ConvertToDateString(model.MHT_AgreementTo);
            viewModel.Internal_FinancingAgrementMaturityDate_Text = model.Internal_FinancingAgrementMaturityDate == null ? "" : CommonClass.ConvertToDateString(model.Internal_FinancingAgrementMaturityDate);
            viewModel.Internal_FinancingAgrementGRN_MRR_Date_Text = model.Internal_FinancingAgrementGRN_MRR_Date == null ? "" : CommonClass.ConvertToDateString(model.Internal_FinancingAgrementGRN_MRR_Date);
            viewModel.NumberPlate_IsDigital_Text = model.NumberPlate_IsDigital == true ? "Digital" : "Analogue";
            return viewModel;
        }

        public string IsValidImage(HttpPostedFileBase Image)
        {
            string result = "";


            //# checks, Imgae is bigger than 300kb, 1mb=1048576
            if (Image.ContentLength > (314572 * 1))
            {
                result += "Image is too big. please, upload small image. ";

            }
            if (CommonClass.IsInvalidImageFormat(Image.ContentType))
            {
                result += "Given file is not an image. ";

            }

            if (result == "")
            {
                result = ValidationStatus.OK;
            }
            return result;
        }
    }
}