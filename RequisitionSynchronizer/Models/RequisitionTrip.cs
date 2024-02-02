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
    
    public partial class RequisitionTrip
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RequisitionTrip()
        {
            this.RequisitionTrip1 = new HashSet<RequisitionTrip>();
            this.Vehicles = new HashSet<Vehicle>();
            this.Vehicles1 = new HashSet<Vehicle>();
        }
    
        public long PK_RequisitionTrip { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string TrackingID { get; set; }
        public long FK_Requisition { get; set; }
        public Nullable<double> WantedCount { get; set; }
        public Nullable<System.Guid> FK_Vehicle { get; set; }
        public string OWN_MHT_DHT { get; set; }
        public string Driver_Staff_ID { get; set; }
        public string Driver_Name { get; set; }
        public string Driver_ContactNumber { get; set; }
        public Nullable<long> TotalAmount { get; set; }
        public Nullable<System.Guid> FK_AppUser_Assigner { get; set; }
        public Nullable<System.DateTime> AssingedAt { get; set; }
        public Nullable<System.DateTime> TentativeFinishingDateTime { get; set; }
        public Nullable<System.Guid> FK_AppUser_Canceller { get; set; }
        public Nullable<System.DateTime> CancelledAt { get; set; }
        public string StatusText { get; set; }
        public Nullable<System.DateTime> StartedAt { get; set; }
        public Nullable<System.Guid> FK_AppUser_Start { get; set; }
        public Nullable<System.Guid> FK_LocationGate_Start { get; set; }
        public string StartAutoOrManaul { get; set; }
        public Nullable<System.DateTime> FinishedAt { get; set; }
        public Nullable<System.Guid> FK_AppUser_Finish { get; set; }
        public Nullable<System.Guid> FK_LocationGate_Finish { get; set; }
        public string FinishAutoOrManaul { get; set; }
        public Nullable<bool> IsParent { get; set; }
        public Nullable<long> FK_RequisitionTrip_Parent { get; set; }
        public Nullable<bool> IsForwarded { get; set; }
        public Nullable<System.Guid> FK_Location_ForwardedTo { get; set; }
        public string PRG_Type_ForwaredTo { get; set; }
        public Nullable<System.DateTime> ForwardedAt { get; set; }
        public Nullable<System.Guid> FK_AppUser_ForwardedBy { get; set; }
    
        public virtual AppUser AppUser { get; set; }
        public virtual Requisition Requisition { get; set; }
        public virtual Vehicle Vehicle { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RequisitionTrip> RequisitionTrip1 { get; set; }
        public virtual RequisitionTrip RequisitionTrip2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vehicle> Vehicles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vehicle> Vehicles1 { get; set; }
    }
}