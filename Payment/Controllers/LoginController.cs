
using Microsoft.AspNetCore.Mvc;
using Payment.Models;
using System.Data.OleDb;

namespace Payment.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]//attribute get for login page
        public IActionResult Login()//method login
        {
            ModelState.Clear();
            return View(new User());
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            // بررسی خالی بودن Username
            if (string.IsNullOrWhiteSpace(user.UserName) || string.IsNullOrWhiteSpace(user.Password) )
            {
                ViewBag.Error = "please fill in all fields";
                return View(user);
            }

            string dbPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Database",
                "Payment.accdb"
            );//path of database

            string connectionString =
                $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};";//اطلاعات لازم برای وصل شدن برنامه به دیتابیس.

            string query =
                "SELECT COUNT(*) FROM [User] WHERE username = ? AND password = ?";

            using OleDbConnection connection =
                new OleDbConnection(connectionString);//برقراری ارتباط با دیتا بیس

            using OleDbCommand command =
                new OleDbCommand(query, connection);//اجرای کوئری

            command.Parameters.AddWithValue("@username", user.UserName);
            command.Parameters.AddWithValue("@password", user.Password);

            connection.Open();//اتصال به دیتابیس را باز کن

            int result = Convert.ToInt32(command.ExecuteScalar());//کوئری را اجرا میکند و میبیند که داده ای که ما وارد کردیم و در دیتا بیس هست یا نه 
            connection.Close();
            if (result > 0)
            {
                // ذخیره نام کاربری در Session
                HttpContext.Session.SetString("UserName", user.UserName);

                return RedirectToAction("Record", "Record");
            }

            ViewBag.Error = "username or password is incorrect";

            return View(user);
        }
    }
}