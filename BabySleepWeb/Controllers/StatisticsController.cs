using Microsoft.AspNetCore.Mvc;

namespace BabySleepWeb.Controllers
{
    public class StatisticsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
