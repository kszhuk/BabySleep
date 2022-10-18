using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Infrastructure.Data.Interfaces
{
    public interface IUserRepository
    {
        string GetUserGuid(string email);
    }
}
