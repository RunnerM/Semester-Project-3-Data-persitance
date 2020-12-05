﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FeedleDataTier.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DisplayedUserName { get; set; }
        public List<Post> UserPosts { get; set; }
        public List<UserConversation> UserConversations { get; set; }
    }
}