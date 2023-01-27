using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Domain.Models
{
    public class EmailMessage
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Email { get; set; }
    }
}
