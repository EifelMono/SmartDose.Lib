using Rowa.Lib.Wcf;

namespace RowaWcf.Tests.TestServices
{
    internal class TestService : ServiceBase<ITestService>, ITestService
    {
        public int NumDoActionCalls = 0;
        
        public TestService(ushort servicePort, string serviceLocation, BindingType bindingType) 
            : base(servicePort, serviceLocation, bindingType)
        {
            Start();
        }

        public TestService()
        {
        }

        public void Initialize(ushort servicePort, string serviceLocation, BindingType bindingType)
        {
            base.Initialize(servicePort, serviceLocation, bindingType);
            Start();
        }

        public void DoAction()
        {
            NumDoActionCalls++;
        }

        public string GetMyEndpoint()
        {
            return base.GetCurrentClientEndpoint();
        }
    }
}
