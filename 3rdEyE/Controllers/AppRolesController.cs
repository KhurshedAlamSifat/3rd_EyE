using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _3rdEyE.Models;
using _3rdEyE.BLL;
using _3rdEyE.ManagingTools;
using _3rdEyE.BLLs;
using _3rdEyE.ViewModels;
using Microsoft.Reporting.WebForms;

namespace _3rdEyE.Controllers
{
    public class AppRolesController : BaseController
    {
        public ActionResult Index()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var list = bll.db.AppRoles.AsEnumerable().ToList();
            return View(list);
        }

        public ActionResult Create()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = new AppRole();
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(AppRole model)
        {
            model.FullName = model.FullName.Trim();

            if (bll.db.AppRoles.Where(m => m.FullName == model.FullName).Any())
            {
                CreateAlertMessage(AlertMessageType.Danger, "Validation Failure", "Another role is already exist wtih this FullName.");
                return View(model);
            }
            model.CreatedAt = DateTime.Now;
            model.FK_CreatedByUser = CurrentUser.PK_User;
            bll.db.AppRoles.Add(model);
            bll.db.SaveChanges();

            CreateAlertMessage(AlertMessageType.Success, "Success", "Successfully added.");
            return RedirectToAction("Manage", new { id = model.PK_AppRole });
        }

        public ActionResult Edit(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.AppRoles.Where(m => m.PK_AppRole == id).FirstOrDefault();
            if (model == null)
            {
                CreateAlertMessage(AlertMessageType.Danger, "Validation Failure", "Role not found in Database");
                return RedirectToAction("index");
            }
            else
            {
                return View(model);
            }
        }
        [HttpPost]
        public ActionResult Edit(AppRole model)
        {
            model.FullName = model.FullName.Trim();
            if (bll.db.AppRoles.Where(m => m.PK_AppRole != model.PK_AppRole && m.FullName == model.FullName).Any())
            {
                CreateAlertMessage(AlertMessageType.Danger, "Validation Failure", "Another role is already exist wtih this FullName.");
                return View(model);
            }
            var db_model = bll.db.AppRoles.Where(m => m.PK_AppRole == model.PK_AppRole).FirstOrDefault();
            if (db_model == null)
            {
                CreateAlertMessage(AlertMessageType.Danger, "Validation Failure", "Role not found in Database");
                return View(model);
            }
            if (bll.db.AppRoles.Where(m => m.PK_AppRole != model.PK_AppRole && m.FullName == model.FullName).Any())
            {
                CreateAlertMessage(AlertMessageType.Danger, "Validation Failure", "Another role is already exist wtih this FullName.");
                return View(model);
            }
            db_model.UpdatedAt = DateTime.Now;
            db_model.FK_UpdatedByUser = CurrentUser.PK_User;

            db_model.FullName = model.FullName;
            db_model.Note = model.Note;
            db_model.IsActive = model.IsActive;
            db_model.IsDeleted = model.IsDeleted;
            bll.db.SaveChanges();

            CreateAlertMessage(AlertMessageType.Success, "Success", "Successfully updated.");
            return RedirectToAction("index");
        }


