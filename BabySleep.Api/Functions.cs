using Amazon.Lambda.Core;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.APIGatewayEvents;
using System.Net;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace BabySleep.Api
{
    /// <summary>
    /// A collection of sample Lambda functions that provide a REST api for doing simple math calculations. 
    /// </summary>
    public class Functions
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Functions()
        {
        }

        /// <summary>
        /// Root route that provides information about the other requests that can be made.
        ///
        /// PackageType is currently required to be set to LambdaPackageType.Image till the upcoming .NET 6 managed
        /// runtime is available. Once the .NET 6 managed runtime is available PackageType will be optional and will
        /// default to Zip.
        /// </summary>
        /// <returns>API descriptions.</returns>
        [LambdaFunction()]
        [HttpApi(LambdaHttpMethod.Get, "/")]
        public string Default()
        {
            var docs = @"BabySleep";
            return docs;
        }
    }
}
