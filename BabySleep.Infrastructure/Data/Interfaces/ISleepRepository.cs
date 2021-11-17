using BabySleep.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Infrastructure.Data.Interfaces
{
    public interface ISleepRepository
    {
        IList<Sleep> GetAll(Guid childGuid);
        Sleep Get(Guid sleepGuid);
        void Add(Sleep sleep);
        void Delete(Guid sleepGuid);
        IList<Sleep> Take(Guid childGuid, DateTime currentDate);
        void Update(Sleep sleep);
    }
}
