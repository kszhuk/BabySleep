using BabySleep.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Infrastructure.Data.Interfaces
{
    public interface ISleepRepository
    {
        Sleep Get(Guid sleepGuid);
        void Add(Sleep sleep);
        void Delete(Guid sleepGuid);
        IList<Sleep> Take(Guid childGuid, DateTime currentDate);
        IList<Sleep> Take(Guid childGuid, DateTime startDate, DateTime endDate);
        void Update(Sleep sleep);
    }
}
