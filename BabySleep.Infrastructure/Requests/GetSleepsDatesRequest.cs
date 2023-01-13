using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Infrastructure.Requests
{
    public class GetSleepsDatesRequest
    {
        public string childGuid { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
    }
}
