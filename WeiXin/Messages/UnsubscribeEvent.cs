using System;

namespace YuChang.Core.Messages
{
    public class UnsubscribeEvent : EventMessage
    {
        public UnsubscribeEvent()
            : base(EventType.Unsubscribe)
        {
        }
    }
}
