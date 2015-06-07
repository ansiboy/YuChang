using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuChang.Core
{
    /// <summary>
    /// 将微信接口中的accessToken封装为对象，并且能自动进行继约。
    /// </summary>
    public class AccessToken
    {
        private string appid;
        private string secret;
        private string token;
        private int expiresIn;
        private DateTime createDateTime;
        
        /// <summary>
        /// AccessToken 的构造函数
        /// </summary>
        /// <param name="appid">公众号的应用ID</param>
        /// <param name="secret">公众号的应用密钥</param>
        internal AccessToken(string appid, string secret)
        {
            this.appid = appid;
            this.secret = secret;
        }

        private bool IsValid()
        {
            return (DateTime.Now - this.createDateTime).TotalSeconds < expiresIn;
        }

        public override string ToString()
        {
            if (this.token != null && IsValid())
                return this.token;

            IsUsing = true;

            try
            {
                var url = string.Format("token?grant_type=client_credential&appid={0}&secret={1}", appid, secret);

                var data = Utility.GetWeiXinJson(url);
                object errcode;
                if (data.TryGetValue("errcode", out errcode))
                {
                    var msg = (string)data["errmsg"];
                    throw Error.WeiXinError((int)errcode, msg);
                }

                this.expiresIn = (int)data["expires_in"];
                this.token = data["access_token"] as string;
                this.createDateTime = DateTime.Now;

                return this.token;
            }
            finally
            {
                IsUsing = false;
            }
        }

        internal bool IsUsing
        {
            get;
            private set;
        }

        /// <summary>
        /// 公众号的应用ID
        /// </summary>
        public string AppId
        {
            get { return this.appid; }
        }

        /// <summary>
        /// 公众号的应用密钥
        /// </summary>
        public string Secret
        {
            get { return this.secret; }
        }
    }
}
