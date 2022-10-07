using BabySleep.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Infrastructure.Data.Interfaces
{
    public interface IChildRepository
    {
        bool Any();
        int Count();
        Child GetFirst();
        Child Get(Guid childGuid);
        void Add(Child child);
        void Delete(Guid childGuid);
        void Update(Child child);
        IList<Child> GetAll();
        IList<Child> GetAll(Guid userGuid);
    }
}
