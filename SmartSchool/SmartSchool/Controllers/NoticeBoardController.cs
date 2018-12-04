using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSchool.Models;
using System.IO;

namespace SmartSchool.Controllers
{
    public class NoticeBoardController : Controller
    {
        // GET: NoticeBoard/PublishNotice // by admin
        [HttpGet]
        public ActionResult PublishNotice()
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

        // POST: NoticeBoard/PublishNotice
        [HttpPost]
        public ActionResult PublishNotice(NoticeBoard notice)
        {
            notice.FileName = Path.GetFileNameWithoutExtension(notice.Files.FileName);
            notice.FilePath = notice.FileName + System.IO.Path.GetExtension(notice.Files.FileName);
            string fileName = "~/NoticeFiles/" + notice.FileName + System.IO.Path.GetExtension(notice.Files.FileName);
            notice.Files.SaveAs(Server.MapPath(fileName));
            try
            {
                if (ModelState.IsValid)
                {
                    if (notice.publishNotice())
                    {
                        ViewBag.Message = "Publish successfully";
                        ModelState.Clear();
                    }
                    else
                    {
                        ViewBag.Message = "Sorry! try again.";
                    }
                    return View();
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: NoticeBoard/ViewNotice // by admin
        [HttpGet]
        public ActionResult ViewNotice()
        {
            if (Request.Cookies.Get("admin") != null)
            {
                NoticeBoard notice = new NoticeBoard();
                notice.Data = notice.viewNotices();
                return View(notice);
            }
            else
            {
                return RedirectToAction("ERP", "ERP");
            }
        }

        // POST: NoticeBoard/ViewNotice
        [HttpPost]
        public ActionResult ViewNotice(NoticeBoard notice)
        {
            notice.Data = notice.viewNotices();
            return View(notice);
        }

        // NoticeBoard // Download Notice file
        [HttpGet]
        public FileResult DownloadFile(int id, NoticeBoard obj)
        {
            obj.GetFiles(id);
            string filepath = Server.MapPath("~/NoticeFiles/" + obj.FilePath);

            return File(filepath, "application/pdf", obj.FileName + ".pdf");
        }

        // NoticeBoard/ViewNotice // by admin
        public ActionResult DeleteFile(int id)
        {
            try
            {
                NoticeBoard notice = new NoticeBoard();
                if (notice.deleteFile(id))
                {
                    ViewBag.AlertMsg = "Delete Successfully";
                }
                return RedirectToAction("ViewNotice");
            }
            catch
            {
                return RedirectToAction("ViewNotice"); ;
            }
        }

        // GET: NoticeBoard/Notice // by all users
        [HttpGet]
        public ActionResult Notice()
        {
            if (Request.Cookies.Get("admin") != null)
            {
                return RedirectToAction("Home", "ERP");
            }
            else
            {
                NoticeBoard notice = new NoticeBoard();
                notice.Data = notice.viewNotices();
                return View(notice);
            }
        }

        // POST: NoticeBoard/Notice
        [HttpPost]
        public ActionResult Notice(NoticeBoard notice)
        {
            notice.Data = notice.viewNotices();
            return View(notice);
        }
    }
}
