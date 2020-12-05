﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

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
        private static void ClientThread(TcpClient client)
        {
            Console.WriteLine("Client connected,Thread started");
            NetworkStream stream = client.GetStream();
            while (true)
            {
                //read
                byte[] dataFromClient = new byte[1024];
                int bytesRead = stream.Read(dataFromClient, 0, dataFromClient.Length);
                string s = Encoding.ASCII.GetString(dataFromClient, 0, bytesRead);
                if (s.Equals("stop"))
                {
                    Console.WriteLine(s);
                    break;
                }
                //respond
                byte[] dataToClient = Encoding.ASCII.GetBytes($"Returning {s}");
                stream.Write(dataToClient, 0, dataToClient.Length);
            }

            // close connection
            client.Close();
        }
    }
}