using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _3rdEyE.Models;
using _3rdEyE.ManagingTools;
using _3rdEyE.ViewModels;
using _3rdEyE.BLL;

namespace _3rdEyE.BLLs
{
    public class BLL_DepoGroup : BaseBLL
    {
        public string IsValidModel_ToCreate(DepoGroup model)
        {
            string result = "";
            if (db.DepoGroups.Where(c => c.Name.ToUpper() == model.Name.Trim()).Any())
            {
                result += "This group name is already used by another User Sub-Group. Please, use another name. ";
            }

            if (result == "")
            {
                result = ValidationStatus.OK;
            }
            return result;
        }
        public string IsValidModel_ToEdit(DepoGroup model)
        {
            string result = "";

            //# checks, registraton number is unique
            var existing = db.DepoGroups.Where(c => c.PK_DepoGroup != model.PK_DepoGroup && c.Name.ToUpper() == model.Name
            .Trim()).Select(v => v).ToList().Count();
            if (existing > 1)
            {
                result += "This group name is already used by another User Sub-Group. Please, use another name. ";
            }

            if (result == "")
            {
                result = ValidationStatus.OK;
            }
            return result;
        }


        public DepoGroup FilterToDBModel(DepoGroup model)
        {
            DepoGroup db_model;
            if (model.PK_DepoGroup.ToString() == "00000000-0000-0000-0000-000000000000")
            {
                db_model = new DepoGroup();
                db_model.PK_DepoGroup = Guid.NewGuid();
                db_model.IsDeleted = false;
                db_model.CreatedAt = DateTime.Now;
                db_model.FK_CreatedByUser = CommonClass.GetCurrentUser().PK_User;
            }
            else
            {
                db_model = db.DepoGroups.Find(model.PK_DepoGroup);
                db_model.UpdatedAt = DateTime.Now;
                db_model.FK_UpdatedByUser = CommonClass.GetCurrentUser().PK_User;
            }

            db_model.FK_Depo = model.FK_Depo;
            db_model.Name = model.Name.Trim().ToUpper();
            return db_model;
        }

        //public VM_DepoGroup ConvertToViewModel(DepoGroup model)
        //{
        //    var viewModel = new VM_DepoGroup();
        //    viewModel.Model = model;
        //    //# Relational property

        //    //# Only view property
        //    viewModel.IsDeleted_Text = model.IsDeleted == true ? "Deleted" : "Undeleted";

        //    viewModel.LisenceRenewalDate_Text = model.LisenceRenewalDate == null ? "" : CommonClass.ConvertToDateString(model.LisenceRenewalDate);
        //    //LisenceRenewalDate_Text
        //    return viewModel;
        //}
    }
}