using Amazon;
using Amazon.Lambda;
using Amazon.Runtime;
using BabySleep.Infrastructure.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BabySleep.Infrastructure.Business.Services
{
    public class SyncAWSService : ISyncAWSService
    {
        public async void Sync()
        {
            //Call sync lambda function
        }
    }
}
