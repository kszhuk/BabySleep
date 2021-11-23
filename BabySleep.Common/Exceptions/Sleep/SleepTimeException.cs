using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Common.Exceptions.Sleep
{
    public class SleepTimeException : Exception
    {
        public override string Message
        {
            get
            {
                return "Sleep's end time should be more than start time";
            }

        }
    }
}
