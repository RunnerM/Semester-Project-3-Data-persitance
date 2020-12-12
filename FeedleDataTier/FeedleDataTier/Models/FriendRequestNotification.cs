using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FeedleDataTier.Models
{
    public class FriendRequestNotification
    {
        [Key]
        [JsonPropertyName("friendRequestId")]
        public int FriendRequestId { get; set; }
        [JsonPropertyName("potentialFriendUserId")]
        
        public int CreatorId { get; set; }
        public int PotentialFriendUserId { get; set; }
        [JsonPropertyName("content")]
        public string Content { get; set; }
        [JsonPropertyName("potentialFriendUserName")]
        public string PotentialFriendUserName { get; set; }
    }
}