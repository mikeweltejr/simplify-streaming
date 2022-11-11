using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;

namespace DynamoDB.DAL.Test
{
    public static class Configuration
    {
        public const string DYNAMO_SERVICE_URL = "http://localhost:8001";

        public static IDynamoDBContext GetDBContext()
        {
            var config = new AmazonDynamoDBConfig();
            config.ServiceURL = DYNAMO_SERVICE_URL;
            var credentials = new BasicAWSCredentials(
                accessKey: "Test",
                secretKey: "Test"
            );

            var client = new AmazonDynamoDBClient(credentials, config);
            var dynamoDb = new DynamoDBContext(client);

            return dynamoDb;
        }
    }
}
