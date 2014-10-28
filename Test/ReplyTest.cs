using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using WeiXin;
using YuChang.Core.Models;


namespace Test
{
    [TestClass]
    public class ReplyTest
    {
        [TestMethod]
        public void TextReplyTest()
        {
            var reply = new TextMessage();
            reply.ToUserName = "toUser";
            reply.FromUserName = "fromUser";
            //reply.CreateTime = UnixTimeToTime(12345678);
            //reply.MsgType = PostMessageType.Text;
            reply.Content = "你好";

            var xml = reply.ToXml();
            Console.WriteLine(xml);
        }

        static DateTime UnixTimeToTime(int timeStamp)
        {

            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));

            long lTime = long.Parse(timeStamp.ToString() + "0000000");

            TimeSpan toNow = new TimeSpan(lTime);

            return dtStart.Add(toNow);

        }
    }
}
