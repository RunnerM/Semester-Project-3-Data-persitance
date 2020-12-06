using System.Text.Json.Serialization;

namespace FeedleDataTier.Network
{
    public class Request
    {
       [JsonPropertyName("type")]
        public RequestType Type { get; set; }

        public Request(RequestType type)
        {
            this.Type = type;
        }
    }
}