using BabySleep.Application.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Application.Interfaces
{
    public interface IChildSleepMainService
    {
        ChildSleepMainDto GetChildSleeps(Guid childGuid, DateTime currentDate);
    }
}
