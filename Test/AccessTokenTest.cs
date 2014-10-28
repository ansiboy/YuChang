using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YuChang.Core;

namespace WeiXin.Test
{
    [TestClass]
    public class AccessTokenTest
    {
        [TestMethod]
        public void ToString()
        {
            var accessToken = new AccessToken("wxe621f4e5e90b13cd", "7cb56e5b6bd302ddb73c4f76a9ec26a2");
            var token = accessToken.ToString();
            Assert.IsNotNull(token);
        }
    }
}
