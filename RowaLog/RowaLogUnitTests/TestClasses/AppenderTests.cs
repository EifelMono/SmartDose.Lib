using Rowa.Lib.Log;
using RowaLogUnitTests.Common;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using FluentAssertions;

namespace RowaLogUnitTests.TestClasses
{
    public class AppenderTests
    {
        /// <summary>
        /// Konstruktor that will be called before each Test is executed 
        /// -> Cleans the old files 
        /// </summary>
        public AppenderTests()
        {
            TestHelper.RemoveTestFiles().Should().BeTrue();
        }

        [Fact]
        //Checks if the Logs for the TraceAppender are logged correctly
        public void AppenderTestTrace()
        {
            LogManager.Initialize(TestGlobals.Product, TestGlobals.Component);

            ILog logger = LogManager.GetLogger("Loggertest");
            //Write 20 Debug Logs
            List<string> logMessages = new List<string>();
            for (int i = 0; i< 20; i++)
            {
                logMessages.Add(TestGlobals.DefaultLogMessage);
                logger.Debug(TestGlobals.DefaultLogMessage); 
            }

            LogManager.Cleanup();

            string[] writtenLogs = TestHelper.ReadAllLinesFromLog(AppenderType.Trace);

            TestHelper.CheckLogEntrys(logMessages.ToArray(), writtenLogs, "Loggertest").Should().BeTrue();
        }

        [Fact]
        //Checks if the Error Logs are written in the Error and the Trace Log 
        public void AppenderTestError()
        { 
            LogManager.Initialize(TestGlobals.Product, TestGlobals.Component);

            ILog logger = LogManager.GetLogger("Loggertest");
            //Write 20 Debug Logs
            List<string> logMessages = new List<string>();
            for (int i = 0; i < 20; i++)
            {
                logMessages.Add(TestGlobals.DefaultLogMessage);
                logger.Fatal(TestGlobals.DefaultLogMessage);
            }

            LogManager.Cleanup();

            string[] writtenErrorLogs = TestHelper.ReadAllLinesFromLog(AppenderType.Error);
            string[] writtenTraceLogs = TestHelper.ReadAllLinesFromLog(AppenderType.Trace);

            //Only of both have worked the Test will be successfull
            TestHelper.CheckLogEntrys(logMessages.ToArray(), writtenErrorLogs, "Loggertest").Should().BeTrue();
            TestHelper.CheckLogEntrys(logMessages.ToArray(), writtenTraceLogs, "Loggertest").Should().BeTrue();
        }

        [Fact]
        //Checks if the Error Logs are written in the Interaction and the Trace Log 
        public void AppenderTestInteraction()
        {
            LogManager.Initialize(TestGlobals.Product, TestGlobals.Component);

            ILog logger = LogManager.GetLogger("Loggertest");
            //Write 20 Debug Logs
            List<string> logMessages = new List<string>();
            for (int i = 0; i < 20; i++)
            {
                logMessages.Add(TestGlobals.DefaultLogMessage);
                logger.UserIn(TestGlobals.DefaultLogMessage);
            }

            LogManager.Cleanup();

            string[] writtenInteractionLogs = TestHelper.ReadAllLinesFromLog(AppenderType.Interaction);
            string[] writtenTraceLogs = TestHelper.ReadAllLinesFromLog(AppenderType.Trace);

            //Only of both have worked the Test will be successfull
            TestHelper.CheckLogEntrys(logMessages.ToArray(), writtenInteractionLogs, "Loggertest").Should().BeTrue();
            TestHelper.CheckLogEntrys(logMessages.ToArray(), writtenTraceLogs, "Loggertest").Should().BeTrue();            
        }

        [Fact]
        //Checks if the Error Logs are written in the Audit and the Trace Log 
        public void AppenderTestAudit()
        {
            LogManager.Initialize(TestGlobals.Product, TestGlobals.Component);

            ILog logger = LogManager.GetLogger("Loggertest");
            //Write 20 Debug Logs
            List<string> logMessages = new List<string>();
            for (int i = 0; i < 20; i++)
            {
                logMessages.Add(TestGlobals.DefaultLogMessage);
                logger.Audit(TestGlobals.DefaultLogMessage);
            }

            LogManager.Cleanup();

            string[] writtenAuditLogs = TestHelper.ReadAllLinesFromLog(AppenderType.Audit);
            string[] writtenTraceLogs = TestHelper.ReadAllLinesFromLog(AppenderType.Trace);

            //Only of both have worked the Test will be successfull
            TestHelper.CheckLogEntrys(logMessages.ToArray(), writtenAuditLogs, "Loggertest").Should().BeTrue(); 
            TestHelper.CheckLogEntrys(logMessages.ToArray(), writtenTraceLogs, "Loggertest").Should().BeTrue();
        }

