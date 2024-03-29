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
    
    public partial class VehicleSharingBidding
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VehicleSharingBidding()
        {
            this.VehicleSharingBidding1 = new HashSet<VehicleSharingBidding>();
            this.VehicleSharingExternalTrips = new HashSet<VehicleSharingExternalTrip>();
        }
    
        public long PK_VehicleSharingBidding { get; set; }
        public long FK_VehicleSharing { get; set; }
        public System.Guid FK_RequisitionAgent_Bidder { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public int ManagableQuantity { get; set; }
        public Nullable<long> PricePerQuantity { get; set; }
        public Nullable<int> ApprovedQuantity { get; set; }
        public Nullable<int> Status_ { get; set; }
        public string StatusText { get; set; }
        public string BidderNote { get; set; }
        public Nullable<System.DateTime> BiddedAt { get; set; }
        public Nullable<System.DateTime> VerifiedAt { get; set; }
        public Nullable<int> BidderRating { get; set; }
        public string BidderRatingNote { get; set; }
        public Nullable<long> FK_VehicleSharingBidding_LessPriced { get; set; }
        public string ApprovalMessage { get; set; }
        public string ApprovalNote { get; set; }
    
        public virtual AppUser AppUser { get; set; }
        public virtual VehicleSharing VehicleSharing { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VehicleSharingBidding> VehicleSharingBidding1 { get; set; }
        public virtual VehicleSharingBidding VehicleSharingBidding2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VehicleSharingExternalTrip> VehicleSharingExternalTrips { get; set; }
    }
}
