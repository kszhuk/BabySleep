using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.Annotations;
using BabySleep.Api.Helpers;
using BabySleep.AWS.Common.Models;

namespace BabySleep.Api
{
    public class Children
    {

        /// <summary>
        /// Returns all children for user
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        [LambdaFunction(Name = "GetChildren")]
        [HttpApi(LambdaHttpMethod.Get, "/getchildren/{userGuid}/")]
        public List<Child> GetChildren(string userGuid)
        {
            var children = new List<Child>();

            var contextDb = DynamoDbContextHelper.GetDynamoDbContext();
            var childQuery = contextDb.QueryAsync<DdbModels.Child>(userGuid.ToUpper());

            var resultChildren = childQuery.GetRemainingAsync().Result;

            foreach (var child in resultChildren)
            {
                children.Add(new Child()
                {
                    ChildGuid = Guid.Parse(child.ChildGUID),
                    BirthDate = DateTime.Parse(child.BirthDate),
                    Name = child.Name,
                    UserGuid = Guid.Parse(child.UserGUID)
                });
            }

            return children;
        }
    }
}
