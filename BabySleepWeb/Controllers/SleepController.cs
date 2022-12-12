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
        private readonly ILogger<LoginController> _logger;

        public SleepController(ILogger<LoginController> logger, IMemoryCache memoryCache,
            IChildSleepMainService sleepService, IChilidSleepEntryService sleepEntryService)
        {
            _memoryCache = memoryCache;
            _sleepService = sleepService;
            _sleepEntryService = sleepEntryService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var data = new ChildSleepMainDto();

            var childGuid = Guid.Empty;
            _memoryCache.TryGetValue(CacheKeys.CurrentChildGuid, out childGuid);

            try
            {
                data = _sleepService.GetChildSleeps(childGuid, DateTime.Now);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Main sleep for child {0} exception {1}", childGuid, ex.Message));
            }

            throw new Exception("test");

            return View(data);
        }

        public IActionResult ChangeDate(DateTime date)
        {
            var data = new ChildSleepMainDto();

            var childGuid = Guid.Empty;
            _memoryCache.TryGetValue(CacheKeys.CurrentChildGuid, out childGuid);

            try
            {
                data = _sleepService.GetChildSleeps(childGuid, date);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Main sleep change date for child {0} exception {1}", childGuid, ex.Message));
            }

            return PartialView("_Sleeps", data);
        }

        public IActionResult AddEditSleep(Guid sleepGuid)
        {
            var inputSleep = new InputSleepModel();

            try
            {
                var sleep = new ChildSleepEntryDto();

                if (sleepGuid == Guid.Empty)
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
                inputSleep = mapper.Map<InputSleepModel>(sleep);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("AddEditSleep for sleepGuid {0} exception {1}", sleepGuid, ex.Message));
            }

            return PartialView("_SleepEntry", inputSleep);
        }

        [HttpPost]
        public IActionResult SleepEntry(InputSleepModel sleep)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_SleepEntry", sleep);
            }

            if ((sleep.EndTime - sleep.StartTime).Ticks < 0)
            {
                ModelState.AddModelError(string.Empty, ChildSleepResources.SleepTimeException);
                return PartialView("_SleepEntry", sleep);
            }

            var childGuid = Guid.Empty;
            _memoryCache.TryGetValue(CacheKeys.CurrentChildGuid, out childGuid);

            try
            {
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
                _logger.LogError(string.Format("SaveSleep for sleepGuid {0} && childGuid {1} exception {2}", 
                    sleep.SleepGuid, childGuid, ex.Message));;
            }

            return PartialView("_SleepEntry", sleep);
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
                _logger.LogError(string.Format("DeleteSleep for sleepGuid {0} exception {1}", sleep.SleepGuid, ex.Message));
            }

            return PartialView("_SleepEntry", sleep);
        }
    }
}
