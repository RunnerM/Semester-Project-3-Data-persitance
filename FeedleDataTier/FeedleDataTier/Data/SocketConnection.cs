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

 namespace FeedleDataTier.Data
{
    public class SocketConnection
    {
        public IDataBasePersistence DbPersistence { get; set; }
        public IPAddress ip { get; set; }
        public TcpListener Listener { get; set; }

        public SocketConnection(IDataBasePersistence dbPersistence)
        {
            this.ip = IPAddress.Parse("127.0.0.1");
            Listener = new TcpListener(ip,5000);
            this.DbPersistence = dbPersistence;
        }

        public SocketConnection(string ipAddress, int portNumber, IDataBasePersistence dbPersistence)
        {
            this.ip = IPAddress.Parse(ipAddress);
            Listener = new TcpListener(ip,portNumber);
            this.DbPersistence = dbPersistence;
        }

        public void Start()
        {
            Listener.Start();
            Thread thread = new Thread(() =>
            {
                Console.WriteLine("Server started..");
                while (true)
                {
                    TcpClient client = Listener.AcceptTcpClient();
                    Thread clientThread = new Thread(() =>ClientThread(client));
                    clientThread.Start();
                }
            });
            thread.Start();
        }
        private void ClientThread(TcpClient client)
        {
            Console.WriteLine("Client connected,Thread started");
            NetworkStream stream = client.GetStream();
            while (true)
            {
                //read
                byte[] dataFromClient = new byte[1024];
                int bytesRead = stream.Read(dataFromClient, 0, dataFromClient.Length);
                string message = Encoding.ASCII.GetString(dataFromClient, 0, bytesRead);
                Console.WriteLine(message);
                Request request =
                    JsonSerializer.Deserialize<Request>(Encoding.ASCII.GetString(dataFromClient, 0, bytesRead));
                if (message.Equals("stop"))
                {
                    Console.WriteLine(message);
                    break;
                }
                //respond
                switch (request.Type)
                {
                    case RequestType.AddPost:
                        
                        AddPostRequest addPostRequest = JsonSerializer.Deserialize<AddPostRequest>(Encoding.ASCII.GetString(dataFromClient, 0,
                            bytesRead));
                        DbPersistence.AddPost(addPostRequest.Post);
                        byte[] dataToClientAddPost = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(addPostRequest));
                        stream.Write(dataToClientAddPost, 0, dataToClientAddPost.Length);
                        break;
                    
                    case RequestType.DeletePost :
                        
                        DeletePostRequest deletePostRequest = JsonSerializer.Deserialize<DeletePostRequest>(
                            Encoding.ASCII.GetString(dataFromClient, 0,
                                bytesRead));
                        DbPersistence.DeletePost(deletePostRequest.PostId);
                        byte[] dataToClientDeletePost = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(deletePostRequest));
                        stream.Write(dataToClientDeletePost, 0, dataToClientDeletePost.Length);
                        break;
                    
                    case RequestType.DeleteUser :

                        DeleteUserRequest deleteUserRequest = JsonSerializer.Deserialize<DeleteUserRequest>(
                            Encoding.ASCII.GetString(dataFromClient, 0, bytesRead));
                        DbPersistence.DeleteUser(deleteUserRequest.UserId);
                        byte[] dataToClientDeleteUser =
                            Encoding.ASCII.GetBytes(JsonSerializer.Serialize(deleteUserRequest));
                        stream.Write(dataToClientDeleteUser, 0, dataToClientDeleteUser.Length);
                        break;
                    
                    case RequestType.GetPosts :
                        List<Post> posts = DbPersistence.GetPosts();
                        if (posts == null)
                        {
                            posts = new List<Post>();
                        }
                        byte[] dataToClientGetPosts =
                            Encoding.ASCII.GetBytes(JsonSerializer.Serialize(new GetPostsResponse(posts)));
                        stream.Write(dataToClientGetPosts,0,dataToClientGetPosts.Length);
                        break;
                    
                    case RequestType.GetUsers:
                        List<User> users = DbPersistence.GetUsers();
                        if (users == null)
                        {
                            users = new List<User>();
                        }
                        Console.WriteLine(JsonSerializer.Serialize(new GetUsersResponse(users)));
                        byte[] dataToClientGetUsers =
                            Encoding.ASCII.GetBytes(JsonSerializer.Serialize(new GetUsersResponse(users)));
                        stream.Write(dataToClientGetUsers,0,dataToClientGetUsers.Length);
                        break;
                    
                    case RequestType.PostUser:
                        PostUserRequest postUserRequest =
                            JsonSerializer.Deserialize<PostUserRequest>(
                                Encoding.ASCII.GetString(dataFromClient, 0, bytesRead));
                        DbPersistence.AddUser(postUserRequest.User);
                        stream.Write(dataFromClient,0,dataFromClient.Length);
                        break;
                    
                    case RequestType.UpdatePost:
                        UpdatePostRequest updatePostRequest =
                            JsonSerializer.Deserialize<UpdatePostRequest>(
                                Encoding.ASCII.GetString(dataFromClient, 0, dataFromClient.Length));
                        DbPersistence.UpdatePost(updatePostRequest.Post);
                        stream.Write(dataFromClient,0,dataFromClient.Length);
                        break;
                    
                    case RequestType.UpdateUser:
                        UpdateUserRequest updateUserRequest =
                            JsonSerializer.Deserialize<UpdateUserRequest>(
                                Encoding.ASCII.GetString(dataFromClient, 0, dataFromClient.Length));
                        DbPersistence.UpdateUser(updateUserRequest.User);
                        stream.Write(dataFromClient,0,dataFromClient.Length);
                        break;
                }
                
            }
            // close connection
            client.Close();
        }
        
    }
}