using BabySleep.Common.Enums;
using BabySleep.Common.Exceptions.Child;
using BabySleep.Common.Exceptions.Sleep;
using BabySleep.Domain.Models;
using BabySleep.Infrastructure.Data.Repositories;
using BabySleep.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BabySleep.Tests.Infrastructure.Data
{
    [Collection("Non-Parallel Collection")]
    public class SleepRepositoryTests
    {
        [Theory]
        [ClassData(typeof(GetSleepsGenerator))]
        public void AddTest(Sleep sleep)
        {
            using (var context = new ApplicationContextFactory().CreateContext())
            {
                Assert.Equal(0, context.Children.Count());

                var repository = new SleepRepository(context);
                repository.Add(sleep);
                Assert.Equal(1, context.Sleeps.Count());

                var contextSleep = context.Sleeps.First();
                Assert.Equal(sleep.AwakeningCount, contextSleep.AwakeningCount);
                Assert.Equal(sleep.ChildGuid, contextSleep.ChildGuid);
                Assert.Equal(sleep.CustomerInfo.Note, contextSleep.Note);
                Assert.Equal(sleep.CustomerInfo.Quality, contextSleep.Quality);
                Assert.Equal(sleep.FallAsleepTime, contextSleep.FallAsleepTime);
                Assert.Equal(sleep.FeedingCount, contextSleep.FeedingCount);
                Assert.Equal(sleep.SleepPlace, (SleepPlace)contextSleep.SleepPlace);
                Assert.Equal(sleep.SleepTime.EndTime, contextSleep.EndTime);
                Assert.Equal(sleep.SleepTime.StartTime, contextSleep.StartTime);
                Assert.NotEqual(contextSleep.SleepGuid, Guid.Empty);
            }
        }

        [Fact]
        public void DeleteTest()
        {
            using (var context = new ApplicationContextFactory().CreateContext())
            {
                var repository = new SleepRepository(context);

                var generator = new GetSleepsGenerator();
                var sleeps = generator.GetTestSleeps();

                Assert.Equal(0, context.Sleeps.Count());

                for (int i = 0; i < sleeps.Count; i++)
                {
                    repository.Add(sleeps[i]);
                }
                Assert.Equal(sleeps.Count, context.Sleeps.Count());

                var contextSleeps = context.Sleeps.ToList();
                for (int i = contextSleeps.Count - 1; i >= 0; i--)
                {
                    Assert.NotNull(repository.Get(contextSleeps[i].SleepGuid));
                    repository.Delete(contextSleeps[i].SleepGuid);
                    Assert.Null(repository.Get(contextSleeps[i].SleepGuid));
                }

                Assert.Equal(0, context.Sleeps.Count());
            }
        }

        [Fact]
        public void DeleteWrongSleepTest()
        {
            using (var context = new ApplicationContextFactory().CreateContext())
            {
                var repository = new SleepRepository(context);

                var generator = new GetSleepsGenerator();
                var sleeps = generator.GetTestSleeps();

                repository.Add(sleeps[0]);
                repository.Add(sleeps[1]);
                Assert.Equal(2, context.Sleeps.Count());

                repository.Delete(Guid.NewGuid());
                Assert.Equal(2, context.Sleeps.Count());
            }
        }

        [Fact]
        public void GetTest()
        {
            using (var context = new ApplicationContextFactory().CreateContext())
            {
                var repository = new SleepRepository(context);

                var generator = new GetSleepsGenerator();
                var sleeps = generator.GetTestSleeps();

                for (int i = 0; i < sleeps.Count; i++)
                {
                    repository.Add(sleeps[i]);
                }

                var contextSleeps = context.Sleeps.ToList();

                foreach (var contextSleep in contextSleeps)
                {
                    var sleep = repository.Get(contextSleep.SleepGuid);
                    Assert.Equal(sleep.AwakeningCount, contextSleep.AwakeningCount);
                    Assert.Equal(sleep.ChildGuid, contextSleep.ChildGuid);
                    Assert.Equal(sleep.CustomerInfo.Note, contextSleep.Note);
                    Assert.Equal(sleep.CustomerInfo.Quality, contextSleep.Quality);
                    Assert.Equal(sleep.FallAsleepTime, contextSleep.FallAsleepTime);
                    Assert.Equal(sleep.FeedingCount, contextSleep.FeedingCount);
                    Assert.Equal(sleep.SleepPlace, (SleepPlace)contextSleep.SleepPlace);
                    Assert.Equal(sleep.SleepTime.EndTime, contextSleep.EndTime);
                    Assert.Equal(sleep.SleepTime.StartTime, contextSleep.StartTime);
                }
            }
        }

        [Fact]
        public void GetNullTest()
        {
            using (var context = new ApplicationContextFactory().CreateContext())
            {
                var repository = new SleepRepository(context);

                var generator = new GetSleepsGenerator();
                var sleeps = generator.GetTestSleeps();

                for (int i = 0; i < sleeps.Count; i++)
                {
                    repository.Add(sleeps[i]);
                }

                var sleep = repository.Get(Guid.NewGuid());
                Assert.Null(sleep);
            }
        }

        [Fact]
        public void TakeTest()
        {
            using (var context = new ApplicationContextFactory().CreateContext())
            {
                var repository = new SleepRepository(context);

                var generator = new GetSleepsGenerator();
                var sleeps = generator.GetTestSleeps();

                for (int i = 0; i < sleeps.Count; i++)
                {
                    repository.Add(sleeps[i]);
                }

                Assert.Equal(sleeps.Count, context.Sleeps.ToList().Count);

                var childGuid = Guid.NewGuid();
                var sleepDate = DateTime.Now;
                var sleepDatePrevious = DateTime.Now.AddDays(-1);
                var sleepDateNext = DateTime.Now.AddDays(1);
                var sleepDateOld = DateTime.Now.AddMonths(-1);
                var sleepDateFuture = DateTime.Now.AddMonths(1);

                //Day sleep
                var sleepGuidCurrentDate = Guid.NewGuid();
                var startTime1 = new DateTime(sleepDate.Year, sleepDate.Month, sleepDate.Day, 13, 0, 0);
                var endTime1 = new DateTime(sleepDate.Year, sleepDate.Month, sleepDate.Day, 15, 30, 0);
                short quality1 = 1;
                var note1 = "Test Day 1";
                short feedingCount1 = 1;
                int fallAsleepTime1 = 1;
                short awakeningCount1 = 1;
                var sleepPlace1 = SleepPlace.Car;
                repository.Add(SleepEntryHelper.FillChildSleep(sleepGuidCurrentDate, childGuid, startTime1, endTime1,
                        sleepPlace1, quality1, note1, feedingCount1, awakeningCount1, fallAsleepTime1));

                var sleepPlace = SleepPlace.Crib;
                short quality = 5;
                var note = "Test Day";
                short feedingCount = 2;
                int fallAsleepTime = 16;
                short awakeningCount = 2;

                //Previous day night sleep
                var sleepGuidPreviousNight = Guid.NewGuid();
                var startTime2 = new DateTime(sleepDatePrevious.Year, sleepDatePrevious.Month, sleepDatePrevious.Day, 21, 0, 0);
                var endTime2 = new DateTime(sleepDate.Year, sleepDate.Month, sleepDate.Day, 06, 30, 0);
                repository.Add(SleepEntryHelper.FillChildSleep(sleepGuidPreviousNight, childGuid, startTime2, endTime2,
                        sleepPlace, quality, note, feedingCount, awakeningCount, fallAsleepTime));

                //Previous day daily sleep
                var sleepGuid3 = Guid.NewGuid();
                var startTime3 = new DateTime(sleepDatePrevious.Year, sleepDatePrevious.Month, sleepDatePrevious.Day, 13, 0, 0);
                var endTime3 = new DateTime(sleepDatePrevious.Year, sleepDatePrevious.Month, sleepDatePrevious.Day, 15, 0, 0);
                repository.Add(SleepEntryHelper.FillChildSleep(sleepGuid3, childGuid, startTime3, endTime3,
                        sleepPlace, quality, note, feedingCount, awakeningCount, fallAsleepTime));

                //Next day night sleep
                var sleepGuidNextNight = Guid.NewGuid();
                var startTime4 = new DateTime(sleepDate.Year, sleepDate.Month, sleepDate.Day, 21, 0, 0);
                var endTime4 = new DateTime(sleepDateNext.Year, sleepDateNext.Month, sleepDateNext.Day, 06, 30, 0);
                repository.Add(SleepEntryHelper.FillChildSleep(sleepGuidNextNight, childGuid, startTime4, endTime4,
                        sleepPlace, quality, note, feedingCount, awakeningCount, fallAsleepTime));

                //Next day daily sleep
                var sleepGuid5 = Guid.NewGuid();
                var startTime5 = new DateTime(sleepDateNext.Year, sleepDateNext.Month, sleepDateNext.Day, 13, 0, 0);
                var endTime5 = new DateTime(sleepDateNext.Year, sleepDateNext.Month, sleepDateNext.Day, 15, 0, 0);
                repository.Add(SleepEntryHelper.FillChildSleep(sleepGuid5, childGuid, startTime5, endTime5,
                        sleepPlace, quality, note, feedingCount, awakeningCount, fallAsleepTime));

                //Old sleep
                var sleepGuid6 = Guid.NewGuid();
                var startTime6 = new DateTime(sleepDateOld.Year, sleepDateOld.Month, sleepDateOld.Day, 13, 0, 0);
                var endTime6 = new DateTime(sleepDateOld.Year, sleepDateOld.Month, sleepDateOld.Day, 15, 0, 0);
                repository.Add(SleepEntryHelper.FillChildSleep(sleepGuid6, childGuid, startTime6, endTime6,
                        sleepPlace, quality, note, feedingCount, awakeningCount, fallAsleepTime));

                //Future sleep
                var sleepGuid7 = Guid.NewGuid();
                var startTime7 = new DateTime(sleepDateFuture.Year, sleepDateFuture.Month, sleepDateFuture.Day, 13, 0, 0);
                var endTime7 = new DateTime(sleepDateFuture.Year, sleepDateFuture.Month, sleepDateFuture.Day, 15, 0, 0);
                repository.Add(SleepEntryHelper.FillChildSleep(sleepGuid7, childGuid, startTime7, endTime7,
                        sleepPlace, quality, note, feedingCount, awakeningCount, fallAsleepTime));

                Assert.Equal(sleeps.Count + 7, context.Sleeps.ToList().Count);

                //verify old date
                var oldSleeps = repository.Take(childGuid, sleepDateOld);
                Assert.Equal(1, oldSleeps.Count);
                Assert.Equal(startTime6, oldSleeps.First().SleepTime.StartTime);
                Assert.Equal(endTime6, oldSleeps.First().SleepTime.EndTime);

                //verify future date
                var futureSleeps = repository.Take(childGuid, sleepDateFuture);
                Assert.Equal(1, futureSleeps.Count);
                Assert.Equal(startTime7, futureSleeps.First().SleepTime.StartTime);
                Assert.Equal(endTime7, futureSleeps.First().SleepTime.EndTime);

                //verify current date sleep
                var currentSleeps = repository.Take(childGuid, sleepDate);
                Assert.Equal(3, currentSleeps.Count);
                var currentDateSleep = currentSleeps.FirstOrDefault(s => s.SleepTime.StartTime == startTime1 && s.SleepTime.EndTime == endTime1);
                Assert.NotNull(currentDateSleep);
                Assert.Equal(awakeningCount1, currentDateSleep.AwakeningCount);
                Assert.Equal(childGuid, currentDateSleep.ChildGuid);
                Assert.Equal(note1, currentDateSleep.CustomerInfo.Note);
                Assert.Equal(quality1, currentDateSleep.CustomerInfo.Quality);
                Assert.Equal(fallAsleepTime1, currentDateSleep.FallAsleepTime);
                Assert.Equal(feedingCount1, currentDateSleep.FeedingCount);
                Assert.Equal(sleepPlace1, (SleepPlace)currentDateSleep.SleepPlace);

                //verify previous date sleep
                var previousSleeps = repository.Take(childGuid, sleepDatePrevious);
                Assert.Equal(2, previousSleeps.Count);

                //verify next date sleep
                var nextSleeps = repository.Take(childGuid, sleepDateNext);
                Assert.Equal(2, nextSleeps.Count);

                //verify not existing date
                var nullSleeps = repository.Take(childGuid, DateTime.Now.AddYears(1));
                Assert.Equal(0, nullSleeps.Count);

                //verify not existing child
                var nullSleepsChild = repository.Take(Guid.NewGuid(), sleepDate);
                Assert.Equal(0, nullSleepsChild.Count);
            }
        }

        [Fact]
        public void TakeTestStatistics()
        {
            using (var context = new ApplicationContextFactory().CreateContext())
            {
                var repository = new SleepRepository(context);

                var generator = new GetSleepsGenerator();
                var sleeps = generator.GetTestSleeps();

                for (int i = 0; i < sleeps.Count; i++)
                {
                    repository.Add(sleeps[i]);
                }

                Assert.Equal(sleeps.Count, context.Sleeps.ToList().Count);

                var childGuid = Guid.NewGuid();
                var sleepDate = DateTime.Now;
                var sleepDatePrevious = DateTime.Now.AddDays(-1);
                var sleepDateNext = DateTime.Now.AddDays(1);
                var sleepDateOld = DateTime.Now.AddMonths(-1);
                var sleepDateFuture = DateTime.Now.AddMonths(1);

                //Day sleep
                var sleepGuidCurrentDate = Guid.NewGuid();
                var startTime1 = new DateTime(sleepDate.Year, sleepDate.Month, sleepDate.Day, 13, 0, 0);
                var endTime1 = new DateTime(sleepDate.Year, sleepDate.Month, sleepDate.Day, 15, 30, 0);
                short quality1 = 1;
                var note1 = "Test Day 1";
                short feedingCount1 = 1;
                int fallAsleepTime1 = 1;
                short awakeningCount1 = 1;
                var sleepPlace1 = SleepPlace.Car;
                repository.Add(SleepEntryHelper.FillChildSleep(sleepGuidCurrentDate, childGuid, startTime1, endTime1,
                        sleepPlace1, quality1, note1, feedingCount1, awakeningCount1, fallAsleepTime1));

                var sleepPlace = SleepPlace.Crib;
                short quality = 5;
                var note = "Test Day";
                short feedingCount = 2;
                int fallAsleepTime = 16;
                short awakeningCount = 2;

                //Previous day night sleep
                var sleepGuidPreviousNight = Guid.NewGuid();
                var startTime2 = new DateTime(sleepDatePrevious.Year, sleepDatePrevious.Month, sleepDatePrevious.Day, 21, 0, 0);
                var endTime2 = new DateTime(sleepDate.Year, sleepDate.Month, sleepDate.Day, 06, 30, 0);
                repository.Add(SleepEntryHelper.FillChildSleep(sleepGuidPreviousNight, childGuid, startTime2, endTime2,
                        sleepPlace, quality, note, feedingCount, awakeningCount, fallAsleepTime));

                //Previous day daily sleep
                var sleepGuid3 = Guid.NewGuid();
                var startTime3 = new DateTime(sleepDatePrevious.Year, sleepDatePrevious.Month, sleepDatePrevious.Day, 13, 0, 0);
                var endTime3 = new DateTime(sleepDatePrevious.Year, sleepDatePrevious.Month, sleepDatePrevious.Day, 15, 0, 0);
                repository.Add(SleepEntryHelper.FillChildSleep(sleepGuid3, childGuid, startTime3, endTime3,
                        sleepPlace, quality, note, feedingCount, awakeningCount, fallAsleepTime));

                //Next day night sleep
                var sleepGuidNextNight = Guid.NewGuid();
                var startTime4 = new DateTime(sleepDate.Year, sleepDate.Month, sleepDate.Day, 21, 0, 0);
                var endTime4 = new DateTime(sleepDateNext.Year, sleepDateNext.Month, sleepDateNext.Day, 06, 30, 0);
                repository.Add(SleepEntryHelper.FillChildSleep(sleepGuidNextNight, childGuid, startTime4, endTime4,
                        sleepPlace, quality, note, feedingCount, awakeningCount, fallAsleepTime));

                //Next day daily sleep
                var sleepGuid5 = Guid.NewGuid();
                var startTime5 = new DateTime(sleepDateNext.Year, sleepDateNext.Month, sleepDateNext.Day, 13, 0, 0);
                var endTime5 = new DateTime(sleepDateNext.Year, sleepDateNext.Month, sleepDateNext.Day, 15, 0, 0);
                repository.Add(SleepEntryHelper.FillChildSleep(sleepGuid5, childGuid, startTime5, endTime5,
                        sleepPlace, quality, note, feedingCount, awakeningCount, fallAsleepTime));

                //Old sleep
                var sleepGuid6 = Guid.NewGuid();
                var startTime6 = new DateTime(sleepDateOld.Year, sleepDateOld.Month, sleepDateOld.Day, 13, 0, 0);
                var endTime6 = new DateTime(sleepDateOld.Year, sleepDateOld.Month, sleepDateOld.Day, 15, 0, 0);
                repository.Add(SleepEntryHelper.FillChildSleep(sleepGuid6, childGuid, startTime6, endTime6,
                        sleepPlace, quality, note, feedingCount, awakeningCount, fallAsleepTime));

                //Future sleep
                var sleepGuid7 = Guid.NewGuid();
                var startTime7 = new DateTime(sleepDateFuture.Year, sleepDateFuture.Month, sleepDateFuture.Day, 13, 0, 0);
                var endTime7 = new DateTime(sleepDateFuture.Year, sleepDateFuture.Month, sleepDateFuture.Day, 15, 0, 0);
                repository.Add(SleepEntryHelper.FillChildSleep(sleepGuid7, childGuid, startTime7, endTime7,
                        sleepPlace, quality, note, feedingCount, awakeningCount, fallAsleepTime));

                Assert.Equal(sleeps.Count + 7, context.Sleeps.ToList().Count);

                //verify old date
                var oldSleeps = repository.Take(childGuid, sleepDateOld.AddDays(-1), sleepDateOld.AddDays(1));
                Assert.Equal(1, oldSleeps.Count);
                Assert.Equal(startTime6, oldSleeps.First().SleepTime.StartTime);
                Assert.Equal(endTime6, oldSleeps.First().SleepTime.EndTime);

                //verify future date
                var futureSleeps = repository.Take(childGuid, sleepDateFuture.AddDays(-1), sleepDateFuture.AddDays(1));
                Assert.Equal(1, futureSleeps.Count);
                Assert.Equal(startTime7, futureSleeps.First().SleepTime.StartTime);
                Assert.Equal(endTime7, futureSleeps.First().SleepTime.EndTime);

                //verify current date sleep
                var currentSleeps = repository.Take(childGuid, sleepDate, sleepDate);
                Assert.Equal(5, currentSleeps.Count);
                var currentDateSleep = currentSleeps.FirstOrDefault(s => s.SleepTime.StartTime == startTime1 && s.SleepTime.EndTime == endTime1);
                Assert.NotNull(currentDateSleep);
                Assert.Equal(awakeningCount1, currentDateSleep.AwakeningCount);
                Assert.Equal(childGuid, currentDateSleep.ChildGuid);
                Assert.Equal(note1, currentDateSleep.CustomerInfo.Note);
                Assert.Equal(quality1, currentDateSleep.CustomerInfo.Quality);
                Assert.Equal(fallAsleepTime1, currentDateSleep.FallAsleepTime);
                Assert.Equal(feedingCount1, currentDateSleep.FeedingCount);
                Assert.Equal(sleepPlace1, (SleepPlace)currentDateSleep.SleepPlace);

                //verify previous date sleep
                var previousSleeps = repository.Take(childGuid, sleepDatePrevious.AddDays(-1), sleepDatePrevious);
                Assert.Equal(4, previousSleeps.Count);

                //verify next date sleep
                var nextSleeps = repository.Take(childGuid, sleepDateNext, sleepDateNext.AddDays(1));
                Assert.Equal(4, nextSleeps.Count);

                //verify sleeps for 3 days
                var sleepsSome = repository.Take(childGuid, sleepDatePrevious, sleepDateNext);
                Assert.Equal(5, sleepsSome.Count);

                //verify not existing date
                var nullSleeps = repository.Take(childGuid, DateTime.Now.AddYears(1), DateTime.Now.AddYears(2));
                Assert.Equal(0, nullSleeps.Count);

                //verify not existing child
                var nullSleepsChild = repository.Take(Guid.NewGuid(), sleepDate, sleepDate.AddDays(1));
                Assert.Equal(0, nullSleepsChild.Count);
            }
        }

        [Fact]
        public void UpdateTest()
        {
            using (var context = new ApplicationContextFactory().CreateContext())
            {
                var repository = new SleepRepository(context);

                var generator = new GetSleepsGenerator();
                var sleeps = generator.GetTestSleeps();

                for (int i = 0; i < sleeps.Count; i++)
                {
                    repository.Add(sleeps[i]);
                }

                var firstSleep = context.Sleeps.First();
                var sleepGuid = firstSleep.SleepGuid;
                var childGuid = firstSleep.ChildGuid;
                var sleepPlace = SleepPlace.Car;
                var startTime = firstSleep.StartTime.AddDays(1);
                var endTime = firstSleep.EndTime.AddDays(1);
                short quality = 6;
                var note = "Test update";
                short feedingCount = 8;
                int fallAsleepTime = 13;
                short awakeningCount = 7;

                var sleep = SleepEntryHelper.FillChildSleep(sleepGuid, childGuid, startTime, endTime,
                        sleepPlace, quality, note, feedingCount, awakeningCount, fallAsleepTime);
                repository.Update(sleep);

                var contextSleep = context.Sleeps.First(c => c.SleepGuid == sleepGuid);

                Assert.Equal(sleep.AwakeningCount, contextSleep.AwakeningCount);
                Assert.Equal(sleep.CustomerInfo.Note, contextSleep.Note);
                Assert.Equal(sleep.CustomerInfo.Quality, contextSleep.Quality);
                Assert.Equal(sleep.FallAsleepTime, contextSleep.FallAsleepTime);
                Assert.Equal(sleep.FeedingCount, contextSleep.FeedingCount);
                Assert.Equal(sleep.SleepPlace, (SleepPlace)contextSleep.SleepPlace);
                Assert.Equal(sleep.SleepTime.EndTime, contextSleep.EndTime);
                Assert.Equal(sleep.SleepTime.StartTime, contextSleep.StartTime);
            }
        }

        [Fact]
        public void AddSleepAlreadyExistsExceptionTest()
        {
            using (var context = new ApplicationContextFactory().CreateContext())
            {
                var repository = new SleepRepository(context);

                var sleepGuid = Guid.NewGuid();
                var childGuid = Guid.NewGuid();
                var sleepPlace = SleepPlace.Car;
                short quality = 8;
                var note = "Test SleepAlreadyExists";
                short feedingCount = 1;
                int fallAsleepTime = 5;
                short awakeningCount = 1;

                var startTime = new DateTime(2021, 11, 15, 11, 0, 0);
                var endTime = new DateTime(2021, 11, 15, 12, 30, 0);

                var baseSleep = SleepEntryHelper.FillChildSleep(sleepGuid, childGuid, startTime, endTime,
                        sleepPlace, quality, note, feedingCount, awakeningCount, fallAsleepTime);
                repository.Add(baseSleep);

                var newStartTime = startTime.AddHours(-1);
                var newEndTime = endTime;
                var testSleep = SleepEntryHelper.FillChildSleep(Guid.NewGuid(), childGuid, newStartTime, newEndTime,
                    sleepPlace, quality, note, feedingCount, awakeningCount, fallAsleepTime);
                Assert.Throws<SleepAlreadyExistsException>(() => repository.Add(testSleep));

                newStartTime = startTime;
                newEndTime = startTime.AddHours(1);
                testSleep = SleepEntryHelper.FillChildSleep(Guid.NewGuid(), childGuid, newStartTime, newEndTime,
                    sleepPlace, quality, note, feedingCount, awakeningCount, fallAsleepTime);
                Assert.Throws<SleepAlreadyExistsException>(() => repository.Add(testSleep));

                newStartTime = startTime.AddMinutes(30);
                newEndTime = startTime.AddHours(1);
                testSleep = SleepEntryHelper.FillChildSleep(Guid.NewGuid(), childGuid, newStartTime, newEndTime,
                    sleepPlace, quality, note, feedingCount, awakeningCount, fallAsleepTime);
                Assert.Throws<SleepAlreadyExistsException>(() => repository.Add(testSleep));

                newStartTime = startTime;
                newEndTime = endTime;
                testSleep = SleepEntryHelper.FillChildSleep(Guid.NewGuid(), childGuid, newStartTime, newEndTime,
                    sleepPlace, quality, note, feedingCount, awakeningCount, fallAsleepTime);
                Assert.Throws<SleepAlreadyExistsException>(() => repository.Add(testSleep));

                newStartTime = startTime.AddMinutes(30);
                newEndTime = endTime;
                testSleep = SleepEntryHelper.FillChildSleep(Guid.NewGuid(), childGuid, newStartTime, newEndTime,
                    sleepPlace, quality, note, feedingCount, awakeningCount, fallAsleepTime);
                Assert.Throws<SleepAlreadyExistsException>(() => repository.Add(testSleep));

                newStartTime = endTime;
                newEndTime = endTime.AddHours(1);
                testSleep = SleepEntryHelper.FillChildSleep(Guid.NewGuid(), childGuid, newStartTime, newEndTime,
                    sleepPlace, quality, note, feedingCount, awakeningCount, fallAsleepTime);
                Assert.Throws<SleepAlreadyExistsException>(() => repository.Add(testSleep));
            }
        }

        [Fact]
        public void UpdateSleepAlreadyExistsExceptionTest()
        {
            using (var context = new ApplicationContextFactory().CreateContext())
            {
                var repository = new SleepRepository(context);

                var sleepGuid = Guid.NewGuid();
                var childGuid = Guid.NewGuid();
                var sleepPlace = SleepPlace.Car;
                short quality = 8;
                var note = "Test SleepAlreadyExists";
                short feedingCount = 1;
                int fallAsleepTime = 5;
                short awakeningCount = 1;

                var startTime = new DateTime(2021, 11, 15, 11, 0, 0);
                var endTime = new DateTime(2021, 11, 15, 12, 30, 0);

                var baseSleep = SleepEntryHelper.FillChildSleep(sleepGuid, childGuid, startTime, endTime,
                        sleepPlace, quality, note, feedingCount, awakeningCount, fallAsleepTime);
                repository.Add(baseSleep);

                var testStartTime = new DateTime(2021, 11, 15, 8, 0, 0);
                var testEndTime = new DateTime(2021, 11, 15, 9, 30, 0);
                var testSleep = SleepEntryHelper.FillChildSleep(Guid.NewGuid(), childGuid, testStartTime, testEndTime,
                        sleepPlace, quality, note, feedingCount, awakeningCount, fallAsleepTime);
                repository.Add(testSleep);

                var testGuid = context.Sleeps.First(s => s.StartTime == testStartTime).SleepGuid;

                var newStartTime = startTime.AddHours(-1);
                var newEndTime = endTime;
                testSleep = SleepEntryHelper.FillChildSleep(testGuid, childGuid, newStartTime, newEndTime,
                    sleepPlace, quality, note, feedingCount, awakeningCount, fallAsleepTime);
                Assert.Throws<SleepAlreadyExistsException>(() => repository.Update(testSleep));

                newStartTime = startTime;
                newEndTime = startTime.AddHours(1);
                testSleep.SleepTime.StartTime = newStartTime;
                testSleep.SleepTime.EndTime = newEndTime;
                Assert.Throws<SleepAlreadyExistsException>(() => repository.Update(testSleep));

                newStartTime = startTime.AddMinutes(30);
                newEndTime = startTime.AddHours(1);
                testSleep.SleepTime.StartTime = newStartTime;
                testSleep.SleepTime.EndTime = newEndTime;
                Assert.Throws<SleepAlreadyExistsException>(() => repository.Update(testSleep));

                newStartTime = startTime;
                newEndTime = endTime;
                testSleep.SleepTime.StartTime = newStartTime;
                testSleep.SleepTime.EndTime = newEndTime;
                Assert.Throws<SleepAlreadyExistsException>(() => repository.Update(testSleep));

                newStartTime = startTime.AddMinutes(30);
                newEndTime = endTime;
                testSleep = SleepEntryHelper.FillChildSleep(Guid.NewGuid(), childGuid, newStartTime, newEndTime,
                    sleepPlace, quality, note, feedingCount, awakeningCount, fallAsleepTime);
                Assert.Throws<SleepAlreadyExistsException>(() => repository.Update(testSleep));

                newStartTime = endTime;
                newEndTime = endTime.AddHours(1);
                testSleep.SleepTime.StartTime = newStartTime;
                testSleep.SleepTime.EndTime = newEndTime;
                Assert.Throws<SleepAlreadyExistsException>(() => repository.Update(testSleep));
            }
        }
    }

    public class GetSleepsGenerator : TheoryData<Sleep>
    {
        public GetSleepsGenerator()
        {
            foreach (var sleep in GetTestSleeps())
            {
                this.Add(sleep);
            }
        }

        public List<Sleep> GetTestSleeps()
        {
            var sleeps = new List<Sleep>();

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

            sleeps.Add(SleepEntryHelper.FillChildSleep(sleepGuid1, childGuid1, startTime1, endTime1,
                        sleepPlace1, quality1, note1, feedingCount1, awakeningCount1, fallAsleepTime1));

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

            sleeps.Add(SleepEntryHelper.FillChildSleep(sleepGuid2, childGuid2, startTime2, endTime2,
                        sleepPlace2, quality2, note2, feedingCount2, awakeningCount2, fallAsleepTime2));

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

            sleeps.Add(SleepEntryHelper.FillChildSleep(sleepGuid3, childGuid3, startTime3, endTime3,
                        sleepPlace3, quality3, note3, feedingCount3, awakeningCount3, fallAsleepTime3));

            //4 sleep
            var sleepGuid4 = Guid.NewGuid();
            var childGuid4 = Guid.NewGuid();
            var sleepPlace4 = SleepPlace.Parents;
            var startTime4 = new DateTime(2021, 11, 15, 21, 0, 0);
            var endTime4 = new DateTime(2021, 11, 16, 06, 30, 0);
            short quality4 = 2;
            var note4 = "Test4";
            short feedingCount4 = 4;
            int fallAsleepTime4 = 45;
            short awakeningCount4 = 5;

            sleeps.Add(SleepEntryHelper.FillChildSleep(sleepGuid4, childGuid4, startTime4, endTime4,
                        sleepPlace4, quality4, note4, feedingCount4, awakeningCount4, fallAsleepTime4));

            return sleeps;
        }
    }
}
