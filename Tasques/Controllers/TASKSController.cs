using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Tasques.Models;
using Tasques.Utils;

namespace Tasques.Controllers
{
    public class TASKSController : Controller
    {
        private tasquesEntities db = new tasquesEntities();
        public class TaskUG
        {
            public TaskUG(TASKS task, string tipo)
            {
                this.task = task;
                this.nomGrup = tipo;
            }

            public TASKS task { get; set; }
            public string nomGrup { get; set; }
        }
        // GET: TASKS
        public ActionResult Index()
        {
            HttpCookie cookie = HttpContext.Request.Cookies.Get("TOKEN");
            List<TaskUG> taskUg = new List<TaskUG>();
            int idUser = UtilsToken.getUserId(cookie.Value);
            var userGroup = (from g in db.GROUPS
                             join ug in db.USERGROUP on g.IDGROUP equals ug.IDGROUP
                             where ug.IDUSER == idUser
                             select g.IDGROUP).ToList();
          
            List<TASKS> userListTask = (from t in db.TASKS
                                           join ut in db.USERTASK on t.IDTASK equals ut.IDTASK
                                           where ut.IDUSER == idUser                                         
                                           select t).ToList();
            foreach (var ul in userListTask)
            {
                taskUg.Add(new TaskUG(ul, "U"));
            }
            foreach (var g in userGroup)
            {
                var groupTask = (from t in db.TASKS
                                join ut in db.USERTASK on t.IDTASK equals ut.IDTASK
                                join gr in db.GROUPS on ut.IDUSER equals gr.IDGROUP
                                where ut.IDUSER == g                                         
                                select new { idtask =t.IDTASK,name=t.NAME, tasques=t.Tasques,nomgrup= gr.GROUPNAME});
                foreach(var gt in groupTask){
                    TASKS t = new TASKS
                    {
                        IDTASK = gt.idtask,
                        NAME = gt.name,
                        Tasques = gt.tasques
                    };
                    taskUg.Add(new TaskUG(t, gt.nomgrup));
                }       
            }
            return View(taskUg);
        }

        // GET: TASKS/Details/5
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

        // GET: TASKS/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TASKS/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDTASK,NAME,Tasques")] TASKS tASKS)
        {
            if (ModelState.IsValid)
            {
                db.TASKS.Add(tASKS);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tASKS);
        }

        // GET: TASKS/Edit/5
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

        // POST: TASKS/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDTASK,NAME,Tasques")] TASKS tASKS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tASKS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tASKS);
        }

        // GET: TASKS/Delete/5
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

        // POST: TASKS/Delete/5
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
