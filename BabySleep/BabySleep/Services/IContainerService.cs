using Autofac;

namespace BabySleep.Services
{
    public interface IContainerService
    {
        void RegisterAppConfig(ContainerBuilder builder);
    }
}
