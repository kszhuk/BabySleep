using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Common.Exceptions.Child
{
    public class ChildPrematureBirthWeekException : Exception
    {
        public override string Message
        {
            get
            {
                return "Birth week for premature born should be between 24 and 37 weeks";
            }

        }
    }
}
