﻿using System.Collections.Generic;
using FeedleDataTier.Models;

namespace FeedleDataTier.Data
{
    public interface IDataBasePersistence
    {
        User UpdateUser(string userAsJson);
        Post UpdatePost(string postAsJson);
        User AddUser(string userAsJson);
        Post AddPost(string postAsJson);
        bool VerifyUser(string login, string password);
        List<Post> GetPosts();
        
    }
}