using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuChang.Core
{
    public class Button
    {
        public ButtonType type;
        public string name;
        public string key;
        public string url;
        public Button[] sub_button;
    }

    public enum ButtonType
    {
        click,
        view,
        scancode_waitmsg,
        scancode_push,
        pic_sysphoto,
        pic_photo_or_album,
        pic_weixin,
        location_select
    }

    public class MenuService
    {
        private WeiXinService weixin;

        internal MenuService(WeiXinService weixin)
        {
            this.weixin = weixin;
        }

        public void create(Button[] button)
        {
            var url = string.Format("menu/create?access_token={0}", weixin.token());
            this.weixin.Call(url, new { errcode = "" }, new { button });
        }

        class GetResult
        {
            internal class Menu
            {
                public Button[] button;
            }

            public Menu menu;
        }

        public Button[] get()
        {
            var url = "menu/get?access_token=" + weixin.token();
            var result = weixin.Call(url, new { menu = new { button = new Button[] { } } });
            return result.menu.button;
        }

        public void delete()
        {
            var url = "menu/delete?access_token=" + weixin.token();
            weixin.Call(url, new { errcode = "" });
        }
    }
}
