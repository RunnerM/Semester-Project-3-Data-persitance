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
        public User UpdateUser(string userAsJson)
        {
            throw new System.NotImplementedException();
        }

        public Post UpdatePost(string postAsJson)
        {
            throw new System.NotImplementedException();
        }

        public User AddUser(string userAsJson)
        {
            throw new System.NotImplementedException();
        }

        public Post AddPost(string postAsJson)
        {
            throw new System.NotImplementedException();
        }

        public bool VerifyUser(string login, string password)
        {
            throw new System.NotImplementedException();
        }

        public List<Post> GetPosts()
        {
            throw new System.NotImplementedException();
        }
    }
}