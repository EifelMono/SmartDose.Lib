using System.ServiceModel;

namespace RowaWcf.Tests.TestServices
{
    interface ITestServiceCallbacks
    {
        [OperationContract(IsOneWay = true)]
        void TestCallback();

        [OperationContract]
        [FaultContract(typeof(ArgumentFault))]
        int TestCallbackWithResult(int param);
    }
}
