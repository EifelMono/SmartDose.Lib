using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rowa.Lib.Wcf;
using RowaWcf.Tests.TestServices;
using System.Threading;

namespace RowaWcf.Tests
{
    [TestClass]
    public class TestClient
    {
        [TestMethod]
        public void Test_Client_Connect_NetTcp()
        {
            using (var svc = new TestService(12340, "TestSingleSvc", Rowa.Lib.Wcf.BindingType.NetTcp))
            using (var client = new Client<ServiceNetTcp.TestServiceClient, ServiceNetTcp.ITestService>("net.tcp://localhost:12340/TestSingleSvc", BindingType.NetTcp))
            {
                using (var connectEvent = new ManualResetEvent(false))
                {
                    client.ConnectionEstablished += (s, e) => { connectEvent.Set(); };
                    client.Start();
                    Assert.IsTrue(connectEvent.WaitOne(10000));
                }
            }
        }

        [TestMethod]
        public void Test_Client_Connect_Delayed_NetTcp()
        {
            using (var client = new Client<ServiceNetTcp.TestServiceClient, ServiceNetTcp.ITestService>("net.tcp://localhost:12340/TestSingleSvc", BindingType.NetTcp))
            {
                using (var connectEvent = new ManualResetEvent(false))
                {
                    client.ConnectionEstablished += (s, e) => { connectEvent.Set(); };
                    client.Start();

                    Assert.IsFalse(connectEvent.WaitOne(10000));

                    using (var svc = new TestService(12340, "TestSingleSvc", Rowa.Lib.Wcf.BindingType.NetTcp))
                    {
                        Assert.IsTrue(connectEvent.WaitOne(10000));
                    }
                }
            }            
        }

        [TestMethod]
        public void Test_Client_Connect_Cancellation_NetTcp()
        {
            using (var client = new Client<ServiceNetTcp.TestServiceClient, ServiceNetTcp.ITestService>("net.tcp://localhost:12340/TestSingleSvc", BindingType.NetTcp))
            {
                using (var connectEvent = new ManualResetEvent(false))
                {
                    client.ConnectionEstablished += (s, e) => { connectEvent.Set(); };
                    client.Start();

                    System.Threading.Thread.Sleep(1000);

                    var start = System.Environment.TickCount;
                    client.Stop();
                    Assert.IsTrue((System.Environment.TickCount - start) <= 1000);
                }
            }
        }

        [TestMethod]
        public void Test_Client_Connect_BasicHttp()
        {
            using (var svc = new TestService(8080, "TestSingleSvc", Rowa.Lib.Wcf.BindingType.BasicHttp))
            using (var client = new Client<ServiceNetTcp.TestServiceClient, ServiceNetTcp.ITestService>("http://localhost:8080/TestSingleSvc", BindingType.BasicHttp))
            {
                using (var connectEvent = new ManualResetEvent(false))
                {
                    client.ConnectionEstablished += (s, e) => { connectEvent.Set(); };
                    client.Start();
                    Assert.IsTrue(connectEvent.WaitOne(10000));
                }
            }
        }

        [TestMethod]
        public void Test_Client_DoAction_NetTcp()
        {
            using (var svc = new TestService(12341, "TestSingleSvc", Rowa.Lib.Wcf.BindingType.NetTcp))
            using (var client = new Client<ServiceNetTcp.TestServiceClient, ServiceNetTcp.ITestService>("net.tcp://localhost:12341/TestSingleSvc", BindingType.NetTcp))
            {
                using (var connectEvent = new ManualResetEvent(false))
                {
                    client.ConnectionEstablished += (s, e) => { connectEvent.Set(); };
                    client.Start();
                    Assert.IsTrue(connectEvent.WaitOne(10000));

                    client.Service.DoAction();
                    Assert.AreEqual(1, svc.NumDoActionCalls);
                }
            }
        }

        [TestMethod]
        public void Test_Client_DoAction_BasicHttp()
        {
            using (var svc = new TestService(8080, "TestSingleSvc", Rowa.Lib.Wcf.BindingType.BasicHttp))
            using (var client = new Client<ServiceNetTcp.TestServiceClient, ServiceNetTcp.ITestService>("http://localhost:8080/TestSingleSvc", BindingType.BasicHttp))
            {
                using (var connectEvent = new ManualResetEvent(false))
                {
                    client.ConnectionEstablished += (s, e) => { connectEvent.Set(); };
                    client.Start();
                    Assert.IsTrue(connectEvent.WaitOne(10000));

                    client.Service.DoAction();
                    Assert.AreEqual(1, svc.NumDoActionCalls);
                }
            }
        }

        
        [TestMethod]
        public void Test_Client_Disconnect_NetTcp()
        {
            using (var svc = new TestService(12342, "TestSingleSvc", Rowa.Lib.Wcf.BindingType.NetTcp))
            using (var client = new Client<ServiceNetTcp.TestServiceClient, ServiceNetTcp.ITestService>("net.tcp://localhost:12342/TestSingleSvc", BindingType.NetTcp))
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
            }
        }

        [Ignore]
        [TestMethod]
        public void Test_Client_Disconnect_BasicHttp()
        {
            using (var svc = new TestService(9080, "TestSingleSvc", Rowa.Lib.Wcf.BindingType.BasicHttp))
            using (var client = new Client<ServiceHttp.TestServiceClient, ServiceHttp.ITestService>("http://localhost:9080/TestSingleSvc", BindingType.BasicHttp))
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

                    Assert.IsTrue(disconnectEvent.WaitOne(20000));
                }
            }
        }
    }
}
