using BabySleep.Application.Services;
using BabySleep.Domain.Models;
using BabySleep.Infrastructure.Data.Interfaces;
using BabySleep.Infrastructure.Data.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BabySleep.Tests.Application
{
    [Collection("AppInitService")]
    public class AppInitServiceTests
    {
        private readonly Mock<IChildRepository> childMockRepository;
        private readonly Mock<ICommonRepository> commonMockRepository;

        public AppInitServiceTests()
        {
            childMockRepository = new Mock<IChildRepository>();
            commonMockRepository = new Mock<ICommonRepository>();
        }

        [Fact]
        public void AnyChildExistTrueTest()
        {
            var mock = new Mock<IChildRepository>();
            mock.Setup(repo => repo.Any()).Returns(true);

            var appInitService = new AppInitService(mock.Object, commonMockRepository.Object);
            Assert.True(appInitService.AnyChildExist());
        }

        [Fact]
        public void AnyChildExistFalseTest()
        {
            var mock = new Mock<IChildRepository>();
            mock.Setup(repo => repo.Any()).Returns(false);

            var appInitService = new AppInitService(mock.Object, commonMockRepository.Object);
            Assert.False(appInitService.AnyChildExist());
        }

        [Theory]
        [ClassData(typeof(ChildValidationDataGenerator))]
        public void GetFirstChildTest(Guid childGuid)
        {
            var child = new Child { 
                ChildGuid = childGuid, 
                Name = "test1", 
                BirthDate = DateTime.Now.AddDays(-3) };
            var mock = new Mock<IChildRepository>();
            mock.Setup(repo => repo.GetFirst()).Returns(child);

            var appInitService = new AppInitService(mock.Object, commonMockRepository.Object);
            Assert.Equal(childGuid, appInitService.GetFirstChild());
        }

        [Theory]
        [InlineData("en")]
        [InlineData("uk")]
        [InlineData("")]
        public void GetLanguageTest(string language)
        {
            var mock = new Mock<ICommonRepository>();
            mock.Setup(repo => repo.GetAppLanguage()).Returns(language);

            var appInitService = new AppInitService(childMockRepository.Object, mock.Object);
            Assert.Equal(language, appInitService.GetAppLanguage());
        }

        public class ChildValidationDataGenerator : TheoryData<Guid>
        {
            public ChildValidationDataGenerator()
            {
                this.Add(Guid.NewGuid());
                this.Add(Guid.Empty);
            }
        }

        private List<Child> GetTestChildren()
        {
            var children = new List<Child>
            {
                new Child { ChildGuid=Guid.NewGuid(), Name = "test1", BirthDate=DateTime.Now.AddDays(-3) },
                new Child { ChildGuid=Guid.NewGuid(), Name = "test2", BirthDate=DateTime.Now.AddMonths(-3) },
                new Child { ChildGuid=Guid.NewGuid(), Name = "test3", BirthDate=DateTime.Now.AddYears(-2) }
            };

            return children;
        }
    }
}
