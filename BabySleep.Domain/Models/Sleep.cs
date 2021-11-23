using BabySleep.Common.Enums;
using BabySleep.Common.Exceptions.Sleep;
using BabySleep.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Domain.Models
{
    public class Sleep
    {
        public Guid SleepGuid { get; private set; }
        public Guid ChildGuid { get; private set; }
        public SleepPlace SleepPlace { get; private set; }
        public short FeedingCount { get; private set; }
        public int FallAsleepTime { get; private set; }//minutes to fall asleep at bedtime
        public short AwakeningCount { get; private set; }
        public CustomerInfo CustomerInfo { get; private set; }
        public SleepTime SleepTime { get; private set; }

        public Sleep()
        {
            SleepGuid = Guid.NewGuid();
        }

        public Sleep(Guid sleepGuid, Guid childGuid, SleepPlace sleepPlace, DateTime startTime, DateTime endTime, 
            short feedingCount, int fallAsleepTime, short awakeningCount, short quality, string note)
        {
            SleepGuid = sleepGuid;
            ChildGuid = childGuid;
            SleepPlace = sleepPlace;
            FeedingCount = feedingCount;
            FallAsleepTime = fallAsleepTime;
            AwakeningCount = awakeningCount;

            SetSleepTime(startTime, endTime);
            SetCustomerInfo(quality, note);
        }

        public Sleep(Guid sleepGuid, Guid childGuid, DateTime startTime, DateTime endTime) : 
            this (sleepGuid, childGuid, SleepPlace.Unknown, startTime, endTime, 0, 0, 0, 0, string.Empty)
        {
        }

        public Sleep(Guid sleepGuid, Guid childGuid, SleepPlace sleepPlace, DateTime startTime, DateTime endTime,
            short feedingCount = 0, int fallAsleepTime = 0, short awakeningCount = 0) :
            this(sleepGuid, childGuid, sleepPlace, startTime, endTime, feedingCount, fallAsleepTime, awakeningCount, 0, string.Empty)
        {
        }

        public void SetSleepTime(DateTime startTime, DateTime endTime)
        {
            SleepTime = new SleepTime(startTime, endTime);
        }

        public void SetCustomerInfo(short quality, string note)
        {
            CustomerInfo = new CustomerInfo(quality, note);
        }

        public bool Validate()
        {
            var duration = (SleepTime.EndTime - SleepTime.StartTime).TotalHours;
            if (duration > Constants.MAX_SLEEP_DURATION)
            {
                throw new SleepDurationException();
            }

            return true;
        }
    }
}
