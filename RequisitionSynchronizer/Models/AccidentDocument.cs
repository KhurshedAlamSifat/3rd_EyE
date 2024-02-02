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
    
    public partial class AccidentDocument
    {
        public long PK_AccidentDocument { get; set; }
        public bool IsDeleted { get; set; }
        public long FK_Accident { get; set; }
        public string Title { get; set; }
        public string IdentitficaitonKey { get; set; }
        public string IdentitficaitonValue { get; set; }
        public string ImageLocation { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<System.DateTime> DeletedAt { get; set; }
        public Nullable<System.Guid> FK_CreatedByUser { get; set; }
        public Nullable<System.Guid> FK_DeletedByUser { get; set; }
    
        public virtual Accident Accident { get; set; }
    }
}
