
using System;
using System.Reflection;
using System.ServiceModel;
using System.Threading.Tasks;
using RowaMore;
using RowaMore.Extensions;

namespace SmartDose.WcfLib
{
    public abstract class ServiceClient : IDisposable, IServiceClient
    {
        public TimeSpan WaitOnFault { get; set; } = TimeSpan.FromSeconds(1);
        public string EndpointAddress { get; protected set; }
        public SecurityMode SecurityMode { get; protected set; }
        // Timeout?!

        public bool ThrowOnConnectionError { get; set; } = false;

        public ServiceClient(string endpointAddress, SecurityMode securityMode = SecurityMode.None)
        {
            EndpointAddress = endpointAddress;
            SecurityMode = securityMode;
            Run();
        }

        protected bool Disposed = false;
        public void Dispose()
        {
            if (!Disposed)
                QueuedEvent.New(ClientEvent.Dispose);
        }

        #region Client
        public ICommunicationObject Client { get; set; }

        public enum ClientEvent
        {
            None,

            Opening,
            Opened,
            Faulted,
            Closing,
            Closed,

            Exception,
            Dispose,

            Restart,
        }
        public QueuedEvent<ClientEvent> QueuedEvent { get; set; } = new QueuedEvent<ClientEvent>();

        public virtual void CreateClient()
            => throw new NotImplementedException();
        #region Client Abstract 
        protected async Task CallAsyncMethode(MethodInfo methodInfo, string methodeName)
        {
            try
            {
                if (methodInfo == null)
                    methodInfo = Client.GetType().GetMethod(methodeName);
                await ((dynamic)methodInfo?.Invoke(Client, null)).ConfigureAwait(false);
            }
            catch { }
        }
        protected MethodInfo OpenAsyncMethod { get; set; } = null;
        public async virtual Task OpenAsync()
            => await CallAsyncMethode(OpenAsyncMethod, nameof(OpenAsync)).ConfigureAwait(false);
        protected MethodInfo CloseAsyncMethod { get; set; } = null;
        public async virtual Task CloseAsync()
            => await CallAsyncMethode(CloseAsyncMethod, nameof(CloseAsync)).ConfigureAwait(false);

        protected MethodInfo SubscribeForCallbacksAsyncMethod { get; set; } = null;
        public async virtual Task SubscribeForCallbacksAsync()
              => await CallAsyncMethode(SubscribeForCallbacksAsyncMethod, nameof(SubscribeForCallbacksAsync)).ConfigureAwait(false);

        protected MethodInfo UnsubscribeForCallbacksAsyncMethod { get; set; } = null;
        public async virtual Task UnsubscribeForCallbacksAsync()
                => await CallAsyncMethode(UnsubscribeForCallbacksAsyncMethod, nameof(UnsubscribeForCallbacksAsync)).ConfigureAwait(false);

        #endregion

        #region Client Events

        public event Action<ClientEvent> OnClientEvent;
        protected void AssignClientEvents(bool on)
        {
            if (Client is null)
                return;
            switch (on)
            {
                case true:
                    Client.Opening += Client_Opening;
                    Client.Opened += Client_Opened;
                    Client.Faulted += Client_Faulted;
                    Client.Closing += Client_Closing;
                    Client.Closed += Client_Closed;
                    break;
                case false:
                    Client.Opening -= Client_Opening;
                    Client.Opened -= Client_Opened;
                    Client.Faulted -= Client_Faulted;
                    Client.Closing -= Client_Closing;
                    Client.Closed -= Client_Closed;
                    break;
            }
        }

        private void Client_Opening(object sender, EventArgs e)
            => QueuedEvent.New(ClientEvent.Opening);
        private void Client_Opened(object sender, EventArgs e)
            => QueuedEvent.New(ClientEvent.Opened);
        private void Client_Faulted(object sender, EventArgs e)
         => QueuedEvent.New(ClientEvent.Faulted);
        private void Client_Closing(object sender, EventArgs e)
            => QueuedEvent.New(ClientEvent.Closing);
        private void Client_Closed(object sender, EventArgs e)
            => QueuedEvent.New(ClientEvent.Closed);
        #endregion

        #region Assign Callbacks
        protected abstract void AssignClientCallbacks(bool on);
        #endregion

        #region Client Run
        public bool IsConnected { get; set; } = false;
        protected virtual void Run()
        {
            Task.Run(async () =>
            {
                try
                {
                    QueuedEvent.OnNew = (e) =>
                    {
                        OnClientEvent?.Invoke(e);
                    };
                    var inFault = false;
                    var running = true;
                    RunOpen();
                    while (running)
                    {
                        if (await QueuedEvent.Next() is var nextEvent && nextEvent.Ok)
                            switch (nextEvent.Value)
                            {
                                case ClientEvent.Dispose:
                                    RunClose();
                                    running = false;
                                    break;
                                case ClientEvent.Opening:
                                    break;
                                case ClientEvent.Opened:
                                    IsConnected = true;
                                    break;
                                case ClientEvent.Restart:
                                case ClientEvent.Faulted:
                                    if (!inFault)
                                    {
                                        inFault = true;
                                        Client.Abort();
                                        RunClose();
                                        await Task.Delay(WaitOnFault.Milliseconds);
                                        RunOpen();
                                        inFault = false;
                                    }
                                    break;
                                case ClientEvent.Closing:
                                    break;
                                case ClientEvent.Closed:
                                    break;
                            }
                    }
                }
                catch (Exception ex)
                {
                    ex.LogException();
                    QueuedEvent.New(ClientEvent.Exception);
                }
                finally
                {
                    RunClose();
                    Disposed = true;
                    QueuedEvent.OnNew = null;
                }
            });
        }

        protected void RunOpen()
        {
            CreateClient();
            AssignClientEvents(true);
            AssignClientCallbacks(true);
            OpenAsync().Wait();
            SubscribeForCallbacksAsync().Wait();
        }

        protected void RunClose()
        {
            IsConnected = false;
            UnsubscribeForCallbacksAsync().Wait();
            CloseAsync().Wait();
            AssignClientCallbacks(false);
            AssignClientEvents(false);
        }
        #endregion
        #endregion

    }
}
