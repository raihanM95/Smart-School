using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSchool.Models;

namespace SmartSchool.Controllers
{
    public class ParentsController : Controller
    {
        // GET: /Parents/Register
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        // POST: /Parents/Register
        /*[HttpPost]
        public ActionResult Register()
        {
            return View();
        }*/

        // GET: /Parents/Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // POST: /Parents/Login
        /*[HttpPost]
        public ActionResult Login()
        {
            return View();
        }*/
    }
}