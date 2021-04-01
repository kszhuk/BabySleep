using BabySleep.Common.Exceptions.Child;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BabySleep.Tests.Domain
{
    [Collection("ChildCreation")]
    public class ChildCreationTests
    {
        [Fact]
        public void CreateDefaultChildTest()
        {
            var child = new BabySleep.Domain.Models.Child();
            Assert.Equal(Guid.Empty, child.ChildGuid);
            Assert.Equal(new DateTime(), child.BirthDate);
            Assert.Null(child.Name);
            Assert.Null(child.Picture);
            Assert.Null(child.BirthWeek);
        }

        [Theory]
        [ClassData(typeof(ChildCreationDataGenerator))]
        public void CreateParamChildTest(Guid childGuid, DateTime birthDate, short? birthWeek, string name, byte[] picture)
        {
            var child = new BabySleep.Domain.Models.Child(childGuid, birthDate, birthWeek, name, picture);
            Assert.Equal(childGuid, child.ChildGuid);
            Assert.Equal(birthDate, child.BirthDate);
            Assert.Equal(birthWeek, child.BirthWeek);
            Assert.Equal(name, child.Name);
            Assert.Equal(picture, child.Picture);
        }

        public class ChildCreationDataGenerator : TheoryData<Guid, DateTime, short?, string, byte[]>
        {
            public ChildCreationDataGenerator()
            {
                this.Add(Guid.Empty, DateTime.MinValue, null, string.Empty, null);
                this.Add(Guid.NewGuid(), DateTime.Now, 30, "test", new byte[] { 0x20, 0x20 });
            }
        }
    }
}
