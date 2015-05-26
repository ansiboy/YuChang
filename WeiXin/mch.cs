using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using tenpayApp;

namespace YuChang.Core
{
    public class mch
    {
        public static class pay
        {
            public static string unifiedorder(string appid, string mch_id, string openid, string key, string body, string notify_url,
                                       string out_trade_no, int total_fee)
            {
                //string appId = this.accessToken.AppId;
                //string parameterValue = this.partnerId;
                var spbill_create_ip = "127.0.0.1";
                string noncestr = TenpayUtil.getNoncestr();
                string parameterValue2 = "JSAPI";
                RequestHandler requestHandler = new RequestHandler(Encoding.UTF8);
                requestHandler.setKey(key);
                requestHandler.setParameter("appid", appid);
                requestHandler.setParameter("body", body);
                requestHandler.setParameter("mch_id", mch_id);
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
                string text2 = (
                    from o in source
                    where o.Name == "err_code"
                    select o.InnerText).SingleOrDefault<string>();
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
}
