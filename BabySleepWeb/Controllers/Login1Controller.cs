using BabySleepWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BabySleepWeb.Controllers
{
    public class Login1Controller : Controller
    {
        //GET
        public IActionResult Login()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(InputLoginModel obj)
        {
            if(!ModelState.IsValid)
            {
                return View(obj);
            }

            ModelState.AddModelError("LoginError", "Can't login");
            ModelState.AddModelError("Password", "Wrong pwd");
            //return RedirectToAction("Index");
            return View(obj);
        }
    }
}
