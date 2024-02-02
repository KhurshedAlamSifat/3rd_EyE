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
    
    public partial class AppMenu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AppMenu()
        {
            this.AppPermissions = new HashSet<AppPermission>();
            this.AppRole_AppMenu = new HashSet<AppRole_AppMenu>();
            this.AppSubMenus = new HashSet<AppSubMenu>();
            this.AppUser_AppMenu = new HashSet<AppUser_AppMenu>();
        }
    
        public long PK_AppMenu { get; set; }
        public string FullName { get; set; }
        public string VisibleName { get; set; }
        public string ModelName { get; set; }
        public string Icon { get; set; }
        public string Link { get; set; }
        public Nullable<int> Sequence { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsActive { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AppPermission> AppPermissions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AppRole_AppMenu> AppRole_AppMenu { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AppSubMenu> AppSubMenus { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AppUser_AppMenu> AppUser_AppMenu { get; set; }
    }
}