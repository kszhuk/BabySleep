using Autofac;
using BabySleep.Application.DTOAssemblers;
using BabySleep.Application.Interfaces;
using BabySleep.Application.Services;
using BabySleep.Infrastructure.Business.Interfaces;
using BabySleep.Infrastructure.Business.Services;
using BabySleep.Infrastructure.Data.Interfaces;
using BabySleep.Infrastructure.Data.RepositoriesAws;
using System.Linq;
using System.Reflection;

namespace BabySleep.Core
{
    public class DIContanerWebModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ChildService>()
                .As<IChildService>();
            builder.RegisterType<ChildSleepMainService>()
                .As<IChildSleepMainService>();
            builder.RegisterType<ChilidSleepEntryService>()
                .As<IChilidSleepEntryService>();
            builder.RegisterType<StatisticsService>()
                .As<IStatisticsService>();
            builder.RegisterType<SyncAWSService>()
                .As<ISyncAWSService>();

            //Application dto assemblers

            builder.RegisterType<ChildDtoAssembler>()
                .As<IChildDtoAssembler>();
            builder.RegisterType<ChildSleepEntryDtoAssembler>()
                .As<IChildSleepEntryDtoAssembler>();
            builder.RegisterType<ChildSleepMainDtoAssembler>()
                .As<IChildSleepMainDtoAssembler>();
            builder.RegisterType<StatisticsDtoAssembler>()
                .As<IStatisticsDtoAssembler>();

            //Infrastructure repositories

            builder.RegisterType<ChildRepositoryAws>()
                .As<IChildRepository>();
            builder.RegisterType<SleepRepositoryAws>()
                .As<ISleepRepository>();

            //Infrastructure services

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
        }
    }
}
