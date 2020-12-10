﻿using System;
 using System.Collections.Generic;
 using System.Linq;
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
                    DataContext.Entry(user).Collection(u => u.SubscriptionUsersInformation).Load();
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

        public void AddComment(Comment comment)
        {
            EntityEntry<Comment> newlyAdded = DataContext.Comments.Add(comment);
            DataContext.SaveChanges();
        }

        public void SendMessage(Message message)
        {
            EntityEntry<Message> newlyAdded = DataContext.Messages.Add(message);
            DataContext.SaveChanges();
        }
    }
}