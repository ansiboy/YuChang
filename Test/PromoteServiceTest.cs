using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YuChang.Core;

namespace WeiXin.Test
{
    [TestClass]
    public class PromoteServiceTest
    {
        [TestMethod]
        public void GenerateSquareCode()
        {
            var accessToken = new AccessToken("wxe621f4e5e90b13cd", "7cb56e5b6bd302ddb73c4f76a9ec26a2");
            var s = new PromoteService(accessToken);
            var ticket = s.GenerateSquareCode(1);
            Assert.IsNotNull(ticket);
            Console.WriteLine(ticket);
            Console.WriteLine(System.Web.HttpUtility.UrlEncode(ticket));
        }
    }
}
