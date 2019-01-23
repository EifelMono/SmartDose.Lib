using System;
using System.Threading.Tasks;
using RowaMore.Extensions;
using SmartDose.WcfLib;
using SmartDose.WcfMasterData10000;

namespace ConsoleTestWcfLinq
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var endPoint = "net.tcp://localhost:10000/MasterData/";
            Console.WriteLine(endPoint);
            using (var serviceClient = new ServiceClientMasterData100000(endPoint))
            {
                ModelBuilder.DebugInfoAll(on: true);  // Sql trace for all in property ServiceResult.Debug
                // in the the sample result.Debug contains the sql!

                serviceClient.OnClientEvent += (e) => Console.WriteLine($"event=>{e.ToString()}");
                serviceClient.OnMedicinesChanged += (listOfMedicines) => Console.WriteLine("Medicine changed");

                Console.WriteLine("Not Connected");
                while (!serviceClient.IsConnected)
                {
                    System.Threading.Thread.Sleep(1000);
                    Console.Write(".");
                }
                Console.WriteLine("Connected");

                var selection = 5;
                switch (selection)
                {
                    #region short intro
                    // How to Start
                    // serviceClient
                    /// .ModelCreate<ModelName>()
                    //  .ModelDelete<ModelName>()
                    //  .ModelRead<ModelName>()
                    //  .ModelUpdate<ModelName>()
                    // or 
                    // serviceClient
                    //   .Model<ModelName>()
                    //     .Create()
                    //     .Delete()
                    //     .Read()
                    //     .Update();
                    // Read works
                    // Create, Delete, Update without function
                    // Execute 
                    //      ExecuteFirstOrDefault, ExecuteFirstOrDefaultAsync for a model Item 
                    //      ExecuteToList, ExecuteToListAsync for a list with models
                    //      with select the item is the select
                    #endregion
                    case 0:
                        {
                            // FirstOrDefault for one item
                            // result.Data = Model here Medicine
                            if (await serviceClient.ModelRead<Medicine>()
                                .Where(m => m.Name.Contains("a"))
                                .ExecFirstOrDefaultAsync() is var result && result.IsOk())
                                Console.WriteLine(result.Data.ToJson());
                            else
                                Console.WriteLine(result.ToErrorString());
                            break;
                        }
                    case 1:
                        {
                            // ToList for a list
                            // result.Data = List<Model> here List<Medicine>
                            if (await serviceClient.ModelRead<Medicine>()
                                .Where(m => m.Name.Contains("a"))
                                .ExecToListAsync() is var result && result.IsOk())
                                Console.WriteLine(result.Data.ToJson());
                            else
                                Console.WriteLine(result.ToErrorString());
                            break;
                        }
                    case 2:
                        {
                            // ToList with OrderBy or OrderByDescending
                            if (await serviceClient.ModelRead<Medicine>()
                                .Where(m => m.Name.Contains("a"))
                                .OrderByDescending(m => m.Manufacturer.Name)
                                .ExecToListAsync() is var result && result.IsOk())
                                Console.WriteLine(result.Data.ToJson());
                            else
                                Console.WriteLine(result.ToErrorString());
                            break;
                        }
                    case 3:
                        {
                            // ToList with paging
                            if (await serviceClient.ModelRead<Medicine>()
                                .Where(m => m.Name.Contains("a"))
                                .Paging(page: 2, pageSize: 10)
                                .ExecToListAsync() is var result && result.IsOk())
                                Console.WriteLine(result.Data.ToJson());
                            else
                                Console.WriteLine(result.ToErrorString());
                            break;
                        }
                    case 4:
                        {
                            {
                                // select Id of Identifier
                                // result.Data = long here the Id of Medicine
                                if (await serviceClient.ModelRead<Medicine>()
                                    .Where(m => m.Identifier == "4711")
                                    .Select(m => m.Id)
                                    .ExecFirstOrDefaultAsync() is var result && result.IsOk())
                                    Console.WriteLine(result.Data.ToJson());
                                else
                                    Console.WriteLine(result.ToErrorString());
                                // SELECT [Extent1].[Id] AS[Id]
                                // FROM[Medicine] AS[Extent1]
                                // WHERE '4711' = [Extent1].[Identifier]
                            }
                            {
                                // select Id of Identifier
                                // result.Data = long here the Id of Tray
                                if (await serviceClient.ModelRead<Tray>()
                                    .Where(m => m.Identifier == "4711")
                                    .Select(m => m.Id)
                                    .ExecFirstOrDefaultAsync() is var result && result.IsOk())
                                    Console.WriteLine(result.Data.ToJson());
                                else
                                    Console.WriteLine(result.ToErrorString());
                                // SELECT [Extent1].[Id] AS[Id]
                                // FROM[Tray] AS[Extent1]
                                // WHERE '4711' = [Extent1].[VisibleIdentifier]
                            }
                            break;
                        }
                    case 5:
                        {
                            // ATTENTION
                            // Select with new object is removed on the Serverside
                            // #undef SELECTWITHANONYMOUSTYPE in 
                            // Rowa.SmartDose.BusinessConnectors.Services.ModelsService
                            // select with new object
                            // this is slow because it needs 200ms 
                            // to create a anonymous object on the server side
                            // result.Data = a anonymous object with the properties of the select
                            if (await serviceClient
                                .Model<Medicine>().Read()
                                .Where(m => m.Name.Contains("a"))
                                .OrderByDescending(m => m.Id)
                                .Select(m => new
                                {
                                    MedicineName = m.Name,
                                    MedicineId = m.Id,
                                    MedicineIdentifier = m.Identifier,
                                    ManufacturerName = m.Manufacturer.Name
                                })
                                .ExecToListAsync() is var result && result.IsOk())
                            {
                                Console.WriteLine(result.Data.ToJson());
                                foreach (var d in result.Data)
                                    Console.WriteLine($"{d.MedicineName} {d.MedicineId} {d.MedicineIdentifier} {d.ManufacturerName}");
                            }
                            else
                                Console.WriteLine(result.ToErrorString());
                            break;
                        }
                    case 6:
                        {
                            // Count for occurrence
                            // result.Data = int 
                            if (await serviceClient
                                .ModelRead<Medicine>()
                                .ExecCountAsync(m => m.Name.Contains("a")) is var result && result.IsOk())
                                Console.WriteLine(result.Data.ToJson());
                            else
                                Console.WriteLine(result.ToErrorString());
                            break;
                        }

                    case 10:
                        {
                            // Enum does not work on the server side 
                            // at this time 
                            // for Where, OrderBy, Orderby, Select
                            if (await serviceClient.ModelRead<Patient>()
                                .Where(p => p.Gender == Gender.Male)
                                .OrderBy(p => p.Id)
                                .ExecFirstOrDefaultAsync() is var result && result.IsOk())
                                Console.WriteLine(result.Data.ToJson());
                            else
                                Console.WriteLine(result.ToErrorString());
                            break;
                        }
                }
                Console.ReadLine();
            }
        }
    }
}
