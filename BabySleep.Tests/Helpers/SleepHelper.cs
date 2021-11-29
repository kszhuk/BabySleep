using BabySleep.Application.DTO;
using BabySleep.Common.Enums;
using BabySleep.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Tests.Helpers
{
    public static class SleepHelper
    {
        public static ChildSleepMainDto FillChildSleepMainDto(List<ChildSleepMainItemDto> sleepItemDtos,
            long daySleepsCount, long daySleepsTime, long nightSleepsTime, long totalSleepsTime)
        {
            return new ChildSleepMainDto()
            {
                ChildSleeps = sleepItemDtos,
                DaySleepsCount = daySleepsCount,
                DaySleepsTime = daySleepsTime,
                NightSleepsTime = nightSleepsTime,
                TotalSleepsTime = totalSleepsTime
            };
        }

        public static ChildSleepMainItemDto FillChildSleepMainItemDto(Guid sleepGuid, SleepType sleepType,
            DateTime startTime, DateTime endTime, string quality, string wakefulness, string note,
            bool isDaySleep, string duration, long durationTicks)
        {
            return new ChildSleepMainItemDto()
            {
                SleepGuid = sleepGuid,
                SleepType = sleepType,
                EndTime = endTime,
                StartTime = startTime,
                Note = note,
                Quality = quality,
                Wakefulness = wakefulness,
                IsDaySleep = isDaySleep,
                Duration = duration,
                DurationTicks = durationTicks
            };
        }

        public static Sleep FillChildSleepMain(Guid sleepGuid, Guid childGuid, SleepPlace sleepPlace, DateTime startTime, DateTime endTime,
            short feedingCount, int fallAsleepTime, short awakeningCount, short quality, string note)
        {
            return new Sleep(sleepGuid, childGuid, sleepPlace, startTime, endTime, feedingCount, fallAsleepTime, awakeningCount, quality, note);
        }
    }
}
