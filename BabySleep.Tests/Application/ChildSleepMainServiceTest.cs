using BabySleep.Application.DTO;
using BabySleep.Application.DTOAssemblers;
using BabySleep.Application.Services;
using BabySleep.Common.Enums;
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
        public void GetChildTest(Guid childGuid, DateTime date, List<Sleep> sleeps, List<ChildSleepMainDto> sleepDtos)
        {
            var mock = new Mock<ISleepRepository>();
            mock.Setup(repo => repo.Take(childGuid, date)).Returns(sleeps);

            var assembler = new ChildSleepMainDtoAssembler();
            var childSleepService = new ChildSleepMainService(mock.Object, assembler);
            Assert.True(JsonComparer.JsonCompare(sleepDtos, childSleepService.GetChildSleeps(childGuid, date)));
            Assert.True(JsonComparer.JsonCompare(sleepDtos, assembler.WriteSleepsDto(sleeps)));
        }

        public class GetMainSleepsDataGenerator : TheoryData<Guid, DateTime, List<Sleep>, List<ChildSleepMainDto>>
        {
            public GetMainSleepsDataGenerator()
            {
                var sleeps = GetTestMainSleeps();
                foreach (var sleep in sleeps)
                {
                    this.Add(sleep.Item1, sleep.Item2, sleep.Item3, sleep.Item4);
                }
            }

            public List<Tuple<Guid, DateTime, List<Sleep>, List<ChildSleepMainDto>>> GetTestMainSleeps()
            {
                var sleeps = new List<Tuple<Guid, DateTime, List<Sleep>, List<ChildSleepMainDto>>>();

                //No sleeps
                sleeps.Add(Tuple.Create(Guid.NewGuid(), DateTime.Now, new List<Sleep>(), new List<ChildSleepMainDto>()));

                //1 sleep
                var childGuid = Guid.NewGuid();
                var date = new DateTime(2021, 11, 15, 12, 0, 0);
                var sleepPlace1 = SleepPlace.BabyStroller;
                var startTime1 = new DateTime(2021, 11, 15, 13, 0, 0);
                var endTime1 = new DateTime(2021, 11, 15, 15, 0, 0);
                var sleepType1 = SleepType.DaySleep;
                short quality1 = 2;
                var note1 = "Test1";
                var sleepGuid1 = Guid.NewGuid();
                var wakefulness1 = string.Empty;

                sleeps.Add(Tuple.Create(childGuid, date,
                    new List<Sleep>()
                    {
                        SleepHelper.FillChildSleepMain(sleepGuid1, childGuid, sleepPlace1, startTime1, endTime1, 0, 0, 0, quality1, note1)
                    },
                    new List<ChildSleepMainDto>()
                    {
                        SleepHelper.FillChildSleepMainDto(sleepGuid1, sleepType1, startTime1, endTime1, FormatQuality(quality1), wakefulness1, note1)
                    }));

                //3 sleeps
                startTime1 = new DateTime(2021, 11, 14, 21, 20, 30);
                endTime1 = new DateTime(2021, 11, 15, 7, 28, 25);
                sleepType1 = SleepType.NightSleep;

                var sleepPlace2 = SleepPlace.Car;
                var startTime2 = new DateTime(2021, 11, 15, 11, 0, 0);
                var endTime2 = new DateTime(2021, 11, 15, 12, 30, 0);
                var sleepType2 = SleepType.DaySleep;
                short quality2 = 5;
                var note2 = "Test2";
                var sleepGuid2 = Guid.NewGuid();
                wakefulness1 = CalcWakefulness(startTime2, endTime1);

                var sleepPlace3 = SleepPlace.Crib;
                var startTime3 = new DateTime(2021, 11, 15, 16, 0, 0);
                var endTime3 = new DateTime(2021, 11, 15, 17, 30, 0);
                var sleepType3 = SleepType.DaySleep;
                short quality3 = 7;
                var note3 = "Test3";
                var sleepGuid3 = Guid.NewGuid();
                var wakefulness2 = CalcWakefulness(startTime3, endTime2);

                var sleepPlace4 = SleepPlace.Parents;
                var startTime4 = new DateTime(2021, 11, 15, 21, 0, 0);
                var endTime4 = new DateTime(2021, 11, 16, 06, 30, 0);
                var sleepType4 = SleepType.NightSleep;
                short quality4 = 9;
                var note4 = "Test4";
                var sleepGuid4 = Guid.NewGuid();
                var wakefulness3 = CalcWakefulness(startTime4, endTime3);
                var wakefulness4 = string.Empty;

                sleeps.Add(Tuple.Create(childGuid, date,
                    new List<Sleep>()
                    {
                        SleepHelper.FillChildSleepMain(sleepGuid1, childGuid, sleepPlace1, startTime1, endTime1, 0, 0, 0, quality1, note1),
                        SleepHelper.FillChildSleepMain(sleepGuid2, childGuid, sleepPlace2, startTime2, endTime2, 0, 0, 0, quality2, note2),
                        SleepHelper.FillChildSleepMain(sleepGuid3, childGuid, sleepPlace3, startTime3, endTime3, 0, 0, 0, quality3, note3),
                        SleepHelper.FillChildSleepMain(sleepGuid4, childGuid, sleepPlace4, startTime4, endTime4, 0, 0, 0, quality4, note4)
                    },
                    new List<ChildSleepMainDto>()
                    {
                        SleepHelper.FillChildSleepMainDto(sleepGuid1, sleepType1, startTime1, endTime1, FormatQuality(quality1), wakefulness1, note1),
                        SleepHelper.FillChildSleepMainDto(sleepGuid2, sleepType2, startTime2, endTime2, FormatQuality(quality2), wakefulness2, note2),
                        SleepHelper.FillChildSleepMainDto(sleepGuid3, sleepType3, startTime3, endTime3, FormatQuality(quality3), wakefulness3, note3),
                        SleepHelper.FillChildSleepMainDto(sleepGuid4, sleepType4, startTime4, endTime4, FormatQuality(quality4), wakefulness4, note4)
                    }));

                return sleeps;
            }

            private string CalcWakefulness(DateTime date1, DateTime date2)
            {
                return (date1 - date2).ToString(@"hh\:mm");
            }

            private string FormatQuality(short quality)
            {
                return string.Format("{0}/10", quality);
            }
        }
    }
}
