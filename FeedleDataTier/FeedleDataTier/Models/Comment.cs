﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
 using System.Text.Json.Serialization;

 namespace FeedleDataTier.Models
{
    public class Comment
    {
        [Key]
        [JsonPropertyName("id")]
        public int CommentId { get; set; }
        [JsonPropertyName("content")]
        public string Content { get; set; }
        [JsonPropertyName("userId")]
        public int UserId { get; set; }
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
        [JsonPropertyName("postId")]
        public int PostId { get; set; }
    }
}