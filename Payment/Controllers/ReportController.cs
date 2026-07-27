using Microsoft.AspNetCore.Mvc;
using Payment.Models;
using System.Data.OleDb;
using System.IO;

namespace Payment.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Report()
        {
            string dbPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Database",
                "Payment.accdb");

            string connectionString =
                $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};";

            string? username = HttpContext.Session.GetString("UserName");

            List<ReportModel> list = new List<ReportModel>();

            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                con.Open();

                string query = @"SELECT username,
                        [date],
                        SUM(price) AS TotalPrice
                 FROM cost
                 WHERE username = ?
                 GROUP BY username,[date]
                 ORDER BY [date]";

                OleDbCommand cmd = new OleDbCommand(query, con);
                cmd.Parameters.AddWithValue("@username", username);

                OleDbDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new ReportModel()
                    {
                        UserName = dr["username"].ToString(),
                        Date = dr["date"].ToString(),
                        TotalPrice = Convert.ToDecimal(dr["TotalPrice"])
                    });
                    
                }
            }

            return View(list);
        }
    }
}