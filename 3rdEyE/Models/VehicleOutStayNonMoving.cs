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
    
    public partial class VehicleOutStayNonMoving
    {
        public long PK_VehicleOutStayNonMoving { get; set; }
        public System.Guid FK_Vehicle { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public System.DateTime StartedAt { get; set; }
        public System.DateTime StartEntryGivenAt { get; set; }
        public Nullable<System.DateTime> EndedAt { get; set; }
        public Nullable<System.DateTime> EndEntryGivenAt { get; set; }
        public Nullable<long> TimeDifferenceMinute { get; set; }
    }
}