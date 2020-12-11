﻿using System;
 using System.Collections.Generic;
using FeedleDataTier.Models;

namespace FeedleDataTier.Data
{
    public interface IDataBasePersistence
    {
        void UpdateUser(User user);
        void UpdatePost(Post post);
        User AddUser(User user);
        Post AddPost(Post post);
        List<Post> GetPosts();

        void DeletePost(int postId);

        void DeleteUser(int userId);

        List<User> GetUsers();

        Comment AddComment(Comment comment);

        Message SendMessage(Message message);

    }
}