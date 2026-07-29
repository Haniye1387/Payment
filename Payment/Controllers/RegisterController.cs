using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using Payment.Models;
using System.Data.OleDb;

namespace Payment.Controllers
{
    public class RegisterController : Controller
    {
       
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            // بررسی خالی نبودن 
            if (string.IsNullOrWhiteSpace(user.UserName)|| string.IsNullOrWhiteSpace(user.Password)|| string.IsNullOrWhiteSpace(user.RePassword))
            {
                ViewBag.Error = "please fill in all fields";
                return View(user);
            }

            
            string dbPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Database",
                "Payment.accdb"
            );
            string connectionString =
                   $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};";
            // بررسی یکی بودن رمزها
            if (user.Password != user.RePassword)
            {
                ViewBag.Error = "Passwords do not match";
                return View(user);
            }

            using (OleDbConnection connection =
                   new OleDbConnection(connectionString))
            {
                connection.Open();

                // بررسی تکراری نبودن Username
                string Query =
                    "SELECT COUNT(*) FROM [User] WHERE [username] = ?";

                using (OleDbCommand Command =
                       new OleDbCommand(Query, connection))
                {
                    Command.Parameters.AddWithValue("@username", user.UserName);

                    int count = Convert.ToInt32(Command.ExecuteScalar());

                    if (count > 0)
                    {
                        ViewBag.Error = "Username already exists";
                        return View(user);
                    }
                }


                // ثبت کاربر
                string insertQuery =
                    @"INSERT INTO [User]
                      ([username], [password])
                      VALUES (?, ?)";

                using (OleDbCommand insertCommand =
                       new OleDbCommand(insertQuery, connection))
                {
                    insertCommand.Parameters.AddWithValue("@username", user.UserName ?? string.Empty);
                    insertCommand.Parameters.AddWithValue("@password", user.Password ??string.Empty);

                    insertCommand.ExecuteNonQuery();// برای  INSER UPDATE DELETE  استفاده میشه که نتیجه ای ندارند
                }
            }

            // رفتن به Login
            return RedirectToAction("Login", "Login");
        }
    }
}