using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _3rdEyE.Models;
using _3rdEyE.ManagingTools;
using _3rdEyE.ViewModels;


namespace _3rdEyE.BLL
{
    public class BLL_Company : BaseBLL
    {
        //public List<Company> GetAllModels()
        //{
        //    var list = db.Companies.AsEnumerable().Select(c => c ).ToList();
        //    return list;
        //}
        //public List<Company> GetActiveModels()
        //{
        //    var list = db.Companies.AsEnumerable().Where(c => c.IsActive == true).Select(c => c).ToList();
        //    return list;
        //}

        //public List<VM_Company> GetAllViewModels()
        //{
        //    var list = db.Companies.AsEnumerable().Select(c => ConvertToViewModel(c)).ToList();
        //    return list;
        //}
        public List<VM_Company> GetAllViewModels()
        {
            var list = db.Companies.AsEnumerable().Where(c => c.IsDeleted == false ).Select(c => ConvertToViewModel(c)).ToList();
            return list;
        }

        public string IsValidModel_ToCreate(Company model)
        {
            string result = "";

            //# checks, name is unique
            if (db.Companies.Where(c => c.Name.ToUpper() == model.Name.ToUpper().Trim()).Any())
            {
                result += "This company name is already used by another compnay. Please, use an unique name. ";
                
            }

            if (result == "")
            {
                result = ValidationStatus.OK;
            }
            return result;
        }
        public string IsValidModel_ToEdit(Company model)
        {
            string result = "";

            //# checks, name is unique
            if (db.Companies.Where(c => c.PK_Company != model.PK_Company && c.Name.ToUpper() == model.Name.ToUpper().Trim()).Any())
            {
                result += "This company name is already used by another compnay. Please, use an unique name.";
                
            }

            if (result == "")
            {
                result = ValidationStatus.OK;
            }
            return result;
        }

        

        public Company FilterToDBModel(Company model)
        {
            var _IsPranRFLGroup = db.GroupOfCompanies.Where(gop=>gop.PK_GroupOfCompany==model.FK_GroupOfCompany).Select(gop=>gop.IsPranRFLGroup).FirstOrDefault();

            Company db_model;
            if (model.PK_Company.ToString() == "00000000-0000-0000-0000-000000000000")
            {
                db_model = new Company();
                db_model.PK_Company = Guid.NewGuid();
                db_model.IsDeleted = false;
            }
            else
            {
                db_model = db.Companies.Find(model.PK_Company);
            }
            
            db_model.FK_GroupOfCompany = model.FK_GroupOfCompany;
            db_model.Name = model.Name.Trim();
            //# checks string property is empty,if YES assign "", if NO then trim()
            db_model.CompanyRegistrationNumber = string.IsNullOrEmpty(model.CompanyRegistrationNumber) ? "" : model.CompanyRegistrationNumber.Trim();
            db_model.CompnayAddress = string.IsNullOrEmpty(model.CompnayAddress) ? "" : model.CompnayAddress.Trim();
            
            //# checks, if a company of RFL=>(set property "")/(if property given empty=>{set property to "" } / {trim spaces} )
            if (_IsPranRFLGroup == true)
            {
                db_model.ExternalOwnersFullName = null;
                db_model.ExternalOwnersNID = null;
                db_model.ExternalOwnersFathersName = null;
                db_model.ExternalOwnersContactNumber = null;
                db_model.ExternalOwnersAddress = null;
            }
            else
            {
                db_model.ExternalOwnersFullName = (string.IsNullOrEmpty(model.ExternalOwnersFullName) ? null : model.ExternalOwnersFullName.Trim());
                db_model.ExternalOwnersNID = (string.IsNullOrEmpty(model.ExternalOwnersNID) ? null : model.ExternalOwnersNID.Trim().ToUpper());
                db_model.ExternalOwnersFathersName = (string.IsNullOrEmpty(model.ExternalOwnersFathersName) ? null : model.ExternalOwnersFathersName.Trim());
                db_model.ExternalOwnersContactNumber = (string.IsNullOrEmpty(model.ExternalOwnersContactNumber) ? null : model.ExternalOwnersContactNumber.Trim().ToUpper());
                db_model.ExternalOwnersAddress = (string.IsNullOrEmpty(model.ExternalOwnersAddress) ? null : model.ExternalOwnersAddress.Trim().ToUpper());
            }

            return db_model;
        }

        public VM_Company ConvertToViewModel(Company model)
        {
            var viewModel = new VM_Company();
            //viewModel.RowSerial = model.RowSerial;
            //viewModel.PK_Company = model.PK_Company;
            //viewModel.IsDeleted = model.IsDeleted;
            //viewModel.IsActive = model.IsActive;
            //viewModel.FK_GroupOfCompany = model.FK_GroupOfCompany;
            //viewModel.Name = model.Name;
            //viewModel.CompanyRegistrationNumber = model.CompanyRegistrationNumber;
            //viewModel.CompnayAddress = model.CompnayAddress;
            //viewModel.ExternalOwnersFullName = model.GroupOfCompany.IsPranRFLGroup == true ? "N/A" : model.ExternalOwnersFullName;
            //viewModel.ExternalOwnersNID = model.GroupOfCompany.IsPranRFLGroup == true ? "N/A" : model.ExternalOwnersNID;
            //viewModel.ExternalOwnersFathersName = model.GroupOfCompany.IsPranRFLGroup == true ? "N/A" : model.ExternalOwnersFathersName;
            //viewModel.ExternalOwnersContactNumber = model.GroupOfCompany.IsPranRFLGroup == true ? "N/A" : model.ExternalOwnersContactNumber;
            //viewModel.ExternalOwnersAddress = model.GroupOfCompany.IsPranRFLGroup == true ? "N/A" : model.ExternalOwnersAddress;
            viewModel.Model = model;

            //# Relational property
            viewModel.GroupOfCompany = model.GroupOfCompany;

            //# Only view property
            viewModel.IsDeleted_Text = model.IsDeleted == true ? "Deleted" : "Undeleted";
            

            return viewModel;
        }
    }
}