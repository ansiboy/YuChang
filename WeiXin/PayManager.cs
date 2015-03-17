using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using tenpayApp;

namespace YuChang.Core
{
    public class PayManager
    {
        private AccessToken accessToken;
        private string partnerId;
        private string partnerKey;
        public PayManager(AccessToken accessToken, string partnerId, string partnerKey)
        {
            this.accessToken = accessToken;
            this.partnerId = partnerId;
            this.partnerKey = partnerKey;
        }
        public string CreateUnifiedorderByJSAPI(uint total_fee, string openid, string notify_url, string out_trade_no, string spbill_create_ip, string body)
        {
            string appId = this.accessToken.AppId;
            string parameterValue = this.partnerId;
            string noncestr = TenpayUtil.getNoncestr();
            string parameterValue2 = "JSAPI";
            RequestHandler requestHandler = new RequestHandler(Encoding.UTF8);
            requestHandler.setKey(this.partnerKey);
            requestHandler.setParameter("appid", appId);
            requestHandler.setParameter("body", body);
            requestHandler.setParameter("mch_id", parameterValue);
            requestHandler.setParameter("nonce_str", noncestr);
            requestHandler.setParameter("notify_url", notify_url);
            requestHandler.setParameter("openid", openid);
            requestHandler.setParameter("out_trade_no", out_trade_no);
            requestHandler.setParameter("spbill_create_ip", spbill_create_ip);
            requestHandler.setParameter("total_fee", total_fee.ToString());
            requestHandler.setParameter("trade_type", parameterValue2);
            string text = requestHandler.createMd5Sign();
            string data = requestHandler.parseXML();
            string address = "https://api.mch.weixin.qq.com/pay/unifiedorder";
            string xml = new WebClient
            {
                Encoding = Encoding.UTF8
            }.UploadString(address, "post", data);
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            XmlElement[] source = xmlDocument.FirstChild.ChildNodes.Cast<XmlElement>().ToArray<XmlElement>();
            string innerText = source.Single((XmlElement o) => o.Name == "return_code").InnerText;
            if (!(innerText != "SUCCESS"))
            {
                return source.Single((XmlElement o) => o.Name == "prepay_id").InnerText;
            }
            //string text2 = (
            //    from o in source
            //    where o.Name == "err_code"
            //    select o.InnerText).SingleOrDefault<string>();
            string text3 = (
                from o in source
                where o.Name == "err_code_des"
                select o.InnerText).SingleOrDefault<string>();
            if (text2 != null && text3 != null)
            {
                throw Error.WeiXinError(text2, text3);
            }
            string msg = (
                from o in source
                where o.Name == "return_msg"
                select o.InnerText).SingleOrDefault<string>() ?? "ERROR";
            throw Error.WeiXinError(innerText, msg);
        }
    }
}
