using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartSchool.Models
{
    public class Admin
    {
        DataManage dam = new DataManage();

        [Required(ErrorMessage = "Please enter your username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Login()
        {
            string Message = "";
            string query = "SELECT* FROM AdminLogin WHERE UserName = '" + UserName + "' AND Password = '" + Password + "'";
            dam.GetDataTable(query);
            if (dam.count != 0)
            {
                Message = "1";
                UserName = UserName;
            }
            else
            {
                Message = "Invalid username or password!";
            }

            return Message;
        }
    }
}