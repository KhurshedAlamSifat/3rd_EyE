//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace _3rdEyE.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DairyVehicle
    {
        public System.Guid PK_DairyVehicle { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public string Destination { get; set; }
        public System.Guid FK_AppUser_CreatedBy { get; set; }
        public System.DateTime CreatedAt { get; set; }
    
        public virtual Vehicle Vehicle { get; set; }
    }
}
