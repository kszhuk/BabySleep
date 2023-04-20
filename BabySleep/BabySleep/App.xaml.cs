using System;
using System.IO;
using System.Linq;
using Xamarin.Forms;
using BabySleep.Views;
using System.Threading.Tasks;
using System.Globalization;
using Autofac;
using BabySleep.Services;
using BabySleep.Core;
using BabySleep.Application.Interfaces;
using System.Threading;
using BabySleep.Resources.Resx;
using Microsoft.Extensions.Configuration;
using BabySleep.Common.Helpers;
using BabySleep.Common.Interfaces;
using Microsoft.Extensions.FileProviders;

namespace BabySleep
{
    public partial class App : Xamarin.Forms.Application
    {
        public static FlyoutPage MasterDetail { get; set; }
        public static IContainer Container { get; set; }
        public static Guid SelectedChildGuid { get; set; }

        public App()
        {
            InitializeContainer();
            InitializeApp();

            InitializeComponent();
            InitMainPage();
        }

        #region PublicMethods
        #region Navigate methods
        public async static Task NavigateMasterDetailPush(Page page)
        {
            MasterDetail.IsPresented = false;
            await MasterDetail.Detail.Navigation.PushAsync(page);
        }

        public async static Task NavigateMasterDetailPop()
        {
            await MasterDetail.Detail.Navigation.PopAsync();
        }

        public static void NavigateFromMenu(Page page)
        {

            MasterDetail.Detail = new NavigationPage(page);
            MasterDetail.Flyout = new MenuPage();
            MasterDetail.IsPresented = false;
        }
        #endregion

        public static bool SetLanguage(string languageName)
        {
            var languages = CultureInfo.GetCultures(CultureTypes.NeutralCultures).ToList();
            var language = CultureInfo.GetCultures(CultureTypes.NeutralCultures).ToList()
                .FirstOrDefault(element => element.Name == languageName);

            if ((language != null && MenuResources.Culture != null && language.Name != MenuResources.Culture.Name) ||
                (language != null && MenuResources.Culture is null))
            {
                EditSettingsResources.Culture = language;
                ChildEntryResources.Culture = language;
                MenuResources.Culture = language;
                GeneralResources.Culture = language;

                Thread.CurrentThread.CurrentCulture = language;
                Thread.CurrentThread.CurrentUICulture = language;

                return true;
            }

            return false;
        }

        public static bool IsSubscribedUser()
        {
            return true;
        }

        public static bool IsLoggedInUser()
        {
            var authService = DependencyService.Resolve<IFirebaseAuthenticationService>();
            return authService.IsSignIn();
        }

        #region Alerts
        public Task ShowException(string title, string message)
        {
            return DependencyService.Get<IAlertBuilderService>().ShowExceptionAsync(title, message, GeneralResources.Ok);
        }

        public Task<bool> ShowQuestion(string title, string message)
        {
            return DependencyService.Get<IAlertBuilderService>().ShowQuestionAsync(title, message, GeneralResources.Yes, GeneralResources.No);
        }
        #endregion
        #endregion

        #region PrivateMethods
        private void InitializeContainer()
        {
            var externalContainer = new DIContainer();
            var builder = externalContainer.CreateContainerBuilder();

            var config = GetConfig();
            builder.Register(c => config).As<ICustomerConfig>();

            DependencyService.Get<IContainerService>().RegisterAppConfig(builder);

            Container = builder.Build();
        }

        private ICustomerConfig GetConfig()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile(new EmbeddedFileProvider(typeof(App).Assembly, typeof(App).Namespace),
                    "appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile(new EmbeddedFileProvider(typeof(App).Assembly, typeof(App).Namespace),
                    "appsettings.local.json", optional: true, reloadOnChange: true)
                .Build();

            CustomerConfig customerConfig = new CustomerConfig();

            customerConfig.SmtpEmail = config.GetValue<string>("Smtp:Email");
            customerConfig.SmtpPassword = config.GetValue<string>("Smtp:Password");

            customerConfig.FirebaseApiKey = config.GetValue<string>("Firebase:ApiKey");

            customerConfig.AwsAccessKey = config.GetValue<string>("Aws:AccessKey");
            customerConfig.AwsSecretKey = config.GetValue<string>("Aws:SecretKey");

            return customerConfig;
        }

        private void InitializeApp()
        {
            var appInitService = Container.Resolve<IAppInitService>();
            SelectedChildGuid = appInitService.GetFirstChild();

            SetLanguage(appInitService.GetAppLanguage());
        }

        private void InitMainPage()
        {
            if (SelectedChildGuid != Guid.Empty)
            {
                MainPage = new MasterPage();
            }
            else
            {
                MainPage = new ChildEntryPage();
            }
        }
        #endregion
    }
}
