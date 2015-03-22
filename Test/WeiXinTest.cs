using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YuChang.Core;

namespace YuChang.Core.Test
{
    [TestClass]
    public class WeiXinTest
    {
        const string appid = "wxf1c24c60e3ac13b7";
        const string secret = "5902b9817acb7a290d4b7c2e6e97d4d3";

        //const string appid = "wxd7066d4e88335b68";
        //const string secret = "9eae7d11cac7c612f05a672d2f8919e7";

        //YuChang.Core.WeiXinService weixin;

        [TestInitialize()]
        public void Initialize()
        {
            //this.weixin = new YuChang.Core.WeiXinService(appid, secret);
        }

        [TestMethod]
        public void tokenTest()
        {
            var token1 = WeiXin.token(appid, secret);
            Assert.IsNotNull(token1);

            var token2 = WeiXin.token(appid, secret);
            Assert.IsNotNull(token2);

            Assert.AreEqual(token1, token2);

            var str1 = token1.ToString();
            Console.WriteLine(str1);
            Assert.IsNotNull(str1);

            var str2 = token2.ToString();
            Assert.IsNotNull(str2);

            Assert.AreEqual(str1, str2);
        }

        [TestMethod]
        public void getcallbackip()
        {
            var token = WeiXin.token(appid, secret);
            var ips = WeiXin.getcallbackip(token);
            Assert.IsNotNull(ips);
        }

        [TestMethod]
        public void menuCreate()
        {
            var btn1 = new Button
            {
                type = ButtonType.click,
                name = "今日歌曲",
                key = "V1001_TODAY_MUSIC"
            };
            var btn2 = new Button
            {
                name = "菜单",
                sub_button = new[] { 
                    new Button{
                        type= ButtonType.view,
                        name="搜索",
                        url="http://www.soso.com/"
                    }
                }
            };
            var buttons = new Button[] { btn1, btn2 };
            //weixin.menu.create(buttons);
            var token = WeiXin.token(appid, secret);
            WeiXin.menu.create(token, buttons);
        }

        [TestMethod]
        public void menuGet()
        {
            var token = WeiXin.token(appid, secret);
            var buttons = WeiXin.menu.get(token);
            Assert.IsNotNull(buttons);
            Assert.IsTrue(buttons.Length > 0);
        }

        [TestMethod]
        public void menuDelete()
        {
            var token = WeiXin.token(appid, secret);
            WeiXin.menu.delete(token);

        }
    }
}
