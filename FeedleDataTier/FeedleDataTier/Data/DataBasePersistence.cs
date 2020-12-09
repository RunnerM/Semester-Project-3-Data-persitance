﻿using System;
 using System.Collections.Generic;
 using System.Linq;
 using FeedleDataTier.DataAccess;
using FeedleDataTier.Models;
 using FeedleDataTier.Network;
 using Microsoft.EntityFrameworkCore.ChangeTracking;

 namespace FeedleDataTier.Data
{
    public class DataBasePersistence : IDataBasePersistence
    {
        private FeedleDBContext DbContext;

        public DataBasePersistence(FeedleDBContext dbContext)
        {
            this.DbContext = dbContext;
        }

        public void UpdateUser(User user)
        {
            DbContext.Users.Update(user);
            DbContext.SaveChanges();
        }

        public void UpdatePost(Post post)
        {
            DbContext.Posts.Update(post);
            DbContext.SaveChanges();
        }

        public void AddUser(User user)
        {
            EntityEntry<User> newlyAdded = DbContext.Users.Add(user);
            DbContext.SaveChanges();
        }

        public void AddPost(Post post)
        {
            EntityEntry<Post> newlyAdded = DbContext.Posts.Add(post);
            DbContext.SaveChanges();
        }

        public List<Post> GetPosts()
        {
            if (DbContext.Posts.Any())
            {
                var posts = DbContext.Posts.ToList();
                foreach (var post in posts)
                {
                    DbContext.Entry(post).Collection(p=>p.Comments).Load();
                }
                return posts;
            }

            return null;
        }
        

        public List<User> GetUsers()
        {
            if (DbContext.Users.Any())
            {
                var users = DbContext.Users.ToList();
                foreach (var user in users)
                {
                    DbContext.Entry(user).Collection(u => u.UserConversations).Load();
                    DbContext.Entry(user).Collection(u => u.SubscriptionUsersInformation).Load();
                    DbContext.Entry(user).Collection(u => u.UserPosts).Load();
                    foreach (var userPost in user.UserPosts)
                    {
                        DbContext.Entry(userPost).Collection(p => p.Comments).Load();
                    }
                }

                return users;
            }

            return null;
        }

        public void DeletePost(int postId)
        {
            var postToRemove = DbContext.Posts.ToList().FirstOrDefault(p => p.Id == postId);
            if (postToRemove != null)
            {
                DbContext.Posts.Remove(postToRemove);
                DbContext.SaveChanges();
            }
        }

        public void DeleteUser(int userId)
        {
            var userToRemove = DbContext.Users.ToList().FirstOrDefault(u => u.Id == userId);
            if (userToRemove != null)
            {
                DbContext.Users.Remove(userToRemove);
                DbContext.SaveChanges();
            }
        }
    }
}