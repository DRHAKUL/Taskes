using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Tasques.Models;

namespace Tasques.Controllers
{
    
    public class UsuariController : Controller
    {
        private static tasquesEntities db = new tasquesEntities();
        private static List<USERTASK> llistaUserTask = db.USERTASK.ToList();
        private static List<TASKS> llistaTask = db.TASKS.ToList();
        private static int idUser = 1;
        // GET: Usuari
        public ActionResult Index()
        {
       
            return View();
        }
        [HttpPost]
        public object getUserTasks()
        {
            foreach(var task in llistaUserTask)
            {
                var a = task.IDTASK;
            }
            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(llistaUserTask);
            return json;
        }
        [HttpPost]
        public object getTasks()
        {
            Dictionary<int,string> dict = new Dictionary<int,string>();
            foreach (var t in llistaTask)
            {
                foreach(var l in llistaUserTask)
                {
                    if (idUser == l.IDUSER && t.IDTASK == l.IDTASK)
                    {
                        dict.Add(t.IDTASK, t.NAME);
                    }
                }
               
            }
            return JsonConvert.SerializeObject(dict);
        }
    }
}