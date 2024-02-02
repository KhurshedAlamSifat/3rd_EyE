using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _3rdEyE.Models;

namespace _3rdEyE.ViewModels
{
    public class VM_Depo
    {
        //public long RowSerial { get; set; }
        //public System.Guid PK_Depo { get; set; }
        //public Nullable<bool> IsDeleted { get; set; }
        //public bool IsActive { get; set; }
        //public string Name { get; set; }
        //public string Latitude { get; set; }
        //public string Longitude { get; set; }

        public Depo Model { get; set; }


        //# Only view property
        public string IsActive_Text { get; set; }
        public string IsDeleted_Text { get; set; }
    }
}