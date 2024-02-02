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
    
    public partial class IndividualRequisitionBidding
    {
        public long RowSerial { get; set; }
        public System.Guid PK_IndividualRequisitionBidding { get; set; }
        public System.Guid FK_IndividualRequisition { get; set; }
        public System.Guid FK_RequisitionAgent_Bidder { get; set; }
        public Nullable<int> ManagableQuantity { get; set; }
        public Nullable<long> PricePerQuantity { get; set; }
        public Nullable<int> ApprovedQuantity { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> BidderRating { get; set; }
        public string BidderRatingNote { get; set; }
    
        public virtual AppUser AppUser { get; set; }
        public virtual IndividualRequisition IndividualRequisition { get; set; }
    }
}
