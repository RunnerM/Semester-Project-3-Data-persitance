using System.Text.Json.Serialization;
using FeedleDataTier.Models;

namespace FeedleDataTier.Network
{
    public class DeleteCommentRequest : Request
    {
        [JsonPropertyName("commentId")]
        public int CommentId { get; set; }
        public DeleteCommentRequest(int commentId) : base(RequestType.DeleteComment)
        {
            CommentId = commentId;
        }
    }
}