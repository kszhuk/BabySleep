using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.AWS.Common.Models
{
    public class Sleep
    {
        public Guid ChildGuid { get; set; }
        public Guid SleepGuid { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public short Quality { get; set; }
        public short FeedingCount { get; set; }
        public short FallAsleepTime { get; set; }
        public short AwakeningCount { get; set; }
        public short SleepPlace { get; set; }
        public short Status { get; set; }
    }
}
