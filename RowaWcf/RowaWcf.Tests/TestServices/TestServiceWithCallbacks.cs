using Rowa.Lib.Wcf;
using System;
using System.Collections.Generic;

namespace RowaWcf.Tests.TestServices
{
    internal class TestServiceWithCallbacks : ServiceWithCallbacksBase<ITestServiceWithCallbacks, ITestServiceCallbacks>, ITestServiceWithCallbacks
    {
        public int NumDoActionCalls = 0;
        public bool LastDoActionWithParamResult = false;
        public AggregateException LastCallbackException = null;
        public int ConnectedClientCount = 0;

        public List<string> GetMetaInformation(ITestServiceCallbacks client)
        {
            return base.GetMetaInformationForClient(client);
        }

        public List<ITestServiceCallbacks> GetClients(string metaInfo)
        {
            return base.GetClientsWithMetaInformation(metaInfo);
        }

        public TestServiceWithCallbacks(ushort servicePort, string serviceLocation, BindingType bindingType) 
            : base(servicePort, serviceLocation, bindingType, 1800, true)
        {
            Start();
        }

        public TestServiceWithCallbacks()
        {
        }

        public void Initialize(ushort servicePort, string serviceLocation, BindingType bindingType)
        {
            base.Initialize(servicePort, serviceLocation, bindingType);
            Start();
        }

        protected override void OnClientConnected(ITestServiceCallbacks client)
        {
            ConnectedClientCount++;
            AssignMetaInformation(client, "Meta1");
            AssignMetaInformation(client, "Meta2");
        }

        protected override void OnClientDisconnected(ITestServiceCallbacks client)
        {
            ConnectedClientCount--;
        }

        public void DoAction()
        {
            NumDoActionCalls++;
            base.Callback(c => c.TestCallback());
            AssignMetaInformation(GetCurrentClient(), "MetaX");
        }

        public void DoActionWithParam(int param)
        {
            try
            {
                if (GetCurrentClient() != null)
                {
                    UnassignMetaInformation(GetCurrentClient(), "MetaX");
                }
                
                LastDoActionWithParamResult = base.Callback<int>(c => c.TestCallbackWithResult(param), 1);                
            }
            catch (AggregateException ex)
            {
                LastCallbackException = ex;
            }            
        }

        public void DoActionWithError()
        {
            try
            {
                base.Callback<int>(c => c.TestCallbackWithResult(int.MaxValue), 1);
            }
            catch (Exception)
            {
            }
        }

        public string GetMyEndpoint()
        {
            return base.GetCurrentClientEndpoint();
        }
    }
}
