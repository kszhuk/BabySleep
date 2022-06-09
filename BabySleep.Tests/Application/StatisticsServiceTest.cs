using BabySleep.Application.DTO;
using BabySleep.Application.DTOAssemblers;
using BabySleep.Application.Services;
using BabySleep.Common.Enums;
using BabySleep.Common.Helpers;
using BabySleep.Domain.Models;
using BabySleep.Infrastructure.Data.Interfaces;
using BabySleep.Tests.Helpers;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace BabySleep.Tests.Application
{
    [Collection("StatisticsServiceTest")]
    public class StatisticsServiceTest
    {
        [Theory]
        [ClassData(typeof(GetStatisticsDataGenerator))]
        public void GetStatisticTest(Guid childGuid, DateTime startDate, DateTime endDate, List<Sleep> sleeps, StatisticsDto sleepDto)
        {
            var mock = new Mock<ISleepRepository>();
            mock.Setup(repo => repo.Take(childGuid, startDate, endDate)).Returns(sleeps);

            var assembler = new StatisticsDtoAssembler();
            var statisticsService = new StatisticsService(mock.Object, assembler);
            Assert.True(JsonComparer.JsonCompare(sleepDto, statisticsService.GetStatistics(childGuid, startDate, endDate)));
            Assert.True(JsonComparer.JsonCompare(sleepDto, assembler.WriteStatisticsDto(sleeps, startDate, endDate)));
        }

        public class GetStatisticsDataGenerator : TheoryData<Guid, DateTime, DateTime, List<Sleep>, StatisticsDto>
        {
            public GetStatisticsDataGenerator()
            {
                var sleeps = GetStatisticsSleeps();
                foreach (var sleep in sleeps)
                {
                    this.Add(sleep.Item1, sleep.Item2, sleep.Item3, sleep.Item4, sleep.Item5);
                }
            }

            public List<Tuple<Guid, DateTime, DateTime, List<Sleep>, StatisticsDto>> GetStatisticsSleeps()
            {
                var sleeps = new List<Tuple<Guid, DateTime, DateTime, List<Sleep>, StatisticsDto>>();

                var currentDate = DateTime.Now;
                var nextDate = currentDate.AddDays(1);
                //No sleeps
                sleeps.Add(Tuple.Create(Guid.NewGuid(), currentDate, nextDate, new List<Sleep>(),
                    new StatisticsDto()
                    {
                        DayHoursStatistics = new List<StatisticsEntryDto>()
                        {
                            StatisticsHelper.FillEmptyStatisticsEntryDto(currentDate),
                            StatisticsHelper.FillEmptyStatisticsEntryDto(nextDate)
                        },
                        DaySleepsCountStatistics = new List<StatisticsEntryDto>()
                        {
                            StatisticsHelper.FillEmptyStatisticsEntryDto(currentDate, false),
                            StatisticsHelper.FillEmptyStatisticsEntryDto(nextDate, false)
                        },
                        NightHoursStatistics = new List<StatisticsEntryDto>()
                        {
                            StatisticsHelper.FillEmptyStatisticsEntryDto(currentDate),
                            StatisticsHelper.FillEmptyStatisticsEntryDto(nextDate)
                        },
                        TotalHoursStatistics = new List<StatisticsEntryDto>()
                        {
                            StatisticsHelper.FillEmptyStatisticsEntryDto(currentDate),
                            StatisticsHelper.FillEmptyStatisticsEntryDto(nextDate)
                        }
                    }));

                //1 sleep
                var childGuid = Guid.NewGuid();
                var date = new DateTime(2021, 11, 15, 12, 0, 0);
                var sleepPlace1 = SleepPlace.BabyStroller;
                var startTime1 = new DateTime(2021, 11, 15, 13, 0, 0);
                var endTime1 = new DateTime(2021, 11, 15, 15, 0, 0);
                short quality1 = 2;
                var note1 = "Test1";
                var sleepGuid1 = Guid.NewGuid();
                var durationStr = "02:00";

                long daySleepsCount = 1;
                long daySleepsTime = 72000000000;
                long totalSleepsTime = 72000000000;

                sleeps.Add(Tuple.Create(childGuid, date, date,
                    new List<Sleep>()
                    {
                        SleepHelper.FillChildSleepMain(sleepGuid1, childGuid, sleepPlace1, startTime1, endTime1, 0, 0, 0, quality1, note1)
                    },
                    new StatisticsDto()
                    {
                        DayHoursStatistics = new List<StatisticsEntryDto>()
                        {
                            StatisticsHelper.FillStatisticsEntryDto(startTime1, daySleepsTime, durationStr)
                        },
                        DaySleepsCountStatistics = new List<StatisticsEntryDto>()
                        {
                            StatisticsHelper.FillStatisticsEntryDto(startTime1, daySleepsCount, daySleepsCount.ToString())
                        },
                        NightHoursStatistics = new List<StatisticsEntryDto>()
                        {
                            StatisticsHelper.FillEmptyStatisticsEntryDto(startTime1)
                        },
                        TotalHoursStatistics = new List<StatisticsEntryDto>()
                        {
                            StatisticsHelper.FillStatisticsEntryDto(startTime1, totalSleepsTime, durationStr)
                        }
                    }));

                //3 sleeps
                startTime1 = new DateTime(2021, 11, 14, 21, 20, 30);
                endTime1 = new DateTime(2021, 11, 15, 7, 28, 25);

                var sleepPlace2 = SleepPlace.Car;
                var startTime2 = new DateTime(2021, 11, 15, 11, 0, 0);
                var endTime2 = new DateTime(2021, 11, 15, 12, 30, 0);
                short quality2 = 5;
                var note2 = "Test2";
                var sleepGuid2 = Guid.NewGuid();

                var sleepPlace3 = SleepPlace.Crib;
                var startTime3 = new DateTime(2021, 11, 15, 16, 0, 0);
                var endTime3 = new DateTime(2021, 11, 15, 17, 30, 0);
                short quality3 = 7;
                var note3 = "Test3";
                var sleepGuid3 = Guid.NewGuid();

                var sleepPlace4 = SleepPlace.Parents;
                var startTime4 = new DateTime(2021, 11, 15, 21, 0, 0);
                var endTime4 = new DateTime(2021, 11, 16, 06, 30, 0);
                short quality4 = 9;
                var note4 = "Test4";
                var sleepGuid4 = Guid.NewGuid();

                daySleepsCount = 2;
                daySleepsTime = 108000000000;
                long nightSleepsTime = 342000000000;
                totalSleepsTime = 450000000000;

                var startDate = DateTimeHelper.FormatEmptyDate(startTime2);
                var endDate = DateTimeHelper.FormatEmptyDate(endTime4);

                sleeps.Add(Tuple.Create(childGuid, startDate, endDate,
                    new List<Sleep>()
                    {
                        SleepHelper.FillChildSleepMain(sleepGuid1, childGuid, sleepPlace1, startTime1, endTime1, 0, 0, 0, quality1, note1),
                        SleepHelper.FillChildSleepMain(sleepGuid2, childGuid, sleepPlace2, startTime2, endTime2, 0, 0, 0, quality2, note2),
                        SleepHelper.FillChildSleepMain(sleepGuid3, childGuid, sleepPlace3, startTime3, endTime3, 0, 0, 0, quality3, note3),
                        SleepHelper.FillChildSleepMain(sleepGuid4, childGuid, sleepPlace4, startTime4, endTime4, 0, 0, 0, quality4, note4)
                    },
                    new StatisticsDto()
                    {
                        DayHoursStatistics = new List<StatisticsEntryDto>()
                        {
                            StatisticsHelper.FillStatisticsEntryDto(startTime2, daySleepsTime, "03:00"),
                            StatisticsHelper.FillEmptyStatisticsEntryDto(endTime4)
                        },
                        DaySleepsCountStatistics = new List<StatisticsEntryDto>()
                        {
                            StatisticsHelper.FillStatisticsEntryDto(startTime2, daySleepsCount, daySleepsCount.ToString()),
                            StatisticsHelper.FillEmptyStatisticsEntryDto(endTime4, false)
                        },
                        NightHoursStatistics = new List<StatisticsEntryDto>()
                        {
                            StatisticsHelper.FillStatisticsEntryDto(startTime2, nightSleepsTime, "09:30"),
                            StatisticsHelper.FillEmptyStatisticsEntryDto(endTime4)
                        },
                        TotalHoursStatistics = new List<StatisticsEntryDto>()
                        {
                            StatisticsHelper.FillStatisticsEntryDto(startTime2, totalSleepsTime, "12:30"),
                            StatisticsHelper.FillEmptyStatisticsEntryDto(endTime4)
                        }
                    }));

                return sleeps;
            }
        }
    }
}
