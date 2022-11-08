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

            var previousDate = DateTimeHelper.FormatEmptyDateAws(date.AddDays(-1));
            var nextDate = DateTimeHelper.FormatEndDateAws(date.AddDays(1));

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
                        previousDate,
                        nextDate
                    ),
                    new ScanCondition
                    (
                        nameof(DdbModels.Sleep.EndTime),
                        ScanOperator.Between,
                        previousDate,
                        nextDate
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
                        Quality = sleep.Quality,
                        StartTime = startTime,
                        EndTime = endTime
                    });
                }
            }

            return sleeps;
        }

        /// <summary>
        /// Returns sleepby sleepGuid
        /// </summary>
        /// <param name="sleepGuid"></param>
        /// <returns></returns>
        [LambdaFunction(Name = "GetSleep")]
        [HttpApi(LambdaHttpMethod.Get, "/getsleep/{sleepGuid}/")]
        public Sleep GetSleep(string sleepGuid)
        {
            var contextDb = DynamoDbContextHelper.GetDynamoDbContext();
            var sleepQuery = contextDb.ScanAsync<DdbModels.Sleep>(new[] {
                    new ScanCondition
                    (
                        nameof(DdbModels.Sleep.SleepGuid),
                        ScanOperator.Equal,
                        sleepGuid.ToUpper()
                    )
              }
            );

            var resultSleep = sleepQuery.GetRemainingAsync().Result;

            if (resultSleep.Any())
            {
                var firstSleep = resultSleep.First();
                var sleep = new Sleep()
                {
                    AwakeningCount = firstSleep.AwakeningCount,
                    ChildGuid = Guid.Parse(firstSleep.ChildGuid),
                    SleepGuid = Guid.Parse(firstSleep.SleepGuid),
                    StartTime = DateTime.Parse(firstSleep.StartTime),
                    EndTime = DateTime.Parse(firstSleep.EndTime),
                    Status = firstSleep.Status,
                    FallAsleepTime = firstSleep.FallAsleepTime,
                    FeedingCount = firstSleep.FeedingCount,
                    Quality = firstSleep.Quality,
                    SleepPlace = firstSleep.SleepPlace
                };
                return sleep;
            }

            return new Sleep();
        }
    }
}
