using Autofac;
using BabySleep.Application.DTO;
using BabySleep.Application.Interfaces;
using BabySleep.Helpers;
using BabySleep.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;

namespace BabySleep.ViewModels
{
    /// <summary>
    /// Displays popup with all children
    /// </summary>
    public class ChildrenPopupViewModel : INotifyPropertyChanged
    {
        public ChildrenPopupViewModel()
        {
            childService = App.Container.Resolve<IChildService>();

            Children = new ObservableCollection<ChildDto>();
            ReloadChildren();

            AddChildCommand = new Command(AddChild);
            ChildSelectedCommand = new Command(SelectChild);
            EditChildCommand = new Command<ChildDto>(EditChild);
            ClosePopupCommand = new Command(ClosePopup);

            SubscribeMessagingCenter();
        }

        #region Properties
        private readonly IChildService childService;

        bool isPopupVisible = false;
        public bool IsPopupVisible
        {
            get => isPopupVisible;
            set
            {
                isPopupVisible = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsPopupVisible)));
            }
        }

        bool isAddVisible;
        public bool IsAddVisible
        {
            get => isAddVisible;
            set
            {
                isAddVisible = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsAddVisible)));
            }
        }

        ChildDto selectedChild;
        public ChildDto SelectedChild
        {
            get => selectedChild;
            set
            {
                selectedChild = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedChild)));
            }
        }

        private int customCollectionViewHeight;
        public int CustomCollectionViewHeight
        {
            get => customCollectionViewHeight;
            set
            {
                customCollectionViewHeight = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CustomCollectionViewHeight)));
            }
        }
        #endregion

        public ObservableCollection<ChildDto> Children { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        #region Commands
        public Command AddChildCommand { get; }
        public Command ChildSelectedCommand { get; }
        public Command ClosePopupCommand { get; }
        public Command EditChildCommand { get; }
        #endregion

        #region Private Methods
        private void ReloadChildren()
        {
            Children.Clear();

            var children = childService.GetChildren();
            foreach (var child in children)
            {
                Children.Add(child);
            }

            CustomCollectionViewHeight = children.Count * 55;
            IsAddVisible = Children.Count <= 4;
        }

        private async void EditChild(ChildDto child)
        {
            if (child is null)
                return;

            await App.NavigateMasterDetailPush(new ChildEntryPage(new ChildEntryPageViewModel(child.ChildGuid)));
        }

        private async void AddChild()
        {
            await App.NavigateMasterDetailPush(new ChildEntryPage(new ChildEntryPageViewModel()));
        }

        private void SelectChild()
        {
            if (SelectedChild is null)
                return;

            App.SelectedChildGuid = SelectedChild.ChildGuid;

            MessagingCenter.Send((App)Xamarin.Forms.Application.Current, Constants.MS_UPDATE_MENU);
            MessagingCenter.Send((App)Xamarin.Forms.Application.Current, Constants.MS_UPDATE_SLEEPS, DateTime.Now);

            IsPopupVisible = false;
            SelectedChild = null;
        }

        private void ClosePopup()
        {
            IsPopupVisible = false;
        }

        private void SubscribeMessagingCenter()
        {
            MessagingCenter.Subscribe<App>((App)Xamarin.Forms.Application.Current, Constants.MS_UPDATE_CHILDREN_POPUP, (sender) =>
            {
                ReloadChildren();
            });
            MessagingCenter.Subscribe<App>((App)Xamarin.Forms.Application.Current, Constants.MS_CHILDREN_POPUP, (sender) =>
            {
                IsPopupVisible = true;
            });
        }
        #endregion
    }
}
