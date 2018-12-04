using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartSchool.Models
{
    [MetadataType(typeof(ParentsLoginMetadata))]
    public partial class ParentsLogin
    {
        public string ConfirmPassword { get; set; }
    }

    public class ParentsLoginMetadata
    {
        //[Required(ErrorMessage = "Please enter your student id")]
        [Display(Name = "Student Id")]
        public string Id { get; set; }

        //[Required(ErrorMessage = "Email  required")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        //[Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Minimum 6 characters required")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Do not match the password")]
        public string ConfirmPassword { get; set; }
    }
}