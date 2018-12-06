using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowaLogUnitTests.Common
{
    public static class ExtensionMethods
    {
        #region ------------- string Array -------------
        /// <summary>
        /// Removes the Last Entry of the given Entry if this one is Empty or null
        /// </summary>
        /// <param name="array"></param>
        public static string[] RemoveLastEntryIfEmpty(this string[] array)
        {
            if (String.IsNullOrEmpty(array[array.Length - 1]))
            {
                string[] buffer = new string[array.Length - 1];
                for (int i = 0; i < array.Length - 1; i++)
                {
                    buffer[i] = array[i];
                }
                return buffer;
            }
            return array; 
        }
        #endregion

    }
}
