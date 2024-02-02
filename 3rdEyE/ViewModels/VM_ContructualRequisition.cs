using _3rdEyE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3rdEyE.ViewModels
{
    public class VM_ContructualRequisition
    {
        public ContructualRequisition Model { get; set; }
        //# Only view property CreatedAt
        public string CreatedAt_Text { get; set; }
        public string ContructActivatingDate_Text { get; set; }
        public string ContructDeactivatingDate_Text { get; set; }
    }
}