
using System;

namespace Rowa.Lib.Log
{
    /// <summary>
    /// Interface which defines the methods of a Rowa compliant WWI file logger.
    /// </summary>
    public interface IWwi : IDisposable
    {
        /// <summary>
        /// Logs the specified message to the WWI file.
        /// </summary>
        /// <param name="message">The message content to log.</param>
        /// <param name="isIncommingMessage">
        /// If set to <c>true</c> the specified message has been received.
        /// If set to <c>false</c> the specified message has been sent.
        /// </param>
        void LogMessage(string message, bool isIncommingMessage = true);

        /// <summary>
        /// Logs the specified message to the WWI file.
        /// </summary>
        /// <param name="message">The message content to log.</param>
        /// <param name="isIncommingMessage">
        /// If set to <c>true</c> the specified message has been received.
        /// If set to <c>false</c> the specified message has been sent.
        /// </param>
        void LogMessage(byte[] message, bool isIncommingMessage = true);

        /// <summary>
        /// Logs the specified message to the WWI file.
        /// </summary>
        /// <param name="message">The message content to log.</param>
        /// <param name="index">The index in the message at which logging begins.</param>
        /// <param name="length">The number of bytes to log from the message array.</param>
        /// <param name="isIncommingMessage">
        /// If set to <c>true</c> the specified message has been received.
        /// If set to <c>false</c> the specified message has been sent.
        /// </param>
        void LogMessage(byte[] message, int index, int length, bool isIncommingMessage = true);
    }
}
