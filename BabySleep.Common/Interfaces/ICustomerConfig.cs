using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Common.Interfaces
{
    public interface ICustomerConfig
    {
        string SmtpEmail { get; set; }
        string SmtpPassword { get; set; }
        string FirebaseApiKey { get; set; }
        string AwsAccessKey { get; set; }
        string AwsSecretKey { get; set; }
    }
}
