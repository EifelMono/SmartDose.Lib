using FluentAssertions;
using Rowa.Lib.Log;
using Rowa.Lib.Log.Configuration;
using Rowa.Lib.Log.Types;
using RowaLogUnitTests.Common;
using RowaLogUnitTests.ReadAndWriteData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit; 

namespace RowaLogUnitTests.TestClasses
{
    public class LogCleanupTest
    {
        /// <summary>
        /// Konstruktor that will be called before each Test is executed 
        /// -> Cleanup old Files from the Tests before 
        /// </summary>
        public LogCleanupTest()
        {
            TestHelper.RemoveTestFiles().Should().BeTrue();
        }

        [Fact]
        ///Tests what happens if you try to get a new Logger
        ///After the CleanupManager has been disposed
        public void CleanupTestGetLoggerAfterDispose()
        {
            //Start a new Process, containing the RowaLogUnitTestExampleApp
            //This App Logs some example entries and calles Cleanup afterwards 
            Process prc = TestHelper.GetUnitTestExampleAppProcess(UnitTestExampleModes.ExecuteLoggingAfterDispose, TestGlobals.Product, TestGlobals.Component, TestGlobals.DefaultLogMessage);

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
        public void CleanupDisposeAfterNonInitialize()
        {
            LogManagerCore logManager = new LogManagerCore(TestGlobals.Product, TestGlobals.Component, String.Empty, "2", true, false);

            Action action = (() => { logManager.Dispose(); });

            action.Should().NotThrow<Exception>();
        }


        [Fact]
        //Tests wheter the Cleanup of RowaLog is working poperly and 
        //if all created Threads are disposed when Cleanup is called
        public void CleanupTestDispose()
        {
            //Start a new Process, containing the RowaLogUnitTestExampleApp
            //This App Logs some example entries and calles Cleanup afterwards 
            Process prc = TestHelper.GetUnitTestExampleAppProcess(UnitTestExampleModes.Static, TestGlobals.Product, TestGlobals.Component, TestGlobals.DefaultLogMessage);

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

        //Tests wheter old Logs will get ziped 
        [Fact]
        public void CleanupTestFileCompresssion()
        { 
            //Inject custom LogConstants in order to reduce the waitingtime 
            //for the cleanup Thread to 0 Seconds -> Thread will run in Loop 
            ConstantsManipulator manipulator = new ConstantsManipulator();
            manipulator.InjectValues(100);

            //Init Log Manager and write the Logs 
            LogManager.Initialize(TestGlobals.Product, TestGlobals.Component);

            ILog logger = LogManager.GetLogger("TestComponent");

            logger.Error(TestGlobals.DefaultLogMessage);
            logger.Audit(TestGlobals.DefaultLogMessage);
            logger.UserIn(TestGlobals.DefaultLogMessage);
            logger.Debug(TestGlobals.DefaultLogMessage);

            //Cleanup in order to release the file Handler and ensure everything was logged
            LogManager.Cleanup();

            DateTime today = DateTime.Today;

            //Get Log Files
            //Change the Times in the File so they can get zipped 
            string[] paths = new string[]
            {
                TestHelper.GetLogFilePath(TestGlobals.Product, TestGlobals.Component, DateTime.Today, AppenderType.Trace, 0),
                TestHelper.GetLogFilePath(TestGlobals.Product, TestGlobals.Component, DateTime.Today, AppenderType.Audit, 0),
                TestHelper.GetLogFilePath(TestGlobals.Product, TestGlobals.Component, DateTime.Today, AppenderType.Error, 0),
                TestHelper.GetLogFilePath(TestGlobals.Product, TestGlobals.Component, DateTime.Today, AppenderType.Interaction, 0)
            };

            TestHelper.ChangeFileWriteAndCreationDates(paths, DateTime.Today.AddDays((Globals.Constants.KeepLogFilesInDays - 2) * -1));

            //Init the LogManager again -> Start Cleanup Thread again
            LogManager.Initialize(TestGlobals.Product, TestGlobals.Component);

            //Add a FileSystem Watcher to Wait for a specific Event
            //Wait Four Times because Four Files will be ziped
            //-> The ols File will later be deleted, therefore we wait for the files to be deleted
            //Then we can be sure everything was finished 
            TestHelper.WaitForFileDeletedInFolder(TestGlobals.DefaultLogFileDirectory, 4000);
            TestHelper.WaitForFileDeletedInFolder(TestGlobals.DefaultLogFileDirectory, 4000);
            TestHelper.WaitForFileDeletedInFolder(TestGlobals.DefaultLogFileDirectory, 4000);
            TestHelper.WaitForFileDeletedInFolder(TestGlobals.DefaultLogFileDirectory, 4000);

            LogManager.Cleanup();

            //Reset the Constants
            manipulator.Reset();

            //Check for Result, there should be 4 Files in the Folder ending with .log7z
            string[] files = FileAndDirectoryManager.GetFileNamesForFolder(TestGlobals.DefaultLogFileDirectory);

            files.Should().NotBeNull();

            bool result = true;

            foreach (string name in files)
            {
                if (!Path.GetFileName(name).EndsWith(TestGlobals.CompromisedFileExtension))
                {
                    result = false;
                }
            }
            result.Should().BeTrue();
        }

        [Fact]
        //Tests if the ols logs will be deleted after
        //the configurated time 
        public void CleanupTestKeepFiles()
        {
            //Init Log Manager and write the Logs 
            LogManager.Initialize(TestGlobals.Product, TestGlobals.Component);

            ILog logger = LogManager.GetLogger("TestComponent");

            logger.Error(TestGlobals.DefaultLogMessage);
            logger.Audit(TestGlobals.DefaultLogMessage);
            logger.UserIn(TestGlobals.DefaultLogMessage);
            logger.Debug(TestGlobals.DefaultLogMessage);

            //Cleanup in order to release the file Handler and ensure everything was logged
            LogManager.Cleanup();

            //Change the Write and Creation Time of all Created File to 2 Days Less than 
            //the current Keep Time 

            //Get Log Files
            string[] paths = new string[]
            {
                TestHelper.GetLogFilePath(TestGlobals.Product, TestGlobals.Component, DateTime.Today, AppenderType.Trace, 0),
                TestHelper.GetLogFilePath(TestGlobals.Product, TestGlobals.Component, DateTime.Today, AppenderType.Audit, 0),
                TestHelper.GetLogFilePath(TestGlobals.Product, TestGlobals.Component, DateTime.Today, AppenderType.Error, 0),
                TestHelper.GetLogFilePath(TestGlobals.Product, TestGlobals.Component, DateTime.Today, AppenderType.Interaction, 0)
            }; 

            TestHelper.ChangeFileWriteAndCreationDates(paths, DateTime.Today.AddDays((Globals.Constants.KeepLogFilesInDays + 2) * -1)); 

            //Inject custom LogConstance in order to reduce the waitingtime 
            //for the cleanup Thread to 100 Milliseconds
            ConstantsManipulator manipulator = new ConstantsManipulator();
            manipulator.InjectValues(100);

            //Init the LogManager again -> Start Cleanup Thread again
            LogManager.Initialize(TestGlobals.Product, TestGlobals.Component);

            //Sleep minimum 4 Seconds to give the CleanupThread the Time to hit
            //Wait 4 Times for Files to be deleted -> We have created four Log Files before 
            TestHelper.WaitForFileDeletedInFolder(TestGlobals.DefaultLogFileDirectory, 4000);
            TestHelper.WaitForFileDeletedInFolder(TestGlobals.DefaultLogFileDirectory, 4000);
            TestHelper.WaitForFileDeletedInFolder(TestGlobals.DefaultLogFileDirectory, 4000);
            TestHelper.WaitForFileDeletedInFolder(TestGlobals.DefaultLogFileDirectory, 4000);

            LogManager.Cleanup();

            //Important, reset the Log Constants!
            manipulator.Reset();

            string[] files = FileAndDirectoryManager.GetFileNamesForFolder(TestGlobals.DefaultLogFileDirectory);

            //files object should exist but have a count of 0
            files.Should().NotBeNull();
            files.Count().Should().Be(0);
        }

        [Fact]
        //Tests wheter RowaLog failes if Dispose is called
        //More than one time from multiple Threads 
        public void CleanupTestMultipleDispose()
        {
            ManualResetEvent disposeEvent = new ManualResetEvent(false);

            LogManager.Initialize(TestGlobals.Product, TestGlobals.Component);

            ILog logger = LogManager.GetLogger("TestComponent");

            //Write 50 logs
            for (int i = 0; i < 50; i++)
            {
                logger.Debug(TestGlobals.DefaultLogMessage);
            }

            List<Task> tasks = new List<Task>();

            for (int i = 0; i < 4; i++)
            {
                //Tasks waits for Dispose Event and
                //calls Cleanup when set 
                tasks.Add(new Task(() =>
                {
                    disposeEvent.WaitOne();

                    LogManager.Cleanup();
                }));
            }

            foreach (Task task in tasks)
            {
                task.Start();
            }

            disposeEvent.Set();

            //If no Exception was hit or the Timeout is over, 
            //everything is fine 
            bool res = true;
            if (!Task.WaitAll(tasks.ToArray(), 3000))
            {
                res = false;
                foreach (Task task in tasks)
                {
                    task.Dispose();
                }
            }
            res.Should().BeTrue(); 
        }

        [Fact]
        //Tests wheter the Log Files are split after 
        //A specific file size 
        public void CleanupTestSplitFilesAndZip()
        {
            //Inject custom LogConstants in order to reduce the 
            //File size after which the LogManger will open new LogFile
            //Too 100 Bytes -> 100 Ascii Chars
            //Out Log Message will be e.g. 10:48:51,689;DEBUG;TestComponent;7;0;0;A L0G message &%$
            //Which has a Size of 56 Bytes, This is written -> then the files will be split
            ConstantsManipulator manipulator = new ConstantsManipulator();
            manipulator.InjectValues(0, 50);

            //Init Log Manager and write the Logs 
            LogManager.Initialize(TestGlobals.Product, TestGlobals.Component);

            ILog logger = LogManager.GetLogger("TestComponent");

            for (int i = 0; i < 2; i++)
            {
                logger.Debug(TestGlobals.DefaultLogMessage);
            }
            Thread.Sleep(1000);

            //Cleanup in order to release the file Handler and ensure everything was logged
            LogManager.Cleanup();

            manipulator.Reset();

            //Read in the Lines of the two files
            string pathFile1 = TestHelper.GetLogFilePath(TestGlobals.Product, TestGlobals.Component, DateTime.Today, AppenderType.Trace, 0);
            string pathFile2 = TestHelper.GetLogFilePath(TestGlobals.Product, TestGlobals.Component, DateTime.Today, AppenderType.Trace, 1);

            //File One should be ziped allready 
            pathFile1 += Globals.Constants.CompressionExtension;

            string[] writtenLogsFile2 = TestHelper.ReadAllLinesFromFile(pathFile2);

            //Both should contain one Log Line with the given Message 
            bool resultFile1 = File.Exists(pathFile1);
            bool resultFile2 = TestHelper.CheckLogEntrys(new string[] { TestGlobals.DefaultLogMessage }, writtenLogsFile2, "TestComponent");

            //Only of both have worked the Test will be successfull
            resultFile1.Should().BeTrue();
            resultFile2.Should().BeTrue(); 
        }

    }
}