using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Application.Interfaces
{
    public interface IUserService
    {
        string GetUserGuid(string email);
    }
}
