using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Validations
{
    public class EmailRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (value == null)
            {
                return false;
            }

            var str = value.ToString();

            if(string.IsNullOrWhiteSpace(str))
            {
                return false;
            }

            try
            {
                var email = new System.Net.Mail.MailAddress(str);
            }
            catch (FormatException)
            {
                return false;
            }

            return true;
        }
    }
}
