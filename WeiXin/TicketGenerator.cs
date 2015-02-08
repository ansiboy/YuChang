using System;
using System.Collections.Generic;
namespace YuChang.Core
{
    public class TicketGenerator
    {
        private AccessToken accessToken;
        public TicketGenerator(AccessToken accessToken)
        {
            this.accessToken = accessToken;
        }
        public string GetJsapiTicket()
        {
            string url = string.Format("ticket/getticket?access_token={0}&type=jsapi", this.accessToken);
            Dictionary<string, object> weiXinJson = Utility.GetWeiXinJson(url);
            return (string)weiXinJson["ticket"];
        }
    }
}
