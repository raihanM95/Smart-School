using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SmartSchool.Models;

namespace SmartSchool.Controllers
{
    public class StudentsController : Controller
    {
        // GET: Students/AddStudents // by admin
        [HttpGet]
        public ActionResult AddStudents()
        {
            if (Request.Cookies.Get("admin") != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("ERP", "ERP");
            }
        }

        // POST: Students/AddStudents
        [HttpPost]
        public ActionResult AddStudents(Students student)
        {
            string fileName = Path.GetFileNameWithoutExtension(student.ImageFile.FileName);
            student.ImagePath = student.Id + fileName + System.IO.Path.GetExtension(student.ImageFile.FileName);
            fileName = "~/StudentsImage/" + student.Id + fileName + System.IO.Path.GetExtension(student.ImageFile.FileName);
            student.ImageFile.SaveAs(Server.MapPath(fileName));
            try
            {
                if (ModelState.IsValid)
                {
                    
                    if (student.addStudents())
                    {
                        ViewBag.Message = "Student info recorded successfully";
                        ModelState.Clear();
                    }
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Students/ViewStudents // by admin
        [HttpGet]
        public ActionResult ViewStudents()
        {
            if (Request.Cookies.Get("admin") != null)
            {
                Students students = new Students();
                students.Data = students.viewStudents();
                return View(students);
            }
            else
            {
                return RedirectToAction("ERP", "ERP");
            }
        }

        // POST: Students/ViewStudents
        [HttpPost]
        public ActionResult ViewStudents(Students students)
        {
            students.Data = students.viewStudents();
            return View(students);
        }

        // GET: Students/UpdateStudentInfo // by admin
        [HttpGet]
        public ActionResult UpdateStudentInfo(string id)
        {
            if (Request.Cookies.Get("admin") != null)
            {
                Students student = new Students();
                return View(student.viewStudents().Find(smodel => smodel.Id == id));
            }
            else
            {
                return RedirectToAction("ERP", "ERP");
            }
        }

        // POST: Students/UpdateStudentInfo
        [HttpPost]
        public ActionResult UpdateStudentInfo(string id, Students student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    student.updateStudentInfo();
                    return RedirectToAction("ViewStudents");
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Students/Register // by student
        [HttpGet]
        public ActionResult Register()
        {
            if (Request.Cookies.Get("student") != null)
            {
                return RedirectToAction("Index", "Students");
            }
            else if (Request.Cookies.Get("admin") != null)
            {
                return RedirectToAction("Home", "ERP");
            }
            else if (Request.Cookies.Get("teacher") != null)
            {
                return RedirectToAction("Index", "Teachers");
            }
            else if (Request.Cookies.Get("parents") != null)
            {
                return RedirectToAction("Index", "Parents");
            }
            else
            {
                return View();
            }
        }

        // POST: Students/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Exclude = "IsEmailVerified,ActivationCode")] StudentLogin studentLogin)
        {
            bool Status = false;
            string message = "";

            if (ModelState.IsValid)
            {
                // Email is already Exist
                var isExist = IsEmailExist(studentLogin.Email);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already exist");
                    return View(studentLogin);
                }

                // Generate Activation Code
                studentLogin.ActivationCode = Guid.NewGuid();

                // Password Hashing
                studentLogin.Password = Crypto.Hash(studentLogin.Password);
                studentLogin.ConfirmPassword = Crypto.Hash(studentLogin.ConfirmPassword);

                studentLogin.IsEmailVerified = false;

                // Save to Database
                using (StudentRegEntities studentReg = new StudentRegEntities())
                {
                    studentReg.StudentLogins.Add(studentLogin);
                    studentReg.SaveChanges();

                    // Send Email to User
                    SendVerificationLinkEmail(studentLogin.Email, studentLogin.ActivationCode.ToString());
                    message = "Registration successfully done. Account activation link " +
                        " has been sent to your email id:" + studentLogin.Email;
                    Status = true;
                }
            }
            else
            {
                message = "Invalid Request";
            }

            ViewBag.Message = message;
            ViewBag.Status = Status;
            return View();
        }

        // GET: Students/VerifyAccount // by student
        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            using (StudentRegEntities studentReg = new StudentRegEntities())
            {
                studentReg.Configuration.ValidateOnSaveEnabled = false; // This line I have added here to avoid 
                                                                        // Confirm password does not match issue on save changes
                var v = studentReg.StudentLogins.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    v.IsEmailVerified = true;
                    studentReg.SaveChanges();
                    Status = true;
                }
                else
                {
                    ViewBag.Message = "Invalid Request";
                }
            }
            ViewBag.Status = Status;
            return View();
        }

        [NonAction]
        public bool IsEmailExist(string email)
        {
            using (StudentRegEntities studentReg = new StudentRegEntities())
            {
                var v = studentReg.StudentLogins.Where(a => a.Email == email).FirstOrDefault();
                return v != null;
            }
        }

        [NonAction]
        public void SendVerificationLinkEmail(string email, string activationCode, string emailFor = "VerifyAccount")
        {
            try
            {
                var verifyUrl = "/Students/" + emailFor + "/" + activationCode;
                var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

                var fromEmail = new MailAddress("sm.school@gmail.com", "SmartSchool");
                var toEmail = new MailAddress(email);
                var fromEmailPassword = "******"; // Replace with actual password

                string subject = "";
                string body = "";
                if (emailFor == "VerifyAccount")
                {
                    subject = "Your account is successfully created!";
                    body = "<br/><br/>Your Smart School account is" +
                        " successfully created. Please click on the below link to verify your account" +
                        " <br/><br/><a href='" + link + "'>" + link + "</a> ";
                }
                else if (emailFor == "ResetPassword")
                {
                    subject = "Reset Password";
                    body = "Hi!<br/><br/>We got request for reset your account password. Please click on the below link to reset your password" +
                        "<br/><br/><a href=" + link + ">Reset Password link</a>";
                }

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
                };

                using (var message = new MailMessage(fromEmail, toEmail)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                    smtp.Send(message);
            }
            catch
            {

            }
        }

        // GET: Students/Login // by student
        [HttpGet]
        public ActionResult Login()
        {
            if (Request.Cookies.Get("student") != null)
            {
                return RedirectToAction("Index", "Students");
            }
            else if (Request.Cookies.Get("admin") != null)
            {
                return RedirectToAction("Home", "ERP");
            }
            else if (Request.Cookies.Get("teacher") != null)
            {
                return RedirectToAction("Index", "Teachers");
            }
            else if (Request.Cookies.Get("parents") != null)
            {
                return RedirectToAction("Index", "Parents");
            }
            else
            {
                return View();
            }
        }

        // POST: Students/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Students student, string ReturnUrl = "")
        {
            string message = "";
            using (StudentRegEntities studentReg = new StudentRegEntities())
            {
                var v = studentReg.StudentLogins.Where(a => a.Id == student.Id).FirstOrDefault();
                if (v != null)
                {
                    if (!v.IsEmailVerified)
                    {
                        ViewBag.ErrorMessage = "Please verify your email first";
                        return View();
                    }
                    if (string.Compare(Crypto.Hash(student.Password), v.Password) == 0)
                    {
                        int timeout = student.RememberMe ? 1440 : 720; // 1440 min = 1 day && 720 min= 12 hour
                        //var ticket = new FormsAuthenticationTicket(student.Id, student.RememberMe, timeout);
                        //string encrypted = FormsAuthentication.Encrypt(ticket);
                        //HttpCookie cookie = new HttpCookie("student", encrypted);
                        HttpCookie cookie = new HttpCookie("student", student.Id);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        Response.Cookies.Add(cookie);
                        ViewBag.User = Request.Cookies.Get("student").Value;
                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Students");
                        }
                    }
                    else
                    {
                        message = "Invalid Id or password";
                    }
                }
                else
                {
                    message = "Invalid credential provided";
                }
            }
            ViewBag.ErrorMessage = message;
            return View();
        }

