using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuChang.Core.Messages
{
    public class LinkMessage : Message
    {
        public LinkMessage()
            : base(MessageType.Link)
        {

        }

        /// <summary>
        /// 开发者微信号
        /// </summary>
        public string ToUserName { get; set; }

        /// <summary>
        /// 发送方帐号（一个OpenID）
        /// </summary>
        public string FromUserName { get; set; }

        /// <summary>
        /// 消息创建时间 
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 视频为video
        /// </summary>
        public MessageType MsgType { get; set; }

        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 消息描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 消息链接
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 消息id
        /// </summary>
        public long MsgId { get; set; }
    }
}
