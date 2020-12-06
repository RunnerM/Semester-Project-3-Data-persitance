﻿using System.Collections.Generic;
using FeedleDataTier.Models;

namespace FeedleDataTier.Data
{
    public interface IDataBasePersistence
    {
        User UpdateUser(User user);
        Post UpdatePost(User user);
        User AddUser(User user);
        Post AddPost(Post post);
        List<Post> GetPosts();

        List<User> GetUsers();

    }
}