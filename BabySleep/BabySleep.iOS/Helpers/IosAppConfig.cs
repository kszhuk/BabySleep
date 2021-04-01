using BabySleep.Common.Helpers;
using System;
using System.IO;

namespace BabySleep.iOS
{
    public class IosAppConfig : AppConfig
    {
        public override string GetConnectionString()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "..", "Library");
        }
    }
}