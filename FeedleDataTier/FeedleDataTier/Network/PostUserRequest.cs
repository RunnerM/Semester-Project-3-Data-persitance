using System.Text.Json.Serialization;
using FeedleDataTier.Models;

namespace FeedleDataTier.Network
{
    public class PostUserRequest : Request
    {
        [JsonPropertyName("user")]
        public User User { get; set; }

        public PostUserRequest(User user) : base(RequestType.PostUser)
        {
            this.User = user;
        }
    }
}