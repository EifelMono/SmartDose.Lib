using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rowa.Lib.Wcf;
using RowaWcf.Tests.TestServices;
using System.Threading;

namespace RowaWcf.Tests
{
    [TestClass]
    public class TestClientWithCallbacks : ServiceWithCallbacksNetTcp.ITestServiceWithCallbacksCallback
    {
        public bool _callbackReceived = false;

        [TestMethod]
        public void Test_ClientWithCallbacks_Connect_NetTcp()
        {
            using (var svc = new TestServiceWithCallbacks(12349, "TestSingleWithCallbacksSvc", Rowa.Lib.Wcf.BindingType.NetTcp))
            using (var client = new ClientWithCallbacks<ServiceWithCallbacksNetTcp.TestServiceWithCallbacksClient,
                                                        ServiceWithCallbacksNetTcp.ITestServiceWithCallbacks, 
                                                        ServiceWithCallbacksNetTcp.ITestServiceWithCallbacksCallback>("net.tcp://localhost:12349/TestSingleWithCallbacksSvc", this, BindingType.NetTcp))
            {
                using (var connectEvent = new ManualResetEvent(false))
                {
                    client.ConnectionEstablished += (s, e) => { connectEvent.Set(); };
                    client.Start();
                    Assert.IsTrue(connectEvent.WaitOne(10000));
                }

                client.Stop();
                svc.Stop();
            }
        }

        [TestMethod]
        public void Test_ClientWithCallbacks_DoAction_NetTcp()
        {
            using (var svc = new TestServiceWithCallbacks(12346, "TestSingleWithCallbacksSvc", Rowa.Lib.Wcf.BindingType.NetTcp))
            using (var client = new ClientWithCallbacks<ServiceWithCallbacksNetTcp.TestServiceWithCallbacksClient,
                                                         ServiceWithCallbacksNetTcp.ITestServiceWithCallbacks,
                                                         ServiceWithCallbacksNetTcp.ITestServiceWithCallbacksCallback>("net.tcp://localhost:12346/TestSingleWithCallbacksSvc", this, BindingType.NetTcp))
            {
                using (var connectEvent = new ManualResetEvent(false))
                {
                    client.ConnectionEstablished += (s, e) => { connectEvent.Set(); };
                    client.Start();
                    Assert.IsTrue(connectEvent.WaitOne(10000));

                    client.Service.DoAction();
                    Assert.AreEqual(1, svc.NumDoActionCalls);
                    Assert.IsTrue(_callbackReceived);
                }

                client.Stop();
                svc.Stop();
            }
        }

        [TestMethod]
        public void Test_ClientWithCallbacks_DoActionWithParam_NetTcp()
        {
            using (var svc = new TestServiceWithCallbacks(12347, "TestSingleWithCallbacksSvc", Rowa.Lib.Wcf.BindingType.NetTcp))
            using (var client = new ClientWithCallbacks<ServiceWithCallbacksNetTcp.TestServiceWithCallbacksClient,
                                                        ServiceWithCallbacksNetTcp.ITestServiceWithCallbacks,
                                                        ServiceWithCallbacksNetTcp.ITestServiceWithCallbacksCallback>("net.tcp://localhost:12347/TestSingleWithCallbacksSvc", this, BindingType.NetTcp))
            {
                using (var connectEvent = new ManualResetEvent(false))
                {
                    client.ConnectionEstablished += (s, e) => { connectEvent.Set(); };
                    client.Start();
                    Assert.IsTrue(connectEvent.WaitOne(10000));

                    client.Service.DoActionWithParam(1);
                    Assert.IsTrue(_callbackReceived);
                }

                client.Stop();
                svc.Stop();
            }
        }

        [TestMethod]
        public void Test_ClientWithCallbacks_DoActionWithError_NetTcp()
        {
            using (var svc = new TestServiceWithCallbacks(12347, "TestSingleWithCallbacksSvc", Rowa.Lib.Wcf.BindingType.NetTcp))
            using (var client = new ClientWithCallbacks<ServiceWithCallbacksNetTcp.TestServiceWithCallbacksClient,
                                                        ServiceWithCallbacksNetTcp.ITestServiceWithCallbacks,
                                                        ServiceWithCallbacksNetTcp.ITestServiceWithCallbacksCallback>("net.tcp://localhost:12347/TestSingleWithCallbacksSvc", this, BindingType.NetTcp))
            {
                using (var connectEvent = new ManualResetEvent(false))
                {
                    client.ConnectionEstablished += (s, e) => { connectEvent.Set(); };
                    client.Start();
                    Assert.IsTrue(connectEvent.WaitOne(10000));
                    connectEvent.Reset();

                    svc.DoActionWithError();
                    Assert.IsFalse(client.IsConnected);
                    Assert.IsTrue(connectEvent.WaitOne(10000));
                }

                client.Stop();
                svc.Stop();
            }
        }

        [TestMethod]
        public void Test_ClientWithCallbacks_Disconnect_NetTcp()
        {
            using (var svc = new TestServiceWithCallbacks(12348, "TestSingleWithCallbacksSvc", Rowa.Lib.Wcf.BindingType.NetTcp))
            using (var client = new ClientWithCallbacks<ServiceWithCallbacksNetTcp.TestServiceWithCallbacksClient,
                                                        ServiceWithCallbacksNetTcp.ITestServiceWithCallbacks,
                                                        ServiceWithCallbacksNetTcp.ITestServiceWithCallbacksCallback>("net.tcp://localhost:12348/TestSingleWithCallbacksSvc", this, BindingType.NetTcp))
            {
                using (var connectEvent = new ManualResetEvent(false))
                using (var disconnectEvent = new ManualResetEvent(false))
                {
                    client.ConnectionEstablished += (s, e) => { connectEvent.Set(); };
                    client.ConnectionClosed += (s, e) => { disconnectEvent.Set(); };

                    client.Start();
                    Assert.IsTrue(connectEvent.WaitOne(10000));

                    svc.Stop();

                    try
                    {
                        client.Service.DoAction();
                    }
                    catch (System.Exception)
                    {
                    }

                    Assert.IsTrue(disconnectEvent.WaitOne(10000));
                }

                client.Stop();
                svc.Stop();
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
            return param;
        }
    }
}
