using BabySleep.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BabySleep.ViewModels
{
    public class StatisticsViewModel : INotifyPropertyChanged, ITabPage
    {
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

        public string CurrentIcon => IsSelected ? "tab_statistics_selected.png" : "tab_statistics_unselected.png";//B0B0B0

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
