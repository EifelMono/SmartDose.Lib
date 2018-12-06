using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Xml;

namespace Rowa.Lib.Wcf
{
    /// <summary>
    /// Class which provides the logic to build the binding of a WCF service endpoint.
    /// </summary>
    internal static class BindingBuilder
    {
        /// <summary>
        /// Builds the WCF service binding based on the specified settings.
        /// </summary>
        /// <param name="bindingType">The WCF binding type to use for the service endpoint.</param>
        /// <param name="communicationTimeoutSeconds">The communication timeout to define in seconds.</param>
        /// <returns>
        /// The built binding based on the specified settings.
        /// </returns>
        public static Binding Build(BindingType bindingType, uint communicationTimeoutSeconds)
        {
            if (communicationTimeoutSeconds == 0)
            {
                throw new ArgumentException(nameof(communicationTimeoutSeconds));
            }

            if (bindingType == BindingType.BasicHttp)
            {
                var binding = new BasicHttpBinding(BasicHttpSecurityMode.None);

                binding.ReaderQuotas = new XmlDictionaryReaderQuotas()
                {
                    MaxDepth = int.MaxValue,
                    MaxStringContentLength = int.MaxValue,
                    MaxArrayLength = int.MaxValue,
                    MaxBytesPerRead = int.MaxValue,
                    MaxNameTableCharCount = int.MaxValue
                };

                binding.MaxReceivedMessageSize = int.MaxValue;
                binding.ReceiveTimeout = TimeSpan.MaxValue;
                binding.SendTimeout = new System.TimeSpan(0, 0, (int)communicationTimeoutSeconds);
                binding.BypassProxyOnLocal = true;

                return binding;
            }
            else
            {
                var binding = new NetTcpBinding(SecurityMode.None);

                binding.ReaderQuotas = new XmlDictionaryReaderQuotas()
                {
                    MaxDepth = int.MaxValue,
                    MaxStringContentLength = int.MaxValue,
                    MaxArrayLength = int.MaxValue,
                    MaxBytesPerRead = int.MaxValue,
                    MaxNameTableCharCount = int.MaxValue
                };

                binding.MaxReceivedMessageSize = int.MaxValue;
                binding.ReceiveTimeout = TimeSpan.MaxValue;
                binding.SendTimeout = new System.TimeSpan(0, 0, (int)communicationTimeoutSeconds);
            
                return binding;
            }            
        }

        /// <summary>
        /// Builds the WCF metadata mex binding based on the specified settings.
        /// </summary>
        /// <param name="bindingType">The WCF binding type to use for the metadata endpoint.</param>
        /// <returns>
        /// The built binding based on the specified settings.
        /// </returns>
        public static Binding BuildMex(BindingType bindingType)
        {
            if (bindingType == BindingType.BasicHttp)
            {
                return MetadataExchangeBindings.CreateMexHttpBinding();
            }
            else
            {
                return MetadataExchangeBindings.CreateMexTcpBinding();
            }     
        }
    }
}
