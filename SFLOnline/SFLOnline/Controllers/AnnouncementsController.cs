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
using System.Security.Claims;
using System.Threading;

namespace SFLOnline.Controllers
{
    public class AnnouncementsController : Controller
    {
        private SchoolContext db = new SchoolContext();
        static string id = "";

        // GET: Announcements
        public ActionResult Index()
        {
            var ctx = Request.GetOwinContext();

            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

            // Get the claims values
            id = identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                                   .Select(c => c.Value).SingleOrDefault();  //Emaili getirdi, ID için ClaimTypes.NameIdentifier yaz Email yerine

            var announcements = db.announcements.Include(a => a.Person).ToList();
            List<Announcement> announcement = new List<Announcement>();
            var check = true;
            foreach(var item in announcements)
            {
                if(announcement.Count() != 0) { 
                    foreach(var item2 in announcement)
                    {
                        if(item.AnnouncementName.ToString().Equals(item2.AnnouncementName.ToString()) && item.description.ToString().Equals(item2.description.ToString()))
                        {
                            check = false;  
                        }
                    }
                    if (check == true)
                    {
                        announcement.Add(item);
                    }
                    check = true;
                }
                else
                {
                    announcement.Add(item);
                }
            }

            return View(announcement);
        }

        // GET: Announcements/Details/5
        public ActionResult Details(int? id)
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

        // GET: Announcements/Create
        public ActionResult Create()
        {
            var Persons = db.Persons.Where(i => i.Id == id).ToList();
            ViewBag.WriterId = new SelectList(Persons, "Id", "Id");
            return View();
        }

        // POST: Announcements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AnnouncementId,WriterId,AnnouncementName,description")] Announcement announcement)
        {
            if (ModelState.IsValid)
            {
                var classes = db.Classes.ToList();
                foreach(var item in classes) {
                    announcement.ClassId = item.ClassId;
                    db.announcements.Add(announcement);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            ViewBag.WriterId = new SelectList(db.Persons, "Id", "LastName", announcement.WriterId);
            return View(announcement);
        }

        // GET: Announcements/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.WriterId = new SelectList(db.Persons, "Id", "LastName", announcement.WriterId);
            return View(announcement);
        }

        // POST: Announcements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AnnouncementId,WriterId,AnnouncementName,description")] Announcement announcement)
        {
            if (ModelState.IsValid)
            {

                db.Entry(announcement).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.WriterId = new SelectList(db.Persons, "Id", "LastName", announcement.WriterId);
            return View(announcement);
        }

        // GET: Announcements/Delete/5
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

        // POST: Announcements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Announcement announcement = db.announcements.Find(id);
            var announcements = db.announcements.Where(i => i.AnnouncementName == announcement.AnnouncementName && i.description == announcement.description).ToList();
            foreach(var item in announcements)
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
