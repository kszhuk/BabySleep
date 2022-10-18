using Amazon.DynamoDBv2.DataModel;

namespace BabySleep.Api.DdbModels
{
    [DynamoDBTable("Users")]
    internal class User
    {
        [DynamoDBHashKey("Email")]
        public string Email { get; set; }

        [DynamoDBRangeKey("UserGuid")]
        public string UserGuid { get; set; }
    }
}
