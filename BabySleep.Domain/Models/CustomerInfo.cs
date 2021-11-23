using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Domain.Models
{
    public class CustomerInfo
    {
        public short Quality { get; private set; }
        public string Note { get; private set; }

        public CustomerInfo(short quality, string note)
        {
            Quality = quality;
            Note = note;
        }
    }
}
