using System.Text.Json.Serialization;
using FeedleDataTier.Models;

namespace FeedleDataTier.Network
{
    public class MakeReactionRequest : Request
    {
        [JsonPropertyName("postReaction")]
        public PostReaction PostReaction { get; set; }
        public MakeReactionRequest(PostReaction postReaction) : base(RequestType.MakeReactionRequest)
        {
            PostReaction = postReaction;
        }
    }
}