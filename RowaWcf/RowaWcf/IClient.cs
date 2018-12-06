using System;

namespace Rowa.Lib.Wcf
{
    /// <summary>
    /// Defines the basic interface of an extended version of a WCF client wich is able to 
    /// automatically reconnect to the configured service in the background.
    /// This interface is required for mocking support in unit tests.
    /// </summary>
    /// <typeparam name="IServiceInterface">The type of the service interface.</typeparam>
    public interface IClient<IServiceInterface> where IServiceInterface : class
    {
        #region Properties

        /// <summary>
        /// Gets a value indicating whether this client is still connected to the server
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Gets the reference to the currently connected WCF service.
        /// </summary>
        IServiceInterface Service { get; }

        #endregion

        #region Events

        /// <summary>
        /// Event which is thrown when the client automatically created a new connection to the WCF service.
        /// </summary>
        event EventHandler ConnectionEstablished;

        /// <summary>
        /// Event which is thrown when the client lost the connection to the WCF service.
        /// </summary>
        event EventHandler ConnectionClosed;

        #endregion
    }
}
