using BabySleep.Application.DTO;

namespace BabySleepWeb.Models
{
    public class StatisticsModel
    {
        //public string[] Labels { get; set; }
        //public string[] TotalHours { get; set; }
        //public string[] NightHours { get; set; }
        //public string[] DayHours { get; set; }
        public List<StatisticsEntryDto> TotalHoursStatistics { get; set; }
        public List<StatisticsEntryDto> NightHoursStatistics { get; set; }
        public List<StatisticsEntryDto> DayHoursStatistics { get; set; }
        public string TotalHoursLabel { get => BabySleep.Resources.Resx.StatisticsResources.TotalHours; }
        public string NightHoursLabel { get => BabySleep.Resources.Resx.StatisticsResources.NightHours; }
        public string DayHoursLabel { get => BabySleep.Resources.Resx.StatisticsResources.DayHours; }
    }
}