        // POST: Students/_LogoutPartial
        //[Authorize]
        //[HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Response.Cookies["student"].Expires = DateTime.Now.AddDays(-1);
            return RedirectToAction("Index", "Home");
        }

        // GET: Students/ForgotPassword // by student
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            if (Request.Cookies.Get("student") != null)
            {
                return RedirectToAction("Index", "Students");
            }
            else if (Request.Cookies.Get("admin") != null)
            {
                return RedirectToAction("Home", "ERP");
            }
            else if (Request.Cookies.Get("teacher") != null)
            {
                return RedirectToAction("Index", "Teachers");
            }
            else if (Request.Cookies.Get("parents") != null)
            {
                return RedirectToAction("Index", "Parents");
            }
            else
            {
                return View();
            }
        }

        // POST: Students/ForgotPassword
        [HttpPost]
        public ActionResult ForgotPassword(string Email)
        {
            bool status = false;

            using (StudentRegEntities studentReg = new StudentRegEntities())
            {
                var account = studentReg.StudentLogins.Where(a => a.Email == Email).FirstOrDefault();
                if (account != null)
                {
                    //Send email for reset password
                    string resetCode = Guid.NewGuid().ToString();
                    SendVerificationLinkEmail(account.Email, resetCode, "ResetPassword");
                    account.ResetPasswordCode = resetCode;

                    //Avoid confirm password not match issue, as we had added a confirm password property
                    studentReg.Configuration.ValidateOnSaveEnabled = false;
                    studentReg.SaveChanges();
                    ViewBag.Message = "Reset password link has been sent to your email id";
                    ModelState.Clear();
                }
                else
                {
                    ViewBag.ErrorMessage = "Sorry! account not found";
                }
            }
            return View();
        }

        // GET: Students/ResetPassword // by student
        public ActionResult ResetPassword(string id)
        {
            //Verify the reset password link
            //Find account associated with this link
            //redirect to reset password page
            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }

            using (StudentRegEntities studentReg = new StudentRegEntities())
            {
                var user = studentReg.StudentLogins.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
                if (user != null)
                {
                    ResetPassword reset = new ResetPassword();
                    reset.ResetCode = id;
                    return View(reset);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        // POST: Students/ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPassword reset)
        {
            if (ModelState.IsValid)
            {
                using (StudentRegEntities studentReg = new StudentRegEntities())
                {
                    var user = studentReg.StudentLogins.Where(a => a.ResetPasswordCode == reset.ResetCode).FirstOrDefault();
                    if (user != null)
                    {
                        user.Password = Crypto.Hash(reset.NewPassword);
                        user.ResetPasswordCode = "";
                        studentReg.Configuration.ValidateOnSaveEnabled = false;
                        studentReg.SaveChanges();
                        ViewBag.message = "New password updated successfully";
                    }
                }
                ModelState.Clear();
            }
            else
            {
                ViewBag.Errormessage = "Something invalid!";
            }
            return View(reset);
        }

        //[Authorize]
        // GET: Students/Index // by student
        public ActionResult Index()
        {
            if (Request.Cookies.Get("student") != null)
            {
                Students student = new Students();
                return View(student.viewStudents().Find(smodel => smodel.Id == Request.Cookies.Get("student").Value));
            }
            else
            {
                return RedirectToAction("Login", "Students");
            }
        }

        // GET: Students/UpdateInfo // by student
        public ActionResult UpdateInfo()
        {
            if (Request.Cookies.Get("student") != null)
            {
                Students student = new Students();
                return View(student.viewStudents().Find(smodel => smodel.Id == Request.Cookies.Get("student").Value));
            }
            else
            {
                return RedirectToAction("Login", "Students");
            }
        }

        // POST: Students/UpdateInfo
        [HttpPost]
        public ActionResult UpdateInfo(Students student)
        {
            string id = Request.Cookies.Get("student").Value;
            string fileName = Path.GetFileNameWithoutExtension(student.ImageFile.FileName);
            student.ImagePath = id + fileName + System.IO.Path.GetExtension(student.ImageFile.FileName);
            fileName = "~/StudentsImage/" + id + fileName + System.IO.Path.GetExtension(student.ImageFile.FileName);
            student.ImageFile.SaveAs(Server.MapPath(fileName));
            try
            {
                if (ModelState.IsValid)
                {
                    student.updateInfo(id);
                    return RedirectToAction("Index");
                }
                return View();
            }
            catch
            {
                return View();
            }
        }
    }
}