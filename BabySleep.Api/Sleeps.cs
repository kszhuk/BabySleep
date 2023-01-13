using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Core;
using BabySleep.Api.Helpers;
using BabySleep.AWS.Common.Models;
using BabySleep.Common.Exceptions.Sleep;
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
            var sleeps = new List<Sleep>();

            try
            {
                var date = DateTime.Now;
                DateTime.TryParse(currentDate, out date);

                var previousDate = DateTimeHelper.FormatEmptyDateAws(date.AddDays(-1));
                var nextDate = DateTimeHelper.FormatEndDateAws(date.AddDays(1));

                var resultSleeps = GetSleepsDdb(childGuid, previousDate, nextDate);

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

                sleeps = sleeps.Where(s => DateTimeHelper.AreEqualDates(date, s.StartTime) || DateTimeHelper.AreEqualDates(date, s.EndTime)).ToList();
            }
            catch (Exception ex)
            {
                LambdaLogger.Log(string.Format("Failed Sleeps.GetSleeps by {0}, {1}: {2}", childGuid, currentDate, ex.Message));
            }

            return sleeps;
        }

        /// <summary>
        /// Returns all sleeps for child between dates
        /// </summary>
        /// <param name="childGuid"></param>
        /// <returns>Sleeps for child</returns>
        [LambdaFunction(Name = "GetSleepsDates")]
        [HttpApi(LambdaHttpMethod.Get, "/getsleepsdates/{childGuid}/{startDate}/{endDate}/")]
        public List<Sleep> GetSleepsDates(string childGuid, string startDate, string endDate)
        {
            var sleeps = new List<Sleep>();

            try
            {
                var dateStart = DateTime.Now;
                DateTime.TryParse(startDate, out dateStart);

                var dateEnd = DateTime.Now;
                DateTime.TryParse(endDate, out dateEnd);

                var previousDate = DateTimeHelper.FormatEmptyDateAws(dateStart.AddDays(-1));
                var nextDate = DateTimeHelper.FormatEndDateAws(dateEnd.AddDays(1));

                var resultSleeps = GetSleepsDdb(childGuid, previousDate, nextDate);

                foreach (var sleep in resultSleeps)
                {
                    var startTime = DateTime.Parse(sleep.StartTime);
                    var endTime = DateTime.Parse(sleep.EndTime);

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
            catch (Exception ex)
            {
                LambdaLogger.Log(string.Format("Failed Sleeps.GetSleeps by {0}, {1}, {2}: {3}", childGuid, startDate, endDate, ex.Message));
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
            try
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
            }
            catch (Exception ex)
            {
                LambdaLogger.Log(string.Format("Failed Sleeps.GetSleep by {0}: {1}", sleepGuid, ex.Message));
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
            try
            {
                var sleepParsed = JsonConvert.DeserializeObject<Sleep>(sleep);

                if (sleepParsed == null)
                {
                    return;
                }

                ValidateSleepTime(sleepParsed.StartTime, sleepParsed.EndTime, sleepParsed.SleepGuid.ToString(), sleepParsed.ChildGuid.ToString());

                var dbClient = DynamoDbContextHelper.GetAmazonDynamoDBClient();

                var putItemRequest = new PutItemRequest
                {
                    TableName = nameof(DdbModels.Sleeps),
                    Item = new Dictionary<string, AttributeValue>
                {
                    {
                      nameof(DdbModels.Sleeps.ChildGuid),
                      new AttributeValue(sleepParsed.ChildGuid.ToString().ToUpper())
                    },
                    {
                      nameof(DdbModels.Sleeps.SleepGuid),
                      new AttributeValue(Guid.NewGuid().ToString().ToUpper())
                    },
                    {
                      nameof(DdbModels.Sleeps.AwakeningCount),
                      new AttributeValue{N = sleepParsed.AwakeningCount.ToString() }
                    },
                    {
                      nameof(DdbModels.Sleeps.StartTime),
                      new AttributeValue(sleepParsed.StartTime.ToString("yyyy-MM-ddTHH:mm:ss"))
                    },
                    {
                      nameof(DdbModels.Sleeps.EndTime),
                      new AttributeValue(sleepParsed.EndTime.ToString("yyyy-MM-ddTHH:mm:ss"))
                    },
                    {
                      nameof(DdbModels.Sleeps.FallAsleepTime),
                      new AttributeValue{N = sleepParsed.FallAsleepTime.ToString() }
                    },
                    {
                      nameof(DdbModels.Sleeps.FeedingCount),
                      new AttributeValue{N = sleepParsed.FeedingCount.ToString() }
                    },
                    {
                      nameof(DdbModels.Sleeps.Quality),
                      new AttributeValue{N = sleepParsed.Quality.ToString() }
                    },
                    {
                      nameof(DdbModels.Sleeps.SleepPlace),
                      new AttributeValue{N = sleepParsed.SleepPlace.ToString() }
                    },
                }
                };

                var response = dbClient.PutItemAsync(putItemRequest).Result;
            }
            catch (Exception ex)
            {
                LambdaLogger.Log(string.Format("Failed Sleeps.AddSleep by {0}: {1}", sleep, ex.Message));
            }
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
            try
            {
                var sleepParsed = JsonConvert.DeserializeObject<Sleep>(sleep);

                if (sleepParsed == null)
                {
                    return;
                }

                ValidateSleepTime(sleepParsed.StartTime, sleepParsed.EndTime, sleepParsed.SleepGuid.ToString(), sleepParsed.ChildGuid.ToString());

                var contextDb = DynamoDbContextHelper.GetDynamoDbContext();

                var sleepQuery = contextDb.ScanAsync<DdbModels.Sleeps>(new[] {
                    new ScanCondition
                    (
                        nameof(DdbModels.Sleeps.SleepGuid),
                        ScanOperator.Equal,
                        sleepParsed.SleepGuid.ToString().ToUpper()
                    )
              }
                );

                var resultSleep = sleepQuery.GetRemainingAsync().Result.FirstOrDefault();

                if (resultSleep == null)
                {
                    return;
                }

                resultSleep.SleepPlace = sleepParsed.SleepPlace;
                resultSleep.FallAsleepTime = sleepParsed.FallAsleepTime;
                resultSleep.AwakeningCount = sleepParsed.AwakeningCount;
                resultSleep.EndTime = sleepParsed.EndTime.ToString("yyyy-MM-ddTHH:mm:ss");
                resultSleep.FeedingCount = sleepParsed.FeedingCount;
                resultSleep.Quality = sleepParsed.Quality;
                resultSleep.StartTime = sleepParsed.StartTime.ToString("yyyy-MM-ddTHH:mm:ss");

                contextDb.SaveAsync(resultSleep).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                LambdaLogger.Log(string.Format("Failed Sleeps.UpdateSleep by {0}: {1}", sleep, ex.Message));
            }
        }

        /// <summary>
        /// Deletes sleep by guid
        /// </summary>
        /// <param name="sleepGuid"></param>
        /// <returns></returns>
        [LambdaFunction(Name = "DeleteSleep")]
        [HttpApi(LambdaHttpMethod.Delete, "/deleteSleep/{sleepGuid}/")]
        public void DeleteSleep(string sleepGuid)
        {
            try
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

                if (resultSleep == null)
                {
                    return;
                }

                contextDb.DeleteAsync(resultSleep).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                LambdaLogger.Log(string.Format("Failed Sleeps.DeleteSleep by {0}: {1}", sleepGuid, ex.Message));
            }
        }

        private void ValidateSleepTime(DateTime startDate, DateTime endDate, string sleepGuid, string childGuid)
        {
            var contextDb = DynamoDbContextHelper.GetDynamoDbContext();

            var sleepQuery = contextDb.QueryAsync<DdbModels.Sleeps>(childGuid.ToUpper());

            var resultSleeps = sleepQuery.GetRemainingAsync().Result.Where(s => s.SleepGuid != sleepGuid.ToUpper());

            var sleeps = new List<Sleep>();
            foreach (var sleep in resultSleeps)
            {
                var startTime = DateTime.Parse(sleep.StartTime);
                var endTime = DateTime.Parse(sleep.EndTime);

                sleeps.Add(new Sleep()
                {
                    StartTime = startTime,
                    EndTime = endTime
                });
            }

            var startIntersectionSleeps = sleeps.Where(s => s.StartTime >= startDate && s.StartTime <= endDate).ToList();
            var endIntersectionSleeps = sleeps.Where(s => s.EndTime >= startDate && s.EndTime <= endDate).ToList();
            var wholeIntersectionSleeps = sleeps.Where(s => s.StartTime <= startDate && s.EndTime >= endDate).ToList();
            if (startIntersectionSleeps.Any() || endIntersectionSleeps.Any() || wholeIntersectionSleeps.Any())
            {
                throw new SleepAlreadyExistsException();
            }
        }

        private List<DdbModels.Sleeps> GetSleepsDdb(string childGuid, string previousDate, string nextDate)
        {
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

            return resultSleeps;
        }
    }
}
