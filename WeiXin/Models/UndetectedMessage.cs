using System;
namespace YuChang.Core.Models
{
    public class UndetectedMessage : Message
    {
        public UndetectedMessage()
            : base(MessageType.Undetected)
        {
        }
    }
}
