using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuChang.Core.Messages
{
    /// <summary>
    /// 语音消息
    /// </summary>
    public class VoiceMessage : Message
    {
        public VoiceMessage()
            : base(MessageType.Voice)
        {

        }

        /// <summary>
        /// 语音消息媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
        public string MediaId { get; set; }

        /// <summary>
        /// 语音格式，如amr，speex等
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// 消息id
        /// </summary>
        public long MsgId { get; set; }
    }
}
