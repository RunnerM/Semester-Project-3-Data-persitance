﻿using System.Collections.Generic;
using FeedleDataTier.DataAccess;
using FeedleDataTier.Models;

namespace FeedleDataTier.Data
{
    public class DataBasePersistence : IDataBasePersistence
    {
        private FeedleDBContext DbContext;

        public DataBasePersistence(FeedleDBContext dbContext)
        {
            this.DbContext = dbContext;
        }

        public User UpdateUser(User user)
        {
            throw new System.NotImplementedException();
        }

        public Post UpdatePost(User user)
        {
            throw new System.NotImplementedException();
        }

        public User AddUser(User user)
        {
            throw new System.NotImplementedException();
        }

        public Post AddPost(Post post)
        {
            throw new System.NotImplementedException();
        }

        public List<Post> GetPosts()
        {
            throw new System.NotImplementedException();
        }

        public List<User> GetUsers()
        {
            throw new System.NotImplementedException();
        }
    }
}