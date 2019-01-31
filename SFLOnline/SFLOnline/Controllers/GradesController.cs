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
    public class GradesController : Controller
    {
        private static int classid=0;
        private SchoolContext db = new SchoolContext();
        
        // GET: Grades
        public ActionResult Index()
        {
            var classes = db.Classes.Include(c => c.Track).OrderBy(i=>i.Track.TrackName);
            return View(classes.ToList());
            //ViewBag.ClassId = new SelectList(db.Classes, "ClassId", "ClassName");            
            
        }
        
        public ActionResult Courses(int? id,int cid)
        {
            classid = cid;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var trackCourses = db.trackCourses.Where(c => c.TrackId == id).ToList();
            
            if (trackCourses == null)
            {
                return HttpNotFound();
            }
            return View(trackCourses);
            
        }
        public static int sid = 0, stid = 0;



        public void GradePercentage2(List<int> values, List<int> values2)
        {
            var count = 0;
            foreach (var item in values)
            {
                var c = db.grades.FirstOrDefault(i => i.Id == item);
                c.Grades = values2[count++];
                db.SaveChanges();
            }

        }

        public ActionResult Grades(int id,int tid)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var activemodule = db.modules.FirstOrDefault(i => i.active == true);
            sid = id;
            stid = tid;
            var gradepercentage = db.gradePercentages.Where(i => i.CourseId == id && i.TrackId == tid && i.ModuleId==activemodule.ModuleId).ToList();
            var students = db.EnrollmentsStudent.Where(i => i.ClassId == classid).ToList();
            var grades = db.grades.ToList();
            if (gradepercentage == null)
            {
                return HttpNotFound();
            }
            var model = Tuple.Create<IEnumerable<EnrollmentStudent>, IEnumerable<GradePercentage>, IEnumerable<Grade>>(students, gradepercentage,grades);
            
            return View(model);
        }


        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(string StudentId)
        {
            if (StudentId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try { 
            var enrollstudent = db.EnrollmentsStudent.FirstOrDefault(i => i.StudentId == StudentId);
            var tid = db.Classes.FirstOrDefault(i => i.ClassId == enrollstudent.ClassId);
            var students = db.EnrollmentsStudent.Where(i => i.StudentId == StudentId).ToList();
            var grades = db.grades.Where(i=>i.StudentId==StudentId).OrderBy(i=>i.GradePercentage.Track.TrackName).ToList();
            var gradepercentages = db.gradePercentages.ToList();
            List<GradePercentage> gradepercentage = new List<GradePercentage>();
            foreach(var item in grades)
            {
                var control = db.gradePercentages.FirstOrDefault(i => i.Id == item.GradePercentageId);
                gradepercentage.Add(control);
            }
            if (gradepercentage == null)
            {
                return HttpNotFound();
            }
            var model = Tuple.Create<IEnumerable<EnrollmentStudent>, IEnumerable<GradePercentage>, IEnumerable<Grade>>(students, gradepercentage, grades);

            return View(model);
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        // GET: Grades/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grade grade = db.grades.Find(id);
            if (grade == null)
            {
                return HttpNotFound();
            }
            return View(grade);
        }

        // GET: Grades/Create
        public ActionResult Create()
        {
            ViewBag.GradePercentageId = new SelectList(db.gradePercentages, "Id", "name");
            ViewBag.ModuleId = new SelectList(db.modules, "ModuleId", "ModuleName");
            ViewBag.StudentId = new SelectList(db.Persons, "Id", "LastName");            
            var grades = db.grades.Include(g => g.GradePercentage).Include(g => g.Module).Include(g => g.Student);
            return View(grades.ToList());
        }

        // POST: Grades/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StudentId,GradePercentageId,ModuleId,Grades")] Grade grade)
        {
            if (ModelState.IsValid)
            {
                db.grades.Add(grade);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GradePercentageId = new SelectList(db.gradePercentages, "Id", "name", grade.GradePercentageId);
            ViewBag.ModuleId = new SelectList(db.modules, "ModuleId", "ModuleName", grade.ModuleId);
            ViewBag.StudentId = new SelectList(db.Persons, "Id", "LastName", grade.StudentId);
            return View(grade);
        }

        // GET: Grades/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grade grade = db.grades.Find(id);
            if (grade == null)
            {
                return HttpNotFound();
            }
            ViewBag.GradePercentageId = new SelectList(db.gradePercentages, "Id", "name", grade.GradePercentageId);
            ViewBag.ModuleId = new SelectList(db.modules, "ModuleId", "ModuleName", grade.ModuleId);
            ViewBag.StudentId = new SelectList(db.Persons, "Id", "LastName", grade.StudentId);
            return View(grade);
        }

        // POST: Grades/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StudentId,GradePercentageId,ModuleId,Grades")] Grade grade)
        {
            if (ModelState.IsValid)
            {
                db.Entry(grade).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GradePercentageId = new SelectList(db.gradePercentages, "Id", "name", grade.GradePercentageId);
            ViewBag.ModuleId = new SelectList(db.modules, "ModuleId", "ModuleName", grade.ModuleId);
            ViewBag.StudentId = new SelectList(db.Persons, "Id", "LastName", grade.StudentId);
            return View(grade);
        }

        // GET: Grades/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grade grade = db.grades.Find(id);
            if (grade == null)
            {
                return HttpNotFound();
            }
            return View(grade);
        }

        // POST: Grades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Grade grade = db.grades.Find(id);
            db.grades.Remove(grade);
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
