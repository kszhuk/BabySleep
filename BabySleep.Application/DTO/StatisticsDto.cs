using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Application.DTO
{
    public class StatisticsDto
    {
        public List<StatisticsEntryDto> TotalHoursStatistics { get; set; }
        public List<StatisticsEntryDto> NightHoursStatistics { get; set; }
        public List<StatisticsEntryDto> DayHoursStatistics { get; set; }
        public List<StatisticsEntryDto> DaySleepsCountStatistics { get; set; }
    }
}
