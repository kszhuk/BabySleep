using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Common.Exceptions.Sleep
{
    public class SleepDurationException : Exception
    {
        public override string Message
        {
            get
            {
                return "Sleep should last less than 14 hours";
            }

        }
    }
}
