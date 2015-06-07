using System;
using System.Collections.Generic;
namespace YuChang.Core
{
    internal static class AccessTokenPool
    {
        private static Dictionary<string, List<access_token>> dic_accessTokens
            = new Dictionary<string, List<access_token>>();

        private static Dictionary<string, string> secrets = new Dictionary<string, string>();

        public static access_token GetAccessToken(string appid, string secret)
        {
            List<access_token> accessTokens;
            if (dic_accessTokens.TryGetValue(appid, out accessTokens) == false)
            {
                dic_accessTokens[appid] = accessTokens = new List<access_token>();
            }

            string old_secret;
            if (secrets.TryGetValue(appid, out old_secret) == false)
            {
                secrets[appid] = old_secret = secret;
            }

            if (old_secret != secret)
                dic_accessTokens[appid] = new List<access_token>();

            access_token result;
            for (int i = 0; i < accessTokens.Count; i++)
            {
                if (!accessTokens[i].IsUsing)
                {
                    result = accessTokens[i];
                    return result;
                }
            }
            //if (string.IsNullOrEmpty(AccessTokenPool.AppId))
            //{
            //    throw Error.AppIdIsRequired();
            //}
            //if (string.IsNullOrEmpty(AccessTokenPool.AppSecret))
            //{
            //    throw Error.AppSecretRequired();
            //}
            access_token accessToken = new access_token(appid, secret);
            accessTokens.Add(accessToken);
            result = accessToken;
            return result;
        }
    }
}
