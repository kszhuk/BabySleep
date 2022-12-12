using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Core;
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
            try
            {
                var contextDb = DynamoDbContextHelper.GetDynamoDbContext();

                var userQuery = contextDb.QueryAsync<DdbModels.User>(email.ToLower());

                var resultUsers = userQuery.GetRemainingAsync().Result;

                if (resultUsers.Any())
                {
                    var userGuid = resultUsers.First().UserGuid.ToString();
                    LambdaLogger.Log(string.Format("User {0} - {1} logged in", email, userGuid));
                    return userGuid;
                }

                LambdaLogger.Log(string.Format("User {0} not logged in", email));
            }
            catch(Exception ex)
            {
                LambdaLogger.Log(string.Format("Failed Users.GetUserGuid : {0}", ex.Message));
            }

            return string.Empty;
        }
    }
}
