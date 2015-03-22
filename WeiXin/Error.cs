using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuChang.Core
{
    public class WeiXinException : Exception
    {
        public WeiXinException(int code, string msg)
            : base(code + ":" + msg)
        {
            this.Code = code.ToString();
            this.Message = msg;
        }

        public WeiXinException(string code, string msg)
            : base(msg)
        {
            this.Code = code;
            this.Message = msg;
        }

        public string Code
        {
            get;
            private set;
        }

        public string Message
        {
            get;
            private set;
        }
    }

    class Error
    {
        internal static Exception ArugmentNull(string paramName)
        {
            var exc = new ArgumentNullException(paramName);
            return exc;
        }

        internal static Exception WeiXinError(int code, string msg)
        {
            var exc = new WeiXinException(code, msg);
            return exc;
        }

        internal static Exception WeiXinError(string code, string msg)
        {
            var exc = new WeiXinException(code, msg);
            return exc;
        }

        internal static Exception MissKeyValue(string key)
        {
            var msg = string.Format("The dictionary is not contains key '{0}'.", key);
            var exc = new Exception(msg);
            return exc;
        }



        internal static Exception NotImplemented()
        {
            return new NotImplementedException();
        }

        internal static Exception AppIdIsRequired()
        {
            var msg = "AppId is required.";
            return new Exception(msg);
        }

        internal static Exception AppSecretRequired()
        {
            var msg = "AppSecret is required.";
            return new Exception(msg);
        }

        internal static Exception ArugmentError(string msg)
        {
            var exc = new ArgumentException(msg);
            return exc;
        }
    }
}
