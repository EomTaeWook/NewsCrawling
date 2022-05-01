using KosherUtils.Framework;
using NewsCrawling.Model;
using NewsCrawling.Template;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace NewsCrawling.Manager
{
    public class DataManager : Singleton<DataManager>
    {
        private Dictionary<NewsTemplate, Dictionary<string, NewsData>> newsDatas = new Dictionary<NewsTemplate, Dictionary<string, NewsData>>();
        private MailConfig mailConfig;
        public void Init(MailConfig mailConfig)
        {
            this.mailConfig = mailConfig;
        }
        public bool AddData(NewsTemplate template, NewsData data)
        {
            if(newsDatas.ContainsKey(template) == false)
            {
                newsDatas.Add(template, new Dictionary<string, NewsData>());
            }
            if(newsDatas[template].ContainsKey(data.Url) == false)
            {
                newsDatas[template].Add(data.Url, data);
                return true;
            }
            return false;
        }
        public bool AddData(NewsTemplate template, List<NewsData> datas)
        {
            if (newsDatas.ContainsKey(template) == false)
            {
                newsDatas.Add(template, new Dictionary<string, NewsData>());
            }
            foreach(var data in datas)
            {
                if(this.AddData(template, data) == false)
                {
                    return false;
                }
            }

            return true;
        }

        public string CreateData()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var data in newsDatas)
            {
                sb.AppendLine(CreateDataBody(data.Key));
                sb.AppendLine();
            }
            return sb.ToString();
        }
        private string CreateDataBody(NewsTemplate newsTemplate)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@$"<b>{newsTemplate.Name}</b>");
            sb.AppendLine(@"<table style=""border: 1px solid black; border-collapse:collapse;""> ");
            sb.AppendLine(@"<tr>");
            sb.AppendLine(@"<th style=""border: 1px solid black; border-collapse: collapse;"">Url</th>");
            sb.AppendLine(@"<th style=""border: 1px solid black; border-collapse: collapse;"">Title</th>");
            sb.AppendLine(@"<th style=""border: 1px solid black; border-collapse: collapse;""></th>");
            sb.AppendLine(@"</tr>");

            foreach(var data in newsDatas[newsTemplate].Values)
            {
                sb.AppendLine(@"<tr>");
                sb.AppendLine(@$"<td style=""border: 1px solid black; border-collapse: collapse;"">{data.Url}</td>");
                sb.AppendLine(@$"<td style=""border: 1px solid black; border-collapse: collapse;"">{data.Title}</td>");
                sb.AppendLine(@$"<td style=""border: 1px solid black; border-collapse: collapse;""><a href=""{data.Url}"">이동</a></td>");
            }
            sb.AppendLine(@"</table>");
            return sb.ToString();
        }
        public async Task SendDataByEmailAsync()
        {
            if(this.mailConfig.SmtpReceiver.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"수신자 입력이 되지 않았습니다.");
                return;
            }
            MailMessage mailMessage = new MailMessage(this.mailConfig.SmtpSender, this.mailConfig.SmtpReceiver[0], this.mailConfig.MailTitle, CreateData());
            for(int i=1; i< this.mailConfig.SmtpReceiver.Count; ++i)
            {
                mailMessage.To.Add(this.mailConfig.SmtpReceiver[i]);
            }
            mailMessage.IsBodyHtml = true;
            mailMessage.BodyEncoding = Encoding.UTF8;
            SmtpClient smtp = new SmtpClient($"{this.mailConfig.SmtpHost}", this.mailConfig.SmtpPort);
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = new System.Net.NetworkCredential(this.mailConfig.MailUserId, this.mailConfig.MailUserPassword);
            try
            {
                await smtp.SendMailAsync(mailMessage);
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"메일 보내기에 실패하였습니다.");
                Console.WriteLine($"{ex.Message}");
            }
            finally
            {
                smtp.Dispose();
            }
            
        }
    }
}
