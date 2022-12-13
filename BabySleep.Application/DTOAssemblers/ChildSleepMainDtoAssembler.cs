using BabySleep.Application.DTO;
using BabySleep.Common.Enums;
using BabySleep.Common.Helpers;
using BabySleep.Domain.Models;
using BabySleep.Resources.Resx;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BabySleep.Application.DTOAssemblers
{
    public class ChildSleepMainDtoAssembler : IChildSleepMainDtoAssembler
    {
        public ChildSleepMainDto WriteSleepsDto(IList<Sleep> sleeps, DateTime currentDate)
        {
            var sleepsDto = new List<ChildSleepMainItemDto>();

            for (int i = 0; i < sleeps.Count(); i++)
            {
                var currentSleep = sleeps[i];
                var wakefulness = (i == sleeps.Count() - 1) ? string.Empty :
                    CalcWakefulness(sleeps[i + 1].SleepTime.StartTime, currentSleep.SleepTime.EndTime);
                sleepsDto.Add(WriteSleepItemDto(sleeps[i], wakefulness));
            }

            var daySleepsTime = CalculateDaySleepsTime(sleepsDto);
            var nightSleepsTime = CalculateNightSleepsTime(sleepsDto, currentDate);
            var totalSleepsTime = daySleepsTime + nightSleepsTime;
            var daySleepsCount = sleepsDto.Count(s => s.IsDaySleep);

            return new ChildSleepMainDto()
            {
                ChildSleeps = sleepsDto,
                DaySleepsTime = daySleepsTime,
                NightSleepsTime = nightSleepsTime,
                TotalSleepsTime = totalSleepsTime,
                DaySleepsCount = daySleepsCount
            };
        }

        private ChildSleepMainItemDto WriteSleepItemDto(Sleep sleep, string wakefulness)
        {
            return new ChildSleepMainItemDto()
            {
                SleepGuid = sleep.SleepGuid,
                SleepType = sleep.SleepTime.SleepType,
                StartTime = sleep.SleepTime.StartTime,
                EndTime = sleep.SleepTime.EndTime,
                Wakefulness = wakefulness,
                Quality = FormatQuality(sleep.CustomerInfo.Quality),
                Note = sleep.CustomerInfo.Note,
                Duration = (sleep.SleepTime.EndTime - sleep.SleepTime.StartTime).ToString(Constants.SHORT_TIME_FORMAT),
                DurationTicks = (sleep.SleepTime.EndTime - sleep.SleepTime.StartTime).Ticks,
                IsDaySleep = sleep.SleepTime.SleepType == SleepType.DaySleep
            };
        }

        private string CalcWakefulness(DateTime date1, DateTime date2)
        {
            return (date1 - date2).ToString(Constants.SHORT_TIME_FORMAT);
        }

        private string FormatQuality(short quality)
        {
            return string.Format(Constants.QUALITY_FORMAT, quality);
        }

        private long CalculateDaySleepsTime(List<ChildSleepMainItemDto> sleeps)
        {
            return sleeps.Where(s => s.IsDaySleep).Sum(s => s.DurationTicks);
        }

        private long CalculateNightSleepsTime(List<ChildSleepMainItemDto> sleeps, DateTime currentDate)
        {
            var nightSleeps = sleeps.Where(s => !s.IsDaySleep);
            long nightSleepsTime = 0;
            foreach (var nightSleep in nightSleeps)
            {
                if (nightSleep.StartTime.Day == currentDate.Day && nightSleep.StartTime.Hour >= Constants.NIGHT_SLEEP_START)
                {
                    nightSleepsTime += nightSleep.DurationTicks;
                    continue;
                }

                var nextDate = nightSleep.StartTime.AddDays(1);
                if (nightSleep.StartTime.Day == nextDate.Day && nightSleep.StartTime.Hour <= Constants.NIGHT_SLEEP_END)
                {
                    nightSleepsTime += nightSleep.DurationTicks;
                    continue;
                }
            }

            return nightSleepsTime;
        }
    }
}
