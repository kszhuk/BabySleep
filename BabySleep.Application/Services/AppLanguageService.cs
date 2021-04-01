using BabySleep.Application.Interfaces;
using BabySleep.Infrastructure.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Application.Services
{
    public class AppLanguageService : IAppLanguageService
    {
        private readonly ICommonRepository commonRepository;
        
        public AppLanguageService(ICommonRepository commonRepository)
        {
            this.commonRepository = commonRepository;
        }

        public string GetAppLanguage()
        {
            return commonRepository.GetAppLanguage();
        }

        public void SetAppLanguage(string language)
        {
            commonRepository.UpdateAppLanguage(language);
        }
    }
}
