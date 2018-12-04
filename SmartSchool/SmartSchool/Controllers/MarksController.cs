using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSchool.Models;

namespace SmartSchool.Controllers
{
    public class MarksController : Controller
    {
        // GET: Marks/EntryMarks // by teacher
        [HttpGet]
        public ActionResult EntryMarks()
        {
            if (Request.Cookies.Get("teacher") != null)
            {
                string id = Request.Cookies.Get("teacher").Value;
                Marks marks = new Marks();
                marks.TeacherID = id;
                marks.Data = marks.getStudents();
                return View(marks);
            }
            else
            {
                return RedirectToAction("Login", "Teachers");
            }
        }

        // POST: Marks/EntryMarks
        [HttpPost]
        public ActionResult EntryMarks(Marks marks)
        {
            string id = Request.Cookies.Get("teacher").Value;
            try
            {
                if (ModelState.IsValid)
                {
                    marks.viewClassWiseSection();
                    marks.viewSectionWiseSubject();
                    marks.TeacherID = id;
                    marks.Data = marks.getStudents();

                    if (marks.entryMarks(id))
                    {
                        ViewBag.Message = "Entry successfully";
                        ModelState.Clear();
                        return RedirectToAction("ViewMarks");
                    }
                }
                return View(marks);
            }
            catch
            {
                return RedirectToAction("EntryMarks");
            }
        }

        // GET: Marks/ViewMarks // by teacher
        [HttpGet]
        public ActionResult ViewMarks()
        {
            if (Request.Cookies.Get("teacher") != null)
            {
                string id = Request.Cookies.Get("teacher").Value;
                Marks marks = new Marks();
                marks.TeacherID = id;
                marks.Data = marks.viewMarks();
                return View(marks);
            }
            else
            {
                return RedirectToAction("Login", "Teachers");
            }
        }

        // POST: Marks/ViewMarks
        [HttpPost]
        public ActionResult ViewMarks(Marks marks)
        {
            try
            {
                string id = Request.Cookies.Get("teacher").Value;
                marks.viewClassWiseSection();
                marks.viewSectionWiseSubject();
                marks.TeacherID = id;
                marks.Data = marks.viewMarks();
                return View(marks);
            }
            catch
            {
                return RedirectToAction("ViewMarks");
            }
        }

        // GET: Marks/LiveResults // by student
        [HttpGet]
        public ActionResult LiveResults()
        {
            if (Request.Cookies.Get("student") != null)
            {
                string id = Request.Cookies.Get("student").Value;
                Marks marks = new Marks();
                marks.StudentID = id;
                marks.Data = marks.liveResult();
                marks.Result();
                return View(marks);
            }
            else
            {
                return RedirectToAction("Login", "Students");
            }
        }

        // POST: Marks/LiveResults
        [HttpPost]
        public ActionResult LiveResults(Marks marks)
        {
            try
            {
                string id = Request.Cookies.Get("student").Value;
                marks.StudentID = id;
                marks.Data = marks.liveResult();
                marks.Result();
                return View(marks);
            }
            catch
            {
                ViewBag.Message = "Result not found!";
                return RedirectToAction("LiveResults");
            }
        }

        // GET: Marks/LiveResult // by parents
        [HttpGet]
        public ActionResult LiveResult()
        {
            if (Request.Cookies.Get("parents") != null)
            {
                string id = Request.Cookies.Get("parents").Value;
                Marks marks = new Marks();
                marks.StudentID = id;
                marks.Data = marks.liveResult();
                marks.Result();
                return View(marks);
            }
            else
            {
                return RedirectToAction("Login", "Parents");
            }
        }

        // POST: Marks/LiveResult
        [HttpPost]
        public ActionResult LiveResult(Marks marks)
        {
            try
            {
                string id = Request.Cookies.Get("parents").Value;
                marks.StudentID = id;
                marks.Data = marks.liveResult();
                marks.Result();
                return View(marks);
            }
            catch
            {
                ViewBag.Message = "Result not found!";
                return RedirectToAction("LiveResult");
            }
        }

        // GET: Marks/Result // by all users
        [HttpGet]
        public ActionResult Result()
        {
            if (Request.Cookies.Get("admin") != null)
            {
                return RedirectToAction("Home", "ERP");
            }
            else
            {
                Marks marks = new Marks();
                marks.Data = marks.Result();
                return View(marks);
            }
        }

        // POST: Marks/Result
        [HttpPost]
        public ActionResult Result(Marks marks)
        {
            try
            {
                marks.Data = marks.Result();
                return View(marks);
            }
            catch
            {
                ViewBag.Message = "Result not found!";
                return RedirectToAction("Result");
            }
        }
    }
}