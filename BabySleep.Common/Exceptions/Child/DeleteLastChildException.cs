using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Common.Exceptions.Child
{
    public class DeleteLastChildException : Exception
    {
        public override string Message
        {
            get
            {
                return "Last child couldn't be deleted";
            }

        }
    }
}
