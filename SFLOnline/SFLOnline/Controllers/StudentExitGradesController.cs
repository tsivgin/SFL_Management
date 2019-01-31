using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using SFLOnline.DAL;
using SFLOnline.Models;

namespace SFLOnline.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StudentExitGradesController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: StudentExitGrades
        public ActionResult Index()
        {
            var studentExitGrades = db.StudentExitGrades.Include(s => s.Exit).Include(s => s.Student);
            return View(studentExitGrades.ToList().OrderBy(i=>i.Exit.ExitName));
        }

        // GET: StudentExitGrades/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentExitGrade studentExitGrade = db.StudentExitGrades.Find(id);
            if (studentExitGrade == null)
            {
                return HttpNotFound();
            }
            return View(studentExitGrade);
        }

        // GET: StudentExitGrades/Create
        public ActionResult Create()
        {
            ViewBag.ExitId = new SelectList(db.Exits, "ExitId", "ExitName");
            ViewBag.StudentId = new SelectList(db.Students, "Id", "Id");
            return View();
        }

        // POST: StudentExitGrades/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StudentId,ExitId,grade")] StudentExitGrade studentExitGrade)
        {
            if (ModelState.IsValid)
            {
                if (ControlGrades(studentExitGrade)) { 
                db.StudentExitGrades.Add(studentExitGrade);
                db.SaveChanges();
                return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorMessage = "This Student already have this Exit Grade";
                }
            }

            ViewBag.ExitId = new SelectList(db.Exits, "ExitId", "ExitName", studentExitGrade.ExitId);
            ViewBag.StudentId = new SelectList(db.Students, "Id", "Id", studentExitGrade.StudentId);
            return View(studentExitGrade);
        }

        public Boolean ControlGrades(StudentExitGrade studentExitGrade)
        {
            var control = db.StudentExitGrades.FirstOrDefault(i => i.StudentId == studentExitGrade.StudentId && i.ExitId == studentExitGrade.ExitId);
            if (control ==null)
            {
                return true;
            }
            return false;
        }


        // GET: StudentExitGrades/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentExitGrade studentExitGrade = db.StudentExitGrades.Find(id);
            if (studentExitGrade == null)
            {
                return HttpNotFound();
            }
            ViewBag.ExitId = new SelectList(db.Exits, "ExitId", "ExitName", studentExitGrade.ExitId);
            ViewBag.StudentId = new SelectList(db.Students, "Id", "Id", studentExitGrade.StudentId);
            return View(studentExitGrade);
        }

        // POST: StudentExitGrades/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StudentId,ExitId,grade")] StudentExitGrade studentExitGrade)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentExitGrade).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ExitId = new SelectList(db.Exits, "ExitId", "ExitName", studentExitGrade.ExitId);
            ViewBag.StudentId = new SelectList(db.Students, "Id", "Id", studentExitGrade.StudentId);
            return View(studentExitGrade);
        }

        // GET: StudentExitGrades/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentExitGrade studentExitGrade = db.StudentExitGrades.Find(id);
            if (studentExitGrade == null)
            {
                return HttpNotFound();
            }
            return View(studentExitGrade);
        }

        // POST: StudentExitGrades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentExitGrade studentExitGrade = db.StudentExitGrades.Find(id);
            db.StudentExitGrades.Remove(studentExitGrade);
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
