using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartSchool.Models
{
    public class Parents: IUser
    {
        //[Required(ErrorMessage = "Id required")]
        [Display(Name = "Student Id")]
        public string Id { get; set; }

        //[Required(ErrorMessage = "Name required")]
        [Display(Name = "Your name")]
        public string Name { get; set; }

        //[Required(ErrorMessage = "Gender required")]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        //[Required(ErrorMessage = "Email required")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        //[Required(ErrorMessage = "Phone no required")]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        //[Required(ErrorMessage = "Address required")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        //[Required(ErrorMessage = "Please enter your username")]
        public string UserName { get; set; }

        //[Required(ErrorMessage = "Please enter your password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}