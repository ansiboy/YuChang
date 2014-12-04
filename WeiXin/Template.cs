using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuChang.Core
{
    public class Template
    {
        private List<TemplateField> fields;
        //public object TopColor;

        public Template(string templateId, string toUser, string topColor = null)
        {
            if (string.IsNullOrEmpty(templateId))
                throw Error.ArugmentNull("templateId");

            if (string.IsNullOrEmpty(toUser))
                throw Error.ArugmentNull("toUser");

            if (string.IsNullOrEmpty(topColor))
                topColor = "#FF0000";

            this.TemplateId = templateId;
            this.ToUser = toUser;
            this.TopColor = topColor;
            this.Url = "";

            this.fields = new List<TemplateField>();
        }

        public string TemplateId { get; set; }

        public string ToUser { get; set; }

        public string Url { get; set; }

        public string TopColor { get; set; }

        public List<TemplateField> Fields
        {
            get
            {
                return this.fields;
            }
        }
    }

    public class TemplateField
    {
        public TemplateField(string name, string value, string color = null)
        {
            if (string.IsNullOrEmpty(name))
                throw Error.ArugmentNull("name");

            if (string.IsNullOrEmpty(value))
                throw Error.ArugmentNull("value");

            if (string.IsNullOrEmpty(color))
                color = "#FF0000";

            this.Name = name;
            this.Value = value;
            this.Color = color;
        }

        public string Name { get; set; }

        public string Value { get; set; }

        public string Color { get; set; }
    }

    public class TemplateManager
    {
        private AccessToken accessToken;

        public TemplateManager(AccessToken accessToken)
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
