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
    
    public partial class MobileMenu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MobileMenu()
        {
            this.MobileRole_MobileMenu = new HashSet<MobileRole_MobileMenu>();
        }
    
        public long PK_MobileMenu { get; set; }
        public string FullName { get; set; }
        public string VisibleName { get; set; }
        public string ModelName { get; set; }
        public string Icon { get; set; }
        public string Link { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsActive { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MobileRole_MobileMenu> MobileRole_MobileMenu { get; set; }
    }
}
