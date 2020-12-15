using System.Collections.Generic;
using System.Text.Json.Serialization;
using FeedleDataTier.Models;

namespace FeedleDataTier.Network
{
    public class RespondToFriendResponse : Request
    {
        [JsonPropertyName("userFriends")]
        public List<UserFriend> UserFriends { get; set; }
        public RespondToFriendResponse(List<UserFriend> userFriends) : base(RequestType.RespondToFriendResponse)
        {
            UserFriends = userFriends;
        }
    }
}