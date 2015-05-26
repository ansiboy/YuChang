using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YuChang.Core;
using YuChang.Core.Models;

namespace YuChang.Test
{
    [TestClass]
    public class CustomMenuTest
    {
        [TestMethod]
        public void GetMenu()
        {
            var appid = "wxe621f4e5e90b13cd";
            var secret = "7cb56e5b6bd302ddb73c4f76a9ec26a2";

            var accessToken = YuChang.Core.WeiXin.token(appid, secret);
            //var cm = //new CustomMenu(new AccessToken(appid, secret));
            var buttons = YuChang.Core.WeiXin.menu.get(accessToken);
        }

        [TestMethod]
        public void SaveMenu()
        {
            var appid = "wxa22b9cfd8fdec01a";
            var secret = "d383a6032a4cef3dc22ba9c5130b60e2";

            //            wxa22b9cfd8fdec01a
            //AppSecret(应用密钥) d383a6032a4cef3dc22ba9c5130b60e2 重置
            var accessToken = YuChang.Core.WeiXin.token(appid, secret);

            //var cm = //new CustomMenu(new AccessToken(appid, secret));
            var buttons = YuChang.Core.WeiXin.menu.get(accessToken); //cm.GetMenu().ToArray();

            var button = new YuChang.Core.Models.Button();
            button.Key = "OnlineCustomerService";
            button.Name = "在线客服";
            button.Type = YuChang.Core.Models.ButtonType.Click;

            var btns = buttons[2].sub_button.Where(o => o.name == "在线客户").ToArray();
            foreach (var b in btns)
            {
                //buttons[2].sub_button.Remove(b);
            }

            //buttons[2].Children.Insert(0, button);
            //cm.SaveMenu(buttons);
        }

        [TestMethod]
        public void Temp()
        {
            var xml = @"<xml>
<ToUserName><![CDATA[touser]]></ToUserName>
<FromUserName><![CDATA[fromuser]]></FromUserName>
<CreateTime>1399197672</CreateTime>
<MsgType><![CDATA[transfer_customer_service]]></MsgType>
</xml>";
            //var p = new MsgProcesser();
            //var msg = p.Process(xml);
            //var x = msg.ToXml();
            var message = Message.FromXml(xml);

            var x = message.ToXml();
        }
    }
}
