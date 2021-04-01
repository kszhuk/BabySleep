using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Common.Exceptions.Child
{
    public class ChildNameLengthException : Exception
    {
        public override string Message
        {
            get
            {
                return "Child name should be less than 50 characters";
            }

        }
    }
}
