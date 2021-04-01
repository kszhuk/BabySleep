using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Common.Exceptions.Child
{
    public class ChildAgeException : Exception
    {
        public override string Message
        {
            get
            {
                return "Child age should be between 0 and 3 years";
            }

        }
    }
}
