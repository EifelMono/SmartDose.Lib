using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Rowa.Lib.Wcf.Setup;

namespace RowaWcf.Tests
{
    /// <summary>
    /// Contains the unit tests for the firewall configuration logic.
    /// </summary>
    [TestClass]
    public class TestFirewall
    {
        [TestInitialize]
        public void TestInitialize()
        {
            // ensure that the unit test engine is running as administrator
            // this is required to successfully process the tests

            try
            {
                var testFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "test.txt");
                var file = new StreamWriter(testFilePath);
                file.Close();
                File.Delete(testFilePath);
            }
            catch (Exception)
            {
                Assert.Fail("Unit test requires administrative privileges to run.");
            }
        }

        [TestMethod]
        public void Test_Firewall_AddRule()
        {
            Assert.IsTrue(Firewall.AddRule(1234, WcfServiceUser.NetworkService));
            Assert.IsTrue(Firewall.AddRule(5678, WcfServiceUser.Everyone));
            Assert.IsTrue(Firewall.AddRule(9101, Environment.UserName));

            Firewall.RemoveRule(1234);
            Firewall.RemoveRule(5678);
            Firewall.RemoveRule(9101);
        }

        [TestMethod]
        public void Test_Firewall_RemoveRule()
        {
            Assert.IsFalse(Firewall.RemoveRule(1234));
            Assert.IsTrue(Firewall.AddRule(1234, WcfServiceUser.NetworkService));
            Assert.IsTrue(Firewall.RemoveRule(1234));
        }

    }
}
