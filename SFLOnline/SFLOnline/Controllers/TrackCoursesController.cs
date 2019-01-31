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
    public class TrackCoursesController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: TrackCourses
        public ActionResult Index()
        {
            var trackCourses = db.trackCourses.Include(t => t.Course).Include(t => t.Track);
            return View(trackCourses.ToList().OrderBy(i=>i.Track.TrackName));
        }

        // GET: TrackCourses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrackCourses trackCourses = db.trackCourses.Find(id);
            if (trackCourses == null)
            {
                return HttpNotFound();
            }
            return View(trackCourses);
        }

        // GET: TrackCourses/Create
        public ActionResult Create()
        {
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Title");
            ViewBag.TrackId = new SelectList(db.Tracks, "TrackId", "TrackName");
            return View();
        }

        // POST: TrackCourses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CourseId,TrackId")] TrackCourses trackCourses)
        {
            if (ModelState.IsValid)
            {
                var x =ControlCourse(trackCourses);
                if (x) { 
                db.trackCourses.Add(trackCourses);
                db.SaveChanges();
                return RedirectToAction("Index");
                }
                else
                    ViewBag.ErrorMessage = "This Track already have this course";
            }

            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Title", trackCourses.CourseId);
            ViewBag.TrackId = new SelectList(db.Tracks, "TrackId", "TrackName", trackCourses.TrackId);
            return View(trackCourses);
        }

        public Boolean ControlCourse(TrackCourses a)
        {
            using (var ctx = new SchoolContext())
            {
                var c = ctx.trackCourses.FirstOrDefault(i => i.TrackId == a.TrackId && i.CourseId == a.CourseId );
                if (c != null)
                {
                    return false;
                }
                return true;
            }
        }

        // GET: TrackCourses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrackCourses trackCourses = db.trackCourses.Find(id);
            if (trackCourses == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Title", trackCourses.CourseId);
            ViewBag.TrackId = new SelectList(db.Tracks, "TrackId", "TrackName", trackCourses.TrackId);
            return View(trackCourses);
        }

        // POST: TrackCourses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CourseId,TrackId")] TrackCourses trackCourses)
        {
            if (ModelState.IsValid)
            {
                var x = ControlCourse(trackCourses);
                if (x) { 
                db.Entry(trackCourses).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
                }
                else
                    ViewBag.ErrorMessage = "This Track already have this course";
            }

            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Title", trackCourses.CourseId);
            ViewBag.TrackId = new SelectList(db.Tracks, "TrackId", "TrackName", trackCourses.TrackId);
            return View(trackCourses);
        }

        // GET: TrackCourses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrackCourses trackCourses = db.trackCourses.Find(id);
            if (trackCourses == null)
            {
                return HttpNotFound();
            }
            return View(trackCourses);
        }

        // POST: TrackCourses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TrackCourses trackCourses = db.trackCourses.Find(id);
            db.trackCourses.Remove(trackCourses);
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
