using System.Text.Json.Serialization;
using FeedleDataTier.Models;

namespace FeedleDataTier.Network
{
    public class MakeFriendRequest : Request
    {
        [JsonPropertyName("friendRequestNotification")] 
        public FriendRequestNotification FriendRequestNotification { get; set; }
        public MakeFriendRequest(FriendRequestNotification friendRequestNotification) : base(RequestType.MakeFriendRequest)
        {
            FriendRequestNotification = friendRequestNotification;
        }
    }
}