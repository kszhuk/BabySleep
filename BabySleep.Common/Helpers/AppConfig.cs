using BabySleep.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Common.Helpers
{
    public abstract class AppConfig : IAppConfig
    {
        public abstract string GetConnectionString();
    }
}
