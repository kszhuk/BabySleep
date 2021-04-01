using Autofac;
using BabySleep.Common.Interfaces;
using BabySleep.iOS.Services;
using BabySleep.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(ContainerService))]
namespace BabySleep.iOS.Services
{
    public class ContainerService: IContainerService
    {
        public void RegisterAppConfig(ContainerBuilder builder)
        {
            builder.RegisterType<IosAppConfig>()
                   .As<IAppConfig>();
        }
    }
}