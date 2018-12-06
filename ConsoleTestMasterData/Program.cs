using System;
using MasterData10000;

namespace ConsoleTestMasterData
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var service = new MasterDataClient("net.tcp://localhost:10000/MasterData/"))
            {
                Console.WriteLine("Hello World!");
            }
        }
    }
}
