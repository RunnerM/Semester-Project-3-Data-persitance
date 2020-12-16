using System.Text.Json.Serialization;

namespace FeedleDataTier.Network
{
    public class DeleteFriendRequest : Request
    {
        [JsonPropertyName("userFriendId")]
        public int UserFriendId { get; set; }

        public DeleteFriendRequest(int userFriendId) : base(RequestType.DeleteFriendRequest)
        {
            this.UserFriendId = userFriendId;
        }
    }
}