using BabySleep.Common.Exceptions.Child;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BabySleep.Tests.Domain
{
    [Collection("ChildValidation")]
    public class ChildValidationTests
    {
        [Theory]
        [ClassData(typeof(ChildValidationDataGenerator))]
        public void ChildNameEmptyTest(Guid childGuid, DateTime birthDate, short? birthWeek, string name, byte[] picture)
        {
            name = string.Empty;
            var child = new BabySleep.Domain.Models.Child(childGuid, birthDate, birthWeek, name, picture);
            Assert.Throws<ChildNameEmptyException>(() => child.Validate());
        }

        [Theory]
        [ClassData(typeof(ChildValidationDataGenerator))]
        public void ChildNameLengthTest(Guid childGuid, DateTime birthDate, short? birthWeek, string name, byte[] picture)
        {
            name = "012345678901234567890123456789012345678901234567891";
            var child = new BabySleep.Domain.Models.Child(childGuid, birthDate, birthWeek, name, picture);
            Assert.Throws<ChildNameLengthException>(() => child.Validate());
        }

        [Theory]
        [ClassData(typeof(ChildValidationDataGenerator))]
        public void ChildAgeEarlyTest(Guid childGuid, DateTime birthDate, short? birthWeek, string name, byte[] picture)
        {
            birthDate = DateTime.Now.AddYears(-4);
            var child = new BabySleep.Domain.Models.Child(childGuid, birthDate, birthWeek, name, picture);
            Assert.Throws<ChildAgeException>(() => child.Validate());
        }

        [Theory]
        [ClassData(typeof(ChildValidationDataGenerator))]
        public void ChildAgeLateTest(Guid childGuid, DateTime birthDate, short? birthWeek, string name, byte[] picture)
        {
            birthDate = DateTime.Now.AddDays(1);
            var child = new BabySleep.Domain.Models.Child(childGuid, birthDate, birthWeek, name, picture);
            Assert.Throws<ChildAgeException>(() => child.Validate());
        }

        [Theory]
        [ClassData(typeof(ChildValidationDataGenerator))]
        public void ChildPrematureBirthWeekEarlyTest(Guid childGuid, DateTime birthDate, short? birthWeek, string name, byte[] picture)
        {
            birthWeek = 20;
            var child = new BabySleep.Domain.Models.Child(childGuid, birthDate, birthWeek, name, picture);
            Assert.Throws<ChildPrematureBirthWeekException>(() => child.Validate());
        }

        [Theory]
        [ClassData(typeof(ChildValidationDataGenerator))]
        public void ChildPrematureBirthWeekLateTest(Guid childGuid, DateTime birthDate, short? birthWeek, string name, byte[] picture)
        {
            birthWeek = 40;
            var child = new BabySleep.Domain.Models.Child(childGuid, birthDate, birthWeek, name, picture);
            Assert.Throws<ChildPrematureBirthWeekException>(() => child.Validate());
        }

        public class ChildValidationDataGenerator : TheoryData<Guid, DateTime, short?, string, byte[]>
        {
            public ChildValidationDataGenerator()
            {
                this.Add(Guid.NewGuid(), DateTime.Now, 30, "test", new byte[] { 0x20, 0x20 });
            }
        }
    }
}
