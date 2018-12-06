
namespace Rowa.Lib.Log
{
    /// <summary>
    /// The default WWI logger to use when no logging framework is available.
    /// </summary>
    internal class NullWwiLogger : IWwi
    {
        /// <summary>
        /// Logs the specified message to the WWI file.
        /// </summary>
        /// <param name="message">The message content to log.</param>
        /// <param name="isIncommingMessage">If set to <c>true</c> the specified message has been received.
        /// If set to <c>false</c> the specified message has been sent.</param>
        public void LogMessage(string message, bool isIncommingMessage = true)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Logs the specified message to the WWI file.
        /// </summary>
        /// <param name="message">The message content to log.</param>
        /// <param name="isIncommingMessage">If set to <c>true</c> the specified message has been received.
        /// If set to <c>false</c> the specified message has been sent.</param>
        public void LogMessage(byte[] message, bool isIncommingMessage = true)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Logs the specified message to the WWI file.
        /// </summary>
        /// <param name="message">The message content to log.</param>
        /// <param name="index">The index in the message at which logging begins.</param>
        /// <param name="length">The number of bytes to log from the message array.</param>
        /// <param name="isIncommingMessage">If set to <c>true</c> the specified message has been received.
        /// If set to <c>false</c> the specified message has been sent.</param>
        public void LogMessage(byte[] message, int index, int length, bool isIncommingMessage = true)
        {
            //Dummy Implementation
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            //Dummy Implementation
        }
    }
}
