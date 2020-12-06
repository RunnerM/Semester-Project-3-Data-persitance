using System.Text.Json.Serialization;
using System.Transactions;
using FeedleDataTier.Models;

namespace FeedleDataTier.Network
{
    public class UpdatePostRequest : Request
    {
        [JsonPropertyName("post")]
        public Post Post { get; set; }

        public UpdatePostRequest(Post post) : base(RequestType.UpdatePost)
        {
            this.Post = post;
        }
    }
}