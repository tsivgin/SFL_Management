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
    public class GradePercentagesController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: GradePercentages
        public ActionResult Index()
        {
            var gradePercentages = db.gradePercentages.Include(g => g.Course).Include(g => g.Track).Include(g => g.Module) .OrderBy(i=>i.Track.TrackName).ThenByDescending(i=>i.Module.ModuleName);
            return View(gradePercentages.ToList());
        }

        // GET: GradePercentages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GradePercentage gradePercentage = db.gradePercentages.Find(id);
            if (gradePercentage == null)
            {
                return HttpNotFound();
            }
            return View(gradePercentage);
        }

        // GET: GradePercentages/Create
        public ActionResult Create()
        {
            
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Title");
            ViewBag.TrackId = new SelectList(db.Tracks, "TrackId", "TrackName");
            var module = db.modules.Where(i => i.active == true).ToList();
            ViewBag.ModuleId = new SelectList(module,"ModuleId","ModuleName");
            
            return View();
        }

        // POST: GradePercentages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CourseId,TrackId,name,percentage,ModuleId")] GradePercentage gradePercentage)
        {
            if (ModelState.IsValid)
            {
                if (ControlCourseTrack(gradePercentage)) { 
                    db.gradePercentages.Add(gradePercentage);
                    db.SaveChanges();
                    AddGrades(gradePercentage);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorMessage = "This Track do not have this course";
                }
            }

            var module = db.modules.Where(i => i.active == true).ToList();
            ViewBag.ModuleId = new SelectList(module, "ModuleId", "ModuleName");
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Title", gradePercentage.CourseId);
            ViewBag.TrackId = new SelectList(db.Tracks, "TrackId", "TrackName", gradePercentage.TrackId);
            return View(gradePercentage);
        }

                
        public Boolean ControlCourseTrack(GradePercentage gradePercentage)
        {
            var control = db.trackCourses.FirstOrDefault(i => i.TrackId == gradePercentage.TrackId && i.CourseId == gradePercentage.CourseId);
            if (control == null)
            {
                return false;
            }
            return true;
        }
        public void AddGrades(GradePercentage a)
        {

            var enrollstudent = db.EnrollmentsStudent.Where(i => i.Class.TrackId == a.TrackId).ToList();
            int moduleid = ModuleControl();
            foreach(var item in enrollstudent) {
                var control = db.grades.FirstOrDefault(i => i.StudentId == item.StudentId && i.GradePercentageId == a.Id);
                if (control == null)
                {
                    Grade g = new Grade();
                    g.GradePercentageId = a.Id;
                    g.ModuleId = moduleid;
                    g.StudentId = item.StudentId;
                    g.Grades = 0;
                    db.grades.Add(g);
                    db.SaveChanges();
                }
            }
        }

        public int ModuleControl()
        {
            Module c = db.modules.FirstOrDefault(i => i.active == true);
            return c.ModuleId;

        }
        public Boolean ClassStudentAdd(EnrollmentStudent student)
        {

            var model = db.Classes.ToList();
            var required = db.Classes.Find(student.ClassId);
            if (required != null)
            {
                required.RequiredQuota++;
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
           
        }

        // GET: GradePercentages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GradePercentage gradePercentage = db.gradePercentages.Find(id);
            if (gradePercentage == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Title", gradePercentage.CourseId);
            ViewBag.TrackId = new SelectList(db.Tracks, "TrackId", "TrackName", gradePercentage.TrackId);
            var module = db.modules.Where(i => i.active == true).ToList();
            ViewBag.ModuleId = new SelectList(module, "ModuleId", "ModuleName");
            return View(gradePercentage);
        }

        // POST: GradePercentages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CourseId,TrackId,name,percentage,ModuleId")] GradePercentage gradePercentage)
        {
            
            if (ModelState.IsValid)
            {
                if (ControlCourseTrack(gradePercentage)) { 
                    db.Entry(gradePercentage).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorMessage = "This Track do not have this course";
                }
            }
            var module = db.modules.Where(i => i.active == true).ToList();
            ViewBag.ModuleId = new SelectList(module, "ModuleId", "ModuleName");
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Title", gradePercentage.CourseId);
            ViewBag.TrackId = new SelectList(db.Tracks, "TrackId", "TrackName", gradePercentage.TrackId);
            return View(gradePercentage);
        }

        // GET: GradePercentages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GradePercentage gradePercentage = db.gradePercentages.Find(id);
            if (gradePercentage == null)
            {
                return HttpNotFound();
            }
            return View(gradePercentage);
        }

        // POST: GradePercentages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GradePercentage gradePercentage = db.gradePercentages.Find(id);
            db.gradePercentages.Remove(gradePercentage);
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
