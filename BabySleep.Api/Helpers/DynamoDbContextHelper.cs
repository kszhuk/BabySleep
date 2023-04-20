using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

namespace BabySleep.Api.Helpers
{
    public class DynamoDbContextHelper : IDynamoDbContextHelper
    {
        private IConfiguration configuration;
        public DynamoDbContextHelper(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private DynamoDBContext context;
        public DynamoDBContext GetDynamoDbContext()
        {
            if (context == null)
            {
                var clientConfig = GetAmazonDynamoDBClient();

                context = new DynamoDBContext(clientConfig);
            }

            return context;
        }

        public AmazonDynamoDBClient GetAmazonDynamoDBClient()
        {
            var accessKey = configuration.GetValue<string>("Aws:AccessKey");
            var secretKey = configuration.GetValue<string>("Aws:SecretKey");

            return new AmazonDynamoDBClient(accessKey, secretKey,
                    RegionEndpoint.EUWest1);
        }
    }
}
