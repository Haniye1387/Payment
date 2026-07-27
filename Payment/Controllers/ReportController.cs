using Microsoft.AspNetCore.Mvc;

namespace Payment.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
