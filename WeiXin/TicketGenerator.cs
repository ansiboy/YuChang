using System;
using System.Collections.Generic;
namespace YuChang.Core
{
    //public class TicketGenerator
    //{
    //    private access_token accessToken;
    //    public TicketGenerator(access_token accessToken)
    //    {
    //        this.accessToken = accessToken;
    //    }
    //    public string GetJsapiTicket()
    //    {
    //        string url = string.Format("ticket/getticket?access_token={0}&type=jsapi", this.accessToken);
    //        Dictionary<string, object> weiXinJson = Utility.GetWeiXinJson(url);
    //        object errorCode;
    //        if (weiXinJson.TryGetValue("errcode", out errorCode) && (int)errorCode != 0)
    //        {
    //            var code = (int)errorCode;
    //            var msg = (string)weiXinJson["errmsg"];
    //            throw Error.WeiXinError(code, msg);
    //        }

    //        return (string)weiXinJson["ticket"];
    //    }
    //}
}
