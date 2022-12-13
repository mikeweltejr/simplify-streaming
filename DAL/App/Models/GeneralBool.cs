using System.Diagnostics.CodeAnalysis;

namespace DynamoDB.DAL.App.Models
{
    public class GeneralBool : DynamoBase
    {
        public string? Id { get; set; }
        public string? Value { get; set; }

        public GeneralBool() {}

        public GeneralBool(string id, string value) : base(id, value)
        {
            this.Id = id;
            this.Value = value;
        }
    }
}
