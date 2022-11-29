using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Annotations;
using BabySleep.Api.Helpers;
using BabySleep.AWS.Common.Models;
using BabySleep.Common.Helpers;
using Newtonsoft.Json;

namespace BabySleep.Api
{
    public class Sleeps
    {
        /// <summary>
        /// Returns all sleeps for child
        /// </summary>
        /// <param name="childGuid"></param>
        /// <returns>Sleeps for child</returns>
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

            var sleepSearch = contextDb.ScanAsync<DdbModels.Sleeps>(new[] {
                    new ScanCondition
                    (
                        nameof(DdbModels.Sleeps.ChildGuid),
                        ScanOperator.Equal,
                        childGuid.ToUpper()
                    ),
                    new ScanCondition
                    (
                        nameof(DdbModels.Sleeps.StartTime),
                        ScanOperator.Between,
                        previousDate,
                        nextDate
                    ),
                    new ScanCondition
                    (
                        nameof(DdbModels.Sleeps.EndTime),
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
        /// Returns sleep by sleepGuid
        /// </summary>
        /// <param name="sleepGuid"></param>
        /// <returns>sleep by guid</returns>
        [LambdaFunction(Name = "GetSleep")]
        [HttpApi(LambdaHttpMethod.Get, "/getsleep/{sleepGuid}/")]
        public Sleep GetSleep(string sleepGuid)
        {
            var contextDb = DynamoDbContextHelper.GetDynamoDbContext();
            var sleepQuery = contextDb.ScanAsync<DdbModels.Sleeps>(new[] {
                    new ScanCondition
                    (
                        nameof(DdbModels.Sleeps.SleepGuid),
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

        /// <summary>
        /// Adds new sleep
        /// </summary>
        /// <param name="sleep"></param>
        /// <returns></returns>
        [LambdaFunction(Name = "AddSleep")]
        [HttpApi(LambdaHttpMethod.Post, "/addSleep/{sleep}/")]
        public void AddSleep(string sleep)
        {
            var stringParsed = JsonConvert.DeserializeObject<Sleep>(sleep);
            var dbClient = DynamoDbContextHelper.GetAmazonDynamoDBClient();

            var putItemRequest = new PutItemRequest
            {
                TableName = nameof(DdbModels.Sleeps),
                Item = new Dictionary<string, AttributeValue>
                {
                    {
                      nameof(DdbModels.Sleeps.ChildGuid),
                      new AttributeValue(stringParsed.ChildGuid.ToString().ToUpper())
                    },
                    {
                      nameof(DdbModels.Sleeps.SleepGuid),
                      new AttributeValue(Guid.NewGuid().ToString().ToUpper())
                    },
                    {
                      nameof(DdbModels.Sleeps.AwakeningCount),
                      new AttributeValue{N = stringParsed.AwakeningCount.ToString() }
                    },
                    {
                      nameof(DdbModels.Sleeps.StartTime),
                      new AttributeValue(stringParsed.StartTime.ToString("yyyy-MM-ddTHH:mm:ss"))
                    },
                    {
                      nameof(DdbModels.Sleeps.EndTime),
                      new AttributeValue(stringParsed.EndTime.ToString("yyyy-MM-ddTHH:mm:ss"))
                    },
                    {
                      nameof(DdbModels.Sleeps.FallAsleepTime),
                      new AttributeValue{N = stringParsed.FallAsleepTime.ToString() }
                    },
                    {
                      nameof(DdbModels.Sleeps.FeedingCount),
                      new AttributeValue{N = stringParsed.FeedingCount.ToString() }
                    },
                    {
                      nameof(DdbModels.Sleeps.Quality),
                      new AttributeValue{N = stringParsed.Quality.ToString() }
                    },
                    {
                      nameof(DdbModels.Sleeps.SleepPlace),
                      new AttributeValue{N = stringParsed.SleepPlace.ToString() }
                    },
                }
            };

            var response = dbClient.PutItemAsync(putItemRequest).Result;
        }

        /// <summary>
        /// Updates sleep
        /// </summary>
        /// <param name="sleep"></param>
        /// <returns></returns>
        [LambdaFunction(Name = "UpdateSleep")]
        [HttpApi(LambdaHttpMethod.Put, "/updateSleep/{sleep}/")]
        public void UpdateSleep(string sleep)
        {
            var stringParsed = JsonConvert.DeserializeObject<Sleep>(sleep);

            var contextDb = DynamoDbContextHelper.GetDynamoDbContext();

            var sleepQuery = contextDb.ScanAsync<DdbModels.Sleeps>(new[] {
                    new ScanCondition
                    (
                        nameof(DdbModels.Sleeps.SleepGuid),
                        ScanOperator.Equal,
                        stringParsed.SleepGuid.ToString().ToUpper()
                    )
              }
            );

            var resultSleep = sleepQuery.GetRemainingAsync().Result.FirstOrDefault();

            if (resultSleep == null)
            {
                return;
            }

            resultSleep.SleepPlace = stringParsed.SleepPlace;
            resultSleep.FallAsleepTime = stringParsed.FallAsleepTime;
            resultSleep.AwakeningCount = stringParsed.AwakeningCount;
            resultSleep.EndTime = stringParsed.EndTime.ToString("yyyy-MM-ddTHH:mm:ss");
            resultSleep.FeedingCount = stringParsed.FeedingCount;
            resultSleep.Quality = stringParsed.Quality;
            resultSleep.StartTime = stringParsed.StartTime.ToString("yyyy-MM-ddTHH:mm:ss");

            contextDb.SaveAsync(resultSleep).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Deletes sleep by guid
        /// </summary>
        /// <param name="sleepGuid"></param>
        /// <returns></returns>
        [LambdaFunction(Name = "DeleteSleep")]
        [HttpApi(LambdaHttpMethod.Delete, "/updateSleep/{sleepGuid}/")]
        public void DeleteSleep(string sleepGuid)
        {
            var contextDb = DynamoDbContextHelper.GetDynamoDbContext();
            var sleepQuery = contextDb.ScanAsync<DdbModels.Sleeps>(new[] {
                    new ScanCondition
                    (
                        nameof(DdbModels.Sleeps.SleepGuid),
                        ScanOperator.Equal,
                        sleepGuid.ToUpper()
                    )
              }
            );

            var resultSleep = sleepQuery.GetRemainingAsync().Result.FirstOrDefault();

            if(resultSleep == null)
            {
                return;
            }

            contextDb.DeleteAsync(resultSleep).GetAwaiter().GetResult();
        }
    }
}
