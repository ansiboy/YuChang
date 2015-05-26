using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace YuChang.Core.Models
{
    public enum MessageType
    {
        [Description("text")]
        Text,

        [Description("image")]
        Image,

        [Description("voice")]
        Voice,

        [Description("video")]
        Video,

        [Description("location")]
        Location,

        [Description("link")]
        Link,

        [Description("event")]
        Event,

        [Description("news")]
        News,

        [Description("music")]
        Music,

        /// <summary>
        /// 未匹配的消息
        /// </summary>
        Undetected,


    }

    
}
