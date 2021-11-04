using BabySleep.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Application.DTO
{
    public class ChildSleepEntryDto
    {
        public Guid SleepGuid { get; set; }
        public Guid ChildGuid { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public SleepPlace SleepPlace { get; set; }
        public short Quality { get; set; }
        public short FeedingCount { get; set; }
        public short AwakeningCount { get; set; }
        public string Notes { get; set; }
        public int FallAsleepTime { get; set; }
    }
}
