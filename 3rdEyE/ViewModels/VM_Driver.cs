using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _3rdEyE.Models;

namespace _3rdEyE.ViewModels
{
    public class VM_Driver
    {
        public Driver Model { get; set; }

        //# Relational property
        //public Depo Depo { get; set; }

        //# Only view property
        public string IsActive_Text { get; set; }
        public string IsDeleted_Text { get; set; }
        public string LisenceRenewalDate_Text { get; set; }
    }
}