using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace YuChang.Core
{
    public enum MediaType
    {
        Image,
        Voice,
        Video,
        Thumb
    }

    public class MediaManager
    {
        private AccessToken accessToken;

        public MediaManager(AccessToken accessToken)
        {
            this.accessToken = accessToken;
        }

        public string Upload(string fileName, MediaType mediaType)
        {
            var url = string.Format("http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", accessToken, mediaType);
            var webClient = new WebClient();
            var bytes = webClient.UploadFile(url, fileName);
            var json = System.Text.Encoding.UTF8.GetString(bytes);
            var serail = new System.Web.Script.Serialization.JavaScriptSerializer();
            var dic = serail.Deserialize<Dictionary<string, object>>(json);
            object errorCode;
            if (dic.TryGetValue("errcode", out errorCode))
            {
                var code = (int)errorCode;
                var msg = (string)dic["errmsg"];
                throw Error.WeiXinError(code, msg);
            }

            return (string)dic["media_id"];
        }

        public void Download(string mediaId, string fileName)
        {
            var url = string.Format("http://file.api.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}", accessToken, mediaId);
            var webClient = new WebClient();
            webClient.DownloadFile(url, fileName);
        }
    }
}
