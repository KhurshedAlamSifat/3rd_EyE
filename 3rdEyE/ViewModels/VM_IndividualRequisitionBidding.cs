using _3rdEyE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3rdEyE.ViewModels
{
    public class VM_IndividualRequisitionBidding
    {
        public IndividualRequisitionBidding Model { get; set; }
        //# Only view property
        public string Status_Text { get; set; }
        public string PossibleJourneyStartDateTime_Text { get; set; }
    }
}