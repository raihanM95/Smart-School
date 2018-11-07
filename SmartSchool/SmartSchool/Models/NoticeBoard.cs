using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
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

        [Display(Name = "Uploaded file")]
        public string FileName { get; set; }

        public byte[] FileContent { get; set; }

        [Required(ErrorMessage = "Date required")]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        [Required]
        [DataType(DataType.Upload)]
        [Display(Name = "Select file")]
        public HttpPostedFileBase Files { get; set; }

        public bool publishNotice()
        {
            //string query = @"INSERT INTO NoticeBoard (Title, FileName, FileContent, Date) VALUES ('" + Title + "', '" + FileName + "', @FileContent, '" + Date + "')";
            
            //SqlCommand command = new SqlCommand(query);
            //command.Parameters.Add("@Title", Title);
            //command.Parameters.Add("@FileName", FileName);
            //command.Parameters.Add("@FileContent", SqlDbType.VarBinary).Value = FileContent;
            //command.Parameters.Add("@Date", Date);
            //int i = dam.Execute(query);


            //int i = dam.Save(command);

            try
            {
                SqlConnection con = new SqlConnection(dam.ConnnectionString);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                DynamicParameters Parm = new DynamicParameters();
                Parm.Add("@Title", Title);
                Parm.Add("@FileName", FileName);
                Parm.Add("@FileContent", FileContent);
                Parm.Add("@Date", Date);
                con.Execute("NoticeBoard", Parm, commandType: System.Data.CommandType.StoredProcedure);
                return true;
            }
            catch (Exception ex) {
                return false;
            }

            /*if (i >= 1)
                return true;
            else
                return false;*/
        }

        public List<NoticeBoard> viewNotices()
        {
            List<NoticeBoard> noticelist = new List<NoticeBoard>();
            string query = @"SELECT* FROM NoticeBoard";

            foreach (DataRow dr in dam.GetDataTable(query).Rows)
            {
                noticelist.Add(new NoticeBoard
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    Title = Convert.ToString(dr["Title"]),
                    FileName = Convert.ToString(dr["FileName"]),
                    FileContent = (Byte[])(dr["FileContent"]),
                });
            }
            return noticelist;
        }
    }
}