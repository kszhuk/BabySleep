﻿using BabySleepWeb.Helpers;
using BabySleepWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using System.Globalization;

namespace BabySleepWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMemoryCache _memoryCache;

        public HomeController(ILogger<HomeController> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            _memoryCache = memoryCache;

            if (CultureInfo.DefaultThreadCurrentCulture == null)
            {
                var language = CultureInfo.GetCultureInfo("en");
                CultureInfo.DefaultThreadCurrentCulture = language;
                CultureInfo.DefaultThreadCurrentUICulture = language;
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var error = HttpContext.Features.Get<IExceptionHandlerFeature>();
            _logger.LogError(string.Format(@"Global exception Message : {0}; \r\n StackTrace : {1}", error.Error.Message, error.Error.StackTrace));

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [AllowAnonymous]
        public IActionResult ChangeLanguage(string languageName)
        {
            try
            {
                var languages = CultureInfo.GetCultures(CultureTypes.NeutralCultures).ToList();
                var language = CultureInfo.GetCultures(CultureTypes.NeutralCultures).ToList()
                    .FirstOrDefault(element => element.Name == languageName);

                if (language != null)
                {
                    CultureInfo.DefaultThreadCurrentCulture = language;
                    CultureInfo.DefaultThreadCurrentUICulture = language;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("ChangeLanguage exception {0}", ex.Message));
            }

            return View("Index");
        }

        [AllowAnonymous]
        public IActionResult ChangeChild(Guid childGuid)
        {
            _memoryCache.Set(CacheKeys.CurrentChildGuid, childGuid);

            return View("Index");
        }
    }
}