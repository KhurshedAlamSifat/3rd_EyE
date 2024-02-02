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
    public class BLL_ContructualRequisition : BaseBLL
    {
        public List<VM_ContructualRequisition> GetAllViewModels()
        {
            var list = db.ContructualRequisitions.AsEnumerable()/*.Where(c => c.Status == false)*/.Select(c => ConvertToViewModel(c)).ToList();
            return list;
        }
        public List<VM_ContructualRequisition> GetAllViewModelsCreateBy_Agent(Guid PK_Agent)
        {
            var list = db.ContructualRequisitions.AsEnumerable()./*Where(m => m.FK_RequisitionAgent == PK_Agent && m.IsDeleted == false).*/Select(c => ConvertToViewModel(c)).ToList();
            return list;
        }

        public string IsValidModel_ToCreate(ContructualRequisition model)
        {
            string result = "";
            //if (db.ContructualRequisitions.Where(c => c.RegistrationNumber.ToUpper() == model.RegistrationNumber.Trim()).Any())
            //{
            //    result += "This registration number is already used by another ContructualRequisition. Please, use an unique registration number. ";
            //}
            if (result == "")
            {
                result = ValidationStatus.OK;
            }
            return result;
        }
        public string IsValidModel_ToEdit(ContructualRequisition model)
        {
            string result = "";

            //# checks, registraton number is unique
            //if (db.ContructualRequisitions.Where(c => c.PK_ContructualRequisition != model.PK_ContructualRequisition && c.RegistrationNumber.ToUpper() == model.RegistrationNumber.Trim()).Any())
            //{
            //    result += "This registration number is already used by another ContructualRequisition. Please, use an unique registration number. ";
            //}

            if (result == "")
            {
                result = ValidationStatus.OK;
            }
            return result;
        }
        public VM_ContructualRequisition ConvertToViewModel(ContructualRequisition model)
        {
            var viewModel = new VM_ContructualRequisition();
            viewModel.Model = model;

            //# Only view property
            viewModel.ContructActivatingDate_Text = model.ContructAcitivatingDate == null ? "" : CommonClass.ConvertToDateString(model.ContructAcitivatingDate);
            viewModel.ContructDeactivatingDate_Text = model.ContructDeactivatingDate == null ? "" : CommonClass.ConvertToDateTimeString(model.ContructDeactivatingDate);
            return viewModel;
        }
    }
}