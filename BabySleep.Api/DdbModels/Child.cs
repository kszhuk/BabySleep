using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabySleep.Api.DdbModels
{
    [DynamoDBTable("Children")]
    internal class Child
    {
        [DynamoDBHashKey("UserGUID")]
        public string UserGUID { get; set; }

        [DynamoDBRangeKey("ChildGUID")]
        public string ChildGUID { get; set; }

        [DynamoDBProperty("BirthDate")]
        public string BirthDate { get; set; }

        [DynamoDBProperty("Name")]
        public string Name { get; set; }
    }
}
