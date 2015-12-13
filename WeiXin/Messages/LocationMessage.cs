using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuChang.Core.Messages
{
    public class LocationMessage : Message
    {
        public LocationMessage()
            : base(MessageType.Location)
        {

        }
        
        /// <summary>
        /// 地理位置维度
        /// </summary>
        public double Location_X { get; set; }

        /// <summary>
        /// 地理位置经度
        /// </summary>
        public double Location_Y { get; set; }

        /// <summary>
        /// 地图缩放大小
        /// </summary>
        public int Scale { get; set; }

        /// <summary>
        /// 地理位置信息
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 消息id
        /// </summary>
        public long MsgId { get; set; }
    }
}
