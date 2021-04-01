namespace BabySleep.BL.Interfaces
{
    public interface IAppInitializer
    {
        string GetAppLanguage();
        bool AnyChildren();
        void Run();
    }
}
