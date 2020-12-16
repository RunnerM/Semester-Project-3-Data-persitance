﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
 using System.Text.Json.Serialization;

 namespace FeedleDataTier.Models
{
    public class Conversation
    {
        [Key]
        [JsonPropertyName("id")]
        public int ConversationId { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("messages")]
        public List<Message> Messages { get; set; }
        [JsonIgnore]
        public List<UserConversation> UserConversations { get; set; }
        
    }
}