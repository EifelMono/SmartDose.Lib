using System;
using System.Threading.Tasks;
using RowaMore;
using RowaMore.Extensions;
using SmartDose.WcfLib;
using SmartDose.WcfMasterData10000;

namespace ConsoleApp1
{

    


    class Program
    {
        static void CreateClass()
        {
            var x = ClassBuilder.NewType(new ClassBuilderDefinition()
                                                .AddProperty<long>("A")
                                                .AddProperty<string>("B"));
            var s = x.ToJson();
        }

        static async Task Main(string[] args)
        {
            CreateClass();
            var endPoint = "net.tcp://localhost:10000/MasterData/";
            Console.WriteLine(endPoint);
            using (var serviceClient = new ServiceClientMasterData100000(endPoint))
            {
                ModelBuilder.DebugInfoAll(on: true); // Sql trace in ServiceResult.Debug

                serviceClient.OnClientEvent += (e) =>
                {
                    Console.WriteLine($"event=>{e.ToString()}");
                };

                serviceClient.OnMedicinesChanged += (listOfMedicines) =>
                {
                    Console.WriteLine("Medicine changed");
                };

                Console.WriteLine("Not Connected");
                while (!serviceClient.IsConnected)
                {
                    System.Threading.Thread.Sleep(1000);
                    Console.Write(".");
                }
                Console.WriteLine("Connected");

                int i = 1;
                if (i == 0)
                {
                    if (await serviceClient.ModelRead<Patient>()
                        // .Where(p => p.Gender == Gender.Male)
                        .OrderBy(p => p.Id)
                        .ExceuteFirstOrDefaultAsync() is var x && x.IsOk())
                        Console.WriteLine(x.Data.ToJson());
                    else
                        Console.WriteLine("Error");
                }

                if (i == 1)
                {
                    if (await serviceClient
                    .Model<Medicine>()
                    .Read()
                    .TableOnly()
                    // .Where(m => m.Name.Contains("a"))
                    .Select(m => new { A = m.Id, B = m.Manufacturer.Name })
                    // .OrderBy(m=> m.Identifier)
                    // .Select(m => m)
                    .ExceuteToListAsync() is var x && x.IsOk())
                    {
                        Console.WriteLine(x.Data.ToJson());
                        //foreach (var xx in x.Data)
                        //{
                        //    //Console.WriteLine(xx.ToJson());
                        //    Console.WriteLine($"A={xx.A}, B={xx.B}");
                        //}
                    }
                    else

                        Console.WriteLine("Error");
                }
                Console.ReadLine();
            }
        }
    }
}
