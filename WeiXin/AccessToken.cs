using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuChang.Core
{
     public class AccessToken
    {
        private string appid;
        private string secret;
        private string token;
        private int expiresIn;
        private DateTime createDateTime;

        public AccessToken(string appid, string secret)
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

            var url = string.Format("token?grant_type=client_credential&appid={0}&secret={1}", appid, secret);

            var data = Utility.GetWeiXinJson(url);
            this.expiresIn = (int)data["expires_in"];
            this.token = data["access_token"] as string;
            this.createDateTime = DateTime.Now;     //娉¨锛氬垱寤烘椂闂村拷鐣ョ綉缁滅殑寤舵椂

            return this.token;
        }

        public string AppId
        {
            get { return this.appid; }
        }

        public string Secret
        {
            get { return this.secret; }
        }
    }
}
