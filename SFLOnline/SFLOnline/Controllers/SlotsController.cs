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
    public class SlotsController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Slots
        public ActionResult Index()
        {
            var slots = db.slots.OrderBy(i => i.SlotNumber).ToList();
            return View(slots);
        }

        // GET: Slots/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slot slot = db.slots.Find(id);
            if (slot == null)
            {
                return HttpNotFound();
            }
            return View(slot);
        }

        // GET: Slots/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Slots/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SlotId,SlotName,SlotNumber")] Slot slot)
        {
            if (ModelState.IsValid)
            {
                if (ControlSlot(slot)) { 
                    db.slots.Add(slot);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorMessage = "This Slot Already given";
                }
            }

            return View(slot);
        }

        public Boolean ControlSlot(Slot slot)
        {
            var control = db.slots.FirstOrDefault(i => i.SlotNumber == slot.SlotNumber);
            if (control == null)
            {
                return true;
            }
            return false;
        }

        // GET: Slots/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slot slot = db.slots.Find(id);
            if (slot == null)
            {
                return HttpNotFound();
            }
            return View(slot);
        }

        // POST: Slots/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SlotId,SlotName,SlotNumber")] Slot slot)
        {
            if (ModelState.IsValid)
            {
                if (ControlSlot(slot)) { 
                    db.Entry(slot).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorMessage = "This Slot Already given";
                }
            }
            return View(slot);
        }

        // GET: Slots/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slot slot = db.slots.Find(id);
            if (slot == null)
            {
                return HttpNotFound();
            }
            return View(slot);
        }

        // POST: Slots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Slot slot = db.slots.Find(id);
            db.slots.Remove(slot);
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
