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
            //Future implementation


            //BasicAWSCredentials awsCredentials = new BasicAWSCredentials("AKIAYEEPPMAQHNEU3BV4", "BPKg54cjMzGbGu3aRv6KZQR0Hh/iwO+QLWOAbVmr");
            //AmazonLambdaConfig lambdaConfig = new AmazonLambdaConfig() { RegionEndpoint = RegionEndpoint.EUWest1 };
            //AmazonLambdaClient lambdaClient = new AmazonLambdaClient(awsCredentials, lambdaConfig);

            //try
            //{
            //    var response = await lambdaClient.InvokeAsync(new Amazon.Lambda.Model.InvokeRequest()
            //    {
            //        FunctionName = "GetSleeps",
            //        InvocationType = InvocationType.RequestResponse,
            //        Payload = @"""CDAFE18A-09E4-4AFF-9896-21A9DD17FC9F"""
            //    });

            //    var r = new StreamReader(response.Payload).ReadToEnd();
            //}
            //catch(Exception ex)
            //{

            //}
        }
    }
}
