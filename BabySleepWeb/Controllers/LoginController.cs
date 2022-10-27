using BabySleep.Application.Interfaces;
using BabySleep.Resources.Resx;
using BabySleepWeb.Helpers;
using BabySleepWeb.Models;
using Firebase.Auth;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace BabySleepWeb.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly FirebaseOptions _config;
        private readonly IUserService _userService;
        private readonly IChildrenHelper _childrenHelper;

        public LoginController(IOptions<FirebaseOptions> options, IUserService userService, IChildrenHelper childrenHelper)
        {
            _config = options.Value;
            _userService = userService;
            _childrenHelper = childrenHelper;
        }

        //GET
        public IActionResult Login()
        {
            return View();
        }

        //POST
        [HttpPost]
        public async Task<IActionResult> Login(InputLoginModel obj)
        {
            if(!ModelState.IsValid)
            {
                return View(obj);
            }

            try
            {
                var auth = new FirebaseAuthProvider(new FirebaseConfig(_config.ApiKey));
                var a = await auth.SignInWithEmailAndPasswordAsync(obj.Email, obj.Password);

                string token = a.FirebaseToken;
                var user = a.User;

                if (token != string.Empty)
                {
                    var userGuid = _userService.GetUserGuid(user.Email);

                    if (userGuid != string.Empty)
                    {
                        await SignInUserAsync(user.Email, userGuid, token, false);
                        _childrenHelper.LoadChildren(userGuid);
                        return RedirectToAction("Index", "Sleep");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, LoginResources.UserAbsentDynamoDb);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, LoginResources.LoginFailed);
                }
            }
            catch (FirebaseAuthException ex)
            {
                if (ex.Reason == AuthErrorReason.UnknownEmailAddress)
                {
                    ModelState.AddModelError(string.Empty, LoginResources.InvalidEmail);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, LoginResources.LoginFailed);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(obj);
        }

        //GET
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }

        private async Task SignInUserAsync(string email, string userGuid, string token, bool isPersistent)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Email, email));
            claims.Add(new Claim(ClaimTypes.Authentication, token));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userGuid));
            var claimIdentity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            var principal = new ClaimsPrincipal(claimIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
            {
                IsPersistent = isPersistent
            });
        }
    }
}
