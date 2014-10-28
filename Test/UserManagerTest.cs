using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YuChang.Core;

namespace WeiXin.Test
{
    [TestClass]
    public class UserManagerTest
    {
        [TestMethod]
        public void GetUserInfo()
        {
            var a = new AccessToken("wxe621f4e5e90b13cd", "7cb56e5b6bd302ddb73c4f76a9ec26a2");
            var m = new UserManager(a);
            var userInfo = m.GetUserInfo("o4mqUjspXEinqvno9XS3RUGEITS8");
            Assert.IsNotNull(userInfo);
            Console.WriteLine(userInfo.NickName);
            
        }
    }
}
