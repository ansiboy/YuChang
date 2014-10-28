using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using tenpayApp;
using YuChang.Core.Models;

namespace YuChang.Core
{
    public class CustomMenu
    {
        private AccessToken accessToken;

        public CustomMenu(AccessToken accessToken)
        {
            this.accessToken = accessToken;
        }

        public IEnumerable<Button> GetMenu()
        {
            var url = string.Format("menu/get?access_token={0}", this.accessToken);
            var data = Utility.GetWeiXinJson(url);
            object menuData;
            if (!data.TryGetValue("menu", out menuData))
                throw Error.MissKeyValue("menu");

            object buttonData;
            if (!((Dictionary<string, object>)menuData).TryGetValue("button", out buttonData))
                throw Error.MissKeyValue("button");

            var buttonArray = buttonData as System.Collections.IEnumerable;
            var buttons = new List<Button>();
            foreach (object item in buttonArray)
            {
                var button = Utility.ParseObjectFromDictionary<Button>((Dictionary<string, object>)item);
                buttons.Add(button);
            }
            return buttons;
        }

        public void SaveMenu(IEnumerable<Button> buttons)
        {
            var url = string.Format("menu/create?access_token={0}", this.accessToken);
            var btn_data = new ArrayList();
            foreach (var button in buttons)
            {
                btn_data.Add(Utility.ParseObjectToDictionary(button));
            }

            var data = new Dictionary<string, object>();
            data["button"] = btn_data;
            var str = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(data);
            var result = Utility.PostString(url, str);
        }




    }
}
