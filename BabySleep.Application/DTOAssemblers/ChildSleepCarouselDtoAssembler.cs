using BabySleep.Application.DTO;
using BabySleep.Common.Enums;
using BabySleep.Common.Helpers;
using BabySleep.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BabySleep.Application.DTOAssemblers
{
    public class ChildSleepCarouselDtoAssembler : IChildSleepCarouselDtoAssembler
    {
        public ChildSleepCarouselDto WriteSleepDto(Sleep sleep)
        {
            if (sleep == null)
            {
                sleep = new Sleep();
            }

            var sleepDto = new ChildSleepCarouselDto();
            sleepDto.SleepDate = FormatEmptyDate(sleep.SleepTime.StartTime);
            sleepDto.ChildSleeps = new List<ChildSleepMainDto>();
            sleepDto.ChildSleeps.Add(new ChildSleepMainDto()
            {
                SleepGuid = sleep.SleepGuid,
                SleepType = sleep.SleepTime.SleepType,
                StartTime = sleep.SleepTime.StartTime,
                EndTime = sleep.SleepTime.EndTime,
                Wakefulness = string.Empty,
                Quality = FormatQuality(sleep.CustomerInfo.Quality),
                Note = sleep.CustomerInfo.Note
            });

            return sleepDto;
        }

        public IList<ChildSleepCarouselDto> WriteSleepsDto(IList<Sleep> sleeps, DateTime currentDate)
        {
            var childSleeps = new List<ChildSleepCarouselDto>();

            DateTime start = FormatEmptyDate(currentDate.AddDays(-(Constants.DAYS_SLEEPS_COUNT - 1)));
            DateTime end = FormatEmptyDate(currentDate.AddDays(1));

            for (DateTime counter = start; counter < end; counter = counter.AddDays(1))
            {
                var sleepsDto = new List<ChildSleepMainDto>();

                var currentSleeps = sleeps.Where(s => AreEqualDates(counter, s.SleepTime.StartTime) ||
                    AreEqualDates(counter, s.SleepTime.EndTime)).OrderBy(s => s.SleepTime.StartTime).ToList();

                for (int i = 0; i < currentSleeps.Count(); i++)
                {
                    var currentSleep = currentSleeps[i];
                    var wakefulness = (i == currentSleeps.Count() - 1) ? string.Empty : 
                        CalcWakefulness(currentSleeps[i + 1].SleepTime.StartTime, currentSleep.SleepTime.EndTime);

                    sleepsDto.Add(new ChildSleepMainDto()
                    {
                        SleepGuid = currentSleep.SleepGuid,
                        SleepType = currentSleep.SleepTime.SleepType,
                        StartTime = currentSleep.SleepTime.StartTime,
                        EndTime = currentSleep.SleepTime.EndTime,
                        Wakefulness = wakefulness,
                        Quality = FormatQuality(currentSleep.CustomerInfo.Quality),
                        Note = currentSleep.CustomerInfo.Note
                    });
                }

                childSleeps.Add(new ChildSleepCarouselDto() { SleepDate = counter, ChildSleeps = sleepsDto });
            }

            return childSleeps;
        }

        private string CalcWakefulness(DateTime date1, DateTime date2)
        {
            return (date1 - date2).ToString(@"hh\:mm");
        }

        private DateTime FormatEmptyDate(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
        }

        private string FormatQuality(short quality)
        {
            return string.Format("{0}/10", quality);
        }

        private bool AreEqualDates(DateTime date1, DateTime date2)
        {
            return date1.Year == date2.Year && date1.Month == date2.Month && date1.Day == date2.Day;
        }
    }
}
