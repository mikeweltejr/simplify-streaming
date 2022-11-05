namespace DynamoDB.DAL.App.Models
{
    public class StreamingService : DynamoBase
    {
        public string Id { get; }
        public string Name { get; }

        public StreamingService(string id, string name) : base(id, SKPrefix.STREAMING_SERVICE + id)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
