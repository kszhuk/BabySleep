using BabySleep.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Common.Helpers
{
    public class CustomerConfig : ICustomerConfig
    {
        public string SmtpEmail { get; set; }
        public string SmtpPassword { get; set; }
        public string FirebaseApiKey { get; set; }
        public string AwsAccessKey { get; set; }
        public string AwsSecretKey { get; set; }
    }
}
