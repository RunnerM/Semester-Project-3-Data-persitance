﻿using System.ComponentModel.DataAnnotations;
 using System.Text.Json.Serialization;

 namespace FeedleDataTier.Models
{
    public class Message
    {
        [Key]
        [JsonPropertyName("id")]
        public int MessageId { get; set; }
        [JsonPropertyName("content")]
        public string Content { get; set; }
        [JsonPropertyName("UserId")]
        public int UserId { get; set; }
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
        [JsonPropertyName("conversationId")]
        public int ConversationId { get; set; }
    }
}