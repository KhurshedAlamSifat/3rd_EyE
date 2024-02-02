using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _3rdEyE.Models;
using _3rdEyE.ManagingTools;
using _3rdEyE.ViewModels;


namespace _3rdEyE.BLL
{
    public class BLL_Event : BaseBLL
    {
        //    //public List<Event> GetAllModels()
        //    //{
        //    //    var list = db.Companies.AsEnumerable().Select(c => c ).ToList();
        //    //    return list;
        //    //}
        //    //public List<Event> GetActiveModels()
        //    //{
        //    //    var list = db.Companies.AsEnumerable().Where(c => c.IsActive == true).Select(c => c).ToList();
        //    //    return list;
        //    //}

        public List<VM_Event> GetAllViewModels()
        {
            var list = db.Events.AsEnumerable().Select(c => ConvertToViewModel(c)).ToList();
            return list;
        }
        public List<VM_Event> GetViewModelsBy_AccessibleDepoesofCurrentUser()
        {
            var accessibleDepoes = db.AppUserAccessibleDepoes.Where(m => m.FK_AppUser == CurrentUser.PK_User && m.IsAccessible == true).Select(m => m.FK_Depo).ToList();
            var pk_vehicles = db.Vehicles.AsEnumerable().Where(c => c.IsDeleted == false).Where(m => accessibleDepoes.Contains(m.FK_Depo)).Select(c => c.PK_Vehicle).ToList();
            var list = db.Events.AsEnumerable().Where(c => c.IsDeleted == false && pk_vehicles.Contains(c.FK_Vehicle)).Select(c => ConvertToViewModel(c)).ToList();
            return list;
        }

        //public List<VM_Event> GetAllViewModelsBy_PK_Depo(Guid? FK_Depo)
        //{
        //    var list = (db.Events.AsEnumerable().Where(eve => db.Vehicles.Where(vehicle => vehicle.FK_Depo == FK_Depo).Select(vehicle => vehicle.PK_Vehicle).Contains(eve.FK_Vehicle) && eve.IsDeleted != true ).Select(eve => ConvertToViewModel(eve))).ToList();
        //    return list;
        //}

        //public List<VM_Event> GetAllViewModelsBy_FK_Vehicle(Guid FK_Vehicle)
        //{
        //    var list = db.Events.AsEnumerable().Where(c => c.IsDeleted == false && c.FK_Vehicle == FK_Vehicle).Select(c => ConvertToViewModel(c)).ToList();
        //    return list;
        //}

        //public string IsValidModel_ToCreate(Event model)
        //{
        //    string result = "";
        //    //if (db.Events.Where(c => c.UniqueIDNumber.ToUpper() == model.UniqueIDNumber.Trim()).Any())
        //    //{
        //    //    result += "This unique ID number is already used by another Event. Please, use an unique id number.";
        //    //    
        //    //}

        //    if (result == "")
        //    {
        //        result = ValidationStatus.OK;
        //    }
        //    return result;
        //}
        //public string IsValidModel_ToEdit(Event model)
        //{
        //    string result = "";

        //    //# checks, registraton number is unique
        //    //var existing = db.Events.Where(c => c.PK_Event != model.PK_Event && c.UniqueIDNumber.ToUpper() == model.UniqueIDNumber.Trim()).Select(v => v).ToList().Count();
        //    //if (existing > 1)
        //    //{
        //    //    result += "This unique ID number is already used by another Event. Please, use an unique id number.";
        //    //    
        //    //}

        //    if (result == "")
        //    {
        //        result = ValidationStatus.OK;
        //    }
        //    return result;
        //}


        //public Event FilterToDBModel(Event model)
        //{
        //    Event db_model;
        //    if (model.PK_Event.ToString() == "00000000-0000-0000-0000-000000000000")
        //    {
        //        db_model = new Event();
        //        db_model.PK_Event = Guid.NewGuid();
        //        db_model.IsDeleted = false;

        //        db_model.CreatedAt = DateTime.Now;
        //        db_model.FK_CreatedByUser = CommonClass.GetCurrentUser().PK_User;
        //        //# contexual property
        //        db_model.AlertOn = true;
        //    }
        //    else
        //    {
        //        db_model = db.Events.Find(model.PK_Event);

        //        db_model.UpdatedAt = DateTime.Now;
        //        db_model.FK_UpdatedByUser = CommonClass.GetCurrentUser().PK_User;
        //    }
        //    db_model.FK_Vehicle = model.FK_Vehicle;
        //    db_model.FK_EventType = model.FK_EventType;
        //    db_model.IssueDate = model.IssueDate;
        //    db_model.IsAlertable = model.IsAlertable;
        //    db_model.AlertDate = model.IsAlertable == true ? model.AlertDate : null;
        //    return db_model;
        //}

        public VM_Event ConvertToViewModel(Event model)
        {
            var viewModel = new VM_Event();
            viewModel.Model = model;
            //# Relational property

            //# Only view property
            viewModel.IsDeleted_Text = model.IsDeleted == true ? "Deleted" : "Undeleted";
            viewModel.IssueDate_Text = model.IssueDate == null ? "" : CommonClass.ConvertToDateString(model.IssueDate);
            viewModel.DepositDate_Text = model.DepositDate == null ? "" : CommonClass.ConvertToDateString(model.DepositDate);
            viewModel.IsAlertable_Text = model.IsAlertable == true ? "Yes" : "No";
            //viewModel.AlertDate_Text = model.AlertDate == null ? "" : CommonClass.ConvertToDateString(model.AlertDate);
            viewModel.ExpirationDate_Text = model.ExpirationDate == null ? "" : CommonClass.ConvertToDateString(model.ExpirationDate);
            viewModel.AlertOn_Text = model.AlertOn == true ? "On" : "Off";

            //LisenceRenewalDate_Text
            return viewModel;
        }
    }
}