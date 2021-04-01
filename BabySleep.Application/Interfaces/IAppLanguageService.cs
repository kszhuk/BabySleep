using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Application.Interfaces
{
    public interface IAppLanguageService
    {
        string GetAppLanguage();
        void SetAppLanguage(string language);
    }
}
