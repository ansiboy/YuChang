using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
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

        [TestMethod]
        public void PayTest()
        {
            //公众账号ID appid 是String(32) 微信分配的公众账号ID
            //商户号mch_id 是String(32) 微信支付分配的商户号
            //设备号device_info 否String(32) 微信支付分配的终端设备号
            //随机字符串nonce_str 是String(32) 随机字符串，不长于32 位
            //签名sign 是String(32) 签名,详细签名方法见3.2 节
            //商品描述body 是String(127) 商品描述
            //附加数据attach 否String(127) 附加数据，原样返回
            //商户订单号out_trade_no 是String(32) 商户系统内部的订单号,32个字符内、可包含字母,确保在商户系统唯一,详细说明

            //总金额total_fee 是Int 订单总金额，单位为分，不能带小数点
            //终端IP spbill_create_ip 是String(16) 订单生成的机器IP
            //交易起始时间time_start 否String(14) 订单生成时间，格式为yyyyMMddHHmmss，如2009 年12 月25 日9 点10 分10 秒表示为20091225091010。时区
            //为GMT+8 beijing。该时间取自商户服务器
            //交易结束时间time_expire 否String(14) 订单失效时间，格式为
            //yyyyMMddHHmmss，如2009 年
            //12 月27 日9 点10 分10 秒表
            //示为20091227091010。时区
            //为GMT+8 beijing。该时间取
            //自商户服务器
            //商品标记goods_tag 否String(32) 商品标记，该字段不能随便
            //填，不使用请填空，使用说
            //明详见第5 节
            //通知地址notify_url 是String(256) 接收微信支付成功通知
            //交易类型trade_type 是String(16) JSAPI、NATIVE、APP
            //用户标识openid 否String(128) 用户在商户appid 下的唯一标识，trade_type 为JSAPI时，此参数必传，获取方式
            //见表头说明。
            //商品ID product_id 否String(32) 只在trade_type 为NATIVE时需要填写。此id 为二维码中包含的商品ID，商户自行维护。


            var data = @"<xml>
<appid>wx2421b1c4370ec43b</appid>
<attach><![CDATA[att1]]></attach>
<body><![CDATA[JSAPI ݷјࡻણ]]></body>
<device_info>1000</device_info>
<mch_id>10000100</mch_id>
<nonce_str>b927722419c52622651a871d1d9ed8b2</nonce_str>
<notify_url>http://wxpay.weixin.qq.com/pub_v2/pay/notify.php</notify_url>
<out_trade_no>1405713376</out_trade_no>
<spbill_create_ip>127.0.0.1</spbill_create_ip>
<total_fee>1</total_fee>
<trade_type>JSAPI</trade_type>
<sign><![CDATA[3CA89B5870F944736C657979192E1CF4]]></sign>
</xml>
";

            var unifiedorder = "https://api.mch.weixin.qq.com/pay/unifiedorder";
            var webclient = new WebClient();
            webclient.Encoding = System.Text.Encoding.UTF8;
            var nonce_str = Guid.NewGuid().ToString().Replace("-", "");
            //var data = "appid=wxa22b9cfd8fdec01a&body=Test&mch_id=1220064601&nonce_str=9TovdI7pNvAm0cHhDkPQrWSa7oVu1zPC&notify_url=weixininter.lanfans.com&openid=o4mqUjjabJss5MJ_fZNQqKnDZ9S8&out_trade_no=9TovdI7pNvAm0cHhDkPQrWSa7oVu1zP&spbill_create_ip=127.0.0.1&time_start=20150211112233&total_fee=3880&trade_type=JSAPI";
            //var sign = GetMD5(data, "utf-8");
            //data = data + "&sign=" + sign;
            var str = webclient.UploadString(unifiedorder, "post", data);
            Console.Out.WriteLine(str);
        }

        public static string GetMD5(string encypStr, string charset)
        {
            string retStr;
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

            //创建md5对象
            byte[] inputBye;
            byte[] outputBye;

            //使用GB2312编码方式把字符串转化为字节数组．
            try
            {
                inputBye = Encoding.GetEncoding(charset).GetBytes(encypStr);
            }
            catch (Exception ex)
            {
                inputBye = Encoding.GetEncoding("GB2312").GetBytes(encypStr);
            }
            outputBye = m5.ComputeHash(inputBye);

            retStr = System.BitConverter.ToString(outputBye);
            retStr = retStr.Replace("-", "").ToUpper();
            return retStr;
        }
    }
}
