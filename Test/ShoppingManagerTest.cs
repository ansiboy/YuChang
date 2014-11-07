using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YuChang.Core;

namespace WeiXin.Test
{
    [TestClass]
    public class ShoppingManagerTest
    {
        [TestMethod]
        public void Deliver()
        {
            var appid = "wxa22b9cfd8fdec01a";
            var secret = "d383a6032a4cef3dc22ba9c5130b60e2";
            var appkey = "IUInS0W9rSnhRsF1AC1YZx9TVX5elH2bbRV4wbcEMN8ykdz4r7ht3jyOjqiYYqZcNi1SIig7EVKckcl6eIQs0GdH6BmmbRo5dElL5e9cinlmbnbXhClKbItAAfE0wuCY";

            var m = new ShoppingManager(appid, secret, appkey);
            m.Deliver("o1Ux1uCLAjgzgBMoHnBbwbHdrYFg", "1220064601201408083213682399", "c33bc4a4e7aa4bfaa176a30465d28ad8");
        }

        [TestMethod]
        public void QueryOrder()
        {
            var appid = "wxa22b9cfd8fdec01a";
            var secret = "d383a6032a4cef3dc22ba9c5130b60e2";
            var appkey = "IUInS0W9rSnhRsF1AC1YZx9TVX5elH2bbRV4wbcEMN8ykdz4r7ht3jyOjqiYYqZcNi1SIig7EVKckcl6eIQs0GdH6BmmbRo5dElL5e9cinlmbnbXhClKbItAAfE0wuCY";

            var m = new ShoppingManager(appid, secret, appkey);
            var partnerid = "1220064601";
            var partnerkey = "a09d39a068eda7245dbc8de29e4f2ba5";
            m.QueryOrder(partnerid, partnerkey, "a347ff10ca4048e38f27a5c7df4819b0");
        }

        [TestMethod]
        public void MCHDonw()
        {
            var appid = "wxa22b9cfd8fdec01a";
            var secret = "d383a6032a4cef3dc22ba9c5130b60e2";
            var appkey = "IUInS0W9rSnhRsF1AC1YZx9TVX5elH2bbRV4wbcEMN8ykdz4r7ht3jyOjqiYYqZcNi1SIig7EVKckcl6eIQs0GdH6BmmbRo5dElL5e9cinlmbnbXhClKbItAAfE0wuCY";

            var m = new ShoppingManager(appid, secret, appkey);
            var partnerid = "1220064601";
            var partnerkey = "a09d39a068eda7245dbc8de29e4f2ba5";
            m.MCHDonw(partnerid, partnerkey, new DateTime(2014, 11, 5));
        }
    }
}
