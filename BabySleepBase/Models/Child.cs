using BabySleepBase.Resx;
using SQLite;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabySleepBase.Models
{
    public class Child
    {
        [PrimaryKey]
        public Guid ChildGuid { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public byte[] Picture { get; set; }
        public short? BirthWeek { get; set; }

        [Ignore]
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

        [Ignore]
        public bool IsEmptyPicture { get => Picture == null; }
    }
}
