using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Common.Exceptions.Child
{
    public class ChildNameEmptyException : Exception
    {
        public override string Message
        {
            get
            {
                return "Child name is required";
            }

        }
    }
}
