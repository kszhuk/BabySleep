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
            BasicAWSCredentials awsCredentials = new BasicAWSCredentials("AKIAYEEPPMAQHNEU3BV4", "BPKg54cjMzGbGu3aRv6KZQR0Hh/iwO+QLWOAbVmr");
            AmazonLambdaConfig lambdaConfig = new AmazonLambdaConfig() { RegionEndpoint = RegionEndpoint.EUWest1 };
            AmazonLambdaClient lambdaClient = new AmazonLambdaClient(awsCredentials, lambdaConfig);

            try
            {
                var response = await lambdaClient.InvokeAsync(new Amazon.Lambda.Model.InvokeRequest()
                {
                    FunctionName = "SyncData",
                    InvocationType = Amazon.Lambda.InvocationType.RequestResponse,
                    Payload = @"{""Name"": ""value1""}"
                });

                var r = new StreamReader(response.Payload).ReadToEnd();
            }
            catch(Exception ex)
            {

            }
        }
    }
}
