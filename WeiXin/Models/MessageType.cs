using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace YuChang.Core.Models
{
    public enum MessageType
    {
        [Description("Text")]
        Text,

        [Description("Image")]
        Image,

        [Description("Voice")]
        Voice,

        [Description("Video")]
        Video,

        [Description("Location")]
        Location,

        [Description("Link")]
        Link,

        [Description("Event")]
        Event,

        [Description("transfer_customer_service")]
        TransferCustomerService,

        [Description("ImageText")]
        ImageText
    }
}
