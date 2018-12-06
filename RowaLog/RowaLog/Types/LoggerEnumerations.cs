using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Rowa.Lib.Log
{
    /// <summary>
    /// This Enum Describes all kinds of logger Errors
    /// </summary>
    public enum LoggerError
    {
        Unknown,
        LogQueueFull,
        LogStreamIOError,
        LogConsoleOutputError,
        LogFileAccessDenied,
        LogDirectoryCreationFailed, 
        LogFileNotFound,
        FormatException
    }

    /// <summary>
    /// This Enum decribes all possible EventTypes for LoogerIO Events
    /// </summary>
    public enum LoggerIOEventType
    {
        NewLogFileCreated, 
    }

    /// <summary>
    /// This Enum Describes all Supported Log Levels
    /// </summary>
    public enum LogLevel
    {
        Debug = 50000,
        Extif,
        Userin,
        Info,
        Warn,
        Error,
        Fatal,
        Audit
    }
}
