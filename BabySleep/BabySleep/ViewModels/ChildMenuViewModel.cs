using Autofac;
using BabySleep.Application.Interfaces;
using BabySleep.Helpers;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace BabySleep.ViewModels
{
    /// <summary>
    /// Selected child's image in menu, shows children popup on image click
    /// </summary>
    public class ChildMenuViewModel : INotifyPropertyChanged
    {
        public ChildMenuViewModel()
        {
            childService = App.Container.Resolve<IChildService>();

            SetProperties();
            SubscribeMessagingCenter();

            SelectChildPictureCommand = new Command(SelectChildPicture);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties
        private readonly IChildService childService;

        byte[] picture;
        public byte[] Picture
        {
            get => picture;
            set
            {
                picture = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Picture)));
            }
        }

        string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }

        bool isEmptyPicture;
        public bool IsEmptyPicture
        {
            get => isEmptyPicture;
            set
            {
                isEmptyPicture = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEmptyPicture)));
            }
        }
        #endregion

        #region Commands
        public Command SelectChildPictureCommand { get; }
        #endregion

        #region Private Methods
        private void SetProperties()
        {
            var child = childService.GetChild(App.SelectedChildGuid);
            if (child.ChildGuid == Guid.Empty)
            {
                child = childService.GetFirstChild();
                App.SelectedChildGuid = child.ChildGuid;
            }

            Picture = child.Picture;
            IsEmptyPicture = child.IsEmptyPicture;
            Name = child.Name;
        }

        private void SubscribeMessagingCenter()
        {
            MessagingCenter.Subscribe<App>((App)Xamarin.Forms.Application.Current, Constants.MS_UPDATE_MENU, (sender) =>
            {
                SetProperties();
            });
        }

        private void SelectChildPicture()
        {
            MessagingCenter.Send((App)Xamarin.Forms.Application.Current, Constants.MS_CHILDREN_POPUP);
        }
        #endregion
    }
}
