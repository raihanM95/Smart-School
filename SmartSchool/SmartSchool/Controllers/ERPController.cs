using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SmartSchool.Models;

namespace SmartSchool.Controllers
{
    public class ERPController : Controller
    {
        // ERP/ERP
        public ActionResult ERP()
        {
            return View();
        }

        // GET: /ERP/Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // POST: /ERP/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Admin admin, string ReturnUrl="")
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string message = admin.Login();
                    if (message.Equals("1"))
                    {
                        /*int timeout = admin.RememberMe ? 1440 : 20;
                        var ticket = new FormsAuthenticationTicket(admin.UserName, admin.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);*/

                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Home", "ERP");
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = message;
                    }
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        //Logout
        /*[Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "ERP");
        }*/

        // ERP/Home
        public ActionResult Home()
        {
            return View();
        }
    }
}