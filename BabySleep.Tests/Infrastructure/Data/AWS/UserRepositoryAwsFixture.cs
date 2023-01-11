using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using BabySleep.Api.Helpers;
using BabySleep.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabySleep.Tests.Infrastructure.Data.AWS
{
    //1. Install docker
    //2.  docker pull amazon/dynamodb-local
    //3. docker run -p 8000:8000 amazon/dynamodb-local
    public class UserRepositoryAwsFixture : IDisposable
    {
        public UserRepositoryAwsFixture()
        {
            //var clientConfig = new AmazonDynamoDBConfig { ServiceURL = "http://localhost:8000" };

            //var dynamoDbClient = new AmazonDynamoDBClient(clientConfig);
            //DynamoDbContextHelper.GetDynamoDbContext(dynamoDbClient);
        }
        public void Dispose()
        {
            //var contextDb = DynamoDbContextHelper.GetDynamoDbContext();

            //var generator = new GetUsersGenerator();
            //var usersList = new List<string>();
            //foreach (var user in generator.GetTestUsers())
            //{
            //    usersList.Add(user.Item1);
            //}

            //var userQuery = contextDb.ScanAsync<Api.DdbModels.Users>(new[] {
            //        new ScanCondition
            //        (
            //            nameof(Api.DdbModels.Users.Email),
            //            ScanOperator.In,
            //            usersList.Select(x => (object)x).ToArray()
            //        )
            //  }
            //    );

            //var resultUsers = userQuery.GetRemainingAsync().Result;

            //var sleepBatch = contextDb.CreateBatchWrite<Api.DdbModels.Users>();

            //foreach (var user in resultUsers)
            //{
            //    sleepBatch.AddDeleteKey(user.Email, user.UserGuid);
            //}
            //sleepBatch.ExecuteAsync().Wait();
        }
    }
}
