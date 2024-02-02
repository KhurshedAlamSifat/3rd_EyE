using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _3rdEyE.Models;

namespace _3rdEyE.ManagingTools
{
    public static class CommonClass
    {


        //# Base Image Directory
        public static string ImageDirectory = @"/__DD_Images/";

        public static double SessionTimeoutMinutes = 30;

        //# Date Format
        public static string ConvertToDateString(DateTime? dateTime)
        {
            return String.Format("{0:dd'/'MM'/'yyyy}", dateTime);
            //dateTime.ToString("dd'/'MM'/'yyyy")
        }

        //# Date Format
        public static string ConvertToDateTimeString(DateTime? dateTime)
        {
            return String.Format("{0:dd/MM/yyyy H:mm}", dateTime);
        }


        //# Date Format
        public static bool IsInvalidImageFormat(string contentType)
        {
            if (contentType.Contains("image/"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //# Set Current User In Session
        public static void SetCurrentUser(AppUser validUser)
        {
            HttpContext.Current.Session[SessionClass.CurrentUser] = validUser;
        }
        //# Get Current User from Session
        public static AppUser GetCurrentUser()
        {
            if (HttpContext.Current.Session[SessionClass.CurrentUser] != null)
            {
                return (AppUser)HttpContext.Current.Session[SessionClass.CurrentUser];
            }
            else
            {
                return null;
            }
        }
        public static void SetRoleMenuListAndRolePermissionsList(AppUser currentUser)
        {
            var db = new DBEnityModelContainer();

            List<WebMenu> RoleMenuListFinal = new List<WebMenu>();

            List<WebMenu> RoleMenuList1 = (from role_menu in db.AppRole_AppMenu.Where(m => m.FK_AppRole == currentUser.FK_AppRole && m.IsAccessible == true)
                                           join menu in db.AppMenus.Where(m => m.IsDeleted != true && m.IsActive != false) on role_menu.FK_AppMenu equals menu.PK_AppMenu
                                           //orderby role_menu.Sequence
                                           select new WebMenu()
                                           {
                                               PK_AppMenu = menu.PK_AppMenu,
                                               FullName = menu.FullName,
                                               VisibleName = menu.VisibleName,
                                               ModelName = menu.ModelName,
                                               Icon = menu.Icon,
                                               Link = menu.Link,
                                               Sequence = menu.Sequence,

                                               SubMenuList = (from submenu in db.AppSubMenus.Where(m => m.FK_AppMenu == menu.PK_AppMenu && m.IsActive == true)
                                                              join role_submenu in db.AppRole_AppSubMenu.Where(m => m.FK_AppRole == currentUser.FK_AppRole && m.IsAccessible == true) on submenu.PK_AppSubMenu equals role_submenu.FK_AppSubMenu
                                                              //orderby submenu.Sequence
                                                              select new WebSubMenu()
                                                              {
                                                                  PK_AppSubMenu = submenu.PK_AppSubMenu,
                                                                  VisibleName = submenu.VisibleName,
                                                                  Link = submenu.Link,
                                                                  Sequence = submenu.Sequence,
                                                              }
                                                    ).OrderBy(m => m.Sequence).ToList(),

                                               PermissionList = (from permission in db.AppPermissions.Where(m => m.FK_AppMenu == menu.PK_AppMenu && m.IsActive == true)
                                                                 join role_permission in db.AppRole_AppPermission.Where(m => m.FK_AppRole == currentUser.FK_AppRole && m.IsAccessible == true) on permission.PK_AppPermission equals role_permission.FK_AppPermission
                                                                 //orderby permission.Sequence
                                                                 select new Permission()
                                                                 {
                                                                     PK_AppPermission = permission.PK_AppPermission,
                                                                     VisibleName = permission.VisibleName,
                                                                     FullName = permission.FullName,
                                                                     Sequence = permission.Sequence,
                                                                 }
                                                ).OrderBy(m => m.Sequence).ToList()
                                           }).OrderBy(m => m.Sequence).ToList();
            RoleMenuListFinal.AddRange(RoleMenuList1);

            List<WebMenu> RoleMenuList2 = (from role_menu in db.AppUser_AppMenu.Where(m => m.FK_AppUser == currentUser.PK_User && m.IsAccessible == true)
                                           join menu in db.AppMenus.Where(m => m.IsDeleted != true && m.IsActive != false) on role_menu.FK_AppMenu equals menu.PK_AppMenu
                                           //orderby role_menu.Sequence
                                           select new WebMenu()
                                           {
                                               PK_AppMenu = menu.PK_AppMenu,
                                               FullName = menu.FullName,
                                               VisibleName = menu.VisibleName,
                                               ModelName = menu.ModelName,
                                               Icon = menu.Icon,
                                               Link = menu.Link,
                                               Sequence = menu.Sequence,

                                               SubMenuList = (from submenu in db.AppSubMenus.Where(m => m.FK_AppMenu == menu.PK_AppMenu && m.IsActive == true)
                                                              join role_submenu in db.AppUser_AppSubMenu.Where(m => m.FK_AppUser == currentUser.PK_User && m.IsAccessible == true) on submenu.PK_AppSubMenu equals role_submenu.FK_AppSubMenu
                                                              //orderby submenu.Sequence
                                                              select new WebSubMenu()
                                                              {
                                                                  PK_AppSubMenu = submenu.PK_AppSubMenu,
                                                                  VisibleName = submenu.VisibleName,
                                                                  Link = submenu.Link,
                                                                  Sequence = submenu.Sequence,
                                                              }
                                                    ).OrderBy(m => m.Sequence).ToList(),

                                               PermissionList = (from permission in db.AppPermissions.Where(m => m.FK_AppMenu == menu.PK_AppMenu && m.IsActive == true)
                                                                 join role_permission in db.AppUser_AppPermission.Where(m => m.FK_AppUser == currentUser.PK_User && m.IsAccessible == true) on permission.PK_AppPermission equals role_permission.FK_AppPermission
                                                                 //orderby permission.Sequence
                                                                 select new Permission()
                                                                 {
                                                                     PK_AppPermission = permission.PK_AppPermission,
                                                                     VisibleName = permission.VisibleName,
                                                                     FullName = permission.FullName,
                                                                     Sequence = permission.Sequence,
                                                                 }
                                                ).OrderBy(m => m.Sequence).ToList()
                                           }).OrderBy(m => m.Sequence).ToList();

            foreach (WebMenu extraWebMenu in RoleMenuList2)
            {
                var existingWebMenu = RoleMenuListFinal.Where(m => m.PK_AppMenu == extraWebMenu.PK_AppMenu).FirstOrDefault();
                if (existingWebMenu != null)
                {
                    if (extraWebMenu.SubMenuList.Count > 0)
                    {
                        existingWebMenu.SubMenuList.AddRange(extraWebMenu.SubMenuList);
                    }
                    if (extraWebMenu.PermissionList.Count > 0)
                    {
                        existingWebMenu.PermissionList.AddRange(extraWebMenu.PermissionList);
                    }
                }
                else
                {
                    RoleMenuListFinal.Add(extraWebMenu);
                }
            }

            HttpContext.Current.Session[SessionClass.RoleMenuList] = RoleMenuListFinal;


            List<Permission> RolePermissionList = RoleMenuListFinal.SelectMany(m => m.PermissionList).Distinct().ToList();

            HttpContext.Current.Session[SessionClass.RolePermissionList] = RolePermissionList;
        }
        public static void _SetRoleMenuListAndRolePermissionsList(AppUser currentUser)
        {
            var db = new DBEnityModelContainer();

            List<WebMenu> RoleMenuListFinal = new List<WebMenu>();

            List<WebMenu> RoleMenuList1 = (from role_menu in db.AppRole_AppMenu.Where(m => m.FK_AppRole == currentUser.FK_AppRole && m.IsAccessible == true)
                                       join menu in db.AppMenus.Where(m => m.IsDeleted != true && m.IsActive != false) on role_menu.FK_AppMenu equals menu.PK_AppMenu
                                       //orderby role_menu.Sequence
                                       select new WebMenu()
                                       {
                                           PK_AppMenu = menu.PK_AppMenu,
                                           FullName = menu.FullName,
                                           VisibleName = menu.VisibleName,
                                           ModelName = menu.ModelName,
                                           Icon = menu.Icon,
                                           Link = menu.Link,
                                           Sequence = menu.Sequence,

                                           SubMenuList = (from submenu in db.AppSubMenus.Where(m => m.FK_AppMenu == menu.PK_AppMenu && m.IsActive == true)
                                                          join role_submenu in db.AppRole_AppSubMenu.Where(m => m.FK_AppRole == currentUser.FK_AppRole && m.IsAccessible == true) on submenu.PK_AppSubMenu equals role_submenu.FK_AppSubMenu
                                                          //orderby submenu.Sequence
                                                          select new WebSubMenu()
                                                          {
                                                              PK_AppSubMenu = submenu.PK_AppSubMenu,
                                                              VisibleName = submenu.VisibleName,
                                                              Link = submenu.Link,
                                                              Sequence = submenu.Sequence,
                                                          }
                                                ).OrderBy(m => m.Sequence).ToList(),

                                           PermissionList = (from permission in db.AppPermissions.Where(m => m.FK_AppMenu == menu.PK_AppMenu && m.IsActive == true)
                                                             join role_permission in db.AppRole_AppPermission.Where(m => m.FK_AppRole == currentUser.FK_AppRole && m.IsAccessible == true) on permission.PK_AppPermission equals role_permission.FK_AppPermission
                                                             //orderby permission.Sequence
                                                             select new Permission()
                                                             {
                                                                 PK_AppPermission = permission.PK_AppPermission,
                                                                 VisibleName = permission.VisibleName,
                                                                 FullName = permission.FullName,
                                                                 Sequence = permission.Sequence,
                                                             }
                                            ).OrderBy(m => m.Sequence).ToList()
                                       }).OrderBy(m => m.Sequence).ToList();
            RoleMenuListFinal.AddRange(RoleMenuList1);

            List<WebMenu> RoleMenuList2 = (from role_menu in db.AppUser_AppMenu.Where(m => m.FK_AppUser == currentUser.PK_User && m.IsAccessible == true)
                                           join menu in db.AppMenus.Where(m => m.IsDeleted != true && m.IsActive != false) on role_menu.FK_AppMenu equals menu.PK_AppMenu
                                           //orderby role_menu.Sequence
                                           select new WebMenu()
                                           {
                                               PK_AppMenu = menu.PK_AppMenu,
                                               FullName = menu.FullName,
                                               VisibleName = menu.VisibleName,
                                               ModelName = menu.ModelName,
                                               Icon = menu.Icon,
                                               Link = menu.Link,
                                               Sequence = menu.Sequence,

                                               SubMenuList = (from submenu in db.AppSubMenus.Where(m => m.FK_AppMenu == menu.PK_AppMenu && m.IsActive == true)
                                                              join role_submenu in db.AppUser_AppSubMenu.Where(m => m.FK_AppUser == currentUser.PK_User && m.IsAccessible == true) on submenu.PK_AppSubMenu equals role_submenu.FK_AppSubMenu
                                                              //orderby submenu.Sequence
                                                              select new WebSubMenu()
                                                              {
                                                                  PK_AppSubMenu = submenu.PK_AppSubMenu,
                                                                  VisibleName = submenu.VisibleName,
                                                                  Link = submenu.Link,
                                                                  Sequence = submenu.Sequence,
                                                              }
                                                    ).OrderBy(m => m.Sequence).ToList(),

                                               PermissionList = (from permission in db.AppPermissions.Where(m => m.FK_AppMenu == menu.PK_AppMenu && m.IsActive == true)
                                                                 join role_permission in db.AppUser_AppPermission.Where(m => m.FK_AppUser == currentUser.PK_User && m.IsAccessible == true) on permission.PK_AppPermission equals role_permission.FK_AppPermission
                                                                 //orderby permission.Sequence
                                                                 select new Permission()
                                                                 {
                                                                     PK_AppPermission = permission.PK_AppPermission,
                                                                     VisibleName = permission.VisibleName,
                                                                     FullName = permission.FullName,
                                                                     Sequence = permission.Sequence,
                                                                 }
                                                ).OrderBy(m => m.Sequence).ToList()
                                           }).OrderBy(m => m.Sequence).ToList();

            RoleMenuListFinal.AddRange(RoleMenuList2);

            HttpContext.Current.Session[SessionClass.RoleMenuList] = RoleMenuListFinal;


            List<Permission> RolePermissionList = RoleMenuListFinal.SelectMany(m => m.PermissionList).Distinct().ToList();

            HttpContext.Current.Session[SessionClass.RolePermissionList] = RolePermissionList;
        }
        public static List<WebMenu> GetRoleMenuList()
        {
            if (HttpContext.Current.Session[SessionClass.RoleMenuList] != null)
            {
                return (List<WebMenu>)HttpContext.Current.Session[SessionClass.RoleMenuList];
            }
            else
            {
                return new List<WebMenu>();
            }
        }
        public static List<Permission> GetRolePermissionList()
        {
            if (HttpContext.Current.Session[SessionClass.RolePermissionList] != null)
            {
                return (List<Permission>)HttpContext.Current.Session[SessionClass.RolePermissionList];
            }
            else
            {
                return new List<Permission>();
            }
        }


        //# Set CurrentLoginID In Session
        public static void SetCurrentLoginID(long CurrentLoginID)
        {
            HttpContext.Current.Session[SessionClass.CurrentLoginID] = CurrentLoginID;
        }
        //# Get CurrentLoginID from Session
        public static long GetCurrentLoginID()
        {
            if (HttpContext.Current.Session[SessionClass.CurrentLoginID] != null)
            {
                return (long)HttpContext.Current.Session[SessionClass.CurrentLoginID];
            }
            else
            {
                return 0;
            }
        }

        //# Set Current User In Session
        public static bool IsInvalidAccess()
        {
            if (HttpContext.Current.Session[SessionClass.CurrentUser] == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public class MenuPermissionItem
        {
            public string AppController_Name { get; set; }
            public string AppAction_Name { get; set; }
            public string AppAction_VisibleName { get; set; }
            public Nullable<bool> AddPermission { get; set; }
            public Nullable<bool> EditPermission { get; set; }
            public Nullable<bool> ViewPermission { get; set; }
            public Nullable<bool> DeletePermission { get; set; }
            public Nullable<bool> EditPermission1 { get; set; }
            public Nullable<bool> EditPermission2 { get; set; }
        }



    }
}