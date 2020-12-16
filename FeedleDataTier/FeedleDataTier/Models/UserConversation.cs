﻿using System;
using System.Text.Json.Serialization;

namespace FeedleDataTier.Models
{
    public class UserConversation
    {
        [JsonPropertyName("userId")]
        public int UserId { get; set; }
        [JsonPropertyName("conversationId")]
        public int ConversationId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        [JsonPropertyName("conversation")]
        public Conversation Conversation { get; set; }

        [JsonPropertyName("withWhomUserId")]
        public int WithWhomId { get; set; }


    }
}