using AutoMapper;
using BabySleep.Application.DTO;
using BabySleep.Application.Interfaces;
using BabySleep.Common.Enums;
using BabySleep.Common.Exceptions.Sleep;
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
                return PartialView("SleepEntry", sleep);
            }

            try
            {
                var childGuid = Guid.Empty;
                _memoryCache.TryGetValue(CacheKeys.CurrentChildGuid, out childGuid);

                var childSleep = new ChildSleepEntryDto()
                {
                    AwakeningCount = sleep.AwakeningCount,
                    EndTime = sleep.EndTime,
                    FallAsleepTime = sleep.FallAsleepTime,
                    FeedingCount = sleep.FeedingCount,
                    Quality = sleep.Quality,
                    SleepGuid = sleep.SleepGuid,
                    SleepPlace = (SleepPlace)sleep.SleepPlaceValue,
                    StartTime = sleep.StartTime,
                    ChildGuid = childGuid
                };
                _sleepEntryService.Save(childSleep);
                return Json(new { redirectToUrl = Url.Action("Index", "Sleep") });
            }
            catch (SleepAlreadyExistsException)
            {
                ModelState.AddModelError(string.Empty, ChildSleepResources.SleepAlreadyExistsException);
            }
            catch (SleepDurationException)
            {
                ModelState.AddModelError(string.Empty, string.Format(ChildSleepResources.SleepDurationException, BabySleep.Common.Helpers.Constants.MAX_SLEEP_DURATION));
            }
            catch (SleepTimeException)
            {
                ModelState.AddModelError(string.Empty, ChildSleepResources.SleepTimeException);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return PartialView("SleepEntry", sleep);
        }

        [HttpPost]
        public IActionResult DeleteSleep(InputSleepModel sleep)
        {
            try
            {
                _sleepEntryService.Delete(sleep.SleepGuid);
                return Json(new { redirectToUrl = Url.Action("Index", "Sleep") });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return PartialView("SleepEntry", sleep);
        }
    }
}
