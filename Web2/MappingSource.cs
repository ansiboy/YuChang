using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ALinq.Mapping;

namespace WeiXin
{
    public class ContextMappingSource : FluentMappingSource
    {
        public ContextMappingSource()
        {
            Map<WeiXinDataContext>(mapping =>
            {
                mapping.Table(o => o.UserInfos, "WX_UserInfo")
                       .Column(o => o.City)
                       .Column(o => o.Country)
                       .Column(o => o.HeadImgUrl)
                       .Column(o => o.Language)
                       .Column(o => o.NickName)
                       .Column(o => o.OpenId)
                       .Column(o => o.Province)
                       .Column(o => o.Sex)
                       .Column(o => o.SubscribeTime);
            });


        }
    }
}