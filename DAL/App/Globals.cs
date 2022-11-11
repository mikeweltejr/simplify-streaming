using System.Diagnostics.CodeAnalysis;
using Amazon.DynamoDBv2.DataModel;

namespace DynamoDB.DAL.App
{
    [ExcludeFromCodeCoverage]
    public static class Globals
    {
        public static readonly DynamoDBOperationConfig DB_CONFIG = new DynamoDBOperationConfig { OverrideTableName="SimplifyStreaming" };
        public const string GSI_1_INDEX = "GSI_1_INDEX";
        public const string GSI_NAME = "GSI_1";
    }
}
