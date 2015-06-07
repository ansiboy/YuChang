using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuChang.Core.Models
{
    public class Template
    {
        private List<TemplateField> fields;
        const string DEFAULT_TOP_COLOR = "#4D6082";

        public Template(string templateId, string topColor = null)
        {
            if (string.IsNullOrEmpty(templateId))
                throw Error.ArugmentNull("templateId");

            if (string.IsNullOrEmpty(topColor))
                topColor = DEFAULT_TOP_COLOR;

            this.TemplateId = templateId;
            this.TopColor = topColor;

            this.fields = new List<TemplateField>();
        }

        public string TemplateId { get; set; }

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
        const string DEFAULT_FIELD_COLOR = "#000000";

        public TemplateField(string name, string value, string color = null)
        {
            if (string.IsNullOrEmpty(name))
                throw Error.ArugmentNull("name");

            if (string.IsNullOrEmpty(value))
                throw Error.ArugmentNull("value");

            if (string.IsNullOrEmpty(color))
                color = DEFAULT_FIELD_COLOR;// "#FF0000";

            this.Name = name;
            this.Value = value;
            this.Color = color;
        }

        public string Name { get; set; }

        public string Value { get; set; }

        public string Color { get; set; }
    }
}
