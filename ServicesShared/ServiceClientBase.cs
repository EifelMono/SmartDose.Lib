#define UseModel
#if MasterData9002
#undef UseModel
#endif
using System;
using System.ServiceModel;
using System.Threading.Tasks;
using RowaMore;
using RowaMore.Extensions;

#if MasterData10000
namespace MasterData10000
#elif Settings10000
namespace Settings10000
#elif MasterData9002
namespace MasterData9002
#else
namespace ServicesShared
#endif
{
    public abstract class ServiceClientBase : IDisposable, IServiceClient
    {
        public TimeSpan WaitOnFault { get; set; } = TimeSpan.FromSeconds(1);
        public string EndpointAddress { get; protected set; }
        public SecurityMode SecurityMode { get; protected set; }
        // Timeout?!

        public bool ThrowOnConnectionError { get; set; } = false;

        public ServiceClientBase(string endpointAddress, SecurityMode securityMode = SecurityMode.None)
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
        public virtual Task OpenAsync() 
            => throw new NotImplementedException();
        public virtual Task CloseAsync() 
            => throw new NotImplementedException();
        public virtual Task SubscribeForCallbacksAsync() 
            => throw new NotImplementedException();
        public virtual Task UnsubscribeForCallbacksAsync() 
            => throw new NotImplementedException();
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
                    RunOpen(withSubscribe: true);
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
                                    // await SubscribeForCallbacksAsync();
                                    break;
                                case ClientEvent.Restart:
                                case ClientEvent.Faulted:
                                    if (!inFault)
                                    {
                                        inFault = true;
                                        Client.Abort();
                                        RunClose();
                                        await Task.Delay(WaitOnFault.Milliseconds);
                                        RunOpen(withSubscribe: true);
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

        protected void RunOpen(bool withSubscribe)
        {
            CreateClient();
            AssignClientEvents(true);
            AssignClientCallbacks(true);
            OpenAsync();
            if (withSubscribe)
                SubscribeForCallbacksAsync().Wait();
        }

        protected void RunClose()
        {
            IsConnected = false;
            UnsubscribeForCallbacksAsync().Wait();
            CloseAsync();
            AssignClientCallbacks(false);
            AssignClientEvents(false);
        }
        #endregion
        #endregion

        #region SafeExecuter Catcher

        protected void Catcher(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        protected void CatcherAsTask(Action action)
        {
            Task.Run(() =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    ex.LogException();
                }
            });
        }

#if UseModel
        protected async Task<TResult> CatcherServiceResultAsync<TResult>(Func<Task<TResult>> func) where TResult : ServiceResult, new()
        {
            try
            {
                return await func().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                if (ThrowOnConnectionError)
                    throw ex;
                return new TResult { Exception = ex, Status = ServiceResultStatus.ErrorConnection };
            }
        }
#endif
        protected async Task<TResult> CatcherAsync<TResult>(Func<Task<TResult>> func)
        {
            try
            {
                return await func().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                if (ThrowOnConnectionError)
                    throw ex;
                return default(TResult);
            }
        }

        protected async Task CatcherAsync(Func<Task> func)
        {
            try
            {
                await func().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                if (ThrowOnConnectionError)
                    throw ex;
            }
        }

        protected async Task CatcherAsyncIgnore(Func<Task> func)
        {
            try
            {
                await func().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                if (ThrowOnConnectionError)
                    throw ex;
            }
        }
        #endregion

        #region Model
#if UseModel
        public ModelBuilder<T> Model<T>() where T : class
        {
            return new ModelBuilder<T>(this) { };
        }

        #region Read
        public virtual Task<ServiceResult> ExecuteModelReadAsync(ReadBuilder readBuilder) 
            => throw new NotImplementedException();

        public ReadBuilder<T> ModelRead<T>() where T : class
        {
            return new ReadBuilder<T>(this) { };
        }
        #endregion

        #region Delete
        public virtual Task<ServiceResult<bool>> ExecuteModelDeleteAsync(DeleteBuilder deleteBuilder)
            => throw new NotImplementedException();

        public DeleteBuilder<T> ModelDelete<T>() where T : class
        {
            return new DeleteBuilder<T>(this) { };
        }
        #endregion

        #region ReadIdOverIdentifier

        public virtual Task<ServiceResult<long>> ExecuteModelReadIdOverIdentifierAsync(ReadIdOverIdentifierBuilder readIdOverIdentifierBuilder)
            => throw new NotImplementedException();
        
        public ReadIdOverIdentifierBuilder<T> ModelReadIdOverIdentifier<T>() where T : class
        {
            return new ReadIdOverIdentifierBuilder<T>(this) { };
        }
      
        #endregion
#endif
        #endregion
    }
}
