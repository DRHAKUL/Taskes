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
using static Tasques.Utils.Utils;

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
        public class LogUser
        {
            public string name { get; set; }
            public string password { get; set; }
        }
        [HttpPost]
        public ActionResult signIn(LogUser u)
        {
            HttpCookie cookie = HttpContext.Request.Cookies.Get("TOKEN");
            if (cookie == null)
            {

                CurrentUser good = UtilsToken.isValidUser(u.name, u.password);
                if (good == null)
                {
                    TempData["msg"] = "<script>alert('Usuari o contrasenya no valids');</script>";
                    return RedirectToAction("Login", "USERS");
                }
                else
                {
                    HttpCookie mycookie = new HttpCookie("TOKEN");
                    mycookie.Value = good.token;
                    var response = HttpContext.Response.Cookies;
                    HttpContext.Response.Cookies.Remove("TOKEN");
                    HttpContext.Response.Cookies.Add(mycookie);
                    if (good.rol == 1)
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("Index", "TASKS");
                    }

                }


            }
            else
            {
                return RedirectToAction("Index", "TASKS");
            }
        }
        public ActionResult Login()
        {
            HttpCookie cookie = HttpContext.Request.Cookies.Get("TOKEN");
            if (cookie == null)
            {

                return View();

            }
            else
            {
                CurrentUser u = UtilsToken.isValidToken(cookie.Value);
                if (u != null)
                {
                    return RedirectToAction("Index", "TASKS");
                }
                else
                {
                    return View();
                }

            }


        }
        // GET: TASKS
        private List<TaskUG> GetUserTask(HttpCookie cookie)
        {

            int idUser = UtilsToken.getUserId(cookie.Value);
            List<TaskUG> taskUg = new List<TaskUG>();
            var userGroup = (from g in db.GROUPS
                             join ug in db.USERGROUP on g.IDGROUP equals ug.IDGROUP
                             where ug.IDUSER == idUser
                             select g.IDGROUP).ToList();

            var userTask = (from t in db.TASKS
                            join uta in db.USERTASK on t.IDTASK equals uta.IDTASK
                            where uta.IDUSER == idUser
                            select new { idtask = uta.IDUSERTASK, name = t.NAME, tasques = uta.TASK });

            foreach (var ut in userTask)
            {

                TASKS t = new TASKS
                {
                    IDTASK = ut.idtask,
                    NAME = ut.name,
                    Tasques = ut.tasques
                };
                taskUg.Add(new TaskUG(t, "U"));
            }
            foreach (var g in userGroup)
            {
                var groupTask = (from t in db.TASKS
                                 join ut in db.USERTASK on t.IDTASK equals ut.IDTASK
                                 join gr in db.GROUPS on ut.IDUSER equals gr.IDGROUP
                                 where ut.IDUSER == g
                                 select new { idtask = t.IDTASK, name = t.NAME, tasques = t.Tasques, nomgrup = gr.GROUPNAME });
                foreach (var gt in groupTask)
                {
                    TASKS t = new TASKS
                    {
                        IDTASK = gt.idtask,
                        NAME = gt.name,
                        Tasques = gt.tasques
                    };
                    taskUg.Add(new TaskUG(t, gt.nomgrup));
                }
            }
            return taskUg;
        }

        public ActionResult Index()
        {
            HttpCookie cookie = HttpContext.Request.Cookies.Get("TOKEN");
            if (cookie == null)
            {
                return RedirectToAction("Login", "TASKS");
            }

            return View(GetUserTask(cookie));
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
            USERTASK uASKS = db.USERTASK.FirstOrDefault(x=>x.IDUSERTASK == id);
            var name = (from t in db.TASKS
                        where t.IDTASK == uASKS.IDTASK
                        select t.NAME).ToList();
            uASKS.USUARIGRUP = name[0];
            if (uASKS == null)
            {
                return HttpNotFound();
            }
            return View(uASKS);
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

        //http://localhost:51684/TASKS/Edit/ChangeState?posPart=1&nomPart=test1&idTasks=1008
        [HttpPost]
        public ActionResult ChangeState()
        {
            string valor = Request.Params["valor"];
            int idTask = 0;
            string tascaCanviada = "";
            if (!string.IsNullOrEmpty(valor))
            {
                string nomPart = valor.Split('+')[0];
                string posPart = valor.Split('+')[1];
                idTask = Int32.Parse(valor.Split('+')[2]);
                var userTask = (from t in db.USERTASK
                                where t.IDUSERTASK == idTask
                                select t).ToList();
                int contador = 0;


                Dictionary<int, Part> dictioTask = Utils.Utils.JsonToDic(userTask[0].TASK);
                string valorPart = dictioTask[Int32.Parse(posPart)].valor;
                if (valorPart == "1")
                {
                    dictioTask[Int32.Parse(posPart)].valor = "0";
                }
                else if(valorPart == "0")
                {
                    dictioTask[Int32.Parse(posPart)].valor = "1";
                }
                else
                {
                    dictioTask[Int32.Parse(posPart)].valor = "0";
                }
                tascaCanviada = Utils.Utils.DicToJson(dictioTask);
            }
        
            var result = db.USERTASK.SingleOrDefault(b => b.IDUSERTASK == idTask);
            if (result != null)
            {
                result.TASK = tascaCanviada;
                db.SaveChanges();
            }
            return RedirectToAction(idTask.ToString(), "TASKS/Edit");
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
