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
    
    public partial class RequisitionVehicleType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RequisitionVehicleType()
        {
            this.InterCompanyRequisitions = new HashSet<InterCompanyRequisition>();
            this.Requisitions = new HashSet<Requisition>();
            this.VehicleSharings = new HashSet<VehicleSharing>();
            this.VehicleSharingDemands = new HashSet<VehicleSharingDemand>();
        }
    
        public int PK_RequisitionVehicleType { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string Title_English { get; set; }
        public string Title_Bangla { get; set; }
        public string Layer1 { get; set; }
        public string Layer1_english { get; set; }
        public string Layer1_bangla { get; set; }
        public string Layer2 { get; set; }
        public string Layer2_english { get; set; }
        public string Layer2_bangla { get; set; }
        public string Layer3 { get; set; }
        public string Layer3_english { get; set; }
        public string Layer3_bangla { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InterCompanyRequisition> InterCompanyRequisitions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Requisition> Requisitions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VehicleSharing> VehicleSharings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VehicleSharingDemand> VehicleSharingDemands { get; set; }
    }
}
