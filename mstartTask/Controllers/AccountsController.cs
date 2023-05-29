using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mstartTask.Models;
using Newtonsoft.Json.Linq;

namespace mstartTask.Controllers
{
    public class AccountsController : Controller
    {
        private MstartTaskEntities db = new MstartTaskEntities();

        // GET: Accounts
        public ActionResult Index(string value, string search)
        {
            var accounts = db.Accounts.Include(a => a.User);

            if (search != null && value != null)
            {
                int iid = Convert.ToInt32(search);
                if (value == "id")
                {
                   
                    return View(accounts.Where(x=>x.UserID==iid).ToList());
                }
                else if (value == "account")
                {
                    return View(accounts.Where(x => x.ID == iid).ToList());
                }
                else
                {
                    return View(accounts.Where(x => x.Account_Number == search).ToList());
                }
            }
            return View(accounts.ToList());
        }

        // GET: Accounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }
        public class CreateAccountViewModel
        {

            public List<string> CurrencyOptions { get; set; }

        }
        // GET: Accounts/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.Users, "ID", "Username");
            ViewBag.CurrencyOptions = Account.CurrencyOptionsList;


            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Server_DateTime,DateTime_UTC,UserID,Account_Number,Balance,Currency,Status")] Account account)
        {
            ViewBag.CurrencyOptions = Account.CurrencyOptionsList;
            if (ModelState.IsValid)
            {
                account.Server_DateTime = DateTime.Now;
                account.DateTime_UTC = DateTime.UtcNow;
                account.Update_DateTime_UTC = DateTime.UtcNow;
                db.Accounts.Add(account);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.Users, "ID", "Username", account.UserID);
            return View(account);
        }

        // GET: Accounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.Users, "ID", "Username", account.UserID);
            ViewBag.CurrencyOptions = Account.CurrencyOptionsList;
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,UserID,Account_Number,Balance,Currency,Status")] Account account, string continues)
        {
            ViewBag.CurrencyOptions = Account.CurrencyOptionsList;

            if (ModelState.IsValid)
            {
                account.Update_DateTime_UTC = DateTime.Now;
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                if (continues != null)
                {
                    return RedirectToAction("Edit", new { id = account.ID });
                }
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.Users, "ID", "Username", account.UserID);
            return View(account);
        }

        // GET: Accounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Account account = db.Accounts.Find(id);
            account.Status = 2;
           
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
