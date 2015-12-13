using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace YuChang.Core.Messages
{
    public abstract class NormalMessage : Message
    {
        protected NormalMessage(MessageType msgType)
            : base(msgType)
        {

        }

        public virtual string ToXml()
        {
            var element = ParseModelToXml();
            var xml = element.OuterXml;
            return xml;
        }

        private XmlElement ParseModelToXml()
        {
            var doc = new XmlDocument();
            var root = (XmlElement)doc.CreateNode(XmlNodeType.Element, "xml", null);
            var properties = (PropertyInfo[])this.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.CanWrite == false || property.GetSetMethod() == null)
                    continue;

                //property.Attributes && PropertyAttributes
                var child = ParsePropertyToNode(doc, property);
                root.AppendChild(child);
            }

            return root;
        }

        private XmlNode ParsePropertyToNode(XmlDocument doc, PropertyInfo property)
        {
            return ParsePropertyToNode(doc, property, this);
        }

        private static XmlNode ParsePropertyToNode(XmlDocument doc, PropertyInfo property, object model)
        {
            var value = property.GetValue(model, null);
            var child = doc.CreateNode(XmlNodeType.Element, property.Name, null);
            if (property.PropertyType == typeof(DateTime))
            {
                child.InnerText = ConvertDateTime((DateTime)value).ToString();
            }
            else if (property.PropertyType == typeof(String))
            {
                child.AppendChild(doc.CreateCDataSection(value as string));
            }
            else if (typeof(Enum).IsAssignableFrom(property.PropertyType))
            {
                var str = Utility.ConvertEnumValue(property.PropertyType, value);
                child.AppendChild(doc.CreateCDataSection(str));
            }
            else
            {
                child.AppendChild(doc.CreateCDataSection(value.ToString()));
            }

            return child;
        }

        private static XmlNode ParseModelToNode(XmlDocument doc, object model, string nodeName)
        {
            if (model == null)
                throw Error.ArugmentNull("model");

            //var doc = new XmlDocument();
            var root = (XmlElement)doc.CreateNode(XmlNodeType.Element, nodeName, null);
            var properties = (PropertyInfo[])model.GetType().GetProperties();
            foreach (var property in properties)
            {
                var child = ParsePropertyToNode(doc, property, model);
                root.AppendChild(child);
            }

            return root;
        }
    }
}
