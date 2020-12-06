using System.Text.Json.Serialization;
using FeedleDataTier.Models;

namespace FeedleDataTier.Network
{
    public class AddPostRequest : Request
    {
        
      [JsonPropertyName("post")]
      public Post Post { get; set; }

      public AddPostRequest(Post post) : base(RequestType.AddPost)
      {
          this.Post = post;
      }
    }
}