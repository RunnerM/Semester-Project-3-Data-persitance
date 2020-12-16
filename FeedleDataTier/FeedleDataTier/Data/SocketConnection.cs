﻿using System;
 using System.Collections.Generic;
 using System.ComponentModel;
 using System.Net;
using System.Net.Sockets;
 using System.Runtime.CompilerServices;
 using System.Text;
 using System.Text.Json;
 using System.Threading;
 using Feedle.Models;
 using FeedleDataTier.Models;
 using FeedleDataTier.Network;
 using Microsoft.VisualBasic.CompilerServices;

 namespace FeedleDataTier.Data
{
    public class SocketConnection
    {
        public IDataBasePersistence DbPersistence { get; set; }
        
        public IPHostEntry host { get; set; }

        public IPAddress ipAddress { get; set; }  
        public IPEndPoint serverAddress {get; set;}
        public IPEndPoint localEndPoint { get; set; }

        public SocketConnection(IDataBasePersistence dbPersistence)
        {
            host = Dns.GetHostEntry("127.0.0.1"); 
            ipAddress = host.AddressList[0]; 
            serverAddress = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);
            localEndPoint = new IPEndPoint(ipAddress, 5000);
            this.DbPersistence = dbPersistence;
        }
        
        public void Start()
        {
            Console.WriteLine("Server started..");
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream,ProtocolType.Tcp);
            serverSocket.Bind(serverAddress);
            serverSocket.Listen(10);
            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    Socket socketToClient = serverSocket.Accept();
                    Console.WriteLine("Client connected...");
                    Thread clientThread = new Thread(() =>ClientThread(socketToClient));
                    clientThread.Start();
                }
            });
            thread.Start();
        }
        private void ClientThread(Socket client)
        {
            Console.WriteLine("Client thread started");
            while (true)
            {
                //read
                byte[] rcvLenBytes = new byte[4];
                client.Receive(rcvLenBytes);
                int rcvLen = BitConverter.ToInt32(rcvLenBytes, 0);
                byte[] dataFromClient = new byte[rcvLen];
                client.Receive(dataFromClient);
                string message = Encoding.ASCII.GetString(dataFromClient, 0, rcvLen);
                Console.WriteLine(message);
                Request request =
                    JsonSerializer.Deserialize<Request>(Encoding.ASCII.GetString(dataFromClient, 0, rcvLen));
                if (message.Equals("stop"))
                {
                    Console.WriteLine(message);
                    break;
                }
                //respond
                switch (request.Type)
                {
                    case RequestType.AddPost:
                        
                        AddPostRequest addPostRequest = JsonSerializer.Deserialize<AddPostRequest>(message);
                        Post newPost = DbPersistence.AddPost(addPostRequest.Post);
                        string responseMessageAddPost = JsonSerializer.Serialize(new AddPostRequest(newPost));
                        int toSendLenAddPost = Encoding.ASCII.GetByteCount(responseMessageAddPost);
                        byte[] toSendBytesAddPost = Encoding.ASCII.GetBytes(responseMessageAddPost);
                        byte[] toSendLenBytesAddPost = BitConverter.GetBytes(toSendLenAddPost);
                        client.Send(toSendLenBytesAddPost);
                        client.Send(toSendBytesAddPost);
                        break;
                    
                    case RequestType.DeletePost :
                        
                        DeletePostRequest deletePostRequest = JsonSerializer.Deserialize<DeletePostRequest>(message);
                        DbPersistence.DeletePost(deletePostRequest.PostId);
                        int toSendDelPost = Encoding.ASCII.GetByteCount(message);
                        byte[] toSendBytesDelPost = Encoding.ASCII.GetBytes(message);
                        byte[] toSendLenBytesDelPost = BitConverter.GetBytes(toSendDelPost);
                        client.Send(toSendLenBytesDelPost);
                        client.Send(toSendBytesDelPost);
                        break;
                    
                    case RequestType.DeleteUser :

                        DeleteUserRequest deleteUserRequest = JsonSerializer.Deserialize<DeleteUserRequest>(message);
                        DbPersistence.DeleteUser(deleteUserRequest.UserId);
                        int toSendDelUser = Encoding.ASCII.GetByteCount(message);
                        byte[] toSendBytesDelUser = Encoding.ASCII.GetBytes(message);
                        byte[] toSendLenBytesDelUser = BitConverter.GetBytes(toSendDelUser);
                        client.Send(toSendLenBytesDelUser);
                        client.Send(toSendBytesDelUser);
                        break;
                    
                    case RequestType.GetPosts :
                        List<Post> posts = DbPersistence.GetPosts();
                        if (posts == null)
                        {
                            posts = new List<Post>();
                        }
                        string getPostsResponseMessage = JsonSerializer.Serialize(new GetPostsResponse(posts));
                        int toGetPosts = Encoding.ASCII.GetByteCount(getPostsResponseMessage);
                        byte[] toSendLenGetPosts = BitConverter.GetBytes(toGetPosts);
                        byte[] dataToClientGetPosts = Encoding.ASCII.GetBytes(getPostsResponseMessage);
                        client.Send(toSendLenGetPosts);
                        client.Send(dataToClientGetPosts);
                        break;
                    
                    case RequestType.GetUsers:
                        List<User> users = DbPersistence.GetUsers();
                        if (users == null)
                        {
                            users = new List<User>();
                        }
                        string getUsersResponseMessage = JsonSerializer.Serialize(new GetUsersResponse(users));
                        int toGetUsers = Encoding.ASCII.GetByteCount(getUsersResponseMessage);
                        byte[] toSendLenGetUsers = BitConverter.GetBytes(toGetUsers);
                        byte[] dataToClientGetUsers = Encoding.ASCII.GetBytes(getUsersResponseMessage);
                        client.Send(toSendLenGetUsers);
                        client.Send(dataToClientGetUsers);
                        break;
                    
                    case RequestType.PostUser:
                        PostUserRequest postUserRequest = JsonSerializer.Deserialize<PostUserRequest>(message);
                        User newUser = DbPersistence.AddUser(postUserRequest.User);
                        string responseMessagePostUser = JsonSerializer.Serialize(new PostUserRequest(newUser));
                        int toSendPostUser = Encoding.ASCII.GetByteCount(responseMessagePostUser);
                        byte[] toSendBytesPostUser = Encoding.ASCII.GetBytes(responseMessagePostUser);
                        byte[] toSendLenBytesPostUser = BitConverter.GetBytes(toSendPostUser);
                        client.Send(toSendLenBytesPostUser);
                        client.Send(toSendBytesPostUser);
                        break;
                    
                    case RequestType.UpdatePost:
                        UpdatePostRequest updatePostRequest = JsonSerializer.Deserialize<UpdatePostRequest>(message);
                        DbPersistence.UpdatePost(updatePostRequest.Post);
                        int toSendUpdatePost = Encoding.ASCII.GetByteCount(message);
                        byte[] toSendBytesUpdatePost = Encoding.ASCII.GetBytes(message);
                        byte[] toSendLenBytesUpdatePost = BitConverter.GetBytes(toSendUpdatePost);
                        client.Send(toSendLenBytesUpdatePost);
                        client.Send(toSendBytesUpdatePost);
                        break;
                    
                    case RequestType.UpdateUser:
                        UpdateUserRequest updateUserRequest = JsonSerializer.Deserialize<UpdateUserRequest>(message);
                        DbPersistence.UpdateUser(updateUserRequest.User);
                        int toSendUpdateUser = Encoding.ASCII.GetByteCount(message);
                        byte[] toSendBytesUpdateUser = Encoding.ASCII.GetBytes(message);
                        byte[] toSendLenBytesUpdateUser = BitConverter.GetBytes(toSendUpdateUser);
                        client.Send(toSendLenBytesUpdateUser);
                        client.Send(toSendBytesUpdateUser);
                        break;
                    case RequestType.AddComment:
                        AddCommentRequest addCommentRequest = JsonSerializer.Deserialize<AddCommentRequest>(message);
                        Comment newComment = DbPersistence.AddComment(addCommentRequest.Comment);
                        string responseMessageAddComment = JsonSerializer.Serialize(new AddCommentRequest(newComment));
                        int toSendAddComment = Encoding.ASCII.GetByteCount(responseMessageAddComment);
                        byte[] toSendBytesAddComment = Encoding.ASCII.GetBytes(responseMessageAddComment);
                        byte[] toSendLenBytesAddComment = BitConverter.GetBytes(toSendAddComment);
                        client.Send(toSendLenBytesAddComment);
                        client.Send(toSendBytesAddComment);
                        break;
                    case RequestType.SendMessage:
                        SendMessageRequest sendMessageRequest = JsonSerializer.Deserialize<SendMessageRequest>(message);
                        Message newMessage = DbPersistence.SendMessage(sendMessageRequest.Message);
                        string responseMessageSendMessage =
                            JsonSerializer.Serialize(new SendMessageRequest(newMessage));
                        int toSendMessage = Encoding.ASCII.GetByteCount(responseMessageSendMessage);
                        byte[] toSendBytesMessage = Encoding.ASCII.GetBytes(responseMessageSendMessage);
                        byte[] toSendLenBytesMessage = BitConverter.GetBytes(toSendMessage);
                        client.Send(toSendLenBytesMessage);
                        client.Send(toSendBytesMessage);
                        break;
                    case RequestType.AddConversation:
                        AddConversationRequest addConversation = JsonSerializer.Deserialize<AddConversationRequest>(message);
                        List<UserConversation
                        > newUserConversations = DbPersistence.AddConversation(addConversation.Conversation,addConversation.CreatorId,addConversation.WithWhomId);
                        string responseMessageAddConversation =
                            JsonSerializer.Serialize(new AddConversationResponse(newUserConversations));
                        int toSendAddConversation = Encoding.ASCII.GetByteCount(responseMessageAddConversation);
                        byte[] toSendBytesAddConversation = Encoding.ASCII.GetBytes(responseMessageAddConversation);
                        byte[] toSendLenBytesConversation = BitConverter.GetBytes(toSendAddConversation);
                        client.Send(toSendLenBytesConversation);
                        client.Send(toSendBytesAddConversation);
                        break;
                    case RequestType.DeleteComment:
                        DeleteCommentRequest deleteCommentRequest = JsonSerializer.Deserialize<DeleteCommentRequest>(message);
                        int deletedCommentId = DbPersistence.DeleteComment(deleteCommentRequest.CommentId);
                        string responseMessageDeleteComment =
                            JsonSerializer.Serialize(new DeleteCommentRequest(deletedCommentId));
                        int toSendDeleteComment = Encoding.ASCII.GetByteCount(responseMessageDeleteComment);
                        byte[] toSendBytesDeleteComment = Encoding.ASCII.GetBytes(responseMessageDeleteComment);
                        byte[] toSendLenBytesDeleteComment = BitConverter.GetBytes(toSendDeleteComment);
                        client.Send(toSendLenBytesDeleteComment);
                        client.Send(toSendBytesDeleteComment);
                        break;
                    case RequestType.MakeFriendRequest:
                        MakeFriendRequest makeFriendRequest = JsonSerializer.Deserialize<MakeFriendRequest>(message);
                        FriendRequestNotification friendRequestNotification = DbPersistence.MakeFriendRequestNotification(makeFriendRequest.FriendRequestNotification);
                        Console.WriteLine(friendRequestNotification.FriendRequestId);
                        string responseMakeFriendRequest = 
                            JsonSerializer.Serialize(new MakeFriendRequest(friendRequestNotification));
                        int toSendMakeFriends = Encoding.ASCII.GetByteCount(responseMakeFriendRequest);
                        byte[] toSendBytesMakeFriendRequest = Encoding.ASCII.GetBytes(responseMakeFriendRequest);
                        byte[] toSendLenBytesMakeFriendRequest = BitConverter.GetBytes(toSendMakeFriends);
                        client.Send(toSendLenBytesMakeFriendRequest);
                        client.Send(toSendBytesMakeFriendRequest);
                        break;
                    case RequestType.RespondToFriendRequest:
                        RespondToFriendRequest respondToFriendRequest= JsonSerializer.Deserialize<RespondToFriendRequest>(message);
                        List<UserFriend> userFriends = DbPersistence.RespondToFriendRequest(respondToFriendRequest.RespondStatus,respondToFriendRequest.FriendRequestNotification);
                        string responseToFriendResponse = 
                            JsonSerializer.Serialize(new RespondToFriendResponse(userFriends));
                        int toSendRespondFriend = Encoding.ASCII.GetByteCount(responseToFriendResponse);
                        byte[] toSendBytesRespondFriend = Encoding.ASCII.GetBytes(responseToFriendResponse);
                        byte[] toSendLenBytesRespondFriend = BitConverter.GetBytes(toSendRespondFriend);
                        client.Send(toSendLenBytesRespondFriend);
                        client.Send(toSendBytesRespondFriend);
                        break;
                    case RequestType.SubscribeToUser:
                        SubscribeToUserRequest subscribeToUserRequest = JsonSerializer.Deserialize<SubscribeToUserRequest>(message);
                        UserSubscription userSubscription = DbPersistence.SubscribeToUser(subscribeToUserRequest.UserSubscription);
                        string responseToSubscribeToUser =
                            JsonSerializer.Serialize(new SubscribeToUserRequest(userSubscription));
                        int toSendSubscribeToUser = Encoding.ASCII.GetByteCount(responseToSubscribeToUser);
                        byte[] toSendBytesSubscribeToUser = Encoding.ASCII.GetBytes(responseToSubscribeToUser);
                        byte[] toSendLenBytesSubscribeToUser = BitConverter.GetBytes(toSendSubscribeToUser);
                        client.Send(toSendLenBytesSubscribeToUser);
                        client.Send(toSendBytesSubscribeToUser);
                        break;
                    case RequestType.UnsubscribeRequest:
                        UnsubscribeRequest unsubscribeRequest = JsonSerializer.Deserialize<UnsubscribeRequest>(message);
                        int unSubIndex = DbPersistence.UnsubscribeFromUser(unsubscribeRequest.SubscriptionId);
                        string responseUnsubscribe = 
                            JsonSerializer.Serialize(new UnsubscribeRequest(unSubIndex));
                        int toSendUnSub = Encoding.ASCII.GetByteCount(responseUnsubscribe);
                        byte[] toSendBytesUnsubscribe = Encoding.ASCII.GetBytes(responseUnsubscribe);
                        byte[] toSendLenBytesUnsubscribe = BitConverter.GetBytes(toSendUnSub);
                        client.Send(toSendLenBytesUnsubscribe);
                        client.Send(toSendBytesUnsubscribe);
                        break;
                    case RequestType.DeleteFriendRequest:
                        DeleteFriendRequest deleteFriendRequest =
                            JsonSerializer.Deserialize<DeleteFriendRequest>(message);
                        int deleteFriendIndex = DbPersistence.DeleteFriend(deleteFriendRequest.UserFriendId);
                        string responseDeleteFriend =
                            JsonSerializer.Serialize(new DeleteFriendRequest(deleteFriendIndex));
                        int toSendDeleteFriend = Encoding.ASCII.GetByteCount(responseDeleteFriend);
                        byte[] toSendBytesDeleteFriendRequest = Encoding.ASCII.GetBytes(responseDeleteFriend);
                        byte[] toSendBytesLenDeleteFriendRequest = BitConverter.GetBytes(toSendDeleteFriend);
                        client.Send(toSendBytesLenDeleteFriendRequest);
                        client.Send(toSendBytesDeleteFriendRequest);
                        break;
                    case RequestType.MakeReactionRequest:
                        MakeReactionRequest makeReactionRequest =
                            JsonSerializer.Deserialize<MakeReactionRequest>(message);
                        PostReaction postReactionResult =
                            DbPersistence.MakePostReaction(makeReactionRequest.PostReaction);
                        string responseMakePostReaction =
                            JsonSerializer.Serialize(new MakeReactionRequest(postReactionResult));
                        int toSendMakePostReaction = Encoding.ASCII.GetByteCount(responseMakePostReaction);
                        byte[] toSendBytesMakePostReaction = Encoding.ASCII.GetBytes(responseMakePostReaction);
                        byte[] toSendBytesLenMakePostReaction = BitConverter.GetBytes(toSendMakePostReaction);
                        client.Send(toSendBytesLenMakePostReaction);
                        client.Send(toSendBytesMakePostReaction);
                        break;
                    case RequestType.DeleteReactionRequest:
                        DeleteReactionRequest deleteReactionRequest =
                            JsonSerializer.Deserialize<DeleteReactionRequest>(message);
                        int deleteReactionResult =
                            DbPersistence.DeleteReaction(deleteReactionRequest.PostReactionId);
                        string responseDeletePostReaction =
                            JsonSerializer.Serialize(new DeleteReactionRequest(deleteReactionResult));
                        int toSendDeletePostReaction = Encoding.ASCII.GetByteCount(responseDeletePostReaction);
                        byte[] toSendBytesMakeDeleteReaction = Encoding.ASCII.GetBytes(responseDeletePostReaction);
                        byte[] toSendBytesLenMakeDeleteReaction = BitConverter.GetBytes(toSendDeletePostReaction);
                        client.Send(toSendBytesLenMakeDeleteReaction);
                        client.Send(toSendBytesMakeDeleteReaction);
                        break;
                    case RequestType.UpdateReactionRequest:
                        UpdateReactionRequest updateReactionRequest =
                            JsonSerializer.Deserialize<UpdateReactionRequest>(message);
                        PostReaction postReactionResultFromUpdating =
                            DbPersistence.UpdatePostReaction(updateReactionRequest.PostReaction);
                        string updateReactionResponse =
                            JsonSerializer.Serialize(new UpdateReactionRequest(postReactionResultFromUpdating));
                        int toSendUpdateBytes = Encoding.ASCII.GetByteCount(updateReactionResponse);
                        byte[] toSendBytesMakeUpdateReaction = Encoding.ASCII.GetBytes(updateReactionResponse);
                        byte[] toSendLenBytesMakeUpdateReaction = BitConverter.GetBytes(toSendUpdateBytes);
                        client.Send(toSendLenBytesMakeUpdateReaction);
                        client.Send(toSendBytesMakeUpdateReaction);
                        break;
                }
                
            }
            // close connection
            client.Close();
        }
        
    }
}