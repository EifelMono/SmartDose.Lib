using System;
using System.Threading.Tasks;
using RowaMore.Extensions;
using SmartDose.WcfLib;
using SmartDose.WcfMasterData10000;

namespace ConsoleApp1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var endPoint = "net.tcp://localhost:10000/MasterData/";
            Console.WriteLine(endPoint);
            using (var serviceClient = new ServiceClientMasterData100000(endPoint))
            {
                ModelBuilder.SetDebugInfoFlagAll(on: true);

                serviceClient.OnClientEvent += (e) =>
                {
                    Console.WriteLine($"event=>{e.ToString()}");
                };

                Console.WriteLine("Not Connected");
                while (!serviceClient.IsConnected)
                {
                    System.Threading.Thread.Sleep(1000);
                    Console.Write(".");
                }
                Console.WriteLine("Connected");

                serviceClient.OnMedicinesChanged += (listOfMedicines) =>
                {
                    Console.WriteLine("Medicine changed");
                };
                var x = await serviceClient
                    .Model<Medicine>()
                    .Read()
                    .Where(m => m.Name.Contains("a"))
                    .ExceuteToListAsync();
                if (x.IsOk())
                    Console.WriteLine(x.Data.ToJson());
                else
                    Console.WriteLine("Error");

                Console.ReadLine();
            }
        }
    }
}
