using Rowa.Lib.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    static class Program
    {
        static void Main(string[] args)
        {

            LogManager.Initialize("RowaLogUnittestProduct", "RowaLogUnittestComponent" + args[0]);

            Console.WriteLine("LogManager was initialized"); 

            Console.ReadKey();

            Console.WriteLine("Key was pressed, finishing!"); 

            LogManager.Cleanup();

            Console.WriteLine("finished!");

            Console.ReadKey(); 
        }
    }
}
