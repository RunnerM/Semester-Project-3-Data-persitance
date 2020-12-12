using System.Text.Json.Serialization;
using Feedle.Models;

namespace FeedleDataTier.Network
{
    public class SubscribeToUserRequest : Request
    {
        [JsonPropertyName("userSubscription")]
        public UserSubscription UserSubscription { get; set; }
        public SubscribeToUserRequest(UserSubscription userSubscription) : base(RequestType.SubscribeToUser)
        {
            this.UserSubscription = userSubscription;
        }
    }
}