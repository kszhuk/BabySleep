using BabySleep.Application.DTO;
using BabySleep.Common.Enums;
using BabySleep.Common.Helpers;
using BabySleep.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BabySleep.Application.DTOAssemblers
{
    public class ChildSleepMainDtoAssembler : IChildSleepMainDtoAssembler
    {
        public ChildSleepMainDto WriteSleepDto(Sleep sleep, string wakefulness)
        {
            return new ChildSleepMainDto()
            {
                SleepGuid = sleep.SleepGuid,
                SleepType = sleep.SleepTime.SleepType,
                StartTime = sleep.SleepTime.StartTime,
                EndTime = sleep.SleepTime.EndTime,
                Wakefulness = wakefulness,
                Quality = FormatQuality(sleep.CustomerInfo.Quality),
                Note = sleep.CustomerInfo.Note
            };
        }

        public IList<ChildSleepMainDto> WriteSleepsDto(IList<Sleep> sleeps)
        {
            var sleepsDto = new List<ChildSleepMainDto>();

            for (int i = 0; i < sleeps.Count(); i++)
            {
                var currentSleep = sleeps[i];
                var wakefulness = (i == sleeps.Count() - 1) ? string.Empty :
                    CalcWakefulness(sleeps[i + 1].SleepTime.StartTime, currentSleep.SleepTime.EndTime);
                sleepsDto.Add(WriteSleepDto(sleeps[i], wakefulness));
            }

            return sleepsDto;
        }

        private string CalcWakefulness(DateTime date1, DateTime date2)
        {
            return (date1 - date2).ToString(@"hh\:mm");
        }

        private string FormatQuality(short quality)
        {
            return string.Format("{0}/10", quality);
        }
    }
}
