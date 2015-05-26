using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using YuChang.Core.Models;
using YuChang.Web.Models;

namespace YuChang.Web
{
    public class WeiXinController : Controller
    {
        public ActionResult UserAuth(string code, string type)
        {
            var appid = ConfigurationManager.AppSettings["appid"];
            if (string.IsNullOrEmpty(appid))
                throw Error.AppSettingItemMiss("appid");

            var secret = ConfigurationManager.AppSettings["secret"];
            if (string.IsNullOrEmpty("secret"))
                throw Error.AppSettingItemMiss("secret");

            if (string.IsNullOrEmpty(type))
                type = "snsapi_base";//snsapi_userinfo

            string url;
            if (string.IsNullOrEmpty(code))
            {
                var redirectUrl = Request.Url.ToString();
                url = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope={2}&state=STATE#wechat_redirect",
                                    appid, HttpUtility.UrlEncode(redirectUrl), type);

                Trace.WriteLine("Go to WeiXin Auth Page:" + url);
                Trace.Flush();

                return Redirect(url);
            }

            var client = new System.Net.WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", appid, secret, code);
            var data = client.DownloadString(url);

            var serializer = new JavaScriptSerializer();
            var obj = serializer.Deserialize<Dictionary<string, object>>(data);

            var userInfo = new UserInfo();
            object fieldValue;
            if (obj.TryGetValue("openid", out fieldValue))
                userInfo.OpenId = (string)fieldValue;

            if (obj.TryGetValue("nickname", out fieldValue))
                userInfo.NickName = (string)fieldValue;

            if (obj.TryGetValue("city", out fieldValue))
                userInfo.City = (string)fieldValue;

            if (obj.TryGetValue("country", out fieldValue))
                userInfo.Country = (string)fieldValue;

            if (obj.TryGetValue("province", out fieldValue))
                userInfo.Province = (string)fieldValue;

            if (obj.TryGetValue("language", out fieldValue))
                userInfo.Language = (string)fieldValue;

            if (obj.TryGetValue("headimgurl", out fieldValue))
                userInfo.HeadImgUrl = (string)fieldValue;

            if (obj.TryGetValue("sex", out fieldValue))
                userInfo.Sex = ((int)fieldValue) == 1 ? Gender.Male : (((int)fieldValue) == 2 ? Gender.Female : Gender.Unknown);

            if (obj.TryGetValue("privilege", out fieldValue))
                userInfo.Privilege = (string)fieldValue;

            if (obj.TryGetValue("unionid", out fieldValue))
                userInfo.Unionid = (string)fieldValue;

            return UserAuthorised(userInfo);
        }

