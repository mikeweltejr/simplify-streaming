using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using DynamoDB.DAL.App.Models;

namespace DynamoDB.DAL.App.Data
{
    [ExcludeFromCodeCoverage]
    public class SeedData
    {
        private readonly IDynamoDBContext _db;
        private const string SEEDED_TITLES = "SeededTitles";
        private const string SEEDED_STREAMING = "SeededStreaming";
        private Dictionary<String, String> _titleMap;

        public SeedData(IDynamoDBContext db)
        {
            _db = db;
            _titleMap = new Dictionary<string, string>();
        }

        public async Task SeedAllTitles()
        {
            List<GeneralBool>? seededTitles = await GetDynamoQueryResults<GeneralBool>("PK", SEEDED_TITLES, "true");

            var titles = await GetTitlesFromJson();
            if(titles == null) return;

            foreach(var title in titles)
            {
                if (title.Name == null || title.Id == null)
                {
                    // TODO: throw exception here, data is invalid at this point.
                    return;
                }

                _titleMap.Add(title.Name, title.Id);
            }

            if (seededTitles.Count <= 0)
            {
                var generalSeed = new GeneralBool(SEEDED_TITLES, "true");
                await _db.SaveAsync(generalSeed, Globals.DB_CONFIG);

                var batchWrite = _db.CreateBatchWrite<Title>(Globals.DB_CONFIG);
                batchWrite.AddPutItems(titles);
                await batchWrite.ExecuteAsync();
            }
        }

        public async Task SeedStreamingTitles()
        {
            List<GeneralBool>? seededTitles = await GetDynamoQueryResults<GeneralBool>("PK", SEEDED_STREAMING, "true");

            if (seededTitles.Count <= 0)
            {
                var generalSeed = new GeneralBool(SEEDED_STREAMING, "true");
                await _db.SaveAsync(generalSeed, Globals.DB_CONFIG);

                var titles = await GetServiceTitlesFromJson();
                if (titles == null) return;

                foreach(var title in titles)
                {
                    if (title.TitleName == null)
                    {
                        // TODO: Throw Exception will have bad data here
                        return;
                    }
                    title.TitleId = _titleMap[title.TitleName];

                    // TODO: Remove
                    // Console.WriteLine($"Title: {title.TitleName} TitleId: {title.TitleId} PK: {title.PK} SK: {title.SK}");
                }

                var batchWrite = _db.CreateBatchWrite<ServiceTitle>(Globals.DB_CONFIG);
                batchWrite.AddPutItems(titles);
                await batchWrite.ExecuteAsync();
            }
        }

        private async Task<List<Title>?> GetTitlesFromJson()
        {
            string jsonData = await File.ReadAllTextAsync("titles.json");
            List<Title>? titles = JsonSerializer.Deserialize<List<Title>>(jsonData);

            return titles;
        }

        private async Task<List<ServiceTitle>?> GetServiceTitlesFromJson()
        {
            var titles = new List<ServiceTitle>();
            string jsonData = await File.ReadAllTextAsync("hbo_max.json");
            List<ServiceTitle>? hboTitles = JsonSerializer.Deserialize<List<ServiceTitle>>(jsonData);

            if (hboTitles != null) titles.AddRange(hboTitles);

            jsonData = await File.ReadAllTextAsync("prime.json");
            List<ServiceTitle>? primeTitles = JsonSerializer.Deserialize<List<ServiceTitle>>(jsonData);

            if (primeTitles != null) titles.AddRange(primeTitles);

            jsonData = await File.ReadAllTextAsync("hard_copies.json");
            List<ServiceTitle>? discTitles = JsonSerializer.Deserialize<List<ServiceTitle>>(jsonData);

            return titles;
        }

        private async Task<List<T>> GetDynamoQueryResults<T>(string pkName, string pkValue, string skValue)
        {
            var qOp = new QueryOperationConfig();
            var keyExpression = new Expression();
            keyExpression.ExpressionStatement = $"{pkName} = :pk and begins_with(SK, :sk)";
            keyExpression.ExpressionAttributeValues = new Dictionary<string, DynamoDBEntry>();
            keyExpression.ExpressionAttributeValues.Add(":pk", pkValue);
            keyExpression.ExpressionAttributeValues.Add(":sk", skValue);
            qOp.KeyExpression = keyExpression;
            var queryResult = _db.FromQueryAsync<T>(qOp, Globals.DB_CONFIG);
            var results = await queryResult.GetRemainingAsync();

            return results;
        }
    }
}
