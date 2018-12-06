using Rowa.Lib.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RowaLogUnitTestsExampleApp
{
    static class Program
    {
        static void Main(string[] args)
        {
            //Look at the Mode the Programm should run in 
            switch (args[0])
            {
                case "Static":
                    ExecuteStatic(args);
                    break;
                case "Core":
                    ExecuteCore(args);
                    break;
                case "ExecuteLoggingAfterDispose":
                    ExecuteLoggingAfterDispose(args);
                    break; 
                default:
                    Environment.Exit(1);
                    break; 
            }
            
        }

        public static void ExecuteCore(string[] args)
        {
            List<Task> tasks = new List<Task>();

            if (!int.TryParse(args[4], out int concurrency))
            {
                Console.WriteLine(args[4]);
                Environment.Exit(1); 
            }

            for (int i = 0; i < concurrency; i++)
            {
                Task task = new Task(() => ExecuteCoreLogger(args[1], args[2],
                                                             50,
                                                             args[3]));
                task.Start();
                tasks.Add(task);
            }

            //Wait for the Tasks to finish
            Task.WaitAll(tasks.ToArray());
        }

        /// <summary>
        /// Creates a new CoreLogger that Loggs the configured  Times to 
        /// the given Component and product and is then disposed 
        /// The logger creates Trace Logs 
        /// </summary>
        /// <param name="component"></param>
        /// <param name="product"></param>
        /// <param name="repeats">Says how often should be logged</param>
        /// <param name="message">The message that should be logged</param>
        private static void ExecuteCoreLogger(string component, string product, int repeats, string message)
        {
            //Init Manager and Log 50 Times 
            LogManagerCore manager = new LogManagerCore(component, product);

            ILog logger = manager.GetLogger("TestComponent");

            for (int i = 0; i < repeats; i++)
            {
                logger.Debug(message);
            }

            manager.Dispose();
        }

        public static void ExecuteLoggingAfterDispose(string[] args)
        {
            LogManager.Initialize(args[1], args[2]);

            ILog logger = LogManager.GetLogger("TestLogger");

            for (int i = 0; i < 50; i++)
            {
                logger.Debug(args[3]);
            }

            using (var wwi = LogManager.GetWwi("Wawi", "192.168.64.1", 6050))
            {
                wwi.LogMessage("<xml>an incoming message</xml>");
                wwi.LogMessage("<xml>an outgoing message</xml>", false);
            }
            LogManager.Cleanup();

            //Execute a Log Entry afer the Logger has been disposed
            LogManager.GetLogger("TestLogger").Debug(args[3]);
            LogManager.GetLogger("TestLogger").Debug(args[3]);
            LogManager.GetLogger("TestLogger").Debug(args[3]);
            LogManager.GetLogger("TestLogger").Debug(args[3]);

            LogManager.GetWwi("TestLogger", "192.168.100.1", 50).LogMessage("<xml>an outgoing message</xml>", false);
            LogManager.GetWwi("TestLogger", "192.168.100.1", 50).LogMessage("<xml>an outgoing message</xml>");
        }

        public static void ExecuteStatic(string[] args)
        {
            LogManager.Initialize(args[1], args[2]);

            ILog logger = LogManager.GetLogger("TestLogger");

            for (int i = 0; i < 50; i++)
            {
                logger.Debug(args[3]);
            }

            using (var wwi = LogManager.GetWwi("Wawi", "192.168.64.1", 6050))
            {
                wwi.LogMessage("<xml>an incoming message</xml>");
                wwi.LogMessage("<xml>an outgoing message</xml>", false);
            }
            LogManager.Cleanup();
        }
    }

}
