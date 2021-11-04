using Autofac;
using BabySleep.Application.DTOAssemblers;
using BabySleep.Application.Interfaces;
using BabySleep.Application.Services;
using BabySleep.EfData;
using BabySleep.EfData.Interfaces;
using BabySleep.Infrastructure.Data.Interfaces;
using BabySleep.Infrastructure.Data.Repositories;
using System.Linq;
using System.Reflection;

namespace BabySleep.Core
{
    public class DIContainer
    {
        public ContainerBuilder CreateContainerBuilder()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ApplicationContext>()
                .As<IApplicationContext>();

            //Application services

            builder.RegisterType<AppInitService>()
                .As<IAppInitService>();
            builder.RegisterType<ChildService>()
                .As<IChildService>();
            builder.RegisterType<AppLanguageService>()
                .As<IAppLanguageService>();
            builder.RegisterType<ChildSleepMainService>()
                .As<IChildSleepMainService>();
            builder.RegisterType<ChilidSleepEntryService>()
                .As<IChilidSleepEntryService>();

            //Application dto assemblers

            builder.RegisterType<ChildDtoAssembler>()
                .As<IChildDtoAssembler>();
            builder.RegisterType<ChildSleepEntryDtoAssembler>()
                .As<IChildSleepEntryDtoAssembler>();
            builder.RegisterType<ChildSleepCarouselDtoAssembler>()
                .As<IChildSleepCarouselDtoAssembler>();

            //Infrastructure repositories

            builder.RegisterType<ChildRepository>()
                .As<IChildRepository>();
            builder.RegisterType<CommonRepository>()
                .As<ICommonRepository>();
            builder.RegisterType<SleepRepository>()
                .As<ISleepRepository>();

            //Infrastructure services



            builder.RegisterAssemblyTypes(Assembly.Load("BabySleep.Common"))
                .Where(t => t.Namespace.Contains("Interfaces"))
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name));

            builder.RegisterAssemblyTypes(Assembly.Load("BabySleep.Application"))
                .Where(t => t.Namespace.Contains("Interfaces"))
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name));

            builder.RegisterAssemblyTypes(Assembly.Load("BabySleep.Infrastructure"))
                .Where(t => t.Namespace.Contains("Data.Interfaces"))
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name));

            builder.RegisterAssemblyTypes(Assembly.Load("BabySleep.Infrastructure"))
                .Where(t => t.Namespace.Contains("Business.Interfaces"))
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name));


            builder.RegisterAssemblyTypes(Assembly.Load("BabySleep.Application"))
                .Where(t => t.Namespace.Contains("DTOAssemblers"))
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name));

            builder.RegisterAssemblyTypes(Assembly.Load("BabySleep.EfData"))
                .Where(t => t.Namespace.Contains("Interfaces"))
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name));

            return builder;
        }
    }
}
