using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using thuviensachcaocap.Models;

namespace thuviensachcaocap.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<Book> books = new List<Book>();

            // 🔥 LẤY EF CONNECTION
            string efConnStr = ConfigurationManager
                .ConnectionStrings["WebDocSachEntities"]
                .ConnectionString;

            // 🔥 TÁCH provider connection string THỦ CÔNG
            string sqlConnStr = efConnStr.Substring(
                efConnStr.IndexOf("provider connection string=\"")
                + "provider connection string=\"".Length);

            sqlConnStr = sqlConnStr.TrimEnd('"');

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                string sql = @"SELECT 
                                BookID, Title, Slug, Description, CoverImage,
                                AuthorID, CategoryID, IsVIP, Status, CreatedAt
                               FROM Books
                               ORDER BY CreatedAt DESC";

                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();

                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    books.Add(new Book
                    {
                        BookID = (int)rd["BookID"],
                        Title = rd["Title"].ToString(),
                        Slug = rd["Slug"].ToString(),
                        Description = rd["Description"].ToString(),
                        CoverImage = rd["CoverImage"].ToString(),
                        AuthorID = rd["AuthorID"] as int?,
                        CategoryID = rd["CategoryID"] as int?,
                        IsVIP = rd["IsVIP"] as bool?,
                        Status = rd["Status"].ToString(),
                        CreatedAt = rd["CreatedAt"] as DateTime?
                    });
                }
            }

            return View(books);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}
