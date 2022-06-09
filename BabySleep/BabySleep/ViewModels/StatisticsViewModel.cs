using Autofac;
using BabySleep.Application.DTO;
using BabySleep.Application.Interfaces;
using BabySleep.Helpers;
using BabySleep.Interfaces;
using BabySleep.Resx;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BabySleep.ViewModels
{
    public class StatisticsViewModel : INotifyPropertyChanged, ITabPage
    {
        public StatisticsViewModel()
        {
            statisticsService = App.Container.Resolve<IStatisticsService>();

            StartDate = DateTime.Now.AddDays(-7);
            EndDate = DateTime.Now;
            RedrawCharts();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties

        private readonly IStatisticsService statisticsService;

        bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
            }
        }

        public string CurrentIcon => IsSelected ? "tab_statistics_selected.png" : "tab_statistics_unselected.png";

        DateTime startDate;
        public DateTime StartDate
        {
            get => startDate;
            set
            {
                if(IsExceedDaysLimit(value, endDate))
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StartDate)));
                    return;
                }
                if (startDate != value)
                {
                    startDate = value;
                    RedrawCharts();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StartDate)));
                }
            }
        }

        DateTime endDate;
        public DateTime EndDate
        {
            get => endDate;
            set
            {
                if (IsExceedDaysLimit(startDate, value))
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EndDate)));
                    return;
                }
                if (endDate != value)
                {
                    endDate = value;
                    RedrawCharts();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EndDate)));
                }
            }
        }

        LineChart totalHours;
        public LineChart TotalHours
        {
            get => totalHours;
            set
            {
                if (totalHours != value)
                {
                    totalHours = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalHours)));
                }
            }
        }

        LineChart dayHours;
        public LineChart DayHours
        {
            get => dayHours;
            set
            {
                if (dayHours != value)
                {
                    dayHours = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DayHours)));
                }
            }
        }

        LineChart nightHours;
        public LineChart NightHours
        {
            get => nightHours;
            set
            {
                if (nightHours != value)
                {
                    nightHours = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NightHours)));
                }
            }
        }

        LineChart daySleeps;
        public LineChart DaySleeps
        {
            get => daySleeps;
            set
            {
                if (daySleeps != value)
                {
                    daySleeps = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DaySleeps)));
                }
            }
        }

        #endregion

        #region Private methods

        private void RedrawCharts()
        {
            var statistics = statisticsService.GetStatistics(App.SelectedChildGuid, StartDate, EndDate);

            TotalHours = CreateLineChart(statistics.TotalHoursStatistics);
            NightHours = CreateLineChart(statistics.NightHoursStatistics);
            DayHours = CreateLineChart(statistics.DayHoursStatistics);
            DaySleeps = CreateLineChart(statistics.DaySleepsCountStatistics);

        }

        private LineChart CreateLineChart(List<StatisticsEntryDto> statisticsItems)
        {
            return new LineChart()
            {
                Entries = ConvertStatisticsToEntries(statisticsItems),
                PointSize = 15,
                LineSize = 12,
                LabelTextSize = 35f,
                LabelOrientation = Orientation.Vertical,
                ValueLabelOrientation = Orientation.Vertical,
                BackgroundColor = SKColors.AliceBlue
            };
        }

        private List<ChartEntry> ConvertStatisticsToEntries(List<StatisticsEntryDto> statistics)
        {
            var entries = new List<ChartEntry>();

            foreach(var item in statistics)
            {
                entries.Add(new ChartEntry(item.Value)
                {
                    Label = item.Label,
                    ValueLabel = item.ValueLabel,
                    Color = SKColor.Parse(Constants.COLOR_HEADER)
                });
            }

            return entries;
        }

        private bool IsExceedDaysLimit(DateTime startDate, DateTime endDate)
        {
            var isExceeds =  (endDate - startDate).TotalDays > 30;
            if(isExceeds)
            {
                ((App)Xamarin.Forms.Application.Current).ShowException(GeneralResources.Statistics, StatisticsResources.ExceedDaysLimit);
            }
            return isExceeds;
        }

        #endregion
    }
}
