using FluentAssertions;
using Rowa.Lib.Log;
using RowaLogUnitTests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RowaLogUnitTests.TestClasses
{
    public class LogMessageTest
    {
        /// <summary>
        /// Konstruktor that will be called before each Test is executed 
        /// -> Cleanup old Files from the Tests before 
        /// </summary>
        public LogMessageTest()
        {
            TestHelper.RemoveTestFiles().Should().BeTrue();
        }

        [Fact]
        //Test the Logging of Json Strings 
        public void LogMessageTestJson()
        {
            const string jsonString = "{\"employees\":[" +
                                      "{ \"firstName\":\"John\", \"lastName\":\"Doe\"}," +
                                      "{ \"firstName\":\"Anna\", \"lastName\":\"Smith\"}," +
                                      "{ \"firstName\":\"Peter\", \"lastName\":\"Jones\"}]}";

            LogManager.Initialize(TestGlobals.Product, TestGlobals.Component);

            ILog logger = LogManager.GetLogger("TestComponent");

            logger.Debug(jsonString);

            LogManager.Cleanup();

            string[] writtenLogs = TestHelper.ReadAllLinesFromLog(AppenderType.Trace);

            TestHelper.CheckLogEntrys(new string[] { jsonString }, writtenLogs, "TestComponent").Should().BeTrue(); 
        }

        [Fact]
        public void FormatExceptionTest()
        {
            LogManager.Initialize(TestGlobals.Product, TestGlobals.Component);

            ILog logger = LogManager.GetLogger("TestComponent");

            logger.Debug(TestGlobals.DefaultLogMessage, new List<int>().ToArray());

            LogManager.Cleanup();

            string[] writtenLogs = TestHelper.ReadAllLinesFromLog(AppenderType.Trace);

            TestHelper.CheckLogEntrys(new string[] { TestGlobals.DefaultLogMessage }, writtenLogs, "TestComponent").Should().BeTrue();
        }

        [Fact]
        //Test the Logging of an XMl String
        public void LogMessageTestXML()
        {
            const string xmlString = "<verzeichnis>< titel > XML TEST </ titel >"
                                      + "< eintrag >< stichwort > $Genf% </ stichwort >< eintragstext >"
                                      + "Genf ist der Sitz von #+</ eintragstext ></ eintrag ></verzeichnis>";

            LogManager.Initialize(TestGlobals.Product, TestGlobals.Component);

            ILog logger = LogManager.GetLogger("TestComponent");

            logger.Audit(xmlString);

            LogManager.Cleanup();

            string[] writtenLogs = TestHelper.ReadAllLinesFromLog(AppenderType.Audit);

            TestHelper.CheckLogEntrys(new string[] { xmlString }, writtenLogs, "TestComponent").Should().BeTrue(); 
        }

        [Fact]
        //Tests wheter Exceptions are logged correctly 
        public void LogMessageTestInnerException()
        {
            LogManager.Initialize(TestGlobals.Product, TestGlobals.Component);

            ILog logger = LogManager.GetLogger("TestComponent");

            Exception exc;

            #region Exception Log 
            try
            {

                throw new AbandonedMutexException("I am an inner Exception");
            }
            catch (Exception ex1)
            {
                try
                {
                    throw new AggregateException("I am another inner Exception", ex1);
                }
                catch (Exception ex2)
                {
                    try
                    {
                        throw new Exception("I am an Exception", ex2);
                    }
                    catch (Exception ex3)
                    {
                        logger.Error(ex3, "MyError with ex");
                        exc = ex3;
                    }
                }

            }
            #endregion

            LogManager.Cleanup();

            string[] writtenLogs = TestHelper.ReadAllLinesFromLog(AppenderType.Error);

            //Add everything from the written Logs together 
            //Same structure as from the Exeption directly
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < writtenLogs.Count(); i++)
            {
                builder.Append(writtenLogs[i]);
                //There should be no Newline on the last entry
                if (i != writtenLogs.Count() - 1)
                {
                    builder.Append(Environment.NewLine);

                }
            }

            string shouldBe = "MyError with ex" + Environment.NewLine + exc.ToString();

            TestHelper.CheckLogEntrys(new string[] { shouldBe }, new string[] { builder.ToString() }, "TestComponent").Should().BeTrue();
        }

        [Fact]
        //Checks if all common special characters can be logged
        //correctly
        public void LogMessageTestSpecialChars()
        {
            LogManager.Initialize(TestGlobals.Product, TestGlobals.Component);

            ILog logger = LogManager.GetLogger("TestComponent");

            string specialChars = @"!\""#$%&'()*+,-./:;<=>?@[\]^_`{|}~";

            logger.UserIn(specialChars);

            LogManager.Cleanup();

            string[] writtenLogs = TestHelper.ReadAllLinesFromLog(AppenderType.Interaction);

            TestHelper.CheckLogEntrys(new string[] { specialChars }, writtenLogs, "TestComponent").Should().BeTrue();
        }

        [Fact]
        //Tests wheter specifying a Task ID works
        public void LogMessageTestWithTaskID()
        {
            LogManager.Initialize(TestGlobals.Product, TestGlobals.Component);

            int counter = 5;
            List<string> logMessages = new List<string>();
            string logString = "123;12;Audit Log 5";

            for (int i = 0; i < 6; i++)
            {
                logMessages.Add(logString);
            }

            ILog logger = LogManager.GetLogger("TestComponent");

            logger.Audit(123, 12, "Audit Log {0}", counter);
            logger.Warning(123, 12, "Audit Log {0}", counter);
            logger.Info(123, 12, "Audit Log {0}", counter);
            logger.UserIn(123, 12, "Audit Log {0}", counter);
            logger.Error(123, 12, "Audit Log {0}", counter);
            logger.Fatal(123, 12, "Audit Log {0}", counter);

            LogManager.Cleanup();

            string[] writtenLogs = TestHelper.ReadAllLinesFromLog(AppenderType.Trace);

            TestHelper.CheckLogEntrys(logMessages.ToArray(), writtenLogs, "TestComponent", true).Should().BeTrue();
        }

        [Fact]
        //Tests wheter logging an Empty string is possible 
        public void LogMessageTestEmptyString()
        {
            LogManager.Initialize(TestGlobals.Product, TestGlobals.Component);

            List<string> logMessages = new List<string>();
            string logString = String.Empty;

            for (int i = 0; i < 6; i++)
            {
                logMessages.Add(logString);
            }

            ILog logger = LogManager.GetLogger("TestComponent");

            logger.Audit(logString);
            logger.Warning(logString);
            logger.Info(logString);
            logger.UserIn(logString);
            logger.Error(logString);
            logger.Fatal(logString);

            LogManager.Cleanup();

            string[] writtenLogs = TestHelper.ReadAllLinesFromLog(AppenderType.Trace);

            TestHelper.CheckLogEntrys(logMessages.ToArray(), writtenLogs, "TestComponent").Should().BeTrue();
        }
    }
}
