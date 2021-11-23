using BabySleep.Common.Exceptions.Sleep;
using BabySleep.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BabySleep.Tests.Domain
{
    [Collection("SleepValidation")]
    public class SleepValidationTests
    {
        [Theory]
        [ClassData(typeof(SleepTimeDataGenerator))]
        public void SleepDurationExceptionTest(DateTime startTime, DateTime endTime)
        {
            var sleep = new Sleep();
            Assert.Null(sleep.SleepTime);

            sleep.SetSleepTime(startTime, endTime);
            Assert.NotNull(sleep.SleepTime);

            Assert.Equal(startTime, sleep.SleepTime.StartTime);
            Assert.Equal(endTime, sleep.SleepTime.EndTime);

            Assert.Throws<SleepDurationException>(() => sleep.Validate());
        }

        [Fact]
        public void SleepTimeException()
        {
            DateTime startTime = new DateTime(2021, 11, 15, 15, 0, 0); 
            DateTime endTime = new DateTime(2021, 11, 15, 13, 0, 0);

            var sleep = new Sleep();
            sleep.SetSleepTime(startTime, endTime);

            Assert.Throws<SleepTimeException>(() => sleep.Validate());
        }

        public class SleepTimeDataGenerator : TheoryData<DateTime, DateTime>
        {
            public SleepTimeDataGenerator()
            {
                this.Add(new DateTime(2021, 11, 15, 0, 0, 0), new DateTime(2021, 11, 15, 14, 30, 0));
                this.Add(new DateTime(2021, 11, 15, 0, 0, 0), new DateTime(2021, 11, 15, 14, 01, 0));

                this.Add(new DateTime(2021, 11, 15, 1, 20, 15), new DateTime(2021, 11, 15, 16, 30, 0));
                this.Add(new DateTime(2021, 11, 15, 1, 20, 15), new DateTime(2021, 11, 15, 16, 01, 0));

                this.Add(new DateTime(2021, 11, 15, 20, 0, 0), new DateTime(2021, 11, 16, 10, 30, 0));
                this.Add(new DateTime(2021, 11, 15, 20, 0, 0), new DateTime(2021, 11, 16, 10, 01, 0));
            }
        }
    }
}