        public ActionResult Manage(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.AppRoles.Where(m => m.PK_AppRole == id).FirstOrDefault();
            if (model == null)
            {
                CreateAlertMessage(AlertMessageType.Danger, "Validation Failure", "Role not found in Database");
                return RedirectToAction("index");
            }
            else
            {
                List<WebMenu> list = (from menu in bll.db.AppMenus.AsEnumerable()
                                   select new WebMenu()
                                   {
                                       PK_AppMenu = menu.PK_AppMenu,
                                       FullName = menu.FullName,
                                       VisibleName = menu.VisibleName,
                                       ModelName = menu.ModelName,
                                       Icon = menu.Icon,
                                       Link = menu.Link,
                                       Sequence = menu.Sequence,


                                       IsAccessible = bll.db.AppRole_AppMenu.Where(m => m.FK_AppRole == id && m.FK_AppMenu == menu.PK_AppMenu && m.IsAccessible == true).Any(),

                                       SubMenuList = (from subMenu in bll.db.AppSubMenus.Where(m => m.FK_AppMenu == menu.PK_AppMenu)
                                                      select new WebSubMenu()
                                                      {
                                                          PK_AppSubMenu = subMenu.PK_AppSubMenu,
                                                          VisibleName = subMenu.VisibleName,
                                                          Link = subMenu.Link,
                                                          IsAccessible = bll.db.AppRole_AppSubMenu.Where(m => m.FK_AppRole == id && m.FK_AppSubMenu == subMenu.PK_AppSubMenu && m.IsAccessible == true).Any(),
                                                          IsActiveParent = subMenu.IsActive,
                                                          Sequence = subMenu.Sequence,
                                                      }
                                                   ).ToList(),

                                       PermissionList = (from permission in bll.db.AppPermissions.Where(m => m.FK_AppMenu == menu.PK_AppMenu)
                                                         select new Permission()
                                                         {
                                                             PK_AppPermission = permission.PK_AppPermission,
                                                             VisibleName = permission.VisibleName,
                                                             FullName = permission.FullName,
                                                             IsAccessible = bll.db.AppRole_AppPermission.Where(m => m.FK_AppRole == id && m.FK_AppPermission == permission.PK_AppPermission && m.IsAccessible == true).Any(),
                                                             IsActiveParent = permission.IsActive,
                                                             Sequence = permission.Sequence,
                                                         }
                                                   ).ToList()
                                   }
                        ).OrderBy(m => m.FullName).ThenBy(m => m.VisibleName).ToList();

                return View(new Tuple<AppRole, List<WebMenu>>(model, list));
            }
        }
        [HttpPost]
        public ActionResult Manage(FormCollection form)
        {
            var FK_AppRole = Convert.ToInt64(form["FK_AppRole"]);
            try
            {
                var appMenus = bll.db.AppMenus.ToList();
                var oldRole_MenuList = bll.db.AppRole_AppMenu.Where(m => m.FK_AppRole == FK_AppRole).ToList();
                foreach (var menu in appMenus)
                {
                    var menuCehckBox = form["menuCehckBox_" + menu.PK_AppMenu];
                    var menuSequence = form["menuSequence_" + menu.PK_AppMenu];
                    if (menuCehckBox != null)
                    {
                        var roleMenu = oldRole_MenuList.Where(m => m.FK_AppMenu == menu.PK_AppMenu).FirstOrDefault();
                        if (roleMenu != null)
                        {
                            roleMenu.IsAccessible = true;
                            //if (!string.IsNullOrEmpty(menuSequence))
                            //{
                            //    roleMenu.Sequence = Convert.ToInt32(menuSequence);
                            //}
                            //else
                            //{
                            //    roleMenu.Sequence = 0;
                            //}

                            //# Manage Submenu
                            var subMenuList = (from submenu in bll.db.AppSubMenus.Where(m => m.FK_AppMenu == roleMenu.FK_AppMenu)
                                               select submenu).ToList();

                            var oldRoleSubMenuList = (from role_submenu in bll.db.AppRole_AppSubMenu.Where(m => m.FK_AppRole == FK_AppRole)
                                                      join submenu in bll.db.AppSubMenus.Where(m => m.FK_AppMenu == roleMenu.FK_AppMenu) on role_submenu.FK_AppSubMenu equals submenu.PK_AppSubMenu
                                                      select role_submenu).ToList();
                            foreach (var subMenu in subMenuList)
                            {
                                var subMenuCehckBox = form["subMenuCehckBox_" + subMenu.PK_AppSubMenu];
                                if (subMenuCehckBox != null)
                                {
                                    var permittedRoleSubMenu = oldRoleSubMenuList.Where(m => m.FK_AppSubMenu == subMenu.PK_AppSubMenu).FirstOrDefault();
                                    if (permittedRoleSubMenu == null)
                                    {
                                        bll.db.AppRole_AppSubMenu.Add(
                                                new AppRole_AppSubMenu()
                                                {
                                                    FK_AppRole = FK_AppRole,
                                                    FK_AppSubMenu = subMenu.PK_AppSubMenu,
                                                    IsAccessible = true
                                                }
                                            );
                                    }
                                    else
                                    {
                                        permittedRoleSubMenu.IsAccessible = true;
                                    }
                                }
                                else
                                {
                                    var permittedRoleSubMenu = oldRoleSubMenuList.Where(m => m.FK_AppSubMenu == subMenu.PK_AppSubMenu).FirstOrDefault();
                                    if (permittedRoleSubMenu != null)
                                    {
                                        permittedRoleSubMenu.IsAccessible = false;
                                    }
                                }
                            }

                            //# Manage Permission
                            var permissionList = (from permission in bll.db.AppPermissions.Where(m => m.FK_AppMenu == roleMenu.FK_AppMenu)
                                                  select permission).ToList();

                            var oldRolePermissionList = (from role_permission in bll.db.AppRole_AppPermission.Where(m => m.FK_AppRole == FK_AppRole)
                                                         join permission in bll.db.AppPermissions.Where(m => m.FK_AppMenu == roleMenu.FK_AppMenu) on role_permission.FK_AppPermission equals permission.PK_AppPermission
                                                         select role_permission).ToList();
                            foreach (var permission in permissionList)
                            {
                                var permissionCehckBox = form["permissionCehckBox_" + permission.PK_AppPermission];
                                if (permissionCehckBox != null)
                                {
                                    var permittedRolePermission = oldRolePermissionList.Where(m => m.FK_AppPermission == permission.PK_AppPermission).FirstOrDefault();
                                    if (permittedRolePermission == null)
                                    {
                                        bll.db.AppRole_AppPermission.Add(
                                                new AppRole_AppPermission()
                                                {
                                                    FK_AppRole = FK_AppRole,
                                                    FK_AppPermission = permission.PK_AppPermission,
                                                    IsAccessible = true
                                                }
                                            );
                                    }
                                    else
                                    {
                                        permittedRolePermission.IsAccessible = true;
                                    }
                                }
                                else
                                {
                                    var permittedRolePermission = oldRolePermissionList.Where(m => m.FK_AppPermission == permission.PK_AppPermission).FirstOrDefault();
                                    if (permittedRolePermission != null)
                                    {
                                        permittedRolePermission.IsAccessible = false;
                                    }
                                }
                            }
                        }
                        else
                        {
                            roleMenu = new AppRole_AppMenu()
                            {
                                FK_AppRole = FK_AppRole,
                                FK_AppMenu = menu.PK_AppMenu,
                                IsAccessible = true
                            };
                            //if (!string.IsNullOrEmpty(menuSequence))
                            //{
                            //    roleMenu.Sequence = Convert.ToInt32(menuSequence);
                            //}
                            //else
                            //{
                            //    roleMenu.Sequence = 0;
                            //}
                            bll.db.AppRole_AppMenu.Add(roleMenu);

                            //# Manage Submenu
                            var subMenuList = (from submenu in bll.db.AppSubMenus.Where(m => m.FK_AppMenu == menu.PK_AppMenu)
                                               select submenu).ToList();

                            foreach (var subMenu in subMenuList)
                            {
                                var subMenuCehckBox = form["subMenuCehckBox_" + subMenu.PK_AppSubMenu];
                                if (subMenuCehckBox != null)
                                {
                                    bll.db.AppRole_AppSubMenu.Add(
                                        new AppRole_AppSubMenu()
                                        {
                                            FK_AppRole = FK_AppRole,
                                            FK_AppSubMenu = subMenu.PK_AppSubMenu,
                                            IsAccessible = true
                                        });
                                }
                            }

                            //# Manage permission
                            var permissionList = (from permission in bll.db.AppPermissions.Where(m => m.FK_AppMenu == menu.PK_AppMenu)
                                                  select permission).ToList();

                            foreach (var permission in permissionList)
                            {
                                var permissionCehckBox = form["permissionCehckBox_" + permission.PK_AppPermission];
                                if (permissionCehckBox != null)
                                {
                                    bll.db.AppRole_AppPermission.Add(
                                        new AppRole_AppPermission()
                                        {
                                            FK_AppRole = FK_AppRole,
                                            FK_AppPermission = permission.PK_AppPermission,
                                            IsAccessible = true
                                        });
                                }
                            }
                        }
                    }
                    else
                    {
                        var roleMenu = oldRole_MenuList.Where(m => m.FK_AppMenu == menu.PK_AppMenu).FirstOrDefault();
                        if (roleMenu != null)
                        {
                            roleMenu.IsAccessible = false;
                        }
                    }

                }
                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "Role permission is successfully updated.");
                return RedirectToAction("Manage", new { id = FK_AppRole });
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                return RedirectToAction("Manage", new { id = FK_AppRole });
            }
        }

        public ActionResult View(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.AppRoles.Where(m => m.PK_AppRole == id).FirstOrDefault();
            if (model == null)
            {
                CreateAlertMessage(AlertMessageType.Danger, "Validation Failure", "Role not found in Database");
                return RedirectToAction("index");
            }
            else
            {
                List<WebMenu> list = (from menu in bll.db.AppMenus.AsEnumerable()
                                   select new WebMenu()
                                   {
                                       PK_AppMenu = menu.PK_AppMenu,
                                       FullName = menu.FullName,
                                       VisibleName = menu.VisibleName,
                                       ModelName = menu.ModelName,
                                       Icon = menu.Icon,
                                       Link = menu.Link,
                                       Sequence = menu.Sequence,


                                       IsAccessible = bll.db.AppRole_AppMenu.Where(m => m.FK_AppRole == id && m.FK_AppMenu == menu.PK_AppMenu && m.IsAccessible == true).Any(),

                                       SubMenuList = (from subMenu in bll.db.AppSubMenus.Where(m => m.FK_AppMenu == menu.PK_AppMenu)
                                                      select new WebSubMenu()
                                                      {
                                                          PK_AppSubMenu = subMenu.PK_AppSubMenu,
                                                          VisibleName = subMenu.VisibleName,
                                                          Link = subMenu.Link,
                                                          IsAccessible = bll.db.AppRole_AppSubMenu.Where(m => m.FK_AppRole == id && m.FK_AppSubMenu == subMenu.PK_AppSubMenu && m.IsAccessible == true).Any(),
                                                          IsActiveParent = subMenu.IsActive,
                                                          Sequence = subMenu.Sequence,
                                                      }
                                                   ).ToList(),

                                       PermissionList = (from permission in bll.db.AppPermissions.Where(m => m.FK_AppMenu == menu.PK_AppMenu)
                                                         select new Permission()
                                                         {
                                                             PK_AppPermission = permission.PK_AppPermission,
                                                             VisibleName = permission.VisibleName,
                                                             FullName = permission.FullName,
                                                             IsAccessible = bll.db.AppRole_AppPermission.Where(m => m.FK_AppRole == id && m.FK_AppPermission == permission.PK_AppPermission && m.IsAccessible == true).Any(),
                                                             IsActiveParent = permission.IsActive,
                                                             Sequence = permission.Sequence,
                                                         }
                                                   ).ToList()
                                   }
                        ).OrderBy(m => m.FullName).ThenBy(m => m.VisibleName).ToList();

                return View(new Tuple<AppRole, List<WebMenu>>(model, list));
            }
        }

        public ActionResult Delete(Int64 id)
        {
            AppRole appRole = bll.db.AppRoles.Find(id);

            var appUsers = bll.db.AppUsers.Where(m => m.FK_AppRole == appRole.PK_AppRole).ToList();
            foreach (var appUser in appUsers)
            {
                appUser.FK_AppRole = null;
            }

            var appRole_AppMenu = bll.db.AppRole_AppMenu.Where(m => m.FK_AppRole == appRole.PK_AppRole).ToList();
            if (appRole_AppMenu.Count > 0)
            {
                bll.db.AppRole_AppMenu.RemoveRange(appRole_AppMenu);
            }

            var appRole_AppSubMenu = bll.db.AppRole_AppSubMenu.Where(m => m.FK_AppRole == appRole.PK_AppRole).ToList();
            if (appRole_AppSubMenu.Count > 0)
            {
                bll.db.AppRole_AppSubMenu.RemoveRange(appRole_AppSubMenu);
            }

            var appRole_AppPermission = bll.db.AppRole_AppPermission.Where(m => m.FK_AppRole == appRole.PK_AppRole).ToList();
            if (appRole_AppMenu.Count > 0)
            {
                bll.db.AppRole_AppPermission.RemoveRange(appRole_AppPermission);
            }

            bll.db.AppRoles.Remove(appRole);
            bll.db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}