using System;
using System.Net.Http;
using Microsoft.AspNetCore.SignalR.Client;
using WebApp.Models;

namespace SignalRDotNetClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to start listening...");
            Console.ReadKey();
            var connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:5001/signalrhub", (opts) =>
                    {
                        opts.HttpMessageHandlerFactory = (message) =>
                        {
                            if (message is HttpClientHandler clientHandler)
                                                // bypass SSL certificate
                                                clientHandler.ServerCertificateCustomValidationCallback +=
                                                (sender, certificate, chain, sslPolicyErrors) => { return true; };
                            return message;
                        };
                    })
                .Build();

            connection.On<Item>("NewItemFromOthers", (item) =>
                    Console.WriteLine($"Someone else created an item  {item.Id}"));

            connection.On("NewClientConnected", () =>
                    Console.WriteLine($"There is a new client!"));


            connection.StartAsync().GetAwaiter().GetResult();

            Console.WriteLine("Listening. Press a key to quit");

            Console.ReadKey();

        }
    }
}
