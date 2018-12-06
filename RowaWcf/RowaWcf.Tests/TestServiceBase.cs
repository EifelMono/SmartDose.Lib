using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RowaWcf.Tests.TestServices;
using System.Net;

namespace RowaWcf.Tests
{
    [TestClass]
    public class TestServiceBase
    {
        [TestMethod]
        public void Test_Service_NetTcp()
        {
            using (var svc = new TestService(12345, "TestSingleSvc", Rowa.Lib.Wcf.BindingType.NetTcp))
            {
                using (var client = new ServiceNetTcp.TestServiceClient())
                {
                    client.Open();
                    client.DoAction();
                    Assert.AreEqual(1, svc.NumDoActionCalls);
                }

                svc.Stop();
            }            
        }

        [TestMethod]
        public void Test_Service_NetTcp_Initialize()
        {
            using (var svc = new TestService())
            {
                svc.Initialize(12345, "TestSingleSvc", Rowa.Lib.Wcf.BindingType.NetTcp);

                using (var client = new ServiceNetTcp.TestServiceClient())
                {
                    client.Open();
                    client.DoAction();
                    Assert.AreEqual(1, svc.NumDoActionCalls);
                }

                svc.Stop();
            }
        }

        [TestMethod]
        public void Test_Service_BasicHttp()
        {
            using (var svc = new TestService(8080, "TestSingleSvc", Rowa.Lib.Wcf.BindingType.BasicHttp))
            {
                using (var client = new ServiceHttp.TestServiceClient())
                {
                    client.Open();
                    client.DoAction();
                    Assert.AreEqual(1, svc.NumDoActionCalls);
                }

                svc.Stop();
            }           
        }

        [TestMethod]
        public void Test_Service_BasicHttp_Initialize()
        {
            using (var svc = new TestService())
            {
                svc.Initialize(8080, "TestSingleSvc", Rowa.Lib.Wcf.BindingType.BasicHttp);

                using (var client = new ServiceHttp.TestServiceClient())
                {
                    client.Open();
                    client.DoAction();
                    Assert.AreEqual(1, svc.NumDoActionCalls);
                }

                svc.Stop();
            }
        }

        [TestMethod]
        public void Test_Service_GetClientEndpoint_NetTcp()
        {
            using (var svc = new TestService(12345, "TestSingleSvc", Rowa.Lib.Wcf.BindingType.NetTcp))
            {
                using (var client = new ServiceNetTcp.TestServiceClient())
                {
                    client.Open();
                    var endpoint = client.GetMyEndpoint();
                    Assert.IsTrue(endpoint.Contains(IPAddress.IPv6Loopback.ToString()) || endpoint.Contains(IPAddress.Loopback.ToString()));
                }

                svc.Stop();
            }
        }

        [TestMethod]
        public void Test_Service_GetClientEndpoint_BasicHttp()
        {
            using (var svc = new TestService(8080, "TestSingleSvc", Rowa.Lib.Wcf.BindingType.BasicHttp))
            {
                using (var client = new ServiceHttp.TestServiceClient())
                {
                    client.Open();
                    var endpoint = client.GetMyEndpoint();
                    Assert.IsTrue(endpoint.Contains(IPAddress.IPv6Loopback.ToString()) || endpoint.Contains(IPAddress.Loopback.ToString()));
                }

                svc.Stop();
            }
        }
    }
}
