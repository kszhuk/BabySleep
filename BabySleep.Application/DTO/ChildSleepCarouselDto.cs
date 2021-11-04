using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Application.DTO
{
    public class ChildSleepCarouselDto
    {
        public DateTime SleepDate { get; set; }
        public List<ChildSleepMainDto> ChildSleeps { get; set; }
    }
}
