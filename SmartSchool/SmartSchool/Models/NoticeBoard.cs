using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using Dapper;

namespace SmartSchool.Models
{
    public class NoticeBoard
    {
        DataManage dam = new DataManage();

        public int Id { get; set; }

        [Required(ErrorMessage = "Title required")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "File name")]
        public string FileName { get; set; }

        [Display(Name = "Uploaded file")]
        public string FilePath { get; set; }

        public HttpPostedFileBase Files { get; set; }

        [Required(ErrorMessage = "Date required")]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        // For SearchBox
        public string Search { get; set; }

        // For "viewNotices" method values store and view in "ViewNotices" html page
        public List<NoticeBoard> Data { get; set; }

        public bool publishNotice()
        {
            string query = @"INSERT INTO NoticeBoard (Title, FileName, FilePath, Date) VALUES ('" + Title + "', '" + FileName + "', '" + FilePath + "', '" + Date + "')";

            int i = dam.Execute(query);

            if (i >= 1)
                return true;
            else
                return false;
        }

        public List<NoticeBoard> viewNotices()
        {
            List<NoticeBoard> noticelist = new List<NoticeBoard>();
            string query = @"SELECT* FROM NoticeBoard WHERE Title LIKE '%" + Search + "%' OR Date LIKE '%" + Search + "%' ORDER BY Date DESC";

            foreach (DataRow dr in dam.GetDataTable(query).Rows)
            {
                noticelist.Add(new NoticeBoard
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    Title = Convert.ToString(dr["Title"]),
                    FileName = Convert.ToString(dr["FileName"]),
                    FilePath = Convert.ToString(dr["FilePath"]),
                    Date = Convert.ToDateTime(dr["Date"]),
                });
            }
            return noticelist;
        }

        public void GetFiles(int id)
        {
            string query = @"SELECT* FROM NoticeBoard WHERE Id = " + Id + "";

            foreach (DataRow dr in dam.GetDataTable(query).Rows)
            {
                FileName = Convert.ToString(dr["FileName"]);
                FilePath = Convert.ToString(dr["FilePath"]);
            }
        }

        public bool deleteFile(int id)
        {
            string query = @"DELETE FROM NoticeBoard WHERE Id = " + id + "";

            int i = dam.Execute(query);

            if (i >= 1)
                return true;
            else
                return false;
        }
    }
}