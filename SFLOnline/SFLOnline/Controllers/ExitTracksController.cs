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
    public class ExitTracksController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: ExitTracks
        public ActionResult Index()
        {
            var exitTracks = db.ExitTracks.Include(e => e.Exit).Include(e => e.Track);
            return View(exitTracks.ToList());
        }

        // GET: ExitTracks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExitTrack exitTrack = db.ExitTracks.Find(id);
            if (exitTrack == null)
            {
                return HttpNotFound();
            }
            return View(exitTrack);
        }

        // GET: ExitTracks/Create
        public ActionResult Create()
        {
            var exits = db.Exits.Where(i => i.ForEnrollment == true).ToList();
            var track = db.Tracks.ToList();
            List<Track> tracks = new List<Track>();
            foreach(var item in track)
            {
                var control = db.ExitTracks.FirstOrDefault(i => i.TrackId == item.TrackId);
                if (control == null)
                {
                    tracks.Add(item);
                }
            }
            ViewBag.ExitId = new SelectList(exits, "ExitId", "ExitName");
            ViewBag.TrackId = new SelectList(tracks, "TrackId", "TrackName");
            return View();
        }

        // POST: ExitTracks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ExitTrackId,ExitId,TrackId,grade")] ExitTrack exitTrack)
        {
            try { 
            if (ModelState.IsValid)
            {
                db.ExitTracks.Add(exitTrack);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var exits = db.Exits.Where(i => i.ForEnrollment == true).ToList();
            var track = db.Tracks.ToList();
            List<Track> tracks = new List<Track>();
            foreach (var item in track)
            {
                var control = db.ExitTracks.FirstOrDefault(i => i.TrackId == item.TrackId);
                if (control == null)
                {
                    tracks.Add(item);
                }
            }
            ViewBag.ExitId = new SelectList(exits, "ExitId", "ExitName");
            ViewBag.TrackId = new SelectList(tracks, "TrackId", "TrackName");
            return View(exitTrack);
        }

        // GET: ExitTracks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExitTrack exitTrack = db.ExitTracks.Find(id);
            if (exitTrack == null)
            {
                return HttpNotFound();
            }
            ViewBag.ExitId = new SelectList(db.Exits, "ExitId", "ExitName", exitTrack.ExitId);
            ViewBag.TrackId = new SelectList(db.Tracks, "TrackId", "TrackName", exitTrack.TrackId);
            return View(exitTrack);
        }

        // POST: ExitTracks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ExitTrackId,ExitId,TrackId,grade")] ExitTrack exitTrack)
        {
            if (ModelState.IsValid)
            {
                db.Entry(exitTrack).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ExitId = new SelectList(db.Exits, "ExitId", "ExitName", exitTrack.ExitId);
            ViewBag.TrackId = new SelectList(db.Tracks, "TrackId", "TrackName", exitTrack.TrackId);
            return View(exitTrack);
        }

        // GET: ExitTracks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExitTrack exitTrack = db.ExitTracks.Find(id);
            if (exitTrack == null)
            {
                return HttpNotFound();
            }
            return View(exitTrack);
        }

        // POST: ExitTracks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ExitTrack exitTrack = db.ExitTracks.Find(id);
            db.ExitTracks.Remove(exitTrack);
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
