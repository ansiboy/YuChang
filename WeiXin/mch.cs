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
                    var prepay_id_element = source.SingleOrDefault((XmlElement o) => o.Name == "prepay_id");
                    if (prepay_id_element != null)
                        return prepay_id_element.InnerText;

                }

                var err_code = source.Where(o => o.Name == "err_code").Select(o => o.InnerText).SingleOrDefault();
                var err_code_des = source.Where(o => o.Name == "err_code_des").Select(o => o.InnerText).SingleOrDefault();

                if (err_code != null && err_code_des != null)
                    throw Error.WeiXinError(err_code, err_code_des);

                //string msg = (
                //    from o in source
                //    where o.Name == "return_msg"
                //    select o.InnerText).SingleOrDefault<string>() ?? "ERROR";

                var msg = source.Where(o => o.Name == "return_msg").Select(o => o.InnerText).SingleOrDefault() ?? "ERROR";

                throw Error.WeiXinError(innerText, msg);
            }
        }
    }
}
