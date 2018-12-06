using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RowaWcf.Tests.TestServices;
using System.Net;
using System.ServiceModel;

namespace RowaWcf.Tests
{
    [TestClass]
    [CallbackBehavior(IncludeExceptionDetailInFaults = true, UseSynchronizationContext = false )]
    public class TestServiceWithCallbacksBase : ServiceWithCallbacksNetTcp.ITestServiceWithCallbacksCallback
    {
        private bool _callbackReceived = false;
        private bool _raiseFaultException = false;

        [TestMethod]
        public void Test_ServiceWithCallbacks_NetTcp()
        {

            using (var svc = new TestServiceWithCallbacks(12345, "TestSingleWithCallbacksSvc", Rowa.Lib.Wcf.BindingType.NetTcp))
            using (var client = new ServiceWithCallbacksNetTcp.TestServiceWithCallbacksClient(new System.ServiceModel.InstanceContext(this)))
            {
                client.Open();
                client.SubscribeForCallbacks();
                client.DoAction();
                
                Assert.AreEqual(1, svc.NumDoActionCalls);
                Assert.AreEqual(true, _callbackReceived);

                _callbackReceived = false;
                client.DoActionWithParam(1);

                Assert.AreEqual(true, _callbackReceived);
                Assert.AreEqual(true, svc.LastDoActionWithParamResult);

                _callbackReceived = false;
                client.DoActionWithParam(0);

                Assert.AreEqual(true, _callbackReceived);
                Assert.AreEqual(false, svc.LastDoActionWithParamResult);

                client.UnsubscribeForCallbacks();
            }
        }

        [TestMethod]
        public void Test_ServiceWithCallbacks_Exception_NetTcp()
        {

            using (var svc = new TestServiceWithCallbacks(12345, "TestSingleWithCallbacksSvc", Rowa.Lib.Wcf.BindingType.NetTcp))
            using (var client = new ServiceWithCallbacksNetTcp.TestServiceWithCallbacksClient(new System.ServiceModel.InstanceContext(this)))
            {
                client.Open();
                client.SubscribeForCallbacks();

                Assert.AreEqual(CommunicationState.Opened, client.State);
                
                svc.DoActionWithError();

                Assert.AreNotEqual(CommunicationState.Opened, client.State);
            }
        }

        [TestMethod]
        public void Test_ServiceWithCallbacks_GetClientEndpoint_NetTcp()
        {
            using (var svc = new TestServiceWithCallbacks(12345, "TestSingleWithCallbacksSvc", Rowa.Lib.Wcf.BindingType.NetTcp))
            using (var client = new ServiceWithCallbacksNetTcp.TestServiceWithCallbacksClient(new System.ServiceModel.InstanceContext(this)))
            {
                client.Open();
                var endpoint = client.GetMyEndpoint();
                Assert.IsTrue(endpoint.Contains(IPAddress.IPv6Loopback.ToString()) || endpoint.Contains(IPAddress.Loopback.ToString()));
            }
        }

        [TestMethod]
        public void Test_ServiceWithCallbacks_NetTcp_Initialize()
        {
            using (var svc = new TestServiceWithCallbacks())
            {
                svc.Initialize(12345, "TestSingleWithCallbacksSvc", Rowa.Lib.Wcf.BindingType.NetTcp);

                using (var client = new ServiceWithCallbacksNetTcp.TestServiceWithCallbacksClient(new System.ServiceModel.InstanceContext(this)))
                {
                    client.Open();
                    client.SubscribeForCallbacks();
                    client.DoAction();

                    Assert.AreEqual(1, svc.NumDoActionCalls);
                    Assert.AreEqual(true, _callbackReceived);

                    _callbackReceived = false;
                    client.DoActionWithParam(1);

                    Assert.AreEqual(true, _callbackReceived);
                    Assert.AreEqual(true, svc.LastDoActionWithParamResult);

                    _callbackReceived = false;
                    client.DoActionWithParam(0);

                    Assert.AreEqual(true, _callbackReceived);
                    Assert.AreEqual(false, svc.LastDoActionWithParamResult);

                    client.UnsubscribeForCallbacks();
                }
            }                
        }

