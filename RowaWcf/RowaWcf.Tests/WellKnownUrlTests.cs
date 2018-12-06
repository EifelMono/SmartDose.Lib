using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rowa.Lib.Wcf.Configuration;

namespace RowaWcf.Tests
{
    [TestClass]
    public class WellKnownUrlTests
    {
        [TestMethod]
        public void TestWellKnownUrls()
        {
            Assert.AreEqual("net.tcp://127.0.0.1:9001/HardwareService", WellKnownUrl.Get(WellKnownUrlType.SDHardware, "127.0.0.1"));
            Assert.AreEqual("net.tcp://127.0.0.1:9012/AuthorizationClient", WellKnownUrl.Get(WellKnownUrlType.SDAuthentication, "127.0.0.1"));
            Assert.AreEqual("net.tcp://127.0.0.1:9004/CanisterHandling", WellKnownUrl.Get(WellKnownUrlType.SDCanisterHandling, "127.0.0.1"));
            Assert.AreEqual("net.tcp://127.0.0.1:9006/Inventory", WellKnownUrl.Get(WellKnownUrlType.SDInventory, "127.0.0.1"));
            Assert.AreEqual("net.tcp://127.0.0.1:9002/MasterData", WellKnownUrl.Get(WellKnownUrlType.SDMasterData, "127.0.0.1"));
            Assert.AreEqual("net.tcp://127.0.0.1:9008/Production", WellKnownUrl.Get(WellKnownUrlType.SDProduction, "127.0.0.1"));
            Assert.AreEqual("net.tcp://127.0.0.1:9005/Reporting", WellKnownUrl.Get(WellKnownUrlType.SDReporting, "127.0.0.1"));
            Assert.AreEqual("net.tcp://127.0.0.1:9000/SDMC", WellKnownUrl.Get(WellKnownUrlType.SDSdmc, "127.0.0.1"));
            Assert.AreEqual("net.tcp://127.0.0.1:9009/Settings", WellKnownUrl.Get(WellKnownUrlType.SDSettings, "127.0.0.1"));
            Assert.AreEqual("net.tcp://127.0.0.1:9003/TrayHandling", WellKnownUrl.Get(WellKnownUrlType.SDTrayHandling, "127.0.0.1"));
            Assert.AreEqual("net.tcp://127.0.0.1:9014/Deblistering", WellKnownUrl.Get(WellKnownUrlType.SDDeblistering, "127.0.0.1"));
        }
    }
}
