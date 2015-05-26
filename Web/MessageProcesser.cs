using System.Diagnostics;
using System.Linq;
using System;
using System.Data.Common;
using System.Data;
using YuChang.Core.Models;
using YuChang.Core;

namespace YuChang.Web
{
    public class MessageProcesser : YuChang.Core.MessageProcesser
    {
        const int MAX_SCENE_ID = 10000;

        private AccessToken accessToken;
        private IDbConnection conn;

        public MessageProcesser(string appid, string secret, IDbConnection conn)
        {
            this.accessToken = AccessTokenPool.GetAccessToken(appid, secret); //new AccessToken(appid, secret);
            this.conn = conn;
        }

        protected override Message DefaultProcess(Message msg)
        {
            conn.Open();
            var command = conn.CreateCommand();
            command.CommandText = @"Select top(1) Id, [Message] 
                                    Order By CreateDateTime desc 
                                    Where MessageType = '" + msg.MsgType + "'";

            if (msg.MsgType == MessageType.Event)
            {
                command.CommandText = command.CommandText + " And EventType = '" + ((EventMessage)msg).Event + "'";
            }

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var xml = reader.GetString(1);
                var message = Message.FromXml(xml);
                message.ToUserName = msg.FromUserName;

                return message;
            }

            return base.DefaultProcess(msg);
        }

        protected override Message ProcessSubscribeEvent(SubscribeEvent msg)
        {
            return base.ProcessSubscribeEvent(msg);
        }

        protected override Message ProcessScanEvent(ScanEvent msg)
        {
            return base.ProcessScanEvent(msg);
        }

        protected override Message ProcessUnsubscribeEvent(UnsubscribeEvent msg)
        {
            return base.ProcessUnsubscribeEvent(msg);
        }
    }
}