using System;
using System.Threading;
using System.Threading.Tasks;
using RowaMore.Extensions;


namespace ProgramWcfConsoleTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await MasterData10000.Run();
            Console.WriteLine("awaiting user input");
            Console.ReadLine();
        }
    }
}
