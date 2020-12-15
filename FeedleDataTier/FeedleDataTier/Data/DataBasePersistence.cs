﻿using System;
 using System.Collections.Generic;
 using System.Linq;
 using Feedle.Models;
 using FeedleDataTier.DataAccess;
using FeedleDataTier.Models;
 using FeedleDataTier.Network;
 using Microsoft.EntityFrameworkCore;
 using Microsoft.EntityFrameworkCore.ChangeTracking;

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
            DataContext.Users.Update(user);
            DataContext.SaveChanges();
        }

        public void UpdatePost(Post post)
        {
            DataContext.Update(post);
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
                    DataContext.Entry(post).Collection(p=>p.Comments).Load();
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
                    DataContext.Entry(user).Collection(u=>u.UserFriends).Load();
                    DataContext.Entry(user).Collection(u=>u.FriendRequestNotifications).Load();
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
            UserConversation forCreator = new UserConversation();
            UserConversation forParticipant = new UserConversation();
            
            forCreator.Conversation = conversation;
            forParticipant.Conversation = conversation;

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
            EntityEntry<FriendRequestNotification> newlyAdded = DataContext.FriendRequestNotifications.Add(friendRequestNotification);
            DataContext.SaveChanges();
            return newlyAdded.Entity;
        }

        public List<UserFriend> RespondToFriendRequest(bool status, FriendRequestNotification friendRequestNotification)
        {
            var toRemove = DataContext.FriendRequestNotifications.FirstOrDefault(f =>
                    f.FriendRequestId == friendRequestNotification.FriendRequestId);
            List<UserFriend> userFriends = new List<UserFriend>();
                if (toRemove != null)
                {
                    DataContext.FriendRequestNotifications.Remove(toRemove);
                    if (status)
                    {
                        UserFriend userFriendForCreator = new UserFriend();
                        UserFriend userFriendForParticipant = new UserFriend();
                        userFriendForCreator.FriendId = friendRequestNotification.PotentialFriendUserId;
                        userFriendForCreator.UserId = friendRequestNotification.UserId;

                        userFriendForParticipant.FriendId = friendRequestNotification.UserId;
                        userFriendForParticipant.UserId = friendRequestNotification.PotentialFriendUserId;

                        EntityEntry<UserFriend> userFriendCreator = DataContext.UserFriends.Add(userFriendForCreator);
                        EntityEntry<UserFriend> userFriendPart = DataContext.UserFriends.Add(userFriendForParticipant);
                        
                        DataContext.SaveChanges();
                        
                        userFriends.Add(userFriendCreator.Entity);
                        userFriends.Add(userFriendPart.Entity);
                        Console.WriteLine(userFriends.Count);

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
            var toRemove = DataContext.UserSubscriptions.FirstOrDefault(u=>u.SubscriptionId == subscriptionId);
            Console.WriteLine(toRemove);
            if (toRemove != null)
            {
                DataContext.UserSubscriptions.Remove(toRemove);
                DataContext.SaveChanges();
                return subscriptionId;
            }

            return -1;
        }
    }
}