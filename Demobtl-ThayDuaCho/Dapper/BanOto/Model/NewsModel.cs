using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class NewsModel
    {
        public string news_id { get; set; }
        public string title { get; set; }
        public DateTime? created_date { get; set; }
        public string content_news { get; set; } 
    }
}
