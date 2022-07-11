using Autofac;
using BabySleep.Application.DTO;
using BabySleep.Application.Interfaces;
using BabySleep.Common.Enums;
using BabySleep.Common.Exceptions.Sleep;
using BabySleep.Helpers;
using BabySleep.Models;
using BabySleep.Resx;
using BabySleep.Validations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;

namespace BabySleep.ViewModels
{
    public class ChildSleepEntryPageViewModel : INotifyPropertyChanged
    {

        public ChildSleepEntryPageViewModel(Guid sleepGuid) : this()
        {
            StopTimer();

            var sleep = sleepService.GetSleep(sleepGuid);

            StartDate = sleep.StartTime;
            EndDate = sleep.EndTime;
            UpdateDuration();

            IsUpdateVisible = StartDate >= MinimumDate;
            IsSaveVisible = false;

            SelectedSleepPlace = (short)sleep.SleepPlace;
            FeedingCount = new ValidatableObject<short?>()
            {
                Value = sleep.FeedingCount
            };
            AwakeningCount = new ValidatableObject<short?>()
            {
                Value = sleep.AwakeningCount
            };
            Quality = sleep.Quality;
            Notes = sleep.Notes;
            FallAsleep = new ValidatableObject<int?>()
            {
                Value = sleep.FallAsleepTime
            };
            SleepGuid = sleep.SleepGuid;
        }

