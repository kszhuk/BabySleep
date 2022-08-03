using Amazon.Lambda.Core;

namespace BabySleep.AWSLambda.SyncData;

public class Function
{

    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    [LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
    public string FunctionHandler(MyData data, ILambdaContext context)
    {
        return $"Hi, {data.Name.ToUpper()}";
    }

    public class MyData
    {
        public string Name { get; set; }    
    }
}
