using BabySleep.Models;
using BabySleep.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;
using System.Reflection;

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
                    Title = Resx.MenuResources.Main,
                    Icon = ImageSource.FromResource("BabySleep.Resources.menu-home-icon.png",
                        typeof(MenuViewModel).GetTypeInfo().Assembly)
                },
                new MenuItemModel(){
                    Id = MenuItemType.EditSettings,
                    Title = Resx.MenuResources.EditLanguage,
                    Icon = ImageSource.FromResource("BabySleep.Resources.google-translate-icon.png",
                        typeof(MenuViewModel).GetTypeInfo().Assembly)
                }
            };
        }

        private void Navigate()
        {
            switch (selectedMenuItem.Id)
            {
                case MenuItemType.Main:
                    App.NavigateFromMenu(new MainPage());
                    break;
                case MenuItemType.EditSettings:
                    App.NavigateFromMenu(new EditLanguagePage());
                    break;
                default:
                    break;

            }
        }
        #endregion
    }
}
