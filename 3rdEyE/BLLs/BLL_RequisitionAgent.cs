using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _3rdEyE.Models;
using _3rdEyE.ManagingTools;
using _3rdEyE.ViewModels;
using _3rdEyE.BLLs;
using _3rdEyE.BLL;

namespace _3rdEyE.BLLs
{
    public class BLL_TransportAgent : BaseBLL
    {
        //public List<VM_AppUser> GetAllViewModels()
        //{
        //    var list = db.AppUsers.AsEnumerable().Where(c => c.IsDeleted == false && (c.AppUserType == "External Transport Agent")).Select(c => ConvertToViewModel(c)).ToList();
        //    return list;
        //}
        //public List<VM_AppUser> GetAllViewModels_OwnPRG_Group()
        //{
        //    var list = db.AppUsers.AsEnumerable().Where(c => c.IsDeleted == false && c.PRG_Type == CurrentUser.PRG_Type).Select(c => ConvertToViewModel(c)).ToList();
        //    return list;
        //}

        public string IsValidModel_ToCreate(AppUser model)
        {
            string result = "";

            if (!(string.IsNullOrEmpty(model.ContactNumber)) && db.AppUsers.Where(c => c.ContactNumber.ToUpper().Trim() == model.ContactNumber.ToUpper().Trim()).Any())
            {
                result += "This contact number is already used by another user or agent. Please, use another contact number. ";
            }

            if (result == "")
            {
                result = ValidationStatus.OK;
            }
            return result;
        }
        public string IsValidModel_ToEdit(AppUser model)
        {
            string result = "";

            if (!(string.IsNullOrEmpty(model.ContactNumber)) && db.AppUsers.Where(c => c.PK_User != model.PK_User && c.ContactNumber.ToUpper().Trim() == model.ContactNumber.ToUpper().Trim()).Any())
            {
                result += "This contact number is already used by another user or agent. Please, use another contact number. ";
            }

            if (result == "")
            {
                result = ValidationStatus.OK;
            }
            return result;
        }

        public AppUser FilterToDBModel(AppUser model)
        {
            AppUser db_model;
            if (model.PK_User.ToString() == "00000000-0000-0000-0000-000000000000")
            {
                db_model = new AppUser();
                db_model.PK_User = Guid.NewGuid();
                db_model.IsDeleted = false;
                db_model.FK_CreatedByUser = CommonClass.GetCurrentUser().PK_User;
                db_model.CreatedAt = DateTime.Now;
                db_model.PermissionAdd = true;
            }
            else
            {
                db_model = db.AppUsers.Find(model.PK_User);
                db_model.FK_UpdatedByUser = CommonClass.GetCurrentUser().PK_User;
                db_model.UpdatedAt = DateTime.Now;
            }
            db_model.UniqueIDNumber = model.ContactNumber;
            db_model.Password = model.Password;
            db_model.FullName = model.FullName.ToUpper().Trim();
            db_model.PRG_Type = "External";
            db_model.AppUserType = "External Transport Agent";
            db_model.FK_Depo = model.FK_Depo;
            db_model.FK_TransportCompany = model.FK_TransportCompany;
            db_model.ContactNumber = model.ContactNumber;
            db_model.ContactAddress = model.ContactAddress;
            return db_model;
        }

        public VM_AppUser ConvertToViewModel(AppUser model)
        {
            var viewModel = new VM_AppUser();
            viewModel.Model = model;


            return viewModel;
        }
    }
}