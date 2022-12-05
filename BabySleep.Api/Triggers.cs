using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.Annotations;
using BabySleep.Api.Helpers;
using BabySleep.Common.Helpers;

namespace BabySleep.Api
{
    public class Triggers
    {
        /// <summary>
        /// Deletes sleep by guid
        /// </summary>
        /// <param name="sleepGuid"></param>
        /// <returns></returns>
        [LambdaFunction(Name = "DeleteOldData")]
        [HttpApi(LambdaHttpMethod.Delete, "/deleteOldData/")]
        public void DeleteOldData()
        {
            var contextDb = DynamoDbContextHelper.GetDynamoDbContext();

            var previousDate = DateTimeHelper.FormatEmptyDateAws(DateTime.Now.AddDays(-7));

            var sleepQuery = contextDb.ScanAsync<DdbModels.Sleeps>(new[] {
                    new ScanCondition
                    (
                        nameof(DdbModels.Sleeps.EndTime),
                        ScanOperator.LessThanOrEqual,
                        previousDate
                    )
              }
            );

            var resultSleeps = sleepQuery.GetRemainingAsync().Result;

            var leftItemsCount = resultSleeps.Count;
            var batchCount = 0;
            var sleepBatch = contextDb.CreateBatchWrite<DdbModels.Sleeps>();

            foreach (var sleep in resultSleeps)
            {
                sleepBatch.AddDeleteKey(sleep.ChildGuid, sleep.SleepGuid);
                batchCount++;

                leftItemsCount--;

                if (batchCount == 25 || leftItemsCount < 1)
                {
                    sleepBatch.ExecuteAsync().Wait();
                    batchCount = 0;
                    sleepBatch = contextDb.CreateBatchWrite<DdbModels.Sleeps>();
                }
            }
        }
    }
}
