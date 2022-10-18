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
        [LambdaFunction()]
        [HttpApi(LambdaHttpMethod.Get, "/getuserguid/{email}/")]
        public string GetUserGuid(string email)
        {
            var contextDb = DynamoDbContextHelper.GetDynamoDbContext();

            var userSearch = contextDb.ScanAsync<DdbModels.User>(
              new[] {
            new ScanCondition
              (
                nameof(DdbModels.User.Email),
                ScanOperator.Equal,
                email.ToLower()
              )
              }
            );

            var resultUsers = userSearch.GetRemainingAsync().Result;

            if(resultUsers.Any())
            {
                return resultUsers.First().UserGuid.ToString();
            }

            return string.Empty;
        }
    }
}
