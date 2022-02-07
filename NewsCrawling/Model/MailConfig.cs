using System;
using System.Collections.Generic;
using System.Text;

namespace NewsCrawling.Model
{
    public class MailConfig
    {
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpSender { get; set; }
        public List<string> SmtpReceiver { get; set; }
        public string MailTitle { get; set; }

        public string MailUserId { get; set; }

        public string MailUserPassword { get; set; }
    }
}
