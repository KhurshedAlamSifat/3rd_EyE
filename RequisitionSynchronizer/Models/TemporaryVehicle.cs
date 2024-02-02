//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RequisitionSynchronizer.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TemporaryVehicle
    {
        public long PK_TemporaryVehicle { get; set; }
        public string DevelopersNote { get; set; }
        public string RegistrationNumber { get; set; }
        public Nullable<System.Guid> FK_Locaiton { get; set; }
        public Nullable<long> FK_PRG_Type { get; set; }
        public Nullable<System.Guid> FK_CreatedByLocationGate { get; set; }
        public string GPNumber { get; set; }
        public string LoadOrEmpty { get; set; }
        public Nullable<long> FK_VehicleInOutManualTypesOfProduct { get; set; }
        public Nullable<long> FK_VehicleInOutManualReason { get; set; }
        public System.DateTime IssueDateTime { get; set; }
        public string IssuedDateTimeAutoOrManual { get; set; }
        public Nullable<bool> IsScannedEntry { get; set; }
        public string Note { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string StatusText { get; set; }
        public Nullable<int> EntryCount { get; set; }
    
        public virtual AppUser AppUser { get; set; }
        public virtual Location Location { get; set; }
        public virtual PRG_Type PRG_Type { get; set; }
    }
}
