using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuChang.Core
{
    public class TemplateService
    {
        private WeiXinService weixin;

        public TemplateService(WeiXinService weixin)
        {
            this.weixin = weixin;
        }

        public void api_add_template(string template_id_short)
        {
            var url = "message/template/send?access_token=" + this.weixin.token();
        }
    }
}
