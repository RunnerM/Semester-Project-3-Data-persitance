using System.Text.Json.Serialization;
using FeedleDataTier.Models;

namespace FeedleDataTier.Network
{
    public class AddConversationRequest : Request
    {
        [JsonPropertyName("conversation")]
        public Conversation Conversation { get; set; }
        [JsonPropertyName("creatorId")]
        public int CreatorId { get; set; }
        [JsonPropertyName("withWhomId")]
        public int WithWhomId { get; set; }

        public AddConversationRequest(Conversation conversation, int creatorId, int withWhomId) : base(RequestType.AddConversation)
        {
            Conversation = conversation;
            CreatorId = creatorId;
            WithWhomId = withWhomId;
        }
        
    }
}