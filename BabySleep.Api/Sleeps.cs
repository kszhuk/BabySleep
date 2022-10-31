using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.Annotations;
using BabySleep.Api.Helpers;
using BabySleep.AWS.Common.Models;
using BabySleep.Common.Helpers;

namespace BabySleep.Api
{
    public class Sleeps
    {
        /// <summary>
        /// Returns all sleeps for child
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        [LambdaFunction(Name = "GetSleeps")]
        [HttpApi(LambdaHttpMethod.Get, "/getsleeps/{childGuid}/{currentDate}/")]
        public List<Sleep> GetSleeps(string childGuid, string currentDate)
        {
            var date = DateTime.Now;
            DateTime.TryParse(currentDate, out date);

            var previousDate = DateTimeHelper.FormatEmptyDate(date.AddDays(-1));
            var nextDate = DateTimeHelper.FormatEmptyDate(date.AddDays(2));

            var sleeps = new List<Sleep>();

            var contextDb = DynamoDbContextHelper.GetDynamoDbContext();

            var sleepSearch = contextDb.ScanAsync<DdbModels.Sleep>(new[] {
                    new ScanCondition
                    (
                        nameof(DdbModels.Sleep.ChildGuid),
                        ScanOperator.Equal,
                        childGuid.ToUpper()
                    ),
                    new ScanCondition
                    (
                        nameof(DdbModels.Sleep.StartTime),
                        ScanOperator.Between,
                        previousDate.ToShortDateString(),
                        nextDate.ToString()
                    ),
                    new ScanCondition
                    (
                        nameof(DdbModels.Sleep.EndTime),
                        ScanOperator.Between,
                        previousDate.ToShortDateString(),
                        nextDate.ToString()
                    )
              }
            );

            var resultSleeps = sleepSearch.GetRemainingAsync().Result;

            foreach (var sleep in resultSleeps)
            {
                var startTime = DateTime.Parse(sleep.StartTime);
                var endTime = DateTime.Parse(sleep.EndTime);

                if (DateTimeHelper.AreEqualDates(date, startTime) || DateTimeHelper.AreEqualDates(date, endTime))
                {
                    sleeps.Add(new Sleep()
                    {
                        ChildGuid = Guid.Parse(sleep.ChildGuid),
                        SleepGuid = Guid.Parse(sleep.SleepGuid),
                        AwakeningCount = sleep.AwakeningCount,
                        Status = sleep.Status,
                        SleepPlace = sleep.SleepPlace,
                        FeedingCount = sleep.FeedingCount,
                        FallAsleepTime = sleep.FallAsleepTime,
                        Note = sleep.Note,
                        Quality = sleep.Quality,
                        StartTime = startTime,
                        EndTime = endTime
                    });
                }
            }

            return sleeps;
        }
    }
}
