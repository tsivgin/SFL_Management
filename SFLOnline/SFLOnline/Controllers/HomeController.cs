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
    public class HomeController : Controller
    {
        private SchoolContext db = new SchoolContext();
       
        public ActionResult Index()
        {
            return RedirectToAction("Login", "Account");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [Authorize(Roles = "Instructor")]
        public ActionResult InstructorHome()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminHome()
        {
            return View();
        }
        [Authorize(Roles = "Student")]
        public ActionResult StudentHome()
        {
            var ctx = Request.GetOwinContext();

            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

            // Get the claims values
            var id = identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                               .Select(c => c.Value).SingleOrDefault();  //Emaili getirdi, ID için ClaimTypes.NameIdentifier yaz Email yerine
            var classes = db.EnrollmentsStudent.FirstOrDefault(i => i.StudentId == id);

            var announcement = db.announcements.Where(i=>i.ClassId==classes.ClassId).OrderByDescending(i=>i.AnnouncementId);
            var module = db.modules.FirstOrDefault(i => i.active == true);
            var studentclass = db.EnrollmentsStudent.FirstOrDefault(i => i.StudentId == id);
            ViewBag.studentclass = studentclass.Class.ClassName;
            ViewBag.module = module.ModuleName;
            return View(announcement);
        }
        [Authorize(Roles = "Instructor")]
        public ActionResult InstructorAccount()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult AdminAccount()
        {
            var ctx = Request.GetOwinContext();

            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

            // Get the claims values
            var id = identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                               .Select(c => c.Value).SingleOrDefault();  //Emaili getirdi, ID için ClaimTypes.NameIdentifier yaz Email yerine


            var person = db.Persons.FirstOrDefault(i => i.Id == id);

            return View(person);
        }
        [Authorize(Roles = "StudentAccount")]
        public ActionResult StudentAccount()
        {
            var ctx = Request.GetOwinContext();

            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

            // Get the claims values
            var id = identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                               .Select(c => c.Value).SingleOrDefault();  //Emaili getirdi, ID için ClaimTypes.NameIdentifier yaz Email yerine


            var module = db.modules.FirstOrDefault(i => i.active == true);
            var studentclass = db.EnrollmentsStudent.FirstOrDefault(i => i.StudentId == id);
            ViewBag.studentclass = studentclass.Class.ClassName;
            ViewBag.module = module.ModuleName;
            var person = db.Persons.FirstOrDefault(i => i.Id == id);
            return View(person);
        }
    }
}