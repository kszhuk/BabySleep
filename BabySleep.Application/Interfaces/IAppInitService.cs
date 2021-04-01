using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Application.Interfaces
{
    public interface IAppInitService
    {
        string GetAppLanguage();
        Guid GetFirstChild();
        bool AnyChildExist();
    }
}
