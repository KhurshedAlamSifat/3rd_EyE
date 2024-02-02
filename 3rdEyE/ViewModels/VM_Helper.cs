using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _3rdEyE.Models;

namespace _3rdEyE.ViewModels
{
    public class VM_Helper
    {
        public Helper Model { get; set; }
        //# Only view property
        public string IsActive_Text { get; set; }
        public string IsDeleted_Text { get; set; }
    }
}