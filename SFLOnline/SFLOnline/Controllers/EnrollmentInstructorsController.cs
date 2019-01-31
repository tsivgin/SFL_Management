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
    public class EnrollmentInstructorsController : Controller
    {
        //static int enrollmentinstructorid = 0;
        //static string instructorid = "";
        static EnrollmentInstructor enrollmentinstructors;
        private SchoolContext db = new SchoolContext();
        
        // GET: EnrollmentInstructors
        public ActionResult Index()
        {
            var enrollmentInstructors = db.enrollmentInstructors.Include(e => e.Class).Include(e => e.Course).OrderBy(i=>i.Class.Track.TrackName);
            
            return View(enrollmentInstructors.ToList());
        }

        // GET: EnrollmentInstructors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnrollmentInstructor enrollmentInstructor = db.enrollmentInstructors.Find(id);
            if (enrollmentInstructor == null)
            {
                return HttpNotFound();
            }
            return View(enrollmentInstructor);
        }

        // GET: EnrollmentInstructors/Create
        public ActionResult Create()
        {

            ViewBag.ClassId = new SelectList(db.Classes, "ClassId", "ClassName");
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Title");
            ViewBag.InstructorId = new SelectList(db.ınstructors, "Id", "Id");
            return View();
        }

        // POST: EnrollmentInstructors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ClassId,CourseId,InstructorId")] EnrollmentInstructor enrollmentInstructor)
        {
            if (ModelState.IsValid)
            {
                EnrollmentInstructorsController en = new EnrollmentInstructorsController();
                var x = en.En(enrollmentInstructor);
                if (x)
                {
                   

                    db.enrollmentInstructors.Add(enrollmentInstructor);
                    db.SaveChanges();
                    enrollmentinstructors = enrollmentInstructor;
                    return RedirectToAction("ClassSchedule");
                    //enrollmentinstructorid = enrollmentInstructor.Id;
                    //instructorid = enrollmentInstructor.InstructorId;

                }
                else
                    ViewBag.ErrorMessage = "This Class already have this course";
                
            }
            ViewBag.InstructorId = new SelectList(db.ınstructors, "Id", "Id");
            ViewBag.ClassId = new SelectList(db.Classes, "ClassId", "ClassName", enrollmentInstructor.ClassId);
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Title", enrollmentInstructor.CourseId);
            return View(enrollmentInstructor);
        }

        public Boolean En(EnrollmentInstructor a)
        {
            using (var ctx = new SchoolContext())
            {
                var c = ctx.enrollmentInstructors.FirstOrDefault(i => i.ClassId == a.ClassId && i.CourseId==a.CourseId);
                if (c != null)
                {
                 
                        return false;
                }
                return true;
            }
        }

        public ActionResult ClassSchedule()
        {
            var slot = db.slots.ToList();
            var days = db.days.ToList();            
            var schedule = db.classSchedules.Where(i => i.EnrollmentInstructor.InstructorId == enrollmentinstructors.InstructorId || i.EnrollmentInstructor.ClassId == enrollmentinstructors.ClassId).ToList();
            var model = Tuple.Create<IEnumerable<Slot>, IEnumerable<Day> ,IEnumerable<ClassSchedule>>(slot, days,schedule);
            return View(model);

        }
        [HttpPost]
        public void ClassSchedule(List<string> values, List<string> values2)
        {
            var count = 0;

            foreach(var item in values)
            {
                count++;
            }

            for(var i = 0; i < count; i++)
            {
                var day = db.days.AsEnumerable().FirstOrDefault(a => a.DaysName.Trim().ToString() == values[i].Trim().ToString());
                var slot = db.slots.AsEnumerable().FirstOrDefault(a => a.SlotName.Trim().ToString() == values2[i].Trim().ToString());
                var control = db.classSchedules.FirstOrDefault(a => a.EnrollmentInstructorId == enrollmentinstructors.Id && a.DayId == day.DayId && a.SlotId == slot.SlotId);
                if (control == null) { 
                    ClassSchedule schedule = new ClassSchedule();
                    schedule.DayId = day.DayId;
                    schedule.SlotId = slot.SlotId;
                    schedule.EnrollmentInstructorId = enrollmentinstructors.Id;
                    db.classSchedules.Add(schedule);
                    db.SaveChanges();
                }
            }
            
            Boolean check = false;
            var selectedschedule = db.classSchedules.Where(i => i.EnrollmentInstructorId == enrollmentinstructors.Id).ToList();
            foreach(var item in selectedschedule)
            {
                check = false;
                for (var i = 0; i < count; i++)
                {
                    if (item.Slot.SlotName.Trim().ToString() == values2[i].Trim().ToString() && item.Day.DaysName.Trim().ToString()== values[i].Trim().ToString())
                    {
                        check = true;
                    }
                }
                if (check == false)
                {
                    ClassSchedule classSchedule = db.classSchedules.Find(item.ClassScheduleId);
                    db.classSchedules.Remove(classSchedule);
                    db.SaveChanges();
                }
            }
            

        }
        

        // GET: EnrollmentInstructors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnrollmentInstructor enrollmentInstructor = db.enrollmentInstructors.Find(id);
            if (enrollmentInstructor == null)
            {
                return HttpNotFound();
            }

            var slot = db.slots.ToList();
            var days = db.days.ToList();
            var schedule = db.classSchedules.Where(i => i.EnrollmentInstructor.InstructorId == enrollmentInstructor.InstructorId || i.EnrollmentInstructor.ClassId == enrollmentInstructor.ClassId).ToList();
            var model = Tuple.Create<IEnumerable<Slot>, IEnumerable<Day>, IEnumerable<ClassSchedule>>(slot, days, schedule);

            ViewBag.ClassId = enrollmentInstructor.ClassId;
            ViewBag.InstructorId = enrollmentInstructor.InstructorId;
            enrollmentinstructors = enrollmentInstructor;

            return View(model);            
        }

        // POST: EnrollmentInstructors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ClassId,CourseId,InstructorId")] EnrollmentInstructor enrollmentInstructor)
        {
            if (ModelState.IsValid)
            {

                db.Entry(enrollmentInstructor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClassId = new SelectList(db.Classes, "ClassId", "ClassName", enrollmentInstructor.ClassId);
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Title", enrollmentInstructor.CourseId);
            return View(enrollmentInstructor);
        }

        public void ControlSchedule(EnrollmentInstructor enrollmentInstructor)
        {

        }

        // GET: EnrollmentInstructors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnrollmentInstructor enrollmentInstructor = db.enrollmentInstructors.Find(id);
            if (enrollmentInstructor == null)
            {
                return HttpNotFound();
            }
            return View(enrollmentInstructor);
        }

        // POST: EnrollmentInstructors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EnrollmentInstructor enrollmentInstructor = db.enrollmentInstructors.Find(id);
            db.enrollmentInstructors.Remove(enrollmentInstructor);
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
