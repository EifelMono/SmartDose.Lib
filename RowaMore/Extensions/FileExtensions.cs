using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RowaMore.Extensions
{
    public static class FileExtensions
    {
        public static string EnsureDirectoryExist(this string thisValue)
        {
            if (!Directory.Exists(thisValue))
                Directory.CreateDirectory(thisValue);
            return thisValue;
        }
    }
}
