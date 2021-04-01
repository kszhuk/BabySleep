using BabySleep.Application.Interfaces;
using BabySleep.Infrastructure.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Application.Services
{
    public class AppInitService : IAppInitService
    {
        private readonly IChildRepository childRepository;
        private readonly ICommonRepository commonRepository;

        public AppInitService(IChildRepository childRepository, ICommonRepository commonRepository)
        {
            this.childRepository = childRepository;
            this.commonRepository = commonRepository;
        }

        public bool AnyChildExist()
        {
            return childRepository.Any();
        }

        public string GetAppLanguage()
        {
            return commonRepository.GetAppLanguage();
        }

        public Guid GetFirstChild()
        {
            return childRepository.GetFirst().ChildGuid;
        }
    }
}
