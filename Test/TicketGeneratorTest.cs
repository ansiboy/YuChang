using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YuChang.Core;

namespace WeiXin.Test
{
    [TestClass]
    public class TicketGeneratorTest
    {
        [TestMethod]
        public void GetJsapiTicket()
        {
            var a = new AccessToken("wxe621f4e5e90b13cd", "7cb56e5b6bd302ddb73c4f76a9ec26a2");
            var t = new TicketGenerator(a);
            var js_ticket = t.GetJsapiTicket();
            Assert.IsNotNull(js_ticket);
            Console.WriteLine(js_ticket);
        }
    }
}
