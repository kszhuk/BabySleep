using BabySleep.Application.DTO;
using BabySleep.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Application.DTOAssemblers
{
    public interface IChildSleepMainDtoAssembler
    {
        ChildSleepMainDto WriteSleepsDto(IList<Sleep> sleeps, DateTime currentDate);
    }
}
