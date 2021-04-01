using Autofac;
using BabySleep.Common.Interfaces;
using BabySleep.Droid;
using BabySleep.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(ContainerService))]
namespace BabySleep.Droid
{
    public class ContainerService : IContainerService
    {
        public void RegisterAppConfig(ContainerBuilder builder)
        {
            builder.RegisterType<AndroidAppConfig>()
                   .As<IAppConfig>();
        }
    }
}