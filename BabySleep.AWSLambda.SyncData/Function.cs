using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.Core;
using BabySleep.AWS.Common.Models;
using BabySleep.AWSLambda.SyncData.Helpers;

namespace BabySleep.AWSLambda.SyncData;

public class Function
{

    ///// <summary>
    ///// A simple function that takes a string and does a ToUpper
    ///// </summary>
    ///// <param name="input"></param>
    ///// <param name="context"></param>
    ///// <returns></returns>
    //[LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
    //public string FunctionHandler(MyData data, ILambdaContext context)
    //{
    //    return $"Hi, {data.Name.ToUpper()}";
    //}

    //public class MyData
    //{
    //    public string Name { get; set; }    
    //}

    /// <summary>
    /// Returns all sleeps for user
    /// </summary>
    /// <param name="userGuid"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    [LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
    public async Task<List<Sleep>> GetSleepsHandler(Guid userGuid, ILambdaContext context)
    {
        var sleeps = new List<Sleep>();

        var userGuidTest = new Guid("CDAFE18A-09E4-4AFF-9896-21A9DD17FC9F");//"BD736AF9-A4B6-4FC4-9883-3B7A33E627A2"

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
        var childrenGuid = resultChildren.Select(c => c.ChildGUID).Select(x => (object)x).ToArray();

        if(!childrenGuid.Any())
        {
            return sleeps;
        }

        var sleepSearch = contextDb.ScanAsync<DdbModels.Sleep>(
          new[] {
            new ScanCondition
              (
                nameof(DdbModels.Sleep.ChildGuid),
                ScanOperator.In,
                childrenGuid
              )
          }
        );

        var resultSleeps = await sleepSearch.GetRemainingAsync();

        foreach (var sleep in resultSleeps)
        {
            sleeps.Add(new Sleep()
            {
                ChildGuid = Guid.Parse(sleep.ChildGuid),
                SleepGuid = Guid.Parse(sleep.SleepGuid),
                StartTime = DateTime.Parse(sleep.StartTime),
                EndTime = DateTime.Parse(sleep.EndTime),
                Quality = sleep.Quality,
                AwakeningCount = sleep.AwakeningCount,
                FallAsleepTime = sleep.FallAsleepTime,
                FeedingCount = sleep.FeedingCount,
                //Note = sleep.Note,
                SleepPlace = sleep.SleepPlace,
                Status = sleep.Status
            });
        }

        return sleeps;
    }
}
