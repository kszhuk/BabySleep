using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using BabySleep.Infrastructure.Data.RepositoriesAws;
using BabySleep.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BabySleep.Tests.Infrastructure.Data.AWS
{
    public class UserRepositoryAwsTests : IClassFixture<UserRepositoryAwsFixture>
    {

    //    [Theory]
    //    [ClassData(typeof(GetUsersGenerator))]
    //    public void GetUser(string email, string userGuid)
    //    {
    //        //if (userGuid != string.Empty)
    //        //{
    //        //    var dbClient = DynamoDbContextHelper.GetAmazonDynamoDBClient();

    //        //    var putItemRequest = new PutItemRequest
    //        //    {
    //        //        TableName = nameof(Api.DdbModels.Users),
    //        //        Item = new Dictionary<string, AttributeValue>
    //        //    {
    //        //        {
    //        //          nameof(Api.DdbModels.Users.Email),
    //        //          new AttributeValue(email)
    //        //        },
    //        //        {
    //        //          nameof(Api.DdbModels.Users.UserGuid),
    //        //          new AttributeValue(userGuid)
    //        //        }
    //        //    }
    //        //    };

    //        //    var response = dbClient.PutItemAsync(putItemRequest).Result;
    //        //}

    //        //var repository = new UserRepositoryAws();
    //        //var userGuidDb = repository.GetUserGuid(email);
    //        //Assert.Equal(userGuid, userGuidDb);
    //    }
    //}

    //public class GetUsersGenerator : TheoryData<string, string>
    //{
    //    public GetUsersGenerator()
    //    {
    //        foreach (var user in GetTestUsers())
    //        {
    //            Add(user.Item1, user.Item2);
    //        }
    //    }

    //    public List<Tuple<string, string>> GetTestUsers()
    //    {
    //        var users = new List<Tuple<string, string>>
    //            {
    //                Tuple.Create("test@test.com", Guid.NewGuid().ToString()),
    //                Tuple.Create("testEmpty@test.com", string.Empty)
    //            };

    //        return users;
    //    }
    }
}
