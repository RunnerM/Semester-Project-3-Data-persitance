using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FeedleDataTier.Models
{
    public class Conversation
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Message> Messages { get; set; }
        public List<UserConversation> UserConversations { get; set; }
    }
}