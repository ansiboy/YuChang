using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;

namespace YuChang.Core.Models
{
    public abstract class Message
    {
        protected Message(MessageType msgType)
        {
            this.MsgType = msgType;
        }

        /// <summary>
        /// 开发者微信号
        /// </summary>
        public string ToUserName { get; set; }

        /// <summary>
        /// 发送方帐号（一个OpenID）
        /// </summary>
        public string FromUserName { get; set; }

        /// <summary>
        /// 消息创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageType MsgType { get; private set; }

        public static Message FromXml(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            var element = doc.DocumentElement;
            Debug.Assert(element != null);
            var msgTypeNode = element.SelectSingleNode("MsgType");
            if (msgTypeNode == null)
                return null;

            var msgType = msgTypeNode.InnerText.ToLower();
            switch (msgType)
            {
                case "text":
                    return ParseXmlToModel<TextMessage>(element);
                case "image":
                    return ParseXmlToModel<ImageMessage>(element);
                case "voice":
                    return ParseXmlToModel<VoiceMessage>(element);
                case "video":
                    return ParseXmlToModel<VideoMessage>(element);
                case "location":
                    return ParseXmlToModel<LocationMessage>(element);
                case "link":
                    return ParseXmlToModel<LinkMessage>(element);
                case "transfer_customer_service":
                    return ParseXmlToModel<TransferCustomerServiceMessage>(element);
                case "event":
                    var eventNode = element.SelectSingleNode("Event");
                    if (eventNode == null)
                    {
                        Trace.WriteLine("Event node not find.");
                        return null;
                    }

                    switch (eventNode.InnerText.ToLower())
                    {
                        case "subscribe":
                            return ParseXmlToModel<SubscribeEvent>(element);
                        case "unsubscribe":
                            return ParseXmlToModel<UnsubscribeEvent>(element);
                        case "scan":
                            return ParseXmlToModel<ScanEvent>(element);
                        case "location":
                            return ParseXmlToModel<LocationEvent>(element);
                        case "click":
                            return ParseXmlToModel<ClickEvent>(element);
                        case "view":
                            return ParseXmlToModel<ViewEvent>(element);
                    }
                    break;
            }
            return null;
        }

        internal static T ParseXmlToModel<T>(XmlElement element)
        {
            var model = Activator.CreateInstance<T>();
            var properties = typeof(T).GetProperties();
            foreach (var p in properties)
            {
                if (p.CanWrite == false)
                    continue;

                var value = GetPropertyValue(element, p);
                p.SetValue(model, value, null);
            }

            return model;
        }

        static object GetPropertyValue(XmlElement rootElement, PropertyInfo property)
        {
            var node = rootElement.SelectSingleNode(property.Name);
            if (node == null)
                return null;

            var valueText = node.InnerText;
            //return value;
            if (property.PropertyType == typeof(DateTime))
                return UnixTimeToTime(valueText);

            if (property.PropertyType == typeof(string))
                return valueText;

            if (typeof(Enum).IsAssignableFrom(property.PropertyType))
            {
                var value = Enum.Parse(property.PropertyType, valueText, true);
                return value;
            }

            return Convert.ChangeType(valueText, property.PropertyType);
        }

        static DateTime UnixTimeToTime(string timeStamp)
        {

            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));

            long lTime = long.Parse(timeStamp + "0000000");

            var toNow = new TimeSpan(lTime);

            return dtStart.Add(toNow);

        }

        public string ToXml()
        {
            var element = ParseModelToXml(this);
            var xml = element.OuterXml;
            return xml;
        }

        static XmlElement ParseModelToXml(object model)
        {
            if (model == null)
                throw Error.ArugmentNull("model");

            var doc = new XmlDocument();
            var root = (XmlElement)doc.CreateNode(XmlNodeType.Element, "xml", null);
            var properties = (PropertyInfo[])model.GetType().GetProperties();
            foreach (var property in properties)
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

                    //var attr = (DescriptionAttribute)property.GetCustomAttributes(typeof(DescriptionAttribute), false).SingleOrDefault();
                    //if (attr != null)
                    //    child.AppendChild(doc.CreateCDataSection(attr.Description));
                    //else
                    //    child.AppendChild(doc.CreateCDataSection(value.ToString().ToLower()));
                }
                else
                {
                    child.AppendChild(doc.CreateCDataSection(value.ToString()));
                }
                root.AppendChild(child);
            }

            return root;
        }

        static int ConvertDateTime(System.DateTime time)
        {

            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));

            return (int)(time - startTime).TotalSeconds;

        }
    }

}
