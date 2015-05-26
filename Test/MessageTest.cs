using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using YuChang.Core.Models;

namespace YuChang.Test
{
    [TestClass]
    public class MessageTest
    {
        [TestMethod]
        public void ParseTextMessage()
        {
            var xml = @"<xml>
 <ToUserName><![CDATA[toUser]]></ToUserName>
 <FromUserName><![CDATA[fromUser]]></FromUserName> 
 <CreateTime>1348831860</CreateTime>
 <MsgType><![CDATA[text]]></MsgType>
 <Content><![CDATA[this is a test]]></Content>
 <MsgId>1234567890123456</MsgId>
 </xml>";

            var doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlElement element = doc.DocumentElement;
            var msg = (TextMessage)Message.FromXml(xml);

            Assert.AreEqual("toUser", msg.ToUserName);
            Assert.AreEqual("fromUser", msg.FromUserName);
            Assert.AreEqual(1348831860, ConvertDateTime(msg.CreateTime));
            Assert.AreEqual(MessageType.Text, msg.MsgType);
            Assert.AreEqual("this is a test", msg.Content);
            Assert.AreEqual(1234567890123456L, msg.MsgId);
        }

        [TestMethod]
        public void ParseImageMessage()
        {
            var xml = @"<xml>
 <ToUserName><![CDATA[toUser]]></ToUserName>
 <FromUserName><![CDATA[fromUser]]></FromUserName>
 <CreateTime>1348831860</CreateTime>
 <MsgType><![CDATA[image]]></MsgType>
 <PicUrl><![CDATA[this is a url]]></PicUrl>
 <MediaId><![CDATA[media_id]]></MediaId>
 <MsgId>1234567890123456</MsgId>
 </xml>";

            var doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlElement element = doc.DocumentElement;
            var msg = (ImageMessage)Message.FromXml(xml);

            Assert.AreEqual("toUser", msg.ToUserName);
            Assert.AreEqual("fromUser", msg.FromUserName);
            Assert.AreEqual(1348831860, ConvertDateTime(msg.CreateTime));
            Assert.AreEqual(MessageType.Image, msg.MsgType);
            Assert.AreEqual("this is a url", msg.PicUrl);
            Assert.AreEqual("media_id", msg.MediaId);
            Assert.AreEqual(1234567890123456, msg.MsgId);
        }

        [TestMethod]
        public void ParseVoiceMessage()
        {
            var xml = @"<xml>
<ToUserName><![CDATA[toUser]]></ToUserName>
<FromUserName><![CDATA[fromUser]]></FromUserName>
<CreateTime>1357290913</CreateTime>
<MsgType><![CDATA[voice]]></MsgType>
<MediaId><![CDATA[media_id]]></MediaId>
<Format><![CDATA[Format]]></Format>
<MsgId>1234567890123456</MsgId>
</xml>";

            var doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlElement element = doc.DocumentElement;
            var msg = (VoiceMessage)Message.FromXml(xml);

            Assert.AreEqual("toUser", msg.ToUserName);
            Assert.AreEqual("fromUser", msg.FromUserName);
            Assert.AreEqual(1357290913, ConvertDateTime(msg.CreateTime));
            Assert.AreEqual(MessageType.Voice, msg.MsgType);
            Assert.AreEqual("media_id", msg.MediaId);
            Assert.AreEqual("Format", msg.Format);
            Assert.AreEqual(1234567890123456, msg.MsgId);
        }

        [TestMethod]
        public void ParseVideoMessage()
        {
            var xml = @"<xml>
<ToUserName><![CDATA[toUser]]></ToUserName>
<FromUserName><![CDATA[fromUser]]></FromUserName>
<CreateTime>1357290913</CreateTime>
<MsgType><![CDATA[video]]></MsgType>
<MediaId><![CDATA[media_id]]></MediaId>
<ThumbMediaId><![CDATA[thumb_media_id]]></ThumbMediaId>
<MsgId>1234567890123456</MsgId>
</xml>";

            var doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlElement element = doc.DocumentElement;
            var msg = (VideoMessage)Message.FromXml(xml);

            Assert.AreEqual("toUser", msg.ToUserName);
            Assert.AreEqual("fromUser", msg.FromUserName);
            Assert.AreEqual(1357290913, ConvertDateTime(msg.CreateTime));
            Assert.AreEqual(MessageType.Video, msg.MsgType);
            Assert.AreEqual("media_id", msg.MediaId);
            Assert.AreEqual("thumb_media_id", msg.ThumbMediaId);
            Assert.AreEqual(1234567890123456, msg.MsgId);
        }

