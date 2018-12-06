using Rowa.Lib.Wcf.Logging;
using System;
using System.ServiceModel;

namespace Rowa.Lib.Wcf
{
    /// <summary>
    /// Implements an extended version of a WCF client wich is able to 
    /// automatically reconnect to the configured service in the background and 
    /// provides support for WCF client callbacks.
    /// </summary>
    /// <typeparam name="TWcfClientType">The type of the generated WCF client to use internally.</typeparam>
    /// <typeparam name="IServiceInterface">The type of the WCF service interface to consume.</typeparam>
    /// <typeparam name="IClientCallbackInterface">The type of the client callback interface to use.</typeparam>
    /// <seealso cref="Rowa.Lib.Wcf.Client{TWcfClientType, IServiceInterface}" />
    /// <seealso cref="System.IDisposable" />
    public class ClientWithCallbacks<TWcfClientType, IServiceInterface, IClientCallbackInterface> : Client<TWcfClientType, IServiceInterface>, IDisposable
                                                                                                    where TWcfClientType : class, ICommunicationObject, IServiceInterface
                                                                                                    where IServiceInterface : class
                                                                                                    where IClientCallbackInterface : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientWithCallbacks{TWcfClientType, IServiceInterface, IClientCallbackInterface}"/> class.
        /// </summary>
        /// <param name="serviceUrl">The url of the WCF service to connect to.</param>
        /// <param name="clientCallbackInstance">The client callback instance to use.</param>
        /// <param name="bindingType">The type of WCF binding to use.</param>
        /// <param name="communicationTimeoutSeconds">The communication timeout to use in seconds.</param>
        /// <param name="connectionCheckIntervalSeconds">The connection check interval to use in seconds.</param>
        public ClientWithCallbacks(string serviceUrl,
                                   IClientCallbackInterface clientCallbackInstance,
                                   BindingType bindingType = BindingType.NetTcp,
                                   uint communicationTimeoutSeconds = DefaultCommunicationTimeoutSeconds,
                                   uint connectionCheckIntervalSeconds = DefaultConnectionCheckIntervalSeconds)
            : base(serviceUrl,
                   bindingType,
                   communicationTimeoutSeconds,
                   connectionCheckIntervalSeconds,
                   clientCallbackInstance)
        {
        }

        /// <summary>
        /// Method that is called when a new WCF client instance has successfully connected to the WCF service.
        /// </summary>
        /// <param name="client">The WCF client instance that just connected to the WCF service.</param>
        protected override void OnWcfClientConnected(TWcfClientType client)
        {
            try
            {
                var methodInfo = client.GetType().GetMethod("SubscribeForCallbacks");

                if (methodInfo != null)
                {
                    methodInfo.Invoke(client, null);
                }
                else
                {
                    this.Fatal($"WCF service interface '{typeof(IServiceInterface).Name}' does not provide the method 'SubscribeForCallbacks'.");
                }
            }
            catch (Exception ex)
            {
                this.Error(ex, $"Subscribing for client callbacks of '{_serviceUrl}' failed.");
            }            
        }

        /// <summary>
        /// Method that is called before the specified WCF client will disconnect from the WCF service.
        /// </summary>
        /// <param name="client">The WCF client instance that is about to be disconnected.</param>
        protected override void OnPrepareWcfClientDisconnect(TWcfClientType client)
        {
            try
            {
                var methodInfo = client.GetType().GetMethod("UnsubscribeForCallbacks");

                if (methodInfo != null)
                {
                    methodInfo.Invoke(client, null);
                }
                else
                {
                    this.Fatal($"WCF service interface '{typeof(IServiceInterface).Name}' does not provide the method 'UnsubscribeForCallbacks'.");
                }
            }
            catch (Exception ex)
            {
                this.Error(ex, $"Unsubscribing for client callbacks of '{_serviceUrl}' failed.");
            }
        }

    }
}
