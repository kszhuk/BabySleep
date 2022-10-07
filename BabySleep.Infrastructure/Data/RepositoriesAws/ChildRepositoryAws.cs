using Amazon;
using Amazon.Lambda;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Runtime;
using BabySleep.Domain.Models;
using BabySleep.Infrastructure.Data.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BabySleep.Infrastructure.Data.RepositoriesAws
{
    public class ChildRepositoryAws : IChildRepository
    {
        public void Add(Child child)
        {
            throw new NotImplementedException();
        }

        public bool Any()
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid childGuid)
        {
            throw new NotImplementedException();
        }

        public Child Get(Guid childGuid)
        {
            throw new NotImplementedException();
        }

        public IList<Child> GetAll()
        {
            //BasicAWSCredentials awsCredentials = new BasicAWSCredentials("AKIAYEEPPMAQHNEU3BV4", "BPKg54cjMzGbGu3aRv6KZQR0Hh/iwO+QLWOAbVmr");
            //AmazonLambdaConfig lambdaConfig = new AmazonLambdaConfig() { RegionEndpoint = RegionEndpoint.EUWest1 };
            //AmazonLambdaClient lambdaClient = new AmazonLambdaClient(awsCredentials, lambdaConfig);

            //try
            //{
            //    var response = lambdaClient.InvokeAsync(new Amazon.Lambda.Model.InvokeRequest()
            //    {
            //        FunctionName = "babysleep-BabySleepApiChildrenGetChildrenGenerated-BHPrdpLDC8mH",
            //        InvocationType = InvocationType.RequestResponse,
            //        Payload = @"""CDAFE18A-09E4-4AFF-9896-21A9DD17FC9F"""
            //    }).Result;

            //    var r = new StreamReader(response.Payload).ReadToEnd();
            //}
            //catch (Exception ex)
            //{
            //    return new List<Child>();
            //};

            return new List<Child>();
        }

        public IList<Child> GetAll(Guid userGuid)
        {
            BasicAWSCredentials awsCredentials = new BasicAWSCredentials("AKIAYEEPPMAQHNEU3BV4", "BPKg54cjMzGbGu3aRv6KZQR0Hh/iwO+QLWOAbVmr");
            AmazonLambdaConfig lambdaConfig = new AmazonLambdaConfig() { RegionEndpoint = RegionEndpoint.EUWest1 };
            AmazonLambdaClient lambdaClient = new AmazonLambdaClient(awsCredentials, lambdaConfig);

            try
            {
                var response = lambdaClient.InvokeAsync(new Amazon.Lambda.Model.InvokeRequest()
                {
                    FunctionName = "GetChildren",
                    InvocationType = InvocationType.RequestResponse,
                    Payload = @"{""pathParameters"": {""userGuid"": """ + userGuid.ToString() + @"""}}"
                }).Result;

                var responseStr = new StreamReader(response.Payload).ReadToEnd();

                var jsonResponse = JsonConvert.DeserializeObject<APIGatewayHttpApiV2ProxyResponse>(responseStr);
                var jsconChildren = JsonConvert.DeserializeObject<List<AWS.Common.Models.Child>>(jsonResponse.Body);

                var result = new List<Child>();

                foreach(var child in jsconChildren)
                {
                    result.Add(new Child(child.ChildGuid, child.BirthDate, null, child.Name, null));
                }

                return result;

            }
            catch (Exception ex)
            {
                return new List<Child>();
            };

            return new List<Child>();
        }

        public Child GetFirst()
        {
            throw new NotImplementedException();
        }

        public void Update(Child child)
        {
            throw new NotImplementedException();
        }
    }
}
