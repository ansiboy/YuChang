using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YuChang.Core.Models;

namespace YuChang.Core
{
    public class MessageProcesser
    {
        public Message Process(string xml)
        {
            var msg = Message.FromXml(xml);
            switch (msg.MsgType)
            {
                case Models.MessageType.Event:
                    var eventType = ((EventMessage)msg).Event;
                    switch (eventType)
                    {
                        case EventType.Click:
                            return ProcessClickEvent((ClickEvent)msg);
                        case EventType.Location:
                            return ProcessLocationEvent((LocationEvent)msg);
                        case EventType.Scan:
                            return ProcessScanEvent((ScanEvent)msg);
                        case EventType.Subscribe:
                            return ProcessSubscribeEvent((SubscribeEvent)msg);
                        case EventType.Unsubscribe:
                            return ProcessUnsubscribeEvent((UnsubscribeEvent)msg);
                        case EventType.View:
                            return ProcessViewEvent((ViewEvent)msg);
                        case EventType.TemplateSendJobFinish:
                            return ProcessTemplateSendJobFinishEvent((TemplateSendJobFinishEvent)msg);
                    }
                    break;
                case Models.MessageType.Image:
                    return ProcessImageMessage((ImageMessage)msg);
                case Models.MessageType.Link:
                    return ProcessLinkMessage((LinkMessage)msg);
                case Models.MessageType.Location:
                    return ProcessLocationMessage((LocationMessage)msg);
                case Models.MessageType.Text:
                    return ProcessTextMessage((TextMessage)msg);
                case Models.MessageType.Video:
                    return ProcessVideoMessage((VideoMessage)msg);
                case Models.MessageType.Voice:
                    return ProcessVoiceMessage((VoiceMessage)msg);
            }
            return null;
        }

        private Message ProcessTemplateSendJobFinishEvent(TemplateSendJobFinishEvent msg)
        {
            return DefaultProcess(msg);
        }

        protected virtual Message ProcessViewEvent(ViewEvent msg)
        {
            return DefaultProcess(msg);
        }

        protected virtual Message ProcessClickEvent(ClickEvent msg)
        {
            return DefaultProcess(msg);
        }

        protected virtual Message ProcessLocationEvent(LocationEvent msg)
        {
            return DefaultProcess(msg);
        }

        protected virtual Message ProcessScanEvent(ScanEvent msg)
        {
            return DefaultProcess(msg);
        }

        protected virtual Message ProcessSubscribeEvent(SubscribeEvent msg)
        {
            return DefaultProcess(msg);
        }

        protected virtual Message ProcessUnsubscribeEvent(UnsubscribeEvent msg)
        {
            return DefaultProcess(msg);
        }

        protected virtual Message ProcessImageMessage(ImageMessage msg)
        {
            return DefaultProcess(msg);
        }

        protected virtual Message ProcessLinkMessage(LinkMessage msg)
        {
            return DefaultProcess(msg);
        }

        protected virtual Message ProcessLocationMessage(LocationMessage msg)
        {
            return DefaultProcess(msg);
        }

        protected virtual Message ProcessTextMessage(TextMessage msg)
        {
            return DefaultProcess(msg);
        }

        protected virtual Message ProcessVideoMessage(VideoMessage msg)
        {
            return DefaultProcess(msg);
        }

        protected virtual Message ProcessVoiceMessage(VoiceMessage msg)
        {
            return DefaultProcess(msg);
        }

        protected virtual Message DefaultProcess(Message msg)
        {
            var reply = new TextMessage();
            reply.ToUserName = msg.FromUserName;
            reply.FromUserName = msg.ToUserName;
            if (msg.MsgType == MessageType.Event)
            {
                reply.Content = string.Format("{0} event is not processed.", ((EventMessage)msg).Event);
            }
            else
            {
                reply.Content = string.Format("{0} message is not processed.", msg.MsgType);
            }
            return reply;
        }


    }
}
