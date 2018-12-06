namespace Rowa.Lib.Wcf
{
    /// <summary>
    /// Defines the interface of a WCF client wich additionally provides an event interface for 
    /// incomming WCF service callbacks.
    /// </summary>
    /// <typeparam name="IServiceInterface">The type of the service interface.</typeparam>
    /// <typeparam name="IClientCallbackEventInterface">The type of the client callback event interface.</typeparam>
    public interface IClientWithCallbackEvents<IServiceInterface, IClientCallbackEventInterface> 
                   : IClient<IServiceInterface> 
                     where IServiceInterface : class
                     where IClientCallbackEventInterface : class
    {
        /// <summary>
        /// Gets the callback handler which provides according events.
        /// </summary>
        IClientCallbackEventInterface Events
        {
            get;
        }
    }
}
