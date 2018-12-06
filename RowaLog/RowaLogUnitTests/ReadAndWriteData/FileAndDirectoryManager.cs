using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowaLogUnitTests.ReadAndWriteData
{
    public static class FileAndDirectoryManager
    {
        #region Methods

        /// <summary>
        /// Reads the Text From a File  
        /// </summary>
        /// <param name="Path"></param>
        /// <returns>The Text if the File Exists
        /// Empty String is not</returns>
        public static string ReadTextFromFile(string Path)
        {
            if (FileExists(Path))
            {
                return File.ReadAllText(Path);
            }
            return ""; 
        }

        /// <summary>
        /// Writes the Given Text to the the File at the given Path
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Text"></param>
        public static bool WriteTextToFile(string Path, string Text)
        {
            try
            {
                File.WriteAllText(Path, Text);
                return true; 
            }
            catch 
            {

            }
            return false; 
        }

        /// <summary>
        /// Writes the given bytes to the given Path and returns wheter this was successfull 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool WriteBytes(string path, byte[] content)
        {
            try
            {
                File.WriteAllBytes(path, content);
                return true; 
            }
            catch
            {
                return false; 
            }
        }
        


        /// <summary>
        /// Returns wheter the given File exsits
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool FileExists(string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        /// Checks wheter a File exists and deletes it if possible 
        /// Returns True when no File exists 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool DeleteFile(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch
            {
                return false;
            }
            return true;
        }
    
        /// <summary>
        /// Returns a string array containing the Names in the given Folder 
        /// If the Folder exists. If not, null will be returned
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string[] GetFileNamesForFolder(string path)
        {
            if (Directory.Exists(path))
            {
                try
                {
                    return Directory.GetFiles(path);
                } catch
                {
                    //Catch the Exception, return null
                }
                
            }
            return null; 
        }

        /// <summary>
        /// Deletes a Directory with the given path if it exists
        /// Beware 
        /// </summary>
        /// <param name="path"></param>
        public static bool DeletDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                try
                {
                    Directory.Delete(path, true);
                }catch
                {
                    //Only Return False if the Deleting failed, 
                    //If Directory does not exists everything is fine 
                    return false;
                }
                
            }
            return true; 
        }


        /// <summary>
        /// Returns the Directory the programm is currently working in 
        /// </summary>
        /// <returns></returns>
        public static string GetWorkingDirectory()
        {
            return Directory.GetCurrentDirectory(); 
        }
        #endregion
    }
}
