using System.Text.Json.Serialization;

namespace FeedleDataTier.Network
{
    public class DeleteReactionRequest : Request
    {
        [JsonPropertyName("postReactionId")]
        public int PostReactionId { get; set; }
        public DeleteReactionRequest(int postReactionId) : base(RequestType.DeleteReactionRequest)
        {
            this.PostReactionId = postReactionId;
        }
    }
}