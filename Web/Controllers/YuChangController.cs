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
        /// <summary>
        /// 获取用户的信息，用户的信息将写入到 Cookie 。
        /// </summary>
        public ActionResult LoadUserInfo()
        {
            return View();
        }

        /// <summary>
        /// 获取微信的版本号
        /// </summary>
        public JsonResult GetWeiXinVersion()
        {
            return Json(new { Version = "" });
        }

    }
}
