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
    public class AdminController : Controller
    {
        private tasquesEntities db = new tasquesEntities();

        // GET: Admin
        public ActionResult Index()
        {
            return View(db.TASKS.ToList());
        }

        // GET: Admin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TASKS tASKS = db.TASKS.Find(id);
            if (tASKS == null)
            {
                return HttpNotFound();
            }
            return View(tASKS);
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult CreateTask(string Name, string TaskList)
        {
            TASKS t = new TASKS
            {
                NAME = Name,
                Tasques = TaskList
            };
            db.TASKS.Add(t);
            db.SaveChanges();
            //if (ModelState.IsValid)
            //{
            //    db.TASKS.Add(tASKS);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            return RedirectToAction("Index");
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TASKS tASKS = db.TASKS.Find(id);
            if (tASKS == null)
            {
                return HttpNotFound();
            }
            return View(tASKS);
        }

        // POST: Admin/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]   
        public ActionResult Edit(string IdTask, string Name, string TaskList)
        {
            TASKS t = new TASKS
            {
                NAME = Name,
                Tasques = TaskList
            };
           
            var task = db.TASKS.Find(Int32.Parse(IdTask));
            task.NAME = t.NAME;
            task.Tasques = t.Tasques;
            db.SaveChanges();
            return RedirectToAction("Index");
          
           
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TASKS tASKS = db.TASKS.Find(id);
            if (tASKS == null)
            {
                return HttpNotFound();
            }
            return View(tASKS);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TASKS tASKS = db.TASKS.Find(id);
            db.TASKS.Remove(tASKS);
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
