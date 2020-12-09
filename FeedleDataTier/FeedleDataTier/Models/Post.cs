﻿﻿using System.Collections.Generic;
 using System.Text.Json.Serialization;

 namespace FeedleDataTier.Models
{
    public class Post
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("content")]
        public string Content { get; set; }
        [JsonPropertyName("authorUserName")]
        public string AuthorUserName { get; set; }
        [JsonPropertyName("day")]
        public int Day { get; set; }
        [JsonPropertyName("month")]
        public int Month { get; set; }
        [JsonPropertyName("year")]
        public int Year { get; set; }
        [JsonPropertyName("hour")]
        public int Hour { get; set; }
        [JsonPropertyName("minute")]
        public int Minute { get; set; }
        [JsonPropertyName("second")]
        public int Second { get; set; }
        // public byte[] images;
        [JsonPropertyName("comments")]
        public List<Comment> Comments { get; set; }
        [JsonPropertyName("approvals")] public int Approvals { get; set; }
        [JsonPropertyName("disapprovals")] public int Disapprovals { get; set; }
    }
}