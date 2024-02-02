using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _3rdEyE.Models;
using _3rdEyE.ManagingTools;
using _3rdEyE.ViewModels;


namespace _3rdEyE.BLL
{
    public class BLL_Helper : BaseBLL
    {
        //    //public List<Helper> GetAllModels()
        //    //{
        //    //    var list = db.Companies.AsEnumerable().Select(c => c ).ToList();
        //    //    return list;
        //    //}
        //    //public List<Helper> GetActiveModels()
        //    //{
        //    //    var list = db.Companies.AsEnumerable().Where(c => c.IsActive == true).Select(c => c).ToList();
        //    //    return list;
        //    //}

        //    //public List<VM_Helper> GetAllViewModels()
        //    //{
        //    //    var list = db.Companies.AsEnumerable().Select(c => ConvertToViewModel(c)).ToList();
        //    //    return list;
        //    //}
        public List<VM_Helper> GetAllViewModels()
        {
            var list = db.Helpers.AsEnumerable().Where(c => c.IsDeleted == false).Select(c => ConvertToViewModel(c)).ToList();
            return list;
        }

        public string IsValidModel_ToCreate(Helper model)
        {
            string result = "";
            if (db.Helpers.Where(c => c.UniqueIDNumber.ToUpper() == model.UniqueIDNumber.Trim()).Any())
            {
                result += "This unique ID number is already used by another helper. Please, use an unique id number. ";
                
            }

            if (result == "")
            {
                result = ValidationStatus.OK;
            }
            return result;
        }
        public string IsValidModel_ToEdit(Helper model)
        {
            string result = "";

            //# checks, registraton number is unique
            var existing = db.Helpers.Where(c => c.PK_Helper != model.PK_Helper && c.UniqueIDNumber.ToUpper() == model.UniqueIDNumber.Trim()).Select(v => v).ToList().Count();
            if (existing > 1)
            {
                result += "This unique ID number is already used by another helper. Please, use an unique id number. ";
                
            }

            if (result == "")
            {
                result = ValidationStatus.OK;
            }
            return result;
        }


        public Helper FilterToDBModel(Helper model)
        {
            Helper db_model;
            if (model.PK_Helper.ToString() == "00000000-0000-0000-0000-000000000000")
            {
                db_model = new Helper();
                db_model.PK_Helper = Guid.NewGuid();
                db_model.IsDeleted = false;
            }
            else
            {
                db_model = db.Helpers.Find(model.PK_Helper);
            }
            
            db_model.FK_Depo = model.FK_Depo;
            db_model.Name = model.Name.Trim().ToUpper();
            db_model.UniqueIDNumber = model.UniqueIDNumber.Trim().ToUpper();
            db_model.PhoneNumber = string.IsNullOrEmpty(model.PhoneNumber) ? "" : model.PhoneNumber.Trim();
            db_model.NID = string.IsNullOrEmpty(model.NID) ? "" : model.NID.Trim().ToUpper();
            return db_model;
        }

        public VM_Helper ConvertToViewModel(Helper model)
        {
            var viewModel = new VM_Helper();
            viewModel.Model = model;
            //# Relational property

            //# Only view property
            viewModel.IsDeleted_Text = model.IsDeleted == true ? "Deleted" : "Undeleted";
            
            //LisenceRenewalDate_Text
            return viewModel;
        }
    }
}