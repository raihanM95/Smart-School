using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartSchool.Models
{
    public class Teachers: IUser
    {
        DataManage dam = new DataManage();

        //[Required(ErrorMessage = "Please enter your id")]
        [Display(Name = "Teacher Id")]
        public string Id { get; set; }

        //[Required(ErrorMessage = "Name required")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Image")]
        public string ImagePath { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }

        //[Required(ErrorMessage = "Initial required")]
        [Display(Name = "Initial")]
        public string Initial { get; set; }

        // For DropDownlist
        public List<SelectListItem> Initials { get; set; }

        //[Required(ErrorMessage = "Designation required")]
        [Display(Name = "Designation")]
        public string Designation { get; set; }

        //[Required(ErrorMessage = "Gender required")]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        //[Required(ErrorMessage = "Email  required")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        //[Required(ErrorMessage = "Phone no required")]
        [Display(Name = "Phone")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        //[Required(ErrorMessage = "Address required")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        //[Required(ErrorMessage = "Please enter your password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public string User { get; set; }

        public int ClassTeacherTblID { get; set; }

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

        // For "viewClassTeachers" method values store and view in "ViewClassTeachers" html page
        public List<Teachers> Data { get; set; }

        public Teachers()
        {
            //For DropDownlist
            Initials = new List<SelectListItem>();
            string query = @"SELECT* FROM Teachers";
            foreach (DataRow dr in dam.GetDataTable(query).Rows)
            {
                Initials.Add(new SelectListItem
                {
                    Text = Convert.ToString(dr["Initial"]),
                    Value = Convert.ToString(dr["Initial"])
                });
            }

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

        public bool addTeachersID()
        {
            string query = @"INSERT INTO Teachers (Id) VALUES ('" + Id + "')";
            
            int i = dam.Execute(query);

            if (i >= 1)
                return true;
            else
                return false;
        }

        public List<Teachers> viewTeachers()
        {
            List<Teachers> teacherslist = new List<Teachers>();
            string query = @"SELECT* FROM Teachers";

            foreach (DataRow dr in dam.GetDataTable(query).Rows)
            {
                teacherslist.Add(new Teachers
                {
                    Id = Convert.ToString(dr["Id"]),
                    Name = Convert.ToString(dr["Name"]),
                    Initial = Convert.ToString(dr["Initial"]),
                    Designation = Convert.ToString(dr["Designation"])
                });
            }
            return teacherslist;
        }

        public List<Teachers> viewDetails()
        {
            List<Teachers> teacherlist = new List<Teachers>();
            string query = @"SELECT* FROM Teachers";

            foreach (DataRow dr in dam.GetDataTable(query).Rows)
            {
                teacherlist.Add(new Teachers
                {
                    Id = Convert.ToString(dr["Id"]),
                    Name = Convert.ToString(dr["Name"]),
                    Initial = Convert.ToString(dr["Initial"]),
                    Designation = Convert.ToString(dr["Designation"]),
                    Gender = Convert.ToString(dr["Gender"]),
                    Email = Convert.ToString(dr["Email"]),
                    Phone = Convert.ToString(dr["Phone"]),
                    Address = Convert.ToString(dr["Address"]),
                    ImagePath = Convert.ToString(dr["Image"])
                });
            }
            return teacherlist;
        }

        public void updateTeacherInfo()
        {
            string query = @"UPDATE Teachers SET Initial = '" + Initial + "', Designation = '" + Designation + "' WHERE Id = '" + Id + "'";

            dam.Execute(query);
        }

        public bool deleteTeacher(string id)
        {
            string query = @"DELETE FROM Teachers WHERE Id = '" + id + "'";

            int i = dam.Execute(query);

            if (i >= 1)
                return true;
            else
                return false;
        }

        public bool addClassTeachers()
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

            string query3 = @"SELECT* FROM Teachers WHERE Initial = '" + Initial + "'";

            foreach (DataRow dr in dam.GetDataTable(query3).Rows)
            {
                Id = Convert.ToString(dr["Id"]);
            }

            string query4 = @"INSERT INTO ClassTeacher (ClassID, SectionID, TeacherID) VALUES (" + ClassID + ", " + SectionID + ", '" + Id + "')";

            int i = dam.Execute(query4);

            if (i >= 1)
                return true;
            else
                return false;
        }

        public List<Teachers> viewClassTeachers()
        {
            List<Teachers> classteacherlist = new List<Teachers>();

            string query = @"SELECT ClassTeacher.Id, Teachers.Initial, Class.ClassNo, Section.SectionNo
                             FROM ((ClassTeacher
                             INNER JOIN Class ON ClassTeacher.ClassID = Class.Id)
                             INNER JOIN Section ON ClassTeacher.SectionID = Section.Id)
                             INNER JOIN Teachers ON ClassTeacher.TeacherID = Teachers.Id
                             WHERE ClassNo = '" + ClassNo + "'";

            foreach (DataRow dr in dam.GetDataTable(query).Rows)
            {
                classteacherlist.Add(new Teachers
                {
                    ClassTeacherTblID = Convert.ToInt32(dr["Id"]),
                    Section = Convert.ToString(dr["SectionNo"]),
                    Initial = Convert.ToString(dr["Initial"]),
                });
            }
            return classteacherlist;
        }

        public bool deleteClassTeacher(string id)
        {
            string query = @"DELETE FROM ClassTeacher WHERE Id = '" + id + "'";

            int i = dam.Execute(query);

            if (i >= 1)
                return true;
            else
                return false;
        }

        public bool checkId()
        {
            string query = @"SELECT Id FROM Teachers WHERE Id = '" + Id + "'";
            dam.GetDataTable(query);
            if (dam.count != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Registration()
        {
            string query = @"UPDATE Teachers SET Initial = '" + Initial + "' WHERE Id = '" + Id + "'";

            dam.Execute(query);
        }

        public void updateInfo(string id)
        {
            string query = @"UPDATE Teachers SET Name = '" + Name + "', Initial = '" + Initial + "', Designation = '" + Designation + "', Gender = '" + Gender + "', Email = '" + Email + "', Phone = '" + Phone + "', Address = '" + Address + "', Image = '" + ImagePath + "' WHERE Id = '" + id + "'";

            dam.Execute(query);
        }

        public List<Teachers> teachersList()
        {
            List<Teachers> teacherlist = new List<Teachers>();
            string query = @"SELECT* FROM Teachers";

            foreach (DataRow dr in dam.GetDataTable(query).Rows)
            {
                teacherlist.Add(new Teachers
                {
                    Name = Convert.ToString(dr["Name"]),
                    Designation = Convert.ToString(dr["Designation"]),
                    Email = Convert.ToString(dr["Email"]),
                    Phone = Convert.ToString(dr["Phone"]),
                    ImagePath = Convert.ToString(dr["Image"])
                });
            }
            return teacherlist;
        }
    }
}