using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Common.Helpers
{
    public static class Constants
    {
        public static int NAME_LENGTH = 50;
        public static int MAX_YEARS = 3;
        public static int BIRTH_WEEK_MIN_VALUE = 24;
        public static int BIRTH_WEEK_MAX_VALUE = 37;
        public static int DAYS_SLEEPS_COUNT = 5;
        public static int MAX_SLEEP_DURATION = 14;
        public const string SHORT_TIME_FORMAT = @"hh\:mm";
        public const string QUALITY_FORMAT = "{0}/10";
        public static int NIGHT_SLEEP_START = 19;
        public static int NIGHT_SLEEP_END = 5;
    }
}
