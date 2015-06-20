using System;

namespace YuChang.Core.Messages
{
    public class TemplateSendJobFinishEvent : EventMessage
    {
        public string MsgID
        {
            get;
            set;
        }
        public string Status
        {
            get;
            set;
        }
        public TemplateSendJobFinishEvent()
            : base(EventType.TemplateSendJobFinish)
        {
        }
    }
}
