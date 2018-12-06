using Rowa.Lib.Wcf.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Rowa.Lib.Wcf
{
    /// <summary>
    /// Base class for all single instance based WCF services which are supporting WCF client callbacks.
    /// </summary>
    /// <typeparam name="IServiceInterface">
    /// The type of the service contract interface to host via WCF.
    /// This interface has to inherit from IClientCallbackService.
    /// </typeparam>
    /// <typeparam name="IClientCallbackInterface">
    /// The type of the client callback interface to use for raising events.
    /// This interface has to define at least one method with the <c>OperationContract</c> attribute.
    /// </typeparam>
    /// <seealso cref=" Rowa.Lib.Wcf.ServiceBase{IServiceInterface}" />
    public class ServiceWithCallbacksBase<IServiceInterface, IClientCallbackInterface> : ServiceBase<IServiceInterface>, IClientCallbackService 
                                                                                         where IServiceInterface : class  
                                                                                         where IClientCallbackInterface : class
    {
        #region Members

        /// <summary>
        /// List with the clients that are currently subscribed for callbacks.
        /// </summary>
        private List<IClientCallbackInterface> _subscribedCallbackClients = new List<IClientCallbackInterface>();

        /// <summary>
        /// Dicitionary of meta information which are assigned to one or more clients.
        /// </summary>
        private Dictionary<string, List<IClientCallbackInterface>> _metaInfoClientAssignments = new Dictionary<string, List<IClientCallbackInterface>>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the count of currently subscribed callback clients.
        /// </summary>
        protected uint SubscribedClientCount
        {
            get
            {
                lock (_subscribedCallbackClients)
                {
                    return Convert.ToUInt32(_subscribedCallbackClients.Count);
                }
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceWithCallbacksBase{IServiceInterface, IClientCallbackInterface}"/> class.
        /// </summary>
        protected ServiceWithCallbacksBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceWithCallbacksBase{IServiceInterface, IClientCallbackInterface}"/> class.
        /// </summary>
        /// <param name="servicePort">
        /// The TCP port to use for the WCF service endpoint.
        /// </param>
        /// <param name="serviceLocation">
        /// The location to use as appendix for the url to identify the WCF service endpoint.
        /// </param>
        /// <param name="bindingType">
        /// The WCF binding type to use for the service endpoint.
        /// </param>
        /// <param name="communicationTimeoutSeconds">
        /// The WCF communication timeout to use in seconds.
        /// </param>
        /// <param name="includeExceptionDetailInFaults">if set to <c>true</c> exception details are included into WCF faults.</param>
        protected ServiceWithCallbacksBase(ushort servicePort, 
                                           string serviceLocation, 
                                           BindingType bindingType = BindingType.NetTcp, 
                                           uint communicationTimeoutSeconds = DefaultCommunicationTimeoutSeconds,
                                           bool includeExceptionDetailInFaults = false)
            : base(servicePort, 
                   serviceLocation, 
                   bindingType, 
                   communicationTimeoutSeconds,
                   includeExceptionDetailInFaults)
        {
            
        }

        /// <summary>
        /// Initializes the specified service port.
        /// </summary>
        /// <param name="servicePort">The TCP port to use for the WCF service endpoint.</param>
        /// <param name="serviceLocation">The location to use as appendix for the url to identify the WCF service endpoint.</param>
        /// <param name="bindingType">The WCF binding type to use for the service endpoint.</param>
        /// <param name="communicationTimeoutSeconds">The WCF communication timeout to use in seconds.</param>
        /// <param name="includeExceptionDetailInFaults">if set to <c>true</c> exception details are included into WCF faults.</param>
        protected override void Initialize(ushort servicePort, 
                                           string serviceLocation, 
                                           BindingType bindingType = BindingType.NetTcp, 
                                           uint communicationTimeoutSeconds = 1800,
                                           bool includeExceptionDetailInFaults = false)
        {
            if (typeof(IClientCallbackInterface).IsInterface == false)
            {
                Dispose();
                throw new ArgumentException($"Specified type {typeof(IClientCallbackInterface).Name} is not an interface type.");
            }

            base.Initialize(servicePort, serviceLocation, bindingType, communicationTimeoutSeconds, includeExceptionDetailInFaults);
        }

        /// <summary>
        /// Subscribes the calling client channel as callback listener.
        /// </summary>
        void IClientCallbackService.SubscribeForCallbacks()
        {
            var clientChannel = OperationContext.Current.GetCallbackChannel<IClientCallbackInterface>();

            if (clientChannel != null)
            {
                lock (_subscribedCallbackClients)
                {
                    if (_subscribedCallbackClients.Contains(clientChannel) == false)
                    {
                        _subscribedCallbackClients.Add(clientChannel);
                    }
                }

                try
                {
                    OnClientConnected(clientChannel);
                }
                catch (Exception ex)
                {
                    this.Error(ex, "Notifying about a new client connection failed.");
                }                
            }
        }        

        /// <summary>
        /// Unsubscribes the calling client channel as callback listener.
        /// </summary>
        void IClientCallbackService.UnsubscribeForCallbacks()
        {
            var clientChannel = OperationContext.Current.GetCallbackChannel<IClientCallbackInterface>();

            if (clientChannel != null)
            {
                lock (_subscribedCallbackClients)
                {
                    _subscribedCallbackClients.Remove(clientChannel);
                }

                try
                {
                    OnClientDisconnected(clientChannel);
                }
                catch (Exception ex)
                {
                    this.Error(ex, "Notifying about a client disconnect failed.");
                }
                finally
                {
                    ClearMetaInformation(clientChannel);
                }
            }
        }

        /// <summary>
        /// Is called when a new client has connected to the service and has subscribed for callbacks.
        /// </summary>
        /// <param name="client">The client that has connected.</param>
        protected virtual void OnClientConnected(IClientCallbackInterface client)
        {
        }

        /// <summary>
        /// Is called when a client has been disconnected or gracefully unsubscribed for callbacks.
        /// </summary>
        /// <param name="client">The client that has disconnected.</param>
        protected virtual void OnClientDisconnected(IClientCallbackInterface client)
        {            
        }

        /// <summary>
        /// Invokes the specified callback at the currently subscribed clients.
        /// </summary>
        /// <param name="callback">The client callback to invoke.</param>
        /// <returns>
        /// <c>true</c> if at least one client received the specified callback; <c>false</c> otherwise.
        /// </returns>
        protected bool Callback(Action<IClientCallbackInterface> callback)
        {
            IClientCallbackInterface[] callbackClients = null;
            var result = false;

            lock (_subscribedCallbackClients)
            {
                callbackClients = _subscribedCallbackClients.ToArray();
            }

            var occurredFaults = new List<FaultException>();

            foreach (var callbackClient in callbackClients)
            {
                try
                {
                    callback.Invoke(callbackClient);
                    result = true;
                }
                catch (FaultException ex)
                {
                    occurredFaults.Add(ex);
                    this.Error(ex, "Detected fault while invoking client callback.");
                }
                catch (Exception ex)
                {
                    this.Error(ex, "Invoking client callback failed -> assume that the affected client is not connected anymore.");

                    RemoveClient(callbackClient);

                    try
                    {
                        OnClientDisconnected(callbackClient);
                    }
                    catch (Exception exSub)
                    {
                        this.Error(exSub, "Notifying about a client disconnect failed.");
                    }
                }
            }

            if (occurredFaults.Count > 0)
            {
                throw new AggregateException(occurredFaults.ToArray());
            }

            return result;
        }

        /// <summary>
        /// Invokes the specified callback at the currently subscribed clients and checks 
        /// whether at least one client returned the expected result.
        /// </summary>
        /// <typeparam name="T">The type of callback result.</typeparam>
        /// <param name="callback">The client callback to invoke.</param>
        /// <param name="expectedResult">The expected callback result value.</param>
        /// <returns>
        /// <c>true</c> if at least one client received the specified callback and returned the expected result; <c>false</c> otherwise.
        /// </returns>
        protected bool Callback<TResult>(Func<IClientCallbackInterface, TResult> callback, TResult expectedResult)
        {
            IClientCallbackInterface[] callbackClients = null;
            var callbackResults = new List<TResult>();

            lock (_subscribedCallbackClients)
            {
                callbackClients = _subscribedCallbackClients.ToArray();
            }

            var occurredFaults = new List<FaultException>();

            foreach (var callbackClient in callbackClients)
            {
                try
                {
                    callbackResults.Add(callback.Invoke(callbackClient));
                }
                catch (FaultException ex)
                {
                    occurredFaults.Add(ex);
                    this.Error(ex, "Detected fault while invoking client callback.");
                }
                catch (Exception ex)
                {
                    this.Error(ex, "Invoking client callback failed -> assume that the affected client is not connected anymore.");

                    RemoveClient(callbackClient);

                    try
                    {
                        OnClientDisconnected(callbackClient);
                    }
                    catch (Exception exSub)
                    {
                        this.Error(exSub, "Notifying about a client disconnect failed.");
                    }
                }
            }

            if (occurredFaults.Count > 0)
            {
                throw new AggregateException(occurredFaults.ToArray());
            }

            return callbackResults.Contains(expectedResult);
        }

        /// <summary>
        /// Gets callback interface for the currently calling WCF client instance.
        /// </summary>
        /// <returns>
        /// Currently calling WCF client if it can be detected; null otherwise.s
        /// </returns>
        protected IClientCallbackInterface GetCurrentClient()
        {
            if (OperationContext.Current == null)
            {
                return null;
            }

            return OperationContext.Current.GetCallbackChannel<IClientCallbackInterface>();
        }

        /// <summary>
        /// Assigns the specified meta information to the specified client.
        /// </summary>
        /// <param name="client">The client to assign the specified meta information to.</param>
        /// <param name="metaInformation">The meta information to assign.</param>
        protected void AssignMetaInformation(IClientCallbackInterface client, string metaInformation)
        {
            if (client == null)
            {
                throw new ArgumentException(nameof(client));
            }

            if (metaInformation == null)
            {
                throw new ArgumentException(nameof(metaInformation));
            }

            lock (_subscribedCallbackClients)
            {
                if (_subscribedCallbackClients.Contains(client) == false)
                {
                    throw new ArgumentException("The specified client is not connected anymore.");
                }
            }

            lock (_metaInfoClientAssignments)
            {
                if (_metaInfoClientAssignments.ContainsKey(metaInformation) == false)
                {
                    _metaInfoClientAssignments[metaInformation] = new List<IClientCallbackInterface>();
                }

                if (_metaInfoClientAssignments[metaInformation].Contains(client) == false)
                {
                    _metaInfoClientAssignments[metaInformation].Add(client);
                }                
            }
        }

        /// <summary>
        /// Removes the specified meta information assignment of the specified client.
        /// </summary>
        /// <param name="client">The client to remove the specified meta information assignment for.</param>
        /// <param name="metaInformation">The meta information to remove.</param>
        protected void UnassignMetaInformation(IClientCallbackInterface client, string metaInformation)
        {
            if (client == null)
            {
                throw new ArgumentException(nameof(client));
            }

            if (metaInformation == null)
            {
                throw new ArgumentException(nameof(metaInformation));
            }

            lock (_metaInfoClientAssignments)
            {
                if (_metaInfoClientAssignments.ContainsKey(metaInformation) == false)
                {
                    return;
                }

                _metaInfoClientAssignments[metaInformation].Remove(client);

                if (_metaInfoClientAssignments[metaInformation].Count == 0)
                {
                    _metaInfoClientAssignments.Remove(metaInformation);
                }
            }
        }

        /// <summary>
        /// Clears all meta data assignments of the specified client.
        /// </summary>
        /// <param name="client">The client to clear all meta data assignments for.</param>
        protected void ClearMetaInformation(IClientCallbackInterface client)
        {
            if (client == null)
            {
                throw new ArgumentException(nameof(client));
            }

            lock (_metaInfoClientAssignments)
            {
                foreach (var metaInformation in _metaInfoClientAssignments.Keys.ToArray())
                {
                    var clients = _metaInfoClientAssignments[metaInformation];

                    if (clients.Contains(client) == false)
                    {
                        continue;
                    }

                    clients.Remove(client);

                    if (clients.Count == 0)
                    {
                        _metaInfoClientAssignments.Remove(metaInformation);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the list of meta information that are currently assigned to the specified client.
        /// </summary>
        /// <param name="client">The client to get the meta information for.</param>
        /// <returns>
        /// Assigned meta information for the specified client.
        /// </returns>
        protected List<string> GetMetaInformationForClient(IClientCallbackInterface client)
        {
            var result = new List<string>();

            lock (_metaInfoClientAssignments)
            {
                foreach (var key in _metaInfoClientAssignments.Keys)
                {
                    if (_metaInfoClientAssignments[key].Contains(client))
                    {
                        result.Add(key);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the clients with meta information.
        /// </summary>
        /// <param name="metaInformation">The meta information.</param>
        /// <returns></returns>
        protected List<IClientCallbackInterface> GetClientsWithMetaInformation(string metaInformation)
        {
            var result = new List<IClientCallbackInterface>();

            lock (_metaInfoClientAssignments)
            {
                if (_metaInfoClientAssignments.ContainsKey(metaInformation))
                {
                    result.AddRange(_metaInfoClientAssignments[metaInformation]);
                }
            }

            return result;
        }
        
        /// <summary>
        /// Removes the specified client from the list of subscribed callback clients.
        /// </summary>
        /// <param name="client">The client to remove.</param>
        protected void RemoveClient(IClientCallbackInterface client)
        {
            lock (_subscribedCallbackClients)
            {
                _subscribedCallbackClients.Remove(client);
            }

            ClearMetaInformation(client);
        }

        #endregion
    }
}
