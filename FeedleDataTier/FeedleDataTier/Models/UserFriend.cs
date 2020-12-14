using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FeedleDataTier.Models
{
    public class UserFriend
    {
        [Key]
        [JsonPropertyName("userFriendId")]
        public int UserFriendId { get; set; }
        [JsonPropertyName("userId")]
        public int UserId { get; set; }
        [JsonPropertyName("friendId")]
        public int FriendId { get; set; }
        [JsonPropertyName("userName")]
        public string UserName { get; set; }
    }
}