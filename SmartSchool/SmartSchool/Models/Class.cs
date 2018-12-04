using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartSchool.Models
{
    public class Class
    {
        DataManage dam = new DataManage();
        //Students students = new Students();

        public int Id { get; set; }

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

        //[Required(ErrorMessage = "Please enter your id")]
        [Display(Name = "Student Id")]
        public string StudentId { get; set; }

        // For DropDownlist
        public List<SelectListItem> StudentIds { get; set; }

        //[Required(ErrorMessage = "Year required")]
        [Display(Name = "Year")]
        public string Year { get; set; }

        // For DropDownlist
        public List<SelectListItem> Years { get; set; }

        //[Required(ErrorMessage = "Roll required")]
        [Display(Name = "Roll")]
        public int Roll { get; set; }

        // For "viewAssign" method values store and view in "ViewClass" html page
        public List<Class> Data { get; set; }

        public Class()
        {
            // For DropDownlist
            Classes = new List<SelectListItem>();
            string query = @"SELECT ClassNo FROM Class";
            foreach (DataRow dr in dam.GetDataTable(query).Rows)
            {
                Classes.Add(new SelectListItem
                {
                    Text = Convert.ToString(dr["ClassNo"]),
                    Value = Convert.ToString(dr["ClassNo"])
                });
            }

            Sections = new List<SelectListItem>();
            string query2 = @"SELECT Section.SectionNo, Class.ClassNo
                              FROM Section
                              INNER JOIN Class ON Section.ClassID = Class.Id
                              WHERE ClassNo = '" + ClassNo + "'";
            foreach (DataRow dr in dam.GetDataTable(query2).Rows)
            {
                Sections.Add(new SelectListItem
                {
                    Text = Convert.ToString(dr["SectionNo"]),
                    Value = Convert.ToString(dr["SectionNo"])
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

            StudentIds = new List<SelectListItem>();
            string query3 = @"SELECT Id FROM Students";
            foreach (DataRow dr in dam.GetDataTable(query3).Rows)
            {
                StudentIds.Add(new SelectListItem
                {
                    Text = Convert.ToString(dr["Id"]),
                    Value = Convert.ToString(dr["Id"])
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

        public bool addClass()
        {
            string query = @"INSERT INTO Class (ClassNo) VALUES ('" + ClassNo + "')";
            string query2 = @"CREATE TABLE Mark_Class"+ ClassNo + " (Id int IDENTITY(1,1) primary key, TeacherID nvarchar (10) not null foreign key references Teachers(Id), StudentID nvarchar (10) not null foreign key references Students(Id), Roll int not null, Section nvarchar (1) not null, SubjectName nvarchar (100) not null, Mid int, Final int, Year nvarchar (4) not null)";
            string query3 = @"CREATE TABLE Attendance_Class" + ClassNo + " (Id int IDENTITY(1,1) primary key, TeacherID nvarchar (10) not null foreign key references Teachers(Id), StudentID nvarchar (10) not null foreign key references Students(Id), Roll int not null, Section nvarchar (1) not null, ADate date, Attendances bit, Year nvarchar (4) not null)";
            int i = dam.Execute(query);
            i = dam.Execute(query2);
            i = dam.Execute(query3);

            if (i >= 1)
                return true;
            else
                return false;

        }

        public bool addSection()
        {
            string query = @"SELECT* FROM Class WHERE ClassNo = '" + ClassNo + "'";

            foreach (DataRow dr in dam.GetDataTable(query).Rows)
            {
                Id = Convert.ToInt32(dr["Id"]);
            }
            string query2 = @"INSERT INTO Section (ClassID, SectionNo) VALUES (" + Id + ",'" + Section + "')";

            int i = dam.Execute(query2);

            if (i >= 1)
                return true;
            else
                return false;
        }

        public List<Class> viewClass()
        {
            List<Class> classlist = new List<Class>();

            string query = @"SELECT Section.Id, Section.SectionNo, Class.ClassNo
                             FROM Section
                             INNER JOIN Class ON Section.ClassID = Class.Id
                             WHERE ClassNo = '" + ClassNo + "'";

            foreach (DataRow dr in dam.GetDataTable(query).Rows)
            {
                classlist.Add(new Class
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    Section = Convert.ToString(dr["SectionNo"]),
                });
            }
            return classlist;
        }

        public bool deleteSection(string id)
        {
            string query = @"DELETE FROM Section WHERE Id = '" + id + "'";

            int i = dam.Execute(query);

            if (i >= 1)
                return true;
            else
                return false;
        }

        public bool assignIntoClass()
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

            string query3 = @"INSERT INTO Assign_Class (ClassID, SectionID, StudentID, Year, Roll) VALUES (" + ClassID + ", " + SectionID + ", '" + StudentId + "', '" + Year + "', " + Roll + ")";

            int i = dam.Execute(query3);

            if (i >= 1)
                return true;
            else
                return false;
        }

        public List<Class> viewAssign()
        {
            List<Class> assignlist = new List<Class>();
            string query = @"SELECT Assign_Class.Id, Assign_Class.Year, Assign_Class.Roll, Assign_Class.StudentID, Class.ClassNo, Section.SectionNo
                             FROM ((Assign_Class
                             INNER JOIN Class ON Assign_Class.ClassID = Class.Id)
                             INNER JOIN Section ON Assign_Class.SectionID = Section.Id)
                             INNER JOIN Students ON Assign_Class.StudentID = Students.Id
                             WHERE ClassNo = '" + ClassNo + "' AND SectionNo = '" + Section + "'";

            foreach (DataRow dr in dam.GetDataTable(query).Rows)
            {
                assignlist.Add(new Class
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    StudentId = Convert.ToString(dr["StudentID"]),
                    ClassNo = Convert.ToString(dr["ClassNo"]),
                    Section = Convert.ToString(dr["SectionNo"]),
                    Year = Convert.ToString(dr["Year"]),
                    Roll = Convert.ToInt32(dr["Roll"]),
                });
            }
            return assignlist;
        }

        public bool deleteAssign(string id)
        {
            string query = @"DELETE FROM Assign_Class WHERE Id = '" + id + "'";

            int i = dam.Execute(query);

            if (i >= 1)
                return true;
            else
                return false;
        }
    }

    /*public class ViewModelClass
    {
        public Class ClassModel { get; set; }
        public IEnumerable<Class> ClassModelList { get; set; }
    }*/
}