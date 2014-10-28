using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuChang.Core.Models
{
    public class ImageReply
    {
        public string ToUserName { get; set; }

        public string FromUserName { get; set; }

        public DateTime CreateTime { get; set; }

        public MessageType MsgType { get; set; }

        public string MediaId { get; set; }
    }
}
