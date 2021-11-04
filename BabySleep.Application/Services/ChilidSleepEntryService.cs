using BabySleep.Application.DTO;
using BabySleep.Application.DTOAssemblers;
using BabySleep.Application.Interfaces;
using BabySleep.Domain.Models;
using BabySleep.Infrastructure.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Application.Services
{
    public class ChilidSleepEntryService : IChilidSleepEntryService
    {
        private readonly ISleepRepository sleepRepository;
        private readonly IChildSleepEntryDtoAssembler sleepDtoAssembler;

        public ChilidSleepEntryService(ISleepRepository sleepRepository, IChildSleepEntryDtoAssembler sleepDtoAssembler)
        {
            this.sleepDtoAssembler = sleepDtoAssembler;
            this.sleepRepository = sleepRepository;
        }

        public void Delete(Guid sleepGuid)
        {
            sleepRepository.Delete(sleepGuid);
        }

        public ChildSleepEntryDto GetSleep(Guid sleepGuid)
        {
            return sleepDtoAssembler.WriteSleepDto(sleepRepository.Get(sleepGuid));
        }

        public void Save(ChildSleepEntryDto sleepDto)
        {
            var sleep = new Sleep(sleepDto.SleepGuid, sleepDto.ChildGuid, sleepDto.SleepPlace,
                sleepDto.StartTime, sleepDto.EndTime, sleepDto.FeedingCount, sleepDto.FallAsleepTime, 
                sleepDto.AwakeningCount, sleepDto.Quality, sleepDto.Notes);

            if (sleep.SleepGuid == Guid.Empty)
            {
                sleepRepository.Add(sleep);
            }
            else
            {
                if (sleep.Validate())
                {
                    sleepRepository.Update(sleep);
                }
            }
        }
    }
}
