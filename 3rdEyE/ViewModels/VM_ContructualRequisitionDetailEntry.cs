using _3rdEyE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3rdEyE.ViewModels
{
    public class VM_ContructualRequisitionDetailEntry
    {
        public ContructualRequisitionDetailEntry Model { get; set; }
        //# Only view property CreatedAt
        public string CreatedAt_Text { get; set; }
        public string ContractAcitivatingDate_Text { get; set; }
        public string ContractDeactivatingDate_Text { get; set; }
    }
}