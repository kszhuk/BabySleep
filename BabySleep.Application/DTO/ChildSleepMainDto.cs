using BabySleep.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Application.DTO
{
    /// <summary>
    /// Is used for ChildSleep tabbed page
    /// </summary>
    public class ChildSleepMainDto
    {
        public List<ChildSleepMainItemDto> ChildSleeps { get; set; }
        public long DaySleepsTime { get; set;  }
        public long DaySleepsCount { get; set; }
        public long NightSleepsTime { get; set; }
        public long TotalSleepsTime { get; set; }
        public string StatisticsDayTotal { get; set; }
        public string StatisticsNightTotal { get; set; }
        public string StatisticsTotal { get; set; }
    }
}
