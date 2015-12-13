using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuChang.Core.Messages
{
    public abstract class EventMessage : Message
    {
        public EventMessage(EventType eventType)
            : base(MessageType.Event)
        {
            this.Event = eventType;
        }

        /// <summary>
        /// 事件类型
        /// </summary>
        public EventType Event { get; private set; }
    }

    public class SubscribeEvent : EventMessage
    {
        public SubscribeEvent()
            : base(EventType.Subscribe)
        {

        }

        /// <summary>
        /// 事件KEY值，qrscene_为前缀，后面为二维码的参数值
        /// </summary>
        public string EventKey { get; set; }

        /// <summary>
        /// 二维码的ticket，可用来换取二维码图片
        /// </summary>
        public string Ticket { get; set; }
    }

    public class ScanEvent : EventMessage
    {
        public ScanEvent()
            : base(EventType.Scan)
        {

        }

        /// <summary>
        /// 事件KEY值，qrscene_为前缀，后面为二维码的参数值
        /// </summary>
        public string EventKey { get; set; }

        /// <summary>
        /// 二维码的ticket，可用来换取二维码图片
        /// </summary>
        public string Ticket { get; set; }
    }

    public class LocationEvent : EventMessage
    {
        public LocationEvent()
            : base(EventType.Location)
        {

        }

        /// <summary>
        /// 地理位置纬度
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// 地理位置经度
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// 地理位置精度
        /// </summary>
        public double Precision { get; set; }
    }

    public class ClickEvent : EventMessage
    {
        public ClickEvent()
            : base(EventType.Click)
        {

        }

        /// <summary>
        /// 事件KEY值，qrscene_为前缀，后面为二维码的参数值
        /// </summary>
        public string EventKey { get; set; }
    }

    public class ViewEvent : EventMessage
    {
        public ViewEvent()
            : base(EventType.View)
        {

        }

        public string EventKey { get; set; }
    }

    
}
