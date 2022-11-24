using AutoMapper;
using BabySleep.Application.DTO;
using BabySleep.Application.Interfaces;
using BabySleep.Resources.Resx;
using BabySleepWeb.Helpers;
using BabySleepWeb.Models;
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
        private readonly IChilidSleepEntryService _sleepEntryService;

        public SleepController(IMemoryCache memoryCache, IChildSleepMainService sleepService, IChilidSleepEntryService sleepEntryService)
        {
            _memoryCache = memoryCache;
            _sleepService = sleepService;
            _sleepEntryService = sleepEntryService;
        }

        public IActionResult Index()
        {
            var childGuid = Guid.Empty;
            _memoryCache.TryGetValue(CacheKeys.CurrentChildGuid, out childGuid);

            var data = _sleepService.GetChildSleeps(childGuid, DateTime.Now);

            return View(data);
        }

        //public IActionResult AddEditSleep()
        //{
        //    return PartialView("SleepEntry", new InputSleepModel());
        //}

        public IActionResult AddEditSleep(Guid sleepGuid)
        {
            var sleep = new ChildSleepEntryDto();

            if(sleepGuid == Guid.Empty)
            {
                sleep.StartTime = DateTime.Now;
                sleep.EndTime = DateTime.Now;
            }
            else
            {
                sleep = _sleepEntryService.GetSleep(sleepGuid);
            }

            var config = new MapperConfiguration(cfg =>
                    cfg.CreateMap<ChildSleepEntryDto, InputSleepModel>()
                    .ForMember(dest => dest.SleepPlaceValue, act => act.MapFrom(src => (short)src.SleepPlace)));
            var mapper = new Mapper(config);
            var inputSleep = mapper.Map<InputSleepModel>(sleep);

            return PartialView("SleepEntry", inputSleep);
        }

        [HttpPost]
        public IActionResult SleepEntry(InputSleepModel sleep)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("SleepEntry", sleep);
            }

            if ((sleep.EndTime - sleep.StartTime).Ticks < 0)
            {
                ModelState.AddModelError(string.Empty, ChildSleepResources.SleepTimeException);
            }

            return PartialView("SleepEntry", sleep);
        }
    }
}
