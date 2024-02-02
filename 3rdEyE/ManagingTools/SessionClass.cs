using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _3rdEyE.Models;


namespace _3rdEyE.ManagingTools
{
    public static class SessionClass
    {
        public const string CurrentUser = "CurrentUser";
        public const string MenuPermissions = "MenuPermissions";
        public const string RoleMenuList = "RoleMenuList";
        public const string RolePermissionList = "RolePermissionList";
        public const string CurrentLoginID = "CurrentLoginID";
        public const string LastUserEventTime = "LastUserEventTime";
        public const string URLToRedirect = "URLToRedirect";

        public static void ClearTemporarySession()
        {
        }

        public static void ClearAllSession()
        {
            HttpContext.Current.Session[CurrentUser] = null;
            HttpContext.Current.Session[MenuPermissions] = null;
            HttpContext.Current.Session[RoleMenuList] = null;
            HttpContext.Current.Session[RolePermissionList] = null;
            HttpContext.Current.Session[CurrentLoginID] = null;
            HttpContext.Current.Session[LastUserEventTime] = null;
            HttpContext.Current.Session[URLToRedirect] = null;

            ClearTemporarySession();
        }
    }
}