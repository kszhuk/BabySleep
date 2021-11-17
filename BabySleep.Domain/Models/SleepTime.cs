using BabySleep.Common.Enums;
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
                if(StartTime.Hour <= 5 || StartTime.Hour >= 19)
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
