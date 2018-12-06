using Rowa.Lib.Wcf;
using System.ServiceModel;

namespace RowaWcf.Tests.TestServices
{
    [ServiceContract(CallbackContract = typeof(ITestServiceCallbacks))]
    interface ITestServiceWithCallbacks : IClientCallbackService
    {
        [OperationContract]
        void DoAction();

        [OperationContract]
        void DoActionWithParam(int param);

        [OperationContract]
        string GetMyEndpoint();
    }
}
