using BabySleep.Application.DTO;
using BabySleep.Common.Enums;
using BabySleep.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Tests.Helpers
{
    public static class SleepEntryHelper
    {
        public static ChildSleepEntryDto FillChildSleepEntryDto(Guid sleepGuid, Guid childGuid,
            DateTime startTime, DateTime endTime, SleepPlace sleepPlace, short quality, string note,
            short feedingCount, short awakeningCount, int fallAsleepTime)
        {
            return new ChildSleepEntryDto()
            {
                SleepGuid = sleepGuid,
                ChildGuid = childGuid,
                EndTime = endTime,
                StartTime = startTime,
                SleepPlace = sleepPlace,
                Notes = note,
                Quality = quality,
                FeedingCount = feedingCount,
                AwakeningCount = awakeningCount,
                FallAsleepTime = fallAsleepTime
            };
        }

        public static Sleep FillChildSleep(Guid sleepGuid, Guid childGuid, 
            DateTime startTime, DateTime endTime, SleepPlace sleepPlace, short quality, string note,
            short feedingCount, short awakeningCount, int fallAsleepTime)
        {
            return new Sleep(sleepGuid, childGuid, sleepPlace, startTime, endTime, feedingCount, fallAsleepTime, awakeningCount, quality, note);
        }
    }
}
