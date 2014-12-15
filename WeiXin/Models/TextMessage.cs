using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuChang.Core.Models
{
    /// <summary>
    /// 文本消息
    /// </summary>
    public class TextMessage : Message
    {
        public TextMessage()
            : base(MessageType.Text)
        {

        }

        public string Content { get; set; }

        /// <summary>
        /// 消息id
        /// </summary>
        public long MsgId { get; set; }
    }


}
