using BabySleep.Common.Enums;
using BabySleep.Common.Exceptions.Child;
using BabySleep.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BabySleep.Tests.Domain
{
    [Collection("SleepCreation")]
    public class SleepCreationTests
    {
        [Fact]
        public void CreateDefaultSleepTest()
        {
            var sleep = new BabySleep.Domain.Models.Sleep();
            Assert.NotEqual(Guid.Empty, sleep.SleepGuid);
            Assert.Equal(Guid.Empty, sleep.ChildGuid);
            Assert.Null(sleep.SleepTime);
            Assert.Null(sleep.CustomerInfo);
            Assert.Equal(SleepPlace.Unknown, sleep.SleepPlace);
            Assert.Equal(0, sleep.AwakeningCount);
            Assert.Equal(0, sleep.FallAsleepTime);
            Assert.Equal(0, sleep.FeedingCount);
        }

        [Theory]
        [ClassData(typeof(SleepCreationDataGenerator))]
        public void CreateParamSleepTest(Guid sleepGuid, Guid childGuid, SleepPlace sleepPlace, DateTime startTime, DateTime endTime,
            short feedingCount, int fallAsleepTime, short awakeningCount, short quality, string note)
        {
            var sleep1 = new Sleep(sleepGuid, childGuid, sleepPlace, startTime, endTime,
                feedingCount, fallAsleepTime, awakeningCount, quality, note);
            Assert.Equal(sleepGuid, sleep1.SleepGuid);
            Assert.Equal(childGuid, sleep1.ChildGuid);
            Assert.Equal(sleepPlace, sleep1.SleepPlace);
            Assert.Equal(startTime, sleep1.SleepTime.StartTime);
            Assert.Equal(endTime, sleep1.SleepTime.EndTime);
            Assert.Equal(feedingCount, sleep1.FeedingCount);
            Assert.Equal(fallAsleepTime, sleep1.FallAsleepTime);
            Assert.Equal(awakeningCount, sleep1.AwakeningCount);
            Assert.Equal(quality, sleep1.CustomerInfo.Quality);
            Assert.Equal(note, sleep1.CustomerInfo.Note);

            var sleep2 = new Sleep(sleepGuid, childGuid, sleepPlace, startTime, endTime);
            Assert.Equal(sleepGuid, sleep2.SleepGuid);
            Assert.Equal(childGuid, sleep2.ChildGuid);
            Assert.Equal(sleepPlace, sleep2.SleepPlace);
            Assert.Equal(startTime, sleep2.SleepTime.StartTime);
            Assert.Equal(endTime, sleep2.SleepTime.EndTime);
            Assert.Equal(0, sleep2.FeedingCount);
            Assert.Equal(0, sleep2.FallAsleepTime);
            Assert.Equal(0, sleep2.AwakeningCount);
            Assert.Equal(0, sleep2.CustomerInfo.Quality);
            Assert.Equal(string.Empty, sleep2.CustomerInfo.Note);

            var sleep3 = new Sleep(sleepGuid, childGuid, sleepPlace, startTime, endTime,
                feedingCount, fallAsleepTime, awakeningCount);
            Assert.Equal(sleepGuid, sleep3.SleepGuid);
            Assert.Equal(childGuid, sleep3.ChildGuid);
            Assert.Equal(sleepPlace, sleep3.SleepPlace);
            Assert.Equal(startTime, sleep3.SleepTime.StartTime);
            Assert.Equal(endTime, sleep3.SleepTime.EndTime);
            Assert.Equal(feedingCount, sleep3.FeedingCount);
            Assert.Equal(fallAsleepTime, sleep3.FallAsleepTime);
            Assert.Equal(awakeningCount, sleep3.AwakeningCount);
            Assert.Equal(0, sleep3.CustomerInfo.Quality);
            Assert.Equal(string.Empty, sleep3.CustomerInfo.Note);
        }

        [Theory]
        [ClassData(typeof(SleepCreationDataGenerator))]
        public void SetCustomerInfoTest(Guid sleepGuid, Guid childGuid, SleepPlace sleepPlace, DateTime startTime, DateTime endTime,
            short feedingCount, int fallAsleepTime, short awakeningCount, short quality, string note)
        {
            var sleep = new Sleep();
            Assert.Null(sleep.CustomerInfo);

            sleep.SetCustomerInfo(quality, note);
            Assert.NotNull(sleep.CustomerInfo);

            Assert.Equal(quality, sleep.CustomerInfo.Quality);
            Assert.Equal(note, sleep.CustomerInfo.Note);
        }

        [Theory]
        [ClassData(typeof(SleepTimeDataGenerator))]
        public void SetSleepTimeTest(DateTime startTime, DateTime endTime, SleepType sleepType)
        {
            var sleep = new Sleep();
            Assert.Null(sleep.SleepTime);

            sleep.SetSleepTime(startTime, endTime);
            Assert.NotNull(sleep.SleepTime);

            Assert.Equal(startTime, sleep.SleepTime.StartTime);
            Assert.Equal(endTime, sleep.SleepTime.EndTime);
            Assert.Equal(endTime - startTime, sleep.SleepTime.Duration);
            Assert.Equal(sleepType, sleep.SleepTime.SleepType);
        }

        public class SleepCreationDataGenerator : TheoryData<Guid, Guid, SleepPlace, DateTime, DateTime,
            short, int, short, short, string>
        {
            public SleepCreationDataGenerator()
            {
                //1 sleep
                var sleepGuid1 = Guid.NewGuid();
                var childGuid1 = Guid.NewGuid();
                var sleepPlace1 = SleepPlace.BabyStroller;
                var startTime1 = new DateTime(2021, 11, 15, 13, 0, 0);
                var endTime1 = new DateTime(2021, 11, 15, 15, 0, 0);
                short quality1 = 10;
                var note1 = "Test1";
                short feedingCount1 = 0;
                int fallAsleepTime1 = 0;
                short awakeningCount1 = 0;

                this.Add(sleepGuid1, childGuid1, sleepPlace1, startTime1, endTime1, feedingCount1, fallAsleepTime1, awakeningCount1, quality1, note1);

                //2 sleep
                var sleepGuid2 = Guid.NewGuid();
                var childGuid2 = Guid.NewGuid();
                var sleepPlace2 = SleepPlace.Car;
                var startTime2 = new DateTime(2021, 11, 15, 11, 0, 0);
                var endTime2 = new DateTime(2021, 11, 15, 12, 30, 0);
                short quality2 = 8;
                var note2 = "Test2";
                short feedingCount2 = 1;
                int fallAsleepTime2 = 5;
                short awakeningCount2 = 1;
                this.Add(sleepGuid2, childGuid2, sleepPlace2, startTime2, endTime2, feedingCount2, fallAsleepTime2, awakeningCount2, quality2, note2);

                //3 sleep
                var sleepGuid3 = Guid.NewGuid();
                var childGuid3 = Guid.NewGuid();
                var sleepPlace3 = SleepPlace.Crib;
                var startTime3 = new DateTime(2021, 11, 15, 16, 0, 0);
                var endTime3 = new DateTime(2021, 11, 15, 17, 30, 0);
                short quality3 = 5;
                var note3 = "Test3";
                short feedingCount3 = 2;
                int fallAsleepTime3 = 15;
                short awakeningCount3 = 2;
                this.Add(sleepGuid3, childGuid3, sleepPlace3, startTime3, endTime3, feedingCount3, fallAsleepTime3, awakeningCount3, quality3, note3);
            }
        }

        public class SleepTimeDataGenerator : TheoryData<DateTime, DateTime, SleepType>
        {
            public SleepTimeDataGenerator()
            {
                this.Add(new DateTime(2021, 11, 15, 13, 0, 0), new DateTime(2021, 11, 15, 15, 30, 0), SleepType.DaySleep);
                this.Add(new DateTime(2021, 11, 15, 11, 10, 27), new DateTime(2021, 11, 15, 13, 36, 54), SleepType.DaySleep);

                this.Add(new DateTime(2021, 11, 15, 19, 0, 0), new DateTime(2021, 11, 15, 21, 30, 0), SleepType.NightSleep);
                this.Add(new DateTime(2021, 11, 15, 19, 0, 0), new DateTime(2021, 11, 15, 3, 30, 0), SleepType.NightSleep);
                this.Add(new DateTime(2021, 11, 15, 19, 0, 0), new DateTime(2021, 11, 15, 7, 30, 0), SleepType.NightSleep);

                this.Add(new DateTime(2021, 11, 15, 21, 10, 0), new DateTime(2021, 11, 15, 21, 30, 0), SleepType.NightSleep);
                this.Add(new DateTime(2021, 11, 15, 21, 15, 0), new DateTime(2021, 11, 15, 3, 30, 0), SleepType.NightSleep);
                this.Add(new DateTime(2021, 11, 15, 21, 40, 0), new DateTime(2021, 11, 15, 7, 30, 0), SleepType.NightSleep);

                this.Add(new DateTime(2021, 11, 15, 1, 15, 0), new DateTime(2021, 11, 15, 3, 30, 0), SleepType.NightSleep);
            }
        }
    }
}
