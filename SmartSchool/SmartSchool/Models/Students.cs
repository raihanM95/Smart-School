using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartSchool.Models
{
    public class Students: IUser
    {
        DataManage dam = new DataManage();

        //[Required(ErrorMessage = "Please enter your id")]
        [Display(Name = "Student Id")]
        public string Id { get; set; }

        // For DropDownlist
        public List<SelectListItem> Ids { get; set; }

        public int AssignTableId { get; set; }

        //[Required(ErrorMessage = "Name required")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        //[Required(ErrorMessage = "DOB required")]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        //[Required(ErrorMessage = "Gender required")]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        //[Required(ErrorMessage = "Class required")]
        [Display(Name = "Class")]
        public int Class { get; set; }

        // For DropDownlist
        public List<SelectListItem> Classes { get; set; }

        //[Required(ErrorMessage = "Section required")]
        [Display(Name = "Section")]
        public string Section { get; set; }

        // For DropDownlist
        public List<SelectListItem> Sections { get; set; }

        //[Required(ErrorMessage = "Year required")]
        [Display(Name = "Year")]
        public string Year { get; set; }

        // For DropDownlist
        public List<SelectListItem> Years { get; set; }

        //[Required(ErrorMessage = "Roll required")]
        [Display(Name = "Roll")]
        public int Roll { get; set; }

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

        public List<Students> viewStudents()
        {
            List<Students> studentslist = new List<Students>();
            string query = @"SELECT* FROM Students";

            foreach (DataRow dr in dam.GetDataTable(query).Rows)
            {
                studentslist.Add(new Students
                {
                    Id = Convert.ToString(dr["Id"]),
                    Name = Convert.ToString(dr["Name"]),
                    //DateOfBirth = Convert.ToDateTime(dr["DateOfBirth"]),
                    Gender = Convert.ToString(dr["Gender"]),
                    Email = Convert.ToString(dr["Email"]),
                    Phone = Convert.ToString(dr["Phone"]),
                    Address = Convert.ToString(dr["Address"]),
                });
            }
            return studentslist;
        }

        public Students()
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

            Ids = new List<SelectListItem>();
            string query = @"SELECT* FROM Students";
            foreach (DataRow dr in dam.GetDataTable(query).Rows)
            {
                Ids.Add(new SelectListItem
                {
                    Text = Convert.ToString(dr["Id"]),
                    Value = Convert.ToString(dr["Id"])
                });
            }
        }

        public bool assignIntoClass()
        {
            int cls = Convert.ToInt32(Class);

            string query = @"INSERT INTO Assign_Class (Class, Section, Year, Student_Id, Roll) VALUES (" + cls + ", '" + Section + "', '" + Year + "', '" + Id + "', " + Roll + ")";

            int i = dam.Execute(query);

            if (i >= 1)
                return true;
            else
                return false;
        }

        public List<Students> viewAssign()
        {
            List<Students> assignlist = new List<Students>();
            string query = @"SELECT* FROM Assign_Class";

            foreach (DataRow dr in dam.GetDataTable(query).Rows)
            {
                assignlist.Add(new Students
                {
                    AssignTableId = Convert.ToInt32(dr["Id"]),
                    Id = Convert.ToString(dr["Student_Id"]),
                    Class = Convert.ToInt32(dr["Class"]),
                    Section = Convert.ToString(dr["Section"]),
                    Year = Convert.ToString(dr["Year"]),
                    Roll = Convert.ToInt32(dr["Roll"]),
                });
            }
            return assignlist;
        }

        public void updateClass()
        {
            string query = @"UPDATE Assign_Class SET Class = " + Class + ", Section = '" + Section + "', Year = '" + Year + "', Student_Id = '" + Id + "', Roll = " + Roll + " WHERE Id = " + AssignTableId + "";

            dam.Execute(query);
        }
    }
}