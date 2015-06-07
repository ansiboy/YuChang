using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YuChang.Web.Models
{
    public enum Gender
    {
        Female,
        Male,
        Unknown
    }

    public class UserInfo
    {
        public string OpenId { get; internal set; }

        public string NickName { get; internal set; }

        public string City { get; internal set; }

        public string Country { get; internal set; }

        public string Province { get; internal set; }

        public string Language { get; internal set; }

        public string HeadImgUrl { get; internal set; }

        public Gender Sex { get; internal set; }

        public string Privilege { get; internal set; }

        public string Unionid { get; internal set; }
    }
}