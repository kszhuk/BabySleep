using BabySleep.Resources.Resx;
using System.ComponentModel.DataAnnotations;

namespace BabySleepWeb.Models
{
    public class InputSleepModel
    {
        public Guid SleepGuid { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        [Required(ErrorMessageResourceName = "SleepPlaceError", ErrorMessageResourceType = typeof(ChildSleepResources))]
        public short SleepPlaceValue { get; set; }
        [Required]
        public short Quality { get; set; }
        [Required(ErrorMessageResourceName = "FeedingCountRequired", ErrorMessageResourceType = typeof(ChildSleepResources))]
        [Range(0, 30, ErrorMessageResourceName = "FeedingCountRange", ErrorMessageResourceType = typeof(ChildSleepResources))]
        public short FeedingCount { get; set; }
        [Required(ErrorMessageResourceName = "AwakeningCountRequired", ErrorMessageResourceType = typeof(ChildSleepResources))]
        [Range(0, 30, ErrorMessageResourceName = "AwakeningCountRange", ErrorMessageResourceType = typeof(ChildSleepResources))]
        public short AwakeningCount { get; set; }
        [Required(ErrorMessageResourceName = "FallAsleepTimeRequired", ErrorMessageResourceType = typeof(ChildSleepResources))]
        [Range(0, 60, ErrorMessageResourceName = "FallAsleepTimeRange", ErrorMessageResourceType = typeof(ChildSleepResources))]
        public int FallAsleepTime { get; set; }
        public bool IsNew => SleepGuid == Guid.Empty;
    }
}
