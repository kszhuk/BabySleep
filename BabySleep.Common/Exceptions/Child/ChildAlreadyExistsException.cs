using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Common.Exceptions.Child
{
    public class ChildAlreadyExistsException : Exception
    {
        public override string Message
        {
            get
            {
                return "Child with such name already exists";
            }

        }
    }
}
