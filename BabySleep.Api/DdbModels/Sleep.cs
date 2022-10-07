using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabySleep.Api.DdbModels
{
    [DynamoDBTable("Sleeps")]
    internal class Sleep
    {
        [DynamoDBHashKey("ChildGuid")]
        public string ChildGuid { get; set; }

        [DynamoDBRangeKey("SleepGuid")]
        public string SleepGuid { get; set; }

        [DynamoDBProperty("StartTime")]
        public string StartTime { get; set; }
        [DynamoDBProperty("EndTime")]
        public string EndTime { get; set; }
        [DynamoDBProperty("Quality")]
        public short Quality { get; set; }
        [DynamoDBProperty("Note")]
        public string Note { get; set; }
        [DynamoDBProperty("FeedingCount")]
        public short FeedingCount { get; set; }
        [DynamoDBProperty("FallAsleepTime")]
        public short FallAsleepTime { get; set; }
        [DynamoDBProperty("AwakeningCount")]
        public short AwakeningCount { get; set; }
        [DynamoDBProperty("SleepPlace")]
        public short SleepPlace { get; set; }
        [DynamoDBProperty("Status")]
        public short Status { get; set; }
    }
}
