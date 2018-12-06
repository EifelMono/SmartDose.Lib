using Rowa.Lib.Log;
using RowaLogUnitTests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using Xunit;
using FluentAssertions;

namespace RowaLogUnitTests.TestClasses
{

    public class CoreTests
    {
        /// <summary>
        /// Konstruktor that will be called before each Test is executed 
        /// -> Cleanup old Files from the Tests before 
        /// </summary>
        public CoreTests()
        {
            TestHelper.RemoveTestFiles().Should().BeTrue();
        }

        #region ------------- Methods -------------
        /// <summary>
        /// Creates a new CoreLogger that Loggs the configured  Times to 
        /// the given Component and product and is then disposed 
        /// The logger creates Trace Logs 
        /// </summary>
        /// <param name="component"></param>
        /// <param name="product"></param>
        /// <param name="repeats">Says how often should be logged</param>
        /// <param name="message">The message that should be logged</param>
        private void ExecuteCoreLogger(string component, string product, int repeats, string message)
        {
            //Init Manager and Log 50 Times 
            LogManagerCore manager = new LogManagerCore(component, product);

            ILog logger = manager.GetLogger("TestComponent");

            for (int i = 0; i < repeats; i++)
            {
                logger.Debug(message);
            }

            Thread.Sleep(500);

            manager.Dispose();
        }
        #endregion

        #region ------------- Facts -------------
        [Fact]
        //Tests wheter the CoreLogging supports logging on 
        //Multiplte Components
        public void CoreTestsMultipleComponents()
        {
            int repeats = 50;
            //Configure the Components that should be used
            List<ProductComponentWrapper> loggerconfig = new List<ProductComponentWrapper>()
            {
                new ProductComponentWrapper()
                {
                    Product = TestGlobals.Product,
                    Component = TestGlobals.Component
                },
                new ProductComponentWrapper()
                {
                    Product = TestGlobals.Product,
                    Component = TestGlobals.Component + "2"
                },
                new ProductComponentWrapper()
                {
                    Product = TestGlobals.Product,
                    Component = TestGlobals.Component + "3"
                },
                new ProductComponentWrapper()
                {
                    Product = TestGlobals.Product,
                    Component = TestGlobals.Component + "4"
                }
            };

            //Create Tasks to work the Components and Product
            List<Task> taks = new List<Task>();

            foreach (ProductComponentWrapper config in loggerconfig)
            {
                Task task = new Task(() => ExecuteCoreLogger(config.Product, config.Component,
                                                             repeats, TestGlobals.DefaultLogMessage));
                task.Start();
                taks.Add(task);
            }

            //Wait for all Tasks to Execute 
            bool wait = Task.WaitAll(taks.ToArray(), 5000);
            wait.Should().BeTrue();
            if (!wait)
            {
                //If the Operation takes longe than 5 Seconds
                //Dispose and make the test fail
                foreach (Task task in taks)
                {
                    task.Dispose();
                }
            }

            //Create List of expected Logs
            List<string> shouldLogEntries = new List<string>();
            for (int i = 0; i < repeats; i++)
            {
                shouldLogEntries.Add(TestGlobals.DefaultLogMessage);
            }

            //Check the Result 
            bool res = true;

            foreach (ProductComponentWrapper config in loggerconfig)
            {
                string[] writtenLines = TestHelper.ReadAllLinesFromLog(AppenderType.Trace);

                if (!TestHelper.CheckLogEntrys(shouldLogEntries.ToArray(), writtenLines, "TestComponent"))
                {
                    wait = false;
                    break;
                }
            }

            res.Should().BeTrue(); 
        }

        [Fact]
        //Tests wheter the CoreLogging supports logging on 
        //With Multiple Cores on the Same Component
        public void CoreTestsMultipleCoreSameComponent()
        {
            int repeats = 50;
            int concurrency = 4;

            //Create Tasks to work the Components and Product
            List<Task> taks = new List<Task>();

            for (int i = 0; i < concurrency; i++)
            {
                Task task = new Task(() => ExecuteCoreLogger(TestGlobals.Product,
                                                             TestGlobals.Component,
                                                             repeats,
                                                             TestGlobals.DefaultLogMessage));
                task.Start();
                taks.Add(task);
            }

            //Wait for all Tasks to Execute 
            bool wait = Task.WaitAll(taks.ToArray(), 5000);
            wait.Should().BeTrue();
            if (!wait)
            {
                //If the Operation takes longe than 5 Seconds
                //Dispose and make the test fail
                foreach (Task task in taks)
                {
                    task.Dispose();
                }
            }

            //Create List of expected Logs
            List<string> shouldLogEntries = new List<string>();
            for (int i = 0; i < repeats * concurrency; i++)
            {
                shouldLogEntries.Add(TestGlobals.DefaultLogMessage);
            }

            string[] writtenLines = TestHelper.ReadAllLinesFromLog(AppenderType.Trace);

            //Check the Result 
            TestHelper.CheckLogEntrys(shouldLogEntries.ToArray(), writtenLines, "TestComponent").Should().BeTrue(); 
        }

        [Fact]
        //Tests whter an Application is Disposed correct
        //If multiple Cores are used 
        public void CoreTestsMultipleCoreDisposeTest()
        {
            //Start a new Process, containing the RowaLogUnitTestExampleApp
            //This App Logs some example entries with some Core Instances 
            //async and calles Cleanup afterwards 
            Process prc = TestHelper.GetUnitTestExampleAppProcess(UnitTestExampleModes.Core, TestGlobals.Product,
                                                                 TestGlobals.Component, TestGlobals.DefaultLogMessage, 4);

            prc.Start();

            bool res = true;
            //If the Cleanup of the Application takes longer than 10 Seconds 
            //Something went wrong during cleanup -> e.g. a Thread is not disposed
            //Properly -> Process is killed and Test failes 
            if (!prc.WaitForExit(10000))
            {
                res = false;
                prc.Kill();
            }

            if (prc.ExitCode != 0)
            {
                res = false;
            }

            res.Should().BeTrue(); 
        }

        [Fact]
        //Test wheter zip is working correctly when multiple
        //Products and Components are used with the Core Logger 
        public void CoreTestMultipleProductAndComponentZip()
        {

        }

        #endregion
    }
}
