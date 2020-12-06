using System.Text.Json.Serialization;
using FeedleDataTier.Models;

namespace FeedleDataTier.Network
{
    public class UpdateUserRequest : Request
    {
        [JsonPropertyName("user")]
        public User User { get; set; }

        public UpdateUserRequest(User user) : base(RequestType.UpdateUser)
        {
            this.User = user;
        }
    }
}