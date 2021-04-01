using BabySleep.Infrastructure.Data.Repositories;
using BabySleep.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace BabySleep.Tests.Infrastructure.Data
{
    [Collection("Non-Parallel Collection")]
    public class CommonRepositoryTests
    {
        [Fact]
        public void UpdateAppLanguageTest()
        {
            using (var context = new ApplicationContextFactory().CreateContext())
            {
                var repository = new CommonRepository(context);

                Assert.Equal(0, context.Settings.Count());

                var language = "en";
                repository.UpdateAppLanguage(language);
                Assert.Equal(1, context.Settings.Count());
                Assert.Equal(language, context.Settings.First().Language);

                language = "uk";
                repository.UpdateAppLanguage(language);
                Assert.Equal(1, context.Settings.Count());
                Assert.Equal(language, context.Settings.First().Language);
            }
        }

        [Fact]
        public void GetAppLanguage()
        {
            using (var context = new ApplicationContextFactory().CreateContext())
            {
                var repository = new CommonRepository(context);

                Assert.Equal(0, context.Settings.Count());

                var language = repository.GetAppLanguage();
                Assert.Equal(1, context.Settings.Count());
                Assert.Equal("en", context.Settings.First().Language);

                language = "uk";
                repository.UpdateAppLanguage(language);
                Assert.Equal(1, context.Settings.Count());
                Assert.Equal(language, repository.GetAppLanguage());
            }
        }
    }
}
