using System.ServiceModel;

namespace Rowa.Lib.Wcf
{
    /// <summary>
    /// Base interface for all WCF services which are supporting WCF client callbacks.
    /// </summary>
    [ServiceContract]
    public interface IClientCallbackService
    {
        /// <summary>
        /// Subscribes the calling client channel for callbacks from the WCF service. 
        /// </summary>
        [OperationContract]
        void SubscribeForCallbacks();

        /// <summary>
        /// Unsubscribes the calling client channel for callbacks from the WCF service. 
        /// </summary>
        [OperationContract]
        void UnsubscribeForCallbacks();
    }
}
