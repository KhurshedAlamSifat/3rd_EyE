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
    
    public partial class ContructualRequisitionCompany
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ContructualRequisitionCompany()
        {
            this.AppUsers = new HashSet<AppUser>();
            this.AppUserSurpervisedContructualCompanies = new HashSet<AppUserSurpervisedContructualCompany>();
            this.ContructualRequisitions = new HashSet<ContructualRequisition>();
        }
    
        public long RowSerial { get; set; }
        public System.Guid PK_ContructualRequisitionCompany { get; set; }
        public bool IsDeleted { get; set; }
        public string Name { get; set; }
        public string RegistrationNumber { get; set; }
        public string ContactNumber { get; set; }
        public string ContactAddress { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AppUser> AppUsers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AppUserSurpervisedContructualCompany> AppUserSurpervisedContructualCompanies { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContructualRequisition> ContructualRequisitions { get; set; }
    }
}
