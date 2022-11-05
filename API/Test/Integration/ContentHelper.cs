using Newtonsoft.Json;
using System.Text;

namespace Integration
{
    public static class ContentHelper
    {
        public static StringContent GetStringContent(object obj)
            => new StringContent(JsonConvert.SerializeObject(obj), Encoding.Default, "application/json");
    }
}
