using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Validations
{
    /// <summary>
    /// Validates birth week  for premature born children (between 24 and 38)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BirthWeekRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (value == null)
            {
                return false;
            }

            if (!Int32.TryParse(value.ToString(), out int birthWeek))
                return false;

            return birthWeek > Common.Helpers.Constants.BIRTH_WEEK_MIN_VALUE && birthWeek < Common.Helpers.Constants.BIRTH_WEEK_MAX_VALUE;
        }
    }
}
