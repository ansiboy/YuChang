using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
namespace YuChang.Core.Models
{
    public class ImageTextMessage : Message
    {
        public int ArticleCount
        {
            get
            {
                return this.Articles.Count;
            }
        }
        public IList<Article> Articles
        {
            get;
            private set;
        }
        public ImageTextMessage()
            : base(MessageType.ImageText)
        {
            this.Articles = new List<Article>();
        }
        protected override XmlNode ParsePropertyToNode(XmlDocument doc, PropertyInfo property)
        {
            XmlNode result;
            if (property.Name == "Articles")
            {
                IList<Article> list = (IList<Article>)property.GetValue(this, null);
                XmlNode xmlNode = doc.CreateNode(XmlNodeType.Element, property.Name, null);
                foreach (Article current in list)
                {
                    XmlNode newChild = Message.ParseModelToNode(doc, current, "item");
                    xmlNode.AppendChild(newChild);
                }
                result = xmlNode;
            }
            else
            {
                result = base.ParsePropertyToNode(doc, property);
            }
            return result;
        }
    }
}
