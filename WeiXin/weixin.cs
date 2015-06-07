using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using YuChang.Core.Models;

namespace YuChang.Core
{
    /// <summary>
    /// 二维码类型
    /// </summary>
    public enum ActionName
    {
        /// <summary>
        /// 临时二维码类型
        /// </summary>
        QR_SCENE,

        /// <summary>
        /// 永久二维码类型
        /// </summary>
        QR_LIMIT_SCENE
    }

    public enum ButtonType
    {
        click,
        view,
        scancode_waitmsg,
        scancode_push,
        pic_sysphoto,
        pic_photo_or_album,
        pic_weixin,
        location_select
    }

    public class Button
    {
        public ButtonType type;
        public string name;
        public string key;
        public string url;
        public Button[] sub_button;
    }

    public static class weixin
    {
        static Encoding DefaultEncoding = Encoding.UTF8;

        internal static string Serialize(object obj)
        {
            var str = JsonConvert.SerializeObject(obj, new StringEnumConverter());
            return str;
        }

        internal static string GetJson(string url)
        {
            return GetJson(url, null as Dictionary<string, string>);
        }

        internal static string GetJson(string url, string value)
        {
            if (string.IsNullOrEmpty(url))
                throw Error.ArugmentNull("url");

            if (string.IsNullOrEmpty(value))
                throw Error.ArugmentNull("value");

            if (!url.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase))
                url = Constants.RequestRoot + url;

            var client = new WebClient();
            client.Encoding = DefaultEncoding;
            var str = client.UploadString(url, "post", value);
            return str;
        }

        internal static string GetJson(string url, Dictionary<string, string> values)
        {
            if (!url.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase))
                url = Constants.RequestRoot + url;

