using System.Collections.Generic;
using System.Text.Json.Serialization;
using FeedleDataTier.Models;

namespace FeedleDataTier.Network
{
    public class AddConversationResponse : Request
    {
        [JsonPropertyName("userConversations")]
        public List<UserConversation> UserConversations { get; set; }

        public AddConversationResponse(List<UserConversation> userConversations) : base(RequestType.AddConversationResponse)
        {
            this.UserConversations = userConversations;
        }
    }
}