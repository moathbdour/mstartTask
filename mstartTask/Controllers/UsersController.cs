using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mstartTask.Models;

namespace mstartTask.Controllers
{
    public class UsersController : Controller
    {
        private MstartTaskEntities db = new MstartTaskEntities();

        // GET: Users
        public ActionResult Index(string value , string search)
        {
            if(search != null && value != null)
            {
                if (value == "id")
                {
                    int iid = Convert.ToInt32(search);
                    return View(db.Users.Where(x=>x.ID== iid));
                }
                else if(value == "name")
                {
                    return View(db.Users.Where(x=>x.Username.Contains(search)));
                }
                else
                {
                    return View(db.Users.Where(x => x.Email.Contains(search)));
                }
            }
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Username,Email,First_Name,Last_Name,Status,Gender,Date_Of_Birth")] User user)
        {//,Server_DateTime,DateTime_UTC,Update_DateTime_UTC,Username
            if (ModelState.IsValid)
            {
                user.Server_DateTime = DateTime.Now;
                user.DateTime_UTC = DateTime.UtcNow;
                user.Update_DateTime_UTC = DateTime.Now;
                db.Users.Add(user);
               
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Server_DateTime,DateTime_UTC,Username,Email,First_Name,Last_Name,Status,Gender,Date_Of_Birth")] User user, string continues)
        {
            //,Server_DateTime,DateTime_UTC,Update_DateTime_UTC
            if (ModelState.IsValid)
            {
                user.Update_DateTime_UTC= DateTime.Now;
                db.Entry(user).State = EntityState.Modified;

                db.SaveChanges();
                if (continues != null)
                {
                    return RedirectToAction("Edit", new { id = user.ID });
                }
                return RedirectToAction("Index");
            }
          
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            user.Status = 2;
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
