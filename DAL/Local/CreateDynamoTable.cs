using System.Diagnostics.CodeAnalysis;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace Local
{
    [ExcludeFromCodeCoverage]
    public static class CreateDynamoTable
    {
        public async static Task CreateTable(this IAmazonDynamoDB dbClient)
        {
            var tableName = "SimplifyStreaming";

            var attributeDefinitions = new List<AttributeDefinition>()
            {
                {new AttributeDefinition{
                    AttributeName = "PK",
                    AttributeType = "S"}},
                {new AttributeDefinition{
                    AttributeName = "SK",
                    AttributeType = "S"}},
                {new AttributeDefinition{
                    AttributeName = "GSI_1",
                    AttributeType = "S"}},
            };

            var tableKeySchema = new List<KeySchemaElement>()
            {
                {new KeySchemaElement {
                    AttributeName = "PK",
                    KeyType = "HASH"}},  //Partition key
                {new KeySchemaElement {
                    AttributeName = "SK",
                    KeyType = "RANGE"}  //Sort key
                }
            };

            var GSI_1 = CreateGSI("GSI_1_INDEX", "GSI_1");

            CreateTableRequest createTableRequest = new CreateTableRequest
            {
                TableName = tableName,
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = (long)20,
                    WriteCapacityUnits = (long)10
                },
                AttributeDefinitions = attributeDefinitions,
                KeySchema = tableKeySchema,
                GlobalSecondaryIndexes = { GSI_1 }
            };

            try
            {
                await dbClient.CreateTableAsync(createTableRequest);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        private static GlobalSecondaryIndex CreateGSI(string name, string indexColumnName)
        {
            var gsiIndex = new GlobalSecondaryIndex
            {
                IndexName = name,
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = (long)10,
                    WriteCapacityUnits = (long)5
                },
                Projection = new Projection { ProjectionType = "ALL" }
            };

            var indexKeySchema = new List<KeySchemaElement> {
                {new KeySchemaElement { AttributeName = indexColumnName, KeyType = "HASH"}},
                {new KeySchemaElement { AttributeName = "SK", KeyType = "RANGE"}}
            };

            gsiIndex.KeySchema = indexKeySchema;

            return gsiIndex;
        }
    }
}
