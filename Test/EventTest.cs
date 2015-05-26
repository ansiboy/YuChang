using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using YuChang.Core.Models;
using System.Data.SqlClient;
using YuChang.Core;

namespace YuChang.Test
{
    [TestClass]
    public class EventTest
    {
        [TestMethod]
        public void ParseSubscribeEvent()
        {
            var xml = @"<xml><ToUserName><![CDATA[gh_78e13f73c3ea]]></ToUserName>
<FromUserName><![CDATA[o4mqUjspXEinqvno9XS3RUGEITS8]]></FromUserName>
<CreateTime>1403518265</CreateTime>
<MsgType><![CDATA[event]]></MsgType>
<Event><![CDATA[subscribe]]></Event>
<EventKey><![CDATA[]]></EventKey>
</xml>";

            var doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlElement element = doc.DocumentElement;
            var msg = (SubscribeEvent)Message.FromXml(xml); //Utility.ParseXmlToModel<SubscribeEvent>(element);

            Assert.AreEqual("gh_78e13f73c3ea", msg.ToUserName);
            Assert.AreEqual("o4mqUjspXEinqvno9XS3RUGEITS8", msg.FromUserName);
            Assert.AreEqual(1403518265, ConvertDateTime(msg.CreateTime));
            Assert.AreEqual(MessageType.Event, msg.MsgType);
            Assert.AreEqual(EventType.Subscribe, msg.Event);
        }

        [TestMethod]
        public void ParseScanSubscribeEvent()
        {
            var xml = @"<xml><ToUserName><![CDATA[toUser]]></ToUserName>
<FromUserName><![CDATA[FromUser]]></FromUserName>
<CreateTime>123456789</CreateTime>
<MsgType><![CDATA[event]]></MsgType>
<Event><![CDATA[subscribe]]></Event>
<EventKey><![CDATA[qrscene_123123]]></EventKey>
<Ticket><![CDATA[TICKET]]></Ticket>
</xml>";

            var doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlElement element = doc.DocumentElement;
            var msg = (SubscribeEvent)Message.FromXml(xml);

            Assert.AreEqual("toUser", msg.ToUserName);
            Assert.AreEqual("FromUser", msg.FromUserName);
            Assert.AreEqual(123456789, ConvertDateTime(msg.CreateTime));
            Assert.AreEqual(MessageType.Event, msg.MsgType);
            Assert.AreEqual(EventType.Subscribe, msg.Event);
            Assert.AreEqual("qrscene_123123", msg.EventKey);
            Assert.AreEqual("TICKET", msg.Ticket);
        }

        [TestMethod]
        public void ParseScanEvent()
        {
            var xml = @"<xml>
<ToUserName><![CDATA[toUser]]></ToUserName>
<FromUserName><![CDATA[o4mqUjspXEinqvno9XS3RUGEITS8]]></FromUserName>
<CreateTime>123456789</CreateTime>
<MsgType><![CDATA[event]]></MsgType>
<Event><![CDATA[SCAN]]></Event>
<EventKey><![CDATA[SCENE_VALUE]]></EventKey>
<Ticket><![CDATA[1]]></Ticket>
</xml>";

            var doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlElement element = doc.DocumentElement;
            var msg = (ScanEvent)Message.FromXml(xml);

            Assert.AreEqual("toUser", msg.ToUserName);
            Assert.AreEqual("o4mqUjspXEinqvno9XS3RUGEITS8", msg.FromUserName);
            Assert.AreEqual(123456789, ConvertDateTime(msg.CreateTime));
            Assert.AreEqual(MessageType.Event, msg.MsgType);
            Assert.AreEqual(EventType.Scan, msg.Event);
            Assert.AreEqual("SCENE_VALUE", msg.EventKey);
            Assert.AreEqual("1", msg.Ticket);


        }

        [TestMethod]
        public void ParseLocationEvent()
        {
            var xml = @"<xml>
<ToUserName><![CDATA[toUser]]></ToUserName>
<FromUserName><![CDATA[fromUser]]></FromUserName>
<CreateTime>123456789</CreateTime>
<MsgType><![CDATA[event]]></MsgType>
<Event><![CDATA[LOCATION]]></Event>
<Latitude>23.137466</Latitude>
<Longitude>113.352425</Longitude>
<Precision>119.385040</Precision>
</xml>";

            var doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlElement element = doc.DocumentElement;
            var msg = (LocationEvent)Message.FromXml(xml);

            Assert.AreEqual("toUser", msg.ToUserName);
            Assert.AreEqual("fromUser", msg.FromUserName);
            Assert.AreEqual(123456789, ConvertDateTime(msg.CreateTime));
            Assert.AreEqual(MessageType.Event, msg.MsgType);
            Assert.AreEqual(EventType.Location, msg.Event);
            Assert.AreEqual(23.137466, msg.Latitude);
            Assert.AreEqual(113.352425, msg.Longitude);
            Assert.AreEqual(119.385040, msg.Precision);
        }

        [TestMethod]
        public void ParseClickEvent()
        {
            var xml = @"<xml>
<ToUserName><![CDATA[toUser]]></ToUserName>
<FromUserName><![CDATA[FromUser]]></FromUserName>
<CreateTime>123456789</CreateTime>
<MsgType><![CDATA[event]]></MsgType>
<Event><![CDATA[CLICK]]></Event>
<EventKey><![CDATA[EVENTKEY]]></EventKey>
</xml>";

            var doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlElement element = doc.DocumentElement;
            var msg = (ClickEvent)Message.FromXml(xml);

            Assert.AreEqual("toUser", msg.ToUserName);
            Assert.AreEqual("FromUser", msg.FromUserName);
            Assert.AreEqual(123456789, ConvertDateTime(msg.CreateTime));
            Assert.AreEqual(MessageType.Event, msg.MsgType);
            Assert.AreEqual(EventType.Click, msg.Event);
            Assert.AreEqual("EVENTKEY", msg.EventKey);
        }

        int ConvertDateTime(System.DateTime time)
        {

            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));

            return (int)(time - startTime).TotalSeconds;

        }

        [TestMethod]
        public void ClickEventMessage()
        {
            var xml = @"<xml>
<ToUserName><![CDATA[toUser]]></ToUserName>
<FromUserName><![CDATA[FromUser]]></FromUserName>
<CreateTime>123456789</CreateTime>
<MsgType><![CDATA[event]]></MsgType>
<Event><![CDATA[CLICK]]></Event>
<EventKey><![CDATA[EVENTKEY]]></EventKey>
</xml>";
            Message.FromXml(xml);
        }

        [TestMethod]
        public void TemplateSendJobFinishEvent()
        {
            var xml = @"
<xml><ToUserName><![CDATA[gh_26fac8adcd13]]></ToUserName>
<FromUserName><![CDATA[o1Ux1uP_0TDyXNvZjrKwOyn0Aprs]]></FromUserName>
<CreateTime>1418619744</CreateTime>
<MsgType><![CDATA[event]]></MsgType>
<Event><![CDATA[TEMPLATESENDJOBFINISH]]></Event>
<MsgID>201843808</MsgID>
<Status><![CDATA[success]]></Status>
</xml>
";
            // Message.FromXml(xml);
            var msg = Message.FromXml(xml) as TemplateSendJobFinishEvent;
            Assert.IsNotNull(msg);
            Assert.AreEqual(EventType.TemplateSendJobFinish, msg.Event);

            var p = new MessageProcesser();
            p.Process(xml);
        }


    }
}
