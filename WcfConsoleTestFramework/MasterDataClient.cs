using Rowa.Lib.Wcf;
using WcfConsoleTestFramework.ServiceReference1;

namespace WcfConsoleTestFramework
{
    class MasterDataClient :
        ClientWithCallbackEvents<MasterDataServiceClient, IMasterDataService, IMasterDataServiceCallback, IMasterDataCallbackEvents>
    {
        public MasterDataClient(string serviceUrl, 
            IMasterDataCallbackEvents clientCallbackInstance, 
            BindingType bindingType = BindingType.NetTcp, 
            uint communicationTimeoutSeconds = 1800, 
            uint connectionCheckIntervalSeconds = 5) : 
            base(serviceUrl, clientCallbackInstance, bindingType, communicationTimeoutSeconds, connectionCheckIntervalSeconds)
        { }
    }
}
