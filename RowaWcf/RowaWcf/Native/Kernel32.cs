using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Rowa.Lib.Wcf.Native
{
    /// <summary>
    /// Class which contains all function import definitions of the kernel32.dll.
    /// </summary>
    internal static class Kernel32
    {
        #region Function Imports

        /// <summary>
        /// Retrieves a pseudo handle for the current process.
        /// </summary>
        /// <returns>
        /// The return value is a pseudo handle to the current process.
        /// </returns>
        [DllImport("kernel32.dll", SetLastError = false)]
        [SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        public static extern IntPtr GetCurrentProcess();

        /// <summary>
        /// Frees the specified local memory object and invalidates its handle.
        /// </summary>
        /// <param name="hMem">A handle to the local memory object.</param>
        /// <returns>If the function succeeds, the return value is IntPtr.Zero.</returns>        
        [DllImport("kernel32.dll", SetLastError = true)]
        [SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        public static extern IntPtr LocalFree(IntPtr hMem);

        /// <summary>
        /// Closes an open object handle.
        /// </summary>
        /// <param name="hObject">
        /// A valid handle to an open object.
        /// </param>
        /// <returns>
        /// <c>true</c> if successful; <c>false</c> otherwise.
        /// </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        [SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        public static extern bool CloseHandle(IntPtr hObject);


        #endregion
    }
}
