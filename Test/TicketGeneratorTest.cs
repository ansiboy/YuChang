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
            //Id	URL	Token	AppId	AppSecret	ApplicationId	CreateDateTime	IsHide	IsDelete	Remark	AppKey	PartnerId	PartnerKey
            //wxa22b9cfd8fdec01a	d383a6032a4cef3dc22ba9c5130b60e2	7BBFA36C-8115-47AD-8D47-9E52B58E7EFD	2014-06-25 15:46:19.643	0	0	NULL	IUInS0W9rSnhRsF1AC1YZx9TVX5elH2bbRV4wbcEMN8ykdz4r7ht3jyOjqiYYqZcNi1SIig7EVKckcl6eIQs0GdH6BmmbRo5dElL5e9cinlmbnbXhClKbItAAfE0wuCY	1220064601	a09d39a068eda7245dbc8de29e4f2ba5
            //var a = new AccessToken("wxe621f4e5e90b13cd", "7cb56e5b6bd302ddb73c4f76a9ec26a2");
            var a = new AccessToken("wxa22b9cfd8fdec01a", "d383a6032a4cef3dc22ba9c5130b60e2");
            var t = new TicketGenerator(a);
            var js_ticket = t.GetJsapiTicket();
            var s = new YuChang.Core.PromoteService(a);
            for (var i = 10842; i <= 10841 + 20; i++)
            {
                var str = s.GenerateSquareCode(i);
                Console.WriteLine(str);
            }
            //Assert.IsNotNull(js_ticket);
            //Console.WriteLine(js_ticket);
        }
    }
}
