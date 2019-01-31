using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using SFLOnline.DAL;
using SFLOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace SFLOnline.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        UserApplication userApp = new UserApplication();

        public AccountController()
        {
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Person user)
        {
            var authenticatedUser = userApp.GetByUsernameAndPassword(user);
            if (authenticatedUser != null)
            {
                var identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Email, user.EMail),
                    new Claim(ClaimTypes.NameIdentifier, authenticatedUser.Id),
                },
                    DefaultAuthenticationTypes.ApplicationCookie);

                if (authenticatedUser is Student)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, "Student"));
                }

                else if (authenticatedUser is Instructor)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, "Instructor"));
                }

                else if (authenticatedUser is Admin)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
                }

                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;

                var claimsPrincipal = new ClaimsPrincipal(identity);
                Thread.CurrentPrincipal = claimsPrincipal;

                authManager.SignIn(new AuthenticationProperties() { IsPersistent = true }, identity);

                if (authenticatedUser is Student)
                {
                    return RedirectToAction("StudentHome", "Home");
                }

                else if (authenticatedUser is Instructor)
                {
                    return RedirectToAction("InstructorHome", "Home");
                }
                else if(authenticatedUser is Admin)
                {
                    return RedirectToAction("AdminHome", "Home");
                }
            }

            // user authN failed
            ModelState.AddModelError("", "Invalid email or password");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOut()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(Person user)
        {
            var userExists = userApp.ValidateUser(user);
            if (userExists != null)
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("SFL Online", "sflyonetim@gmail.com"));
                message.To.Add(new MailboxAddress(user.FullName, user.EMail));
                message.Subject = "SFL ONLINE - Password Recovery";

                message.Body = new TextPart("plain")
                {
                    Text = @"Dear User,

Your password has been successfully reset. You can login with the password sflonlinerocks. Once logged in, you can change your password.

-- SFL ONLINE Administration"
                };

                using (var client = new SmtpClient())
                {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    client.Connect("smtp.gmail.com", 587, false);

                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate("sflyonetim@gmail.com", "Kingofsfl123");

                    client.Send(message);
                    client.Disconnect(true);
                }
                userApp.ChangePassword(userExists);
                return RedirectToAction("Login", "Account");
            }
            ModelState.AddModelError("", "No such user found.");
           return RedirectToAction("Login", "Account");
        }
    }
}