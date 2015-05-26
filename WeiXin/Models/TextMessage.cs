using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuChang.Core.Models
{
    /// <summary>
    /// 文本消息
    /// </summary>
    public class TextMessage : NormalMessage
    {
        public TextMessage()
            : base(MessageType.Text)
        {

        }

        public string Content { get; set; }

        /// <summary>
        /// 消息id
        /// </summary>
        public long MsgId { get; internal set; }

        //protected override System.Xml.XmlElement ParseModelToXml()
        //{
        //    return base.ParseModelToXml();
        //}
    }


}
