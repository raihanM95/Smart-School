using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Models
{
    public interface IUser
    {
        // Property
        string Id { get; set; }
        string Name { get; set; }
        string Gender { get; set; }
        string Email { get; set; }
        string Phone { get; set; }
        string Address { get; set; }
        string Password { get; set; }

        // Methods
        /*void Registration();
        string Login();
        string forgetPassword();*/
    }
}
