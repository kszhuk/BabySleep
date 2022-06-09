using BabySleep.Common.Enums;
using BabySleep.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Domain.Models
{
    public class SleepTime
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration
        {
            get
            {
                return EndTime - StartTime;
            }
        }
        public SleepType SleepType
        {
            get
            {
                if(StartTime.Hour <= Constants.NIGHT_SLEEP_END || StartTime.Hour >= Constants.NIGHT_SLEEP_START)
                {
                    return SleepType.NightSleep;
                }
                return SleepType.DaySleep;
            }
        }

        public SleepTime(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
