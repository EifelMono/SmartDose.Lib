using System;
using System.ServiceModel;

namespace Rowa.Lib.Wcf
{
    /// <summary>
    /// Implements an extended version of a WCF client wich is able to 
    /// automatically reconnect to the configured service in the background and 
    /// provides support for WCF client callbacks by providing an accordingly implemented
    /// callback event handler.
    /// </summary>
    /// <typeparam name="TWcfClientType">The type of the generated WCF client to use internally.</typeparam>
    /// <typeparam name="IServiceInterface">The type of the WCF service interface to consume.</typeparam>
    /// <typeparam name="IClientCallbackInterface">The type of the client callback interface to use.</typeparam>
    /// <typeparam name="IClientCallbackEventInterface">The type of the callback handler and event provider interface.</typeparam>
    /// <seealso cref="Rowa.Lib.Wcf.ClientWithCallbacks{TWcfClientType, IServiceInterface, IClientCallbackInterface}" />
    /// <seealso cref="System.IDisposable" />
    public class ClientWithCallbackEvents<TWcfClientType, IServiceInterface, IClientCallbackInterface, IClientCallbackEventInterface> 
               : ClientWithCallbacks<TWcfClientType, IServiceInterface, IClientCallbackInterface>,
                 IClientWithCallbackEvents<IServiceInterface, IClientCallbackEventInterface>, IDisposable
                 where TWcfClientType : class, ICommunicationObject, IServiceInterface
                 where IServiceInterface : class
                 where IClientCallbackInterface : class
                 where IClientCallbackEventInterface : class, IClientCallbackInterface
    {
        /// <summary>
        /// Gets the callback handler which provides according events.
        /// </summary>
        public IClientCallbackEventInterface Events
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientWithCallbacks{TWcfClientType, IServiceInterface, IClientCallbackInterface}"/> class.
        /// </summary>
        /// <param name="serviceUrl">The url of the WCF service to connect to.</param>
        /// <param name="clientCallbackInstance">The client callback instance to use.</param>
        /// <param name="bindingType">The type of WCF binding to use.</param>
        /// <param name="communicationTimeoutSeconds">The communication timeout to use in seconds.</param>
        /// <param name="connectionCheckIntervalSeconds">The connection check interval to use in seconds.</param>
        public ClientWithCallbackEvents(string serviceUrl,
                                        IClientCallbackEventInterface clientCallbackInstance,
                                        BindingType bindingType = BindingType.NetTcp,
                                        uint communicationTimeoutSeconds = DefaultCommunicationTimeoutSeconds,
                                        uint connectionCheckIntervalSeconds = DefaultConnectionCheckIntervalSeconds)
            : base(serviceUrl,
                   clientCallbackInstance,
                   bindingType,
                   communicationTimeoutSeconds,
                   connectionCheckIntervalSeconds)
        {
            Events = clientCallbackInstance;
        }
    }
}
