using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RowaMore.Extensions;
using MasterData10000;


namespace ProgramWcfConsoleTest
{
    public class MasterData10000
    {
        public static async Task Run()
        {
            using (var serviceClient = new ServiceClient("net.tcp://localhost:10000/MasterData/"))
            {
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

                Console.WriteLine("g => MedicinesGetMedcineByIdentifierAsync(\"1\")");
                Console.WriteLine("m,n,l => Query Medicine l=list");
                Console.WriteLine("c => Query Customer");
                Console.WriteLine("e => exit");
                bool running = true;
                while (running)
                {
                    var key = Console.ReadKey();
                    switch (key.KeyChar)
                    {
                        case 'e':
                        case 'E':
                            running = false;
                            break;
                        case 'r':
                        case 'R':
                            serviceClient.QueuedEvent.New(ServiceClientBase.ClientEvent.Restart);
                            break;
                        case 'g':
                        case 'G':
                            {
                                Console.WriteLine("MedicinesGetMedcineByIdentifierAsync");
                                if (await serviceClient.MedicinesGetMedcineByIdentifierAsync("1") is var med && med.IsOk)
                                {
                                    Console.WriteLine($"MedicinesGetMedcineByIdentifierAsync Data={med.Data.ToJson()}");
                                }
                                else
                                    Console.WriteLine($"MedicinesGetMedcineByIdentifierAsync Error Result='{med.Status}' ({med.StatusAsInt})");
                                break;
                            }
                        case 'l':
                        case 'L':
                            {
                                {
                                    Console.WriteLine("--Query Medicines A* ");
                                    if (await serviceClient
                                                .NewQuery<Medicine>()
                                                .Where(m => m.Name.StartsWith("A"))
                                                .ToListAsync() is var medList && medList.IsOk)
                                    {
                                        // Console.WriteLine($"Query medicine Data={medList.Data.ToJson()}");
                                        // Console.WriteLine($"sql {medList.Debug}");
                                        medList.Data.ForEach(m => Console.WriteLine(m.Name));
                                    }
                                    else
                                        Console.WriteLine($"Query medicine Error Result='{medList.Status}' ({medList.StatusAsInt})");
                                }
                                {
                                    Console.WriteLine("--Query Medicines A* Paging(2,2) (no orderby internal by key)");
                                    if (await serviceClient
                                                .NewQuery<Medicine>()
                                                .Where(m => m.Name.StartsWith("A"))
                                                .Paging(2, 2)
                                                .ToListAsync() is var medList && medList.IsOk)
                                    {
                                        // Console.WriteLine($"Query medicine Data={medList.Data.ToJson()}");
                                        // Console.WriteLine($"sql {medList.Debug}");
                                        medList.Data.ForEach(m => Console.WriteLine(m.Name));
                                    }
                                    else
                                        Console.WriteLine($"Query medicine Error Result='{medList.Status}' ({medList.StatusAsInt})");
                                }
                                {
                                    Console.WriteLine("--Query Medicines orderby Identifier");
                                    if (await serviceClient
                                                .NewQuery<Medicine>()
                                                .Where(m => m.Name.StartsWith("A"))
                                                .OrderBy(m => m.Identifier)
                                                .ToListAsync() is var medList && medList.IsOk)
                                    {
                                        // Console.WriteLine($"Query medicine Data={medList.Data.ToJson()}");
                                        // Console.WriteLine($"sql {medList.Debug}");
                                        medList.Data.ForEach(m => Console.WriteLine(m.Name));
                                    }
                                    else
                                        Console.WriteLine($"Query medicine Error Result='{medList.Status}' ({medList.StatusAsInt})");
                                }
                                {
                                    Console.WriteLine("--Query Medicines orderbydescending Identifier");
                                    if (await serviceClient
                                                .NewQuery<Medicine>()
                                                .Where(m => m.Name.StartsWith("A"))
                                                .OrderByDescending(m => m.Identifier)
                                                .ToListAsync() is var medList && medList.IsOk)
                                    {
                                        // Console.WriteLine($"Query medicine Data={medList.Data.ToJson()}");
                                        // Console.WriteLine($"sql {medList.Debug}");
                                        medList.Data.ForEach(m => Console.WriteLine(m.Name));
                                    }
                                    else
                                        Console.WriteLine($"Query medicine Error Result='{medList.Status}' ({medList.StatusAsInt})");
                                }
                                {
                                    Console.WriteLine("--Query Medicines orderby Identifier Paging");
                                    if (await serviceClient
                                                .NewQuery<Medicine>()
                                                .Where(m => m.Manufacturer.Name.StartsWith("A"))
                                                .OrderBy(m => m.Manufacturer.Name)
                                                .Paging(1, 2)
                                                .ToListAsync() is var medList && medList.IsOk)
                                    {
                                        // Console.WriteLine($"Query medicine Data={medList.Data.ToJson()}");
                                        // Console.WriteLine($"sql {medList.Debug}");
                                        medList.Data.ForEach(m => Console.WriteLine(m.Name));
                                    }
                                    else
                                        Console.WriteLine($"Query medicine Error Result='{medList.Status}' ({medList.StatusAsInt})");
                                }
                                break;
                            }
                        case 'm':
                        case 'M':
                            {
                                {
                                    Console.WriteLine("Query Medicine med1");
                                    if (await serviceClient
                                                .NewQuery<Medicine>()
                                                .Where(m => m.Name == "med1")
                                                .FirstOrDefaultAsync() is var med && med.IsOk)
                                    {
                                        Console.WriteLine($"Query medicine Data={med.Data.ToJson()}");
                                    }
                                    else
                                        Console.WriteLine($"Query medicine Error Result='{med.Status}' ({med.StatusAsInt})");
                                }

                                {
                                    Console.WriteLine("Query Medicine 900004106");
                                    if (await serviceClient
                                                .NewQuery<Medicine>()
                                                .Where(m => m.Identifier == "900004106")
                                                .FirstOrDefaultAsync() is var med && med.IsOk)
                                    {
                                        Console.WriteLine($"Query medicine Data={med.Data.ToJson()}");
                                    }
                                    else
                                        Console.WriteLine($"Query medicine Error Result='{med.Status}' ({med.StatusAsInt})");
                                }
                                break;
                            }


                        case 'n':
                        case 'N':
                            {
                                var value = "ddasdddd";
                                Console.WriteLine("Query Medicine");
                                if (await serviceClient
                                            .NewQuery<Medicine>()
                                            .Where(m => m.Name == value)
                                            .OrderBy(m => m.Manufacturer.Name == value)
                                            .FirstOrDefaultAsync() is var med && med.IsOk)
                                {
                                    Console.WriteLine($"Query medicine Data={med.Data.ToJson()}");
                                }
                                else
                                    Console.WriteLine($"Query medicine Error Result='{med.Status}' ({med.StatusAsInt})");
                                break;
                            }
                        case 'c':
                        case 'C':
                            {
                                Console.WriteLine("Query Customer");
                                if (await serviceClient
                                            .NewQuery<Customer>()
                                            .Where(m => m.Name == "2dd")
                                            .FirstOrDefaultAsync() is var med && med.IsOk)
                                {
                                    Console.WriteLine($"Query Customer Data={med.Data.ToJson()}");
                                }
                                else
                                    Console.WriteLine($"Query Customer Error Result='{med.Status}' ({med.StatusAsInt})");
                                break;
                            }
                    }
                }
                Console.WriteLine("THE END!");
            }
        }
    }
}
