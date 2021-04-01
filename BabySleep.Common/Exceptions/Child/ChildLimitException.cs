using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Common.Exceptions.Child
{
    public class ChildLimitException : Exception
    {
        public override string Message
        {
            get
            {
                return "You've reached limit of children";
            }

        }
    }
}