        protected virtual ActionResult UserAuthorised(UserInfo userInfo)
        {
            return Json(userInfo, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Paid()
        {
            //============================================================
            // 说明：这个接口是由外部系统调用，GetApplicationId 和 DataSource 不起作用，
            // 在这里不能直接调用这两个方法。
            //============================================================
            Trace.WriteLine("ShoutTaoWeiXinPurchase at " + DateTime.Now);

            var orgId = Guid.Parse("7BBFA36C-8115-47AD-8D47-9E52B58E7EFD");

            Stream s = Request.InputStream;
            byte[] b = new byte[s.Length];
            s.Read(b, 0, (int)s.Length);
            //<xml><OpenId><![CDATA[o1Ux1uHZkxsST2_8Fiy_dJfziqbQ]]></OpenId><AppId><![CDATA[wxa22b9cfd8fdec01a]]></AppId><IsSubscribe>1</IsSubscribe><TimeStamp>1407418465</TimeStamp><NonceStr><![CDATA[3ZMKJpeLwA8IpMrQ]]></NonceStr><AppSignature><![CDATA[ad2f0146edf5b38248f8007e1fbdba7900d38feb]]></AppSignature><SignMethod><![CDATA[sha1]]></SignMethod></xml>
            var postStr = Encoding.UTF8.GetString(b);
            Trace.WriteLine(postStr);

            //==========================================
            // For Debug
            //                postStr = @"<xml><appid><![CDATA[wxe621f4e5e90b13cd]]></appid>
            //<bank_type><![CDATA[ICBC_DEBIT]]></bank_type>
            //<cash_fee><![CDATA[10]]></cash_fee>
            //<fee_type><![CDATA[CNY]]></fee_type>
            //<is_subscribe><![CDATA[Y]]></is_subscribe>
            //<mch_id><![CDATA[10035475]]></mch_id>
            //<nonce_str><![CDATA[51EF186E18DC00C2D31982567235C559]]></nonce_str>
            //<openid><![CDATA[o4mqUjm9hp5qZB0xh0lEkki0SfjU]]></openid>
            //<out_trade_no><![CDATA[1334a94f104a4106a67e43a6be564bf6]]></out_trade_no>
            //<result_code><![CDATA[SUCCESS]]></result_code>
            //<return_code><![CDATA[SUCCESS]]></return_code>
            //<sign><![CDATA[9B27F40620358ECCFB23041418F132CF]]></sign>
            //<time_end><![CDATA[20150302182000]]></time_end>
            //<total_fee>10</total_fee>
            //<trade_type><![CDATA[JSAPI]]></trade_type>
            //<transaction_id><![CDATA[1010010275201503020023832476]]></transaction_id>
            //</xml>";

            var doc = XDocument.Parse(postStr);
            var xml = doc.Root;
            var openId = xml.Elements("openid").Single().Value;
            var appId = xml.Element("appid").Value;
            var isSubscribe = xml.Element("is_subscribe").Value;
            var timeStamp = xml.Element("time_end").Value;
            var year = Convert.ToInt32(timeStamp.Substring(0, 4));
            var month = Convert.ToInt32(timeStamp.Substring(4, 2));
            var day = Convert.ToInt32(timeStamp.Substring(6, 2));

            var nonceStr = xml.Element("nonce_str").Value;
            var appSignature = xml.Element("sign").Value;
            var total_fee = Convert.ToInt32(xml.Element("total_fee").Value);
            var out_trade_no = xml.Element("out_trade_no").Value;
            var transaction_id = xml.Element("transaction_id").Value;
            var bank_type = xml.Element("bank_type").Value;
            var cash_fee = Convert.ToInt32(xml.Element("cash_fee").Value);
            var fee_type = xml.Element("fee_type").Value;
            var mch_id = xml.Element("mch_id").Value;

            var payment = new Payment
            {
                AppId = appId,
                AppSignature = appSignature,
                IsSubscribe = isSubscribe == "1",
                NonceStr = nonceStr,
                OpenId = openId,
                OutTradeNO = out_trade_no,
                TimeEnd = new DateTime(year, month, day),
                TotalFee = total_fee,
                TransactionId = transaction_id,
                BankType = bank_type,
                CashFee = cash_fee,
                FeeType = fee_type,
                MchId = mch_id
            };

            OnPaid(payment);
            Trace.WriteLine("Order purchse success.");
            var reslt =
@"<xml>
   <return_code><![CDATA[SUCCESS]]></return_code>
   <return_msg><![CDATA[OK]]></return_msg>
</xml>";
            return Content(reslt);
        }

        public void OnPaid(Payment payment)
        {

        }

        public ActionResult ProcessMessage()
        {
            if (Request.HttpMethod.ToLower() == "get")
            {
                string echostr = Request.QueryString["echostr"];
                if (!String.IsNullOrEmpty(echostr))
                    return Content(echostr);

                return Content("Invalid Request");
            }

            Stream s = System.Web.HttpContext.Current.Request.InputStream;
            byte[] b = new byte[s.Length];
            s.Read(b, 0, (int)s.Length);
            var postStr = Encoding.UTF8.GetString(b);
            if (string.IsNullOrEmpty(postStr)) //请求处理
                return Content("");

            string reply = "";
            var msg = Message.FromXml(postStr);
            switch (msg.MsgType)
            {
                case MessageType.Event:
                    var eventType = ((EventMessage)msg).Event;
                    switch (eventType)
                    {
                        case EventType.Click:
                            reply = ProcessClickEvent((ClickEvent)msg);
                            break;
                        case EventType.Location:
                            reply = ProcessLocationEvent((LocationEvent)msg);
                            break;
                        case EventType.Scan:
                            reply = ProcessScanEvent((ScanEvent)msg);
                            break;
                        case EventType.Subscribe:
                            reply = ProcessSubscribeEvent((SubscribeEvent)msg);
                            break;
                        case EventType.Unsubscribe:
                            reply = ProcessUnsubscribeEvent((UnsubscribeEvent)msg);
                            break;
                        case EventType.View:
                            reply = ProcessViewEvent((ViewEvent)msg);
                            break;
                        case EventType.TemplateSendJobFinish:
                            reply = ProcessTemplateSendJobFinishEvent((TemplateSendJobFinishEvent)msg);
                            break;
                    }
                    break;
                case MessageType.Image:
                    reply = ProcessImageMessage((ImageMessage)msg);
                    break;
                case MessageType.Link:
                    reply = ProcessLinkMessage((LinkMessage)msg);
                    break;
                case MessageType.Location:
                    reply = ProcessLocationMessage((LocationMessage)msg);
                    break;
                case MessageType.Text:
                    reply = ProcessTextMessage((TextMessage)msg);
                    break;
                case MessageType.Video:
                    reply = ProcessVideoMessage((VideoMessage)msg);
                    break;
                case MessageType.Voice:
                    reply = ProcessVoiceMessage((VoiceMessage)msg);
                    break;

            }

            return Content(reply);
        }

        private string ProcessTemplateSendJobFinishEvent(TemplateSendJobFinishEvent msg)
        {
            return DefaultProcess(msg);
        }

        protected virtual string ProcessViewEvent(ViewEvent msg)
        {
            return DefaultProcess(msg);
        }

        protected virtual string ProcessClickEvent(ClickEvent msg)
        {
            return DefaultProcess(msg);
        }

        protected virtual string ProcessLocationEvent(LocationEvent msg)
        {
            return DefaultProcess(msg);
        }

        protected virtual string ProcessScanEvent(ScanEvent msg)
        {
            return DefaultProcess(msg);
        }

        protected virtual string ProcessSubscribeEvent(SubscribeEvent msg)
        {
            return DefaultProcess(msg);
        }

        protected virtual string ProcessUnsubscribeEvent(UnsubscribeEvent msg)
        {
            return DefaultProcess(msg);
        }

        protected virtual string ProcessImageMessage(ImageMessage msg)
        {
            return DefaultProcess(msg);
        }

        protected virtual string ProcessLinkMessage(LinkMessage msg)
        {
            return DefaultProcess(msg);
        }

        protected virtual string ProcessLocationMessage(LocationMessage msg)
        {
            return DefaultProcess(msg);
        }

        protected virtual string ProcessTextMessage(TextMessage msg)
        {
            return DefaultProcess(msg);
        }

        protected virtual string ProcessVideoMessage(VideoMessage msg)
        {
            return DefaultProcess(msg);
        }

        protected virtual string ProcessVoiceMessage(VoiceMessage msg)
        {
            return DefaultProcess(msg);
        }

        protected virtual string DefaultProcess(Message msg)
        {
            var reply = new TextMessage();
            reply.ToUserName = msg.FromUserName;
            reply.FromUserName = msg.ToUserName;
            if (msg.MsgType == MessageType.Event)
            {
                reply.Content = string.Format("{0} event is not processed.", ((EventMessage)msg).Event);
            }
            else
            {
                reply.Content = string.Format("{0} message is not processed.", msg.MsgType);
            }

            return reply.ToXml();
        }


    }
}
