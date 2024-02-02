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
    public class AppPermissionsController : BaseController
    {
        private DBEnityModelContainer db = new DBEnityModelContainer();

        // GET: AppPermissions
        public ActionResult Index()
        {
            var appPermissions = db.AppPermissions.Include(a => a.AppMenu);
            return View(appPermissions.ToList());
        }

        // GET: AppPermissions/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppPermission appPermission = db.AppPermissions.Find(id);
            if (appPermission == null)
            {
                return HttpNotFound();
            }
            return View(appPermission);
        }

        // GET: AppPermissions/Create
        public ActionResult Create(Int64? id)
        {
            ViewBag.FK_AppMenu = new SelectList(db.AppMenus, "PK_AppMenu", "FullName", id);
            return View();
        }

        // POST: AppPermissions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PK_AppPermission,FK_AppMenu,FullName,VisibleName,Sequence")] AppPermission appPermission)
        {
            if (ModelState.IsValid)
            {
                if (appPermission.Sequence != null)
                {
                    appPermission.IsActive = true;
                }
                else
                {
                    appPermission.IsActive = false;
                }
                db.AppPermissions.Add(appPermission);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FK_AppMenu = new SelectList(db.AppMenus, "PK_AppMenu", "FullName", appPermission.FK_AppMenu);
            return View(appPermission);
        }

        // GET: AppPermissions/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppPermission appPermission = db.AppPermissions.Find(id);
            if (appPermission == null)
            {
                return HttpNotFound();
            }
            ViewBag.FK_AppMenu = new SelectList(db.AppMenus, "PK_AppMenu", "FullName", appPermission.FK_AppMenu);
            return View(appPermission);
        }

        // POST: AppPermissions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PK_AppPermission,FK_AppMenu,FullName,VisibleName,Sequence")] AppPermission appPermission)
        {
            if (ModelState.IsValid)
            {
                if (appPermission.Sequence != null)
                {
                    appPermission.IsActive = true;
                }
                else
                {
                    appPermission.IsActive = false;
                }
                db.Entry(appPermission).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FK_AppMenu = new SelectList(db.AppMenus, "PK_AppMenu", "FullName", appPermission.FK_AppMenu);
            return View(appPermission);
        }

        // GET: AppPermissions/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppPermission appPermission = db.AppPermissions.Find(id);
            if (appPermission == null)
            {
                return HttpNotFound();
            }
            return View(appPermission);
        }

        // POST: AppPermissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            AppPermission appPermission = db.AppPermissions.Find(id);
            var appRole_AppPermission = appPermission.AppRole_AppPermission.ToList();
            if (appRole_AppPermission.Count > 0)
            {
                db.AppRole_AppPermission.RemoveRange(appRole_AppPermission);
            }
            db.AppPermissions.Remove(appPermission);
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
