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
    
    public partial class RequisitionAgentNotification
    {
        public long PK_RequisitionAgentNotification { get; set; }
        public Nullable<System.Guid> FK_RequisitionAgent { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string ViewLink { get; set; }
        public int Status { get; set; }
        public string Category { get; set; }
    
        public virtual AppUser AppUser { get; set; }
    }
}