﻿using System;
using RowaMore.Extensions;
using SmartDose.WcfLib;
using SmartDose.WcfMasterData10000;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var endPoint = "net.tcp://localhost:10000/MasterData/";
            Console.WriteLine(endPoint);
            using (var serviceClient = new ServiceClientMasterData100000(endPoint))
            {
                ModelBuilder.SwitchDebugInfoFlagAll(on: true);

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
                var x = serviceClient
                    .Model<Medicine>()
                    .Read()
                    .FirstOrDefault(m => m.Name == "med1");
                if (x.IsOk())
                    Console.WriteLine(x.ToJson());
                else
                    Console.WriteLine("Error");

                Console.ReadLine();

            }
        }
    }
}
