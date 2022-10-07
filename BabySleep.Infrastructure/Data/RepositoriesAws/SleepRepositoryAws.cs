using BabySleep.Domain.Models;
using BabySleep.Infrastructure.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Infrastructure.Data.RepositoriesAws
{
    public class SleepRepositoryAws : ISleepRepository
    {
        public void Add(Sleep sleep)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid sleepGuid)
        {
            throw new NotImplementedException();
        }

        public Sleep Get(Guid sleepGuid)
        {
            throw new NotImplementedException();
        }

        public IList<Sleep> Take(Guid childGuid, DateTime currentDate)
        {
            throw new NotImplementedException();
        }

        public IList<Sleep> Take(Guid childGuid, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public void Update(Sleep sleep)
        {
            throw new NotImplementedException();
        }
    }
}
