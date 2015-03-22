using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuChang.Core
{
    partial class WeiXinService
    {
        public class MassService
        {
            private WeiXinService weixin;
            internal MassService(WeiXinService weixin)
            {
                this.weixin = weixin;
            }

            /// <summary>
            /// 根据分组进行群发【订阅号与服务号认证后均可用】
            /// </summary>
            public string sendall(string mediaIdOrContent, MessageType msgtype, string group_id)
            {
                object filter;
                if (!string.IsNullOrEmpty(group_id))
                {
                    filter = new { is_to_all = false, group_id };
                }
                else
                {
                    filter = new { is_to_all = true };
                }

                object obj;
                switch (msgtype)
                {
                    case MessageType.mpnews:
                        obj = new
                        {
                            filter,
                            mpnews = new { media_id = mediaIdOrContent },
                            msgtype
                        };
                        break;
                    case MessageType.text:
                        obj = new
                        {
                            filter,
                            text = new { content = mediaIdOrContent },
                            msgtype
                        };
                        break;
                    case MessageType.voice:
                        obj = new
                        {
                            filter,
                            voice = new { media_id = mediaIdOrContent },
                            msgtype
                        };
                        break;
                    case MessageType.image:
                        obj = new
                        {
                            filter,
                            image = new { media_id = mediaIdOrContent },
                            msgtype
                        };
                        break;
                    default:
                        var msg = string.Format("'{0}' is a unknown media type", msgtype);
                        throw Error.ArugmentNull(msg);
                }

                var serial = new System.Web.Script.Serialization.JavaScriptSerializer();

                var data = serial.Serialize(obj);

                var url = "message/mass/sendall?access_token=" + weixin.token();
                var json = weixin.GetJson(url, data);
                var dic = weixin.Deserialize(json, new { msg_id = "" });
                return dic.msg_id;
            }

            /// <summary>
            /// 根据OpenID列表群发【订阅号不可用，服务号认证后可用】
            /// </summary>
            public string send(string mediaIdOrContent, MessageType msgtype, string[] touser)
            {
                if (string.IsNullOrEmpty(mediaIdOrContent))
                    throw Error.ArugmentNull("mediaIdOrContent");

                if (touser == null)
                    throw Error.ArugmentNull("touser");

                if (touser.Length == 0)
                {
                    var msg = "Length of the touser argument cannt be zero.";
                    throw Error.ArugmentError(msg);
                }

                object obj;
                switch (msgtype)
                {
                    case MessageType.mpnews:
                        obj = new
                        {
                            touser,
                            mpnews = new { media_id = mediaIdOrContent },
                            msgtype
                        };
                        break;
                    case MessageType.text:
                        obj = new
                        {
                            touser,
                            text = new { content = mediaIdOrContent },
                            msgtype
                        };
                        break;
                    case MessageType.voice:
                        obj = new
                        {
                            touser,
                            voice = new { media_id = mediaIdOrContent },
                            msgtype
                        };
                        break;
                    case MessageType.image:
                        obj = new
                        {
                            touser,
                            image = new { media_id = mediaIdOrContent },
                            msgtype
                        };
                        break;
                    default:
                        var msg = string.Format("'{0}' is a unknown media type", msgtype);
                        throw Error.ArugmentNull(msg);
                }

                var serial = new System.Web.Script.Serialization.JavaScriptSerializer();

                var data = serial.Serialize(obj);

                var url = "message/mass/send?access_token=" + weixin.token();
                var json = weixin.GetJson(url, data);
                var dic = weixin.Deserialize(json, new { msg_id = "" });
                return (string)dic.msg_id;
            }

            public void delete(string msg_Id)
            {
                var url = "message/mass/send?access_token=" + weixin.token();
                var json = weixin.GetJson(url);
                var dic = weixin.Deserialize(json, new { errorcode = "" });
            }
        }
    }

}
