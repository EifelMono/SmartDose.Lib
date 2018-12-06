using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Rowa.Lib.Log.Types
{
    internal static class LogFileStreamManagerExtensions
    {
        public static long GetFileSize(this LogFileStreamManager filemanager, string filePath)
        {
            if (!File.Exists(filePath)) return 0;

            var stream = filemanager.GetStream(filePath);

            return stream == null ? 0 : stream.Size;
        }
    }
}