        [TestMethod]
        public void Test_ServiceWithCallbacks_NetTcp_CallbackFaults()
        {
            _raiseFaultException = true;

            using (var svc = new TestServiceWithCallbacks())
            {
                svc.Initialize(12345, "TestSingleWithCallbacksSvc", Rowa.Lib.Wcf.BindingType.NetTcp);

                using (var client = new ServiceWithCallbacksNetTcp.TestServiceWithCallbacksClient(new System.ServiceModel.InstanceContext(this)))
                {
                    client.Open();
                    client.SubscribeForCallbacks();

                    client.DoActionWithParam(1);

                    Assert.AreEqual(true, _callbackReceived);
                    Assert.AreEqual(1, svc.ConnectedClientCount);
                    Assert.IsNotNull(svc.LastCallbackException);
                    Assert.AreEqual(1, svc.LastCallbackException.InnerExceptions.Count);
                    Assert.IsTrue(svc.LastCallbackException.InnerExceptions[0] is FaultException<ArgumentFault>);
                   

                    client.UnsubscribeForCallbacks();
                }

                // test with multiple clients
                using (var client1 = new ServiceWithCallbacksNetTcp.TestServiceWithCallbacksClient(new System.ServiceModel.InstanceContext(this)))
                using (var client2 = new ServiceWithCallbacksNetTcp.TestServiceWithCallbacksClient(new System.ServiceModel.InstanceContext(this)))
                using (var client3 = new ServiceWithCallbacksNetTcp.TestServiceWithCallbacksClient(new System.ServiceModel.InstanceContext(this)))
                {
                    client1.Open();
                    client2.Open();
                    client3.Open();
                    client1.SubscribeForCallbacks();
                    client2.SubscribeForCallbacks();
                    client3.SubscribeForCallbacks();

                    client1.DoActionWithParam(1);

                    Assert.AreEqual(3, svc.ConnectedClientCount);
                    Assert.IsNotNull(svc.LastCallbackException);
                    Assert.AreEqual(3, svc.LastCallbackException.InnerExceptions.Count);
                    Assert.IsTrue(svc.LastCallbackException.InnerExceptions[0] is FaultException<ArgumentFault>);
                    Assert.IsTrue(svc.LastCallbackException.InnerExceptions[1] is FaultException<ArgumentFault>);
                    Assert.IsTrue(svc.LastCallbackException.InnerExceptions[2] is FaultException<ArgumentFault>);


                    client1.UnsubscribeForCallbacks();
                    client2.UnsubscribeForCallbacks();
                    client3.UnsubscribeForCallbacks();
                }

            }
        }

        [TestMethod]
        public void Test_ServiceWithCallbacks_NetTcp_MetaInformation()
        {
            using (var svc = new TestServiceWithCallbacks())
            {
                svc.Initialize(12345, "TestSingleWithCallbacksSvc", Rowa.Lib.Wcf.BindingType.NetTcp);

                using (var client = new ServiceWithCallbacksNetTcp.TestServiceWithCallbacksClient(new System.ServiceModel.InstanceContext(this)))
                {
                    client.Open();
                    client.SubscribeForCallbacks();

                    Assert.AreEqual(1, svc.GetClients("Meta1").Count);
                    Assert.AreEqual(2, svc.GetMetaInformation(svc.GetClients("Meta1")[0]).Count);


                    client.DoAction();
                    Assert.AreEqual(3, svc.GetMetaInformation(svc.GetClients("Meta1")[0]).Count);
                    Assert.AreEqual(1, svc.GetClients("MetaX").Count);

                    var metaInfo = svc.GetMetaInformation(svc.GetClients("Meta1")[0]);

                    Assert.AreEqual("Meta1", metaInfo[0]);
                    Assert.AreEqual("Meta2", metaInfo[1]);
                    Assert.AreEqual("MetaX", metaInfo[2]);


                    client.DoActionWithParam(1);
                    Assert.AreEqual(2, svc.GetMetaInformation(svc.GetClients("Meta1")[0]).Count);
                    Assert.AreEqual(0, svc.GetClients("MetaX").Count);

                    client.UnsubscribeForCallbacks();

                    Assert.AreEqual(0, svc.GetClients("Meta1").Count);
                    Assert.AreEqual(0, svc.GetClients("Meta2").Count);
                    Assert.AreEqual(0, svc.GetClients("MetaX").Count);

                }

                // test with multiple clients
                using (var client1 = new ServiceWithCallbacksNetTcp.TestServiceWithCallbacksClient(new System.ServiceModel.InstanceContext(this)))
                using (var client2 = new ServiceWithCallbacksNetTcp.TestServiceWithCallbacksClient(new System.ServiceModel.InstanceContext(this)))
                using (var client3 = new ServiceWithCallbacksNetTcp.TestServiceWithCallbacksClient(new System.ServiceModel.InstanceContext(this)))
                {
                    client1.Open();
                    client2.Open();
                    client3.Open();
                    client1.SubscribeForCallbacks();
                    client2.SubscribeForCallbacks();
                    client3.SubscribeForCallbacks();

                    Assert.AreEqual(3, svc.GetClients("Meta1").Count);
                    Assert.AreEqual(2, svc.GetMetaInformation(svc.GetClients("Meta1")[0]).Count);

                    client1.DoAction();

                    Assert.AreEqual(3, svc.GetMetaInformation(svc.GetClients("Meta1")[0]).Count);
                    Assert.AreEqual(1, svc.GetClients("MetaX").Count);

                    client1.DoActionWithParam(1);
                    Assert.AreEqual(2, svc.GetMetaInformation(svc.GetClients("Meta1")[0]).Count);
                    Assert.AreEqual(0, svc.GetClients("MetaX").Count);

                    client1.UnsubscribeForCallbacks();
                    client2.UnsubscribeForCallbacks();
                    client3.UnsubscribeForCallbacks();

                    Assert.AreEqual(0, svc.GetClients("Meta1").Count);
                    Assert.AreEqual(0, svc.GetClients("Meta2").Count);
                    Assert.AreEqual(0, svc.GetClients("MetaX").Count);
                }

            }
        }

        public void TestCallback()
        {
            _callbackReceived = true;
            
        }

        public int TestCallbackWithResult(int param)
        {
            if (param == int.MaxValue)
            {
                throw new Exception("Kaputt");
            }

            _callbackReceived = true;

            if (_raiseFaultException)
            {
                throw new FaultException<ArgumentFault>(new ArgumentFault("param"));
            }

            return param;
        }
    }
}
