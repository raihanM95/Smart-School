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

        //[Required(ErrorMessage = "Name required")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Image")]
        public string ImagePath { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }

        //[Required(ErrorMessage = "DOB required")]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        //[Required(ErrorMessage = "Gender required")]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

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

        // For SearchBox
        public string Search { get; set; }

        // For "viewStudents" method values store and view in "ViewStudents" html page
        public List<Students> Data { get; set; }

        public Students()
        {
            //For DropDownlist
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
        }

        public bool addStudents()
        {
            string query = @"INSERT INTO Students (Id, Name, DateOfBirth, Gender, Email, Phone, Address, Image) VALUES ('" + Id + "', '" + Name + "', '" + DateOfBirth + "', '" + Gender + "', '" + Email + "', '" + Phone + "', '" + Address + "', '" + ImagePath + "')";

            int i = dam.Execute(query);

            if (i >= 1)
                return true;
            else
                return false;
        }

        public List<Students> viewStudents()
        {
            List<Students> studentslist = new List<Students>();
            string query = @"SELECT* FROM Students WHERE Id LIKE '%" + Search + "%' OR Name LIKE '%" + Search + "%' OR Address LIKE '%" + Search + "%'";

            foreach (DataRow dr in dam.GetDataTable(query).Rows)
            {
                studentslist.Add(new Students
                {
                    Id = Convert.ToString(dr["Id"]),
                    Name = Convert.ToString(dr["Name"]),
                    DateOfBirth = Convert.ToDateTime(dr["DateOfBirth"]),
                    Gender = Convert.ToString(dr["Gender"]),
                    Email = Convert.ToString(dr["Email"]),
                    Phone = Convert.ToString(dr["Phone"]),
                    Address = Convert.ToString(dr["Address"]),
                    ImagePath = Convert.ToString(dr["Image"])
                });
            }
            return studentslist;
        }

        public void updateStudentInfo()
        {
            string query = @"UPDATE Students SET Name = '" + Name + "', DateOfBirth = '" + DateOfBirth + "', Gender = '" + Gender + "', Email = '" + Email + "', Phone = '" + Phone + "', Address = '" + Address + "' WHERE Id = '" + Id + "'";

            dam.Execute(query);
        }

        public void updateInfo(string id)
        {
            string query = @"UPDATE Students SET Name = '" + Name + "', Email = '" + Email + "', Phone = '" + Phone + "', Address = '" + Address + "', Image = '" + ImagePath + "' WHERE Id = '" + id + "'";

            dam.Execute(query);
        }
    }
}