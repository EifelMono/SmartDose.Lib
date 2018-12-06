using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RowaWcf.Tests.TestServices;
using System.IO;
using System.Xml.Serialization;
using System.Text;

namespace RowaWcf.Tests
{
    [TestClass]
    public class TestGuiMessageService
    {
        [TestMethod]
        public void Test_MessageService_ProcessMessage()
        {
            using (var svc = new TestMessageService(false))
            using (var client = new MessageService.MessageServiceClient())
            {
                client.Open();

                var msg = new TestCommandMessage()
                {
                    Id = 1, Result = string.Empty
                };

                var resultMessageString = client.ProcessMessage(SerializeMessage<TestCommandMessage>(msg));

                Assert.IsFalse(string.IsNullOrEmpty(resultMessageString));

                var resultMessage = DeserializeMessage<TestCommandMessage>(resultMessageString);

                Assert.IsNotNull(resultMessage);
                Assert.AreEqual(msg.Id + 1, resultMessage.Id);
                Assert.AreEqual(resultMessage.Id.ToString(), resultMessage.Result);
                Assert.AreEqual(false, svc.HandlerHasBeenCalled);
            }
        }

        [TestMethod]
        public void Test_MessageService_ProcessMessageWithHandler()
        {
            using (var svc = new TestMessageService(true))
            using (var client = new MessageService.MessageServiceClient())
            {
                client.Open();

                var msg = new TestCommandMessage()
                {
                    Id = 1,
                    Result = string.Empty
                };

                var resultMessageString = client.ProcessMessage(SerializeMessage<TestCommandMessage>(msg));

                Assert.IsFalse(string.IsNullOrEmpty(resultMessageString));

                var resultMessage = DeserializeMessage<TestCommandMessage>(resultMessageString);

                Assert.IsNotNull(resultMessage);
                Assert.AreEqual(msg.Id + 1, resultMessage.Id);
                Assert.AreEqual(resultMessage.Id.ToString(), resultMessage.Result);
                Assert.AreEqual(true, svc.HandlerHasBeenCalled);

            }
        }

        [TestMethod]
        public void Test_MessageService_GetMessage()
        {
            using (var svc = new TestMessageService(false))
            using (var client = new MessageService.MessageServiceClient())
            {
                client.Open();

                var msg = new TestCommandMessage()
                {
                    Id = 1,
                    Result = string.Empty
                };

                var resultMessageString = client.ProcessMessage(SerializeMessage<TestCommandMessage>(msg));

                Assert.IsFalse(string.IsNullOrEmpty(resultMessageString));

                var resultMessage = DeserializeMessage<TestCommandMessage>(resultMessageString);

                Assert.IsNotNull(resultMessage);
                Assert.AreEqual(msg.Id + 1, resultMessage.Id);
                Assert.AreEqual(resultMessage.Id.ToString(), resultMessage.Result);

                var eventMessageString = client.GetMessage(string.Empty);

                Assert.IsFalse(string.IsNullOrEmpty(eventMessageString));

                var eventMessage = DeserializeMessage<TestEventMessage>(eventMessageString);
                Assert.IsNotNull(eventMessage);
                Assert.AreEqual(resultMessage.Result, eventMessage.EventData);
            }
        }

        private string SerializeMessage<T>(T message)
        {
            using (var m = new MemoryStream())
            {
                var xml = new XmlSerializer(typeof(T));
                xml.Serialize(m, message);
                m.Flush();

                return Encoding.UTF8.GetString(m.GetBuffer(), 0, (int)m.Length);
            }
        }

        private T DeserializeMessage<T>(string message)
        {
            using (var m = new MemoryStream(Encoding.UTF8.GetBytes(message)))
            {
                var xml = new XmlSerializer(typeof(T));
                return (T)xml.Deserialize(m);
            }
        }
    }
}
