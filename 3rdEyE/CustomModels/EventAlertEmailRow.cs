using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3rdEyE.CustomModels
{
    public class EventAlertEmailRow
    {
        public string vehicle_Reg { get; set; }
        public string vehicle_Depo { get; set; }
        public string fitness_paper_expirationDate { get; set; }
        public string insurance_expirationDate { get; set; }
        public string route_permit_expirationDate { get; set; }
        public string tax_token_expirationDate { get; set; }
    }
}