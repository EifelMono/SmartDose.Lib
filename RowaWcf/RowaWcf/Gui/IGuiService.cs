using System.ServiceModel;

namespace Rowa.Lib.Wcf.Gui
{
    /// <summary>
    /// WCF service interface which is used for a message based
    /// communication between a WCF service and a standard Rowa UI.
    /// This interface name is used in combination with Mosaic.
    /// </summary>
    [ServiceContract]
    public interface IGuiService
    {
        /// <summary>
        /// Is called to get notification from the service side.
        /// This method will block until a notification is available to be delivered 
        /// to the client or a timeout occured.
        /// </summary>
        /// <param name="message">
        /// Optional serialized filter message for the notifications to retrieve.
        /// </param>
        /// <returns>
        /// Notification related object if successful; null otherwise.
        /// </returns>
        [OperationContract]
        string GetMessage(string filter);

        /// <summary>
        /// Is called to request data or actions from the service.
        /// </summary>
        /// <param name="message">
        /// Holds the serialized request message.
        /// </param>
        /// <returns>
        /// Result of the specified request.
        /// </returns>
        [OperationContract]
        string ProcessMessage(string message);
    }
}
