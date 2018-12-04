using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSchool.Models;

namespace SmartSchool.Controllers
{
    public class AttendanceController : Controller
    {
        // GET: Attendance/EntryAttendance // by teacher
        [HttpGet]
        public ActionResult EntryAttendance()
        {
            if (Request.Cookies.Get("teacher") != null)
            {
                string id = Request.Cookies.Get("teacher").Value;
                Attendance attendance = new Attendance();
                attendance.TeacherID = id;
                attendance.Data = attendance.getStudents();

                return View(attendance);
            }
            else
            {
                return RedirectToAction("Login", "Teachers");
            }
        }

        // POST: Attendance/EntryAttendance
        [HttpPost]
        public ActionResult EntryAttendance(Attendance attendance)
        {
            string id = Request.Cookies.Get("teacher").Value;
            try
            {
                if (ModelState.IsValid)
                {
                    attendance.viewClassWiseSection();
                    attendance.TeacherID = id;
                    attendance.Data = attendance.getStudents();

                    if (attendance.entryAttendance(id))
                    {
                        ViewBag.Message = "Entry successfully";
                        ModelState.Clear();
                        return RedirectToAction("ViewAttendance");
                    }
                }
                return View(attendance);
            }
            catch
            {
                return View(attendance);
            }
        }

        // GET: Attendance/ViewAttendance // by teacher
        [HttpGet]
        public ActionResult ViewAttendance()
        {
            if (Request.Cookies.Get("teacher") != null)
            {
                string id = Request.Cookies.Get("teacher").Value;
                Attendance attendance = new Attendance();
                attendance.TeacherID = id;
                attendance.Data = attendance.viewAttendance();

                return View(attendance);
            }
            else
            {
                return RedirectToAction("Login", "Teachers");
            }
        }

        // POST: Attendance/ViewAttendance
        [HttpPost]
        public ActionResult ViewAttendance(Attendance attendance)
        {
            string id = Request.Cookies.Get("teacher").Value;
            attendance.viewClassWiseSection();
            attendance.TeacherID = id;
            attendance.Data = attendance.viewAttendance();
            return View(attendance);
        }

        // GET: Attendance/LiveAttendance // by parents
        [HttpGet]
        public ActionResult LiveAttendance()
        {
            if (Request.Cookies.Get("parents") != null)
            {
                string id = Request.Cookies.Get("parents").Value;
                Attendance attendance = new Attendance();
                attendance.StudentID = id;
                attendance.Data = attendance.liveAttendance();
                return View(attendance);
            }
            else
            {
                return RedirectToAction("Login", "Parents");
            }
        }

        // POST: Attendance/LiveAttendance
        [HttpPost]
        public ActionResult LiveAttendance(Attendance attendance)
        {
            string id = Request.Cookies.Get("parents").Value;
            attendance.StudentID = id;
            attendance.Data = attendance.liveAttendance();
            return View(attendance);
        }
    }
}