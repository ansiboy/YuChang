using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YuChang.Core;
using YuChang.Core.Models;

namespace WeiXin.Test
{
    [TestClass]
    public class TemplateManagerTest
    {
        [TestMethod]
        public void SendTest()
        {
            // var appid = "wxa22b9cfd8fdec01a";
            //var secret = "d383a6032a4cef3dc22ba9c5130b60e2";
            var accessToken = new AccessToken("wxa22b9cfd8fdec01a", "d383a6032a4cef3dc22ba9c5130b60e2");
            //var accessToken = new AccessToken("wxe621f4e5e90b13cd", "7cb56e5b6bd302ddb73c4f76a9ec26a2"); //vknew
            //var tmp = new Template("USLehg7XUyraVd2-FevC9NuTvzcCdTdbSnqTOamiZjE", "o1Ux1uGtRiummfXjhZ7vyIqNlzTU");
            var tmp = new Template("USLehg7XUyraVd2-FevC9NuTvzcCdTdbSnqTOamiZjE", "o1Ux1uGtRiummfXjhZ7vyIqNlzTU");
            tmp.Fields.Add(new TemplateField("first", "您好，你的商品已发货"));
            tmp.Fields.Add(new TemplateField("keyword1", "申通快递"));
            tmp.Fields.Add(new TemplateField("keyword2", "1111111111"));
            tmp.Fields.Add(new TemplateField("keyword3", "多个商品"));
            tmp.Fields.Add(new TemplateField("keyword4", "5"));
            tmp.Fields.Add(new TemplateField("remark", "请注意查收。"));

            var m = new TemplateManager(accessToken);
            m.Send(tmp);
        }
    }
}
