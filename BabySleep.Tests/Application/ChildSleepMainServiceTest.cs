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
    [Collection("ChildSleepMainServiceTest")]
    public class ChildSleepMainServiceTest
    {
        [Theory]
        [ClassData(typeof(GetMainSleepsDataGenerator))]
        public void GetChildTest(Guid childGuid, DateTime date, List<Sleep> sleeps, ChildSleepMainDto sleepDto)
        {
            var mock = new Mock<ISleepRepository>();
            mock.Setup(repo => repo.Take(childGuid, date)).Returns(sleeps);

            var assembler = new ChildSleepMainDtoAssembler();
            var childSleepService = new ChildSleepMainService(mock.Object, assembler);
            Assert.True(JsonComparer.JsonCompare(sleepDto, childSleepService.GetChildSleeps(childGuid, date)));
            Assert.True(JsonComparer.JsonCompare(sleepDto, assembler.WriteSleepsDto(sleeps, date)));
        }

        public class GetMainSleepsDataGenerator : TheoryData<Guid, DateTime, List<Sleep>, ChildSleepMainDto>
        {
            public GetMainSleepsDataGenerator()
            {
                var sleeps = GetTestMainSleeps();
                foreach (var sleep in sleeps)
                {
                    this.Add(sleep.Item1, sleep.Item2, sleep.Item3, sleep.Item4);
                }
            }

            public List<Tuple<Guid, DateTime, List<Sleep>, ChildSleepMainDto>> GetTestMainSleeps()
            {
                var sleeps = new List<Tuple<Guid, DateTime, List<Sleep>, ChildSleepMainDto>>();

                //No sleeps
                sleeps.Add(Tuple.Create(Guid.NewGuid(), DateTime.Now, new List<Sleep>(), 
                    new ChildSleepMainDto() { ChildSleeps = new List<ChildSleepMainItemDto>() }));

                //1 sleep
                var childGuid = Guid.NewGuid();
                var date = new DateTime(2021, 11, 15, 12, 0, 0);
                var sleepPlace1 = SleepPlace.BabyStroller;
                var startTime1 = new DateTime(2021, 11, 15, 13, 0, 0);
                var endTime1 = new DateTime(2021, 11, 15, 15, 0, 0);
                var sleepType1 = SleepType.DaySleep;
                short quality1 = 2;
                string qualityStr1 = "2/10";
                var note1 = "Test1";
                var sleepGuid1 = Guid.NewGuid();
                var wakefulness1 = string.Empty;
                var isDaySleep1 = true;
                var durationStr1 = "02:00";
                var durationTicks1 = 72000000000;

                long daySleepsCount = 1;
                long daySleepsTime = 72000000000;
                long nightSleepsTime = 0;
                long totalSleepsTime = 72000000000;

                sleeps.Add(Tuple.Create(childGuid, date,
                    new List<Sleep>()
                    {
                        SleepHelper.FillChildSleepMain(sleepGuid1, childGuid, sleepPlace1, startTime1, endTime1, 0, 0, 0, quality1, note1)
                    },
                    SleepHelper.FillChildSleepMainDto(
                        new List<ChildSleepMainItemDto>()
                        {
                            SleepHelper.FillChildSleepMainItemDto(sleepGuid1, sleepType1, startTime1, endTime1, qualityStr1, wakefulness1, note1,
                                isDaySleep1, durationStr1, durationTicks1)
                        }, daySleepsCount, daySleepsTime, nightSleepsTime, totalSleepsTime)
                    ));

                //3 sleeps
                startTime1 = new DateTime(2021, 11, 14, 21, 20, 30);
                endTime1 = new DateTime(2021, 11, 15, 7, 28, 25);
                sleepType1 = SleepType.NightSleep;
                isDaySleep1 = false;
                durationStr1 = "10:07";
                durationTicks1 = 364750000000;

                var sleepPlace2 = SleepPlace.Car;
                var startTime2 = new DateTime(2021, 11, 15, 11, 0, 0);
                var endTime2 = new DateTime(2021, 11, 15, 12, 30, 0);
                var sleepType2 = SleepType.DaySleep;
                short quality2 = 5;
                string qualityStr2 = "5/10";
                var note2 = "Test2";
                var sleepGuid2 = Guid.NewGuid();
                var isDaySleep2 = true;
                var durationStr2 = "01:30";
                var durationTicks2 = 54000000000;
                wakefulness1 = "03:31";

                var sleepPlace3 = SleepPlace.Crib;
                var startTime3 = new DateTime(2021, 11, 15, 16, 0, 0);
                var endTime3 = new DateTime(2021, 11, 15, 17, 30, 0);
                var sleepType3 = SleepType.DaySleep;
                short quality3 = 7;
                string qualityStr3 = "7/10";
                var note3 = "Test3";
                var sleepGuid3 = Guid.NewGuid();
                var isDaySleep3 = true;
                var durationStr3 = "01:30";
                var durationTicks3 = 54000000000;
                var wakefulness2 = "03:30";

                var sleepPlace4 = SleepPlace.Parents;
                var startTime4 = new DateTime(2021, 11, 15, 21, 0, 0);
                var endTime4 = new DateTime(2021, 11, 16, 06, 30, 0);
                var sleepType4 = SleepType.NightSleep;
                short quality4 = 9;
                string qualityStr4 = "9/10";
                var note4 = "Test4";
                var sleepGuid4 = Guid.NewGuid();
                var isDaySleep4 = false;
                var durationStr4 = "09:30";
                var durationTicks4 = 342000000000;
                var wakefulness3 = "03:30";
                var wakefulness4 = string.Empty;

                daySleepsCount = 2;
                daySleepsTime = 108000000000;
                nightSleepsTime = 377050000000;
                totalSleepsTime = 485050000000;

                sleeps.Add(Tuple.Create(childGuid, date,
                    new List<Sleep>()
                    {
                        SleepHelper.FillChildSleepMain(sleepGuid1, childGuid, sleepPlace1, startTime1, endTime1, 0, 0, 0, quality1, note1),
                        SleepHelper.FillChildSleepMain(sleepGuid2, childGuid, sleepPlace2, startTime2, endTime2, 0, 0, 0, quality2, note2),
                        SleepHelper.FillChildSleepMain(sleepGuid3, childGuid, sleepPlace3, startTime3, endTime3, 0, 0, 0, quality3, note3),
                        SleepHelper.FillChildSleepMain(sleepGuid4, childGuid, sleepPlace4, startTime4, endTime4, 0, 0, 0, quality4, note4)
                    },
                    SleepHelper.FillChildSleepMainDto(
                        new List<ChildSleepMainItemDto>()
                        {
                            SleepHelper.FillChildSleepMainItemDto(sleepGuid1, sleepType1, startTime1, endTime1, qualityStr1, wakefulness1, note1,
                                isDaySleep1, durationStr1, durationTicks1),
                            SleepHelper.FillChildSleepMainItemDto(sleepGuid2, sleepType2, startTime2, endTime2, qualityStr2, wakefulness2, note2,
                                isDaySleep2, durationStr2, durationTicks2),
                            SleepHelper.FillChildSleepMainItemDto(sleepGuid3, sleepType3, startTime3, endTime3, qualityStr3, wakefulness3, note3,
                                isDaySleep3, durationStr3, durationTicks3),
                            SleepHelper.FillChildSleepMainItemDto(sleepGuid4, sleepType4, startTime4, endTime4, qualityStr4, wakefulness4, note4,
                                isDaySleep4, durationStr4, durationTicks4)
                        }, daySleepsCount, daySleepsTime, nightSleepsTime, totalSleepsTime)));

                return sleeps;
            }
        }
    }
}
