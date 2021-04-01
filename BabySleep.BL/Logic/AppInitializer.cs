using BabySleep.BL.Interfaces;
using BabySleep.EfData;
using BabySleep.EfData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Linq;

namespace BabySleep.BL.Logic
{
    public class AppInitializer : IAppInitializer
    {
        private readonly ApplicationContext context;
        private ITestDI _testDI;
        public AppInitializer(ITestDI testDI)
        {
            //context = new ApplicationContext(Helpers.Environment.GetDbFolder());
            //InitializeDb();
            _testDI = testDI;
        }

        public void Run()
        {
            _testDI.Run();
        }

        public bool AnyChildren()
        {
            return context.Children.ToList().Any();
        }

        public string GetAppLanguage()
        {
            var settings = context.Settings.Where(s => s.Language == "en").ToList();
        
            if (!settings.Any())
            {
                var setting = new Setting()
                {
                    RowId = 0,
                    Language = "en"
                };
                settings.Add(setting);
                context.SaveChanges();
            }

            var language = context.Settings.First(s => s.Language == "en").Language;

            Resx.Resources.Culture = CultureInfo.GetCultures(CultureTypes.NeutralCultures).ToList()
                .FirstOrDefault(element => element.Name == language);
            return language;
        }

        private void InitializeDb()
        {
            context.Database.Migrate(); //Applies any pending migrations. Will create the database if it does not already exist.
        }
    }
}
