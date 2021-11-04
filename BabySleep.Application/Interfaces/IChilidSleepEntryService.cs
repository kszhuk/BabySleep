using BabySleep.Application.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Application.Interfaces
{
    public interface IChilidSleepEntryService
    {
        ChildSleepEntryDto GetSleep(Guid sleepGuid);
        void Save(ChildSleepEntryDto sleep);
        void Delete(Guid sleepGuid);
    }
}
