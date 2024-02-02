using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _3rdEyE.Models;

namespace _3rdEyE.ViewModels
{
    public class VM_InstantRequisition
    {
        public InstantRequisition Model { get; set; }

        //# Only view property
        public string Created_Text { get; set; }
    }
}