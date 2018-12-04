using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartSchool.Models
{
    public class Attendance
    {
        DataManage dam = new DataManage();

        //[Required(ErrorMessage = "required")]
        [Display(Name = "Attendance")]
        public bool Attendances { get; set; }

        //[Required(ErrorMessage = "Date required")]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime? ADate { get; set; }

        public string TeacherID { get; set; }

        //[Required(ErrorMessage = "Id required")]
        [Display(Name = "Student Id")]
        public string StudentID { get; set; }

        //[Required(ErrorMessage = "Year required")]
        [Display(Name = "Year")]
        public string Year { get; set; }

        // For DropDownlist
        public List<SelectListItem> Years { get; set; }

        //[Required(ErrorMessage = "Roll required")]
        [Display(Name = "Roll")]
        public int Roll { get; set; }

        //[Required(ErrorMessage = "Class required")]
        [Display(Name = "Class")]
        public string ClassNo { get; set; }

        // For DropDownlist
        public List<SelectListItem> Classes { get; set; }

        //[Required(ErrorMessage = "Section required")]
        [Display(Name = "Section")]
        public string Section { get; set; }

        // For DropDownlist
        public List<SelectListItem> Sections { get; set; }

        // For Search
        public string Search { get; set; }

        public storeAttendance storeAttendance { get; set; }
        public Attendance[] viewModels { get; set; }
        // For "entryAttendance" method values store and view in "EntryAttendance" html page
        public List<Attendance> Data { get; set; }

        public Attendance()
        {
            // For DropDownlist
            Years = new List<SelectListItem>()
            {
                new SelectListItem {
                    Text = "2018", Value = "2018"
                },
                new SelectListItem {
                    Text = "2019", Value = "2019"
                },
                new SelectListItem {
                    Text = "2020", Value = "2020"
                },
                new SelectListItem {
                    Text = "2021", Value = "2021"
                }
            };

            Classes = new List<SelectListItem>();
            string query2 = @"SELECT ClassNo FROM Class";
            foreach (DataRow dr in dam.GetDataTable(query2).Rows)
            {
                Classes.Add(new SelectListItem
                {
                    Text = Convert.ToString(dr["ClassNo"]),
                    Value = Convert.ToString(dr["ClassNo"])
                });
            }

            Sections = new List<SelectListItem>();
            string query3 = @"SELECT Section.SectionNo, Class.ClassNo
                              FROM Section
                              INNER JOIN Class ON Section.ClassID = Class.Id
                              WHERE ClassNo = '" + ClassNo + "'";
            foreach (DataRow dr in dam.GetDataTable(query3).Rows)
            {
                Sections.Add(new SelectListItem
                {
                    Text = Convert.ToString(dr["SectionNo"]),
                    Value = Convert.ToString(dr["SectionNo"])
                });
            }
        }

        public List<SelectListItem> viewClassWiseSection()
        {
            Sections = new List<SelectListItem>();
            string query = @"SELECT Section.SectionNo, Class.ClassNo
                              FROM Section
                              INNER JOIN Class ON Section.ClassID = Class.Id
                              WHERE ClassNo = '" + ClassNo + "'";
            foreach (DataRow dr in dam.GetDataTable(query).Rows)
            {
                Sections.Add(new SelectListItem
                {
                    Text = Convert.ToString(dr["SectionNo"]),
                    Value = Convert.ToString(dr["SectionNo"])
                });
            }
            return Sections;
        }

        public List<Attendance> getStudents()
        {
            List<Attendance> entrylist = new List<Attendance>();
            string query = @"SELECT Assign_Class.StudentID, Assign_Class.Year, Assign_Class.Roll, Class.ClassNo, Section.SectionNo
                             FROM (((Assign_Class
                             INNER JOIN Class ON Assign_Class.ClassID = Class.Id)
                             INNER JOIN Section ON Assign_Class.SectionID = Section.Id)
                             INNER JOIN Students ON Assign_Class.StudentID = Students.Id)
                             INNER JOIN Assign_Subject ON Assign_Class.ClassID = Assign_Subject.ClassID AND Assign_Class.SectionID = Assign_Subject.SectionID
                             WHERE ClassNo = '" + ClassNo + "' AND SectionNo = '" + Section + "' AND TeacherID = '" + TeacherID + "' AND Year = '" + Year + "'";

            foreach (DataRow dr in dam.GetDataTable(query).Rows)
            {
                entrylist.Add(new Attendance
                {
                    StudentID = Convert.ToString(dr["StudentID"]),
                    ClassNo = Convert.ToString(dr["ClassNo"]),
                    Section = Convert.ToString(dr["SectionNo"]),
                    Year = Convert.ToString(dr["Year"]),
                    Roll = Convert.ToInt32(dr["Roll"])
                });
            }
            return entrylist;
        }

        int i;
        public bool entryAttendance(string id)
        {
            foreach (var viewModel in viewModels)
            {
                string query = @"INSERT INTO Attendance_Class" + ClassNo + " (TeacherID, StudentID, Roll, Section, ADate, Attendances, Year) VALUES ('" + id + "','" + viewModel.StudentID + "', " + viewModel.Roll + ", '" + Section + "', '" + ADate + "', '" + viewModel.Attendances + "', '" + Year + "')";
                i = dam.Execute(query);
            }

            if (i >= 1)
                return true;
            else
                return false;
        }

        public List<Attendance> viewAttendance()
        {
            List<Attendance> attendancelist = new List<Attendance>();
            string query = @"SELECT* FROM Attendance_Class" + ClassNo + " WHERE TeacherID = '" + TeacherID + "' AND Section = '" + Section + "' AND Year = '" + Year + "' AND Attendances = 'True' AND ADate LIKE '%" + Search + "%'";

            foreach (DataRow dr in dam.GetDataTable(query).Rows)
            {
                attendancelist.Add(new Attendance
                {
                    StudentID = Convert.ToString(dr["StudentID"]),
                    Roll = Convert.ToInt32(dr["Roll"]),
                    ADate = Convert.ToDateTime(dr["ADate"])
                });
            }
            return attendancelist;
        }

        public List<Attendance> liveAttendance()
        {
            List<Attendance> attendancelist = new List<Attendance>();
            string query = @"SELECT* FROM Attendance_Class" + ClassNo + " WHERE StudentID = '" + StudentID + "' AND Attendances = 'True' AND ADate LIKE '%" + Search + "%'";

            foreach (DataRow dr in dam.GetDataTable(query).Rows)
            {
                attendancelist.Add(new Attendance
                {
                    ADate = Convert.ToDateTime(dr["ADate"])
                });
            }
            return attendancelist;
        }
    }

    public class storeAttendance
    {
        public bool[] Attendancess { get; set; }
        public string[] StudentIDs { get; set; }
        public int[] Rolls { get; set; }
    }
}