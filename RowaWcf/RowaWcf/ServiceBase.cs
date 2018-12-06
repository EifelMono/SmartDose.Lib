using Rowa.Lib.Wcf.Logging;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace Rowa.Lib.Wcf
{
    /// <summary>
    /// Base class for all single instance based WCF services.
    /// </summary>
    /// <typeparam name="IServiceInterface">The type of the service contract interface to host via WCF.</typeparam>
    /// <seealso cref="System.IDisposable" />
    [ServiceBehavior(
          InstanceContextMode = InstanceContextMode.Single
        , ConcurrencyMode = ConcurrencyMode.Multiple
        , UseSynchronizationContext = false)
    ]
    public class ServiceBase<IServiceInterface> : IDisposable where IServiceInterface : class
    {
        #region Constants

        /// <summary>
        /// The default communication timeout in seconds.
        /// </summary>
        protected const uint DefaultCommunicationTimeoutSeconds = 1800;

        #endregion

        #region Members

        /// <summary>
        /// The URL that is used to publish this WCF service. 
        /// </summary>
        private string _serviceUrl = string.Empty;

        /// <summary>
        /// Reference to the self-hosted WCF service. 
        /// </summary>
        private ServiceHost _serviceHost = null;

        /// <summary>
        /// Flag whether this instance is disposed.
        /// </summary>
        protected bool _isDisposed = false;

        #endregion

        #region Protected Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceBase{IServiceInterface}"/> class.
        /// </summary>
        protected ServiceBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceBase{I}"/> class.
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
        protected ServiceBase(ushort servicePort, 
                              string serviceLocation, 
                              BindingType bindingType = BindingType.NetTcp,
                              uint communicationTimeoutSeconds = DefaultCommunicationTimeoutSeconds,
                              bool includeExceptionDetailInFaults = false)
        {
            Initialize(servicePort, serviceLocation, bindingType, communicationTimeoutSeconds, includeExceptionDetailInFaults);
        }

        /// <summary>
        /// Initializes the specified service port.
        /// </summary>
        /// <param name="servicePort">The TCP port to use for the WCF service endpoint.</param>
        /// <param name="serviceLocation">The location to use as appendix for the url to identify the WCF service endpoint.</param>
        /// <param name="bindingType">The WCF binding type to use for the service endpoint.</param>
        /// <param name="communicationTimeoutSeconds">The WCF communication timeout to use in seconds.</param>
        /// <param name="includeExceptionDetailInFaults">if set to <c>true</c> exception details are included into WCF faults.</param>
        protected virtual void Initialize(ushort servicePort,
                                          string serviceLocation,
                                          BindingType bindingType = BindingType.NetTcp,
                                          uint communicationTimeoutSeconds = DefaultCommunicationTimeoutSeconds,
                                          bool includeExceptionDetailInFaults = false)
        {
            if (_serviceHost != null)
            {
                _serviceHost.Abort();
                _serviceHost.Close();
                _serviceHost = null;
            }

            if (typeof(IServiceInterface).IsInterface == false)
            {
                throw new ArgumentException($"Specified type {typeof(IServiceInterface).Name} is not an interface type.");
            }

            if (GetType().GetInterface(typeof(IServiceInterface).Name) == null)
            {
                throw new ArgumentException($"This instance does not implement interface type {typeof(IServiceInterface).Name}.");
            }

            if (servicePort == 0)
            {
                throw new ArgumentException(nameof(servicePort));
            }

            if (string.IsNullOrEmpty(serviceLocation))
            {
                throw new ArgumentException(nameof(serviceLocation));
            }

            if (communicationTimeoutSeconds == 0)
            {
                throw new ArgumentException(nameof(communicationTimeoutSeconds));
            }

            var uri = UriBuilder.Build(servicePort, serviceLocation, bindingType);
            var serviceBinding = BindingBuilder.Build(bindingType, communicationTimeoutSeconds);
            var metaDataBinding = BindingBuilder.BuildMex(bindingType);

            _serviceUrl = uri.ToString();
            _serviceHost = new ServiceHost(this, uri);

            if (bindingType == BindingType.BasicHttp)
            {
                _serviceHost.Description.Behaviors.Add(new ServiceMetadataBehavior() { HttpGetEnabled = true, HttpsGetEnabled = true });
            }
            else
            {
                _serviceHost.Description.Behaviors.Add(new ServiceMetadataBehavior());
            }

            if (includeExceptionDetailInFaults)
            {
                var debugBehaviour = _serviceHost.Description.Behaviors.Find<ServiceDebugBehavior>();

                if (debugBehaviour == null)
                {
                    _serviceHost.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });
                }
                else
                {
                    if (debugBehaviour.IncludeExceptionDetailInFaults == false)
                    {
                        debugBehaviour.IncludeExceptionDetailInFaults = true;
                    }
                }
            }

            _serviceHost.AddServiceEndpoint(typeof(IMetadataExchange), metaDataBinding, "mex");
            _serviceHost.AddServiceEndpoint(typeof(IServiceInterface), serviceBinding, _serviceUrl);
        }

        /// <summary>
        /// Starts the WCF service by opening the according service host.
        /// </summary>
        public virtual void Start()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }

            if (_serviceHost.State == CommunicationState.Opened)
            {
                throw new InvalidOperationException("Service is already started.");
            }


            this.Info($"Starting WCF service '{_serviceUrl}' ...");
            _serviceHost.Open();
        }

        /// <summary>
        /// Stops the WCF service by closing the according service host.
        /// </summary>
        public virtual void Stop()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }

            if (_serviceHost != null)
            {
                if (_serviceHost.State == CommunicationState.Closed)
                {
                    return;
                }

                this.Info($"Stopping WCF service '{_serviceUrl}' ...");

                _serviceHost.Abort();
                _serviceHost.Close();
            }
            
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }

            if (disposing)
            {
                Stop();
            }

            _isDisposed = true;
        }

        /// <summary>
        /// Gets the endpoint information of the currently calling client.
        /// </summary>
        /// <returns>
        /// Endpoint information of the currently calling client.
        /// </returns>
        protected string GetCurrentClientEndpoint()
        {
            var context = OperationContext.Current;
            var properties = context.IncomingMessageProperties;
            var endpointProperty = properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            return $"{endpointProperty.Address}:{endpointProperty.Port}";
        }

        #endregion

    }
}
