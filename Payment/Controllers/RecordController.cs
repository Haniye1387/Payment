using Microsoft.AspNetCore.Mvc;
using Payment.Models;
using System.Data.OleDb;
using System.IO;

namespace Payment.Controllers
{
    public class RecordController : Controller
    {

       
        [HttpGet]
        public IActionResult Record()
        {
            return View(new Cost());
        }

        [HttpPost]
        public IActionResult Record(Cost cost)
        {
            string? username = HttpContext.Session.GetString("UserName");

            string dbPath = Path.Combine(
               Directory.GetCurrentDirectory(),
               "Database",
               "Payment.accdb"
           );//path of database

            string connectionString =
                $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};";
            if (string.IsNullOrWhiteSpace(username) ||
             string.IsNullOrWhiteSpace(cost.TypeOfCost) ||
              cost.Price <= 0)
            {
                ViewBag.Error = "Please fill all fields correctly.";
                return View(cost);
            }
            cost.UserName = username;
            cost.Date = DateTime.Now.ToString("yyyy/MM/dd");
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                string query = @"
                    INSERT INTO cost
                    (username, [type], price, [date])
                    VALUES (?, ?, ?, ?)";

                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("username", cost.UserName);
                    command.Parameters.AddWithValue("type", cost.TypeOfCost);
                    command.Parameters.AddWithValue("price", cost.Price);
                    command.Parameters.AddWithValue("date", cost.Date);

                    command.ExecuteNonQuery();
                }
            }

            // بعد از ثبت، فرم خالی می‌شود
            return RedirectToAction("Record");
        }

        [HttpGet]
        public IActionResult Report()
        {
            return RedirectToAction("Report", "Report");
        }
    
}

}
