using BabySleep.Common.Interfaces;
using BabySleep.Domain.Models;
using BabySleep.EfData;
using BabySleep.EfData.Interfaces;
using BabySleep.EfData.Models;
using BabySleep.Infrastructure.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BabySleep.Infrastructure.Data.Repositories
{
    public class CommonRepository : ICommonRepository
    {
        private readonly IApplicationContext context;

        public CommonRepository(IApplicationContext context)
        {
            this.context = context;
        }

        public string GetAppLanguage()
        {
            var languageType = (short)SettingType.Language;
            var settings = context.Settings.Where(s => s.Type == languageType).ToList();
            var language = "en";

            if (settings.Any())
            {
                language = settings.First().Language;
            }
            else
            {
                CreateNewLanguageSetting(language);
            }

            Domain.Resx.Resources.Culture = CultureInfo.GetCultures(CultureTypes.NeutralCultures).ToList()
                .FirstOrDefault(element => element.Name == language);
            return language;
        }

        public void UpdateAppLanguage(string language)
        {
            var setting = context.Settings.FirstOrDefault(s => s.Type == (short)SettingType.Language);

            if(setting == null)
            {
                CreateNewLanguageSetting(language);
            }
            else
            {
                setting.Language = language;
                context.SaveChanges();
            }

            Domain.Resx.Resources.Culture = CultureInfo.GetCultures(CultureTypes.NeutralCultures).ToList()
                .FirstOrDefault(element => element.Name == language);
        }

        private void CreateNewLanguageSetting(string language)
        {
            var setting = new EfData.Models.Setting()
            {
                RowId = 0,
                Language = language,
                Type = (short)SettingType.Language
            };
            context.Settings.Add(setting);
            context.SaveChanges();
        }
    }
}
