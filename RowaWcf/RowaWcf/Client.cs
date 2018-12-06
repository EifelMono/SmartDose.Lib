using Rowa.Lib.Wcf.Logging;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;

namespace Rowa.Lib.Wcf
{
    /// <summary>
    /// Implements an extended version of a WCF client wich is able to 
    /// automatically reconnect to the configured service in the background.
    /// </summary>
    /// <typeparam name="TWcfClientType">The type of the generated WCF client to use internally.</typeparam>
    /// <typeparam name="IServiceInterface">The type of the WCF service interface to consume.</typeparam>
    /// <seealso cref="System.IDisposable" />
    public class Client<TWcfClientType, IServiceInterface> : IClient<IServiceInterface>, IDisposable 
                                                             where TWcfClientType : class, ICommunicationObject, IServiceInterface
                                                             where IServiceInterface : class
    {
        #region Constants

        /// <summary>
        /// The default connection check/reconnect interval in seconds.
        /// </summary>
        public const uint DefaultConnectionCheckIntervalSeconds = 5;
        
        /// <summary>
        /// The default communication timeout in seconds.
        /// </summary>
        public const uint DefaultCommunicationTimeoutSeconds = 1800;

        /// <summary>
        /// The default open timeout in milli seconds.
        /// </summary>
        private const int DefaultOpenTimeoutMilliSeconds = 60000;

        #endregion

        #region Members

        /// <summary>
        /// The url of the WCF service to connect to.
        /// </summary>
        protected string _serviceUrl = string.Empty;

        /// <summary>
        /// The connection check and reconnect interval to use in milliseconds.
        /// </summary>
        private int _connectionCheckIntervalMilliseconds = 0;

        /// <summary>
        /// The WCF binding to use when creating the client.
        /// </summary>
        private Binding _binding = null;

        /// <summary>
        /// The WCF endpoint to use when creating the client.
        /// </summary>
        private EndpointAddress _endpoint = null;

        /// <summary>
        /// An optional instance context argument to use when creating the TWcfClientType instance.
        /// </summary>
        private object _instanceContextObject = null;

        /// <summary>
        /// The thread which is processing the connection checks and reconnects in the background.
        /// </summary>
        private Thread _connectThread = null;

        /// <summary>
        /// The event which is used to shutdown the background thread.
        /// </summary>
        private ManualResetEvent _shutdownThreadEvent = new ManualResetEvent(false);

        /// <summary>
        /// Queue which is used to asynchronously raise events in the correct order.
        /// </summary>
        private Queue<Delegate> _eventQueue = new Queue<Delegate>();

        /// <summary>
        /// The generated WCF client itself which is currently connected to the service.
        /// </summary>
        private TWcfClientType _wcfClient = null;

        /// <summary>
        /// This avoid a "Cannot access a disposed object" Exception in the client. (hopefully)
        /// </summary>
        private object _syncLock = new object();

        /// <summary>
        /// Flag whether the client has already been disposed.
        /// </summary>
        protected bool _isDisposed = false;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether this client is still connected to the server
        /// </summary>
        public bool IsConnected
        {
            get
            {
                lock (_syncLock)
                {
                    return (_wcfClient.State == CommunicationState.Opened);
                }
            }
        }
        
        /// <summary>
        /// Gets the reference to the currently connected WCF service.
        /// </summary>
        public IServiceInterface Service
        {
            get {  lock (_syncLock) { return _wcfClient; } }
        }

        #endregion

        #region Events

        /// <summary>
        /// Event which is thrown when the client automatically created a new connection to the WCF service.
        /// </summary>
        public event EventHandler ConnectionEstablished;

        /// <summary>
        /// Event which is thrown when the client lost the connection to the WCF service.
        /// </summary>
        public event EventHandler ConnectionClosed;

        #endregion

        #region Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="Client{TWcfClientType, IServiceInterface}" /> class.
        /// </summary>
        /// <param name="serviceUrl">The url of the WCF service to connect to.</param>
        /// <param name="bindingType">The type of WCF binding to use.</param>
        /// <param name="communicationTimeoutSeconds">The communication timeout to use in seconds.</param>
        /// <param name="connectionCheckIntervalSeconds">The connection check interval to use in seconds.</param>
        /// <param name="instanceContextObject">Optional instance context object to use.</param>
        public Client(string serviceUrl, 
                      BindingType bindingType = BindingType.NetTcp,
                      uint communicationTimeoutSeconds = DefaultCommunicationTimeoutSeconds,
                      uint connectionCheckIntervalSeconds = DefaultConnectionCheckIntervalSeconds,
                      object instanceContextObject = null)
        {
            if (string.IsNullOrEmpty(serviceUrl))
            {
                throw new ArgumentException(nameof(serviceUrl));
            }

            if (communicationTimeoutSeconds == 0)
            {
                throw new ArgumentException(nameof(communicationTimeoutSeconds));
            }

            if (connectionCheckIntervalSeconds == 0)
            {
                throw new ArgumentException(nameof(connectionCheckIntervalSeconds));
            }

            _serviceUrl = serviceUrl;
            _connectionCheckIntervalMilliseconds = (int)(connectionCheckIntervalSeconds * 1000);
            _endpoint = new EndpointAddress(serviceUrl);
            _binding = BindingBuilder.Build(bindingType, communicationTimeoutSeconds);
            _instanceContextObject = instanceContextObject;
            _wcfClient = CreateWcfClient();

            this.Info($"Prepared WCF client for '{_serviceUrl}'.");
        }
                
        /// <summary>
        /// Starts the automatic reconnect logic of the client.
        /// </summary>
        public virtual void Start()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            if (_connectThread != null)
            {
                throw new InvalidOperationException("Client is already started.");
            }

            _shutdownThreadEvent.Reset();
            _connectThread = new Thread(new ThreadStart(RunConnectionChecks));
            _connectThread.Priority = ThreadPriority.BelowNormal;
            _connectThread.IsBackground = true;
            _connectThread.Start();

            this.Debug($"Started client for '{_serviceUrl}'.");
        }

