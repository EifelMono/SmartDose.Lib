using FluentAssertions;
using Rowa.Lib.Log;
using Rowa.Lib.Log.Types;
using RowaLogUnitTests.Common;
using RowaLogUnitTests.ReadAndWriteData;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace RowaLogUnitTests.TestClasses
{
    public class LoggerTests
    {
        /// <summary>
        /// Konstruktor that will be called before each Test is executed 
        /// -> Cleanup old Files from the Tests before 
        /// </summary>
        public LoggerTests()
        {
            TestHelper.RemoveTestFiles().Should().BeTrue();
        }

        [Fact]
        //Tests wheter the Appender Log Entry works
        public void LoggerTestsAppender()
        {
            LogManager.Initialize(TestGlobals.Product, TestGlobals.Component);

            ILog logger = LogManager.GetLogger("TestLogger");

            List<string> acutalMessages = new List<string>();

            for (int i = 0; i < 5; i++)
            {
                logger.Append(new LogEntry()
                {
                    Level = LogLevel.Debug,
                    Format = TestGlobals.DefaultLogMessage,
                });
                acutalMessages.Add(TestGlobals.DefaultLogMessage);
            }

            LogManager.Cleanup();

            string[] writtenLogs = TestHelper.ReadAllLinesFromLog(AppenderType.Trace);

            TestHelper.CheckLogEntrys(acutalMessages.ToArray(), writtenLogs, "TestLogger").Should().BeTrue(); 
        }

        [Fact]
        //Tests wheter the Appender Log Entry works
        public void GetTypeLoggerTest()
        {
            LogManager.Initialize(TestGlobals.Product, TestGlobals.Component);

            ILog logger = LogManager.GetLogger(typeof(Helper.LoggerTypeTestType));
            ILog logger2 = LogManager.GetLogger(typeof(Helper.Types.LoggerTypeTestType));

            List<string> acutalInteractionMessages = new List<string>();
            List<string> acutalAuditMessages = new List<string>();

            for (int i = 0; i < 5; i++)
            {
                logger.Append(new LogEntry()
                {
                    Level = LogLevel.Userin,
                    Format = TestGlobals.DefaultLogMessage,
                });
                logger2.Append(new LogEntry()
                {
                    Level = LogLevel.Audit,
                    Format = TestGlobals.DefaultLogMessage,
                });
                acutalInteractionMessages.Add(TestGlobals.DefaultLogMessage);
                acutalAuditMessages.Add(TestGlobals.DefaultLogMessage);
            }

            LogManager.Cleanup();

            string[] writtenInteractionLogs = TestHelper.ReadAllLinesFromLog(AppenderType.Interaction);
            string[] writtenAuditLogs = TestHelper.ReadAllLinesFromLog(AppenderType.Audit);

            TestHelper.CheckLogEntrys(acutalInteractionMessages.ToArray(), writtenInteractionLogs, typeof(Helper.LoggerTypeTestType).ToString()).Should().BeTrue();
            TestHelper.CheckLogEntrys(acutalAuditMessages.ToArray(), writtenAuditLogs, typeof(Helper.Types.LoggerTypeTestType).ToString()).Should().BeTrue();
        }

        [Fact]
        ///Writes a Debug Entry to a Logger after the Logger has been disposed
        public void LoggerTestWriteDisposedLogger()
        {
            LogManager.Initialize(TestGlobals.Product, TestGlobals.Component);

            ILog logger = LogManager.GetLogger("TestLogger");

            List<string> acutalMessages = new List<string>();

            for (int i = 0; i < 5; i++)
            {
                logger.Append(new LogEntry()
                {
                    Level = LogLevel.Debug,
                    Format = TestGlobals.DefaultLogMessage,
                });
                acutalMessages.Add(TestGlobals.DefaultLogMessage);
            }

            LogManager.Cleanup();

            logger.Debug(TestGlobals.DefaultLogMessage);

            string[] writtenLogs = TestHelper.ReadAllLinesFromLog(AppenderType.Trace);

            TestHelper.CheckLogEntrys(acutalMessages.ToArray(), writtenLogs, "TestLogger").Should().BeTrue(); 
        }

        [Fact]
        ///Writes a Debug Entry to a Logger after the Logger has been disposed
        public void LoggerTestWriteDisposedWwiLogger()
        { 
            LogManager.Initialize(TestGlobals.Product, TestGlobals.Component);

            Dictionary<string, bool> acutalMessages = new Dictionary<string, bool>();

            string desc = "WaWi";
            string ip = "192.168.64.1";
            ushort port = 6050;

            IWwi wwi = LogManager.GetWwi(desc, ip, port);

            wwi.LogMessage("<xml>an incoming message</xml>");
            acutalMessages.Add("<xml>an incoming message</xml>", true);
            wwi.LogMessage("<xml>an outgoing message</xml>", false);
            acutalMessages.Add("<xml>an outgoing message</xml>", false);

            wwi.Dispose();

            LogManager.Cleanup();

            IWwi wwi2 = LogManager.GetWwi(desc, ip, port);
            wwi2.LogMessage("<xml>an incoming message</xml>");
            wwi.LogMessage("<xml>an incoming message</xml>");

            string[] writtenLogs = TestHelper.ReadAllLinesFromWawiLog(desc, ip, port);

            TestHelper.CheckWaWiLogEntrys(acutalMessages, writtenLogs).Should().BeTrue();
        }

        /// <summary>
        /// Test wheter it is possible to Dispose a Logger after 
        /// each write and then get a new one
        /// </summary>
        [Fact]
        public void LoggerWwiTestDiposeAfterEachWrite()
        {
            LogManager.Initialize(TestGlobals.Product, TestGlobals.Component);

            //Write 20 Debug Logs
            string desc = "WaWi";
            string ip = "192.168.64.1";
            ushort port = 6050;

            Dictionary<string, bool> acutalMessages = new Dictionary<string, bool>();

            for (int i = 0; i < 50; i++)
            {
                IWwi wwi = LogManager.GetWwi(desc, ip, port);
                if (i % 2 == 0)
                {
                    wwi.LogMessage($"<xml>{i} an incoming message</xml>");
                    acutalMessages.Add($"<xml>{i} an incoming message</xml>", true);
                }
                else
                {
                    wwi.LogMessage($"<xml>{i} an incoming message</xml>", false);
                    acutalMessages.Add($"<xml>{i} an incoming message</xml>", false);
                }
                wwi.Dispose();
            }

            LogManager.Cleanup();

            string[] writtenLogs = TestHelper.ReadAllLinesFromWawiLog(desc, ip, port);

            TestHelper.CheckWaWiLogEntrys(acutalMessages, writtenLogs).Should().BeTrue();
        }


        [Fact]
        //Tests wheter the PLC Logger is settings the right Header, 
        //and wheter the Logs have the right Format
        public void LoggerTestsPLC()
        {
            LogManager.Initialize(TestGlobals.Product, TestGlobals.Component);

            ILog logger = LogManager.GetPLCLogger("Loggertest");

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
                string module = "Belt[1]";
                //Write Serveral Log Messages and add them to the right list 

                //Debug Log 
                logMessages.Add(TestGlobals.DefaultLogMessage);
                logger.Append(new LogEntry()
                {
                    Level = LogLevel.Debug,
                    Format = TestGlobals.DefaultLogMessage,
                    Module = module,
                });

                logMessages.Add(TestGlobals.DefaultLogMessage);
                logger.Append(new LogEntry()
                {
                    Level = LogLevel.Info,
                    Format = TestGlobals.DefaultLogMessage,
                    Module = module,
                });

                //Audit Log 
                logger.Append(new LogEntry()
                {
                    Level = LogLevel.Audit,
                    Format = TestGlobals.DefaultLogMessage,
                    Module = module,
                });
                logMessages.Add(TestGlobals.DefaultLogMessage);
                logMessagesAudit.Add(TestGlobals.DefaultLogMessage);

                //Error Logs 
                logger.Append(new LogEntry()
                {
                    Level = LogLevel.Warn,
                    Format = TestGlobals.DefaultLogMessage,
                    Module = module,
                });
                logMessages.Add(TestGlobals.DefaultLogMessage);
                logMessagesError.Add(TestGlobals.DefaultLogMessage);

                logger.Append(new LogEntry()
                {
                    Level = LogLevel.Error,
                    Format = TestGlobals.DefaultLogMessage,
                    Module = module,
                });
                logMessages.Add(TestGlobals.DefaultLogMessage);
                logMessagesError.Add(TestGlobals.DefaultLogMessage);

                logger.Append(new LogEntry()
                {
                    Level = LogLevel.Fatal,
                    Format = TestGlobals.DefaultLogMessage,
                    Module = module,
                });
                logMessages.Add(TestGlobals.DefaultLogMessage);
                logMessagesError.Add(TestGlobals.DefaultLogMessage);

                //Interaction Logs
                logger.Append(new LogEntry()
                {
                    Level = LogLevel.Extif,
                    Format = TestGlobals.DefaultLogMessage,
                    Module = module,
                });
                logMessages.Add(TestGlobals.DefaultLogMessage);
                logMessagesInteraction.Add(TestGlobals.DefaultLogMessage);

                logger.Append(new LogEntry()
                {
                    Level = LogLevel.Userin,
                    Format = TestGlobals.DefaultLogMessage,
                    Module = module,
                });
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
            TestHelper.CheckLogEntrys(logMessages.ToArray(), writtenTraceLogs, "Loggertest", HeaderName: "PLC", PLCLog: true).Should().BeTrue();
            TestHelper.CheckLogEntrys(logMessagesError.ToArray(), writterErrorLogs, "Loggertest", HeaderName: "PLC", PLCLog: true).Should().BeTrue();
            TestHelper.CheckLogEntrys(logMessagesInteraction.ToArray(), writtenInteractionLogs, "Loggertest", HeaderName: "PLC", PLCLog: true).Should().BeTrue();
            TestHelper.CheckLogEntrys(logMessagesAudit.ToArray(), writtenAuditLogs, "Loggertest", HeaderName: "PLC", PLCLog: true).Should().BeTrue();
            TestHelper.CheckLogEntrys(logMessages.ToArray(), writtenConsoleLogs, "Loggertest", PLCLog: true).Should().BeTrue();
        }

        [Fact]
        //Tests whter the WWI Logger is working correctly
        public void LoggerTestsWwiLogger()
        {
            LogManager.Initialize(TestGlobals.Product, TestGlobals.Component);

            string desc = "WaWi";
            string ip = "192.168.64.1";
            ushort port = 6050;

            Dictionary<string, bool> acutalMessages = new Dictionary<string, bool>();

            using (var wwi = LogManager.GetWwi(desc, ip, port))
            {
                wwi.LogMessage("<xml>an incoming message</xml>");
                acutalMessages.Add("<xml>an incoming message</xml>", true);
                wwi.LogMessage("<xml>an outgoing message</xml>", false);
                acutalMessages.Add("<xml>an outgoing message</xml>", false);
            }

            LogManager.Cleanup();

            string[] writtenLogs = TestHelper.ReadAllLinesFromWawiLog(desc, ip, port);

            TestHelper.CheckWaWiLogEntrys(acutalMessages, writtenLogs).Should().BeTrue(); 
        }

        [Fact]
        //Tests the Wwi Logger with bigger XML strings 
        public void LoggerTestsWwiLogger2()
        {
            //Define the Messages that should be logged 
            string firstOut = @"< XML >< TIOTransSysStatusResponse ID = \""112074\"" TransferPoint = \""192.168.64.97:1\"""
                            + @"SelectedOutput = "" Operational = \""True\"" Ready = \""False\"" BusyForOutput = "" >< OutputDetail Output"
                            + @"= \""1\"" Operational = \""True\"" /></ TIOTransSysStatusResponse ></ XML >";
            string firstIn = @"< XML >< TIOTransSysStatusQuery ID = \""112075\"" TransferPoint = \""192.168.64.97:2\"" /></ XML >";
            string secondOut = @"< XML >< TIOTransSysStatusResponse ID = \""112075\"" TransferPoint = \""192.168.64.97:2\"" SelectedOutput"
                            + @"= "" Operational = \""True\"" Ready = \""False\"" BusyForOutput = "" >< OutputDetail Output = \""2\"" "
                            + @"Operational = \""True\"" /></ TIOTransSysStatusResponse ></ XML >";
            string secondIn = @"< XML >< TIOTransSysStatusQuery ID = \""112076\"" TransferPoint = \""192.168.64.97:3\"" /></ XML >";

            LogManager.Initialize(TestGlobals.Product, TestGlobals.Component);

            string desc = "WaWi";
            string ip = "192.168.10.89";
            ushort port = 1234;

            Dictionary<string, bool> acutalMessages = new Dictionary<string, bool>();

            using (var wwi = LogManager.GetWwi(desc, ip, port))
            {
                wwi.LogMessage(firstOut, false);
                acutalMessages.Add(firstOut, false);
                wwi.LogMessage(firstIn, true);
                acutalMessages.Add(firstIn, true);
                wwi.LogMessage(secondOut, false);
                acutalMessages.Add(secondOut, false);
                wwi.LogMessage(secondIn, true);
                acutalMessages.Add(secondIn, true);
            }

            LogManager.Cleanup();

            string[] writtenLogs = TestHelper.ReadAllLinesFromWawiLog(desc, ip, port);

            TestHelper.CheckWaWiLogEntrys(acutalMessages, writtenLogs).Should().BeTrue(); 
        }

        [Fact]
        //Tests whter the Null Logger is working correctly 
        public void LoggerTestsNullLogger()
        {
            LogManager.InitializeNull();

            ILog logger = LogManager.GetLogger("TestComponent");

            for (int i = 0; i < 50; i++)
            {
                logger.Debug(TestGlobals.DefaultLogMessage);
                logger.Audit(TestGlobals.DefaultLogMessage);
                logger.Error(TestGlobals.DefaultLogMessage);
                logger.UserIn(TestGlobals.DefaultLogMessage);
            }

            LogManager.Cleanup();

            //OK, there should be not files in the Log Directory because the Logger 
            //was initialized with null

            //FileName == null means the Component Folder 
            //Does not event exist -> Correct behavior here
            //Because with the null logger nothing is initialized 
            FileAndDirectoryManager.GetFileNamesForFolder(TestGlobals.DefaultLogFileDirectory).Should().BeNull(); 
        }
    }
}
