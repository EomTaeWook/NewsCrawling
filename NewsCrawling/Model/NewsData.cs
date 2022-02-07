using System;
using System.Collections.Generic;
using System.Text;

namespace NewsCrawling.Model
{
    public class NewsData
    {
        public string Url { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
        public void SetUrl(string url)
        {
            url = url.Trim();
            if (url.StartsWith("\"") == true)
            {
                url = url.Remove(0, 1);
            }
            if (url.EndsWith("\"") == true)
            {
                url = url.Remove(url.Length - 1, 1);
            }
            this.Url = url;
        }
    }
}
