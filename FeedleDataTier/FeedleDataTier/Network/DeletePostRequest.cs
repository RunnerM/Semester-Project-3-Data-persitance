using System.Text.Json.Serialization;

namespace FeedleDataTier.Network
{
    public class DeletePostRequest : Request
    {

        [JsonPropertyName("postId")] 
        public int PostId { get; set; }

        public DeletePostRequest(int postId) : base(RequestType.DeletePost)
        {
            this.PostId = postId;
        }
    }
}