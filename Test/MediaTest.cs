using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YuChang.Core;

namespace WeiXin.Test
{
    [TestClass]
    public class MediaTest
    {
        string mediaId;
        [TestMethod]
        public void UploadTest()
        {
            var a = new AccessToken("wxe621f4e5e90b13cd", "7cb56e5b6bd302ddb73c4f76a9ec26a2");
            var m = new Media(a);
            var mediaId = m.Upload(@"C:\Users\maishu\Pictures\100.jpg", MediaType.Image);
            Assert.IsNotNull(mediaId);
            Console.WriteLine(mediaId);

            this.mediaId = mediaId;
        }

        [TestMethod]
        public void DownloadTest()
        {
            UploadTest();
            var a = new AccessToken("wxe621f4e5e90b13cd", "7cb56e5b6bd302ddb73c4f76a9ec26a2");
            var m = new Media(a);
            m.Download(mediaId, "C:/1.jpg");
        }
    }
}
