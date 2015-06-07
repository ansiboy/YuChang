using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YuChang.Core.Models;

namespace YuChang.Core
{


    internal class TemplateManager
    {
        private access_token accessToken;

        public TemplateManager(access_token accessToken)
        {
            this.accessToken = accessToken;
        }

        public void Send(Template template)
        {
            if (template == null)
                throw Error.ArugmentNull("template");


            var data = ConvertTemplateToJson(template);
            var result = Utility.PostString("message/template/send?access_token=" + this.accessToken, data);

            var dic = Utility.Deserialize<Dictionary<string, object>>(result);
            var code = (int)dic["errcode"];
            var msg = (string)dic["errmsg"];
            if (code != 0)
                throw Error.WeiXinError(code, msg);
        }

        string ConvertTemplateToJson(Template template)
        {
            var dic = new Dictionary<string, object>();
            dic["touser"] = template.ToUser;
            dic["template_id"] = template.TemplateId;
            dic["url"] = template.Url;
            dic["topcolor"] = template.TopColor;

            var data = new Dictionary<string, object>();
            dic["data"] = data;

            foreach (var field in template.Fields)
            {
                var item = new Dictionary<string, string>();
                item["value"] = field.Value;
                item["color"] = field.Color;

                data[field.Name] = item;
            }

            var text = Utility.Serialize(dic);
            return text;
        }


    }
}
