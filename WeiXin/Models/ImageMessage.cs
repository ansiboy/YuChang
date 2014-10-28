using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuChang.Core.Models
{
    /// <summary>
    /// 图片消息
    /// </summary>
    public class ImageMessage : Message
    {
        public ImageMessage()
            : base(MessageType.Image)
        {

        }

        /// <summary>
        /// 图片链接
        /// </summary>
        public string PicUrl { get; set; }

        /// <summary>
        /// 图片消息媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
        public string MediaId { get; set; }

        /// <summary>
        /// 消息id
        /// </summary>
        public long MsgId { get; set; }
    }
}
