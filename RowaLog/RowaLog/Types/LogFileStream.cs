using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using Rowa.Lib.Log.Events;
using Rowa.Lib.Log.Observer;

namespace Rowa.Lib.Log.Types
{
    /// <summary>
    /// Represents a LogFilestream as promise
    /// </summary>
    internal class LogFileStream
    {
        /// <summary>
        /// Filestream used for logging -- IMPORTANT: the container is not owned by the class. Do not dispose!
        /// </summary>
        private readonly LogFileStreamContainer _container;

        /// <summary>
        /// Returns the Current Filesize
        /// </summary>
        public long Size => _container.Size;

        
        /// <summary>
        /// Class Constructor
        /// </summary>
        /// <param name="logEncoding">Ecoding of the Filestream</param>
        /// <param name="maxFileOpenTimeMs">Maximum Open Time of the Log</param>
        public LogFileStream(LogFileStreamContainer container)
        {
            _container = container;
        }


        /// <summary>
        /// Writes a Log Entry to the Given File
        /// </summary>
        /// <param name="value">Entry to be written to the file</param>
        /// <returns>true if successful</returns>
        public bool Write(string value)
        {
            return _container.Write(value);
        }

        /// <summary>
        /// Writes a Log Entry to the Given File
        /// </summary>
        /// <param name="value">Entry to be written to the file</param>
        /// <param name="offset">offset in value to write from</param>
        /// <param name="count">number of bytes to write</param>
        /// <returns></returns>
        public bool Write(byte[] value, int offset, int count)
        {
            return _container.Write(value, offset, count);
        }

    }
}
