using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Feedle.Models
{
    public class UserSubscription
    {
        [Key]
        [JsonPropertyName("userSubscriptionId")]
        public int UserSubscriptionId { get; set; }
        [JsonPropertyName("userId")]
        public int UserId { get; set; }
        [JsonPropertyName("subscriptionId")]
        public int SubscriptionId { get; set; }
        [JsonPropertyName("userName")]
        public string UserName { get; set; }
    }
}