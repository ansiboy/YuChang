﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        const string appid = "wxf0bb634c8352c4f3";
        const string secret = "e6ce53f5054c4d6c0dacd2360996a98b";

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
            var token1 = weixin.token(appid, secret);
            Assert.IsNotNull(token1);

            var token2 = weixin.token(appid, secret);
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
            var token = weixin.token(appid, secret);
            var ips = weixin.getcallbackip(token);
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
            var token = weixin.token(appid, secret);
            weixin.menu.create(token, buttons);
        }

        [TestMethod]
        public void menuGet()
        {
            var token = weixin.token(appid, secret);
            var buttons = weixin.menu.get(token);
            Assert.IsNotNull(buttons);
            Assert.IsTrue(buttons.Length > 0);
        }

        [TestMethod]
        public void menuDelete()
        {
            var token = weixin.token(appid, secret);
            weixin.menu.delete(token);

        }

        [TestMethod]
        public void Temp()
        {
            var token = weixin.token(appid, secret);
            var result = weixin.user.get(token, "");
            Console.WriteLine(result.data.openid[0]);

            var prepayId = mch.pay.unifiedorder(appid, "1236045602", result.data.openid[0], "a312b8e09667d4b9c25fae66c5822d6e",
                                  "body", "http://www.163.com", "11111111111", 22222);
        }

        [TestMethod]
        public void qrcode_create()
        {
            var token = weixin.token(appid, secret);
            var result = weixin.qrcode.create(token, ActionName.QR_LIMIT_SCENE, 0);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.ticket);
            Assert.IsNotNull(result.url);
        }

        [TestMethod]
        public void ticket_getticket()
        {
            var token = weixin.token(appid, secret);
            var result = weixin.ticket.getticket(token, TicketType.jsapi);
            Assert.AreEqual(0, result.errcode);
        }
    }
}
