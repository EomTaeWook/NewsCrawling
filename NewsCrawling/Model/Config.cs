using System;
using System.Collections.Generic;
using System.Text;

namespace NewsCrawling.Model
{
    public class Config
    {
        public string JsonPath { get; set; }

        public int RequestDelayMilliseconds { get; set; }

        public List<string> Keywords { get; set; } = new List<string>();
    }
}
