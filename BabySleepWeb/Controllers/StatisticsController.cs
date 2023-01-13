using BabySleep.Application.Interfaces;
using BabySleepWeb.Helpers;
using BabySleepWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace BabySleepWeb.Controllers
{
    [Authorize]
    public class StatisticsController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IStatisticsService _statisticsService;
        private readonly IMemoryCache _memoryCache;

        public StatisticsController(ILogger<LoginController> logger, IMemoryCache memoryCache, IStatisticsService statisticsService)
        {
            _logger = logger;
            _statisticsService = statisticsService;
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetStatistics(DateTime startDate, DateTime endDate)
        {
            var childGuid = Guid.Empty;
            _memoryCache.TryGetValue(CacheKeys.CurrentChildGuid, out childGuid);

            var data = _statisticsService.GetStatistics(childGuid, startDate, endDate);

            var statistics = new StatisticsModel();

            statistics.TotalHoursStatistics = data.TotalHoursStatistics;
            statistics.NightHoursStatistics = data.NightHoursStatistics;
            statistics.DayHoursStatistics = data.DayHoursStatistics;

            var json = JsonConvert.SerializeObject(statistics);

            return Json(json);
        }
    }
}
