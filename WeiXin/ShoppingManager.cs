using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using tenpayApp;

namespace YuChang.Core
{
    public class ShoppingManager
    {
        private AccessToken accessToken;
        private string appKey;

        public ShoppingManager(AccessToken accessToken, string appKey)
        {
            this.accessToken = accessToken;
            this.appKey = appKey;
        }

        public ShoppingManager(string appid, string secret, string appKey)
            : this(new AccessToken(appid, secret), appKey)
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

            var serial = new System.Web.Script.Serialization.JavaScriptSerializer();
            var str = serial.Serialize(values);

            var client = new WebClient();
            client.Encoding = Utility.DefaultEncoding;
            var result = client.UploadString(url, "post", str);
            var dic = serial.Deserialize<Dictionary<string, object>>(result);
            var code = (int)dic["errcode"];
            var msg = (string)dic["errmsg"];
            if (code != 0)
                throw Error.WeiXinError(code, msg);

        }

        public static int DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return Convert.ToInt32((dateTime - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds);
        }
    }
}
