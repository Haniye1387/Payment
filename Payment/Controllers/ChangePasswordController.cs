using Microsoft.AspNetCore.Mvc;
using Payment.Models;
using System.Data.OleDb;

namespace Payment.Controllers
{
    public class ChangePasswordController : Controller
    {
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePassword model)
        {
            if (string.IsNullOrWhiteSpace(model.UserName) ||
                string.IsNullOrWhiteSpace(model.NewPassword))
            {
                ViewBag.Error = "Please fill all fields.";
                return View(model);
            }

            string dbPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Database",
                "Payment.accdb");

            string connectionString =
                $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};";

            using OleDbConnection connection =
                new OleDbConnection(connectionString);

            connection.Open();

          
               string query = "UPDATE [User] SET [password]=? WHERE [username]=?"; 

            using OleDbCommand command =
                new OleDbCommand(query, connection);

            command.Parameters.AddWithValue("@password", model.NewPassword);
            command.Parameters.AddWithValue("@username", model.UserName);

            int result = command.ExecuteNonQuery();

            if (result > 0)
            {
                ViewBag.Success = "Password changed successfully.";
            }
            else
            {
                ViewBag.Error = "Username not found.";
            }

            return View();
        }
    }
}