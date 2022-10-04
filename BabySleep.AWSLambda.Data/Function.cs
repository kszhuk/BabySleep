using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.Core;
using BabySleep.AWS.Common.Models;
using BabySleep.AWSLambda.Data.Helpers;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace BabySleep.AWSLambda.Data;

public class Function
{

    /// <summary>
    /// Returns all children for user
    /// </summary>
    /// <param name="userGuid"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    [LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
    public async Task<List<Child>> GetChildrenHandler(Guid userGuid, ILambdaContext context)
    {
        var children = new List<Child>();

        //var userGuidTest = new Guid("CDAFE18A-09E4-4AFF-9896-21A9DD17FC9F");//"BD736AF9-A4B6-4FC4-9883-3B7A33E627A2"

        var contextDb = DynamoDbContextHelper.GetDynamoDbContext();

        var childSearch = contextDb.ScanAsync<DdbModels.Child>(
          new[] {
            new ScanCondition
              (
                nameof(DdbModels.Child.UserGUID),
                ScanOperator.Equal,
                userGuid.ToString().ToUpper()
              )
          }
        );

        var resultChildren = await childSearch.GetRemainingAsync();

        foreach(var child in resultChildren)
        {
            children.Add(new Child()
            {
                ChildGuid = Guid.Parse(child.ChildGUID),
                BirthDate = DateTime.Parse(child.BirthDate),
                Name = child.Name,
                UserGuid = userGuid
            });
        }

        return children;
    }
}
