using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuChang.Core.Models
{
    /// <summary>
    /// 视频消息
    /// </summary>
    public class VideoMessage : Message
    {
        public VideoMessage()
            : base(MessageType.Video)
        {

        }

        /// <summary>
        /// 视频消息媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
        public string MediaId { get; set; }

        /// <summary>
        /// 视频消息缩略图的媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
        public string ThumbMediaId { get; set; }

        /// <summary>
        /// 消息id
        /// </summary>
        public long MsgId { get; set; }
    }
}
