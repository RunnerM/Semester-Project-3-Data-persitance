using FeedleDataTier.Models;
using Microsoft.EntityFrameworkCore;

namespace FeedleDataTier.DataAccess
{
    public class FeedleDBContext : DbContext
    {
        private DbSet<User> Users;
        private DbSet<Post> Posts;
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=Feedle_db;Username=postgres;Password=3228");
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