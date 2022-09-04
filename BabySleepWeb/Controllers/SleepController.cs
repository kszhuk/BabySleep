using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BabySleepWeb.Controllers
{
    [Authorize]
    public class SleepController : Controller
    {
        public IActionResult Index()
        {
            //var a = ViewContext.RouteData.Values["controller"];
            return View();
        }
    }
}
