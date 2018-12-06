using Rowa.Lib.Wcf.Gui;

namespace RowaWcf.Tests.TestServices
{
    internal class TestMessageService : MessageService<BaseMessage, Rowa.Lib.Wcf.Gui.IMessageService>
    {
        public bool HandlerHasBeenCalled = false;

        public TestMessageService(bool registerHandler)
            : base(8080, "Gui", false)
        {
            if (registerHandler == false)
            {
                RegisterMessageType<TestCommandMessage>();
            }
            else
            {
                RegisterMessageType<TestCommandMessage>((m) => MyMessageHandler((TestCommandMessage)m));
            }

            RegisterMessageType<TestEventMessage>();

            Start();
        }

        protected override void ProcessMessage(BaseMessage message)
        {
            if (message is TestCommandMessage)
            {
                var msg = (TestCommandMessage)message;
                msg.Id++;
                msg.Result = msg.Id.ToString();

                SendMessage(new TestEventMessage() { EventData = msg.Result });
            }
        }

        private void MyMessageHandler(TestCommandMessage message)
        {
            HandlerHasBeenCalled = true;

            var msg = (TestCommandMessage)message;
            msg.Id++;
            msg.Result = msg.Id.ToString();
        }
    }
}
