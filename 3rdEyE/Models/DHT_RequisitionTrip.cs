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
    
    public partial class DHT_RequisitionTrip
    {
        public long PK_DHT_RequisitionTrip { get; set; }
        public Nullable<System.Guid> FK_Vehicle { get; set; }
        public string RegistrationNumber { get; set; }
        public Nullable<long> FK_RequisitionTrip { get; set; }
        public Nullable<System.Guid> FK_Location_From { get; set; }
        public Nullable<System.Guid> FK_Location_To { get; set; }
        public Nullable<System.DateTime> FinalWantedAtDateTime { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
    }
}
