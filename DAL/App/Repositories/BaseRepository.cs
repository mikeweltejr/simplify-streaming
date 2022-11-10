using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace DynamoDB.DAL.App.Repositories
{
    public class BaseRepository
    {
        public static readonly DynamoDBOperationConfig DB_CONFIG = new DynamoDBOperationConfig { OverrideTableName="SimplifyStreaming" };
        private readonly IDynamoDBContext _db;

        public BaseRepository(IDynamoDBContext db)
        {
            _db = db;
        }

        public async Task<List<T>> GetDynamoQueryResults<T>(string pkName, string pkValue, string skValue)
        {
            return await GetDynamoQueryResults<T>(null, pkName, pkValue, skValue);
        }

        public async Task<List<T>> GetDynamoQueryResults<T>(string? indexName, string pkName, string pkValue, string skValue)
        {
            var qOp = new QueryOperationConfig();
            if (indexName != null) qOp.IndexName = indexName;
            var keyExpression = new Expression();
            keyExpression.ExpressionStatement = $"{pkName} = :pk and begins_with(SK, :sk)";
            keyExpression.ExpressionAttributeValues = new Dictionary<string, DynamoDBEntry>();
            keyExpression.ExpressionAttributeValues.Add(":pk", pkValue);
            keyExpression.ExpressionAttributeValues.Add(":sk", skValue);
            qOp.KeyExpression = keyExpression;
            var queryResult = _db.FromQueryAsync<T>(qOp, DB_CONFIG);
            var results = await queryResult.GetRemainingAsync();

            return results;
        }
    }
}
