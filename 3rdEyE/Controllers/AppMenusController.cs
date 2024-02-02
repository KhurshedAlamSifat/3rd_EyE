using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _3rdEyE.ManagingTools;
using _3rdEyE.Models;

namespace _3rdEyE.Controllers
{
    public class AppMenusController : BaseController
    {
        private DBEnityModelContainer db = new DBEnityModelContainer();

        public ActionResult Index()
        {
            return View(db.AppMenus.ToList());
        }

        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppMenu appMenu = db.AppMenus.Find(id);
            if (appMenu == null)
            {
                return HttpNotFound();
            }
            return View(appMenu);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PK_AppMenu,FullName,VisibleName,ModelName,Icon,Link,IsDeleted,IsActive,Sequence")] AppMenu appMenu)
        {
            if (ModelState.IsValid)
            {
                db.AppMenus.Add(appMenu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(appMenu);
        }

        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppMenu appMenu = db.AppMenus.Find(id);
            if (appMenu == null)
            {
                return HttpNotFound();
            }
            return View(appMenu);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PK_AppMenu,FullName,VisibleName,ModelName,Icon,Link,IsDeleted,IsActive,Sequence")] AppMenu appMenu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appMenu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(appMenu);
        }


        public ActionResult ManagePermissionAndSubMenu(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppMenu appMenu = db.AppMenus.Find(id);
            if (appMenu == null)
            {
                return HttpNotFound();
            }
            return View(appMenu);
        }
        [HttpPost]
        public ActionResult ManagePermissionAndSubMenu(FormCollection form)
        {
            if (CommonClass.IsInvalidAccess())
            {
                return Redirect("/Access/Login");
            }
            var FK_AppMenu = Convert.ToInt64(form["FK_AppMenu"]);
            try
            {

                var appPermissions = db.AppPermissions.Where(m => m.FK_AppMenu == FK_AppMenu).ToList();
                foreach (var item in appPermissions)
                {
                    var sequence_text = form["AppPermission_Sequence_" + item.PK_AppPermission.ToString()];
                    if (string.IsNullOrEmpty(sequence_text))
                    {
                        item.IsActive = false;
                        item.Sequence = null;
                    }
                    else
                    {
                        item.IsActive = true;
                        item.Sequence = Convert.ToInt32(sequence_text);
                    }
                }

                var appSubMenus = db.AppSubMenus.Where(m => m.FK_AppMenu == FK_AppMenu).ToList();
                foreach (var item in appSubMenus)
                {
                    var sequence_text = form["AppSubMenu_Sequence_" + item.PK_AppSubMenu.ToString()];
                    if (string.IsNullOrEmpty(sequence_text))
                    {
                        item.IsActive = false;
                        item.Sequence = null;
                    }
                    else
                    {
                        item.IsActive = true;
                        item.Sequence = Convert.ToInt32(sequence_text);
                    }
                }

                db.SaveChanges();
                CreateAlertMessage(AlertMessageType.Success, "Success", "Menu-SubMenu Link is successfully updated.");
                return RedirectToAction("ManagePermissionAndSubMenu", new { id = FK_AppMenu });
            }
            catch (Exception exception)
            {
                CreateAlertMessage(AlertMessageType.Warning, "Warning", exception.Message);
                return RedirectToAction("ManagePermissionAndSubMenu", new { id = FK_AppMenu });
            }
        }

        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppMenu appMenu = db.AppMenus.Find(id);
            if (appMenu == null)
            {
                return HttpNotFound();
            }
            return View(appMenu);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            AppMenu appMenu = db.AppMenus.Find(id);

            var appRole_AppMenu = db.AppRole_AppMenu.Where(m => m.FK_AppMenu == appMenu.PK_AppMenu).ToList();
            if (appRole_AppMenu.Count > 0)
            {
                db.AppRole_AppMenu.RemoveRange(appRole_AppMenu);
            }

            var appSubMenuList = db.AppSubMenus.Where(m => m.FK_AppMenu == appMenu.PK_AppMenu).ToList();
            if (appSubMenuList.Count > 0)
            {
                foreach (var item in appSubMenuList)
                {
                    var appRole_AppSubMenu = db.AppRole_AppSubMenu.Where(m => m.FK_AppSubMenu == item.PK_AppSubMenu).ToList();
                    if (appRole_AppSubMenu.Count > 0)
                    {
                        db.AppRole_AppSubMenu.RemoveRange(appRole_AppSubMenu);
                    }
                }
                db.AppSubMenus.RemoveRange(appSubMenuList);
            }

            var appPermissionList = db.AppPermissions.Where(m => m.FK_AppMenu == appMenu.PK_AppMenu).ToList();
            if (appPermissionList.Count > 0)
            {
                foreach (var item in appPermissionList)
                {
                    var appRole_AppPermission = db.AppRole_AppPermission.Where(m => m.FK_AppPermission == item.PK_AppPermission).ToList();
                    if (appRole_AppPermission.Count > 0)
                    {
                        db.AppRole_AppPermission.RemoveRange(appRole_AppPermission);
                    }
                }
            }

            db.AppMenus.Remove(appMenu);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
