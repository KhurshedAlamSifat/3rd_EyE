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
    
    public partial class Event
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Event()
        {
            this.EventDocuments = new HashSet<EventDocument>();
        }
    
        public long RowSerial { get; set; }
        public System.Guid PK_Event { get; set; }
        public string DevelopersNote { get; set; }
        public bool IsDeleted { get; set; }
        public System.Guid FK_Vehicle { get; set; }
        public string FK_EventType { get; set; }
        public string OtherEventTypeDetail { get; set; }
        public Nullable<System.DateTime> IssueDate { get; set; }
        public Nullable<System.DateTime> DepositDate { get; set; }
        public Nullable<bool> IsAlertable { get; set; }
        public Nullable<System.DateTime> ExpirationDate { get; set; }
        public Nullable<System.DateTime> AlertDate { get; set; }
        public Nullable<bool> AlertOn { get; set; }
        public Nullable<System.DateTime> RenewedOn { get; set; }
        public Nullable<System.Guid> PK_RenewedEvent { get; set; }
        public Nullable<System.Guid> FK_AppUser_RenewedBy { get; set; }
        public string PolicyNumber { get; set; }
        public Nullable<decimal> PrimaryAmount { get; set; }
        public Nullable<decimal> AdvancedIncomeTax { get; set; }
        public Nullable<decimal> PremiumAmount { get; set; }
        public Nullable<decimal> SupplementaryDutyAmount { get; set; }
        public Nullable<decimal> FinancialAssistanceFund { get; set; }
        public Nullable<decimal> DigitalRegistrationFee { get; set; }
        public Nullable<decimal> HirePurchase { get; set; }
        public Nullable<decimal> WithdrawHirePurchaseAmount { get; set; }
        public Nullable<decimal> FineAmount { get; set; }
        public Nullable<decimal> AdditionalAmount { get; set; }
        public Nullable<decimal> OtherAmount { get; set; }
        public string OtherNote { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public string StatusText { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<System.DateTime> IssuedAt { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public Nullable<System.DateTime> DeletedAt { get; set; }
        public Nullable<System.Guid> FK_CreatedByUser { get; set; }
        public Nullable<System.Guid> FK_IssuedByUser { get; set; }
        public Nullable<System.Guid> FK_UpdatedByUser { get; set; }
        public Nullable<System.Guid> FK_DeletedByUser { get; set; }
    
        public virtual EventType EventType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EventDocument> EventDocuments { get; set; }
        public virtual Vehicle Vehicle { get; set; }
    }
}
