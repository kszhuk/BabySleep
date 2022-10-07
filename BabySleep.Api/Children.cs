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
        [LambdaFunction()]
        [HttpApi(LambdaHttpMethod.Get, "/getchildren/{userGuid}/")]
        public List<Child> GetChildren(string userGuid)
        {
            var children = new List<Child>();

            var contextDb = DynamoDbContextHelper.GetDynamoDbContext();

            var childSearch = contextDb.ScanAsync<DdbModels.Child>(
              new[] {
            new ScanCondition
              (
                nameof(DdbModels.Child.UserGUID),
                ScanOperator.Equal,
                userGuid.ToUpper()
              )
              }
            );

            var resultChildren = childSearch.GetRemainingAsync().Result;

            foreach (var child in resultChildren)
            {
                children.Add(new Child()
                {
                    ChildGuid = Guid.Parse(child.ChildGUID),
                    BirthDate = DateTime.Parse(child.BirthDate),
                    Name = child.Name,
                    UserGuid = Guid.Parse(userGuid)
                });
            }

            return children;
        }
    }
}
