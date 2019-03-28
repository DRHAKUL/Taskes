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
    public class AssignarTasquesController : Controller
    {
        private tasquesEntities db = new tasquesEntities();
        public class TasquesUsuari
        {
            public int iduserTask { get; set; }
            public int idUsuari { get; set; }
            public string nomUsuari { get; set; }
            public int idTasca { get; set; }
            public string nomTasca { get; set; }
            public string partsTasca { get; set; }
            public string usuariOGrup { get; set; }
        }
        // GET: AssignarTasque

        public List<TasquesUsuari> getTasquesUsuari()
        {
            List<TasquesUsuari> llistaTasquesUsuari = new List<TasquesUsuari>();
            var tasques = (from t in db.USERTASK
                           join u in db.USERS on t.IDUSER equals u.IDUSER
                           join ta in db.TASKS on t.IDTASK equals ta.IDTASK
                           where t.USUARIGRUP == "U"
                           select new { idUserTask = t.IDUSERTASK, idUser = t.IDUSER, nomUser = u.NAME, idTask = ta.IDTASK, nomTask = ta.NAME, parts = ta.Tasques, usuariOGrup = t.USUARIGRUP })
                          .Union(from t in db.USERTASK
                                 join g in db.GROUPS on t.IDUSER equals g.IDGROUP
                                 join ta in db.TASKS on t.IDTASK equals ta.IDTASK
                                 where t.USUARIGRUP == "G"
                                 select new { idUserTask = t.IDUSERTASK, idUser = t.IDUSER, nomUser = g.GROUPNAME, idTask = ta.IDTASK, nomTask = ta.NAME, parts = ta.Tasques, usuariOGrup = t.USUARIGRUP }
                                 );

            foreach (var task in tasques)
            {
                TasquesUsuari tu = new TasquesUsuari();
                tu.iduserTask = task.idUserTask;
                tu.idUsuari = (int)task.idUser;
                tu.nomUsuari = task.nomUser;
                tu.idTasca = task.idTask;
                tu.nomTasca = task.nomTask;
                tu.partsTasca = (string)task.parts;
                tu.usuariOGrup = task.usuariOGrup;
                llistaTasquesUsuari.Add(tu);
            }
            return llistaTasquesUsuari;
        }
        public ActionResult Index()
        {
            
            return View(getTasquesUsuari());
        }

        // GET: AssignarTasque/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USERTASK uSERTASK = db.USERTASK.Find(id);
            if (uSERTASK == null)
            {
                return HttpNotFound();
            }
            return View(uSERTASK);
        }

        // GET: AssignarTasque/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AssignarTasque/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDUSERTASK,IDTASK,IDUSER,TASK")] USERTASK uSERTASK)
        {
            if (ModelState.IsValid)
            {
                db.USERTASK.Add(uSERTASK);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(uSERTASK);
        }

        // GET: AssignarTasque/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USERTASK uSERTASK = db.USERTASK.Find(id);
            if (uSERTASK == null)
            {
                return HttpNotFound();
            }
            return View(uSERTASK);
        }

        // POST: AssignarTasque/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDUSERTASK,IDTASK,IDUSER,TASK")] USERTASK uSERTASK)
        {
            if (ModelState.IsValid)
            {
                db.Entry(uSERTASK).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(uSERTASK);
        }

        // GET: AssignarTasque/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USERTASK uSERTASK = db.USERTASK.Find(id);
            if (uSERTASK == null)
            {
                return HttpNotFound();
            }
            return View(uSERTASK);
        }

        // POST: AssignarTasque/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            USERTASK uSERTASK = db.USERTASK.Find(id);
            db.USERTASK.Remove(uSERTASK);
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
        public class Assignacio
        {
            public int idTask { get; set; }
            public int idUserG { get; set; }
        }
        [HttpPost]
        public ActionResult Assignar()
        {
            var idTasca = Int32.Parse(Request.Params["slTasca"]);
            var id = Request.Params["slusuari"];
          
            
            if (idTasca > 0 && !string.IsNullOrEmpty(id))
            {
                int idUser = Int32.Parse(id.Split(',')[0]);
                string tipo = id.Split(',')[1];
                var finded = db.TASKS.FirstOrDefault(x => x.IDTASK == (idTasca));
                USERTASK nova = new USERTASK
                {
                    IDTASK = idTasca,
                    IDUSER = idUser,
                    TASK = finded.Tasques,
                    USUARIGRUP = tipo

                };
                db.USERTASK.Add(nova);
                db.SaveChanges();
            }
            return View("Index", getTasquesUsuari());
        }
        
    }
}
