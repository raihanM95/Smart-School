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
    public class TeachersController : Controller
    {
        DataManage dam = new DataManage();

        // GET: Teachers/AddTeachersID // by admin
        [HttpGet]
        public ActionResult AddTeachersID()
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

        // POST: Teachers/AddTeachersID
        [HttpPost]
        public ActionResult AddTeachersID(Teachers teacher)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (teacher.addTeachersID())
                    {
                        ViewBag.Message = "Teacher id record successfully";
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

        // Teachers/ViewTeachers // by admin
        public ActionResult ViewTeachers()
        {
            if (Request.Cookies.Get("admin") != null)
            {
                Teachers teachers = new Teachers();
                return View(teachers.viewTeachers());
            }
            else
            {
                return RedirectToAction("ERP", "ERP");
            }
        }

        // Teachers/ViewDetails // by admin
        public ActionResult ViewDetails(string id)
        {
            if(Request.Cookies.Get("admin") != null)
            {
                Teachers teacher = new Teachers();
                return View(teacher.viewDetails().Find(smodel => smodel.Id == id));
            }
            else
            {
                return RedirectToAction("ERP", "ERP");
            }
        }

        // GET: Teachers/UpdateTeacherInfo // by admin
        [HttpGet]
        public ActionResult UpdateTeacherInfo(string id)
        {
            if (Request.Cookies.Get("admin") != null)
            {
                Teachers teacher = new Teachers();
                return View(teacher.viewTeachers().Find(smodel => smodel.Id == id));
            }
            else
            {
                return RedirectToAction("ERP", "ERP");
            }
        }

        // POST: Teachers/UpdateTeacherInfo
        [HttpPost]
        public ActionResult UpdateTeacherInfo(string id, Teachers teacher)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    teacher.updateTeacherInfo();
                    return RedirectToAction("ViewTeachers");
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // Teachers/ViewTeachers // by admin
        public ActionResult DeleteTeacher(string id)
        {
            try
            {
                Teachers teacher = new Teachers();
                if (teacher.deleteTeacher(id))
                {
                    ViewBag.AlertMsg = "Delete Successfully";
                }
                return RedirectToAction("ViewTeachers");
            }
            catch
            {
                return RedirectToAction("ViewTeachers"); ;
            }
        }

        // GET: Teachers/AddClassTeachers // by admin
        [HttpGet]
        public ActionResult AddClassTeachers()
        {
            if (Request.Cookies.Get("admin") != null)
            {
                Teachers teacher = new Teachers();
                return View(teacher);
            }
            else
            {
                return RedirectToAction("ERP", "ERP");
            }
        }

        // POST: Teachers/AddClassTeachers
        [HttpPost]
        public ActionResult AddClassTeachers(Teachers teacher)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    teacher.viewClassWiseSection();
                    if (teacher.addClassTeachers())
                    {
                        ViewBag.Message = "Add successfully";
                        ModelState.Clear();
                    }
                }
                return View(teacher);
            }
            catch
            {
                return View(teacher);
            }
        }

        // GET: Teachers/ViewClassTeachers // by admin
        [HttpGet]
        public ActionResult ViewClassTeachers()
        {
            if (Request.Cookies.Get("admin") != null)
            {
                Teachers teacher = new Teachers();
                teacher.Data = teacher.viewClassTeachers();
                return View(teacher);
            }
            else
            {
                return RedirectToAction("ERP", "ERP");
            }
        }

        // POST: Teachers/ViewClassTeachers
        [HttpPost]
        public ActionResult ViewClassTeachers(Teachers teacher)
        {
            teacher.Data = teacher.viewClassTeachers();
            return View(teacher);
        }

        // Teachers/ViewClassTeachers // by admin
        public ActionResult DeleteClassTeacher(string id)
        {
            try
            {
                Teachers teacher = new Teachers();
                if (teacher.deleteClassTeacher(id))
                {
                    ViewBag.AlertMsg = "Delete Successfully";
                }
                return RedirectToAction("ViewClassTeachers");
            }
            catch
            {
                return RedirectToAction("ViewClassTeachers"); ;
            }
        }

        // GET: Teachers/Register // by teacher
        [HttpGet]
        public ActionResult Register()
        {
            if (Request.Cookies.Get("teacher") != null)
            {
                return RedirectToAction("Index", "Teachers");
            }
            else if (Request.Cookies.Get("admin") != null)
            {
                return RedirectToAction("Home", "ERP");
            }
            else if (Request.Cookies.Get("parents") != null)
            {
                return RedirectToAction("Index", "Parents");
            }
            else if (Request.Cookies.Get("student") != null)
            {
                return RedirectToAction("Index", "Students");
            }
            else
            {
                return View();
            }
        }

        // POST: Teachers/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Exclude = "IsEmailVerified,ActivationCode")] TeacherLogin teacherLogin)
        {
            bool Status = false;
            string message = "";
            
            if (ModelState.IsValid)
            {
                // Email is already Exist
                var isExist = IsEmailExist(teacherLogin.Email);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already exist");
                    return View(teacherLogin);
                }

                // Generate Activation Code
                teacherLogin.ActivationCode = Guid.NewGuid();

                // Password Hashing
                teacherLogin.Password = Crypto.Hash(teacherLogin.Password);
                teacherLogin.ConfirmPassword = Crypto.Hash(teacherLogin.ConfirmPassword);
                
                teacherLogin.IsEmailVerified = false;

                // Save to Database
                using (TeacherRegEntities teacherReg = new TeacherRegEntities())
                {
                    Teachers teacher = new Teachers();
                    teacher.Id = teacherLogin.Id;
                    if (teacher.checkId())
                    {
                        teacher.Initial = teacherLogin.Initial;
                        teacher.Registration();
                        teacherReg.TeacherLogins.Add(teacherLogin);
                        teacherReg.SaveChanges();

                        // Send Email to User
                        SendVerificationLinkEmail(teacherLogin.Email, teacherLogin.ActivationCode.ToString());
                        message = "Registration successfully done. Account activation link " +
                            " has been sent to your email id:" + teacherLogin.Email;
                        Status = true;
                    }
                    else
                    {
                        message = "Invalid id";
                    }
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

        // GET: Teachers/VerifyAccount // by teacher
        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            using (TeacherRegEntities teacherReg = new TeacherRegEntities())
            {
                teacherReg.Configuration.ValidateOnSaveEnabled = false; // This line I have added here to avoid 
                                                                // Confirm password does not match issue on save changes
                var v = teacherReg.TeacherLogins.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    v.IsEmailVerified = true;
                    teacherReg.SaveChanges();
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
            using (TeacherRegEntities teacherReg = new TeacherRegEntities())
            {
                var v = teacherReg.TeacherLogins.Where(a => a.Email == email).FirstOrDefault();
                return v != null;
            }
        }

        [NonAction]
        public void SendVerificationLinkEmail(string email, string activationCode, string emailFor = "VerifyAccount")
        {
            try
            {
                var verifyUrl = "/Teachers/" + emailFor + "/" + activationCode;
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

        // GET: Teachers/Login // by teacher
        [HttpGet]
        public ActionResult Login()
        {
            if(Request.Cookies.Get("teacher") != null)
            {
                return RedirectToAction("Index", "Teachers");
            }
            else if (Request.Cookies.Get("admin") != null)
            {
                return RedirectToAction("Home", "ERP");
            }
            else if (Request.Cookies.Get("parents") != null)
            {
                return RedirectToAction("Index", "Parents");
            }
            else if (Request.Cookies.Get("student") != null)
            {
                return RedirectToAction("Index", "Students");
            }
            else
            {
                return View();
            }
        }

        // POST: Teachers/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Teachers teacher, string ReturnUrl = "")
        {
            string message = "";
            using (TeacherRegEntities teacherReg = new TeacherRegEntities())
            {
                var v = teacherReg.TeacherLogins.Where(a => a.Id == teacher.Id).FirstOrDefault();
                if (v != null)
                {
                    if (!v.IsEmailVerified)
                    {
                        ViewBag.ErrorMessage = "Please verify your email first";
                        return View();
                    }
                    if (string.Compare(Crypto.Hash(teacher.Password), v.Password) == 0)
                    {
                        int timeout = teacher.RememberMe ? 1440 : 720; // 1440 min = 1 day && 720 min= 12 hour
                        //var ticket = new FormsAuthenticationTicket(teacher.Id, teacher.RememberMe, timeout);
                        //string encrypted = FormsAuthentication.Encrypt(ticket);
                        //HttpCookie cookie = new HttpCookie("teacher", encrypted);
                        HttpCookie cookie = new HttpCookie("teacher", teacher.Id);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        Response.Cookies.Add(cookie);
                        teacher.User = Request.Cookies.Get("teacher").Value;
                        
                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Teachers");
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

        // POST: Teachers/_LogoutPartial
        //[Authorize]
        //[HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Response.Cookies["teacher"].Expires = DateTime.Now.AddDays(-1);
            return RedirectToAction("ERP", "ERP");
        }

        // GET: Teachers/ForgotPassword // by teacher
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            if (Request.Cookies.Get("teacher") != null)
            {
                return RedirectToAction("Index", "Teachers");
            }
            else if (Request.Cookies.Get("admin") != null)
            {
                return RedirectToAction("Home", "ERP");
            }
            else if (Request.Cookies.Get("parents") != null)
            {
                return RedirectToAction("Index", "Parents");
            }
            else if (Request.Cookies.Get("student") != null)
            {
                return RedirectToAction("Index", "Students");
            }
            else
            {
                return View();
            }
        }

        // POST: Teachers/ForgotPassword
        [HttpPost]
        public ActionResult ForgotPassword(string Email)
        {
            bool status = false;

            using (TeacherRegEntities teacherReg = new TeacherRegEntities())
            {
                var account = teacherReg.TeacherLogins.Where(a => a.Email == Email).FirstOrDefault();
                if (account != null)
                {
                    //Send email for reset password
                    string resetCode = Guid.NewGuid().ToString();
                    SendVerificationLinkEmail(account.Email, resetCode, "ResetPassword");
                    account.ResetPasswordCode = resetCode;

                    //Avoid confirm password not match issue, as we had added a confirm password property
                    teacherReg.Configuration.ValidateOnSaveEnabled = false;
                    teacherReg.SaveChanges();
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

        // GET: Teachers/ResetPassword // by teacher
        public ActionResult ResetPassword(string id)
        {
            //Verify the reset password link
            //Find account associated with this link
            //redirect to reset password page
            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }

            using (TeacherRegEntities teacherReg = new TeacherRegEntities())
            {
                var user = teacherReg.TeacherLogins.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
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

        // POST: Teachers/ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPassword reset)
        {
            if (ModelState.IsValid)
            {
                using (TeacherRegEntities teacherReg = new TeacherRegEntities())
                {
                    var user = teacherReg.TeacherLogins.Where(a => a.ResetPasswordCode == reset.ResetCode).FirstOrDefault();
                    if (user != null)
                    {
                        user.Password = Crypto.Hash(reset.NewPassword);
                        user.ResetPasswordCode = "";
                        teacherReg.Configuration.ValidateOnSaveEnabled = false;
                        teacherReg.SaveChanges();
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
        // GET: Teachers/Index // by teacher
        public ActionResult Index()
        {
            if (Request.Cookies.Get("teacher") != null)
            {
                Teachers teacher = new Teachers();
                return View(teacher.viewDetails().Find(smodel => smodel.Id == Request.Cookies.Get("teacher").Value));
            }
            else
            {
                return RedirectToAction("Login", "Teachers");
            }
        }

        // GET: Teachers/UpdateInfo // by teacher
        public ActionResult UpdateInfo()
        {
            if (Request.Cookies.Get("teacher") != null)
            {
                Teachers teacher = new Teachers();
                return View(teacher.viewDetails().Find(smodel => smodel.Id == Request.Cookies.Get("teacher").Value));
            }
            else
            {
                return RedirectToAction("Login", "Teachers");
            }
        }

        // POST: Teachers/UpdateInfo
        [HttpPost]
        public ActionResult UpdateInfo(Teachers teacher)
        {
            string id = Request.Cookies.Get("teacher").Value;
            string fileName = Path.GetFileNameWithoutExtension(teacher.ImageFile.FileName);
            teacher.ImagePath = id + fileName + System.IO.Path.GetExtension(teacher.ImageFile.FileName);
            fileName = "~/TeachersImage/" + id + fileName + System.IO.Path.GetExtension(teacher.ImageFile.FileName);
            teacher.ImageFile.SaveAs(Server.MapPath(fileName));
            try
            {
                if (ModelState.IsValid)
                {
                    teacher.updateInfo(id);
                    return RedirectToAction("Index");
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // Teachers/TeachersList // by all users
        public ActionResult TeachersList()
        {
            if (Request.Cookies.Get("admin") != null)
            {
                return RedirectToAction("Home", "ERP");
            }
            else
            {
                Teachers teachers = new Teachers();
                return View(teachers.teachersList());
            }
        }
    }
}