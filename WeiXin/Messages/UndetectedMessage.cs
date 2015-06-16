using System;

namespace YuChang.Core.Messages
{
    public class UndetectedMessage : Message
    {
        public UndetectedMessage()
            : base(MessageType.Undetected)
        {
        }
    }

    public class UndetectedEvent : EventMessage
    {
        public UndetectedEvent()
            : base(EventType.Undetected)
        {

        }
    }
}
