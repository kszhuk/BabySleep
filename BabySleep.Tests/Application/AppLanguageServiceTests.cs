using BabySleep.Application.Services;
using BabySleep.Infrastructure.Data.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BabySleep.Tests.Application
{
    [Collection("AppLanguageService")]
    public class AppLanguageServiceTests
    {
        [Theory]
        [InlineData("en")]
        [InlineData("uk")]
        [InlineData("")]
        public void GetLanguageTest(string language)
        {
            var mock = new Mock<ICommonRepository>();
            mock.Setup(repo => repo.GetAppLanguage()).Returns(language);

            var appLanguageService = new AppLanguageService(mock.Object);
            Assert.Equal(language, appLanguageService.GetAppLanguage());
        }

        [Theory]
        [InlineData("en")]
        [InlineData("uk")]
        [InlineData("")]
        public void SetAppLanguageTest(string language)
        {
            var mock = new Mock<ICommonRepository>();
            mock.Setup(repo => repo.UpdateAppLanguage(language));

            var appLanguageService = new AppLanguageService(mock.Object);
            appLanguageService.SetAppLanguage(language);

            mock.Verify(repo => repo.UpdateAppLanguage(language), Times.Once());
        }
    }
}
