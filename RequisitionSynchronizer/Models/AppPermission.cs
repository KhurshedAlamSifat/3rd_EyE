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
    
    public partial class AppPermission
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AppPermission()
        {
            this.AppRole_AppPermission = new HashSet<AppRole_AppPermission>();
        }
    
        public long PK_AppPermission { get; set; }
        public long FK_AppMenu { get; set; }
        public string VisibleName { get; set; }
        public string FullName { get; set; }
        public Nullable<int> Sequence { get; set; }
        public Nullable<bool> IsActive { get; set; }
    
        public virtual AppMenu AppMenu { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AppRole_AppPermission> AppRole_AppPermission { get; set; }
    }
}
