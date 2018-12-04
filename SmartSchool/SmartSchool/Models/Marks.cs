using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartSchool.Models
{
    public class Marks
    {
        DataManage dam = new DataManage();

        //[Required(ErrorMessage = "Exam required")]
        [Display(Name = "Exam")]
        public string Exam { get; set; }

        // For DropDownlist
        public List<SelectListItem> Exams { get; set; }

        //[Required(ErrorMessage = "Marks required")]
        [Display(Name = "Marks")]
        public int Mark { get; set; }

        [Display(Name = "Mid marks")]
        public int Mid { get; set; }

        [Display(Name = "Mid total: ")]
        public int TotalMid { get; set; }

        [Display(Name = "Final marks")]
        public int Final { get; set; }

        [Display(Name = "Final total: ")]
        public int TotalFinal { get; set; }

        //[Required(ErrorMessage = "Subject required")]
        [Display(Name = "Subject")]
        public string SubjectName { get; set; }

        // For DropDownlist
        public List<SelectListItem> Subjects { get; set; }

        public string TeacherID { get; set; }

        //[Required(ErrorMessage = "Id required")]
        [Display(Name = "Student Id")]
        public string StudentID { get; set; }

        //[Required(ErrorMessage = "Name required")]
        //[Display(Name = "Student name")]
        //public string StudentName { get; set; }

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

        public storeMarks storeMarks { get; set; }
        public Marks[] viewModels { get; set; }
        // For "entryMarks" method values store and view in "EntryMarks" html page
        public List<Marks> Data { get; set; }

        public Marks()
        {
            // For DropDownlist
            Exams = new List<SelectListItem>()
            {
                new SelectListItem {
                    Text = "Mid", Value = "Mid"
                },
                new SelectListItem {
                    Text = "Final", Value = "Final"
                }
            };

            Subjects = new List<SelectListItem>();
            string query = @"SELECT Subject.Id, Subject.SubjectName, Class.ClassNo, Section.SectionNo
                             FROM ((Subject
                             INNER JOIN Class ON Subject.ClassID = Class.Id)
                             INNER JOIN Section ON Subject.SectionID = Section.Id)
                             WHERE ClassNo = '" + ClassNo + "' AND SectionNo = '" + Section + "'";
            foreach (DataRow dr in dam.GetDataTable(query).Rows)
            {
                Subjects.Add(new SelectListItem
                {
                    Text = Convert.ToString(dr["SubjectName"]),
                    Value = Convert.ToString(dr["SubjectName"])
                });
            }

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

        public List<SelectListItem> viewSectionWiseSubject()
        {
            Subjects = new List<SelectListItem>();
            string query = @"SELECT Subject.Id, Subject.SubjectName, Class.ClassNo, Section.SectionNo
                             FROM ((Subject
                             INNER JOIN Class ON Subject.ClassID = Class.Id)
                             INNER JOIN Section ON Subject.SectionID = Section.Id)
                             WHERE ClassNo = '" + ClassNo + "' AND SectionNo = '" + Section + "'";
            foreach (DataRow dr in dam.GetDataTable(query).Rows)
            {
                Subjects.Add(new SelectListItem
                {
                    Text = Convert.ToString(dr["SubjectName"]),
                    Value = Convert.ToString(dr["SubjectName"])
                });
            }
            return Subjects;
        }

        public List<Marks> getStudents()
        {
            List<Marks> entrylist = new List<Marks>();
            string query = @"SELECT Assign_Class.StudentID, Assign_Class.Year, Assign_Class.Roll, Class.ClassNo, Section.SectionNo
                             FROM (((Assign_Class
                             INNER JOIN Class ON Assign_Class.ClassID = Class.Id)
                             INNER JOIN Section ON Assign_Class.SectionID = Section.Id)
                             INNER JOIN Students ON Assign_Class.StudentID = Students.Id)
                             INNER JOIN Assign_Subject ON Assign_Class.ClassID = Assign_Subject.ClassID AND Assign_Class.SectionID = Assign_Subject.SectionID
                             WHERE ClassNo = '" + ClassNo + "' AND SectionNo = '" + Section + "' AND TeacherID = '" + TeacherID + "' AND Year = '" + Year + "'";

            foreach (DataRow dr in dam.GetDataTable(query).Rows)
            {
                entrylist.Add(new Marks
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
        public bool entryMarks(string id)
        {
            if(Exam == "Mid")
            {
                foreach(var viewModel in viewModels)
                {
                    string query = @"INSERT INTO Mark_Class" + ClassNo + " (TeacherID, StudentID, Roll, Section, SubjectName, Mid, Year) VALUES ('" + id + "','" + viewModel.StudentID + "', " + viewModel.Roll + ", '" + Section + "', '" + SubjectName + "', " + viewModel.Mark + ", '" + Year + "')";
                    i = dam.Execute(query);
                }
            }
            else if(Exam == "Final")
            {
                foreach (var viewModel in viewModels)
                {
                    string query2 = @"UPDATE Mark_Class" + ClassNo + " SET Final = " + viewModel.Mark + " WHERE TeacherID = '" + id + "' AND StudentID = '" + viewModel.StudentID + "' AND Roll = " + viewModel.Roll + " AND Section = '" + Section + "' AND SubjectName = '" + SubjectName + "' AND Year = '" + Year + "'";

                    i = dam.Execute(query2);
                }
            }
            
            if (i >= 1)
                return true;
            else
                return false;
        }

        public List<Marks> viewMarks()
        {
            List<Marks> markslist = new List<Marks>();
            string query = @"SELECT* FROM Mark_Class" + ClassNo + " WHERE TeacherID = '" + TeacherID + "' AND Section = '" + Section + "' AND SubjectName = '" + SubjectName + "' AND Year = '" + Year + "'";

            foreach (DataRow dr in dam.GetDataTable(query).Rows)
            {
                markslist.Add(new Marks
                {
                    StudentID = Convert.ToString(dr["StudentID"]),
                    Roll = Convert.ToInt32(dr["Roll"]),
                    Mid = Convert.ToInt32(dr["Mid"]),
                    Final = Convert.ToInt32(dr["Final"])
                });
            }
            return markslist;
        }

        public List<Marks> liveResult()
        {
            List<Marks> markslist = new List<Marks>();
            string query = @"SELECT* FROM Mark_Class" + ClassNo + " WHERE StudentID = '" + StudentID + "'";

            foreach (DataRow dr in dam.GetDataTable(query).Rows)
            {
                markslist.Add(new Marks
                {
                    StudentID = Convert.ToString(dr["StudentID"]),
                    Roll = Convert.ToInt32(dr["Roll"]),
                    Section = Convert.ToString(dr["Section"]),
                    SubjectName = Convert.ToString(dr["SubjectName"]),
                    Mid = Convert.ToInt32(dr["Mid"]),
                    Final = Convert.ToInt32(dr["Final"])
                });
            }
            return markslist;
        }

        public List<Marks> Result()
        {
            List<Marks> markslist = new List<Marks>();
            string query = @"SELECT SUM(Mid) AS Mid, SUM(Final) AS Final FROM Mark_Class" + ClassNo + " WHERE StudentID = '" + StudentID + "'";

            foreach (DataRow dr in dam.GetDataTable(query).Rows)
            {
                markslist.Add(new Marks
                {
                    TotalMid = Convert.ToInt32(dr["Mid"]),
                    TotalFinal = Convert.ToInt32(dr["Final"])
                });
            }
            return markslist;
        }
    }

    public class storeMarks
    {
        public int[] Markss { get; set; }
        public string[] StudentIDs { get; set; }
        public int[] Rolls { get; set; }
    }
}