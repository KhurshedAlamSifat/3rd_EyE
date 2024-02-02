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
    public class BLL_InstantRequisition : BaseBLL
    {
        //public List<VM_InstantRequisition> GetAllViewModels()
        //{
        //    var list = db.InstantRequisitions.AsEnumerable().Where(c => c.IsDeleted == false).Select(c => ConvertToViewModel(c)).ToList();
        //    return list;
        //}

        public string IsValidModel_ToCreate(InstantRequisition model)
        {
            string result = "";

            //# checks, name is unique
            //if (db.Companies.Where(c => c.Name.ToUpper() == model.Name.ToUpper().Trim()).Any())
            //{
            //    result += "This company name is already used by another compnay. Please, use an unique name. ";

            //}

            if (result == "")
            {
                result = ValidationStatus.OK;
            }
            return result;
        }
        public string IsValidModel_ToEdit(InstantRequisition model)
        {
            string result = "";

            //# checks, name is unique
            //if (db.Companies.Where(c => c.PK_Company != model.PK_Company && c.Name.ToUpper() == model.Name.ToUpper().Trim()).Any())
            //{
            //    result += "This company name is already used by another compnay. Please, use an unique name.";

            //}

            if (result == "")
            {
                result = ValidationStatus.OK;
            }
            return result;
        }


        public VM_InstantRequisition ConvertToViewModel(InstantRequisition model)
        {
            var viewModel = new VM_InstantRequisition();
            viewModel.Model = model;

            //# Only view property
            viewModel.Created_Text = model.CreatedAt != null ? CommonClass.ConvertToDateTimeString(model.CreatedAt) : "";
            return viewModel;
        }
    }
}