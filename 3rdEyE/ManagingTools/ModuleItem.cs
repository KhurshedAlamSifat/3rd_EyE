using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3rdEyE.ManagingTools
{
    public class ModuleItem
    {
        public string AppModule_Name { get; set; }
        public string AppModule_Icon { get; set; }
        public string AppModule_Link { get; set; }
        public List<MenuItem> MenuList { get; set; }
        //public string AppController_Name { get; set; }
        //public string AppAction_Name { get; set; }
        //public string AppAction_VisibleName { get; set; }

    }
    public class MenuItem
    {
        public string AppController_Name { get; set; }
        public string AppAction_Name { get; set; }
        public string AppAction_VisibleName { get; set; }
        public Nullable<int> Sequence { get; set; }
    }

    public class MobileMenu
    {
        public long PK_MobileMenu { get; set; }
        public string FullName { get; set; }
        public string VisibleName { get; set; }
        public string ModelName { get; set; }
        public string Icon { get; set; }
        public string Link { get; set; }
        //public Nullable<bool> IsDeleted { get; set; }
        //public Nullable<bool> IsActive { get; set; }

        //For View Only
        public Nullable<bool> IsAccessible { get; set; }
        public Nullable<int> Sequence { get; set; }

        public List<WebSubMenu> SubMenuList { get; set; }
        public List<Permission> PermissionList { get; set; }
    }
    public class WebMenu
    {
        public long PK_AppMenu { get; set; }
        public string FullName { get; set; }
        public string VisibleName { get; set; }
        public string ModelName { get; set; }
        public string Icon { get; set; }
        public string Link { get; set; }
        //public Nullable<bool> IsDeleted { get; set; }
        //public Nullable<bool> IsActive { get; set; }

        //For View Only
        public Nullable<bool> IsAccessible { get; set; }
        public Nullable<int> Sequence { get; set; }

        public List<WebSubMenu> SubMenuList { get; set; }
        public List<Permission> PermissionList { get; set; }
    }

    public class WebSubMenu
    {
        public long PK_AppSubMenu { get; set; }
        //public long FK_AppMenu { get; set; }
        public string VisibleName { get; set; }
        public string Link { get; set; }
        public Nullable<int> Sequence { get; set; }
        //public Nullable<bool> IsActive { get; set; }

        //For View Only
        public Nullable<bool> IsAccessible { get; set; }//For View Only
        public Nullable<bool> IsActiveParent { get; set; }//For View Only
    }
    public class Permission
    {
        public long PK_AppPermission { get; set; }
        //public long FK_AppMenu { get; set; }
        public string VisibleName { get; set; }
        public string FullName { get; set; }
        public Nullable<int> Sequence { get; set; }
        //public Nullable<bool> IsActive { get; set; }

        //For View Only
        public Nullable<bool> IsAccessible { get; set; }//For View Only
        public Nullable<bool> IsActiveParent { get; set; }//For View Only
    }
}