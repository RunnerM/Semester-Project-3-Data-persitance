using System.Text.Json.Serialization;
using FeedleDataTier.Models;

namespace FeedleDataTier.Network
{
    public class AddCommentRequest : Request
    {
        [JsonPropertyName("comment")]
        public Comment Comment { get; set; }
        public AddCommentRequest(Comment comment) : base(RequestType.AddComment)
        {
            this.Comment = comment;
        }
    }
}