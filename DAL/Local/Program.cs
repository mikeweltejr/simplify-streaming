using Amazon.DynamoDBv2;
using Amazon.Runtime;

namespace Local
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if(args.Length < 1)
            {
                Console.Error.Write("Must specify port for DynamoDB (API=8000, UnitTest=8001, IntegrationTest=8002)");
                Environment.Exit(1);
            }

            var port = args[0];

            var dbConfig = new AmazonDynamoDBConfig();
            var credentials = new BasicAWSCredentials(accessKey: "Test", secretKey: "Test");
            dbConfig.ServiceURL = $"http://localhost:{port.ToString()}";
            var dbClient = new AmazonDynamoDBClient(credentials, dbConfig);

            await CreateDynamoTable.CreateTable(dbClient);
        }
    }
}
