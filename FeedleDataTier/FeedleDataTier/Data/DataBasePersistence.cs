﻿using System;
 using System.Collections.Generic;
 using System.Linq;
 using Feedle.Models;
 using FeedleDataTier.DataAccess;
using FeedleDataTier.Models;
 using FeedleDataTier.Network;
 using Microsoft.EntityFrameworkCore;
 using Microsoft.EntityFrameworkCore.ChangeTracking;
 using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;

 namespace FeedleDataTier.Data
{
    public class DataBasePersistence : IDataBasePersistence
    {
        private FeedleDBContext DataContext;

        public DataBasePersistence(FeedleDBContext dbContext)
        {
            this.DataContext = dbContext;
        }

        public void UpdateUser(User user)
        {
            bool tracking = DataContext.ChangeTracker.Entries<User>().Any(x => x.Entity.Id == user.Id);
            if (!tracking)
            {
                DataContext.Users.Update(user);
            }
            else
            {
                var userToUpdate = DataContext.Users.FirstOrDefault(u => u.Id == user.Id);
                if (userToUpdate != null)
                {
                    userToUpdate.Password = user.Password;
                    userToUpdate.UserImageSrc = user.UserImageSrc;
                }
            }

            DataContext.SaveChanges();
        }

        public void UpdatePost(Post post)
        {
            bool tracking = DataContext.ChangeTracker.Entries<Post>().Any(x => x.Entity.PostId == post.PostId);
            if (!tracking)
            {
                DataContext.Update(post);
            }

            var postToUpdate = DataContext.Posts.FirstOrDefault(p => p.PostId == post.PostId);
            if (postToUpdate != null)
            {
                postToUpdate.Title = post.Title;
                postToUpdate.Content = post.Content;
                postToUpdate.PostImageSrc = post.PostImageSrc;
            }

            DataContext.SaveChanges();
        }

        public User AddUser(User user)
        {
            EntityEntry<User> newlyAdded = DataContext.Users.Add(user);
            DataContext.SaveChanges();
            return newlyAdded.Entity;
        }

        public Post AddPost(Post post)
        {
            EntityEntry<Post> newlyAdded = DataContext.Posts.Add(post);
            DataContext.SaveChanges();
            return newlyAdded.Entity;
        }

        public List<Post> GetPosts()
        {
            if (DataContext.Posts.Any())
            {
                var posts = DataContext.Posts.ToList();
                foreach (var post in posts)
                {
                    DataContext.Entry(post).Collection(p => p.Comments).Load();
                    DataContext.Entry(post).Collection(p => p.PostReactions).Load();
                }

                return posts;
            }

            return null;
        }


        public List<User> GetUsers()
        {
            if (DataContext.Users.Any())
            {
                var users = DataContext.Users.ToList();
                foreach (var user in users)
                {
                    DataContext.Entry(user).Collection(u => u.UserConversations).Load();
                    foreach (var userConversation in user.UserConversations)
                    {
                        DataContext.Entry(userConversation).Reference(uc => uc.Conversation).Load();
                        DataContext.Entry(userConversation.Conversation).Collection(ucc => ucc.Messages).Load();
                    }

                    DataContext.Entry(user).Collection(u => u.UserSubscriptions).Load();
                    DataContext.Entry(user).Collection(u => u.UserFriends).Load();
                    DataContext.Entry(user).Collection(u => u.FriendRequestNotifications).Load();
                    DataContext.Entry(user).Collection(u => u.UserPosts).Load();
                    foreach (var userPost in user.UserPosts)
                    {
                        DataContext.Entry(userPost).Collection(p => p.Comments).Load();
                    }
                }

                return users;
            }

            return null;
        }

        public void DeletePost(int postId)
        {
            var postToRemove = DataContext.Posts.ToList().FirstOrDefault(p => p.PostId == postId);
            if (postToRemove != null)
            {
                DataContext.Posts.Remove(postToRemove);
                DataContext.SaveChanges();
            }
        }

        public void DeleteUser(int userId)
        {
            var userToRemove = DataContext.Users.ToList().FirstOrDefault(u => u.Id == userId);
            if (userToRemove != null)
            {
                DataContext.Users.Remove(userToRemove);
                DataContext.SaveChanges();
            }
        }

        public Comment AddComment(Comment comment)
        {
            EntityEntry<Comment> newlyAdded = DataContext.Comments.Add(comment);
            DataContext.SaveChanges();
            return newlyAdded.Entity;

        }

        public Message SendMessage(Message message)
        {
            EntityEntry<Message> newlyAdded = DataContext.Messages.Add(message);
            DataContext.SaveChanges();
            return newlyAdded.Entity;
        }

        public List<UserConversation> AddConversation(Conversation conversation, int creatorId, int withWhomId)
        {
            EntityEntry<Conversation> newlyAdded = DataContext.Conversations.Add(conversation);
            DataContext.SaveChanges();
            UserConversation forCreator = new UserConversation();
            UserConversation forParticipant = new UserConversation();

            forCreator.Conversation = newlyAdded.Entity;
            forParticipant.Conversation = newlyAdded.Entity;

            forCreator.WithWhomId = withWhomId;
            forCreator.UserId = creatorId;

            forCreator.ConversationId = conversation.ConversationId;
            forParticipant.ConversationId = conversation.ConversationId;

            forParticipant.WithWhomId = creatorId;
            forParticipant.UserId = withWhomId;


            EntityEntry<UserConversation> uc = DataContext.UserConversations.Add(forCreator);
            EntityEntry<UserConversation> uc2 = DataContext.UserConversations.Add(forParticipant);
            DataContext.SaveChanges();

            List<UserConversation> userConversations = new List<UserConversation>();

            userConversations.Add(uc.Entity);
            userConversations.Add(uc2.Entity);


            return userConversations;
        }

