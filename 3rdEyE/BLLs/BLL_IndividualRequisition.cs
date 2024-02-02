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
    public class BLL_IndividualRequisition : BaseBLL
    {
        public string IsValidModel_ToCreate(IndividualRequisition model)
        {
            string result = "";
            //if (db.IndividualRequisitions.Where(c => c.RegistrationNumber.ToUpper() == model.RegistrationNumber.Trim()).Any())
            //{
            //    result += "This registration number is already used by another IndividualRequisition. Please, use an unique registration number. ";
            //}
            if (result == "")
            {
                result = ValidationStatus.OK;
            }
            return result;
        }
        public string IsValidModel_ToEdit(IndividualRequisition model)
        {
            string result = "";

            //# checks, registraton number is unique
            //if (db.IndividualRequisitions.Where(c => c.PK_IndividualRequisition != model.PK_IndividualRequisition && c.RegistrationNumber.ToUpper() == model.RegistrationNumber.Trim()).Any())
            //{
            //    result += "This registration number is already used by another IndividualRequisition. Please, use an unique registration number. ";
            //}

            if (result == "")
            {
                result = ValidationStatus.OK;
            }
            return result;
        }
        public VM_IndividualRequisition ConvertToViewModel(IndividualRequisition model)
        {
            var viewModel = new VM_IndividualRequisition();
            viewModel.Model = model;

            //# Only view property
            viewModel.CreationDateTime_Text = model.CreatedAt == null ? "" : CommonClass.ConvertToDateString(model.CreatedAt);
            viewModel.PossibleJourneyStartDateTime_Text = model.PossibleJourneyStartDateTime == null ? "" : CommonClass.ConvertToDateTimeString(model.PossibleJourneyStartDateTime);
            if (model.Status == 0)
            {
                viewModel.Status_Text = "Open";
            }
            if (model.Status == 1)
            {
                viewModel.Status_Text = "Close";
            }
            return viewModel;
        }
    }
}