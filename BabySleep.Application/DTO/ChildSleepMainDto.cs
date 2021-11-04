﻿using BabySleep.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Application.DTO
{
    /// <summary>
    /// Is used for ChildSleep tabbed page
    /// </summary>
    public class ChildSleepMainDto
    {
        public Guid SleepGuid { get; set; }
        public bool IsDaySleep => SleepType == SleepType.DaySleep;
        public SleepType SleepType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Quality { get; set; }
        public string Wakefulness { get; set; }
        public string Note { get; set; }

        public string Duration
        {
            get
            {
                return (EndTime - StartTime).ToString(@"hh\:mm");
            }
        }

    }
}
