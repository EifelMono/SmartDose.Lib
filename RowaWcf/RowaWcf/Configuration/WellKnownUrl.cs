using System;

namespace Rowa.Lib.Wcf.Configuration
{
    /// <summary>
    /// Class which provides the well known WCF urls for different products.
    /// </summary>
    public static class WellKnownUrl
    {
        #region Constants

        private const string SDHardwareServiceUrl = "net.tcp://{0}:9001/HardwareService";
        private const string SDAuthenticationServiceUrl = "net.tcp://{0}:9012/AuthorizationClient";
        private const string SDCanisterHandlingServiceUrl = "net.tcp://{0}:9004/CanisterHandling";
        private const string SDInventoryServiceUrl = "net.tcp://{0}:9006/Inventory";
        private const string SDMasterDataServiceUrl = "net.tcp://{0}:9002/MasterData";
        private const string SDProductionServiceUrl = "net.tcp://{0}:9008/Production";
        private const string SDReportingServiceUrl = "net.tcp://{0}:9005/Reporting";
        private const string SDSdmcServiceUrl = "net.tcp://{0}:9000/SDMC";
        private const string SDSettingsServiceUrl = "net.tcp://{0}:9009/Settings";
        private const string SDTrayHandlingServiceUrl = "net.tcp://{0}:9003/TrayHandling";
        private const string SDVialFillingServiceUrl = "net.tcp://{0}:9010/VialFilling";
        private const string SDSystemManagementServiceUrl = "net.tcp://{0}:9011/SystemManagement";
        private const string SDDeblisteringServiceUrl = "net.tcp://{0}:9014/Deblistering";
        private const string SDPouchDesignHandlingServiceUrl = "net.tcp://{0}:9013/PouchDesignHandling";
        private const string SDHardwareHandlingServiceUrl = "net.tcp://{0}:9016/HardwareHandling";
        private const string SDDualInspectionServiceUrl = "net.tcp://{0}:9015/DualInspection";
        private const string SDPouchSequenceRuleHandlingServiceUrl = "net.tcp://{0}:9017/PouchSequenceRuleHandling";

        #endregion


        /// <summary>
        /// Gets the well known url of the specified type for a service that is running locally.
        /// </summary>
        /// <param name="type">The url type to get.</param>
        /// <returns>
        /// Requested well known url.
        /// </returns>
        public static string Get(WellKnownUrlType type)
        {
            return Get(type, "127.0.0.1");
        }

        /// <summary>
        /// Gets the well known url of the specified type for the specified service address.
        /// </summary>
        /// <param name="type">The url type to get.</param>
        /// <param name="serviceAddress">The service address to use when building the url.</param>
        /// <returns>
        /// Requested well known url.
        /// </returns>
        public static string Get(WellKnownUrlType type, string serviceAddress)
        {
            if (string.IsNullOrEmpty(serviceAddress))
                throw new ArgumentNullException(nameof(serviceAddress));

            switch (type)
            {
                case WellKnownUrlType.SDHardware: return string.Format(SDHardwareServiceUrl, serviceAddress);
                case WellKnownUrlType.SDAuthentication: return string.Format(SDAuthenticationServiceUrl, serviceAddress);
                case WellKnownUrlType.SDCanisterHandling: return string.Format(SDCanisterHandlingServiceUrl, serviceAddress);
                case WellKnownUrlType.SDInventory: return string.Format(SDInventoryServiceUrl, serviceAddress);
                case WellKnownUrlType.SDMasterData: return string.Format(SDMasterDataServiceUrl, serviceAddress);
                case WellKnownUrlType.SDProduction: return string.Format(SDProductionServiceUrl, serviceAddress);
                case WellKnownUrlType.SDReporting: return string.Format(SDReportingServiceUrl, serviceAddress);
                case WellKnownUrlType.SDSdmc: return string.Format(SDSdmcServiceUrl, serviceAddress);
                case WellKnownUrlType.SDSettings: return string.Format(SDSettingsServiceUrl, serviceAddress);
                case WellKnownUrlType.SDTrayHandling: return string.Format(SDTrayHandlingServiceUrl, serviceAddress);
                case WellKnownUrlType.SDVialFilling: return string.Format(SDVialFillingServiceUrl, serviceAddress);
                case WellKnownUrlType.SDSystemManagement: return string.Format(SDSystemManagementServiceUrl, serviceAddress);
                case WellKnownUrlType.SDDeblistering: return string.Format(SDDeblisteringServiceUrl, serviceAddress);
                case WellKnownUrlType.SDPouchDesignHandling: return string.Format(SDPouchDesignHandlingServiceUrl, serviceAddress);
                case WellKnownUrlType.SDHardwareHandling: return string.Format(SDHardwareHandlingServiceUrl, serviceAddress);
                case WellKnownUrlType.SDDualInspection: return string.Format(SDDualInspectionServiceUrl, serviceAddress);
                case WellKnownUrlType.SDPouchSequenceRuleHandling: return string.Format(SDPouchSequenceRuleHandlingServiceUrl, serviceAddress);
            }

            return string.Empty;
        }
    }
}
