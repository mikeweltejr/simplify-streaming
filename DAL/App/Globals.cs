using System.Diagnostics.CodeAnalysis;
using Amazon.DynamoDBv2.DataModel;

namespace DynamoDB.DAL.App
{
    [ExcludeFromCodeCoverage]
    public static class Globals
    {
        public static readonly DynamoDBOperationConfig DB_CONFIG = new DynamoDBOperationConfig { OverrideTableName="SimplifyStreaming" };
    }
}
