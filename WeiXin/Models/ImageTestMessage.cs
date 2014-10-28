using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiXin.Models;

namespace YuChang.Core.Models
{
    public class ImageTextMessage : Message
    {
        public ImageTextMessage()
            : base(MessageType.ImageText)
        {
            this.Articles = new List<Article>();
        }

        public int ArticleCount
        {
            get { return this.Articles.Count; }
        }

        public IList<Article> Articles
        {
            get;
            private set;
        }
    }

    public class Article
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string PicUrl { get; set; }

        public string Url { get; set; }
    }
}
