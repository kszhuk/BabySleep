using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

namespace BabySleep.Api.Helpers
{
    public class DynamoDbContextHelper
    {
        private static DynamoDBContext context;
        public static DynamoDBContext GetDynamoDbContext()
        {
            if (context == null)
            {
                var clientConfig = GetAmazonDynamoDBClient();

                context = new DynamoDBContext(clientConfig);
            }

            return context;
        }

        public static AmazonDynamoDBClient GetAmazonDynamoDBClient()
        {
            return new AmazonDynamoDBClient("AKIAYEEPPMAQNU3HK77C", "D5n5+ZGtC6XLTFTHcucujCOMTAwbeWW+dSPti4AI",
                    RegionEndpoint.EUWest1);
        }
    }
}
