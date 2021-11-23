using BabySleep.Application.DTO;
using BabySleep.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Application.DTOAssemblers
{
    public class ChildSleepEntryDtoAssembler : IChildSleepEntryDtoAssembler
    {
        public ChildSleepEntryDto WriteSleepDto(Sleep sleep)
        {
            if (sleep == null)
            {
                sleep = new Sleep(Guid.Empty, Guid.Empty, new DateTime(), new DateTime());
            }

            var sleepDto = new ChildSleepEntryDto()
            {
                AwakeningCount = sleep.AwakeningCount,
                ChildGuid = sleep.ChildGuid,
                EndTime = sleep.SleepTime.EndTime,
                FallAsleepTime = sleep.FallAsleepTime,
                FeedingCount = sleep.FeedingCount,
                Notes = sleep.CustomerInfo.Note,
                Quality = sleep.CustomerInfo.Quality,
                SleepGuid = sleep.SleepGuid,
                SleepPlace = sleep.SleepPlace,
                StartTime = sleep.SleepTime.StartTime
            };

            return sleepDto;
        }
    }
}
