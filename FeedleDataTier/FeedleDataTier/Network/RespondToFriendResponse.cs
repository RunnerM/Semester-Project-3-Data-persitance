using System.Collections.Generic;
using System.Text.Json.Serialization;
using FeedleDataTier.Models;

namespace FeedleDataTier.Network
{
    public class RespondToFriendResponse : Request
    {
        [JsonPropertyName("userFriend")]
        public List<UserFriend> UserFriends { get; set; }
        public RespondToFriendResponse(List<UserFriend> userFriends) : base(RequestType.RespondToFriendResponse)
        {
            UserFriends = userFriends;
        }
    }
}