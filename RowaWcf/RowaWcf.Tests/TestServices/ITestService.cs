using System.ServiceModel;

namespace RowaWcf.Tests.TestServices
{
    [ServiceContract]
    internal interface ITestService
    {
        [OperationContract]
        void DoAction();

        [OperationContract]
        string GetMyEndpoint();
    }
}