        [TestMethod]
        public void ParseLocationMessage()
        {
            var xml = @"<xml>
<ToUserName><![CDATA[toUser]]></ToUserName>
<FromUserName><![CDATA[fromUser]]></FromUserName>
<CreateTime>1351776360</CreateTime>
<MsgType><![CDATA[location]]></MsgType>
<Location_X>23.134521</Location_X>
<Location_Y>113.358803</Location_Y>
<Scale>20</Scale>
<Label><![CDATA[位置信息]]></Label>
<MsgId>1234567890123456</MsgId>
</xml>";

            var doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlElement element = doc.DocumentElement;
            var msg = (LocationMessage)Message.FromXml(xml);

            Assert.AreEqual("toUser", msg.ToUserName);
            Assert.AreEqual("fromUser", msg.FromUserName);
            Assert.AreEqual(1351776360, ConvertDateTime(msg.CreateTime));
            Assert.AreEqual(MessageType.Location, msg.MsgType);
            Assert.AreEqual(23.134521, msg.Location_X);
            Assert.AreEqual(113.358803, msg.Location_Y);
            Assert.AreEqual(20, msg.Scale);
            Assert.AreEqual("位置信息", msg.Label);
            Assert.AreEqual(1234567890123456, msg.MsgId);
        }

        [TestMethod]
        public void ParseLinkMessage()
        {
            var xml = @"<xml>
<ToUserName><![CDATA[toUser]]></ToUserName>
<FromUserName><![CDATA[fromUser]]></FromUserName>
<CreateTime>1351776360</CreateTime>
<MsgType><![CDATA[link]]></MsgType>
<Title><![CDATA[公众平台官网链接]]></Title>
<Description><![CDATA[公众平台官网链接]]></Description>
<Url><![CDATA[url]]></Url>
<MsgId>1234567890123456</MsgId>
</xml> ";

            var doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlElement element = doc.DocumentElement;
            var msg = (LinkMessage)Message.FromXml(xml);

            Assert.AreEqual("toUser", msg.ToUserName);
            Assert.AreEqual("fromUser", msg.FromUserName);
            Assert.AreEqual(1351776360, ConvertDateTime(msg.CreateTime));
            Assert.AreEqual(MessageType.Link, msg.MsgType);
            Assert.AreEqual("公众平台官网链接", msg.Title);
            Assert.AreEqual("公众平台官网链接", msg.Description);
            Assert.AreEqual("url", msg.Url);
            Assert.AreEqual(1234567890123456, msg.MsgId);
        }

        [TestMethod]
        public void ParseImageTextTest()
        {
            var xml = @"<xml>
<ToUserName><![CDATA[toUser]]></ToUserName>
<FromUserName><![CDATA[fromUser]]></FromUserName>
<CreateTime>12345678</CreateTime>
<MsgType><![CDATA[news]]></MsgType>
<ArticleCount>2</ArticleCount>
<Articles>
<item>
<Title><![CDATA[title1]]></Title> 
<Description><![CDATA[description1]]></Description>
<PicUrl><![CDATA[picurl]]></PicUrl>
<Url><![CDATA[url]]></Url>
</item>
<item>
<Title><![CDATA[title]]></Title>
<Description><![CDATA[description]]></Description>
<PicUrl><![CDATA[picurl]]></PicUrl>
<Url><![CDATA[url]]></Url>
</item>
</Articles>
</xml>";
            var msg = Message.FromXml(xml);
            Assert.IsNotNull(msg);
            Assert.AreEqual(MessageType.News, msg.MsgType);

            var news = (ImageTextMessage)msg;
            Assert.AreEqual(2, news.ArticleCount);

            Assert.AreEqual("title1", news.Articles[0].Title);
            Assert.AreEqual("title", news.Articles[1].Title);
        }

        [TestMethod]
        public void ConvertImageTextTest()
        {
            var img_text = new ImageTextMessage();
            img_text.CreateTime = DateTime.Now;
            img_text.Articles.Add(new Article
            {
                Description = "description",
                PicUrl = "picurl",
                Title = "title",
                Url = "url"
            });

            var xml = img_text.ToXml();

        }

        [TestMethod]
        public void ConvertMusicTest()
        {
            var img_text = new MusicMessage()
            {
                CreateTime = DateTime.Now,
                Description = "description",
                HQMusicUrl = "URL",
                MusicURL = "URL",
            };
            //img_text.CreateTime = DateTime.Now;
            //img_text..Add(new Article
            //{
            //    Description = "description",
            //    PicUrl = "picurl",
            //    Title = "title",
            //    Url = "url"
            //});

            var xml = img_text.ToXml();

        }

        int ConvertDateTime(System.DateTime time)
        {

            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));

            return (int)(time - startTime).TotalSeconds;

        }
    }
}
