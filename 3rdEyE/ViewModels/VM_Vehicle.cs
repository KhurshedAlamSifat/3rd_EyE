using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _3rdEyE.Models;

namespace _3rdEyE.ViewModels
{
    public class VM_Vehicle
    {
        public Vehicle Model { get; set; }

        //# Only view property
        public string IsActive_Text { get; set; }
        public string IsDeleted_Text { get; set; }
        //public string UpdatedAt_Text { get; set; }
        public string RegisrationDate_Text { get; set; }
        public string Internal_PurchaseDate_Text { get; set; }
        public string Internal_ShowTemperature_Text { get; set; }
        public string NumberPlate_IsDigital_Text { get; set; }
        public string MHT_AgreementFrom_Text { get; set; }
        public string MHT_AgreementTo_Text { get; set; }
        public string Internal_FinancingAgrementMaturityDate_Text { get; set; }
        public string Internal_FinancingAgrementGRN_MRR_Date_Text { get; set; }
    }
}