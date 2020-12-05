using System;
using FeedleDataTier.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FeedleDataTier.Data;

namespace FeedleDataTier
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddScoped<FeedleDBContext>();
            services.AddScoped<IDataBasePersistence, DataBasePersistence>();
            services.AddTransient<DataBasePersistence>();
            var serviceProvider = services.BuildServiceProvider();
            var implementation = serviceProvider.GetService<FeedleDBContext>();
            var databaseService = new DataBasePersistence(implementation);
            var socket = new SocketConnection(databaseService);
            socket.Start();
            Console.WriteLine("server is working");
        }
    }
}