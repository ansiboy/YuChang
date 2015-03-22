using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace YuChang.Core
{
    public partial class WeiXinService
    {
        const string RequestRoot = "https://api.weixin.qq.com/cgi-bin/";
        static Encoding DefaultEncoding = Encoding.UTF8;

        private string appid;
        private string secret;

        MenuService _menu;



        public WeiXinService(string appid, string secret)
        {
            if (appid == null)
                throw Error.ArugmentNull("appid");

            if (secret == null)
                throw Error.ArugmentNull("secret");

            this.appid = appid;
            this.secret = secret;

        }

        internal T Deserialize<T>(string json, T anonymousObj) where T : class
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


        internal string Serialize(object obj)
        {
            var str = JsonConvert.SerializeObject(obj, new StringEnumConverter());
            return str;
        }

        internal string GetJson(string url)
        {
            return this.GetJson(url, null as Dictionary<string, string>);
        }

        internal string GetJson(string url, string value)
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

        internal string GetJson(string url, Dictionary<string, string> values)
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

        internal T Call<T>(string url, T anonymousObj) where T : class
        {
            return Call<T>(url, anonymousObj, null);
        }

        internal T Call<T>(string url, T anonymousObj, object obj) where T : class
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

        public AccessToken token()
        {
            return AccessTokenPool.GetAccessToken(appid, secret);
        }

        public string[] getcallbackip()
        {
            var path = "getcallbackip?access_token=" + token();
            var result = Call(path, new { ip_list = new string[] { } });
            return result.ip_list;
        }

        public MenuService menu
        {
            get
            {
                if (this._menu == null)
                    this._menu = new MenuService(this);

                return this._menu;
            }
        }

        public enum MediaType
        {
            image,
            voice,
            video,
            thumb
        }

        public class MediaService
        {
            public class UploadResult
            {
                public MediaType type;
                public string media_id;
                public DateTime created_at;
            }

            public class Article
            {
                public string thumb_media_id;
                public string author;
                public string title;
                public string content_source_url;
                public string content;
                public string digest;
                public string show_cover_pic;
            }

            private WeiXinService weixin;
            internal MediaService(WeiXinService weixin)
            {
                this.weixin = weixin;
            }

            public UploadResult upload(MediaType type, string fileName)
            {
                var url = string.Format("http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", weixin.token(), type);
                var webClient = new WebClient();
                var bytes = webClient.UploadFile(url, fileName);
                var json = System.Text.Encoding.UTF8.GetString(bytes);

                return weixin.Deserialize(json, new UploadResult());
            }

            public UploadResult uploadnews(Article[] articles)
            {
                if (articles == null)
                    throw Error.ArugmentNull("articles");

                if (articles.Length == 0)
                {
                    var msg = "Length of the articles argument cannt be zero.";
                    throw Error.ArugmentError(msg);
                }

                var url = "media/uploadnews?access_token=" + weixin.token();
                var serial = new System.Web.Script.Serialization.JavaScriptSerializer();
                var data = serial.Serialize(new { articles });
                var json = weixin.GetJson(url, data);

                return weixin.Deserialize(json, new UploadResult());
            }
        }

        public enum MessageType
        {
            mpnews,
            text,
            voice,
            image
        }



        public class MessageService
        {
            private WeiXinService weixin;

            internal MessageService(WeiXinService weixin)
            {
                this.weixin = weixin;
                this.mass = new WeiXinService.MassService(weixin);
            }

            public WeiXinService.MassService mass
            {
                get;
                private set;
            }


        }
    }

   
}



