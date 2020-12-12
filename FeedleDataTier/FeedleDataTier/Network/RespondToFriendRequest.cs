using System.Text.Json.Serialization;
using FeedleDataTier.Models;

namespace FeedleDataTier.Network
{
    public class RespondToFriendRequest : Request
    {
        [JsonPropertyName("respondStatus")]
        public bool RespondStatus { get; set; }
        [JsonPropertyName("friendRequestNotification")]
        public FriendRequestNotification FriendRequestNotification { get; set; }
        public RespondToFriendRequest(bool respondStatus, FriendRequestNotification friendRequestNotification) : base(RequestType.RespondToFriendRequest)
        {
            FriendRequestNotification = friendRequestNotification;
            RespondStatus = respondStatus;
        }
    }
}