        public int DeleteComment(int commentId)
        {
            var commentToRemove = DataContext.Comments.FirstOrDefault(comment => comment.CommentId == commentId);
            if (commentToRemove != null)
            {
                DataContext.Comments.Remove(commentToRemove);
                DataContext.SaveChanges();
                return commentToRemove.CommentId;
            }

            return -1;
        }

        public FriendRequestNotification MakeFriendRequestNotification(
            FriendRequestNotification friendRequestNotification)
        {
            EntityEntry<FriendRequestNotification> newlyAdded =
                DataContext.FriendRequestNotifications.Add(friendRequestNotification);
            Console.WriteLine(newlyAdded.Entity.FriendRequestId);
            DataContext.SaveChanges();
            Console.WriteLine(newlyAdded.Entity.FriendRequestId);
            return newlyAdded.Entity;
        }

        public List<UserFriend> RespondToFriendRequest(bool status, FriendRequestNotification friendRequestNotification)
        {
            var toRemove = DataContext.FriendRequestNotifications.FirstOrDefault(f =>
                f.FriendRequestId == friendRequestNotification.FriendRequestId);
            var echoToRemove = DataContext.FriendRequestNotifications.FirstOrDefault(f =>
                f.CreatorId == friendRequestNotification.CreatorId &&
                f.UserId == friendRequestNotification.PotentialFriendUserId);
            List<UserFriend> userFriends = new List<UserFriend>();
            if (toRemove != null && echoToRemove != null)
            {
                DataContext.FriendRequestNotifications.Remove(toRemove);
                DataContext.FriendRequestNotifications.Remove(echoToRemove);
                if (status)
                {
                    UserFriend userFriendForCreator = new UserFriend();
                    UserFriend userFriendForParticipant = new UserFriend();
                    userFriendForCreator.FriendId = friendRequestNotification.PotentialFriendUserId;
                    userFriendForCreator.UserId = friendRequestNotification.UserId;
                    userFriendForCreator.UserName = friendRequestNotification.CreatorUserName;

                    userFriendForParticipant.FriendId = friendRequestNotification.UserId;
                    userFriendForParticipant.UserId = friendRequestNotification.PotentialFriendUserId;
                    userFriendForParticipant.UserName = friendRequestNotification.PotentialFriendUserName;

                    EntityEntry<UserFriend> userFriendCreator = DataContext.UserFriends.Add(userFriendForCreator);
                    EntityEntry<UserFriend> userFriendPart = DataContext.UserFriends.Add(userFriendForParticipant);

                    DataContext.SaveChanges();

                    userFriends.Add(userFriendCreator.Entity);
                    userFriends.Add(userFriendPart.Entity);

                    return userFriends;
                }

                DataContext.SaveChanges();
            }

            return null;

        }

        public UserSubscription SubscribeToUser(UserSubscription userSubscription)
        {
            EntityEntry<UserSubscription> newlyAdded = DataContext.UserSubscriptions.Add(userSubscription);
            DataContext.SaveChanges();
            return newlyAdded.Entity;
        }

        public int UnsubscribeFromUser(int subscriptionId)
        {
            Console.WriteLine(subscriptionId);
            var toRemove = DataContext.UserSubscriptions.FirstOrDefault(u => u.SubscriptionId == subscriptionId);
            Console.WriteLine(toRemove);
            if (toRemove != null)
            {
                DataContext.UserSubscriptions.Remove(toRemove);
                DataContext.SaveChanges();
                return subscriptionId;
            }

            return -1;
        }

        public int DeleteFriend(int userFriendId)
        {
            var toRemove = DataContext.UserFriends.FirstOrDefault(uf => uf.UserFriendId == userFriendId);
            var toRemoveSecond = DataContext.UserFriends.FirstOrDefault(uf =>
                uf.UserId == toRemove.FriendId && uf.FriendId == toRemove.UserId);
            if (toRemove != null && toRemoveSecond != null)
            {
                DataContext.UserFriends.Remove(toRemove);
                DataContext.UserFriends.Remove(toRemoveSecond);
                DataContext.SaveChanges();
                return toRemove.UserFriendId;
            }

            return -1;
        }

        public int DeleteReaction(int postReactionId)
        {
            var toRemove =
                DataContext.PostReactions.FirstOrDefault(reaction => reaction.PostReactionId == postReactionId);
            if (toRemove != null)
            {
                DataContext.PostReactions.Remove(toRemove);
                DataContext.SaveChanges();
                return toRemove.PostReactionId;
            }

            return -1;
        }

        public PostReaction MakePostReaction(PostReaction postReaction)
        {
            EntityEntry<PostReaction> newlyAdded = DataContext.PostReactions.Add(postReaction);
            DataContext.SaveChanges();
            return newlyAdded.Entity;
        }

        public PostReaction UpdatePostReaction(PostReaction postReaction)
        {
            bool tracking = DataContext.ChangeTracker.Entries<PostReaction>()
                .Any(x => x.Entity.PostReactionId == postReaction.PostReactionId);
            if (!tracking)
            {
                EntityEntry<PostReaction> updated = DataContext.Update(postReaction);
                DataContext.SaveChanges();
                return updated.Entity;
            }

            var reactionToUpdate =
                DataContext.PostReactions.FirstOrDefault(p => p.PostReactionId == postReaction.PostReactionId);
            if (reactionToUpdate != null)
            {
                reactionToUpdate.Status = postReaction.Status;
                DataContext.SaveChanges();
                return reactionToUpdate;
            }

            return null;
        }
    }
}