            if (values == null)
            {
                var client = new WebClient();
                client.Encoding = DefaultEncoding;
                var str = client.DownloadString(url);
                return str;
            }
            else
            {
                var c = new NameValueCollection();
                foreach (string key in values.Keys)
                {
                    c[key] = values[key];
                }

                var client = new WebClient();
                client.Encoding = DefaultEncoding;
                var bytes = client.UploadValues(url, "post", c);

                var str = System.Text.Encoding.UTF8.GetString(bytes);
                return str;
            }
        }

        internal static T Call<T>(string url, T anonymousObj) where T : class
        {
            return Call<T>(url, anonymousObj, null);
        }

        internal static T Call<T>(string url, T anonymousObj, object obj) where T : class
        {
            if (string.IsNullOrEmpty(url))
                throw Error.ArugmentNull("url");

            string json;
            if (obj != null)
            {
                var data = Serialize(obj);
                json = GetJson(url, data);
            }
            else
            {
                json = GetJson(url);
            }

            return Deserialize<T>(json, anonymousObj);
        }

        internal static T Deserialize<T>(string json, T anonymousObj) where T : class
        {
            if (json.IndexOf("errcode") > 0)
            {
                var serail = new System.Web.Script.Serialization.JavaScriptSerializer();
                var dic = serail.Deserialize<Dictionary<string, object>>(json);
                object errorCode;
                if (dic.TryGetValue("errcode", out errorCode) && (int)errorCode != 0)
                {
                    var code = (int)errorCode;
                    var msg = (string)dic["errmsg"];
                    throw Error.WeiXinError(code, msg);
                }
            }

            var obj = JsonConvert.DeserializeAnonymousType(json, anonymousObj);
            return obj;

        }

        public static AccessToken token(string appid, string secret)
        {
            return AccessTokenPool.GetAccessToken(appid, secret);
        }

        public static string[] getcallbackip(AccessToken token)
        {
            var path = "getcallbackip?access_token=" + token;
            var result = Call(path, new { ip_list = new string[] { } });
            return result.ip_list;
        }



        public static class menu
        {
            public static void create(AccessToken token, Button[] button)
            {
                var url = string.Format("menu/create?access_token={0}", token);
                weixin.Call(url, new { errcode = "" }, new { button });
            }

            public static Button[] get(AccessToken token)
            {
                var url = "menu/get?access_token=" + token;
                var result = weixin.Call(url, new { menu = new { button = new Button[] { } } });
                return result.menu.button;
            }

            public static void delete(AccessToken token)
            {
                var url = "menu/delete?access_token=" + token;
                weixin.Call(url, new { errcode = "" });
            }
        }

        public static class qrcode
        {
            //enum ActionName
            //{
            //    QR_SCENE,
            //    QR_LIMIT_SCENE
            //}

            public class CreateResult
            {
                public string ticket;
                public int expire_seconds;
                public string url;
            }

            public static CreateResult create(AccessToken token, ActionName action_name, int scene_id)
            {
                var path = "qrcode/create?access_token=" + token;
                object data;
                if (action_name == ActionName.QR_SCENE)
                {
                    data = new
                    {
                        expire_seconds = 604800,
                        action_name = action_name.ToString(),
                        action_info = new
                        {
                            scene = new { scene_id }
                        }
                    };
                }
                else
                {
                    data = new
                    {
                        action_name = action_name.ToString(),
                        action_info = new
                        {
                            scene = new { scene_id }
                        }
                    };
                }

                var obj = Call<CreateResult>(path, new CreateResult(), data);
                return obj;
            }
        }

        public static class template
        {
            public static void api_set_industry(AccessToken token, string industry_id1, string industry_id2)
            {
                var url = "template/api_set_industry?access_token=" + token;
                Call(url, new { errcode = "" }, new { industry_id1, industry_id2 });
            }

            public static string api_add_template(AccessToken token, string template_id_short)
            {
                var url = "message/template/send?access_token=" + token;
                var obj = Call(url, new { errcode = "", template_id = "" });
                return obj.template_id;
            }
        }

        public static class ticket
        {
            public class getticket_result
            {
                public int errcode;
                public string errmsg;
                public string ticket;
                public int expires_in;
            }

            public static getticket_result getticket(AccessToken token)
            {
                var url = "ticket/getticket?type=wx_card&access_token=" + token;
                return Call(url, new getticket_result());
            }
        }

        public static class message
        {
            public enum MessageType
            {
                mpnews,
                text,
                voice,
                image,
                mpvideo
            }

            public static class template
            {

                /// <summary>
                /// 发送模板消息
                /// </summary>
                /// <param name="token"></param>
                /// <param name="touser">接收模板消息的用户的openid</param>
                /// <param name="template">模版消息对象</param>
                /// <param name="url">模版消息的链接</param>
                /// <returns></returns>
                public static string send(AccessToken token, string touser, Template template, string url = null)
                {
                    if (token == null)
                        throw Error.ArugmentNull("token");

                    if (string.IsNullOrEmpty(touser))
                        throw Error.ArugmentNull("touser");

                    if (template == null)
                        throw Error.ArugmentNull("template");

                    if (url == null)
                        url = "";

                    var template_id = template.TemplateId;
                    var topcolor = template.TopColor;

                    var data = new Dictionary<string, object>();
                    foreach (var field in template.Fields)
                    {
                        var item = new Dictionary<string, string>();
                        item["value"] = field.Value;
                        item["color"] = field.Color;

                        data[field.Name] = item;
                    }

                    var path = "message/template/send?access_token=" + token;
                    var obj = Call(path, new { errcode = "", errmsg = "", msgid = "" }, new { touser, template_id, url, topcolor, data });
                    return obj.msgid;
                }
            }

            public static class mass
            {
                /// <summary>
                /// 根据分组进行群发【订阅号与服务号认证后均可用】
                /// </summary>
                public static string sendall(AccessToken token, string mediaIdOrContent, MessageType msgtype, string group_id)
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

                    var url = "message/mass/sendall?access_token=" + token;
                    var json = weixin.GetJson(url, data);
                    var dic = weixin.Deserialize(json, new { msg_id = "" });
                    return dic.msg_id;
                }

                /// <summary>
                /// 根据OpenID列表群发【订阅号不可用，服务号认证后可用】
                /// </summary>
                public static string send(AccessToken token, string mediaIdOrContent, MessageType msgtype, string[] touser)
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

                    var url = "message/mass/send?access_token=" + token;
                    var json = weixin.GetJson(url, data);
                    var dic = weixin.Deserialize(json, new { msg_id = "" });
                    return (string)dic.msg_id;
                }

                /// <summary>
                /// 删除群发【订阅号与服务号认证后均可用】
                /// </summary>
                /// <param name="msg_Id"></param>
                public static void delete(AccessToken token, string msg_Id)
                {
                    var url = "message/mass/send?access_token=" + token;
                    Call(url, new { errorcode = "", errmsg = "" });
                }

                /// <summary>
                /// 预览接口【订阅号与服务号认证后均可用】
                /// </summary>
                public static void preview(AccessToken token, string touser, string mediaIdOrContent, MessageType msgtype)
                {
                    var url = "message/mass/preview?access_token=" + token;
                    var obj_result = new { errcode = "", errmsg = "", msg_id = "" };
                    switch (msgtype)
                    {
                        case MessageType.mpnews:
                            obj_result = Call(url, obj_result, new { touser, mpnews = new { media_id = mediaIdOrContent }, msgtype });
                            break;
                        case MessageType.text:
                            obj_result = Call(url, obj_result, new { touser, text = new { content = mediaIdOrContent, msgtype } });
                            break;
                        case MessageType.voice:
                            obj_result = Call(url, obj_result, new { touser, voice = new { media_id = mediaIdOrContent }, msgtype });
                            break;
                        case MessageType.image:
                            obj_result = Call(url, obj_result, new { touser, image = new { media_id = mediaIdOrContent }, msgtype });
                            break;
                        case MessageType.mpvideo:
                            obj_result = Call(url, obj_result, new { touser, mpvideo = new { media_id = mediaIdOrContent }, msgtype });
                            break;
                        default:
                            var msg = string.Format("'{0}' type is not supported.");
                            throw Error.ArugmentError(msg);
                    }
                }

                /// <summary>
                /// 查询群发消息发送状态【订阅号与服务号认证后均可用】
                /// </summary>
                /// <param name="msg_id">群发消息后返回的消息id</param>
                /// <returns>消息发送后的状态，SEND_SUCCESS表示发送成功</returns>
                public static string get(AccessToken token, string msg_id)
                {
                    var url = "message/mass/get?access_token=" + token;
                    var result = new { msg_status = "" };
                    result = Call(url, result, new { msg_id });
                    return result.msg_status;
                }
            }
        }

        public class Article
        {
            /// <summary>
            /// 图文消息的封面图片素材id（必须是永久mediaID）
            /// </summary>
            public string thumb_media_id;
            /// <summary>
            /// 作者
            /// </summary>
            public string author;
            /// <summary>
            /// 标题
            /// </summary>
            public string title;
            /// <summary>
            /// 图文消息的原文地址，即点击“阅读原文”后的URL
            /// </summary>
            public string content_source_url;
            /// <summary>
            /// 图文消息的具体内容，支持HTML标签，必须少于2万字符，小于1M，且此处会去除JS
            /// </summary>
            public string content;
            /// <summary>
            /// 图文消息的摘要，仅有单图文消息才有摘要，多图文此处为空
            /// </summary>
            public string digest;
            /// <summary>
            /// 是否显示封面，0为false，即不显示，1为true，即显示
            /// </summary>
            public string show_cover_pic;
        }

        public static class media
        {
            public class UploadResult
            {
                public MediaType type;
                public string media_id;
                public DateTime created_at;
            }



            public static UploadResult uploadnews(AccessToken token, Article[] articles)
            {
                if (articles == null)
                    throw Error.ArugmentNull("articles");

                if (articles.Length == 0)
                {
                    var msg = "Length of the articles argument cannt be zero.";
                    throw Error.ArugmentError(msg);
                }

                var url = "media/uploadnews?access_token=" + token;
                var serial = new System.Web.Script.Serialization.JavaScriptSerializer();
                var data = serial.Serialize(new { articles });
                var json = weixin.GetJson(url, data);

                return weixin.Deserialize(json, new UploadResult());
            }

            /// <summary>
            /// 新增临时素材
            /// </summary>
            /// <param name="access_token">调用接口凭证</param>
            /// <param name="type">媒体文件类型，分别有图片（image）、语音（voice）、视频（video）和缩略图（thumb）</param>
            /// <param name="uploadFilePath">要上传的文件路径</param>
            /// <returns></returns>
            public static UploadResult upload(AccessToken access_token, MediaType type, string uploadFilePath)
            {
                var url = string.Format("http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", access_token, type);
                var webClient = new WebClient();
                var bytes = webClient.UploadFile(url, uploadFilePath);
                var json = System.Text.Encoding.UTF8.GetString(bytes);
                var result = new UploadResult();
                result = Deserialize(json, result);
                return result;
            }

            /// <summary>
            /// 新增永久图文素材
            /// </summary>
            /// <param name="access_token">调用接口凭证</param>
            /// <param name="media_id">媒体文件ID</param>
            /// <param name="saveFilePath">要保存的文件路径</param>
            public static void get(AccessToken access_token, string media_id, string saveFilePath)
            {
                var url = string.Format("media/get?access_token={0}&media_id={1}", access_token, media_id);
                var webClient = new WebClient();
                webClient.DownloadFile(url, saveFilePath);
            }

        }

        public static class material
        {
            /// <summary>
            /// 新增永久素材
            /// </summary>
            /// <param name="access_token">调用接口凭证</param>
            /// <param name="articles">图文素材</param>
            /// <returns>媒体文件ID</returns>
            public static string add_news(AccessToken access_token, Article[] articles)
            {
                var url = "material/add_news?access_token=" + access_token;
                var result = new { media_id = "" };
                result = Call(url, result, new { articles });
                return result.media_id;
            }

            public static string add_material(AccessToken access_token, MediaType type, string uploadFilePath)
            {
                var url = "material/add_material?access_token=" + access_token;
                //var url = string.Format("http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", access_token, type);
                var webClient = new WebClient();
                var bytes = webClient.UploadFile(url, uploadFilePath);
                var json = System.Text.Encoding.UTF8.GetString(bytes);
                var result = new { media_id = "" };
                result = Deserialize(json, result);
                return result.media_id;
            }
        }

        public static class user
        {
            public class Data
            {
                public string[] openid;
            }

            public class UserGetResult
            {
                public int total;
                public int count;
                public Data data;
                public string next_openid;
            }

            public static UserGetResult get(AccessToken token, string next_openid)
            {
                var url = string.Format("user/get?access_token={0}&next_openid={1}", token, next_openid);
                var json = weixin.GetJson(url);
                var result = new UserGetResult();
                result = Deserialize(json, result);
                return result;
            }
        }

    }
}
