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

        List<UserConversation> AddConversation(Conversation conversation, int creatorId, int withWhomId);

        int DeleteComment(int commentId);

        FriendRequestNotification MakeFriendRequestNotification(FriendRequestNotification friendRequestNotification);

        List<UserFriend> RespondToFriendRequest(bool status,FriendRequestNotification friendRequestNotification);
        UserSubscription SubscribeToUser(UserSubscription userSubscription);

        int UnsubscribeFromUser(int subscriptionId);

        int DeleteFriend(int userFriendId);

        PostReaction MakePostReaction(PostReaction postReaction);

        PostReaction UpdatePostReaction(PostReaction postReaction);

        int DeleteReaction(int postReactionId);

    }
}