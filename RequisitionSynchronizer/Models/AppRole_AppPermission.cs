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
    
    public partial class AppRole_AppPermission
    {
        public long PK_AppRole_AppPermission { get; set; }
        public long FK_AppRole { get; set; }
        public long FK_AppPermission { get; set; }
        public Nullable<bool> IsAccessible { get; set; }
    
        public virtual AppPermission AppPermission { get; set; }
        public virtual AppRole AppRole { get; set; }
    }
}
