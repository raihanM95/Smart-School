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
        // GET: NoticeBoard/PublishNotice
        [HttpGet]
        public ActionResult PublishNotice()
        {
            return View();
        }

        // POST: NoticeBoard/PublishNotice
        [HttpPost]
        public ActionResult PublishNotice(NoticeBoard notice)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string FileExt = Path.GetExtension(notice.Files.FileName).ToUpper();
                    if (FileExt == ".PDF")
                    {
                        byte[] uploadFile = new byte[notice.Files.InputStream.Length];
                        notice.Files.InputStream.Read(uploadFile, 0, uploadFile.Length);
                        notice.FileName = notice.Files.FileName;
                        notice.FileContent = uploadFile;

                        //Byte[] data = new byte[notice.Files.ContentLength];
                        //notice.Files.InputStream.Read(data, 0, notice.Files.ContentLength);
                        //notice.FileName = notice.Files.FileName; ;
                        //notice.FileContent = data;

                        //Stream str = notice.Files.InputStream;
                        //BinaryReader Br = new BinaryReader(str);
                        //Byte[] FileDet = Br.ReadBytes((Int32)str.Length);
                        //notice.FileName = notice.Files.FileName;
                        //notice.FileContent = FileDet;

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
                    else
                    {
                        ViewBag.Message = "Invalid File";
                    }
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // NoticeBoard/ViewNotice
        public ActionResult ViewNotice()
        {
            return View();
        }

        [HttpGet]
        public FileResult DownLoadFile(int id)
        {
            List<NoticeBoard> ObjFiles = new NoticeBoard().viewNotices();
            var FileById = (from FC in ObjFiles
                            where FC.Id.Equals(id)
                            select new { FC.FileName, FC.FileContent }).ToList().FirstOrDefault();
            return File(FileById.FileContent, "application/pdf", FileById.FileName);
        }

        // NoticeBoard/DeleteNotice
        //public ActionResult DeleteNotice(int id)
        //{
        //    return View();
        //}
    }
}
