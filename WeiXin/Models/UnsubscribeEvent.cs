using System;
namespace YuChang.Core.Models
{
    public class UnsubscribeEvent : EventMessage
    {
        public UnsubscribeEvent()
            : base(EventType.Unsubscribe)
        {
        }
    }
}
