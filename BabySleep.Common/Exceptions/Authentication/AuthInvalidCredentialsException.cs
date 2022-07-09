using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Common.Exceptions.Authentication
{
    public class AuthInvalidUserException : Exception
    {
        public override string Message
        {
            get
            {
                return "Invalid user";
            }

        }
    }
}
