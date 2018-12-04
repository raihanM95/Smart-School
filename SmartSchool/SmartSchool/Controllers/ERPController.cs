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
            if (Request.Cookies.Get("admin") != null)
            {
                return RedirectToAction("Home", "ERP");
            }
            else if (Request.Cookies.Get("teacher") != null)
            {
                return RedirectToAction("Index", "Teachers");
            }
            else if (Request.Cookies.Get("parents") != null)
            {
                return RedirectToAction("Index", "Parents");
            }
            else if (Request.Cookies.Get("student") != null)
            {
                return RedirectToAction("Index", "Students");
            }
            else
            {
                return View();
            }
        }

        // GET: ERP/Login // for admin
        [HttpGet]
        public ActionResult Login()
        {
            if (Request.Cookies.Get("admin") != null)
            {
                return RedirectToAction("Home", "ERP");
            }
            else if (Request.Cookies.Get("teacher") != null)
            {
                return RedirectToAction("Index", "Teachers");
            }
            else if (Request.Cookies.Get("parents") != null)
            {
                return RedirectToAction("Index", "Parents");
            }
            else
            {
                return View();
            }
        }

        // POST: ERP/Login
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
                        int timeout = 1440; // 1440 min = 1 day
                        var ticket = new FormsAuthenticationTicket(admin.UserName, false, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        HttpCookie cookie = new HttpCookie("admin", encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        Response.Cookies.Add(cookie);

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
        //[Authorize]
        //[HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Response.Cookies["admin"].Expires = DateTime.Now.AddDays(-1);
            return RedirectToAction("ERP", "ERP");
        }

        // ERP/Home // by admin
        public ActionResult Home()
        {
            if (Request.Cookies.Get("admin") != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "ERP");
            }
        }
    }
}