using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SFLOnline.DAL;
using SFLOnline.Models;

namespace SFLOnline.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ExitsController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Exits
        public ActionResult Index()
        {
            return View(db.Exits.ToList());
        }

        // GET: Exits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exit exit = db.Exits.Find(id);
            if (exit == null)
            {
                return HttpNotFound();
            }
            return View(exit);
        }

        // GET: Exits/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Exits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ExitId,ExitName,ForEnrollment")] Exit exit)
        {
            if (ModelState.IsValid)
            {
                if (ControlEnrollment(exit)) { 
                db.Exits.Add(exit);
                db.SaveChanges();
                return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorMessage = "Already have Exit Exam ForEnrollment";
                }
            }

            return View(exit);
        }

        public Boolean ControlEnrollment(Exit exit)
        {
            var exits = db.Exits.FirstOrDefault(i => i.ForEnrollment == true);
            if(exit.ForEnrollment==true && exits == null)
            {
                return true;
            }
            else if(exit.ForEnrollment==true && exits != null)
            {
                return false;
            }
            return true;

        }

        // GET: Exits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exit exit = db.Exits.Find(id);
            if (exit == null)
            {
                return HttpNotFound();
            }
            return View(exit);
        }

        // POST: Exits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ExitId,ExitName,ForEnrollment")] Exit exit)
        {
            if (ModelState.IsValid)
            {
                if (ControlEnrollment(exit)) {
                    var ex = db.Exits.FirstOrDefault(i => i.ExitId == exit.ExitId);
                    ex.ExitName = exit.ExitName;
                    ex.ForEnrollment = exit.ForEnrollment;
                    db.SaveChanges();
                return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorMessage = "Already have Exit Exam ForEnrollment";
                }
            }
            return View(exit);
        }

        // GET: Exits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exit exit = db.Exits.Find(id);
            if (exit == null)
            {
                return HttpNotFound();
            }
            return View(exit);
        }

        // POST: Exits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Exit exit = db.Exits.Find(id);
            db.Exits.Remove(exit);
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
