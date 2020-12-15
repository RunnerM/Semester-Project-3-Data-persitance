using System.Text.Json.Serialization;

namespace FeedleDataTier.Network
{
    public class UnsubscribeRequest : Request
    {
        [JsonPropertyName("subscriptionId")]
        public int SubscriptionId { get; set; }
        public UnsubscribeRequest(int subscriptionId) : base(RequestType.UnsubscribeRequest)
        {
            SubscriptionId = subscriptionId;
        }
    }
}