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
    
    public partial class PoliceCase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PoliceCase()
        {
            this.PoliceCase_PoliceCaseLaw = new HashSet<PoliceCase_PoliceCaseLaw>();
            this.PoliceCaseDocuments = new HashSet<PoliceCaseDocument>();
        }
    
        public long RowSerial { get; set; }
        public System.Guid PK_PoliceCase { get; set; }
        public bool IsDeleted { get; set; }
        public System.Guid FK_Vehicle { get; set; }
        public string AccusedDriverStaffID { get; set; }
        public string AccusedDriverName { get; set; }
        public string CaseID { get; set; }
        public string PoliceContactNumber { get; set; }
        public Nullable<long> FK_District { get; set; }
        public Nullable<long> FK_Upazila { get; set; }
        public string TypeOfFault { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> IssueDate { get; set; }
        public Nullable<bool> IsAlertable { get; set; }
        public Nullable<System.DateTime> AlertDate { get; set; }
        public Nullable<bool> IsSolved { get; set; }
        public Nullable<System.DateTime> SolvedEntryGivenAt { get; set; }
        public Nullable<System.Guid> FK_SolvedEntryGivenUser { get; set; }
        public Nullable<bool> IsPaid { get; set; }
        public Nullable<System.DateTime> PaidEntryGivenAt { get; set; }
        public Nullable<System.Guid> FK_PaidEntryGivenUser { get; set; }
        public Nullable<long> PrimaryAmount { get; set; }
        public Nullable<long> OtherAmount { get; set; }
        public string OtherNote { get; set; }
        public Nullable<long> TotalAmount { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<System.DateTime> SolvedOn { get; set; }
        public string SolvedNote { get; set; }
        public Nullable<System.Guid> FK_CreatedByUser { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public Nullable<System.Guid> FK_UpdatedByUser { get; set; }
        public Nullable<System.DateTime> DeletedAt { get; set; }
        public Nullable<System.Guid> FK_DeletedByUser { get; set; }
    
        public virtual District District { get; set; }
        public virtual Vehicle Vehicle { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PoliceCase_PoliceCaseLaw> PoliceCase_PoliceCaseLaw { get; set; }
        public virtual Upazila Upazila { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PoliceCaseDocument> PoliceCaseDocuments { get; set; }
    }
}
