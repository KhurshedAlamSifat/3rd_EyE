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
    public class BLL_IndividualRequisitionBidding : BaseBLL
    {
        //public List<VM_IndividualRequisitionBidding> GetAllViewModels_Bidder()
        //{
        //    var list = db.IndividualRequisitionBiddings.AsEnumerable().Where(c => c.FK_RequisitionAgent_Bidder ==  CurrentUser.PK_User && c.Status != -1).Select(c => ConvertToViewModel(c)).ToList();
        //    return list;
        //}

        //public List<VM_IndividualRequisitionBidding> GetAllViewModelsCreateBy_FK_IndividualRequisition(Guid PK_VM_IndividualRequisition)
        //{
        //    var list = db.IndividualRequisitionBiddings.AsEnumerable().Where(m=>m.FK_IndividualRequisition == PK_VM_IndividualRequisition).Select(c => ConvertToViewModel(c)).ToList();
        //    return list;
        //}

        public string IsValidModel_ToCreate(IndividualRequisitionBidding model)
        {
            string result = "";
            if (db.IndividualRequisitionBiddings.Where(c => c.FK_IndividualRequisition == model.FK_IndividualRequisition && c.FK_RequisitionAgent_Bidder == model.FK_RequisitionAgent_Bidder).Any())
            {
                result += "This Individual Requisition is already bidded. Can not bid more than once.";
            }
            if (result == "")
            {
                result = ValidationStatus.OK;
            }
            return result;
        }

        public IndividualRequisitionBidding FilterToDBModel(IndividualRequisitionBidding model)
        {
            IndividualRequisitionBidding db_model;
            db_model = new IndividualRequisitionBidding();
            db_model.PK_IndividualRequisitionBidding = Guid.NewGuid();

            db_model.FK_IndividualRequisition = model.FK_IndividualRequisition;
            db_model.FK_RequisitionAgent_Bidder= model.FK_RequisitionAgent_Bidder;
            db_model.ManagableQuantity = model.ManagableQuantity;
            db_model.PricePerQuantity = model.PricePerQuantity;
            db_model.Status= model.Status;
            return db_model;
        }

        public VM_IndividualRequisitionBidding ConvertToViewModel(IndividualRequisitionBidding model)
        {
            var viewModel = new VM_IndividualRequisitionBidding();
            viewModel.Model = model;

            //# Only view property
            viewModel.PossibleJourneyStartDateTime_Text = model.IndividualRequisition.PossibleJourneyStartDateTime == null ? "" : CommonClass.ConvertToDateTimeString(model.IndividualRequisition.PossibleJourneyStartDateTime);
            if (model.Status==1)
            {
                viewModel.Status_Text = "Proposed";
            }else if (model.Status == 2)
            {
                viewModel.Status_Text = "Bidded";
            }
            else if (model.Status == 3)
            {
                viewModel.Status_Text = "Approved";
            }
            else if (model.Status == -1)
            {
                viewModel.Status_Text = "Cancelled";
            }
            else if (model.Status == -2)
            {
                viewModel.Status_Text = "Rejected";
            }
            return viewModel;
        }
    }
}