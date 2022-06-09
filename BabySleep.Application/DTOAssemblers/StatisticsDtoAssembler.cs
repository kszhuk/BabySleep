using BabySleep.Application.DTO;
using BabySleep.Common.Helpers;
using BabySleep.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BabySleep.Application.DTOAssemblers
{
    public class StatisticsDtoAssembler : IStatisticsDtoAssembler
    {
        public StatisticsDto WriteStatisticsDto(IList<Sleep> sleeps, DateTime startDate, DateTime endDate)
        {
            var statisticsDto = new StatisticsDto();

            var totalHours = new List<StatisticsEntryDto>();
            var nightHours = new List<StatisticsEntryDto>();
            var dayHours = new List<StatisticsEntryDto>();
            var daySleepsCount = new List<StatisticsEntryDto>();

            var childSleepMainDtoAssembler = new ChildSleepMainDtoAssembler();

            for (var currentDate = startDate; currentDate <= endDate; currentDate = currentDate.AddDays(1))
            {
                var sleepsDate = sleeps.Where(s => DateTimeHelper.AreEqualDates(currentDate, s.SleepTime.StartTime) || DateTimeHelper.AreEqualDates(currentDate, s.SleepTime.EndTime)).ToList();
                var childSleeps = childSleepMainDtoAssembler.WriteSleepsDto(sleepsDate, currentDate);

                totalHours.Add(WriteStatisticsEntryDto(currentDate, childSleeps.TotalSleepsTime));
                nightHours.Add(WriteStatisticsEntryDto(currentDate, childSleeps.NightSleepsTime));
                dayHours.Add(WriteStatisticsEntryDto(currentDate, childSleeps.DaySleepsTime));
                daySleepsCount.Add(WriteStatisticsEntryDto(currentDate, childSleeps.DaySleepsCount, false));
            }


            statisticsDto.DayHoursStatistics = dayHours;
            statisticsDto.DaySleepsCountStatistics = daySleepsCount;
            statisticsDto.NightHoursStatistics = nightHours;
            statisticsDto.TotalHoursStatistics = totalHours;

            return statisticsDto;
        }

        private StatisticsEntryDto WriteStatisticsEntryDto(DateTime date, long value, bool isDate = true)
        {
            return new StatisticsEntryDto()
            {
                Label = date.ToString("MMM dd"),
                Value = value,
                ValueLabel = isDate ? new TimeSpan(value).ToString(Constants.SHORT_TIME_FORMAT) : value.ToString()
            };
        }
    }
}
