using System;
using System.Text.Json.Serialization;

namespace FeedleDataTier.Models
{
    public class UserConversation
    {
        public int UserId { get; set; }
        public int ConversationId { get; set; }
        public User User { get; set; }
        public Conversation Conversation { get; set; }
        
    }
}