using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RowaWcf.Tests.TestServices;
using Rowa.Lib.Wcf;
using System.Threading;

namespace RowaWcf.Tests
{
    internal delegate void CallBackHandler();
    internal delegate int CallBackWithResultHandler(int param);

    internal interface ICallbackHandler
    {
        event CallBackHandler OnTestCallback;
        event CallBackWithResultHandler OnTestCallbackWithResult;
    }

    internal class CallbackHandler : ServiceWithCallbacksNetTcp.ITestServiceWithCallbacksCallback, ICallbackHandler
    {
        public event CallBackHandler OnTestCallback;
        public event CallBackWithResultHandler OnTestCallbackWithResult;

        public void TestCallback()
        {
            this.OnTestCallback.Invoke();
        }

        public int TestCallbackWithResult(int param)
        {
            return this.OnTestCallbackWithResult.Invoke(param);
        }
    }

    [TestClass]
    public class TestClientWithCallbackEvents
    { 
        [TestMethod]
        public void Test_ClientWithCallbackEvents_Connect_NetTcp()
        {
            var callbackHandler = new CallbackHandler(); 

            using (var svc = new TestServiceWithCallbacks(12349, "TestSingleWithCallbacksSvc", Rowa.Lib.Wcf.BindingType.NetTcp))
            using (var client = new ClientWithCallbacks<ServiceWithCallbacksNetTcp.TestServiceWithCallbacksClient,
                                                        ServiceWithCallbacksNetTcp.ITestServiceWithCallbacks,
                                                        ServiceWithCallbacksNetTcp.ITestServiceWithCallbacksCallback>("net.tcp://localhost:12349/TestSingleWithCallbacksSvc", callbackHandler, BindingType.NetTcp))
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
        public void Test_ClientWithCallbackEvents_DoAction_NetTcp()
        {
            var callbackHandler = new CallbackHandler();
            var callbackReceived = false;

            using (var svc = new TestServiceWithCallbacks(12346, "TestSingleWithCallbacksSvc", Rowa.Lib.Wcf.BindingType.NetTcp))
            using (var client = new ClientWithCallbacks<ServiceWithCallbacksNetTcp.TestServiceWithCallbacksClient,
                                                         ServiceWithCallbacksNetTcp.ITestServiceWithCallbacks,
                                                         ServiceWithCallbacksNetTcp.ITestServiceWithCallbacksCallback>("net.tcp://localhost:12346/TestSingleWithCallbacksSvc", callbackHandler, BindingType.NetTcp))
            {
                using (var connectEvent = new ManualResetEvent(false))
                {
                    client.ConnectionEstablished += (s, e) => { connectEvent.Set(); };
                    callbackHandler.OnTestCallback += () => { callbackReceived = true; };
                    client.Start();
                    Assert.IsTrue(connectEvent.WaitOne(10000));

                    client.Service.DoAction();
                    Assert.AreEqual(1, svc.NumDoActionCalls);
                    Assert.IsTrue(callbackReceived);
                }

                client.Stop();
                svc.Stop();
            }
        }

        [TestMethod]
        public void Test_ClientWithCallbackEvents_DoActionWithParam_NetTcp()
        {
            var callbackHandler = new CallbackHandler();
            var callbackReceived = false;

            using (var svc = new TestServiceWithCallbacks(12347, "TestSingleWithCallbacksSvc", Rowa.Lib.Wcf.BindingType.NetTcp))
            using (var client = new ClientWithCallbacks<ServiceWithCallbacksNetTcp.TestServiceWithCallbacksClient,
                                                        ServiceWithCallbacksNetTcp.ITestServiceWithCallbacks,
                                                        ServiceWithCallbacksNetTcp.ITestServiceWithCallbacksCallback>("net.tcp://localhost:12347/TestSingleWithCallbacksSvc", callbackHandler, BindingType.NetTcp))
            {
                using (var connectEvent = new ManualResetEvent(false))
                {
                    client.ConnectionEstablished += (s, e) => { connectEvent.Set(); };
                    callbackHandler.OnTestCallbackWithResult += (p) => { callbackReceived = true; return 1; };
                    client.Start();
                    Assert.IsTrue(connectEvent.WaitOne(10000));

                    svc.DoActionWithParam(1);

                    Assert.IsTrue(svc.LastDoActionWithParamResult);
                    Assert.IsTrue(callbackReceived);
                }

                client.Stop();
                svc.Stop();
            }
        }

        [TestMethod]
        public void Test_ClientWithCallbackEvents_Disconnect_NetTcp()
        {
            var callbackHandler = new CallbackHandler();

            using (var svc = new TestServiceWithCallbacks(12348, "TestSingleWithCallbacksSvc", Rowa.Lib.Wcf.BindingType.NetTcp))
            using (var client = new ClientWithCallbacks<ServiceWithCallbacksNetTcp.TestServiceWithCallbacksClient,
                                                        ServiceWithCallbacksNetTcp.ITestServiceWithCallbacks,
                                                        ServiceWithCallbacksNetTcp.ITestServiceWithCallbacksCallback>("net.tcp://localhost:12348/TestSingleWithCallbacksSvc", callbackHandler, BindingType.NetTcp))
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
    }
}
