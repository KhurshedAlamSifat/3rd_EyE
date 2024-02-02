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
    public class MobileRolesController : BaseController
    {
        public ActionResult Index()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var list = bll.db.MobileRoles.AsEnumerable().ToList();
            return View(list);
        }

        public ActionResult Create()
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = new MobileRole();
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(MobileRole model)
        {
            model.FullName = model.FullName.Trim();

            if (bll.db.MobileRoles.Where(m => m.FullName == model.FullName).Any())
            {
                CreateAlertMessage(AlertMessageType.Danger, "Validation Failure", "Another role is already exist wtih this FullName.");
                return View(model);
            }
            model.CreatedAt = DateTime.Now;
            model.FK_CreatedByUser = CurrentUser.PK_User;
            bll.db.MobileRoles.Add(model);
            bll.db.SaveChanges();

            CreateAlertMessage(AlertMessageType.Success, "Success", "Successfully added.");
            return RedirectToAction("Manage", new { id = model.PK_MobileRole });
        }

        public ActionResult Edit(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.MobileRoles.Where(m => m.PK_MobileRole == id).FirstOrDefault();
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
        public ActionResult Edit(MobileRole model)
        {
            model.FullName = model.FullName.Trim();
            if (bll.db.MobileRoles.Where(m => m.PK_MobileRole != model.PK_MobileRole && m.FullName == model.FullName).Any())
            {
                CreateAlertMessage(AlertMessageType.Danger, "Validation Failure", "Another role is already exist wtih this FullName.");
                return View(model);
            }
            var db_model = bll.db.MobileRoles.Where(m => m.PK_MobileRole == model.PK_MobileRole).FirstOrDefault();
            if (db_model == null)
            {
                CreateAlertMessage(AlertMessageType.Danger, "Validation Failure", "Role not found in Database");
                return View(model);
            }
            if (bll.db.MobileRoles.Where(m => m.PK_MobileRole != model.PK_MobileRole && m.FullName == model.FullName).Any())
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
            var model = bll.db.MobileRoles.Where(m => m.PK_MobileRole == id).FirstOrDefault();
            if (model == null)
            {
                CreateAlertMessage(AlertMessageType.Danger, "Validation Failure", "Role not found in Database");
                return RedirectToAction("index");
            }
            else
            {
                List<ManagingTools.MobileMenu> list = (from menu in bll.db.MobileMenus.AsEnumerable()
                                      select new ManagingTools.MobileMenu()
                                      {
                                          PK_MobileMenu = menu.PK_MobileMenu,
                                          FullName = menu.FullName,
                                          VisibleName = menu.VisibleName,
                                          ModelName = menu.ModelName,
                                          Icon = menu.Icon,
                                          Link = menu.Link,
                                          Sequence = bll.db.MobileRole_MobileMenu.Where(m => m.FK_MobileRole == id && m.FK_MobileMenu == menu.PK_MobileMenu).Any() ?
                                          bll.db.MobileRole_MobileMenu.Where(m => m.FK_MobileRole == id && m.FK_MobileMenu == menu.PK_MobileMenu).FirstOrDefault().Sequence : 0,


                                          IsAccessible = bll.db.MobileRole_MobileMenu.Where(m => m.FK_MobileRole == id && m.FK_MobileMenu == menu.PK_MobileMenu && m.IsAccessible == true).Any(),

                                      }
                        ).OrderBy(m => m.FullName).ThenBy(m => m.VisibleName).ToList();

                return View(new Tuple<MobileRole, List<ManagingTools.MobileMenu>>(model, list));
            }
        }
        [HttpPost]
        public ActionResult Manage(FormCollection form)
        {
            var FK_MobileRole = Convert.ToInt64(form["FK_MobileRole"]);
            try
            {
                var MobileMenus = bll.db.MobileMenus.ToList();
                var oldRole_MenuList = bll.db.MobileRole_MobileMenu.Where(m => m.FK_MobileRole == FK_MobileRole).ToList();
                foreach (var menu in MobileMenus)
                {
                    var menuCehckBox = form["menuCehckBox_" + menu.PK_MobileMenu];
                    var menuSequence = form["menuSequence_" + menu.PK_MobileMenu];
                    if (menuCehckBox != null)
                    {
                        var roleMenu = oldRole_MenuList.Where(m => m.FK_MobileMenu == menu.PK_MobileMenu).FirstOrDefault();
                        if (roleMenu != null)
                        {
                            roleMenu.IsAccessible = true;
                            if (!string.IsNullOrEmpty(menuSequence))
                            {
                                roleMenu.Sequence = Convert.ToInt32(menuSequence);
                            }
                            else
                            {
                                roleMenu.Sequence = 0;
                            }

                        }
                        else
                        {
                            roleMenu = new MobileRole_MobileMenu()
                            {
                                FK_MobileRole = FK_MobileRole,
                                FK_MobileMenu = menu.PK_MobileMenu,
                                IsAccessible = true
                            };
                            if (!string.IsNullOrEmpty(menuSequence))
                            {
                                roleMenu.Sequence = Convert.ToInt32(menuSequence);
                            }
                            else
                            {
                                roleMenu.Sequence = 0;
                            }
                            bll.db.MobileRole_MobileMenu.Add(roleMenu);
                        }
                    }
                    else
                    {
                        var roleMenu = oldRole_MenuList.Where(m => m.FK_MobileMenu == menu.PK_MobileMenu).FirstOrDefault();
                        if (roleMenu != null)
                        {
                            roleMenu.IsAccessible = false;
                        }
                    }

                }
                bll.db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "Role permission is successfully updated.");
                return RedirectToAction("Manage", new { id = FK_MobileRole });
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                return RedirectToAction("Manage", new { id = FK_MobileRole });
            }
        }

        public ActionResult View(Int64 id)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var model = bll.db.MobileRoles.Where(m => m.PK_MobileRole == id).FirstOrDefault();
            if (model == null)
            {
                CreateAlertMessage(AlertMessageType.Danger, "Validation Failure", "Role not found in Database");
                return RedirectToAction("index");
            }
            else
            {
                List<ManagingTools.MobileMenu> list = (from menu in bll.db.MobileMenus.AsEnumerable()
                                                       select new ManagingTools.MobileMenu()
                                                       {
                                                           PK_MobileMenu = menu.PK_MobileMenu,
                                                           FullName = menu.FullName,
                                                           VisibleName = menu.VisibleName,
                                                           ModelName = menu.ModelName,
                                                           Icon = menu.Icon,
                                                           Link = menu.Link,
                                                           Sequence = bll.db.MobileRole_MobileMenu.Where(m => m.FK_MobileRole == id && m.FK_MobileMenu == menu.PK_MobileMenu).Any() ?
                                                           bll.db.MobileRole_MobileMenu.Where(m => m.FK_MobileRole == id && m.FK_MobileMenu == menu.PK_MobileMenu).FirstOrDefault().Sequence : 0,


                                                           IsAccessible = bll.db.MobileRole_MobileMenu.Where(m => m.FK_MobileRole == id && m.FK_MobileMenu == menu.PK_MobileMenu && m.IsAccessible == true).Any(),

                                                       }
                        ).OrderBy(m => m.FullName).ThenBy(m => m.VisibleName).ToList();

                return View(new Tuple<MobileRole, List<ManagingTools.MobileMenu>>(model, list));
            }
        }

        public ActionResult Delete(Int64 id)
        {
            MobileRole MobileRole = bll.db.MobileRoles.Find(id);

            var MobileUsers = bll.db.AppUsers.Where(m => m.FK_MobileRole == MobileRole.PK_MobileRole).ToList();
            foreach (var MobileUser in MobileUsers)
            {
                MobileUser.FK_MobileRole = null;
            }

            var MobileRole_MobileMenu = bll.db.MobileRole_MobileMenu.Where(m => m.FK_MobileRole == MobileRole.PK_MobileRole).ToList();
            if (MobileRole_MobileMenu.Count > 0)
            {
                bll.db.MobileRole_MobileMenu.RemoveRange(MobileRole_MobileMenu);
            }

            bll.db.MobileRoles.Remove(MobileRole);
            bll.db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}