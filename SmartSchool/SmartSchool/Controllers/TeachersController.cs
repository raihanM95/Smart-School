using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSchool.Models;

namespace SmartSchool.Controllers
{
    public class TeachersController : Controller
    {
        DataManage dam = new DataManage();
        // GET: Teachers/AddTeachersID
        [HttpGet]
        public ActionResult AddTeachersID()
        {
            return View();
        }

        // POST: Teachers/AddTeachersID
        [HttpPost]
        public ActionResult AddTeachersID(Teachers teacher)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (teacher.addTeachersID())
                    {
                        ViewBag.Message = "Teacher id record successfully";
                        ModelState.Clear();
                    }
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // Teachers/ViewTeachers
        public ActionResult ViewTeachers()
        {
            Teachers teachers = new Teachers();
            return View(teachers.viewTeachers());
        }

        // Teachers/ViewDetails
        public ActionResult ViewDetails(string id)
        {
            Teachers teacher = new Teachers();
            return View(teacher.viewDetails().Find(smodel => smodel.Id == id));
        }

        // GET: Teachers/UpdateTeacherInfo
        [HttpGet]
        public ActionResult UpdateTeacherInfo(string id)
        {
            Teachers teacher = new Teachers();
            return View(teacher.viewTeachers().Find(smodel => smodel.Id == id));
        }

        // POST: Teachers/UpdateTeacherInfo
        [HttpPost]
        public ActionResult UpdateTeacherInfo(string id, Teachers teacher)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    teacher.updateTeacherInfo();
                    return RedirectToAction("ViewTeachers");
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // Teachers/DeleteTeacher
        public ActionResult DeleteTeacher(string id)
        {
            try
            {
                Teachers teacher = new Teachers();
                if (teacher.deleteTeacher(id))
                {
                    ViewBag.AlertMsg = "Delete Successfully";
                }
                return RedirectToAction("ViewTeachers");
            }
            catch
            {
                return RedirectToAction("ViewTeachers"); ;
            }
        }

        // GET: Teachers/AssignTeachers
        [HttpGet]
        public ActionResult AssignTeacher()
        {
            Teachers teacher = new Teachers();
            return View(teacher);
        }

        // POST: Teachers/AssignTeachers
        [HttpPost]
        public ActionResult AssignTeacher(Teachers teacher)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (teacher.assignTeacher())
                    {
                        ViewBag.Message = "Assign successfully";
                        ModelState.Clear();
                    }
                }
                return View(teacher);
            }
            catch
            {
                return View(teacher);
            }
        }

        // Teachers/ViewAssign
        public ActionResult ViewAssign()
        {
            Teachers teachers = new Teachers();
            return View(teachers.viewAssign());
        }

        // GET: Teachers/UpdateAssign
        [HttpGet]
        public ActionResult UpdateAssign(int id)
        {
            Teachers teacher = new Teachers();
            return View(teacher.viewAssign().Find(smodel => smodel.AssignTableId == id));
        }

        // POST: Teachers/UpdateAssign
        [HttpPost]
        public ActionResult UpdateAssign(int id, Teachers teacher)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    teacher.updateAssign();
                    return RedirectToAction("ViewAssign");
                }
                return View(teacher);
            }
            catch
            {
                return View(teacher);
            }
        }

        // GET: /Teachers/Register
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        // POST: /Teachers/Register
        /*[HttpPost]
        public ActionResult Register()
        {
            return View();
        }*/

        // GET: /Teachers/Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // POST: /Teachers/Login
        /*[HttpPost]
        public ActionResult Login()
        {
            return View();
        }*/
    }
}