using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RowaMore.Extensions;
using MasterData9002;

namespace ProgramWcfConsoleTest
{
    public class MasterData9002
    {
        public static async Task Run()
        {
            var endPoint = "net.tcp://localhost:9002/MasterData/";
            Console.WriteLine(endPoint);
            using (var serviceClient = new ServiceClient(endPoint))
            {
                serviceClient.OnClientEvent += (e) =>
                {
                    Console.WriteLine($"event=>{e.ToString()}");
                };

                serviceClient.OnSetMedicines +=  (medicines, initialSyncronization)=> 
                {
                    Console.WriteLine(medicines.ToJson());
                };

                Console.WriteLine("Not Connected");
                while (!serviceClient.IsConnected)
                {
                    System.Threading.Thread.Sleep(1000);
                    Console.Write(".");
                }
                Console.WriteLine("Connected");

                Console.WriteLine("m => get medicine");
                Console.WriteLine("e => exit");
                var running = true;
                while (running)
                {
                    var key = Console.ReadKey();
                    switch (key.KeyChar)
                    {
                        case 'a':
                            await serviceClient.SubscribeForCallbacksAsync();
                            break;
                        case 'b':
                            await serviceClient.UnsubscribeForCallbacksAsync();
                            break;
                        case 'e':
                        case 'E':
                            running = false;
                            break;

                        case 'm':
                        case 'M':
                            {
                                try
                                {
                                    Console.WriteLine("Get Medicine med1");
                                    var med = await serviceClient.GetMedicinesAsync(
                                        SearchFilter
                                            .New()
                                            .AddByName("med1"));
                                    if (med != null)
                                        Console.WriteLine($"medicine Data={med.ToJson()}");
                                    else
                                        Console.WriteLine($"med not found");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Exception {ex}");
                                }
                                break;
                            }
                    }
                }
                Console.WriteLine("THE END!");
            }
        }
    }
}
