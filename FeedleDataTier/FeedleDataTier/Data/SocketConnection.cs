﻿using System;
 using System.Collections.Generic;
 using System.Net;
using System.Net.Sockets;
 using System.Runtime.CompilerServices;
 using System.Text;
 using System.Text.Json;
 using System.Threading;
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
                        String getPostsResponseMessage = JsonSerializer.Serialize(new GetPostsResponse(posts));
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
                        String getUsersResponseMessage = JsonSerializer.Serialize(new GetUsersResponse(users));
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
                        DbPersistence.AddComment(addCommentRequest.Comment);
                        int toSendAddComment = Encoding.ASCII.GetByteCount(message);
                        byte[] toSendBytesAddComment = Encoding.ASCII.GetBytes(message);
                        byte[] toSendLenBytesAddComment = BitConverter.GetBytes(toSendAddComment);
                        client.Send(toSendLenBytesAddComment);
                        client.Send(toSendBytesAddComment);
                        break;
                    case RequestType.SendMessage:
                        SendMessageRequest sendMessageRequest = JsonSerializer.Deserialize<SendMessageRequest>(message);
                        DbPersistence.SendMessage(sendMessageRequest.Message);
                        int toSendMessage = Encoding.ASCII.GetByteCount(message);
                        byte[] toSendBytesMessage = Encoding.ASCII.GetBytes(message);
                        byte[] toSendLenBytesMessage = BitConverter.GetBytes(toSendMessage);
                        client.Send(toSendLenBytesMessage);
                        client.Send(toSendBytesMessage);
                        break;
                }
                
            }
            // close connection
            client.Close();
        }
        
    }
}