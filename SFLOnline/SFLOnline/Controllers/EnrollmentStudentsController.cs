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
    public class EnrollmentStudentsController : Controller
    {
        
        private SchoolContext db = new SchoolContext();
        
        // GET: EnrollmentStudents
        public ActionResult Index()
        {
            var enrollmentsStudent = db.EnrollmentsStudent.Include(e => e.Class).OrderBy(i=>i.Class.Track.TrackName);
            return View(enrollmentsStudent.ToList());
        }

        // GET: EnrollmentStudents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnrollmentStudent enrollmentStudent = db.EnrollmentsStudent.Find(id);
            if (enrollmentStudent == null)
            {
                return HttpNotFound();
            }
            return View(enrollmentStudent);
        }

        public ActionResult EnrollmentGrades()
        {
            var exit = db.Exits.Where(i=>i.ForEnrollment == true).ToList();
            var exittrack = db.ExitTracks.Where(i=>i.Exit.ForEnrollment==true).ToList();
            ViewBag.ClassId = new SelectList(exit, "ExitId", "ExitName");

            var model = Tuple.Create<IEnumerable<Exit>, IEnumerable<ExitTrack>>(exit, exittrack);

            return View(model);
        }   

        public ActionResult EnrollStudent()
        {
            var exit = db.Exits.FirstOrDefault(i => i.ForEnrollment == true);
            var exittrack = db.ExitTracks.Where(i => i.Exit.ForEnrollment == true).ToList();
            var studentexitgrade = db.StudentExitGrades.ToList();
            //List<EnrollmentStudent> enrollstudent = new List<EnrollmentStudent>;
            foreach(var item in studentexitgrade)
            {
               
                foreach(var item2 in exittrack)
                {
                    var control = db.EnrollmentsStudent.FirstOrDefault(i => i.StudentId == item.StudentId);
                    if (item.grade < item2.grade && control==null)
                    {
                        EnrollmentStudent enrollstudent = new EnrollmentStudent();
                        enrollstudent.StudentId = item.StudentId;
                        var classes = db.Classes.FirstOrDefault(i => i.TrackId == item2.TrackId && i.Quota > i.RequiredQuota);
                        enrollstudent.ClassId = classes.ClassId;
                        classes.RequiredQuota = classes.RequiredQuota + 1;
                        db.EnrollmentsStudent.Add(enrollstudent);
                        db.SaveChanges();
                        AddAttendance(enrollstudent);
                        AddGrades(enrollstudent);
                        break;
                    }
                }
               
            }

            return RedirectToAction("Index");
        }

        // GET: EnrollmentStudents/Create
        public ActionResult Create()
        {
            ControlClass();
            ControlStudent();         
            return View();
        }

        public void ControlClass()
        {
            var classes = db.Classes.ToList();
            List<Class> classlist = new List<Class>();
            foreach(var control in classes)
            {
                if (control.Quota != control.RequiredQuota)
                {
                    classlist.Add(control);
                        
                }
            }
            ViewBag.ClassId = new SelectList(classlist, "ClassId", "ClassName");
        }
        public void ControlStudent()
        {
            var active = true;
            var studentlist = db.Students.ToList();
            var enrollstudent = db.EnrollmentsStudent.ToList();
            List<Student> studentId = new List<Student>();
            using (var ctx = new SchoolContext())
            {
                foreach(var student in studentlist) {                    
                        foreach (var control in enrollstudent) { 
                            if(student.Id==control.StudentId)
                            active = false;                        
                        }

                    if (active)
                    {
                        studentId.Add(student);
                    }
                    active = true;
                    
                }
                ViewBag.StudentId = new SelectList(studentId, "Id", "Id");
            }

        }
        
        

        // POST: EnrollmentStudents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ClassId,StudentId")] EnrollmentStudent enrollmentStudent)
        {
            if (ModelState.IsValid)
            {
                    ClassStudentAdd(enrollmentStudent);
                    AddGrades(enrollmentStudent);
                    AddAttendance(enrollmentStudent);
                    db.EnrollmentsStudent.Add(enrollmentStudent);
                    db.SaveChanges();
                return RedirectToAction("Index");
                


            }
            ViewBag.StudentId = new SelectList(db.Students, "Id", "Id");
            ViewBag.ClassId = new SelectList(db.Classes, "ClassId", "ClassName", enrollmentStudent.ClassId);
            return View(enrollmentStudent);
        }

        public void AddAttendance(EnrollmentStudent a)
        {
            var control = db.studentAttendances.FirstOrDefault(i => i.StudentId == a.StudentId);
            if (control == null) { 
            var student = db.Students.FirstOrDefault(i => i.Id == a.StudentId);
            StudentAttendance Attendance = new StudentAttendance();
            Attendance.StudentId = student.Id;
            Attendance.Attendance = 0;
            db.studentAttendances.Add(Attendance);
            db.SaveChanges();
            }
        }

        public void AddGrades(EnrollmentStudent a)
        {
                using (var ctx = new SchoolContext())
                {
                    var c = db.Classes.FirstOrDefault(i => i.ClassId == a.ClassId);
                    if (c != null)
                    {
                        var gradepercentage = db.gradePercentages.Where(i => i.TrackId == c.TrackId).ToList();

                    
                    
                        int moduleid = ModuleControl();
                        foreach (var item in gradepercentage)
                        {
                            var control = db.grades.FirstOrDefault(i => i.StudentId == a.StudentId && i.GradePercentageId==item.Id);
                        if (control == null) { 
                            Grade g = new Grade();
                            g.GradePercentageId = item.Id;
                            g.ModuleId = moduleid;
                            g.StudentId = a.StudentId;
                            g.Grades = 0;
                            db.grades.Add(g);
                            db.SaveChanges();
                        }
                    }
                    
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
            if (required != null) {                 
                required.RequiredQuota++;
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public static int classid = 0;
        // GET: EnrollmentStudents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnrollmentStudent enrollmentStudent = db.EnrollmentsStudent.Find(id);
            if (enrollmentStudent == null)
            {
                return HttpNotFound();
            }
            classid = enrollmentStudent.ClassId;
            ViewBag.ClassId = new SelectList(db.Classes, "ClassId", "ClassName", enrollmentStudent.ClassId);
            return View(enrollmentStudent);
        }

        // POST: EnrollmentStudents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ClassId,StudentId")] EnrollmentStudent enrollmentStudent)
        {
            if (ModelState.IsValid)
            {
               var x = ClassStudentEditAdd(enrollmentStudent);
                if (x) { 
                    db.Entry(enrollmentStudent).State = EntityState.Modified;
                    db.SaveChanges();
                    AddGrades(enrollmentStudent);
                    return RedirectToAction("Index");
                }
                else
                    ViewBag.ErrorMessage = "This Class already have FULL";
            }
            ViewBag.ClassId = new SelectList(db.Classes, "ClassId", "ClassName", enrollmentStudent.ClassId);
            return View(enrollmentStudent);
        }

        public Boolean ClassStudentEditAdd(EnrollmentStudent student)
        {
            var model = db.Classes.ToList();
            var classAdd = db.Classes.Find(student.ClassId);
            var classDelete = db.Classes.Find(classid);
            if (classAdd != null && classid!=classAdd.ClassId && classAdd.RequiredQuota!=classAdd.Quota)
            {
                classDelete.RequiredQuota--;
                classAdd.RequiredQuota++;
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        // GET: EnrollmentStudents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnrollmentStudent enrollmentStudent = db.EnrollmentsStudent.Find(id);
            if (enrollmentStudent == null)
            {
                return HttpNotFound();
            }
            return View(enrollmentStudent);
        }

        // POST: EnrollmentStudents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EnrollmentStudent enrollmentStudent = db.EnrollmentsStudent.Find(id);
            var x = ClassStudentDelete(enrollmentStudent);
            db.EnrollmentsStudent.Remove(enrollmentStudent);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public Boolean ClassStudentDelete(EnrollmentStudent student)
        {
            var model = db.Classes.ToList();
            var required = db.Classes.Find(student.ClassId);
            if (required != null)
            {
                required.RequiredQuota--;
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }           
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
