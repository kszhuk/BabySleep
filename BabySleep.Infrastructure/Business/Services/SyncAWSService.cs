using Amazon;
using Amazon.Lambda;
using Amazon.Runtime;
using BabySleep.Common.Interfaces;
using BabySleep.Infrastructure.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BabySleep.Infrastructure.Business.Services
{
    public class SyncAWSService : ISyncAWSService
    {
        private readonly ICustomerConfig config;

        public SyncAWSService(ICustomerConfig config)
        {
            this.config = config;
        }

        public async void Sync()
        {
            //Call sync lambda function
        }
    }
}
