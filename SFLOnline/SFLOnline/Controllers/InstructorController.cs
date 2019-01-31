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
    [Authorize(Roles = "Instructor")]
    public class InstructorController : Controller
    {
        private SchoolContext db = new SchoolContext();

        static string instructorid = "";
        // GET: Instructor
        public ActionResult Index()
        {

            getInstructorId();
            var enrollmentınstructor = db.enrollmentInstructors.Where(i=>i.InstructorId==instructorid).ToList();
            List<Class> classes = new List<Class>();
            List<Course> courses = new List<Course>();
            foreach(var item in enrollmentınstructor)
            {
                courses.Add(db.Courses.FirstOrDefault(i => i.CourseId == item.CourseId));
                classes.Add(db.Classes.FirstOrDefault(i => i.ClassId == item.ClassId));

            }
            var model = Tuple.Create<IEnumerable<Course>, IEnumerable<Class>>(courses, classes);

            return View(model);
        }

        public void getInstructorId()
        {
            var ctx = Request.GetOwinContext();

            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

            // Get the claims values
            instructorid = identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                               .Select(c => c.Value).SingleOrDefault();  //Emaili getirdi, ID için ClaimTypes.NameIdentifier yaz Email yerine


        }

        public ActionResult Announcement()
        {
            getInstructorId();
            var enrollmentınstructor = db.enrollmentInstructors.Where(i => i.InstructorId == instructorid).ToList();
            List<Class> classes = new List<Class>();
            List<Course> courses = new List<Course>();
            foreach (var item in enrollmentınstructor)
            {
                courses.Add(db.Courses.FirstOrDefault(i => i.CourseId == item.CourseId));
                classes.Add(db.Classes.FirstOrDefault(i => i.ClassId == item.ClassId));

            }
            var model = Tuple.Create<IEnumerable<Course>, IEnumerable<Class>>(courses, classes);

            return View(model);
        }
        static int classid = 0;
        public ActionResult WriteAnnouncement(int id)
        {
            getInstructorId();

            classid = id;
            var Persons = db.Persons.Where(i => i.Id == instructorid).ToList();
            //var classes = db.Classes.Where(i => i.ClassId == id).ToList();
            //ViewBag.ClassId = new SelectList(classes, "Id", "ClassName");
            ViewBag.WriterId = new SelectList(Persons, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult WriteAnnouncement([Bind(Include = "AnnouncementId,WriterId,AnnouncementName,description")] Announcement announcement)
        {
            if (ModelState.IsValid){

                    announcement.WriterId = instructorid;
                    announcement.ClassId = classid;
                    db.announcements.Add(announcement);
                    db.SaveChanges();
                
                return RedirectToAction("Announcements");
            }

            ViewBag.WriterId = new SelectList(db.Persons, "Id", "LastName", announcement.WriterId);
            return View(announcement);
        }

        public ActionResult Announcements()
        {
            var announcements = db.announcements.Where(i => i.WriterId == instructorid).ToList();
            return View(announcements);
        }

        
        
        public ActionResult Select(int id)
        {
            classid = id;
            return View();

        }
        public ActionResult Grade()
        {
            
            var enrollmentInstructor = db.enrollmentInstructors.FirstOrDefault(i => i.ClassId == classid);
            var track = db.Classes.FirstOrDefault(i => i.ClassId== classid);
            var gradepercentage = db.gradePercentages.Where(i => i.CourseId == enrollmentInstructor.CourseId && i.TrackId == track.TrackId).ToList();

            var students = db.EnrollmentsStudent.Where(i => i.ClassId == classid).ToList();
            var grades = db.grades.ToList();
            if (gradepercentage == null)
            {
                return HttpNotFound();
            }            
            var model = Tuple.Create<IEnumerable<EnrollmentStudent>, IEnumerable<GradePercentage>, IEnumerable<Grade>>(students, gradepercentage, grades);

            return View(model);            
        }
        public ActionResult Attendance()
        {
            List<StudentAttendance> student = new List<StudentAttendance>();
            var students = db.EnrollmentsStudent.Where(i => i.ClassId == classid).ToList();
            foreach(var item in students)
            {
                student.Add(db.studentAttendances.FirstOrDefault(i => i.StudentId == item.StudentId));
            }            
            //StudentAttendance studentAttendance = db.studentAttendances.Find(studentid);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }
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
        [HttpPost]
        public ActionResult Attendance(List<int> values, List<int> values2)
        {
            var count = 0;
            foreach (var item in values2)
            {
                var c = db.studentAttendances.FirstOrDefault(i => i.Id == item);
                c.Attendance = c.Attendance + values[count++];
                db.SaveChanges();
            }

            List<StudentAttendance> student = new List<StudentAttendance>();
            var students = db.EnrollmentsStudent.Where(i => i.ClassId == classid).ToList();
            foreach (var item in students)
            {
                student.Add(db.studentAttendances.FirstOrDefault(i => i.StudentId == item.StudentId));
            }
            //StudentAttendance studentAttendance = db.studentAttendances.Find(studentid);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        public ActionResult WeeklySchedule()
        {
            getInstructorId();

            var instructorclass = db.enrollmentInstructors.Where(i => i.InstructorId == instructorid).ToList();
            List<ClassSchedule> classschedules = new List<ClassSchedule>();
            foreach (var item in instructorclass)
            {
                foreach(var item2 in db.classSchedules.ToList())
                {
                    if (item2.EnrollmentInstructorId == item.Id)
                    {
                        classschedules.Add(item2);
                    }

                }
            }
            var slots = db.slots.ToList().OrderBy(i => i.SlotNumber);
            var days = db.days.ToList().OrderBy(i => i.DayNumber);
            var model = Tuple.Create<IEnumerable<ClassSchedule>, IEnumerable<Slot>, IEnumerable<Day>>(classschedules, slots, days);
            return View(model);
        }

        // GET: Instructor/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.ınstructors.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        // GET: Instructor/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Instructor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,LastName,FirstMidName,EMail,Password")] Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                db.Persons.Add(instructor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(instructor);
        }

        // GET: Instructor/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.ınstructors.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        // POST: Instructor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,LastName,FirstMidName,EMail,Password")] Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(instructor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(instructor);
        }

        // GET: Instructor/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Announcement announcement = db.announcements.Find(id);
            if (announcement == null)
            {
                return HttpNotFound();
            }
            return View(announcement);
        }

        // POST: Instructor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            Announcement announcement = db.announcements.Find(id);
            var announcements = db.announcements.Where(i => i.AnnouncementName == announcement.AnnouncementName && i.description == announcement.description).ToList();
            foreach (var item in announcements)
            {
                db.announcements.Remove(item);
            }
            db.announcements.Remove(announcement);
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
