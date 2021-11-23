using BabySleep.Common.Enums;
using BabySleep.Common.Exceptions.Sleep;
using BabySleep.Common.Helpers;
using BabySleep.Domain.Models;
using BabySleep.EfData.Interfaces;
using BabySleep.Infrastructure.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BabySleep.Infrastructure.Data.Repositories
{
    public class SleepRepository : ISleepRepository
    {
        private readonly IApplicationContext context;

        public SleepRepository(IApplicationContext context)
        {
            this.context = context;
        }

        public void Add(Sleep sleep)
        {
            ValidateSleepTime(sleep.SleepTime, sleep.SleepGuid, sleep.ChildGuid);

            var efSleep = new EfData.Models.Sleep()
            {
                AwakeningCount = sleep.AwakeningCount,
                ChildGuid = sleep.ChildGuid,
                EndTime = sleep.SleepTime.EndTime,
                FallAsleepTime = (short)sleep.FallAsleepTime,
                FeedingCount = sleep.FeedingCount,
                Note = sleep.CustomerInfo.Note,
                Quality = sleep.CustomerInfo.Quality,
                SleepGuid = Guid.NewGuid(),
                SleepPlace = (short)sleep.SleepPlace,
                StartTime = sleep.SleepTime.StartTime
            };

            context.Sleeps.Add(efSleep);
            context.SaveChanges();
        }

        public void Delete(Guid sleepGuid)
        {
            var sleep = context.Sleeps.FirstOrDefault(s => s.SleepGuid == sleepGuid);

            if (sleep != null)
            {
                context.Sleeps.Remove(sleep);
                context.SaveChanges();
            }
        }

        public Sleep Get(Guid sleepGuid)
        {
            return ConvertToDomain(context.Sleeps.AsNoTracking().FirstOrDefault(s => s.SleepGuid == sleepGuid));
        }

        public IList<Sleep> Take(Guid childGuid, DateTime currentDate)
        {
            var previousDate = FormatEmptyDate(currentDate.AddDays(-1));
            var nextDate = FormatEmptyDate(currentDate.AddDays(2));
            var childSleeps = context.Sleeps.AsNoTracking().Where(s => s.ChildGuid == childGuid &&
                ((s.StartTime >= previousDate && s.StartTime <= nextDate) || (s.EndTime >= previousDate && s.EndTime <= nextDate))).
                OrderBy(c => c.StartTime).ToList();
            return childSleeps.Where(s => AreEqualDates(currentDate, s.StartTime) || AreEqualDates(currentDate, s.EndTime)).
                Select(c => ConvertToDomain(c)).ToList();
        }

        public void Update(Sleep sleep)
        {
            ValidateSleepTime(sleep.SleepTime, sleep.SleepGuid, sleep.ChildGuid);

            var efSleep = context.Sleeps.FirstOrDefault(s => s.SleepGuid == sleep.SleepGuid);

            if (efSleep != null)
            {
                efSleep.AwakeningCount = sleep.AwakeningCount;
                efSleep.EndTime = sleep.SleepTime.EndTime;
                efSleep.FallAsleepTime = (short)sleep.FallAsleepTime;
                efSleep.FeedingCount = sleep.FeedingCount;
                efSleep.Note = sleep.CustomerInfo.Note;
                efSleep.Quality = sleep.CustomerInfo.Quality;
                efSleep.SleepPlace = (short)sleep.SleepPlace;
                efSleep.StartTime = sleep.SleepTime.StartTime;

                context.SaveChanges();
            }
        }

        private Sleep ConvertToDomain(EfData.Models.Sleep sleep)
        {
            if (sleep == null)
            {
                return null;
            }

            return new Sleep(sleep.SleepGuid, sleep.ChildGuid, (SleepPlace)sleep.SleepPlace, sleep.StartTime, sleep.EndTime,
                sleep.FeedingCount, sleep.FallAsleepTime, sleep.AwakeningCount, sleep.Quality, sleep.Note);
        }

        private void ValidateSleepTime(SleepTime sleepTime, Guid sleepGuid, Guid childGuid)
        {
            var startIntersectionSleeps = context.Sleeps.Where(s => s.ChildGuid == childGuid &&
                s.StartTime >= sleepTime.StartTime && s.StartTime <= sleepTime.EndTime && s.SleepGuid != sleepGuid).ToList();
            var endIntersectionSleeps = context.Sleeps.Where(s => s.ChildGuid == childGuid &&
                s.EndTime >= sleepTime.StartTime && s.EndTime <= sleepTime.EndTime && s.SleepGuid != sleepGuid).ToList();
            var wholeIntersectionSleeps = context.Sleeps.Where(s => s.ChildGuid == childGuid &&
                s.StartTime <= sleepTime.StartTime && s.EndTime >= sleepTime.EndTime && s.SleepGuid != sleepGuid).ToList();
            if (startIntersectionSleeps.Any() || endIntersectionSleeps.Any() || wholeIntersectionSleeps.Any())
            {
                throw new SleepAlreadyExistsException();
            }
        }

        private DateTime FormatEmptyDate(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
        }

        private bool AreEqualDates(DateTime date1, DateTime date2)
        {
            return date1.Year == date2.Year && date1.Month == date2.Month && date1.Day == date2.Day;
        }
    }
}
