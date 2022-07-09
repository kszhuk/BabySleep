using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BabySleep.Validations
{
    public class PasswordRule<T> : IValidationRule<T>
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

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniMaxChars = new Regex(@".{8,15}");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            return hasNumber.IsMatch(str) && hasUpperChar.IsMatch(str) && hasMiniMaxChars.IsMatch(str) &&
                hasLowerChar.IsMatch(str) && hasSymbols.IsMatch(str);
        }
    }
}
