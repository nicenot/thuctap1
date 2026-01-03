using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Web.Mvc;
using thuviensachcaocap.Models;

namespace thuviensachcaocap.Controllers
{
    public class BooksController : Controller
    {
        private readonly string _connStr =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        // GET: Books
        public ActionResult Index()
        {
            List<Book> books = new List<Book>();

            using (SqlConnection conn = new SqlConnection(_connStr))
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

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Book book = null;

            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                string sql = "SELECT * FROM Books WHERE BookID = @id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();
                SqlDataReader rd = cmd.ExecuteReader();

                if (rd.Read())
                {
                    book = new Book
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
                    };
                }
            }

            if (book == null)
                return HttpNotFound();

            return View(book);
        }
    }
}
