using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace YuChang.Core.Models
{
    //public abstract class ReplyMessage
    //{
    //    protected ReplyMessage(Message msg, ReplyMessageType msgType)
    //        : this(msgType)
    //    {
    //        this.ToUserName = msg.FromUserName;
    //        this.FromUserName = msg.ToUserName;
    //    }

    //    protected ReplyMessage(ReplyMessageType msgType)
    //    {
    //        this.MsgType = msgType;
    //        CreateTime = DateTime.Now;
    //    }

    //    /// <summary>
    //    /// 接收方帐号（收到的OpenID）
    //    /// </summary>
    //    public string ToUserName { get; set; }

    //    /// <summary>
    //    /// 开发者微信号
    //    /// </summary>
    //    public string FromUserName { get; set; }

    //    /// <summary>
    //    /// 消息创建时间
    //    /// </summary>
    //    public DateTime CreateTime { get; private set; }

    //    /// <summary>
    //    /// 消息类型
    //    /// </summary>
    //    public ReplyMessageType MsgType { get; private set; }

    //    public string ToXml()
    //    {
    //        var element = ParseModelToXml(this);
    //        var xml = element.OuterXml;
    //        return xml;
    //    }

    //    static XmlElement ParseModelToXml(object model)
    //    {
    //        if (model == null)
    //            throw Error.ArugmentNull("model");

    //        XmlDocument doc = new XmlDocument();
    //        var root = (XmlElement)doc.CreateNode(XmlNodeType.Element, "xml", null);
    //        var properties = (PropertyInfo[])model.GetType().GetProperties();
    //        foreach (var property in properties)
    //        {
    //            var value = property.GetValue(model, null);
    //            var child = doc.CreateNode(XmlNodeType.Element, property.Name, null);
    //            if (property.PropertyType == typeof(DateTime))
    //            {
    //                child.InnerText = ConvertDateTime((DateTime)value).ToString();
    //            }
    //            else if (typeof(Enum).IsAssignableFrom(property.PropertyType))
    //            {
    //                child.AppendChild(doc.CreateCDataSection(value.ToString().ToLower()));
    //            }
    //            else
    //            {
    //                child.AppendChild(doc.CreateCDataSection(value.ToString()));
    //            }
    //            root.AppendChild(child);
    //        }

    //        return root;
    //    }

    //    static int ConvertDateTime(System.DateTime time)
    //    {

    //        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));

    //        return (int)(time - startTime).TotalSeconds;

    //    }

    //}
}
