using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FeedleDataTier.Models
{
    public class FriendRequestNotification
    {
        [Key]
        [JsonPropertyName("friendRequestId")]
        public int FriendRequestId { get; set; }
        
        [JsonPropertyName("creatorId")]
        public int CreatorId { get; set; }
        [JsonPropertyName("userId")]
        public int UserId { get; set; }
        [JsonPropertyName("creatorUserName")]
        public string CreatorUserName { get; set; }
        [JsonPropertyName("potentialFriendUserId")]
        public int PotentialFriendUserId { get; set; }
        [JsonPropertyName("content")]
        public string Content { get; set; }
        [JsonPropertyName("potentialFriendUserName")]
        public string PotentialFriendUserName { get; set; }
    }
}