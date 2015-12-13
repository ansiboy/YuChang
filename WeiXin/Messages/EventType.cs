using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuChang.Core.Messages
{
    public enum EventType
    {
        Subscribe,
        Unsubscribe,
        Scan,
        Location,
        Click,
        View,
        TemplateSendJobFinish,
        Undetected,
    }
}
