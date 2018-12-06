using System.IO;
using System.Reflection;

namespace RowaLogUnitTests.Common
{
    public static class TestGlobals
    {
        /// <summary>
        /// The max Size of the LogQueue 
        /// </summary>
        public const int MaxQueueSize = 1000000; 

        /// <summary>
        /// The default Value for the Product that should be used while the Unittest
        /// </summary>
        public const string Product = "RowaLogUnittestProduct";

        /// <summary>
        /// A default Value for the Component that should be used while the Unittest
        /// </summary>
        public const string Component = "RowaLogUnittestComponent";

        /// <summary>
        /// The Default Path where the LogFiles are saved
        /// </summary>
        public const string RowaLogFileDirectory = @"C:\ProgramData\Rowa\Protocol";

        /// <summary>
        /// Returns the Path For the DefaultLogFileDirectory
        /// for the Default Product and Component 
        /// </summary>
        public static string DefaultLogFileDirectory = RowaLogFileDirectory + Path.DirectorySeparatorChar + Product + Path.DirectorySeparatorChar + Component;

        /// <summary>
        /// Returns the Default Extension that is used for the compromised 
        /// files
        /// </summary>
        public const string CompromisedFileExtension = ".log7z"; 

        /// <summary>
        /// Message that is Logged per Default 
        /// </summary>
        public const string DefaultLogMessage = "A L0G message &%$";

        /// <summary>
        /// The Path to the LogFile containing all variables
        /// </summary>
        public const string DefaultLogPath = RowaLogFileDirectory + @"\{product}\{component}\{date}.{product}.{component}.{appender}.{count}.log";

        /// <summary>
        /// The Path to the WaWi LogFiles containing all variables 
        /// </summary>
        public const string DefaultWaWiLogPath = RowaLogFileDirectory + @"\{product}\{component}\{date}.{description}.{sourceIP}_{port}.wwi";
        /// <summary>
        /// Returns the current working Directory of the Application
        /// </summary>
        public static string WorkingDirectory => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); 
    }
}
