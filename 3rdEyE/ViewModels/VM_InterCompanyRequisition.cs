using _3rdEyE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3rdEyE.ViewModels
{
    public class VM_InterCompanyRequisition
    {
        public InterCompanyRequisition Model { get; set; }
        //# Only view property
        public string CreationDateTime_Text { get; set; }
        public string UpdateDateTime_Text { get; set; }
        public string PossibleJourneyStartDateTime_Text { get; set; }
        public string VerifiedAt_Text { get; set; }
        public string Status_Text { get; set; }
    }
}