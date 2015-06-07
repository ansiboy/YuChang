using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YuChang.Web
{
    class AppSettingItemMissException : Exception
    {
        public AppSettingItemMissException(string name)
            : base(GetMessage(name))
        {

        }

        static string GetMessage(string name)
        {
            var msg = string.Format("The app setting item '{0}' is not config.", name);
            return msg;
        }
    }

    public class Error
    {
        internal static Exception AppSettingItemMiss(string name)
        {
            return new AppSettingItemMissException(name);
        }
    }
}