        public ChildSleepEntryPageViewModel()
        {
            InitService();

            IsEnabled = false;
            //IsEnabled = true;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;

            IsUpdateVisible = false;
            IsSaveVisible = StartDate >= MinimumDate;

            TimerClickCommand = new Command(TimerClick);
            SaveCommand = new Command(Save);
            DeleteCommand = new Command(Delete);

            InitTimer();
            InitSleepPlace();

            AddValidations();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties

        private IChilidSleepEntryService sleepService;

        private Guid sleepGuid;
        public Guid SleepGuid
        {
            get => sleepGuid;
            set
            {
                sleepGuid = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SleepGuid)));
            }
        }

        private string duration;
        public string Duration
        {
            get => duration;
            set
            {
                duration = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Duration)));
            }
        }

        private DateTime minimumDate = DateTime.Now.AddMonths(-1);
        public DateTime MinimumDate
        {
            get => minimumDate;
            set
            {
                minimumDate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MinimumDate)));
            }
        }

        private DateTime maximumDate = DateTime.Now.AddDays(1);
        public DateTime MaximumDate
        {
            get => maximumDate;
            set
            {
                maximumDate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MaximumDate)));
            }
        }

        private DateTime startDate;
        public DateTime StartDate
        {
            get => startDate;
            set
            {
                startDate = value;
                UpdateDuration();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StartDate)));
            }
        }

        private DateTime endDate;
        public DateTime EndDate
        {
            get => endDate;
            set
            {
                endDate = value;
                UpdateDuration();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EndDate)));
            }
        }

        private bool isEnabled;
        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                isEnabled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEnabled)));
            }
        }

        private string timerText;
        public string TimerText
        {
            get => timerText;
            set
            {
                timerText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TimerText)));
            }
        }

        private Timer SleepTimer;

        object selectedSleepPlace;
        public object SelectedSleepPlace
        {
            get => selectedSleepPlace;
            set
            {
                selectedSleepPlace = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedSleepPlace)));
            }
        }

        private ValidatableObject<short?> feedingCount = new ValidatableObject<short?>() { Value = 0 };
        public ValidatableObject<short?> FeedingCount
        {
            get => feedingCount;
            set
            {
                feedingCount = value;
                AddValidations();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FeedingCount)));
            }
        }

        private ValidatableObject<short?> awakeningCount = new ValidatableObject<short?>() { Value = 0 };
        public ValidatableObject<short?> AwakeningCount
        {
            get => awakeningCount;
            set
            {
                awakeningCount = value;
                AddValidations();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AwakeningCount)));
            }
        }

        private short quality = 5;
        public short Quality
        {
            get => quality;
            set
            {
                quality = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Quality)));
            }
        }

        private string notes;
        public string Notes
        {
            get => notes;
            set
            {
                notes = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Notes)));
            }
        }

        private ValidatableObject<int?> fallAsleep = new ValidatableObject<int?>() { Value = Constants.FALL_ASLEEP_TIME };
        public ValidatableObject<int?> FallAsleep
        {
            get => fallAsleep;
            set
            {
                fallAsleep = value;
                AddValidations();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FallAsleep)));
            }
        }

        private bool isUpdateVisible;
        public bool IsUpdateVisible
        {
            get => isUpdateVisible;
            set
            {
                isUpdateVisible = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsUpdateVisible)));
            }
        }

        private bool isSaveVisible;
        public bool IsSaveVisible
        {
            get => isSaveVisible;
            set
            {
                isSaveVisible = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSaveVisible)));
            }
        }

        #endregion

        #region Commands
        public Command SaveCommand { get; }
        public Command DeleteCommand { get; }
        public Command TimerClickCommand { get; }
        #endregion

        #region Private methods
        #region Timer
        private void InitTimer()
        {

            TimerText = ChildSleepResources.Stop;
            SleepTimer = new Timer
            {
                Interval = 1000
            };
            SleepTimer.Elapsed += OnTimedEvent;
            SleepTimer.Start();
        }

        private async void TimerClick()
        {
            var currentTime = DateTime.Now;

            if (!IsEnabled)
            {
                StopTimer();
            }
            else
            {
                TimerText = ChildSleepResources.Stop;
                SleepTimer.Start();
                IsEnabled = false;
                StartDate = currentTime;
            }
            EndDate = currentTime;
        }

        public void StopTimer()
        {
            TimerText = ChildSleepResources.Start;
            SleepTimer.Stop();
            IsEnabled = true;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var elapsedTime = (DateTime.Now - StartDate);
                UpdateDuration(elapsedTime);
            });
        }

        private void UpdateDuration()
        {
            if ((EndDate - StartDate).Ticks < 0)
            {
                Duration = "00:00:00";
            }
            else
            {
                Duration = string.Format("{0:hh\\:mm\\:ss}", EndDate - StartDate);
            }
        }

        private void UpdateDuration(TimeSpan elapsedTime)
        {
            Duration = string.Format("{0:hh\\:mm\\:ss}", elapsedTime);
        }
        #endregion

        private void InitService()
        {
            if (sleepService == null)
            {
                sleepService = App.Container.Resolve<IChilidSleepEntryService>();
            }
        }

        private void InitSleepPlace()
        {
            SelectedSleepPlace = (short)SleepPlace.Crib;
        }

        private async void Save()
        {
            if (!AreFieldsValid())
            {
                return;
            }

            if ((EndDate - StartDate).Ticks < 0)
            {
                await ((App)Xamarin.Forms.Application.Current).ShowException(ChildSleepResources.SleepMain, ChildSleepResources.SleepTimeException);
                return;
            }

            try
            {
                var childSleep = new ChildSleepEntryDto()
                {
                    AwakeningCount = awakeningCount.Value ?? 0,
                    EndTime = endDate,
                    FallAsleepTime = fallAsleep.Value ?? Constants.FALL_ASLEEP_TIME,
                    FeedingCount = feedingCount.Value ?? 0,
                    Notes = notes,
                    Quality = quality,
                    SleepGuid = sleepGuid,
                    //SleepPlace = (SleepPlace)selectedSleepPlace,
                    StartTime = startDate,
                    ChildGuid = App.SelectedChildGuid
                };
                sleepService.Save(childSleep);

                MessagingCenter.Send((App)Xamarin.Forms.Application.Current, Constants.MS_UPDATE_SLEEPS, StartDate);
                await App.NavigateMasterDetailPop();
            }
            catch (SleepAlreadyExistsException)
            {
                await ((App)Xamarin.Forms.Application.Current).ShowException(ChildSleepResources.SleepMain, ChildSleepResources.SleepAlreadyExistsException);
            }
            catch (SleepDurationException)
            {
                await ((App)Xamarin.Forms.Application.Current).ShowException(ChildSleepResources.SleepMain,
                    string.Format(ChildSleepResources.SleepDurationException, Common.Helpers.Constants.MAX_SLEEP_DURATION));
            }
            catch (SleepTimeException)
            {
                await ((App)Xamarin.Forms.Application.Current).ShowException(ChildSleepResources.SleepMain,
                    ChildSleepResources.SleepTimeException);
            }
            catch (Exception ex)
            {
                await ((App)Xamarin.Forms.Application.Current).ShowException(ChildSleepResources.SleepMain, ex.Message);
            }
        }

        private async void Delete()
        {
            var result = await ((App)Xamarin.Forms.Application.Current).ShowQuestion(ChildSleepResources.DeleteSleepTitle, ChildSleepResources.DeleteSleepQuestion);
            if (result)
            {
                try
                {
                    sleepService.Delete(SleepGuid);

                    MessagingCenter.Send((App)Xamarin.Forms.Application.Current, Constants.MS_UPDATE_SLEEPS, StartDate);
                    await App.NavigateMasterDetailPop();
                }
                catch (Exception ex)
                {
                    await ((App)Xamarin.Forms.Application.Current).ShowException(ChildSleepResources.SleepMain, ex.Message);
                }
            }
        }

        private void AddValidations()
        {
            if (!feedingCount.Validations.Any())
            {
                FeedingCount.Validations.Add(new IsNotNullOrEmptyRule<short?> { ValidationMessage = ChildSleepResources.FeedingsPlaceholder });
            }

            if (!awakeningCount.Validations.Any())
            {
                AwakeningCount.Validations.Add(new IsNotNullOrEmptyRule<short?> { ValidationMessage = ChildSleepResources.AwakeningsPlaceholder });
            }

            if (!fallAsleep.Validations.Any())
            {
                FallAsleep.Validations.Add(new IsNotNullOrEmptyRule<int?> { ValidationMessage = ChildSleepResources.FallAsleepPlaceholder });
            }
        }

        private bool AreFieldsValid()
        {
            var isFeedingCountValid = FeedingCount.Validate();
            var isAwakeningCountValid = AwakeningCount.Validate();
            var isFallAsleepValid = FallAsleep.Validate();

            return isFeedingCountValid && isAwakeningCountValid && isFallAsleepValid;
        }
        #endregion
    }
}
