using BabySleep.Application.DTO;
using BabySleep.Application.DTOAssemblers;
using BabySleep.Application.Interfaces;
using BabySleep.Common.Helpers;
using BabySleep.Infrastructure.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace BabySleep.Application.Services
{
    public class ChildSleepMainService : IChildSleepMainService
    {
        private readonly ISleepRepository sleepRepository;
        private readonly IChildSleepMainDtoAssembler sleepDtoAssembler;

        public ChildSleepMainService(ISleepRepository sleepRepository, IChildSleepMainDtoAssembler sleepDtoAssembler)
        {
            this.sleepDtoAssembler = sleepDtoAssembler;
            this.sleepRepository = sleepRepository;
        }

        public IList<ChildSleepMainDto> GetChildSleeps(Guid childGuid, DateTime currentDate)
        {
            return sleepDtoAssembler.WriteSleepsDto(sleepRepository.Take(childGuid, currentDate));
        }
    }
}
