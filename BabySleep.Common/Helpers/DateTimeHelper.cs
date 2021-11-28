﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Common.Helpers
{
    public static class DateTimeHelper
    {
        public static DateTime FormatEmptyDate(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
        }

        public static bool AreEqualDates(DateTime date1, DateTime date2)
        {
            return date1.Year == date2.Year && date1.Month == date2.Month && date1.Day == date2.Day;
        }
    }
}
