using BabySleep.Models;
using BabySleep.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;
using System.Reflection;
using BabySleep.Services;
using BabySleep.Resources.Resx;

namespace BabySleep.ViewModels
{
    /// <summary>
    /// Fill menu and handle navigation
    /// </summary>
    public class MenuViewModel : INotifyPropertyChanged
    {
        public MenuViewModel()
        {
            FillMenuItemList();
        }

        public ObservableCollection<MenuItemModel> MenuItemList { get; set; }

        #region Properties
        private MenuItemModel selectedMenuItem;
        public MenuItemModel SelectedMenuItem
        {
            get => selectedMenuItem;
            set
            {
                selectedMenuItem = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedMenuItem)));

                Navigate();
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        #region Private Methods
        private void FillMenuItemList()
        {
            MenuItemList = new ObservableCollection<MenuItemModel>()
            {
                new MenuItemModel(){
                    Id = MenuItemType.Main,
                    Title = MenuResources.Main,
                    Icon = ImageSource.FromResource("BabySleep.Resources.menu-home-icon.png",
                        typeof(MenuViewModel).GetTypeInfo().Assembly)
                },
                new MenuItemModel(){
                    Id = MenuItemType.EditLanguage,
                    Title = MenuResources.EditLanguage,
                    Icon = ImageSource.FromResource("BabySleep.Resources.google-translate-icon.png",
                        typeof(MenuViewModel).GetTypeInfo().Assembly)
                },
                new MenuItemModel(){
                    Id = MenuItemType.EditAccountInfo,
                    Title = MenuResources.EditAccountInfo,
                    Icon = ImageSource.FromResource("BabySleep.Resources.menu-info-item.png",
                        typeof(MenuViewModel).GetTypeInfo().Assembly)
                },
                new MenuItemModel(){
                    Id = MenuItemType.Sync,
                    Title = MenuResources.Sync,
                    Icon = ImageSource.FromResource("BabySleep.Resources.menu-sync-icon.png",
                        typeof(MenuViewModel).GetTypeInfo().Assembly)
                }
            };

            if(App.IsLoggedInUser())
            {
                MenuItemList.Add(new MenuItemModel()
                {
                    Id = MenuItemType.LogOut,
                    Title = MenuResources.LogOut,
                    Icon = ImageSource.FromResource("BabySleep.Resources.menu-logout-icon.png",
                        typeof(MenuViewModel).GetTypeInfo().Assembly)
                                
                });
            }
            else
            {
                MenuItemList.Add(new MenuItemModel()
                {
                    Id = MenuItemType.LogIn,
                    Title = MenuResources.LogIn,
                    Icon = ImageSource.FromResource("BabySleep.Resources.menu-login-icon.png",
                        typeof(MenuViewModel).GetTypeInfo().Assembly)
                });
            }
        }

        private void Navigate()
        {
            switch (selectedMenuItem.Id)
            {
                case MenuItemType.Main:
                    App.NavigateFromMenu(new MainPage());
                    break;
                case MenuItemType.EditLanguage:
                    App.NavigateFromMenu(new EditLanguagePage());
                    break;
                case MenuItemType.EditAccountInfo:
                    App.NavigateFromMenu(new EditAccountInfoPage());
                    break;
                case MenuItemType.Sync:
                    App.NavigateFromMenu(new SyncPage());
                    break;
                case MenuItemType.LogIn:
                    NavigateLogin();
                    break;
                case MenuItemType.LogOut:
                    NavigateLogout();
                    break;
                default:
                    break;

            }
        }

        private void NavigateLogin()
        {
            if(App.IsSubscribedUser())
            {
                App.NavigateFromMenu(new LogInPage());
            }
            else
            {
                ((App)Xamarin.Forms.Application.Current).ShowException(
                    MenuResources.LogIn, LoginResources.SubscribedException);
            }
        }

        private void NavigateLogout()
        {
            var authService = DependencyService.Resolve<IFirebaseAuthenticationService>();
            authService.SignOut();

            App.NavigateFromMenu(new LogInPage());
        }
        #endregion
    }
}
