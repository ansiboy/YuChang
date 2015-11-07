using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YuChang.Core.Messages;

namespace YuChang.Test
{
    [TestClass]
    public class TextMessageTest
    {
        [TestMethod]
        public void ToXml()
        {
            var message = new TextMessage();
            message.ToUserName = "toUser";
            message.FromUserName = "fromUser";
            message.CreateTime = DateTime.Now;
            message.Content = "Hello World";

            var xml = message.ToXml();
            Console.WriteLine(xml);
        }
    }
}
