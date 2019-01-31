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
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private SchoolContext db = new SchoolContext();
        static int trackid = 0;
        static string studentid = "";
        // GET: Student

        
        [Authorize]
        public ActionResult Index()
        {
            getstudentid();
            var classes =db.EnrollmentsStudent.FirstOrDefault(i => i.StudentId == studentid);
            var track = db.Classes.FirstOrDefault(i => i.ClassId == classes.ClassId);
            var course = db.trackCourses.Where(i => i.TrackId == track.TrackId).ToList();
            trackid = track.TrackId;
            var module = db.modules.FirstOrDefault(i => i.active == true);
            ViewBag.Module = module.ModuleName;
            ViewBag.Class = classes.Class.ClassName;
            ViewBag.Track = track.Track.TrackName;
            //var studentAttendances = db.studentAttendances.Include(s => s.Student);
            return View(course);
        }
        public void getstudentid()
        {
            var ctx = Request.GetOwinContext();

            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

            // Get the claims values
            studentid = identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                               .Select(c => c.Value).SingleOrDefault();  //Emaili getirdi, ID için ClaimTypes.NameIdentifier yaz Email yerine

        }

        public ActionResult Select(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var gradepercentage = db.gradePercentages.Where(i => i.TrackId == trackid && i.CourseId == id).ToList();
            List<Grade> gradestudent= new List<Grade>();
            getstudentid();
            foreach (var item in gradepercentage)
            {
                gradestudent.Add(db.grades.FirstOrDefault(i => i.GradePercentageId == item.Id && i.StudentId == studentid));
                //count++;
            }

            //ViewBag.StudentId = new SelectList(db.Persons, "Id", "LastName", studentAttendance.StudentId);
            return View(gradestudent);
        }

        public ActionResult Attendance()
        {
            getstudentid();
            var student = db.studentAttendances.FirstOrDefault(i => i.StudentId == studentid);
            var attendance = db.ınformationPasseds.FirstOrDefault();
            ViewBag.attendance = attendance.AttendanceLimit;
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        public ActionResult WeeklySchedule()
        {
            getstudentid();
            var studentclass = db.EnrollmentsStudent.FirstOrDefault(i => i.StudentId == studentid);
            var enrollid = db.enrollmentInstructors.Where(i => i.ClassId == studentclass.ClassId).ToList();
            List<ClassSchedule> classschedules = new List<ClassSchedule>();

            foreach (var item in enrollid)
            {
                foreach (var item2 in db.classSchedules.ToList())
                {
                    if (item2.EnrollmentInstructorId == item.Id)
                    {
                        classschedules.Add(item2);
                    }
                }
            }
            var slots = db.slots.ToList().OrderBy(i=>i.SlotNumber);
            var days = db.days.ToList().OrderBy(i=>i.DayNumber);
            var model = Tuple.Create<IEnumerable<ClassSchedule>, IEnumerable<Slot>, IEnumerable<Day>>(classschedules, slots, days);
            return View(model);
        }

        public ActionResult AcademicStatus()
        {
            getstudentid();
            var student = db.Students.FirstOrDefault(i => i.Id == studentid);
            if (student == null)
            {
                return HttpNotFound();
            }
            var modules = db.modules.ToList();
            var classes = db.EnrollmentsStudent.FirstOrDefault(i => i.StudentId == student.Id);
            var courses = db.trackCourses.Where(i => i.TrackId == classes.Class.TrackId).ToList();
            var gradepercentages = db.gradePercentages.Where(i => i.TrackId == classes.Class.TrackId).ToList().OrderBy(i=>i.CourseId);


            double sum = 0;
            List<double> sums = new List<double>();
            List<Course> courseid = new List<Course>();
            List<Module> module = new List<Module>();
            foreach(var item in gradepercentages)
            {
                var grades = db.grades.FirstOrDefault(i => i.GradePercentageId == item.Id && i.StudentId==student.Id);
                if(courseid.Count != 0)
                {
                    var controlcourse = courseid.FirstOrDefault(i => i.CourseId == item.CourseId);
                    var controlmodule = module.FirstOrDefault(i => i.ModuleId == item.ModuleId);
                    if (controlcourse != null && controlmodule!=null)
                    {
                        sum += (grades.Grades) * item.percentage / 100;
                    }
                    else
                    {
                        sums.Add(sum);
                        sum = 0;
                        sum += (grades.Grades) * item.percentage / 100;                        
                        courseid.Add(item.Course);
                        module.Add(item.Module);
                    }
                }
                else
                {
                    sum += (grades.Grades) * item.percentage / 100;
                    courseid.Add(item.Course);
                    module.Add(item.Module);
                }

            }
            sums.Add(sum);

            
            List<string> coursename = new List<string>();
            List<int> coursepercentange = new List<int>();
            
            foreach(var item in courseid)
            {
                coursename.Add(item.Title);
                coursepercentange.Add(item.Credits);
            }

            
            List<double> moduleaverage = new List<double>();
            List<Module> countmodule = new List<Module>();
            double summodule = 0;
            for(var i=0;i<coursename.Count;i++)
            {
                var control = countmodule.FirstOrDefault(a => a.ModuleId == module[i].ModuleId);

                if (countmodule.Count != 0) {                            

                    if(control != null)
                        {
                            summodule += (sums[i] * coursepercentange[i]) / 100;
                        }
                    else
                    {
                        moduleaverage.Add(summodule);
                        summodule = 0;
                        summodule+= (sums[i] * coursepercentange[i]) / 100;                    
                        countmodule.Add(module[i]);
                    }

                }
                else
                {
                    summodule += (sums[i] * coursepercentange[i]) / 100;
                    countmodule.Add(module[i]);

                }
            }
            moduleaverage.Add(summodule);            
            ViewBag.Sums = moduleaverage;
            ViewBag.modules = countmodule;
            return View(student);
        }

        public ActionResult CalculateExit()
        {
            getstudentid();
            var student = db.Students.FirstOrDefault(i => i.Id == studentid);
            if (student == null)
            {
                return HttpNotFound();
            }
            var modules = db.modules.ToList();
            var classes = db.EnrollmentsStudent.FirstOrDefault(i => i.StudentId == student.Id);
            var courses = db.trackCourses.Where(i => i.TrackId == classes.Class.TrackId).ToList();
            var gradepercentages = db.gradePercentages.Where(i => i.TrackId == classes.Class.TrackId).ToList().OrderBy(i => i.CourseId);


            double sum = 0;
            List<double> sums = new List<double>();
            List<Course> courseid = new List<Course>();
            List<Module> module = new List<Module>();
            foreach (var item in gradepercentages)
            {
                var grades = db.grades.FirstOrDefault(i => i.GradePercentageId == item.Id && i.StudentId == student.Id);
                if (courseid.Count != 0)
                {
                    var controlcourse = courseid.FirstOrDefault(i => i.CourseId == item.CourseId);
                    var controlmodule = module.FirstOrDefault(i => i.ModuleId == item.ModuleId);
                    if (controlcourse != null && controlmodule != null)
                    {
                        sum += (grades.Grades) * item.percentage / 100;
                    }
                    else
                    {
                        sums.Add(sum);
                        sum = 0;
                        sum += (grades.Grades) * item.percentage / 100;
                        courseid.Add(item.Course);
                        module.Add(item.Module);
                    }
                }
                else
                {
                    sum += (grades.Grades) * item.percentage / 100;
                    courseid.Add(item.Course);
                    module.Add(item.Module);
                }

            }
            sums.Add(sum);


            List<string> coursename = new List<string>();
            List<int> coursepercentange = new List<int>();

            foreach (var item in courseid)
            {
                coursename.Add(item.Title);
                coursepercentange.Add(item.Credits);
            }


            List<double> moduleaverage = new List<double>();
            List<Module> countmodule = new List<Module>();
            double summodule = 0;
            for (var i = 0; i < coursename.Count; i++)
            {
                var control = countmodule.FirstOrDefault(a => a.ModuleId == module[i].ModuleId);

                if (countmodule.Count != 0)
                {

                    if (control != null)
                    {
                        summodule += (sums[i] * coursepercentange[i]) / 100;
                    }
                    else
                    {
                        moduleaverage.Add(summodule);
                        summodule = 0;
                        summodule += (sums[i] * coursepercentange[i]) / 100;
                        countmodule.Add(module[i]);
                    }

                }
                else
                {
                    summodule += (sums[i] * coursepercentange[i]) / 100;
                    countmodule.Add(module[i]);

                }
            }
            moduleaverage.Add(summodule);


           
            ViewBag.modules = countmodule;

            var moduless = db.modules.ToList();
            var average = 0.0;
            for(var i = 0; i < moduleaverage.Count; i++)
            {
                average += moduleaverage[i];
            }
            average = average / modules.Count;

            var information = db.ınformationPasseds.FirstOrDefault();
            var exitexam = 0.0;
            //if (average < information.gradeAverage)
            //{
            //    ViewBag.averageerror = "Your Grade Average is not greater than " + information.gradeAverage.ToString();
            //}            
            //else
            //{
                exitexam = (information.passed * 2) - average;
            if (exitexam > 100)
            {
                ViewBag.averageerror = "Your Grade Average is not enough";
            }
            else
                ViewBag.exitexam = "You should get higher than this note: " + exitexam;
            //}


            ViewBag.average = average;
            return View(student);

        }


        // GET: Student/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentAttendance studentAttendance = db.studentAttendances.Find(id);
            if (studentAttendance == null)
            {
                return HttpNotFound();
            }
            return View(studentAttendance);
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            ViewBag.StudentId = new SelectList(db.Persons, "Id", "LastName");
            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StudentId,Attendance")] StudentAttendance studentAttendance)
        {
            if (ModelState.IsValid)
            {
                db.studentAttendances.Add(studentAttendance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StudentId = new SelectList(db.Persons, "Id", "LastName", studentAttendance.StudentId);
            return View(studentAttendance);
        }

        // GET: Student/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentAttendance studentAttendance = db.studentAttendances.Find(id);
            if (studentAttendance == null)
            {
                return HttpNotFound();
            }
            ViewBag.StudentId = new SelectList(db.Persons, "Id", "LastName", studentAttendance.StudentId);
            return View(studentAttendance);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StudentId,Attendance")] StudentAttendance studentAttendance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentAttendance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StudentId = new SelectList(db.Persons, "Id", "LastName", studentAttendance.StudentId);
            return View(studentAttendance);
        }

        // GET: Student/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentAttendance studentAttendance = db.studentAttendances.Find(id);
            if (studentAttendance == null)
            {
                return HttpNotFound();
            }
            return View(studentAttendance);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentAttendance studentAttendance = db.studentAttendances.Find(id);
            db.studentAttendances.Remove(studentAttendance);
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
