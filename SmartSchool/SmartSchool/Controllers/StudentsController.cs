using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSchool.Models;

namespace SmartSchool.Controllers
{
    public class StudentsController : Controller
    {
        // Students/ViewStudents
        public ActionResult ViewStudents()
        {
            Students students = new Students();
            return View(students.viewStudents());
        }

        // GET: Students/AssignIntoClass
        [HttpGet]
        public ActionResult AssignIntoClass()
        {
            Students student = new Students();
            return View(student);
        }

        // POST: Students/AssignIntoClass
        [HttpPost]
        public ActionResult AssignIntoClass(Students student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (student.assignIntoClass())
                    {
                        ViewBag.Message = "Assign successfully";
                        ModelState.Clear();
                    }
                }
                return View(student);
            }
            catch
            {
                return View(student);
            }
        }

        // Students/ViewAssign
        public ActionResult ViewAssign()
        {
            Students students = new Students();
            return View(students.viewAssign());
        }

        // GET: Students/UpdateClass
        [HttpGet]
        public ActionResult UpdateClass(int id)
        {
            Students student = new Students();
            return View(student.viewAssign().Find(smodel => smodel.AssignTableId == id));
        }

        // POST: Students/UpdateClass
        [HttpPost]
        public ActionResult UpdateClass(int id, Students student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    student.updateClass();
                    return RedirectToAction("ViewAssign");
                }
                return View(student);
            }
            catch
            {
                return View(student);
            }
        }

        // GET: /Students/Register
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        // POST: /Students/Register
        /*[HttpPost]
        public ActionResult Register()
        {
            return View();
        }*/

        // GET: /Students/Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // POST: /Students/Login
        /*[HttpPost]
        public ActionResult Login()
        {
            return View();
        }*/
    }
}