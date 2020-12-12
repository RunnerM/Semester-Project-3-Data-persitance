﻿using System;
 using System.Collections.Generic;
 using Feedle.Models;
 using FeedleDataTier.Models;
 using FeedleDataTier.Network;

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

        Conversation AddConversation(Conversation conversation, int creatorId);

        int DeleteComment(int commentId);

        FriendRequestNotification MakeFriendRequestNotification(FriendRequestNotification friendRequestNotification);

        int RespondToFriendRequest(bool status,FriendRequestNotification friendRequestNotification);
        UserSubscription SubscribeToUser(UserSubscription userSubscription);

        int UnsubscribeFromUser(int subscriptionId);

    }
}