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
                ModelBuilder.DebugInfoAll(on: true);

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
                    .TableOnly()
                    .Where(m => m.Name.Contains("a"))
                    // .Select(m => new { A = m.Id, B = m.Identifier })
                    .Select(m => m.Id)
                    .ExceuteToListAsync();
                if (x.IsOk())
                {
                    Console.WriteLine(x.Data.ToJson());
                    foreach (var xx in x.Data)
                    {
                        Console.WriteLine(xx.ToJson());
                       // Console.WriteLine($"A={xx.A}, B={xx.B}");
                    }
                }
                else
                    Console.WriteLine("Error");

                Console.ReadLine();
            }
        }
    }
}
