using BabySleep.Application.DTO;
using BabySleep.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Application.DTOAssemblers
{
    public interface IChildSleepMainDtoAssembler
    {
        ChildSleepMainDto WriteSleepDto(Sleep sleep, string wakefulness);
        IList<ChildSleepMainDto> WriteSleepsDto(IList<Sleep> sleeps);
    }
}
