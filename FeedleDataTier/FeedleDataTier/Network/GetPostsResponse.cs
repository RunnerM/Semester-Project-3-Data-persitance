using System.Collections.Generic;
using System.Text.Json.Serialization;
using FeedleDataTier.Models;

namespace FeedleDataTier.Network
{
    public class GetPostsResponse : Request
    {
        [JsonPropertyName("posts")]
        public List<Post> Posts { get; set; }
        
        public GetPostsResponse(List<Post> posts) : base(RequestType.GetPosts)
        {
            this.Posts = posts;
        }
    }
}