using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSchool.Models;

namespace SmartSchool.Controllers
{
    public class ClassController : Controller
    {
        // GET: Class/AddClass // by admin
        [HttpGet]
        public ActionResult AddClass()
        {
            if (Request.Cookies.Get("admin") != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("ERP", "ERP");
            }
        }

        // POST: Class/AddClass
        [HttpPost]
        public ActionResult AddClass(Class cls)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (cls.addClass())
                    {
                        ViewBag.Message = "Add successfully";
                        ModelState.Clear();
                    }
                }
                return View(cls);
            }
            catch
            {
                return View(cls);
            }
        }

        // GET: Class/AddSection // by admin
        [HttpGet]
        public ActionResult AddSection()
        {
            if (Request.Cookies.Get("admin") != null)
            {
                Class cls = new Class();
                return View(cls);
            }
            else
            {
                return RedirectToAction("ERP", "ERP");
            }
        }

        // POST: Class/AddSection
        [HttpPost]
        public ActionResult AddSection(Class cls)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (cls.addSection())
                    {
                        ViewBag.Message = "Add successfully";
                        ModelState.Clear();
                    }
                }
                return View(cls);
            }
            catch
            {
                return View(cls);
            }
        }

        // GET: Class/ViewClass // by admin
        [HttpGet]
        public ActionResult ViewClass()
        {
            if (Request.Cookies.Get("admin") != null)
            {
                Class cls = new Class();
                cls.Data = cls.viewClass();
                return View(cls);
            }
            else
            {
                return RedirectToAction("ERP", "ERP");
            }
        }

        // POST: Class/ViewClass
        [HttpPost]
        public ActionResult ViewClass(Class cls)
        {
            cls.Data = cls.viewClass();
            return View(cls);
        }

        // Class/ViewClass // by admin
        public ActionResult DeleteSection(string id)
        {
            try
            {
                Class cls = new Class();
                if (cls.deleteSection(id))
                {
                    ViewBag.AlertMsg = "Delete Successfully";
                }
                return RedirectToAction("ViewClass");
            }
            catch
            {
                return RedirectToAction("ViewClass"); ;
            }
        }

        // GET: Class/AssignIntoClass // by admin
        [HttpGet]
        public ActionResult AssignIntoClass()
        {
            if (Request.Cookies.Get("admin") != null)
            {
                Class cls = new Class();
                return View(cls);
            }
            else
            {
                return RedirectToAction("ERP", "ERP");
            }
        }

        // POST: Class/AssignIntoClass
        [HttpPost]
        public ActionResult AssignIntoClass(Class cls)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    cls.viewClassWiseSection();
                    if (cls.assignIntoClass())
                    {
                        ViewBag.Message = "Assign successfully";
                        ModelState.Clear();
                    }
                }
                return View(cls);
            }
            catch
            {
                return View(cls);
            }
        }

        // GET: Class/ViewAssign // by admin
        [HttpGet]
        public ActionResult ViewAssign()
        {
            if (Request.Cookies.Get("admin") != null)
            {
                Class cls = new Class();
                cls.Data = cls.viewAssign();
                return View(cls);
            }
            else
            {
                return RedirectToAction("ERP", "ERP");
            }
        }

        // POST: Class/ViewAssign
        [HttpPost]
        public ActionResult ViewAssign(Class cls)
        {
            cls.Data = cls.viewAssign();
            cls.Sections = cls.viewClassWiseSection();
            return View(cls);
        }

        // Class/ViewAssign // by admin
        public ActionResult DeleteAssign(string id)
        {
            try
            {
                Class cls = new Class();
                if (cls.deleteAssign(id))
                {
                    ViewBag.AlertMsg = "Delete Successfully";
                }
                return RedirectToAction("ViewAssign");
            }
            catch
            {
                return RedirectToAction("ViewAssign"); ;
            }
        }
    }
}