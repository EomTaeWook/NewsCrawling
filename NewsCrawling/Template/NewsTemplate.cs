using System;
using System.Collections.Generic;
using System.Text;
using TemplateContainers;

namespace NewsCrawling.Template
{
    public class NewsTemplate : BaseTemplate
    {
        public class CATagGroup
        {
            public string ATag { get; set; }

            public string Href { get; set; }

            public string Title { get; set; }

        }
        public string Url { get; set; }

        public CATagGroup ATagGroup { get; set; }

        public string NewsContentRegex { get; set; }

        public string Suffix { get; set; }
    }
}
