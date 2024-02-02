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
    public class AppSubMenusController : BaseController
    {
        private DBEnityModelContainer db = new DBEnityModelContainer();

        // GET: AppSubMenus
        public ActionResult Index()
        {
            var appSubMenus = db.AppSubMenus.Include(a => a.AppMenu);
            return View(appSubMenus.ToList());
        }

        // GET: AppSubMenus/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppSubMenu appSubMenu = db.AppSubMenus.Find(id);
            if (appSubMenu == null)
            {
                return HttpNotFound();
            }
            return View(appSubMenu);
        }

        // GET: AppSubMenus/Create
        public ActionResult Create(Int64? id)
        {
            ViewBag.FK_AppMenu = new SelectList(db.AppMenus, "PK_AppMenu", "FullName", id);
            return View();
        }

        // POST: AppSubMenus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PK_AppSubMenu,FK_AppMenu,VisibleName,Link,Sequence")] AppSubMenu appSubMenu)
        {
            if (ModelState.IsValid)
            {
                if (appSubMenu.Sequence != null)
                {
                    appSubMenu.IsActive = true;
                }
                else
                {
                    appSubMenu.IsActive = false;
                }
                db.AppSubMenus.Add(appSubMenu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FK_AppMenu = new SelectList(db.AppMenus, "PK_AppMenu", "FullName", appSubMenu.FK_AppMenu);
            return View(appSubMenu);
        }

        // GET: AppSubMenus/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppSubMenu appSubMenu = db.AppSubMenus.Find(id);
            if (appSubMenu == null)
            {
                return HttpNotFound();
            }
            ViewBag.FK_AppMenu = new SelectList(db.AppMenus, "PK_AppMenu", "FullName", appSubMenu.FK_AppMenu);
            return View(appSubMenu);
        }

        // POST: AppSubMenus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PK_AppSubMenu,FK_AppMenu,VisibleName,Link,Sequence")] AppSubMenu appSubMenu)
        {
            if (ModelState.IsValid)
            {
                if (appSubMenu.Sequence != null)
                {
                    appSubMenu.IsActive = true;
                }
                else
                {
                    appSubMenu.IsActive = false;
                }
                db.Entry(appSubMenu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FK_AppMenu = new SelectList(db.AppMenus, "PK_AppMenu", "FullName", appSubMenu.FK_AppMenu);
            return View(appSubMenu);
        }

        // GET: AppSubMenus/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppSubMenu appSubMenu = db.AppSubMenus.Find(id);
            if (appSubMenu == null)
            {
                return HttpNotFound();
            }
            return View(appSubMenu);
        }

        // POST: AppSubMenus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            AppSubMenu appSubMenu = db.AppSubMenus.Find(id);
            var appRole_AppSubMenu = appSubMenu.AppRole_AppSubMenu.ToList();
            if (appRole_AppSubMenu.Count > 0)
            {
                db.AppRole_AppSubMenu.RemoveRange(appRole_AppSubMenu);
            }
            db.AppSubMenus.Remove(appSubMenu);
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
