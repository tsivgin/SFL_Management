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
    public class InformationPassedsController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: InformationPasseds
        public ActionResult Index()
        {
            var ınformationPasseds = db.ınformationPasseds.Include(i => i.Exit);
            return View(ınformationPasseds.ToList());
        }

        // GET: InformationPasseds/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InformationPassed informationPassed = db.ınformationPasseds.Find(id);
            if (informationPassed == null)
            {
                return HttpNotFound();
            }
            return View(informationPassed);
        }

        // GET: InformationPasseds/Create
        public ActionResult Create()
        {
            var exits = db.Exits.ToList();
            List<Exit> exit = new List<Exit>();
            foreach (var item in exits)
            {
                var control = db.ınformationPasseds.FirstOrDefault(i => i.ExitId == item.ExitId);
                var control2 = db.ınformationPasseds.FirstOrDefault();
                if (control == null && control2 == null)
                {
                    exit.Add(item);
                }
                else
                {
                    ViewBag.control = "hide";
                }
            }
            ViewBag.ExitId = new SelectList(exit, "ExitId", "ExitName");
            return View();
        }

        // POST: InformationPasseds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InformationPassedId,ExitId,gradeAverage,passed,AttendanceLimit")] InformationPassed informationPassed)
        {
            if (ModelState.IsValid)
            {
                db.ınformationPasseds.Add(informationPassed);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ExitId = new SelectList(db.Exits, "ExitId", "ExitName", informationPassed.ExitId);
            return View(informationPassed);
        }

        // GET: InformationPasseds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InformationPassed informationPassed = db.ınformationPasseds.Find(id);
            if (informationPassed == null)
            {
                return HttpNotFound();
            }
            ViewBag.ExitId = new SelectList(db.Exits, "ExitId", "ExitName", informationPassed.ExitId);
            return View(informationPassed);
        }

        // POST: InformationPasseds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InformationPassedId,ExitId,gradeAverage,passed,AttendanceLimit")] InformationPassed informationPassed)
        {
            if (ModelState.IsValid)
            {
                var control = db.ınformationPasseds.FirstOrDefault(i => i.InformationPassedId == informationPassed.InformationPassedId);
                control.gradeAverage = informationPassed.gradeAverage;
                control.passed = informationPassed.passed;
                control.AttendanceLimit = informationPassed.AttendanceLimit;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ExitId = new SelectList(db.Exits, "ExitId", "ExitName", informationPassed.ExitId);
            return View(informationPassed);
        }

        // GET: InformationPasseds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InformationPassed informationPassed = db.ınformationPasseds.Find(id);
            if (informationPassed == null)
            {
                return HttpNotFound();
            }
            return View(informationPassed);
        }

        // POST: InformationPasseds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InformationPassed informationPassed = db.ınformationPasseds.Find(id);
            db.ınformationPasseds.Remove(informationPassed);
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
