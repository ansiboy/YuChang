using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web1.Controllers
{
    public class YuChangController : Controller
    {
        DbConnection CreateConnection()
        {
            var conn = "";
            return new SqlConnection(conn);
        }

        public ActionResult Index()
        {
            //
            var conn = CreateConnection();
            try
            {
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText = "Select Id, [Message] Where MessageType = @messageType and EventType = @eventType";
          
            }
            finally
            {
                conn.Close();
            }
            return Content("");
        }

    }
}
