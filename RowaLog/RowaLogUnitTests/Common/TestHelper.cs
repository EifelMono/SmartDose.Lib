using Rowa.Lib.Log.Configuration;
using RowaLogUnitTests.ReadAndWriteData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace RowaLogUnitTests.Common
{
    public static class TestHelper
    {
        #region ------------- Methods -------------
        /// <summary>
        /// Remove all Files contained in the given product and component
        /// </summary>
        public static bool RemoveTestFiles(string product, string component)
        {
            string pathToDir = TestGlobals.RowaLogFileDirectory + Path.DirectorySeparatorChar + product + Path.DirectorySeparatorChar + component; 
            if (!FileAndDirectoryManager.DeletDirectory(pathToDir))
            {
                return false; 
            }
            return true; 
        }

        /// <summary>
        /// Method call is blocked till there are changes in the 
        /// given Folder or the Timeout was hit
        /// </summary>
        /// <param name="path"></param>
        /// <param name="Timeout"></param>
        /// <returns>True when there were changes, false if the Timeout hit</returns>
        public static bool WaitForFileDeletedInFolder(string path, int Timeout)
        {
            //Add a FileSystem Watcher to Wait for a specific Event
            ManualResetEvent waiter = new ManualResetEvent(false);
            FileSystemWatcher watcher = new FileSystemWatcher(path)
            {
                EnableRaisingEvents = true
            };
            watcher.Deleted += SystemChanged;

            void SystemChanged(object sender, FileSystemEventArgs e)
            {
                waiter.Set();
            }

            //Wait a Maximum of 4 Seconds for the Changes to Take affect 
            return waiter.WaitOne(Timeout);
        }

        /// <summary>
        /// Removes multiple testfiles at once
        /// The Dictionary holds combinations of 
        /// product and components that should be cleaned
        /// </summary>
        /// <param name="files"></param>
        /// <returns>true if all Files can be cleaned, false if not</returns>
        public static bool RemoveTestFiles(List<ProductComponentWrapper> destinations)
        {
            bool res = true;
            foreach (ProductComponentWrapper entry in destinations)
            {
                if (!RemoveTestFiles(entry.Product, entry.Component))
                {
                    res = false;
                }
            }
            return res;

        }
        /// <summary>
        /// Checks wheter the given LogMessages are represented in the File and are in the correct Order
        /// </summary>
        /// <param name="shouldLogMessages"></param>
        /// <param name="actualEntrieLines"></param>
        /// <param name="component"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        public static bool CheckLogEntrys(string[] shouldLogMessages, string[] actualEntryLines, string loggername, bool includeTaskID = false, string HeaderName = "", bool PLCLog = false)
        {
            bool res = true;
            if (!string.IsNullOrEmpty(HeaderName))
            {
                //Check the Header seperatly
                if (!CheckHeader(HeaderName, actualEntryLines[0]))
                {
                    res = false;
                }
                //Remove the HeaderLine from the actual Entries
                string[] copy = new string[actualEntryLines.Length - 1]; 
                for(int i = 1; i <actualEntryLines.Count(); i++)
                {
                    copy[i - 1] = actualEntryLines[i]; 
                }
                actualEntryLines = copy; 
            }
            if (shouldLogMessages.Length == actualEntryLines.Length)
            {
                for (int i = 0; i< actualEntryLines.Count(); i++)
                {
                    if (!CheckEntry(actualEntryLines[i], shouldLogMessages[i], loggername, includeTaskID, PLCLog))
                    {
                        res = false; 
                    }
                }
            } else
            {
                res = false; 
            }

            return res;
        }

        /// <summary>
        /// Checks the given WaWiEntrys with the given should Messages
        /// The Dirctionary describes the string messages and wheter
        /// They had been incoming (true) or outgoing (false) 
        /// </summary>
        /// <param name="shouldLogMessageWithIncoming"></param>
        /// <param name="actualEntryLines"></param>
        /// <returns></returns>
        public static bool CheckWaWiLogEntrys(Dictionary<string, bool> shouldLogMessageWithIncoming, string[] actualEntryLines)
        {
            bool res = true;
            if (shouldLogMessageWithIncoming.Count() == actualEntryLines.Length)
            {
                for (int i = 0; i < actualEntryLines.Count(); i++)
                {
                    KeyValuePair<string, bool> entry = shouldLogMessageWithIncoming.ElementAt(i); 
                    if (!CheckWaWiEntry(actualEntryLines[i], entry.Key, entry.Value))
                    {
                        res = false;
                    }
                }
            }
            else
            {
                res = false;
            }

            return res;
        }

        /// <summary>
        /// Changes the Create and Write time of the given files to the given Date
        /// </summary>
        /// <param name="files"></param>
        /// <param name="newDate"></param>
        /// <returns>Wheter the changes have been successfull or not</returns>
        public static bool ChangeFileWriteAndCreationDates(string[] files, DateTime newDate)
        {
            bool success = true; 
            foreach(string file in files)
            {
                FileInfo info = new FileInfo(file); 

                if (info.Exists)
                {
                    try
                    {
                        info.CreationTime = newDate;
                        info.LastWriteTime = newDate;
                    }
                    catch
                    {
                        success = false; 
                    }
                }
            }
            return success; 
        }

        /// <summary>
        /// Returns a new Process that is used to Start the 
        /// RowaLogUnitTestsExampleApp with the given arguments
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static Process GetUnitTestExampleAppProcess(UnitTestExampleModes mode, string product,
                                                           string component, string message, int concurrency = 1)
        {
            return new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "RowaLogUnitTestsExampleApp.exe",
                    Arguments = mode.ToString() + " " + product + " " + component + " " + message.Replace(" ", "") + " " + concurrency.ToString(),
                    UseShellExecute = false,
                    RedirectStandardOutput = false,
                    RedirectStandardInput = false,
                    RedirectStandardError = false,
                    CreateNoWindow = true,
                    WorkingDirectory = TestGlobals.WorkingDirectory,
                }
            };
        }

        /// <summary>
        /// Checks wheter the given HeaderName and HeaderEntry 
        /// are fitting the right Format 
        /// </summary>
        /// <param name="HeaderName"></param>
        /// <param name="actualEntry"></param>
        /// <returns></returns>
        public static bool CheckHeader(string headerName, string actualEntry)
        {
            string pattern = @"^\#RowaLog.\b"+ headerName + @"\b\;\d+\.\d+$";

            Regex regex = new Regex(pattern);

            return regex.IsMatch(actualEntry); 
        }

        /// <summary>
        /// Checks if the given LogEntry fits the given Information 
        /// </summary>
        /// <param name="LogEntry"></param>
        /// <param name="message"></param>
        /// <param name="logger"></param>
        /// <param name="component"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        public static bool CheckEntry(string LogEntry, string message, string logger, bool includeTaskID = false, bool plcLog = false)
        {
            //TODO: Replacing w+ with the LoggerType that was used 
            string pattern = @"\d\d\:\d\d\:\d\d\,\d\d\d\;\w+\;\b" + logger + @"\b\;";
            if (plcLog)
            {
                pattern += @".+?(?=;)\;"; 
            }
            pattern += @"\d+\;";
            if (!includeTaskID)
            {
                //Add the Two last digets to the pattern if the TaskID is auto 
                //generated 
                pattern += @"\d+\;\d+\;"; 
            }
            return CheckEntry(pattern, LogEntry, message); 
        }

        /// <summary>
        /// Checks if the given LogEntry contains the given message
        /// and has the right Format 
        /// </summary>
        /// <param name="LogEntry"></param>
        /// <param name="message"></param>
        /// <param name="incoming"></param>
        /// <returns></returns>
        public static bool CheckWaWiEntry(string LogEntry, string message, bool incoming)
        {
            //incoming messages are type R in log, outgoing type S
            string type = incoming == true ? "R" : "S"; 
            string pattern = @"\d\d\:\d\d\:\d\d\,\d\d\d\ " + type + @"\:";

            return CheckEntry(pattern, LogEntry, message); 
        }

        /// <summary>
        /// Checks if the given Message is included in the given Entry
        /// and if the beginning of the Entry fitts the given regex pattern 
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="LogEntry"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool CheckEntry(string pattern, string LogEntry, string message)
        {
            bool res = false; 
            Regex rex = new Regex(pattern);
            
            //First test if the preLog was formated correctly
            if (rex.IsMatch(LogEntry))
            {
                //Then check the Log Message 
                System.Text.RegularExpressions.Match match = rex.Match(LogEntry);
                string prestring = match.Value;
                if (LogEntry.Remove(0, prestring.Length).Equals(message))
                {
                    res = true;
                }
            }
            return res;
        }

        /// <summary>
        /// Removes all Files contained in the Default Component and Product 
        /// </summary>
        /// <returns></returns>
        public static bool RemoveTestFiles()
        {
            return RemoveTestFiles(TestGlobals.Product, TestGlobals.Component); 
        }

        /// <summary>
        /// Reads all Informations from a Log with Default Values an the given AppenderType
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string[] ReadAllLinesFromLog(AppenderType type)
        {
            return ReadAllLinesFromLog(TestGlobals.Product, TestGlobals.Component, DateTime.Today, type); 
        }
        
        /// <summary>
        /// Reads all Lines from the WaWiLog with the given Infos
        /// using the TestGlobals Component, Product
        /// And the Date of Today 
        /// </summary>
        /// <param name="description"></param>
        /// <param name="sourceIP"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static string[] ReadAllLinesFromWawiLog(string description, string sourceIP, ushort port)
        {
            return ReadAllLinesFromWaWiLog(TestGlobals.Product, TestGlobals.Component, DateTime.Today,
                                           description, sourceIP, port); 
        }
        /// <summary>
        /// Trys to Read all Lines from the Log File fitting the given Informations
        /// </summary>
        /// <param name="product"></param>
        /// <param name="component"></param>
        /// <returns></returns>
        public static string[] ReadAllLinesFromLog(string product, string component, DateTime date, AppenderType type) 
        {
            string path = GetLogFilePath(product, component, date, type);
            return ReadAllLinesFromFile(path); 
        }

        /// <summary>
        /// Reads all Lines from the WaWiLog fitting the given Information
        /// </summary>
        /// <param name="product"></param>
        /// <param name="component"></param>
        /// <param name="date"></param>
        /// <param name="description"></param>
        /// <param name="sourceIP"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static string[] ReadAllLinesFromWaWiLog(string product, string component, DateTime date, string description, string sourceIP, ushort port)
        {
            string path = GetWaWiLogFilePath(product, component, date, description, sourceIP, port);
            return ReadAllLinesFromFile(path);
        }

        /// <summary>
        /// Reads all Lines from the File at the given path 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string[] ReadAllLinesFromFile(string path)
        {
            string[] stringLines = new string[0];
            if (FileAndDirectoryManager.FileExists(path))
            {
                stringLines = File.ReadAllLines(path);
            }
            return stringLines.ToArray();
        }

        /// <summary>
        /// Gets the Path for a Specfici Log file with the given Information 
        /// </summary>
        /// <param name="product"></param>
        /// <param name="component"></param>
        /// <param name="date"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetLogFilePath(string product, string component, DateTime date, AppenderType type, int count = 0)
        {
            string result = TestGlobals.DefaultLogPath;
            result = result.Replace("{product}", product);
            result = result.Replace("{component}", component);
            result = result.Replace("{date}", date.ToString("yyyyMMdd"));
            result = result.Replace("{appender}", type.ToString());
            result = result.Replace("{count}", count.ToString("D2")); 
            return result; 
        }

        /// <summary>
        /// Returns the Path to a WaWi File that is
        /// described by the given Parameters 
        /// </summary>
        /// <param name="product"></param>
        /// <param name="component"></param>
        /// <param name="date"></param>
        /// <param name="sourceIP"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static string GetWaWiLogFilePath(string product, string component, DateTime date, string description, string sourceIP, ushort port)
        {
            string result = TestGlobals.DefaultWaWiLogPath;
            result = result.Replace("{product}", product);
            result = result.Replace("{component}", component);
            result = result.Replace("{date}", date.ToString("yyyyMMdd"));
            result = result.Replace("{description}", description); 
            result = result.Replace("{sourceIP}", sourceIP);
            result = result.Replace("{port}", port.ToString());
            return result; 
        }

        #endregion
    }
}
