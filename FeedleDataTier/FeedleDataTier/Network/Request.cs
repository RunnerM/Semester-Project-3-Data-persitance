using System.Collections.Specialized;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace FeedleDataTier.Network
{
    public class Request
    {
       [JsonPropertyName("type")]
       [JsonConverter(typeof(JsonStringEnumConverter))]
        public RequestType Type { get; set; }

        public Request(RequestType type)
        {
            this.Type = type;
        }
    }
}