        [Fact]
        //Checks if the Error Logs are written at the Console and in the Trace Log 
        public void AppenderTestConsole()
        {
            LogManager.Initialize(TestGlobals.Product, TestGlobals.Component);

            ILog logger = LogManager.GetLogger("Loggertest");
            //Write 20 Debug Logs
            List<string> logMessages = new List<string>();
            string[] writtenConsoleLogs;
            //Log and Read in the Console Output
            using (var writer = new StringWriter())
            {
                Console.SetOut(writer);

                for (int i = 0; i < 20; i++)
                {
                    logMessages.Add(TestGlobals.DefaultLogMessage);
                    logger.Debug(TestGlobals.DefaultLogMessage);
                }

                LogManager.Cleanup();

                writer.Flush(); // when you're done, make sure everything is written out

                //Split the console Output to a new string[]
                string output = writer.GetStringBuilder().ToString();
                writtenConsoleLogs = output.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            }

            //Cut of the last entry if it is null or empty 
            writtenConsoleLogs = writtenConsoleLogs.RemoveLastEntryIfEmpty();

            string[] writtenTraceLogs = TestHelper.ReadAllLinesFromLog(AppenderType.Trace);

            //Only of both have worked the Test will be successfull
            TestHelper.CheckLogEntrys(logMessages.ToArray(), writtenConsoleLogs, "Loggertest").Should().BeTrue();
            TestHelper.CheckLogEntrys(logMessages.ToArray(), writtenTraceLogs, "Loggertest").Should().BeTrue(); 
        }

        [Fact]
        //Checks a combination of all appenders in order to test wheter the seperation
        //also works in different combinations
        public void AppenderTestAll()
        {
            LogManager.Initialize(TestGlobals.Product, TestGlobals.Component);

            ILog logger = LogManager.GetLogger("Loggertest");

            //Create the Lists to hold the Values for the Varius Logs
            List<string> logMessages = new List<string>();
            List<string> logMessagesError = new List<string>();
            List<string> logMessagesAudit = new List<string>();
            List<string> logMessagesInteraction = new List<string>();
            string[] writtenConsoleLogs;

            //Log and Read in the Console Output
            using (var writer = new StringWriter())
            {
                Console.SetOut(writer);

                //Write Serveral Log Messages and add them to the right list 

                //Debug Log 
                logMessages.Add(TestGlobals.DefaultLogMessage);
                logger.Debug(TestGlobals.DefaultLogMessage);

                logMessages.Add(TestGlobals.DefaultLogMessage);
                logger.Info(TestGlobals.DefaultLogMessage);

                //Audit Log 
                logger.Audit(TestGlobals.DefaultLogMessage);
                logMessages.Add(TestGlobals.DefaultLogMessage);
                logMessagesAudit.Add(TestGlobals.DefaultLogMessage);

                //Error Logs 
                logger.Warning(TestGlobals.DefaultLogMessage);
                logMessages.Add(TestGlobals.DefaultLogMessage);
                logMessagesError.Add(TestGlobals.DefaultLogMessage);

                logger.Error(TestGlobals.DefaultLogMessage);
                logMessages.Add(TestGlobals.DefaultLogMessage);
                logMessagesError.Add(TestGlobals.DefaultLogMessage);

                logger.Fatal(TestGlobals.DefaultLogMessage);
                logMessages.Add(TestGlobals.DefaultLogMessage);
                logMessagesError.Add(TestGlobals.DefaultLogMessage);

                //Interaction Logs
                logger.ExtIf(TestGlobals.DefaultLogMessage);
                logMessages.Add(TestGlobals.DefaultLogMessage);
                logMessagesInteraction.Add(TestGlobals.DefaultLogMessage);

                logger.UserIn(TestGlobals.DefaultLogMessage);
                logMessages.Add(TestGlobals.DefaultLogMessage);
                logMessagesInteraction.Add(TestGlobals.DefaultLogMessage);

                //Cleanup and Read in the Console Messages 
                LogManager.Cleanup();

                writer.Flush(); // when you're done, make sure everything is written out

                //Split the console Output to a new string[]
                string output = writer.GetStringBuilder().ToString();
                writtenConsoleLogs = output.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            }

            //Cut of the last entry if it is null or empty 
            writtenConsoleLogs = writtenConsoleLogs.RemoveLastEntryIfEmpty();

            //Read in the different Logs and then test if everythign was correct 
            string[] writtenTraceLogs = TestHelper.ReadAllLinesFromLog(AppenderType.Trace);
            string[] writterErrorLogs = TestHelper.ReadAllLinesFromLog(AppenderType.Error);
            string[] writtenInteractionLogs = TestHelper.ReadAllLinesFromLog(AppenderType.Interaction);
            string[] writtenAuditLogs = TestHelper.ReadAllLinesFromLog(AppenderType.Audit);

            //Only of all have worked the Test will be successfull
            TestHelper.CheckLogEntrys(logMessages.ToArray(), writtenTraceLogs, "Loggertest").Should().BeTrue(); 
            TestHelper.CheckLogEntrys(logMessagesError.ToArray(), writterErrorLogs, "Loggertest").Should().BeTrue();
            TestHelper.CheckLogEntrys(logMessagesInteraction.ToArray(), writtenInteractionLogs, "Loggertest").Should().BeTrue();
            TestHelper.CheckLogEntrys(logMessagesAudit.ToArray(), writtenAuditLogs, "Loggertest").Should().BeTrue();
            TestHelper.CheckLogEntrys(logMessages.ToArray(), writtenConsoleLogs, "Loggertest").Should().BeTrue();
        }
    }
}
