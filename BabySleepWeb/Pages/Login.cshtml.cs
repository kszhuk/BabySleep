using BabySleepWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BabySleepWeb.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;

        [BindProperty]
        public InputLoginModel InputLoginModel { get; set; }

        public LoginModel(ILogger<LoginModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.AddModelError(String.Empty, "Can't login");
            ModelState.AddModelError("InputLoginModel.Password", "Wrong pwd");

            if (!ModelState.IsValid)
                return Page();

            //var res = await _studentService.AddStudentAsync(this.Student);

            //if (res)
            //    return RedirectToPage("./Index");
            //else
                return RedirectToAction("Index", "Home");   
        }

        //public async Task<IActionResult> OnPostAsync()
        //{
        //    if (!ModelState.IsValid)
        //        return Page();

        //    return RedirectToAction("Index", "Home");
        //       // return Page();
        //}
    }
}
