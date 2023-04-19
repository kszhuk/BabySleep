using BabySleep.Common.Interfaces;
using BabySleep.Infrastructure.Data.Interfaces;
using BabySleep.Infrastructure.Helpers;
using BabySleep.Infrastructure.Requests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabySleep.Infrastructure.Data.RepositoriesAws
{
    public class UserRepositoryAws : IUserRepository
    {
        private AwsHelper awsHelper;
        public UserRepositoryAws(ICustomerConfig config)
        {
            awsHelper = new AwsHelper(config);
        }

        public string GetUserGuid(string email)
        {
            try
            {
                var request = new GetUserRequest() { email = email };
                var jsonRequest = JsonConvert.SerializeObject(request);
                var jsonResponse = awsHelper.GetLambdaResponse(AwsFunctionsEnum.GetUserGuid, jsonRequest);

                return jsonResponse;

            }
            catch
            {
                return string.Empty;
            };
        }
    }
}
