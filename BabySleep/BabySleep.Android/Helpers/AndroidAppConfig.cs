using BabySleep.Common.Helpers;
using System;

namespace BabySleep.Droid
{
    public class AndroidAppConfig : AppConfig
    {
        public override string GetConnectionString()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); 
        }
    }
}