using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Infrastructure.Data.Interfaces
{
    public interface ICommonRepository
    {
        string GetAppLanguage();
        void UpdateAppLanguage(string language);
    }
}
