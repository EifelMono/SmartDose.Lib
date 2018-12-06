using System;
using System.Collections.Generic;
using Rowa.Lib.Wcf.Logging;

namespace Rowa.Lib.Wcf.Gui
{
    /// <summary>
    /// Implements a default Rowa UI message service with basic functionality.
    /// </summary>
    public class MessageService<TMessageType, IServiceInterface> : ServiceBase<IServiceInterface>, 
                                                                   IMessageService, IGuiService, IServiceClient 
                                                                   where TMessageType : class
                                                                   where IServiceInterface : class
    {
        #region Constants

        /// <summary>
        /// The default timeout in milliseconds for GetMessage to return.
        /// </summary>
        private const int GetMessageTimeoutMilliSeconds = 5000;

        #endregion

        #region Members

        /// <summary>
        /// The XML message parser to use.
        /// </summary>
        private XmlMessageParser<TMessageType> _messageParser;

        /// <summary>
        /// Queue for messages that are pending to be returned via "GetMessage".
        /// </summary>
        private MessageQueue _messageQueue;

        /// <summary>
        /// The reference to trace the RAW messages that has been received or sent.
        /// </summary>
        private object _messageTrace;

        /// <summary>
        /// The dictionary of message processing methods for the different message types.
        /// </summary>
        private Dictionary<string, Action<TMessageType>> _messageProcessors = new Dictionary<string, Action<TMessageType>>();

        #endregion

        #region IMessageService Methods

        /// <summary>
        /// Is called to get notification from the service side.
        /// This method will block until a notification is available to deliver to the client or a timeout occured.
        /// </summary>
        /// <param name="message">
        ///  Optional serialized filter message for the notifications to retrieve.
        /// </param>
        /// <returns>
        /// Notification related object if successful; null otherwise.
        /// </returns>
        public string GetMessage(string message)
        {
            var resultMessage = _messageQueue.WaitForDequeue(GetMessageTimeoutMilliSeconds);

            if (resultMessage == null)
            {
                return null;
            }

            if ((_messageTrace != null) && (string.IsNullOrEmpty(resultMessage) == false))
            {
                LogManager.LogMessage(_messageTrace, resultMessage, false);
            }

            return resultMessage;
        }

        /// <summary>
        /// Is called to request data or actions from the service.
        /// </summary>
        /// <param name="message">
        /// Holds the serialized request message.
        /// </param>
        /// <returns>
        /// Result of the specified request.
        /// </returns>
        public string ProcessMessage(string message)
        {
            try
            {
                if ((_messageTrace != null) && (string.IsNullOrEmpty(message) == false))
                {
                    LogManager.LogMessage(_messageTrace, message, true);
                }

                var messageObject = _messageParser.Deserialize(message);

                if (messageObject == null)
                {
                    return null;
                }

                var messageTypeName = messageObject.GetType().Name;

                if (_messageProcessors.ContainsKey(messageTypeName))
                {
                    _messageProcessors[messageTypeName].Invoke(messageObject);
                }
                else
                {
                    ProcessMessage(messageObject);
                }                

                var resultMessage = _messageParser.Serialize(messageObject);

                if ((this._messageTrace != null) && (string.IsNullOrEmpty(resultMessage) == false))
                {
                    LogManager.LogMessage(_messageTrace, resultMessage, false);
                }

                return resultMessage;
            }
            catch (Exception ex)
            {
                this.Error(ex, $"Processing message '{message}' failed!");
            }

            return null;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageService{TMessageType, IServiceInterface}"/> class.
        /// </summary>
        protected MessageService()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageService{TMessageType}" /> class.
        /// </summary>
        /// <param name="servicePort">The tcp port of the WCF service.</param>
        /// <param name="serviceLocation">The location of the WCF service.</param>
        /// <param name="enableMessageTrace">if set to <c>true</c> tracing of RAW messages is enabled.</param>
        /// <param name="logUnknownMessageAsError">if set to <c>true</c> unknown messages are logged as error.</param>
        protected MessageService(ushort servicePort, 
                                 string serviceLocation, 
                                 bool enableMessageTrace,
                                 bool logUnknownMessageAsError = true)
            : base(servicePort, 
                   serviceLocation, 
                   BindingType.BasicHttp, 
                   DefaultCommunicationTimeoutSeconds)
        {
            _messageParser = new XmlMessageParser<TMessageType>(logUnknownMessageAsError);
            _messageQueue = new MessageQueue();

            if (enableMessageTrace)
            {
                _messageTrace = LogManager.GetWwi(serviceLocation, "127.0.0.1", servicePort);
            }
        }

        /// <summary>
        /// Initializes the specified service port.
        /// </summary>
        /// <param name="servicePort">The TCP port to use for the WCF service endpoint.</param>
        /// <param name="serviceLocation">The location to use as appendix for the url to identify the WCF service endpoint.</param>
        /// <param name="bindingType">The WCF binding type to use for the service endpoint.</param>
        /// <param name="communicationTimeoutSeconds">The WCF communication timeout to use in seconds.</param>
        public void Initialize(ushort servicePort,
                               string serviceLocation,
                               bool enableMessageTrace,
                               bool logUnknownMessageAsError = true)
        {
            base.Initialize(servicePort, serviceLocation, BindingType.BasicHttp, DefaultCommunicationTimeoutSeconds);

            _messageParser = new XmlMessageParser<TMessageType>(logUnknownMessageAsError);
            _messageQueue = new MessageQueue();

            if (enableMessageTrace)
            {
                _messageTrace = LogManager.GetWwi(serviceLocation, "127.0.0.1", servicePort);
            }
        }
        
        /// <summary>
        /// Registers the specified message type as supported message type for processing.
        /// </summary>
        /// <typeparam name="T">Message type to register.</typeparam>
        protected void RegisterMessageType<T>() where T : TMessageType
        {
            _messageParser.AddMessageType(typeof(T), null);
        }

        /// <summary>
        /// Registers the specified message type as supported message type for processing
        /// and additionally registers a handler action which is called when the message
        /// has been received.
        /// </summary>
        /// <typeparam name="T">Message type to register.</typeparam>
        /// <param name="messageHandler">The message handler to register.</param>
        protected void RegisterMessageType<T>(Action<TMessageType> messageHandler) where T : TMessageType
        {
            if (messageHandler == null)
            {
                throw new ArgumentException(nameof(messageHandler));
            }

            _messageParser.AddMessageType(typeof(T), null);
            _messageProcessors.Add(typeof(T).Name, messageHandler);
        }

        /// <summary>
        /// Registers the specified message type as supported message type for processing.
        /// </summary>
        /// <typeparam name="T">Message type to register.</typeparam>
        /// <param name="alternateTypeName"> Alternate type name to use for this message.</param>
        protected void RegisterMessageType<T>(string alternateTypeName = null) where T : TMessageType
        {
            _messageParser.AddMessageType(typeof(T), alternateTypeName);
        }

        /// <summary>
        /// Registers the specified message type as supported message type for processing
        /// and additionally registers a handler action which is called when the message
        /// has been received.
        /// </summary>
        /// <typeparam name="T">Message type to register.</typeparam>
        /// <param name="messageHandler">The message handler to register.</param>
        /// <param name="alternateTypeName"> Alternate type name to use for this message.</param>
        protected void RegisterMessageType<T>(Action<TMessageType> messageHandler,
                                              string alternateTypeName = null) where T : TMessageType
        {
            _messageParser.AddMessageType(typeof(T), alternateTypeName);
        }

        /// <summary>
        /// Sends the specified message to the connected WCF client
        /// by enqueuing it in the according message queue.
        /// </summary>
        /// <param name="message">
        /// The message to send.
        /// </param>
        protected void SendMessage(TMessageType message)
        {
            _messageQueue.Enqueue(_messageParser.Serialize(message));
        }

        /// <summary>
        /// Processes the specified message object.
        /// This method is intended to be overriden by child classes.
        /// </summary>
        /// <param name="message">
        /// The message to process.
        /// </param>
        protected virtual void ProcessMessage(TMessageType message)
        {
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }

            base.Dispose(disposing);

            if (disposing)
            {
                try
                {
                    if (_messageTrace != null)
                    {
                        if (_messageTrace is IDisposable)
                        {
                            ((IDisposable)_messageTrace).Dispose();
                        }
                        
                        _messageTrace = null;
                    }
                }
                catch (Exception ex)
                {
                    this.Error(ex, "Stopping WCF message service failed!");
                }
            }
        }

        #endregion
    }
}
