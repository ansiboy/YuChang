using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using tenpayApp;

namespace YuChang.Core
{
    public class ShoppingManager
    {
        private access_token accessToken;
        private string appKey;

        public ShoppingManager(access_token accessToken, string appKey)
        {
            this.accessToken = accessToken;
            this.appKey = appKey;
        }

        public ShoppingManager(string appid, string secret, string appKey)
            : this(new access_token(appid, secret), appKey)
        {

        }

        /// <summary>
        /// 发货，告知微信后台该订单处于发货状态。
        /// </summary>
        /// <param name="openid">购买用户的 OpenId，已经放在最终支付结果通知的 PostData 里了。</param>
        /// <param name="transid">交易单号</param>
        /// <param name="outTradeNO">第三方订单号</param>
        public void Deliver(string openid, string transid, string outTradeNO)
        {
            var paySignReqHandler = new RequestHandler(Utility.DefaultEncoding);

            var timeStamp = DateTimeToUnixTimestamp(DateTime.Now);
            paySignReqHandler.setParameter("appid", this.accessToken.AppId);
            paySignReqHandler.setParameter("appkey", this.appKey);
            paySignReqHandler.setParameter("openid", openid);
            paySignReqHandler.setParameter("transid", transid);
            paySignReqHandler.setParameter("out_trade_no", outTradeNO);
            paySignReqHandler.setParameter("deliver_timestamp", timeStamp.ToString());
            paySignReqHandler.setParameter("deliver_status", "1");
            paySignReqHandler.setParameter("deliver_msg", "ok");
            var paySign = paySignReqHandler.createSHA1Sign();

            var url = "https://api.weixin.qq.com/pay/delivernotify?access_token=" + accessToken;
            var values = new Dictionary<string, string>();
            values.Add("appid", this.accessToken.AppId);
            values.Add("openid", openid);
            values.Add("transid", transid);
            values.Add("out_trade_no", outTradeNO);
            values.Add("deliver_timestamp", timeStamp.ToString());
            values.Add("deliver_status", "1");
            values.Add("deliver_msg", "ok");
            values.Add("app_signature", paySign);
            values.Add("sign_method", "sha1");

            var str = Utility.Serialize(values);

            var client = new WebClient();
            client.Encoding = Utility.DefaultEncoding;
            var result = client.UploadString(url, "post", str);
            var dic = Utility.Deserialize<Dictionary<string, object>>(result);
            var code = (int)dic["errcode"];
            var msg = (string)dic["errmsg"];
            if (code != 0)
                throw Error.WeiXinError(code, msg);

        }

        public Dictionary<string, object> QueryOrder(string partnerId, string partnerKey, string outTradeNO)
        {
            var str = string.Format("out_trade_no={0}&partner={1}&key={2}", outTradeNO, partnerId, partnerKey);
            var sign = MD5Encoding(str).ToUpper();
            var package = string.Format("out_trade_no={0}&partner={1}&sign={2}", outTradeNO, partnerId, sign);
            var timeStamp = DateTimeToUnixTimestamp(DateTime.Now).ToString();

            var paySignReqHandler = new RequestHandler(Utility.DefaultEncoding);
            paySignReqHandler.setParameter("appid", this.accessToken.AppId);
            paySignReqHandler.setParameter("appkey", this.appKey);
            paySignReqHandler.setParameter("package", package);
            paySignReqHandler.setParameter("timestamp", timeStamp);
            var app_signature = paySignReqHandler.createSHA1Sign();

            var values = new Dictionary<string, string>();
            values.Add("appid", this.accessToken.AppId);
            values.Add("package", package);
            values.Add("timestamp", timeStamp);
            values.Add("app_signature", app_signature);
            values.Add("sign_method", "sha1");

            var url = "https://api.weixin.qq.com/pay/orderquery?access_token=" + accessToken;
            //var serial = new System.Web.Script.Serialization.JavaScriptSerializer();

            str = Utility.Serialize(values);
            var client = new WebClient();
            client.Encoding = Utility.DefaultEncoding;
            var result = client.UploadString(url, "post", str);
            var dic = Utility.Deserialize<Dictionary<string, object>>(result);
            var code = (int)dic["errcode"];
            var msg = (string)dic["errmsg"];
            if (code != 0)
                throw Error.WeiXinError(code, msg);

            return dic;
        }

        public Dictionary<string, object> MCHDonw(string partnerId, string partnerKey, DateTime transTime)
        {
            var trans_time = transTime.ToString("yyyy-MM-dd");
            var stamp = DateTimeToUnixTimestamp(DateTime.Now).ToString();
            var sign = MD5Encoding(string.Format("spid={0}&trans_time={1}&stamp={2}&key={3}", partnerId, trans_time, stamp, partnerKey));

            var dic = new NameValueCollection();
            dic.Add("spid", partnerId);
            dic.Add("trans_time", trans_time);
            dic.Add("stamp", stamp);
            dic.Add("sign", sign);

            var url = "http://mch.tenpay.com/cgi-bin/mchdown_real_new.cgi";
            var client = new WebClient();
            client.Encoding = Encoding.GetEncoding("gbk"); //Utility.DefaultEncoding;
            var bytes = client.UploadValues(url, "post", dic);
            var xml = client.Encoding.GetString(bytes);
            //<?xml version="1.0" encoding="gb2312" ?> 
            //<root> 
            //  <retcode>0</retcode> 
            //  <retmsg></retmsg> 
            //  <partner>1900000109</partner> 
            //<status>0</status> 
            //<sign>8DB4A013A8B515349C307F1E448CE836</sign> 
            //</root> 

            return null;
        }


        public static int DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return Convert.ToInt32((dateTime - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds);
        }

        static string MD5Encoding(string rawPass)
        {
            // 创建MD5类的默认实例：MD5CryptoServiceProvider
            MD5 md5 = MD5.Create();
            byte[] bs = Encoding.UTF8.GetBytes(rawPass);
            byte[] hs = md5.ComputeHash(bs);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hs)
            {
                // 以十六进制格式格式化
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
