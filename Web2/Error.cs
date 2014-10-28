using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeiXin
{
    public class NotConfigConnectionStringException : Exception
    {
        public NotConfigConnectionStringException(string connectionName)
            : base(string.Format("The connection string is not config in the config file.", connectionName))
        {

        }
    }

    public static class Error
    {
        public static Exception NotConfigConnectionString(string connectionName)
        {
            return new NotConfigConnectionStringException(connectionName);
        }

    }
}