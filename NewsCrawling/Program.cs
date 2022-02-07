using NewsCrawling.Manager;
using NewsCrawling.Model;
using NewsCrawling.Template;
using System;
using System.IO;
using System.Text;
using TemplateContainers;

namespace NewsCrawling
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var mailJsonFileName = @"mailConfig.json";
                var config = Newtonsoft.Json.JsonConvert.DeserializeObject<Config>(File.ReadAllText(@"config.json"));
#if DEBUG
                config.JsonPath = $@"..\..\..\..\..\Datas\";
                mailJsonFileName = $@"mailConfig_debug.json";
#endif
                var mailConfig = Newtonsoft.Json.JsonConvert.DeserializeObject<MailConfig>(File.ReadAllText(mailJsonFileName));

                RegisterProvider();
                InitTemplate(config);
                DataManager.Instance.Init(mailConfig);
                CrawlingManager.Instance.Init(config.RequestDelayMilliseconds, config.Keywords);
                CrawlingManager.Instance.RunAsync().GetAwaiter().GetResult();

                DataManager.Instance.SendDataByEmailAsync().GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{ex.Message}");
                Console.ReadLine();
            }
        }
        private static void InitTemplate(Config confingModel)
        {
            TemplateContainer<NewsTemplate>.Load(confingModel.JsonPath, "NewsTemplate.json");
        }
        private static void RegisterProvider()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
    }
}