        /// <summary>
        /// Stops the automatic reconnect logic of the client and closes the existing WCF connection.
        /// </summary>
        public virtual void Stop()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            this.Debug($"Stopping client for '{_serviceUrl}'.");

            if (_connectThread != null)
            {
                _shutdownThreadEvent.Set();
                _connectThread.Join();
                _connectThread = null;
            }

            if (_wcfClient != null)
            {
                OnPrepareWcfClientDisconnect(_wcfClient);

                _wcfClient.Abort();
                _wcfClient.Close();
            }

            RaiseEvent(this.ConnectionClosed);
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

                lock (_eventQueue)
                {
                    _eventQueue.Clear();
                }

                _shutdownThreadEvent.Dispose();
            }

            _isDisposed = true;
        }
                
        /// <summary>
        /// Method that is called when a new WCF client instance has successfully connected to the WCF service.
        /// </summary>
        /// <param name="client">The WCF client instance that just connected to the WCF service.</param>
        protected virtual void OnWcfClientConnected(TWcfClientType client)
        {

        }

        /// <summary>
        /// Method that is called before the specified WCF client will disconnect from the WCF service.
        /// </summary>
        /// <param name="client">The WCF client instance that is about to be disconnected.</param>
        protected virtual void OnPrepareWcfClientDisconnect(TWcfClientType client)
        {

        }

        /// <summary>
        /// Creates a new instance of the generated WCF client with the specified constructor arguments.
        /// </summary>
        /// <returns>
        /// Newly created WCF client instance.
        /// </returns>
        private TWcfClientType CreateWcfClient()
        {
            if (_instanceContextObject == null)
            {
                return (TWcfClientType)Activator.CreateInstance(typeof(TWcfClientType), _binding, _endpoint);
            }

            return (TWcfClientType)Activator.CreateInstance(typeof(TWcfClientType), new InstanceContext(_instanceContextObject), _binding, _endpoint);
        }

        /// <summary>
        /// Runs the connection checks and automatic reconnect thread.
        /// </summary>
        private void RunConnectionChecks()
        {
            var wasConnected = false;

            do
            {
                try
                {
                    if (_wcfClient.State == CommunicationState.Opened)
                    {                       
                        continue;
                    }

                    if (wasConnected)
                    {
                        RaiseEvent(this.ConnectionClosed);
                    }

                    lock (_syncLock)
                    {
                        if (_wcfClient.State == CommunicationState.Faulted)
                        {
                            _wcfClient.Abort();
                        }

                        _wcfClient.Close();

                        this.Info($"Creating new WCF client for '{_serviceUrl}'.");

                        var currentClient = CreateWcfClient();

                        var openResult = currentClient.BeginOpen(null, null);
                        var waitHandles = new WaitHandle[] { _shutdownThreadEvent, openResult.AsyncWaitHandle };
                        var waitResult = WaitHandle.WaitAny(waitHandles, DefaultOpenTimeoutMilliSeconds);

                        if ((waitResult != WaitHandle.WaitTimeout) &&
                            (waitHandles[waitResult] == _shutdownThreadEvent))
                        {
                            this.Info($"Opening WCF client for '{_serviceUrl}' was cancelled.");
                            return;
                        }

                        currentClient.EndOpen(openResult);
                        OnWcfClientConnected(currentClient);
                        Interlocked.Exchange(ref _wcfClient, currentClient);
                    }

                    wasConnected = true;
                    RaiseEvent(this.ConnectionEstablished);
                }
                catch (Exception ex)
                {
                    this.Error(ex, $"Opening WCF client for '{_serviceUrl}' failed.");
                }
            }
            while (_shutdownThreadEvent.WaitOne(_connectionCheckIntervalMilliseconds) == false);
        }

        /// <summary>
        /// Asynchronously raises events of this client to prevent blocking by event handler.
        /// </summary>
        /// <param name="eventMethod">The event to raise asynchronously.</param>
        private void RaiseEvent(Delegate eventMethod)
        {
            if (eventMethod == null)
            {
                return;
            }

            lock (_eventQueue)
            {
                _eventQueue.Enqueue(eventMethod);
            }

            ThreadPool.QueueUserWorkItem((o) =>
            {
                try
                {
                    lock (_eventQueue)
                    {
                        if (_eventQueue.Count > 0)
                        {
                            var e = _eventQueue.Dequeue();
                            e.DynamicInvoke(this, new EventArgs());
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.Error(ex, $"Raising event '{eventMethod.Method.Name}' failed.");
                }
            });
        }

        #endregion
    }
}
