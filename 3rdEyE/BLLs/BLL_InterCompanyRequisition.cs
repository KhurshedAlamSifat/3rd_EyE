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
    public class BLL_InterCompanyRequisition : BaseBLL
    {
        public string IsValidModel_ToCreate(InterCompanyRequisition model)
        {
            string result = "";
            //if (db.InterCompanyRequisitions.Where(c => c.RegistrationNumber.ToUpper() == model.RegistrationNumber.Trim()).Any())
            //{
            //    result += "This registration number is already used by another InterCompanyRequisition. Please, use an unique registration number. ";
            //}
            if (result == "")
            {
                result = ValidationStatus.OK;
            }
            return result;
        }
        public string IsValidModel_ToEdit(InterCompanyRequisition model)
        {
            string result = "";

            //# checks, registraton number is unique
            //if (db.InterCompanyRequisitions.Where(c => c.PK_InterCompanyRequisition != model.PK_InterCompanyRequisition && c.RegistrationNumber.ToUpper() == model.RegistrationNumber.Trim()).Any())
            //{
            //    result += "This registration number is already used by another InterCompanyRequisition. Please, use an unique registration number. ";
            //}

            if (result == "")
            {
                result = ValidationStatus.OK;
            }
            return result;
        }
        public VM_InterCompanyRequisition ConvertToViewModel(InterCompanyRequisition model)
        {
            var viewModel = new VM_InterCompanyRequisition();
            viewModel.Model = model;

            //# Only view property
            viewModel.CreationDateTime_Text = model.CreatedAt == null ? "" : CommonClass.ConvertToDateTimeString(model.CreatedAt);
            viewModel.UpdateDateTime_Text= model.UpdatedAt == null ? "" : CommonClass.ConvertToDateTimeString(model.UpdatedAt);
            viewModel.PossibleJourneyStartDateTime_Text = model.PossibleJourneyStartDateTime == null ? "" : CommonClass.ConvertToDateTimeString(model.PossibleJourneyStartDateTime);
            viewModel.VerifiedAt_Text = model.VerifiedAt == null ? "" : CommonClass.ConvertToDateTimeString(model.VerifiedAt);
            if (model.Status == 0)
            {
                viewModel.Status_Text = "Pending";
            }
            else if (model.Status == 1)
            {
                viewModel.Status_Text = "Accepted";
            }
            else if (model.Status == -1)
            {
                viewModel.Status_Text = "Rejected";
            }
            return viewModel;
        }
    }
}