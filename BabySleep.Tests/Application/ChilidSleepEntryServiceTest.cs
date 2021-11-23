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
    [Collection("ChilidSleepEntryServiceTest")]
    public class ChilidSleepEntryServiceTest
    {
        [Theory]
        [ClassData(typeof(GetEntrySleepsDataGenerator))]
        public void GetChildTest(Sleep sleep, ChildSleepEntryDto sleepDto)
        {
            var mock = new Mock<ISleepRepository>();
            mock.Setup(repo => repo.Get(sleepDto.SleepGuid)).Returns(sleep);

            var assembler = new ChildSleepEntryDtoAssembler();
            var sleepService = new ChilidSleepEntryService(mock.Object, assembler);
            Assert.True(JsonComparer.JsonCompare(sleepDto, sleepService.GetSleep(sleepDto.SleepGuid)));
            Assert.True(JsonComparer.JsonCompare(sleepDto, assembler.WriteSleepDto(sleep)));
        }

        [Theory]
        [ClassData(typeof(GetEntrySleepsDataGenerator))]
        public void DeleteSleepTest(Sleep sleep, ChildSleepEntryDto sleepDto)
        {
            if(sleep == null)
            {
                return;
            }

            var sleepGuid = sleep.SleepGuid;

            var mock = new Mock<ISleepRepository>();
            mock.Setup(repo => repo.Delete(sleepGuid));

            var sleepService = new ChilidSleepEntryService(mock.Object, new ChildSleepEntryDtoAssembler());
            sleepService.Delete(sleepGuid);

            mock.Verify(repo => repo.Delete(sleepGuid), Times.Once());
        }

        [Theory]
        [ClassData(typeof(GetEntrySleepsDataGenerator))]
        public void SaveSleepTest(Sleep sleep, ChildSleepEntryDto sleepDto)
        {
            var mock = new Mock<ISleepRepository>();

            if (sleep == null)
            {
                mock.Setup(repo => repo.Add(sleep));
            }
            else
            {
                mock.Setup(repo => repo.Update(sleep));
            }

            var sleepService = new ChilidSleepEntryService(mock.Object, new ChildSleepEntryDtoAssembler());
            sleepService.Save(sleepDto);

            if (sleep == null)
            {
                mock.Verify(repo => repo.Add(It.Is<Sleep>(s => s.SleepGuid == sleepDto.SleepGuid)), Times.Once());
            }
            else
            {
                mock.Verify(repo => repo.Update(It.Is<Sleep>(s => s.SleepGuid == sleepDto.SleepGuid)), Times.Once());
            }
        }

        public class GetEntrySleepsDataGenerator : TheoryData<Sleep, ChildSleepEntryDto>
        {
            public GetEntrySleepsDataGenerator()
            {
                var sleeps = GetTestEntrySleeps();
                foreach (var sleep in sleeps)
                {
                    this.Add(sleep.Item1, sleep.Item2);
                }
            }

            public List<Tuple<Sleep, ChildSleepEntryDto>> GetTestEntrySleeps()
            {
                var sleeps = new List<Tuple<Sleep, ChildSleepEntryDto>>();

                //Empty sleep
                sleeps.Add(Tuple.Create<Sleep, ChildSleepEntryDto> (null,
                    SleepEntryHelper.FillChildSleepEntryDto(Guid.Empty, Guid.Empty, new DateTime(), new DateTime(), 
                        new Sleep().SleepPlace, 0, string.Empty, 0, 0, 0)
                ));

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

                sleeps.Add(Tuple.Create(
                    SleepEntryHelper.FillChildSleep(sleepGuid1, childGuid1, startTime1, endTime1,
                        sleepPlace1, quality1, note1, feedingCount1, awakeningCount1, fallAsleepTime1),
                    SleepEntryHelper.FillChildSleepEntryDto(sleepGuid1, childGuid1, startTime1, endTime1,
                        sleepPlace1, quality1, note1, feedingCount1, awakeningCount1, fallAsleepTime1)
                ));

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

                sleeps.Add(Tuple.Create(
                    SleepEntryHelper.FillChildSleep(sleepGuid2, childGuid2, startTime2, endTime2,
                        sleepPlace2, quality2, note2, feedingCount2, awakeningCount2, fallAsleepTime2),
                    SleepEntryHelper.FillChildSleepEntryDto(sleepGuid2, childGuid2, startTime2, endTime2,
                        sleepPlace2, quality2, note2, feedingCount2, awakeningCount2, fallAsleepTime2)
                ));

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

                sleeps.Add(Tuple.Create(
                    SleepEntryHelper.FillChildSleep(sleepGuid3, childGuid3, startTime3, endTime3,
                        sleepPlace3, quality3, note3, feedingCount3, awakeningCount3, fallAsleepTime3),
                    SleepEntryHelper.FillChildSleepEntryDto(sleepGuid3, childGuid3, startTime3, endTime3,
                        sleepPlace3, quality3, note3, feedingCount3, awakeningCount3, fallAsleepTime3)
                ));

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

                sleeps.Add(Tuple.Create(
                    SleepEntryHelper.FillChildSleep(sleepGuid4, childGuid4, startTime4, endTime4,
                        sleepPlace4, quality4, note4, feedingCount4, awakeningCount4, fallAsleepTime4),
                    SleepEntryHelper.FillChildSleepEntryDto(sleepGuid4, childGuid4, startTime4, endTime4,
                        sleepPlace4, quality4, note4, feedingCount4, awakeningCount4, fallAsleepTime4)
                ));

                return sleeps;
            }
        }
    }
}
