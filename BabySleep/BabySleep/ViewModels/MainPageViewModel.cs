using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using Xamarin.Forms;
using BabySleep.Views;
using BabySleep.Helpers;
using BabySleep.Application.DTO;
using BabySleep.Interfaces;

namespace BabySleep.ViewModels
{
    public class MainPageViewModel
    {
        public MainPageViewModel()
        {
            Children = new ObservableCollection<ChildDto>();
            ReloadChildren();

            AddChildCommand = new Command(async () =>
                {
                    await App.NavigateMasterDetailPush(new ChildEntryPage(
                        new ChildEntryPageViewModel()));
                });

            ChildSelectedCommand = new Command(async () =>
                {
                    if (SelectedChild is null)
                        return;

                    await App.NavigateMasterDetailPush(new ChildEntryPage(new ChildEntryPageViewModel(SelectedChild.ChildGuid)));

                    SelectedChild = null;
                });

            MessagingCenter.Subscribe<App>((App)Xamarin.Forms.Application.Current, Constants.MS_UPDATE_CHILDREN_POPUP, (sender) =>
            {
                ReloadChildren();
            });
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

        public ObservableCollection<ChildDto> Children { get; set; }

        public Command AddChildCommand { get; }
        public Command ChildSelectedCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void ReloadChildren()
        {
            Children.Clear();
            //foreach (var child in App.BabySleepEngine.Children)
            //{
            //    Children.Add(child);
            //}
        }
    }
}
