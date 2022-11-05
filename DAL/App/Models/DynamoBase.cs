using System.Text.Json.Serialization;

namespace DynamoDB.DAL.App.Models
{
    public class DynamoBase
    {
        [JsonIgnore]
        public string PK { get; }
        [JsonIgnore]
        public string SK { get; }
        [JsonIgnore]
        public string? GSI_1 { get; }

        public DynamoBase(string pk, string sk)
        {
            this.PK = pk;
            this.SK = sk;
        }
        public DynamoBase(string pk, string sk, string gsi_1)
        {
            this.PK = pk;
            this.SK = sk;
            this.GSI_1 = gsi_1;
        }
    }
}
