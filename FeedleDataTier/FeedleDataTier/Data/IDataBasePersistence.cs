﻿using System.Collections.Generic;
using FeedleDataTier.Models;

namespace FeedleDataTier.Data
{
    public interface IDataBasePersistence
    {
        void UpdateUser(User user);
        void UpdatePost(Post post);
        void AddUser(User user);
        void AddPost(Post post);
        List<Post> GetPosts();

        void DeletePost(int postId);

        void DeleteUser(int userId);

        List<User> GetUsers();

    }
}