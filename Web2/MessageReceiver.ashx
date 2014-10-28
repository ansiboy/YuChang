<%@ WebHandler Language="C#" Class="WeiXin.MessageReceiver" %>
#define TRACE

using System.Web;

namespace WeiXin
{
    public class MessageReceiver : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            const string TOKEN = "5201314";
            const string appid = "wxe621f4e5e90b13cd";
            const string secret = "7cb56e5b6bd302ddb73c4f76a9ec26a2";
            var handler = new HttpHandler(TOKEN, new ShouTaoMessageProcesser(appid, secret));
            handler.Process(context.Request, context.Response);
        }

        public virtual bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}