using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Infrastructure.Requests
{
    public class GetSleepsRequest
    {
        public string childGuid { get; set; }
        public string currentDate { get; set; }
    }
}
