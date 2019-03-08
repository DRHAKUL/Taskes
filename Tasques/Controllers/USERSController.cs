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
    public class USERSController : Controller
    {
        private tasquesEntities db = new tasquesEntities();
        public class LogUser{
            public string  name { get; set; }
            public string password { get; set; }
        }
        // GET: USERS
        public ActionResult Index()
        {

            return View(db.USERS.ToList());
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
                CurrentUser u =  UtilsToken.isValidToken(cookie.Value);
                if (u != null)
                {
                    return RedirectToAction("Index", "USERS");
                }
                else
                {
                    return View();
                }
                
            }
            
            
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
                        return RedirectToAction("Index", "Usuari");
                    }
                    
                }
               
                
            }
            else
            {
                return View();
            }
        }
      
        // GET: USERS/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USERS uSERS = db.USERS.Find(id);
            if (uSERS == null)
            {
                return HttpNotFound();
            }
            return View(uSERS);
        }

        // GET: USERS/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: USERS/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDUSER,USERNAME,NAME,PASSWORD,SURNAMES")] USERS uSERS)
        {
            if (ModelState.IsValid)
            {
                db.USERS.Add(uSERS);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(uSERS);
        }

        // GET: USERS/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USERS uSERS = db.USERS.Find(id);
            if (uSERS == null)
            {
                return HttpNotFound();
            }
            return View(uSERS);
        }

        // POST: USERS/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDUSER,USERNAME,NAME,PASSWORD,SURNAMES")] USERS uSERS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(uSERS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(uSERS);
        }

        // GET: USERS/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USERS uSERS = db.USERS.Find(id);
            if (uSERS == null)
            {
                return HttpNotFound();
            }
            return View(uSERS);
        }

        // POST: USERS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            USERS uSERS = db.USERS.Find(id);
            db.USERS.Remove(uSERS);
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
