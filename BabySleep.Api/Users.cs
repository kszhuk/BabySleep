using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.Annotations;
using BabySleep.Api.Helpers;

namespace BabySleep.Api
{
    public class Users
    {
        /// <summary>
        /// Returns guid for user
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [LambdaFunction(Name = "GetUserGuid")]
        [HttpApi(LambdaHttpMethod.Get, "/getuserguid/{email}/")]
        public string GetUserGuid(string email)
        {
            var contextDb = DynamoDbContextHelper.GetDynamoDbContext();

            var userQuery = contextDb.QueryAsync<DdbModels.User>(email.ToLower());

            var resultUsers = userQuery.GetRemainingAsync().Result;

            if(resultUsers.Any())
            {
                return resultUsers.First().UserGuid.ToString();
            }

            return string.Empty;
        }
    }
}
