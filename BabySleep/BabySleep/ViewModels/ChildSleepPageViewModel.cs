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
            ChildSleepsMain = new ObservableCollection<ChildSleepCarouselDto>();
            ReloadSleeps();

            AddSleepCommand = new Command(AddSleep);
            EditSleepCommand = new Command<ChildSleepMainDto>(EditSleep);

            SubscribeMessagingCenter();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #region Commands
        public Command AddSleepCommand { get; }
        public Command EditSleepCommand { get; set; }
        #endregion

        #region Properties

        private readonly IChildSleepMainService childSleepMainService;

        public ObservableCollection<ChildSleepCarouselDto> ChildSleepsMain { get; set; }

        ChildSleepCarouselDto selectedChildSleep;
        public ChildSleepCarouselDto SelectedChildSleep
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

        int indicatorCount = Common.Helpers.Constants.DAYS_SLEEPS_COUNT;
        public int IndicatorCount
        {
            get => indicatorCount;
            set
            {
                indicatorCount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IndicatorCount)));
            }
        }

        bool isChangedPosition = true;

        int carouselPosition = Common.Helpers.Constants.DAYS_SLEEPS_COUNT - 1;
        public int CarouselPosition
        {
            get => carouselPosition;
            set
            {

                carouselPosition = value;
                if (carouselPosition >= 0 && carouselPosition < ChildSleepsMain.Count)
                {
                    isChangedPosition = true;
                    //CarouselCurrentItem = ChildSleepsMain[carouselPosition];
                    if (CurrentDate != ChildSleepsMain[carouselPosition].SleepDate)
                    {
                        CurrentDate = ChildSleepsMain[carouselPosition].SleepDate;
                    }

                    //if (CurrentDate == ChildSleepsMain[Common.Helpers.Constants.DAYS_SLEEPS_COUNT - 1].SleepDate && carouselPosition == 0)
                    //{
                    //    carouselPosition = Common.Helpers.Constants.DAYS_SLEEPS_COUNT - 1;
                    //}
                    //else
                    //{
                    //    carouselPosition = Common.Helpers.Constants.DAYS_SLEEPS_COUNT - 1;
                    //}
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CarouselPosition)));
            }
        }

        ChildSleepCarouselDto carouselCurrentItem;
        public ChildSleepCarouselDto CarouselCurrentItem
        {
            get => carouselCurrentItem;
            set
            {
                carouselCurrentItem = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CarouselCurrentItem)));
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

                    if (!isChangedPosition)
                    {
                        //CarouselPosition = 0;
                        //CarouselPosition = Common.Helpers.Constants.DAYS_SLEEPS_COUNT - 1;
                        //ChildSleepsMain = new ObservableCollection<ChildSleepCarouselDto>();
                        ReloadSleeps(currentDate);
                        //CarouselPosition = 0;
                        //CarouselPosition = Common.Helpers.Constants.DAYS_SLEEPS_COUNT - 1;
                        //CarouselCurrentItem = ChildSleepsMain[Common.Helpers.Constants.DAYS_SLEEPS_COUNT - 1];
                    }
                    isChangedPosition = false;

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

        private void ReloadSleeps()
        {
            ReloadSleeps(DateTime.Now);
        }

        private void ReloadSleeps(DateTime maxDate)
        {
            //foreach (var sleep in ChildSleepsMain)
            //{
            //    sleep.ChildSleeps.Clear();
            //    sleep.ChildSleeps = new List<ChildSleepMainDto>();
            //}
            ChildSleepsMain.Clear();
            //ChildSleepsMain = new ObservableCollection<ChildSleepCarouselDto>();

            var sleeps = childSleepMainService.GetChildSleeps(App.SelectedChildGuid, maxDate);
            foreach (var sleep in sleeps)
            {
                ChildSleepsMain.Add(sleep);
            }
            CarouselPosition = Common.Helpers.Constants.DAYS_SLEEPS_COUNT - 1;
        }

        private void SubscribeMessagingCenter()
        {
            MessagingCenter.Subscribe<App, DateTime>((App)Xamarin.Forms.Application.Current, Constants.MS_UPDATE_SLEEPS, (sender, maxDate) =>
            {
                CurrentDate = DateTime.Now;
                //CarouselPosition = 0;
                //CurrentDate = maxDate;
                //CarouselPosition = Common.Helpers.Constants.DAYS_SLEEPS_COUNT - 1;
                //ReloadSleeps(maxDate);
            });
        }
        #endregion
    }
}
