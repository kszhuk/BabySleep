using BabySleep.Application.DTO;
using BabySleep.Application.DTOAssemblers;
using BabySleep.Application.Interfaces;
using BabySleep.Common.Helpers;
using BabySleep.Infrastructure.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace BabySleep.Application.Services
{
    public class ChildSleepMainService : IChildSleepMainService
    {
        private readonly ISleepRepository sleepRepository;
        private readonly IChildSleepCarouselDtoAssembler sleepDtoAssembler;

        public ChildSleepMainService(ISleepRepository sleepRepository, IChildSleepCarouselDtoAssembler sleepDtoAssembler)
        {
            this.sleepDtoAssembler = sleepDtoAssembler;
            this.sleepRepository = sleepRepository;
        }

        public IList<ChildSleepCarouselDto> GetChildSleeps(Guid childGuid, DateTime maxDate)
        {
            return sleepDtoAssembler.WriteSleepsDto(sleepRepository.Take(childGuid, Constants.DAYS_SLEEPS_COUNT, maxDate), maxDate);
        }
        //public List<ChildSleepCarouselDto> GetChildSleeps(Guid childGuid)
        //{
        //    var currentDate = DateTime.Now;

        //    var childSleeps = new List<ChildSleepCarouselDto>();

        //    DateTime start = currentDate.AddDays(-4);
        //    DateTime end = currentDate;

        //    for (DateTime counter = start; counter <= end; counter = counter.AddDays(1))
        //    {
        //        var sleeps = new List<ChildSleepMainDto>();
        //        var prevDay = counter.AddDays(-1);
        //        var nextDay = counter.AddDays(1);
        //        sleeps.Add(new ChildSleepMainDto()
        //        {
        //            SleepGuid = Guid.NewGuid(),
        //            SleepType = SleepType.NightSleep,
        //            StartTime = new DateTime(prevDay.Year, prevDay.Month, prevDay.Day, 21, 10, 0),
        //            EndTime = new DateTime(counter.Year, counter.Month, counter.Day, 7, 5, 0),
        //            Wakefulness = new DateTime(prevDay.Year, prevDay.Month, prevDay.Day, 6, 15, 0).ToString(@"hh\:mm"),
        //            Quality = "2/10",
        //            Notes = "123"
        //        });
        //        sleeps.Add(new ChildSleepMainDto()
        //        {
        //            SleepGuid = Guid.NewGuid(),
        //            SleepType = SleepType.DaySleep,
        //            StartTime = new DateTime(counter.Year, counter.Month, counter.Day, 13, 15, 0),
        //            EndTime = new DateTime(counter.Year, counter.Month, counter.Day, 15, 30, 0),
        //            Wakefulness = new DateTime(prevDay.Year, prevDay.Month, prevDay.Day, 5, 10, 0).ToString(@"hh\:mm"),
        //            Quality = "5/10",
        //            Notes = ""
        //        });
        //        sleeps.Add(new ChildSleepMainDto()
        //        {
        //            SleepGuid = Guid.NewGuid(),
        //            SleepType = SleepType.NightSleep,
        //            StartTime = new DateTime(counter.Year, counter.Month, counter.Day, 21, 5, 0),
        //            EndTime = new DateTime(nextDay.Year, nextDay.Month, nextDay.Day, 7, 15, 0),
        //            Wakefulness = "",
        //            Quality = "9/10",
        //            Notes = "abcdefghi jklmnopqrstuvw xyzABCDEFGHIJKLMNOPQRSTUVWXY Z0123456789"
        //        });

        //        childSleeps.Add(new ChildSleepCarouselDto() { SleepDate = counter, ChildSleeps = sleeps });
        //        //calculatedDates.Add(counter);
        //    }

        //    return childSleeps;

        //}
    }
}
