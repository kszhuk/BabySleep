using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Common.Exceptions.Authentication
{
    public class AuthUserCollisionException : Exception
    {
        public override string Message
        {
            get
            {
                return "The email address is already in use by another account";
            }
        }
    }
}
