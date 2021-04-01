using BabySleep.Common.Exceptions.Child;
using BabySleep.Common.Helpers;
using BabySleep.Domain.Resx;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Domain.Models
{
    public class Child
    {
        public Child()
        {
            ChildGuid = Guid.Empty;
        }

        public Child(Guid childGuid, DateTime birthDate, short? birthWeek,
            string name, byte[] picture)
        {
            ChildGuid = childGuid;
            Name = name.Trim();
            BirthDate = birthDate;
            Picture = picture;
            BirthWeek = birthWeek;
        }

        public bool Validate()
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new ChildNameEmptyException();
            }

            if (Name.Length > Constants.NAME_LENGTH)
            {
                throw new ChildNameLengthException();
            }

            if (BirthDate > DateTime.Now || BirthDate < DateTime.Now.AddYears(-Constants.MAX_YEARS))
            {
                throw new ChildAgeException();
            }

            if (BirthWeek != null && (BirthWeek.Value < Constants.BIRTH_WEEK_MIN_VALUE || BirthWeek.Value > Constants.BIRTH_WEEK_MAX_VALUE))
            {
                throw new ChildPrematureBirthWeekException();
            }

            return true;
        }

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
