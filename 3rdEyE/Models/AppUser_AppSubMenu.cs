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
    
    public partial class AppUser_AppSubMenu
    {
        public long PK_AppUser_AppSubMenu { get; set; }
        public System.Guid FK_AppUser { get; set; }
        public long FK_AppSubMenu { get; set; }
        public Nullable<bool> IsAccessible { get; set; }
    
        public virtual AppSubMenu AppSubMenu { get; set; }
        public virtual AppUser AppUser { get; set; }
    }
}
