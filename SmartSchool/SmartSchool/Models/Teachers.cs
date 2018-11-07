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

        public int AssignTableId { get; set; }

        //[Required(ErrorMessage = "Name required")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        //[Required(ErrorMessage = "Initial required")]
        [Display(Name = "Initial")]
        public string Initial { get; set; }

        // For DropDownlist
        public List<SelectListItem> Initials { get; set; }

        //[Required(ErrorMessage = "Designation required")]
        [Display(Name = "Designation")]
        public string Designation { get; set; }

        //[Required(ErrorMessage = "Subject required")]
        [Display(Name = "Subject")]
        public string Subject { get; set; }

        // For DropDownlist
        public List<SelectListItem> Subjects { get; set; }

        //[Required(ErrorMessage = "Class required")]
        [Display(Name = "Class")]
        public int Class { get; set; }

        // For DropDownlist
        public List<SelectListItem> Classes { get; set; }

        //[Required(ErrorMessage = "Class required")]
        [Display(Name = "For class teacher")]
        public int ClassTeacher { get; set; }

        //[Required(ErrorMessage = "Section required")]
        [Display(Name = "Section")]
        public string Section { get; set; }

        // For DropDownlist
        public List<SelectListItem> Sections { get; set; }

        //[Required(ErrorMessage = "Section required")]
        [Display(Name = "For class teacher")]
        public string SectionTeacher { get; set; }

        //[Required(ErrorMessage = "Gender required")]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        //[Required(ErrorMessage = "Email  required")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        //[Required(ErrorMessage = "Phone no required")]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        //[Required(ErrorMessage = "Address required")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        //[Required(ErrorMessage = "Please enter your password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

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
                    Phone = Convert.ToString(dr["Phone"])
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

        public Teachers()
        {
            //All list for DropDownlist
            Classes = new List<SelectListItem>()
            {
                new SelectListItem {
                    Text = "6", Value = "6"
                },
                new SelectListItem {
                    Text = "7", Value = "7"
                },
                new SelectListItem {
                    Text = "8", Value = "8"
                },
                new SelectListItem {
                    Text = "9", Value = "9"
                },
                new SelectListItem {
                    Text = "10", Value = "10"
                }
            };

            Sections = new List<SelectListItem>()
            {
                new SelectListItem {
                    Text = "A", Value = "A"
                },
                new SelectListItem {
                    Text = "B", Value = "B"
                },
                new SelectListItem {
                    Text = "C", Value = "C"
                }
            };

            Subjects = new List<SelectListItem>()
            {
                new SelectListItem {
                    Text = "Bangla", Value = "Bangla"
                },
                new SelectListItem {
                    Text = "English", Value = "English"
                },
                new SelectListItem {
                    Text = "Math", Value = "Math"
                },
                new SelectListItem {
                    Text = "ICT", Value = "ICT"
                }
            };

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
        }

        public bool assignTeacher()
        {
            int cls = Convert.ToInt32(Class);
            int tcls = Convert.ToInt32(ClassTeacher);

            string query = @"INSERT INTO Assign_Teacher (Class, Section, Subject, Initial, ClassT_Class, ClassT_Section) VALUES (" + cls + ", '" + Section + "', '" + Subject + "', '" + Initial + "', " + tcls + ", '" + SectionTeacher + "')";

            int i = dam.Execute(query);

            if (i >= 1)
                return true;
            else
                return false;
        }

        public List<Teachers> viewAssign(){
            List<Teachers> assignlist = new List<Teachers>();
            string query = @"SELECT* FROM Assign_Teacher";

            foreach (DataRow dr in dam.GetDataTable(query).Rows)
            {
                assignlist.Add(new Teachers
                {
                    AssignTableId = Convert.ToInt32(dr["Id"]),
                    Initial = Convert.ToString(dr["Initial"]),
                    Subject = Convert.ToString(dr["Subject"]),
                    Class = Convert.ToInt32(dr["Class"]),
                    Section = Convert.ToString(dr["Section"]),
                    ClassTeacher = Convert.ToInt32(dr["ClassT_Class"]),
                    SectionTeacher = Convert.ToString(dr["ClassT_Section"])
                });
            }
            return assignlist;
        }

        public void updateAssign()
        {
            string query = @"UPDATE Assign_Teacher SET Class = " + Class + ", Section = '" + Section + "', Subject = '" + Subject + "', Initial = '" + Initial + "', ClassT_Class = " + ClassTeacher + ", ClassT_Section = '" + SectionTeacher + "' WHERE Id = " + AssignTableId + "";

            dam.Execute(query);
        }
    }
}