using System;
using System.Xml;
namespace YuChang.Core.Models
{
    public class Music
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string MusicUrl { get; set; }

        public string HQMusicUrl { get; set; }

        public string ThumbMediaId { get; set; }
    }

    public class MusicMessage : NormalMessage
    {
        #region MyRegion
        //public string Title
        //{
        //    get;
        //    set;
        //}
        //public string Description
        //{
        //    get;
        //    set;
        //}
        //public string MusicURL
        //{
        //    get;
        //    set;
        //}
        //public string HQMusicUrl
        //{
        //    get;
        //    set;
        //}
        //public string ThumbMediaId
        //{
        //    get;
        //    set;
        //} 
        #endregion

        public MusicMessage()
            : base(MessageType.Music)
        {
            this.Music = new Music();
        }

        public Music Music
        {
            get;
            private set;
        }

        public override string ToXml()
        {
            var element = ParseModelToXml();
            return element.OuterXml;
        }

        XmlElement ParseModelToXml()
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlElement xmlElement = (XmlElement)xmlDocument.CreateNode(XmlNodeType.Element, "xml", null);
            XmlNode xmlNode = xmlDocument.CreateNode(XmlNodeType.Element, "ToUserName", null);
            xmlNode.InnerText = base.ToUserName;
            xmlElement.AppendChild(xmlNode);
            XmlNode xmlNode2 = xmlDocument.CreateNode(XmlNodeType.Element, "FromUserName", null);
            xmlNode2.InnerText = base.FromUserName;
            xmlElement.AppendChild(xmlNode2);
            XmlNode xmlNode3 = xmlDocument.CreateNode(XmlNodeType.Element, "CreateTime", null);
            xmlNode3.InnerText = Message.ConvertDateTime(base.CreateTime).ToString();
            xmlElement.AppendChild(xmlNode3);
            XmlNode xmlNode4 = xmlDocument.CreateNode(XmlNodeType.Element, "MsgType", null);
            xmlNode4.InnerText = "music";
            xmlElement.AppendChild(xmlNode4);
            XmlNode xmlNode5 = xmlDocument.CreateNode(XmlNodeType.Element, "Music", null);
            xmlElement.AppendChild(xmlNode5);
            XmlNode xmlNode6 = xmlDocument.CreateNode(XmlNodeType.Element, "Title", null);
            xmlNode6.InnerText = this.Music.Title;
            xmlNode5.AppendChild(xmlNode6);
            XmlNode xmlNode7 = xmlDocument.CreateNode(XmlNodeType.Element, "Description", null);
            xmlNode7.InnerText = this.Music.Description;
            xmlNode5.AppendChild(xmlNode7);
            XmlNode xmlNode8 = xmlDocument.CreateNode(XmlNodeType.Element, "MusicURL", null);
            xmlNode8.InnerText = this.Music.MusicUrl;
            xmlNode5.AppendChild(xmlNode8);
            XmlNode xmlNode9 = xmlDocument.CreateNode(XmlNodeType.Element, "HQMusicUrl", null);
            xmlNode9.InnerText = this.Music.HQMusicUrl;
            xmlNode5.AppendChild(xmlNode9);
            if (!string.IsNullOrEmpty(this.Music.ThumbMediaId))
            {
                XmlNode xmlNode10 = xmlDocument.CreateNode(XmlNodeType.Element, "ThumbMediaId", null);
                xmlNode10.InnerText = this.Music.ThumbMediaId;
                xmlNode5.AppendChild(xmlNode10);
            }
            return xmlElement;
        }
    }
}
