using BabySleep.Application.Interfaces;
using BabySleepWeb.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace BabySleepWeb.Controllers
{
    [Authorize]
    public class SleepController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IChildSleepMainService _sleepService;

        public SleepController(IMemoryCache memoryCache, IChildSleepMainService sleepService)
        {
            _memoryCache = memoryCache;
            _sleepService = sleepService;
        }

        public IActionResult Index()
        {
            var childGuid = Guid.Empty;
            _memoryCache.TryGetValue(CacheKeys.CurrentChildGuid, out childGuid);

            var data = _sleepService.GetChildSleeps(childGuid, DateTime.Now);

            return View(data);
        }
    }
}
