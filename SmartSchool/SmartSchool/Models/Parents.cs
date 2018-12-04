using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartSchool.Models
{
    public class Parents: IUser
    {
        DataManage dam = new DataManage();

        //[Required(ErrorMessage = "Id required")]
        [Display(Name = "Student Id")]
        public string Id { get; set; }

        //[Required(ErrorMessage = "Name required")]
        [Display(Name = "Your name")]
        public string Name { get; set; }

        [Display(Name = "Image")]
        public string ImagePath { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }

        //[Required(ErrorMessage = "Gender required")]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        //[Required(ErrorMessage = "Relationship required")]
        [Display(Name = "Relationship")]
        public string Relationship { get; set; }

        // For DropDownlist
        public List<SelectListItem> Relationships { get; set; }

        //[Required(ErrorMessage = "Occupation required")]
        [Display(Name = "Occupation")]
        public string Occupation { get; set; }

        //[Required(ErrorMessage = "Annualincome required")]
        [Display(Name = "Annual Income")]
        public string Annualincome { get; set; }

        //[Required(ErrorMessage = "Email required")]
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

        public Parents()
        {
            Relationships = new List<SelectListItem>()
            {
                new SelectListItem {
                    Text = "Father", Value = "Father"
                },
                new SelectListItem {
                    Text = "Mother", Value = "Mother"
                },
                new SelectListItem {
                    Text = "Other", Value = "Other"
                }
            };
        }

        public bool checkId()
        {
            string query = @"SELECT Id FROM Students WHERE Id = '" + Id + "'";
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
            string query = @"INSERT INTO Parents (StudentID) VALUES ('" + Id + "')";

            dam.Execute(query);
        }

        public List<Parents> viewParentsInfo()
        {
            List<Parents> infolist = new List<Parents>();
            string query = @"SELECT* FROM Parents";

            foreach (DataRow dr in dam.GetDataTable(query).Rows)
            {
                infolist.Add(new Parents
                {
                    Id = Convert.ToString(dr["StudentID"]),
                    Name = Convert.ToString(dr["Name"]),
                    Gender = Convert.ToString(dr["Gender"]),
                    Relationship = Convert.ToString(dr["Relationship"]),
                    Occupation = Convert.ToString(dr["Occupation"]),
                    Annualincome = Convert.ToString(dr["Annualincome"]),
                    Email = Convert.ToString(dr["Email"]),
                    Phone = Convert.ToString(dr["Phone"]),
                    Address = Convert.ToString(dr["Address"]),
                    ImagePath = Convert.ToString(dr["Image"])
                });
            }
            return infolist;
        }

        public void updateInfo(string id)
        {
            string query = @"UPDATE Parents SET Name = '" + Name + "', Gender = '" + Gender + "', Relationship = '" + Relationship + "', Occupation = '" + Occupation + "', Annualincome = '" + Annualincome + "', Email = '" + Email + "', Phone = '" + Phone + "', Address = '" + Address + "', Image = '" + ImagePath + "' WHERE StudentID = '" + id + "'";

            dam.Execute(query);
        }
    }
}