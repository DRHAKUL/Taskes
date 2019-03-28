using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Tasques.Models;

namespace Tasques.Controllers
{
    public class GROUPSController : Controller
    {
        private tasquesEntities db = new tasquesEntities();

        // GET: GROUPS
        public ActionResult Index()
        {
            return View(db.GROUPS.ToList());
        }

        // GET: GROUPS/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GROUPS gROUPS = db.GROUPS.Find(id);
            if (gROUPS == null)
            {
                return HttpNotFound();
            }
            return View(gROUPS);
        }

        // GET: GROUPS/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GROUPS/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDGROUP,GROUPNAME")] GROUPS gROUPS)
        {
            if (ModelState.IsValid)
            {
                db.GROUPS.Add(gROUPS);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gROUPS);
        }

        // GET: GROUPS/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GROUPS gROUPS = db.GROUPS.Find(id);
            if (gROUPS == null)
            {
                return HttpNotFound();
            }
            return View(gROUPS);
        }

        // POST: GROUPS/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDGROUP,GROUPNAME")] GROUPS gROUPS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gROUPS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gROUPS);
        }

        // GET: GROUPS/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GROUPS gROUPS = db.GROUPS.Find(id);
            if (gROUPS == null)
            {
                return HttpNotFound();
            }
            return View(gROUPS);
        }

        // POST: GROUPS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GROUPS gROUPS = db.GROUPS.Find(id);
            db.GROUPS.Remove(gROUPS);
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
