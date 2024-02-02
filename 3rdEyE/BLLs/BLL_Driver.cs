using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _3rdEyE.Models;
using _3rdEyE.ManagingTools;
using _3rdEyE.ViewModels;


namespace _3rdEyE.BLL
{
    public class BLL_Driver : BaseBLL
    {
        //    //public List<Driver> GetAllModels()
        //    //{
        //    //    var list = db.Companies.AsEnumerable().Select(c => c ).ToList();
        //    //    return list;
        //    //}
        //    //public List<Driver> GetActiveModels()
        //    //{
        //    //    var list = db.Companies.AsEnumerable().Where(c => c.IsActive == true).Select(c => c).ToList();
        //    //    return list;
        //    //}

        //    //public List<VM_Driver> GetAllViewModels()
        //    //{
        //    //    var list = db.Companies.AsEnumerable().Select(c => ConvertToViewModel(c)).ToList();
        //    //    return list;
        //    //}
        public List<VM_Driver> GetAllViewModels()
        {
            var list = db.Drivers.AsEnumerable().Where(c => c.IsDeleted == false).Select(c => ConvertToViewModel(c)).ToList();
            return list;
        }

        public string IsValidModel_ToCreate(Driver model)
        {
            string result = "";
            if (db.Drivers.Where(c => c.PhoneNumber.ToUpper() == model.PhoneNumber.Trim()).Any())
            {
                result += "This phone number is already used by another driver. Please, use another phone number. ";
            }

            if (result == "")
            {
                result = ValidationStatus.OK;
            }
            return result;
        }
        public string IsValidModel_ToEdit(Driver model)
        {
            string result = "";

            //# checks, registraton number is unique
            var existing = db.Drivers.Where(c => c.PK_Driver != model.PK_Driver && c.PhoneNumber.ToUpper() == model.PhoneNumber.Trim()).Select(v => v).ToList().Count();
            if (existing > 1)
            {
                result += "This phone number is already used by another driver. Please, use another phone number. ";
                
            }

            if (result == "")
            {
                result = ValidationStatus.OK;
            }
            return result;
        }


        public Driver FilterToDBModel(Driver model)
        {
            Driver db_model;
            if (model.PK_Driver.ToString() == "00000000-0000-0000-0000-000000000000")
            {
                db_model = new Driver();
                db_model.PK_Driver = Guid.NewGuid();
                db_model.IsDeleted = false;
            }
            else
            {
                db_model = db.Drivers.Find(model.PK_Driver);
            }
            
            db_model.FK_Depo = model.FK_Depo;
            db_model.Name = model.Name.Trim().ToUpper();
            db_model.UniqueIDNumber = string.IsNullOrEmpty(model.UniqueIDNumber) ? "" : model.UniqueIDNumber.Trim();
            db_model.PhoneNumber = string.IsNullOrEmpty(model.PhoneNumber) ? "" : model.PhoneNumber.Trim();
            db_model.NID = string.IsNullOrEmpty(model.NID) ? "" : model.NID.Trim().ToUpper();
            db_model.LisenceNumber = string.IsNullOrEmpty(model.LisenceNumber) ? "" : model.LisenceNumber.Trim().ToUpper();
            db_model.LisenceRenewalDate = model.LisenceRenewalDate;
            return db_model;
        }

        public VM_Driver ConvertToViewModel(Driver model)
        {
            var viewModel = new VM_Driver();
            viewModel.Model = model;
            //# Relational property

            //# Only view property
            viewModel.IsDeleted_Text = model.IsDeleted == true ? "Deleted" : "Undeleted";
            
            viewModel.LisenceRenewalDate_Text = model.LisenceRenewalDate == null ? "" : CommonClass.ConvertToDateString(model.LisenceRenewalDate);
            //LisenceRenewalDate_Text
            return viewModel;
        }
    }
}