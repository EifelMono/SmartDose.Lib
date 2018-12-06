using System;

namespace Rowa.Lib.Wcf
{
    /// <summary>
    /// Class which provides the logic to build the complete URI of a WCF service endpoint.
    /// </summary>
    internal static class UriBuilder
    {
        /// <summary>
        /// Builds the WCF service endpoint URI based on the specified settings.
        /// </summary>
        /// <param name="servicePort">The TCP port to use for the WCF service endpoint.</param>
        /// <param name="serviceLocation">The location to use as appendix for the url to identify the WCF service endpoint.</param>
        /// <param name="bindingType">The WCF binding type to use for the service endpoint.</param>
        /// <returns>
        /// The built URI based on the specified settings.
        /// </returns>
        public static Uri Build(ushort servicePort, string serviceLocation, BindingType bindingType)
        {
            if (servicePort == 0)
            {
                throw new ArgumentException(nameof(servicePort));
            }

            if (string.IsNullOrEmpty(serviceLocation))
            {
                throw new ArgumentException(nameof(serviceLocation));
            }

            var uriScheme = Uri.UriSchemeNetTcp;

            switch (bindingType)
            {
                case BindingType.BasicHttp: uriScheme = Uri.UriSchemeHttp; break;
                case BindingType.NetTcp: uriScheme = Uri.UriSchemeNetTcp; break;
            }

            return new Uri($"{uriScheme}://{Environment.MachineName}:{servicePort}/{serviceLocation}/");
        }
    }
}
