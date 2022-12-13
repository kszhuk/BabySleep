using BabySleep.Common.Enums;
using BabySleep.Common.Helpers;
using BabySleep.Resources.Resx;
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
        public string StatisticsDayTotal { get => string.Format(ChildSleepResources.StatisticsDayTotal, DaySleepsCount,
                new TimeSpan(DaySleepsTime).ToString(Constants.SHORT_TIME_FORMAT)); }
        public string StatisticsNightTotal { get => new TimeSpan(NightSleepsTime).ToString(Constants.SHORT_TIME_FORMAT); }
        public string StatisticsTotal { get => new TimeSpan(TotalSleepsTime).ToString(Constants.SHORT_TIME_FORMAT); }
    }
}
