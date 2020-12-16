using System.Text.Json.Serialization;
using FeedleDataTier.Models;

namespace FeedleDataTier.Network
{
    public class UpdateReactionRequest : Request
    {
        [JsonPropertyName("postReaction")]
        public PostReaction PostReaction { get; set; }
        public UpdateReactionRequest(PostReaction postReaction) : base(RequestType.UpdateReactionRequest)
        {
           PostReaction = postReaction;
        }
    }
}