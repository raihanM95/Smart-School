using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartSchool.Models
{
    public class Subject
    {
        DataManage dam = new DataManage();
        //Teachers teacher = new Teachers();
        //Class cls = new Class();

        public int Id { get; set; }

        public int SubjectID { get; set; }

        //[Required(ErrorMessage = "Subject required")]
        [Display(Name = "Subject")]
        public string SubjectName { get; set; }

        // For DropDownlist
        public List<SelectListItem> Subjects { get; set; }

        public string TeacherID { get; set; }

        //[Required(ErrorMessage = "Initial required")]
        [Display(Name = "Initial")]
        public string Initial { get; set; }

        // For DropDownlist
        public List<SelectListItem> Initials { get; set; }

        public int ClassID { get; set; }

        //[Required(ErrorMessage = "Class required")]
        [Display(Name = "Class")]
        public string ClassNo { get; set; }

        // For DropDownlist
        public List<SelectListItem> Classes { get; set; }

        public int SectionID { get; set; }

        //[Required(ErrorMessage = "Section required")]
        [Display(Name = "Section")]
        public string Section { get; set; }

        // For DropDownlist
        public List<SelectListItem> Sections { get; set; }

        // For "viewSubject" method values store and view in "ViewSubject" html page
        public List<Subject> Data { get; set; }

        public Subject()
        {
            // For DropDownlist
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

            Initials = new List<SelectListItem>();
            string query2 = @"SELECT Initial FROM Teachers";
            foreach (DataRow dr in dam.GetDataTable(query2).Rows)
            {
                Initials.Add(new SelectListItem
                {
                    Text = Convert.ToString(dr["Initial"]),
                    Value = Convert.ToString(dr["Initial"])
                });
            }

            Classes = new List<SelectListItem>();
            string query3 = @"SELECT ClassNo FROM Class";
            foreach (DataRow dr in dam.GetDataTable(query3).Rows)
            {
                Classes.Add(new SelectListItem
                {
                    Text = Convert.ToString(dr["ClassNo"]),
                    Value = Convert.ToString(dr["ClassNo"])
                });
            }

            Sections = new List<SelectListItem>();
            string query4 = @"SELECT Section.SectionNo, Class.ClassNo
                              FROM Section
                              INNER JOIN Class ON Section.ClassID = Class.Id
                              WHERE ClassNo = '" + ClassNo + "'";
            foreach (DataRow dr in dam.GetDataTable(query4).Rows)
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
            string query4 = @"SELECT Section.SectionNo, Class.ClassNo
                              FROM Section
                              INNER JOIN Class ON Section.ClassID = Class.Id
                              WHERE ClassNo = '" + ClassNo + "'";
            foreach (DataRow dr in dam.GetDataTable(query4).Rows)
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

        public bool addSubject()
        {
            string query = @"SELECT* FROM Class WHERE ClassNo = '" + ClassNo + "'";

            foreach (DataRow dr in dam.GetDataTable(query).Rows)
            {
                ClassID = Convert.ToInt32(dr["Id"]);
            }

            string query2 = @"SELECT* FROM Section WHERE ClassID = " + ClassID + " AND SectionNo = '" + Section + "'";

            foreach (DataRow dr in dam.GetDataTable(query2).Rows)
            {
                SectionID = Convert.ToInt32(dr["Id"]);
            }

            string query3 = @"INSERT INTO Subject (ClassID, SectionID, SubjectName) VALUES (" + ClassID + ", " + SectionID + ", '" + SubjectName + "')";

            int i = dam.Execute(query3);

            if (i >= 1)
                return true;
            else
                return false;
        }

        public List<Subject> viewSubject()
        {
            List<Subject> subjectlist = new List<Subject>();

            string query = @"SELECT Subject.Id, Subject.SubjectName, Class.ClassNo, Section.SectionNo
                             FROM ((Subject
                             INNER JOIN Class ON Subject.ClassID = Class.Id)
                             INNER JOIN Section ON Subject.SectionID = Section.Id)
                             WHERE ClassNo = '" + ClassNo + "' AND SectionNo = '" + Section + "'";

            foreach (DataRow dr in dam.GetDataTable(query).Rows)
            {
                subjectlist.Add(new Subject
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    SubjectName = Convert.ToString(dr["SubjectName"]),
                });
            }
            return subjectlist;
        }

        public bool deleteSubject(string id)
        {
            string query = @"DELETE FROM Subject WHERE Id = '" + id + "'";

            int i = dam.Execute(query);

            if (i >= 1)
                return true;
            else
                return false;
        }

        public bool assignSubject()
        {
            string query = @"SELECT* FROM Class WHERE ClassNo = '" + ClassNo + "'";

            foreach (DataRow dr in dam.GetDataTable(query).Rows)
            {
                ClassID = Convert.ToInt32(dr["Id"]);
            }

            string query2 = @"SELECT* FROM Section WHERE ClassID = " + ClassID + " AND SectionNo = '" + Section + "'";

            foreach (DataRow dr in dam.GetDataTable(query2).Rows)
            {
                SectionID = Convert.ToInt32(dr["Id"]);
            }

            string query3 = @"SELECT* FROM Subject WHERE ClassID = " + ClassID + " AND SectionID = " + SectionID + " AND SubjectName = '" + SubjectName + "'";

            foreach (DataRow dr in dam.GetDataTable(query3).Rows)
            {
                SubjectID = Convert.ToInt32(dr["Id"]);
            }

            string query4 = @"SELECT* FROM Teachers WHERE Initial = '" + Initial + "'";

            foreach (DataRow dr in dam.GetDataTable(query4).Rows)
            {
                TeacherID = Convert.ToString(dr["Id"]);
            }

            string query5 = @"INSERT INTO Assign_Subject (ClassID, SectionID, SubjectID, TeacherID) VALUES (" + ClassID + ", " + SectionID + ", " + SubjectID + ", '" + TeacherID + "')";

            int i = dam.Execute(query5);

            if (i >= 1)
                return true;
            else
                return false;
        }

        public List<Subject> viewAssign()
        {
            List<Subject> assignlist = new List<Subject>();
            string query = @"SELECT Assign_Subject.Id, Class.ClassNo, Section.SectionNo, Teachers.Initial, Subject.SubjectName
                             FROM (((Assign_Subject
                             INNER JOIN Class ON Assign_Subject.ClassID = Class.Id)
                             INNER JOIN Section ON Assign_Subject.SectionID = Section.Id)
                             INNER JOIN Teachers ON Assign_Subject.TeacherID = Teachers.Id)
                             INNER JOIN Subject ON Assign_Subject.SubjectID = Subject.Id
                             WHERE ClassNo = '" + ClassNo + "' AND SectionNo = '" + Section + "'";

            foreach (DataRow dr in dam.GetDataTable(query).Rows)
            {
                assignlist.Add(new Subject
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    SubjectName = Convert.ToString(dr["SubjectName"]),
                    Initial = Convert.ToString(dr["Initial"]),
                    ClassNo = Convert.ToString(dr["ClassNo"]),
                    Section = Convert.ToString(dr["SectionNo"]),
                });
            }
            return assignlist;
        }

        public bool deleteAssign(string id)
        {
            string query = @"DELETE FROM Assign_Subject WHERE Id = '" + id + "'";

            int i = dam.Execute(query);

            if (i >= 1)
                return true;
            else
                return false;
        }
    }
}