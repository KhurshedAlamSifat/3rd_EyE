using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _3rdEyE.Models;
using _3rdEyE.ManagingTools;
using _3rdEyE.ViewModels;


namespace _3rdEyE.BLL
{
    public class BLL_PoliceCase : BaseBLL
    {
        //    //public List<PoliceCase> GetAllModels()
        //    //{
        //    //    var list = db.Companies.AsEnumerable().Select(c => c ).ToList();
        //    //    return list;
        //    //}
        //    //public List<PoliceCase> GetActiveModels()
        //    //{
        //    //    var list = db.Companies.AsEnumerable().Where(c => c.IsActive == true).Select(c => c).ToList();
        //    //    return list;
        //    //}

        //    //public List<VM_Case> GetAllViewModels()
        //    //{
        //    //    var list = db.Companies.AsEnumerable().Select(c => ConvertToViewModel(c)).ToList();
        //    //    return list;
        //    //}
        //public List<VM_PoliceCase> GetAllViewModels()
        //{
        //    var list = db.PoliceCases.AsEnumerable().Where(c => c.IsDeleted == false).Select(c => ConvertToViewModel(c)).ToList();
        //    return list;
        //}
        //public List<VM_PoliceCase> GetUnsolvedViewModels()
        //{
        //    var list = db.PoliceCases.AsEnumerable().Where(c => c.IsDeleted == false && c.IsSolved != true).Select(c => ConvertToViewModel(c)).ToList();
        //    return list;
        //}
        //public List<VM_PoliceCase> GetUnsolvedViewModelsBy()
        //{
        //    var accessibleDepoes = db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();

        //    var list = (db.PoliceCases.AsEnumerable().Where(pc => db.Vehicles.Where(vehicle => accessibleDepoes.Contains(vehicle.FK_Depo)).Select(vehicle => vehicle.PK_Vehicle).Contains(pc.FK_Vehicle) && pc.IsDeleted != true && pc.IsSolved != true).Select(eve => ConvertToViewModel(eve))).ToList();
        //    return list;
        //}
        //public List<VM_PoliceCase> GetSolvedViewModelsBy()
        //{
        //    var accessibleDepoes = db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();

        //    var list = (db.PoliceCases.AsEnumerable().Where(pc => db.Vehicles.Where(vehicle => accessibleDepoes.Contains(vehicle.FK_Depo)).Select(vehicle => vehicle.PK_Vehicle).Contains(pc.FK_Vehicle) && pc.IsDeleted != true && pc.IsSolved == true).Select(eve => ConvertToViewModel(eve))).ToList();
        //    return list;
        //}
        //public List<VM_PoliceCase> GetSolvedViewModels()
        //{
        //    var list = db.PoliceCases.AsEnumerable().Where(c => c.IsDeleted == false && c.IsSolved == true).Select(c => ConvertToViewModel(c)).ToList();
        //    return list;
        //}

        //public List<VM_PoliceCase> GetAllViewModelsBy_FK_Vehicle(Guid FK_Vehicle)
        //{
        //    var list = db.PoliceCases.AsEnumerable().Where(c => c.IsDeleted == false && c.FK_Vehicle == FK_Vehicle).Select(c => ConvertToViewModel(c)).ToList();
        //    return list;
        //}

        //public string IsValidModel_ToCreate(PoliceCase model)
        //{
        //    string result = "";
        //    //if (db.PoliceCases.Where(c => c.UniqueIDNumber.ToUpper() == model.UniqueIDNumber.Trim()).Any())
        //    //{
        //    //    result += "This unique ID number is already used by another PoliceCase. Please, use an unique id number.";
        //    //    
        //    //}

        //    if (result == "")
        //    {
        //        result = ValidationStatus.OK;
        //    }
        //    return result;
        //}
        //public string IsValidModel_ToEdit(PoliceCase model)
        //{
        //    string result = "";

        //    //# checks, registraton number is unique
        //    //var existing = db.PoliceCases.Where(c => c.PK_Case != model.PK_Case && c.UniqueIDNumber.ToUpper() == model.UniqueIDNumber.Trim()).Select(v => v).ToList().Count();
        //    //if (existing > 1)
        //    //{
        //    //    result += "This unique ID number is already used by another PoliceCase. Please, use an unique id number.";
        //    //    
        //    //}

        //    if (result == "")
        //    {
        //        result = ValidationStatus.OK;
        //    }
        //    return result;
        //}


        //public PoliceCase FilterToDBModel(PoliceCase model)
        //{
        //    PoliceCase db_model;
        //    if (model.PK_PoliceCase.ToString() == "00000000-0000-0000-0000-000000000000")
        //    {
        //        db_model = new PoliceCase();
        //        db_model.PK_PoliceCase = Guid.NewGuid();
        //        db_model.IsDeleted = false;

        //        db_model.CreatedAt = DateTime.Now;
        //        db_model.FK_CreatedByUser = CommonClass.GetCurrentUser().PK_User;
        //        //# contexual property
        //        db_model.IsSolved = false;
        //    }
        //    else
        //    {
        //        db_model = db.PoliceCases.Find(model.PK_PoliceCase);

        //        db_model.UpdatedAt = DateTime.Now;
        //        db_model.FK_UpdatedByUser = CommonClass.GetCurrentUser().PK_User;
        //    }
        //    db_model.FK_Vehicle = model.FK_Vehicle;
        //    db_model.IssueDate = model.IssueDate;
        //    db_model.IsAlertable = model.IsAlertable;
        //    db_model.AlertDate = model.IsAlertable == true ? model.AlertDate : null;
        //    //db_model.PrimaryAmount = model.PrimaryAmount;
        //    //db_model.PrimaryAmount = model.PrimaryAmount;
        //    //db_model.PrimaryAmount = model.PrimaryAmount;
        //    //db_model.PrimaryAmount = model.PrimaryAmount;
        //    //db_model.PrimaryAmount = model.PrimaryAmount;
        //    return db_model;
        //}

        public VM_PoliceCase ConvertToViewModel(PoliceCase model)
        {
            var viewModel = new VM_PoliceCase();
            viewModel.Model = model;
            //# Relational property

            //# Only view property
            viewModel.IsDeleted_Text = model.IsDeleted == true ? "Deleted" : "Undeleted";
            viewModel.IssueDate_Text = model.IssueDate == null ? "" : CommonClass.ConvertToDateString(model.IssueDate);
            viewModel.IsAlertable_Text = model.IsAlertable == true ? "Yes" : "No";
            viewModel.AlertDate_Text = model.AlertDate == null ? "" : CommonClass.ConvertToDateString(model.AlertDate);
            viewModel.SolvedOn_Text= model.SolvedOn == null ? "" : CommonClass.ConvertToDateString(model.SolvedOn);
            viewModel.IsSolved_Text = model.IsSolved == true ? "Solved" : "Unsolved";
            //LisenceRenewalDate_Text
            return viewModel;
        }
    }
}