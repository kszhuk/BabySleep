using BabySleep.Application.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Tests.Helpers
{
    public static class StatisticsHelper
    {
        public static StatisticsEntryDto FillEmptyStatisticsEntryDto(DateTime currentDate, bool isDate = true)
        {
            return FillStatisticsEntryDto(currentDate, 0, isDate ? "00:00" : "0");
        }

        public static StatisticsDto FillStatisticsDto(DateTime currentDate, bool isDate = true)
        {
            var statisticsDto = new StatisticsDto();



            return statisticsDto;
        }

        public static StatisticsEntryDto FillStatisticsEntryDto(DateTime currentDate, float value, string valueLabel)
        {
            var statisticsEntryDto = new StatisticsEntryDto()
            {
                Value = value,
                Label = currentDate.ToString("MMM dd"),
                ValueLabel = valueLabel
            };

            return statisticsEntryDto;
        }
    }
}
