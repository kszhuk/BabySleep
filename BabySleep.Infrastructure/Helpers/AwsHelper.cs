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
                case AwsFunctionsEnum.GetUserGuid:
                    functionName = AwsFunctionsConstants.GET_USER_GUID;
                    break;
                case AwsFunctionsEnum.GetSleeps:
                    functionName = AwsFunctionsConstants.GET_SLEEPS;
                    break;
                case AwsFunctionsEnum.GetSleep:
                    functionName = AwsFunctionsConstants.GET_SLEEP;
                    break;
                case AwsFunctionsEnum.AddSleep:
                    functionName = AwsFunctionsConstants.ADD_SLEEP;
                    break;
                case AwsFunctionsEnum.UpdateSleep:
                    functionName = AwsFunctionsConstants.UPDATE_SLEEP;
                    break;
                case AwsFunctionsEnum.DeleteSleep:
                    functionName = AwsFunctionsConstants.DELETE_SLEEP;
                    break;
            }

            var response = lambdaClient.InvokeAsync(new Amazon.Lambda.Model.InvokeRequest()
            {
                FunctionName = functionName,
                InvocationType = InvocationType.RequestResponse,
                Payload = @"{""pathParameters"": " + pathParameters + @"}"
            }).Result;

            var responseStr = new StreamReader(response.Payload).ReadToEnd();

            if(responseStr.Contains(typeof(Common.Exceptions.Sleep.SleepAlreadyExistsException).Name))
            {
                throw new Common.Exceptions.Sleep.SleepAlreadyExistsException();
            }

            var jsonResponse = JsonConvert.DeserializeObject<APIGatewayHttpApiV2ProxyResponse>(responseStr);

            return jsonResponse.Body;
        }
    }

    internal enum AwsFunctionsEnum
    {
        GetChildren,
        GetUserGuid,
        GetSleeps,
        GetSleep,
        AddSleep,
        UpdateSleep,
        DeleteSleep
    }
}
