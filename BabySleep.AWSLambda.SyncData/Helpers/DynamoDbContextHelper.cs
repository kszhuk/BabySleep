using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

namespace BabySleep.AWSLambda.SyncData.Helpers
{
    internal class DynamoDbContextHelper
    {
        private static DynamoDBContext context;
        public static DynamoDBContext GetDynamoDbContext()
        {
            if (context == null)
            {
                var clientConfig = new AmazonDynamoDBClient("AKIAYEEPPMAQHJZKUYHD", "1Bge87vU9hgLM0nmUaEfS9JC5+GcSdT6q163Zwu6",
                    RegionEndpoint.EUWest1);

                context = new DynamoDBContext(clientConfig);
            }

            return context;
        }
    }
}
