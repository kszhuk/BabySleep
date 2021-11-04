using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Common.Exceptions.Sleep
{
    public class SleepAlreadyExistsException : Exception
    {
        public override string Message
        {
            get
            {
                return "Sleep in this time frame already exists";
            }

        }
    }
}
