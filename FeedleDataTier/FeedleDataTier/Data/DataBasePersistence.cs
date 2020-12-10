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
            DataContext.Posts.Update(post);
            DataContext.SaveChanges();
        }

        public void AddUser(User user)
        {
            EntityEntry<User> newlyAdded = DataContext.Users.Add(user);
            DataContext.SaveChanges();
        }

        public void AddPost(Post post)
        {
            User userToBeUpdated = DataContext.Users.FirstOrDefault(u => u.UserName.Equals(post.AuthorUserName));
            if (userToBeUpdated != null)
            {
                userToBeUpdated.UserPosts.Add(post);
                DataContext.Update(userToBeUpdated);
                DataContext.SaveChanges();
            }
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
            var postToRemove = DataContext.Posts.ToList().FirstOrDefault(p => p.Id == postId);
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
    }
}