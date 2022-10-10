using Amazon;
using Amazon.Lambda;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Runtime;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BabySleep.Infrastructure.Helpers
{
    internal class AwsHelper
    {
        public string GetLambdaResponse(AwsFunctionsEnum function, string pathParameters)
        {
            BasicAWSCredentials awsCredentials = new BasicAWSCredentials("AKIAYEEPPMAQHNEU3BV4", "BPKg54cjMzGbGu3aRv6KZQR0Hh/iwO+QLWOAbVmr");
            AmazonLambdaConfig lambdaConfig = new AmazonLambdaConfig() { RegionEndpoint = RegionEndpoint.EUWest1 };
            AmazonLambdaClient lambdaClient = new AmazonLambdaClient(awsCredentials, lambdaConfig);

            var functionName = string.Empty;
            switch(function)
            {
                case AwsFunctionsEnum.GetChildren:
                    functionName = AwsFunctionsConstants.GET_CHILDREN;
                    break;
            }

            var response = lambdaClient.InvokeAsync(new Amazon.Lambda.Model.InvokeRequest()
            {
                FunctionName = functionName,
                InvocationType = InvocationType.RequestResponse,
                Payload = @"{""pathParameters"": " + pathParameters + @"}"
            }).Result;

            var responseStr = new StreamReader(response.Payload).ReadToEnd();

            var jsonResponse = JsonConvert.DeserializeObject<APIGatewayHttpApiV2ProxyResponse>(responseStr);

            return jsonResponse.Body;
        }
    }

    internal enum AwsFunctionsEnum
    {
        GetChildren
    }
}
