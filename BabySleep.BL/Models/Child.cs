using BabySleep.BL.Resx;
using System;

namespace BabySleep.BL.Models
{
    public class Child
    { 
        public Guid ChildGuid { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public byte[] Picture { get; set; }
        public short? BirthWeek { get; set; }

        public string Age
        {
            get
            {
                var today = DateTime.Today;

                // Calculate the age.
                var years = today.Year - BirthDate.Year;

                // Go back to the year in which the person was born in case of a leap year
                if (BirthDate.Date > today.AddYears(-years))
                {
                    years--;
                }

                var months = today.Month - BirthDate.Month;
                // Full month hasn't completed
                if (today.Day < BirthDate.Day)
                {
                    months--;
                }

                if (months < 0)
                {
                    months += 12;
                }

                if (months == 0)
                {
                    return Resources.Newborn;
                }

                if (years > 0)
                {
                    return string.Format(Resources.AgeYear, years, months);
                }

                return string.Format(Resources.AgeMonth, months);
            }
        }
        public bool IsEmptyPicture { get => Picture == null; }
        public int RealAgeMonths
        {
            get
            {
                var now = DateTime.Now;
                var ageMonths = (now.Month - BirthDate.Month) + 12 * (now.Year - BirthDate.Year);
                if (BirthWeek != null)
                {
                    var realWeeksDiff = 38 - BirthWeek;
                    ageMonths -= (int)realWeeksDiff / 4;
                }

                return ageMonths;
            }
        }
    }
}
