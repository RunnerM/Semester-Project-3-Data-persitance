﻿using Feedle.Models;
 using FeedleDataTier.Models;
using Microsoft.EntityFrameworkCore;

namespace FeedleDataTier.DataAccess
{
    public class FeedleDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        
        public DbSet<UserFriend> UserFriends { get; set; }
        
        public DbSet<FriendRequestNotification> FriendRequestNotifications { get; set; }
        
        public DbSet<UserSubscription> UserSubscriptions { get; set; }

        public DbSet<Conversation> Conversations { get; set; }
        
        public DbSet<UserConversation> UserConversations { get; set; }
        
        public DbSet<Comment> Comments { get; set; }
        
        public DbSet<Message> Messages { get; set; }
        
        public DbSet<PostReaction> PostReactions { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseNpgsql("Host=localhost;Database=Feedle_db;Username=postgres;Password=3228");
            optionsBuilder.UseSqlite("Data Source = Feedle.db");
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserConversation>()
                .HasKey(uc => new {uc.UserId, uc.ConversationId});

            modelBuilder.Entity<UserConversation>()
                .HasOne(uc => uc.User)
                .WithMany(user => user.UserConversations)
                .HasForeignKey(uc => uc.UserId);

            modelBuilder.Entity<UserConversation>()
                .HasOne(uc => uc.Conversation)
                .WithMany(conversation => conversation.UserConversations)
                .HasForeignKey(uc => uc.ConversationId);
        }
    }
}