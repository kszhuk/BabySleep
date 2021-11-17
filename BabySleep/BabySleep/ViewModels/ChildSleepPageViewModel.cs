using Autofac;
using BabySleep.Application.DTO;
using BabySleep.Application.Interfaces;
using BabySleep.Helpers;
using BabySleep.Interfaces;
using BabySleep.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace BabySleep.ViewModels
{
    /// <summary>
    /// Displays all sleeps for current child
    /// </summary>
    public class ChildSleepPageViewModel : INotifyPropertyChanged, ITabPage
    {
        public ChildSleepPageViewModel()
        {
            childSleepMainService = App.Container.Resolve<IChildSleepMainService>();
            ChildSleepsMain = new ObservableCollection<ChildSleepMainDto>();
            ReloadSleeps();

            AddSleepCommand = new Command(AddSleep);
            EditSleepCommand = new Command<ChildSleepMainDto>(EditSleep);
            PreviousDateCommand = new Command(SelectPreviousDate);
            NextDateCommand = new Command(SelectNextDate);

            SubscribeMessagingCenter();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #region Commands
        public Command AddSleepCommand { get; }
        public Command EditSleepCommand { get; set; }
        public Command PreviousDateCommand { get; set; }
        public Command NextDateCommand { get; set; }
        #endregion

        #region Properties

        private readonly IChildSleepMainService childSleepMainService;

        public ObservableCollection<ChildSleepMainDto> ChildSleepsMain { get; set; }

        ChildSleepMainDto selectedChildSleep;
        public ChildSleepMainDto SelectedChildSleep
        {
            get => selectedChildSleep;
            set
            {
                selectedChildSleep = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedChildSleep)));
            }
        }

        bool isSelected = true;
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
            }
        }

        bool isNextVisible = true;
        public bool IsNextVisible
        {
            get => isNextVisible;
            set
            {
                isNextVisible = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsNextVisible)));
            }
        }

        DateTime currentDate;
        public DateTime CurrentDate
        {
            get => currentDate;
            set
            {
                if (currentDate != value)
                {
                    currentDate = value;
                    IsNextVisible = !(currentDate.Year == DateTime.Now.Year && 
                        currentDate.Month == DateTime.Now.Month && currentDate.Day == DateTime.Now.Day);
                    ReloadSleeps(currentDate);

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentDate)));
                }
            }
        }

        public string CurrentIcon => IsSelected ? "tab_sleep_selected.png" : "tab_sleep_unselected.png";

        #endregion

        #region Private Methods
        private async void AddSleep()
        {
            await App.NavigateMasterDetailPush(new ChildSleepEntryPage());
        }

        private async void EditSleep(ChildSleepMainDto sleep)
        {
            if (sleep == null)
            {
                return;
            }

            await App.NavigateMasterDetailPush(new ChildSleepEntryPage(new ChildSleepEntryPageViewModel(sleep.SleepGuid)));
        }

        private void SelectPreviousDate()
        {
            CurrentDate = CurrentDate.AddDays(-1);
        }

        private void SelectNextDate()
        {
            CurrentDate = CurrentDate.AddDays(1);
        }

        private void ReloadSleeps()
        {
            ReloadSleeps(DateTime.Now);
        }

        private void ReloadSleeps(DateTime currentDate)
        {
            CurrentDate = currentDate;
            ChildSleepsMain.Clear();

            var sleeps = childSleepMainService.GetChildSleeps(App.SelectedChildGuid, currentDate);
            foreach (var sleep in sleeps)
            {
                ChildSleepsMain.Add(sleep);
            }
        }

        private void SubscribeMessagingCenter()
        {
            MessagingCenter.Subscribe<App, DateTime>((App)Xamarin.Forms.Application.Current, Constants.MS_UPDATE_SLEEPS, (sender, currentDate) =>
            {
                ReloadSleeps(currentDate);
            });
        }
        #endregion
    }
}
