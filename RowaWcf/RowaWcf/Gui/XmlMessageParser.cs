using Rowa.Lib.Wcf.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace Rowa.Lib.Wcf.Gui
{
    /// <summary>
    /// Class which implements the functionality to parse and serialize message objects of the specified type.
    /// </summary>
    internal sealed class XmlMessageParser<TMessageType> where TMessageType : class
    {
        #region Contants

        /// <summary>
        /// Regular expression to easily extract the serialized type out of an XML message string.
        /// </summary>
        private const string MessageTypeRegex = "^.*\\<(?<type>[^ ]+)";

        #endregion

        #region Members

        /// <summary>
        /// XML writer settings to use during serialization.
        /// </summary>
        private readonly XmlWriterSettings _xmlWriterSettings;

        /// <summary>
        /// XML Serializer settings to use during serialization.
        /// </summary>
        private readonly XmlSerializerNamespaces _xmlSerializerNamespaces;

        /// <summary>
        /// Regular expression to use for parse incomming XML content for the message type.
        /// </summary>
        private readonly Regex _messageTypeRegex;

        /// <summary>
        /// Map of XmlSerializer instances and their according types used for deserializing objects.
        /// </summary>
        private Dictionary<string, XmlSerializer> _deserializerMap = new Dictionary<string, XmlSerializer>();

        /// <summary>
        /// Map of XmlSerializer instances and their according types used for serializing objects.
        /// </summary>
        private Dictionary<string, XmlSerializer> _serializerMap = new Dictionary<string, XmlSerializer>();

        /// <summary>
        /// Flag whether to log unknown messages as errors.
        /// </summary>
        private bool _logUnknownMessageAsError = false;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlMessageParser{TMessageType}" /> class.
        /// </summary>
        /// <param name="logUnknownMessageAsError">if set to <c>true</c> .</param>
        public XmlMessageParser(bool logUnknownMessageAsError = true)
        {
            _logUnknownMessageAsError = logUnknownMessageAsError;
            _xmlWriterSettings = new XmlWriterSettings();
            _xmlWriterSettings.Encoding = new UTF8Encoding(false);
            _xmlWriterSettings.OmitXmlDeclaration = true;
            _xmlWriterSettings.CheckCharacters = false;
            _xmlSerializerNamespaces = new XmlSerializerNamespaces();
            _xmlSerializerNamespaces.Add(string.Empty, string.Empty);
            _messageTypeRegex = new Regex(MessageTypeRegex,
                                          RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Adds the specified message type to the list of supported messages.
        /// </summary>
        /// <param name="messageType">
        /// Type of message to add.
        /// </param>
        /// <param name="alternateTypeName">
        /// Alternate type name to use for this message.
        /// </param>
        public void AddMessageType(Type messageType, string alternateTypeName = null)
        {
            if (messageType == null)
            {
                throw new ArgumentException(nameof(messageType));
            }

            var isValidMessageType = false;
            var requiredBaseType = typeof(TMessageType).Name;
            var baseType = messageType.BaseType;

            while (baseType != typeof(object))
            {
                if (string.Compare(baseType.Name, requiredBaseType) == 0)
                {
                    isValidMessageType = true;
                    break;
                }

                baseType = baseType.BaseType;
            }
            
            if (isValidMessageType == false)
            {
                throw new ArgumentException("The specified message type does not fit to the required base type.");
            }

            lock (_deserializerMap)
            {
                if (string.IsNullOrEmpty(alternateTypeName))
                {
                    _deserializerMap.Add(messageType.Name, new XmlSerializer(messageType));
                }
                else
                {
                    _deserializerMap.Add(alternateTypeName, new XmlSerializer(messageType));
                }
            }

            lock (_serializerMap)
            {
                _serializerMap.Add(messageType.Name, new XmlSerializer(messageType));
            }
        }

        /// <summary>
        /// Deserializes the specified serialized message object.
        /// </summary>
        /// <param name="message">
        /// The message xml representation.
        /// </param>
        /// <returns>
        /// The deserialized message object if successful; null otherwise.
        /// </returns>
        public TMessageType Deserialize(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return null;
            }

            try
            {
                lock (_deserializerMap)
                {
                    if (message.StartsWith("<?xml"))
                    {
                        message = message.Substring(message.IndexOf('<', 1));
                    }

                    var typeMatch = _messageTypeRegex.Match(message);

                    if (typeMatch.Success == false)
                    {
                        this.Error($"Received malformed message 'message'.");
                        return null;
                    }

                    var messageType = typeMatch.Groups["type"].Value;

                    if (_deserializerMap.ContainsKey(messageType) == false)
                    {
                        if (_logUnknownMessageAsError)
                        {
                            this.Error($"Received message of type '{message}' is not supported.");
                        }
                        
                        return null;
                    }

                    using (var reader = new StringReader(message))
                    {
                        return (TMessageType)_deserializerMap[messageType].Deserialize(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Error(ex, $"Deserializing message '{message}' failed!");
            }

            return null;
        }

        /// <summary>
        /// Serializes the specified message object.
        /// </summary>
        /// <param name="message">
        /// The message object to serialize.
        /// </param>
        /// <returns>
        /// Appropriate XML message representation if successful; null otherwise.
        /// </returns>
        public string Serialize(TMessageType message)
        {
            if (message == null)
            {
                return null;
            }

            try
            {
                lock (_serializerMap)
                {
                    var messageType = message.GetType().Name;

                    if (_serializerMap.ContainsKey(messageType) == false)
                    {
                        if (_logUnknownMessageAsError)
                        {
                            this.Error($"Message '{message.GetType().Name}' is not supported.");
                        }
                        
                        return null;
                    }

                    var writer = new StringWriter();
                    using (var xw = XmlWriter.Create(writer, _xmlWriterSettings))
                    {
                        var serializer = _serializerMap[messageType];
                        serializer.Serialize(xw, message, _xmlSerializerNamespaces);
                        xw.Flush(); writer.Flush();
                        return writer.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                this.Error(ex, $"Serializing message '{message}' failed!");
            }

            return null;
        }
    }
}
