using FluentAssertions;
using Rowa.Lib.Log;
using RowaLogUnitTests.Common;
using RowaLogUnitTests.ReadAndWriteData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RowaLogUnitTests.TestClasses
{
    public class ErrorTest
    {
        /// <summary>
        /// Konstruktor that will be called before each Test is executed 
        /// -> Cleanup old Files from the Tests before 
        /// </summary>
        public ErrorTest()
        {
            TestHelper.RemoveTestFiles().Should().BeTrue();
        }

        /// <summary>
        /// Test that blocks the Log File and tests the Logger behavoir
        /// </summary>
        [Fact]
        public void LoggerFileBlockingTest()
        {
            //Write a TestMessage 
            LogManagerCore core = new LogManagerCore(TestGlobals.Product, TestGlobals.Component);

            ILog logger = core.GetLogger("TestLogger");

            logger.Debug(TestGlobals.DefaultLogMessage);

            core.Dispose();

            //Get the Filename for the Debug Log File 
            string[] files = Directory.GetFiles(TestGlobals.DefaultLogFileDirectory);

            files.Length.Should().Be(1); 

            string file = files[0];

            List<string> actualMessages = new List<string>
            {
                TestGlobals.DefaultLogMessage
            };

            //Write More Logs than the max LogQueue Size and check for Error 
            LogManagerCore coreManager = new LogManagerCore(TestGlobals.Product, TestGlobals.Component);

            //Get exclusive Access for that File 
            using (FileStream stream = new FileStream(file, FileMode.Append, FileAccess.Write, FileShare.None))
            {
                ILog loggerQueueBlocking = coreManager.GetLogger("TestLogger");

                //Write 1 Log More than the Max Queue Size 
                for (int i = 0; i <= 50; i++)
                {
                    loggerQueueBlocking.Debug(TestGlobals.DefaultLogMessage);
                    actualMessages.Add(TestGlobals.DefaultLogMessage);
                }

            }

            //Give Enough Time for the Event to hit
            Thread.Sleep(2000);

            coreManager.Dispose();

            string[] writtenLogs = TestHelper.ReadAllLinesFromLog(AppenderType.Trace);

            TestHelper.CheckLogEntrys(actualMessages.ToArray(), writtenLogs, "TestLogger").Should().BeTrue(); 
        }

        /// <summary>
        /// Test that tries to Fill be LogQueue 
        /// </summary>
        [Fact]
        public void LogQueueFullTest()
        {
            //Write a TestMessage 
            LogManagerCore core = new LogManagerCore(TestGlobals.Product, TestGlobals.Component);

            ILog logger = core.GetLogger("TestLogger");

            logger.Debug(TestGlobals.DefaultLogMessage);

            core.Dispose();

            //Get the Filename for the Debug Log File 
            string[] files = Directory.GetFiles(TestGlobals.DefaultLogFileDirectory);

            files.Length.Should().Be(1); 

            string file = files[0];

            bool _eventWasSet = false;

            //Get exclusive Access for that File 
            using (FileStream stream = new FileStream(file, FileMode.Append, FileAccess.Write, FileShare.None))
            {
                //Write More Logs than the max LogQueue Size and check for Error 
                LogManagerCore coreManager = new LogManagerCore(TestGlobals.Product, TestGlobals.Component);

                coreManager.OnError += OnLoggerError;
                
                ILog loggerFullQueue = coreManager.GetLogger("TestLogger");

                //Write 1 Log More than the Max Queue Size 
                for (int i = 0; i <= TestGlobals.MaxQueueSize + 1; i++)
                {
                    loggerFullQueue.Debug(TestGlobals.DefaultLogFileDirectory);
                }

                coreManager.Dispose();
            }

            //Give Enough Time for the Event to hit
            Thread.Sleep(2000);

            _eventWasSet.Should().BeTrue(); 

            void OnLoggerError(LogErrorEventArgs args)
            {
                if (args.Error == LoggerError.LogQueueFull)
                {
                    _eventWasSet = true;
                }
            }
        }
    }
}
