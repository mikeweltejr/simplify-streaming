using System.Text.Json.Serialization;
using ThirdParty.Json.LitJson;

namespace DynamoDB.DAL.App.Models
{
    public class ServiceTitle : DynamoBase
    {
        private Service _serviceId;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("StreamingService")]
        public Service ServiceId {
            get { return _serviceId; }
            set {
                _serviceId = value;
                PK = SKPrefix.SERVICE + value;
            }
        }
        private string? _titleId;
        public string? TitleId {
            get { return _titleId; }
            set {
                _titleId = value;
                SK = SKPrefix.SERVICE_TITLE + value;
            }
        }
        [JsonPropertyName("Name")]
        public string? TitleName { get; set; }
        public TitleType Type { get; set; }

        public ServiceTitle() {}

        public ServiceTitle(Service serviceId, string titleId, string titleName, TitleType type)
            : base(SKPrefix.SERVICE + serviceId, SKPrefix.SERVICE_TITLE + titleId, titleId)
        {
            this.ServiceId = serviceId;
            this.TitleId = titleId;
            this.TitleName = titleName;
            this.Type = type;
        }
    }
}
