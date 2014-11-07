using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Collections;

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
            this.createDateTime = DateTime.Now;     //注：创建时间忽略网络的延时

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

    internal class Constants
    {
        public const string RequestRoot = "https://api.weixin.qq.com/cgi-bin/";
    }

    public class UserInfo
    {
        public string OpenId { get; set; }

        public string NickName { get; set; }

        public Gender Sex { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string Province { get; set; }

        public string Language { get; set; }

        public string HeadImgUrl { get; set; }

        public string SubscribeTime { get; set; }
    }

    public enum Gender
    {
        None,
        Male,
        Female
    }

    public class UserOpenIdCollection : IEnumerable<string>
    {
        private IEnumerable<string> items;

        public UserOpenIdCollection(IEnumerable<string> items)
        {
            this.items = items;
        }

        public int Total { get; internal set; }

        public IEnumerator<string> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        public string NextOpenId { get; internal set; }

        public int Count { get; internal set; }
    }

    public class UserManager
    {
        private AccessToken accessToken;

        public UserManager(AccessToken accessToken)
        {
            this.accessToken = accessToken;
        }

        //public UserInfo GetUserInfo(string userOpenId)
        //{
        //    return GetUserInfo<UserInfo>(userOpenId);
        //}

        public UserInfo GetUserInfo(string userOpenId)
        {
            var url = string.Format("user/info?access_token={0}&openid={1}&lang=zh_CN",
                                    accessToken, userOpenId);

            var data = Utility.GetWeiXinJson(url);

            var userInfo = new UserInfo();
            userInfo.OpenId = data["openid"] as string;
            userInfo.NickName = data["nickname"] as string;
            userInfo.City = data["city"] as string;
            userInfo.Country = data["country"] as string;
            userInfo.Province = data["province"] as string;
            userInfo.Language = data["language"] as string;
            userInfo.HeadImgUrl = data["headimgurl"] as string;
            //userInfo.SubscribeTime = data["subscribe_time"] as string;

            switch (((int)data["sex"]))
            {
                case 0:
                    userInfo.Sex = Gender.None;
                    break;
                case 1:
                    userInfo.Sex = Gender.Male;
                    break;
                case 2:
                    userInfo.Sex = Gender.Female;
                    break;
            }

            return userInfo;
        }

        /// <summary>
        /// 获取多个关注用户的 OpenId
        /// </summary>
        /// <param name="previousOpenId">前一个用户的 OpenId</param>
        /// <returns>多个用户的 OpenId 集合</returns>
        public UserOpenIdCollection GetUserOpenIds(string previousOpenId = null)
        {
            var url = string.Format("user/get?access_token={0}", accessToken);
            if (previousOpenId != null)
                url = url + "&next_openid=" + previousOpenId;

            var data = Utility.GetWeiXinJson(url);

            IEnumerable<string> openids;


            object openIdObject;

            if (string.IsNullOrEmpty((string)data["next_openid"]))
            {
                openids = new string[] { };
            }
            else
            {
                ((IDictionary<string, object>)data["data"]).TryGetValue("openid", out openIdObject);
                openids = ((ArrayList)openIdObject).Cast<string>();
            }

            var result = new UserOpenIdCollection(openids);
            result.Total = Convert.ToInt32(data["total"]);
            result.Count = Convert.ToInt32(data["count"]);
            result.NextOpenId = (string)data["next_openid"];

            return result;
        }

        /// <summary>
        /// 获取所有关注用户的 OpenId
        /// </summary>
        /// <param name="previousOpenId">前一个用户的 OpenId</param>
        /// <returns>多个用户的 OpenId 集合</returns>
        public IEnumerable<string> GetAllUserOpenIds()
        {
            var openids = new List<string>();
            var openIdCollection = this.GetUserOpenIds();
            while (openIdCollection.NextOpenId.Trim() != string.Empty)
            {
                foreach (var openid in openIdCollection)
                    openids.Add(openid);

                openIdCollection = this.GetUserOpenIds(openIdCollection.NextOpenId);
            }

            return openids;
        }
    }
}