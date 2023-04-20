using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

namespace BabySleep.Api.Helpers
{
    public interface IDynamoDbContextHelper
    {
        DynamoDBContext GetDynamoDbContext();
        AmazonDynamoDBClient GetAmazonDynamoDBClient();
    }
}
