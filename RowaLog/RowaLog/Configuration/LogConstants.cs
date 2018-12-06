using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rowa.Lib.Log.Configuration
{
    internal class LogConstants
    {
        #region ------------- protected Fields -------------
        /// <summary>
        /// Directory and Filename of the Logfile as Template string
        /// </summary>
        protected string _filePath;

        /// <summary>
        /// The String Prefix Value that should be used for Headers in LogFiles
        /// </summary>
        protected string _headerFormat;

        /// <summary>
        /// Base Directory of the Log Files to be stored in 
        /// </summary>
        protected string _logBaseDir; 

        /// <summary>
        /// The Interval the LogCleanupThread is executed
        /// </summary>
        protected int _cleanupThreadIntervalMilliseconds;

        /// <summary>
        /// Defines how long a WWi log File is kept (in Days)
        /// </summary>
        protected uint _keepWwiFilesInDays;

        /// <summary>
        /// Defines how long a log File is kept (in Days)
        /// </summary>
        protected uint _keepLogFilesInDays;

        /// <summary>
        /// Defines how long a Audit log File is kept (in Days)
        /// </summary>
        protected uint _keepAuditFilesInDays;

        /// <summary>
        /// Maximum Size of a Log File in Bytes
        /// </summary>
        protected int _maxLogFileSizeInBytes;

        /// <summary>
        /// Defines if Old WWi Files should be compressed
        /// </summary>
        protected bool _compressOldWwiFiles;

        /// <summary>
        /// String Encoding of the Log entry
        /// </summary>
        protected bool _compressOldLogFiles;

        /// <summary>
        /// String Encoding of the Log entry
        /// </summary>
        protected Encoding _logEncoding;

        /// <summary>
        /// Maximum number of retries when working synchronous
        /// </summary>
        protected int _maxSynchronousRetries;

        /// <summary>
        /// Ignorefilter for Logfile Cleanup
        /// </summary>
        protected List<string> _cleanupIgnoreFilter;

        /// <summary>
        /// Provides the Value an LogFile has when it is compressed 
        /// </summary>
        protected string _compressionExtension;

        /// <summary>
        /// The Name of the EventViewer Source the RowaLog Logs will 
        /// be written in
        /// </summary>
        protected string _eventViewerSourceName;

        /// <summary>
        /// The Name of the Cleanup Log in the Event Viewer that should be 
        /// used 
        /// </summary>
        protected string _eventViewerCleanupName;

        /// <summary>
        /// The Logger name that should be used for Cleanup FATAL Error Messages
        /// </summary>
        protected string _cleanupFatalErrorLogName;

        /// <summary>
        /// Maximum Amout of Time in Milliseconds, a LogFile should be kept open in 
        /// Stream 
        /// </summary>
        protected int _maxFileOpenTimeMs; 
        #endregion

        #region ------------- Konstruktor -------------
        public LogConstants()
        {
            //Set the Values 
            //Case the Operating System is win xp, use a hard coded path
            if (Environment.OSVersion.Version.Major < 6)
            {
                _logBaseDir = @"C:\ProgramData\"; 
            } else
            {
                _logBaseDir = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData); 
            }
            _logBaseDir += @"\Rowa\Protocol";
            _filePath = _logBaseDir + @"\{product}\{component}\{date[yyyyMMdd]}.{product}.{component}.{appender}.{count[D2]}.log{compression}"; 
            _compressionExtension += "7z"; 
            _headerFormat = "#RowaLog.{headerType};{headerVersion}{newLine}";
            _cleanupThreadIntervalMilliseconds = 300000;
            _keepWwiFilesInDays = 28;
            _keepLogFilesInDays = 28;
            _keepAuditFilesInDays = 28;
            _maxLogFileSizeInBytes = 103809024;
            _compressOldLogFiles = true;
            _compressOldWwiFiles = true;
            _logEncoding = Encoding.UTF8; 
            _cleanupIgnoreFilter = new List<string> { "*.params.log", "*.scan.Log", "*.stock.log", "*.fridges.log" };
            _eventViewerSourceName = "RLog/Main";
            _eventViewerCleanupName = "RLog/Cleanup";
            _cleanupFatalErrorLogName = "RowaLogCleanupFatalError";
            _maxFileOpenTimeMs = 500;
            _maxSynchronousRetries = 3;
        }
        #endregion

        #region ------------- Properties -------------
        /// <summary>
        /// The Logger Name that should be used to write LogCleanup Fatal Error 
        /// </summary>
        internal string CleanupFatalErrorLogName => _cleanupFatalErrorLogName; 

        /// <summary>
        /// Name for the Cleanup Log that should be set in the EventViewer
        /// </summary>
        internal string EventViewerCleanupName => _eventViewerCleanupName; 

        /// <summary>
        /// The Name of the Event Viewer Source that 
        /// should be used 
        /// </summary>
        internal string EventViewerSourceName => _eventViewerSourceName; 

        /// <summary>
        /// The Extension of a Logfile when it is compressed 
        /// </summary>
        internal string CompressionExtension => _compressionExtension; 

        /// <summary>
        /// Directory and Filename of the Logfile as Template string
        /// </summary>
        internal string FilePath => _filePath;

        /// <summary>
        /// The base Directory where the LogFiles will be stored
        /// </summary>
        internal string LogBaseDirectory => _logBaseDir; 

        /// <summary>
        /// The String Prefix Value that should be used for Headers in LogFiles
        /// </summary>
        internal string HeaderFormat => _headerFormat; 

        /// <summary>
        /// The Interval the LogCleanupThread is executed
        /// </summary>
        internal int CleanupThreadIntervalMilliseconds => _cleanupThreadIntervalMilliseconds;

        /// <summary>
        /// The Maximum Amout of Time a LogFile Should be kept open in 
        /// milliseconds 
        /// </summary>
        internal int MaxFileOpenTimeMs => _maxFileOpenTimeMs;

        /// <summary>
        /// Maximum number of retries when working synchronous
        /// </summary>
        internal int MaxSynchronousRetries => _maxSynchronousRetries;

        /// <summary>
        /// Defines how long a WWi log File is kept (in Days)
        /// </summary>
        internal uint KeepWwiFilesInDays => _keepWwiFilesInDays;

        /// <summary>
        /// Defines how long a log File is kept (in Days)
        /// </summary>
        internal uint KeepLogFilesInDays => _keepLogFilesInDays;

        /// <summary>
        /// Defines how long a Audit log File is kept (in Days)
        /// </summary>
        internal uint KeepAuditFilesInDays => _keepAuditFilesInDays;

        /// <summary>
        /// Maximum Size of a Log File in Bytes
        /// </summary>
        internal long MaxLogFileSizeInBytes => _maxLogFileSizeInBytes;

        /// <summary>
        /// Defines if Old WWi Files should be compressed
        /// </summary>
        internal bool CompressOldWwiFiles => _compressOldLogFiles;

        /// <summary>
        /// Defines if old Log Files should be compressed
        /// </summary>
        internal bool CompressOldLogFiles => _compressOldWwiFiles;

        /// <summary>
        /// String Encoding of the Log entry
        /// </summary>
        internal Encoding LogEncoding => _logEncoding;

        /// <summary>
        /// Ignorefilter for Logfile Cleanup
        /// </summary>
        internal List<string> CleanupIgnoreFilter => _cleanupIgnoreFilter; 
        #endregion
    }
}
