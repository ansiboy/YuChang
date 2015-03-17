using System;
using System.Collections.Generic;
namespace YuChang.Core
{
    public class AccessTokenPool
    {
        private static List<AccessToken> accessTokens = new List<AccessToken>();
        public static string AppId
        {
            get;
            set;
        }
        public static string AppSecret
        {
            get;
            set;
        }
        public static AccessToken GetAccessToken()
        {
            AccessToken result;
            for (int i = 0; i < AccessTokenPool.accessTokens.Count; i++)
            {
                if (!AccessTokenPool.accessTokens[i].IsUsing)
                {
                    result = AccessTokenPool.accessTokens[i];
                    return result;
                }
            }
            if (string.IsNullOrEmpty(AccessTokenPool.AppId))
            {
                throw Error.AppIdIsRequired();
            }
            if (string.IsNullOrEmpty(AccessTokenPool.AppSecret))
            {
                throw Error.AppSecretRequired();
            }
            AccessToken accessToken = new AccessToken(AccessTokenPool.AppId, AccessTokenPool.AppSecret);
            AccessTokenPool.accessTokens.Add(accessToken);
            result = accessToken;
            return result;
        }
    }
}
