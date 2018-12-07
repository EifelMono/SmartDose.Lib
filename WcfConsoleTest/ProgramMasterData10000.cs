using System;
using System.Threading.Tasks;
using RowaMore.Extensions;
using MasterData10000;


namespace ProgramWcfConsoleTest
{
    public class MasterData10000
    {
        public static async Task Run()
        {
            var endPoint = "net.tcp://localhost:10000/MasterData/";
            Console.WriteLine(endPoint);
            using (var serviceClient = new ServiceClient(endPoint))
            {
                QueryBuilder.SwitchDebugInfoAll(true);

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


                Console.WriteLine("a => async Test");
                Console.WriteLine("c => Query Customer");
                Console.WriteLine("g => MedicinesGetMedcineByIdentifierAsync(\"1\")");
                Console.WriteLine("m,n,l => Query Medicine l=list");
                Console.WriteLine("t => test medicine query");
                Console.WriteLine("e => exit");
                bool running = true;
                while (running)
                {
                    var key = Console.ReadKey();
                    if (!serviceClient.IsConnected)
                        Console.WriteLine("Not Connected");
                    else
                        switch (key.KeyChar)
                        {
                            case 'd':
                            case 'D':
                                {
                                    Console.WriteLine("Test Debug");
                                    var meda = await serviceClient
                                        .ModelQuery<Medicine>()
                                        .FirstOrDefaultAsync(m => m.Name == "med1");
                                    Console.WriteLine($"Query medicine result={meda.ToJson()}");
                                    var medb = await serviceClient
                                        .ModelQuery<Medicine>()
                                        .UseTableOnly()
                                        .FirstOrDefaultAsync(m => m.Name == "med1");
                                    Console.WriteLine($"Query medicine result={medb.ToJson()}");
                                    break;
                                }
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
                                                    .ModelQuery<Medicine>()
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
                                                    .ModelQuery<Medicine>()
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
                                                    .ModelQuery<Medicine>()
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
                                                    .ModelQuery<Medicine>()
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
                                                    .ModelQuery<Medicine>()
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
                                                    .ModelQuery<Medicine>()
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
                                                    .ModelQuery<Medicine>()
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
                                                .ModelQuery<Medicine>()
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
                                                .ModelQuery<Customer>()
                                                .Where(m => m.Name == "2dd")
                                                .FirstOrDefaultAsync() is var med && med.IsOk)
                                    {
                                        Console.WriteLine($"Query Customer Data={med.Data.ToJson()}");
                                    }
                                    else
                                        Console.WriteLine($"Query Customer Error Result='{med.Status}' ({med.StatusAsInt})");
                                    break;
                                }
                            case 'f':
                            case 'F':
                                {
                                    Console.WriteLine("Query Medicine");
                                    if (await serviceClient
                                                .ModelQuery<Medicine>()
                                                .Where(m => m.Manufacturer.Name == "Andreas")
                                                .FirstOrDefaultAsync() is var med && med.IsOk)
                                    {
                                        Console.WriteLine($"Query Medicine Data={med.Data.ToJson()}");
                                    }
                                    else
                                        Console.WriteLine($"Query Medicine Error Result='{med.Status}' ({med.StatusAsInt})");
                                    break;
                                }
                            case 't':
                            case 'T':
                                {
                                    Console.WriteLine("Query Medicine");
                                    if (await serviceClient
                                                .ModelQuery<Medicine>()
                                                .FirstOrDefaultAsync(m => m.Name == "med1") is var med && med.IsOk)
                                        Console.WriteLine($"Query medicine Data={med.Data.ToJson()}");
                                    else
                                        Console.WriteLine($"Query medicine Error Result='{med.Status}' ({med.StatusAsInt})");

                                    if (await serviceClient
                                               .ModelIdentifierToIdAsync<Medicine>(med.Data.Identifier) is var id1 && id1.IsOk)
                                        Console.WriteLine($"IdentifierToId id {med.Data.Id} found for {med.Data.Identifier}");
                                    else
                                        Console.WriteLine($"IdentifierToId id not found for {med.Data.Identifier} Error Result='{id1.Status}' ({id1.StatusAsInt})");
                                    break;
                                }
                        }
                }
                Console.WriteLine("THE END!");
            }
        }
    }
}
