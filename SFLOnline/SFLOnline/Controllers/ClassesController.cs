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
    public class ClassesController : Controller
    {
        
        private SchoolContext db = new SchoolContext();

        // GET: Classes
        public ActionResult Index()
        {
            
            var classes = db.Classes.Include(c => c.Track).OrderBy(i=>i.Track.TrackName);
            return View(classes.ToList());
        }

        // GET: Classes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class @class = db.Classes.Find(id);
            if (@class == null)
            {
                return HttpNotFound();
            }
            return View(@class);
        }

        // GET: Classes/Create
        public ActionResult Create()
        {
            ViewBag.TrackId = new SelectList(db.Tracks, "TrackId", "TrackName");
            return View();
        }

        // POST: Classes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClassId,ClassName,Quota,RequiredQuota,TrackId")] Class @classes)
        {
            if (ModelState.IsValid)
            {
                if (ControlClass(classes)) { 

                db.Classes.Add(classes);
                db.SaveChanges();
                return RedirectToAction("Index");
                }
                else
                {

                }
            }
            
            

            ViewBag.TrackId = new SelectList(db.Tracks, "TrackId", "TrackName", @class.TrackId);
            return View(classes);
        }

        public Boolean ControlClass(Class classes)
        {
            var control = db.Classes.FirstOrDefault(i => i.ClassName == classes.ClassName);
            if (control != null)
            {
                return true;
            }
            else
                return false;
        }

        // GET: Classes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class @class = db.Classes.Find(id);
            if (@class == null)
            {
                return HttpNotFound();
            }
            ViewBag.TrackId = new SelectList(db.Tracks, "TrackId", "TrackName", @class.TrackId);
            return View(@class);
        }

        // POST: Classes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClassId,ClassName,Quota,RequiredQuota,TrackId")] Class @class)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@class).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TrackId = new SelectList(db.Tracks, "TrackId", "TrackName", @class.TrackId);
            return View(@class);
        }

        // GET: Classes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class @class = db.Classes.Find(id);
            if (@class == null)
            {
                return HttpNotFound();
            }
            return View(@class);
        }

        // POST: Classes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Class @class = db.Classes.Find(id);
            db.Classes.Remove(@class);
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
