using BabySleepWeb.Models;
using Firebase.Auth;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace BabySleepWeb.Pages
{
    [AllowAnonymous]
    public class Login2Model : PageModel
    {
        private static string ApiKey = "AIzaSyD93_x_1DrB7YpWraKhctJOLjPvWK6aNHU";
        private static string Bucket = "babysleepapp-d7b8b.appspot.com";

        private readonly ILogger<Login2Model> _logger;

        [BindProperty]
        public InputLoginModel InputLoginModel { get; set; }

        public Login2Model(ILogger<Login2Model> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnGet(string returnUrl)
        {
            //try
            //{
            //    if(this.Request.IsAuthenticated)
            //    {
            //        return this.Redirect(returnUrl);
            //    }
            //}
            //catch
            //{

            //}

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //ModelState.AddModelError(String.Empty, "Can't login");
            //ModelState.AddModelError("InputLoginModel.Password", "Wrong pwd");

            //if (!ModelState.IsValid)
            //    return Page();

            //    return RedirectToAction("Index", "Home");
            //    
            try
            {
                var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
                var a = await auth.SignInWithEmailAndPasswordAsync(InputLoginModel.Email, InputLoginModel.Password);

                string token = a.FirebaseToken;
                var user = a.User;

                if (token != string.Empty)
                {
                    await SignInUserAsync(user.Email, token, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login or password");
                }
                //var a = await auth.CreateUserWithEmailAndPasswordAsync(InputLoginModel.Email, InputLoginModel.Password);
                //ModelState.AddModelError(string.Empty, "VerifyLogin");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return Page();
        }

        private async Task SignInUserAsync(string email, string token, bool isPersistent)
        {
            var claims = new List<Claim>();

            try
            {
                claims.Add(new Claim(ClaimTypes.Email, email));
                claims.Add(new Claim(ClaimTypes.Authentication, token));
                var claimIdentity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

                var principal = new ClaimsPrincipal(claimIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                {
                    IsPersistent = isPersistent
                });


                //var ctx = HttpContext.SignInAsync();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        //{
        //    new Claim(ClaimTypes.Name, user.Email),
        //    new Claim("FullName", user.FullName),
        //    new Claim(ClaimTypes.Role, "Administrator"),
        //};

        //    var claimsIdentity = new ClaimsIdentity(
        //        claims, CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> OnPostLogout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Page();
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
