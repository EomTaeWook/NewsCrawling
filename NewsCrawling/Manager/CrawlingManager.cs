using KosherUtils.Coroutine;
using KosherUtils.Framework;
using NewsCrawling.Model;
using NewsCrawling.Template;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TemplateContainers;

namespace NewsCrawling.Manager
{
    public class CrawlingManager : Singleton<CrawlingManager>
    {
        private static HttpClient httpClient = new HttpClient();
        private int requestDelayMilliseconds = 0;
        private List<string> keywords;
        public void Init(int delay, List<string> keywords)
        {
            this.requestDelayMilliseconds = delay;

            if(keywords == null)
            {
                keywords = new List<string>();
            }
            this.keywords = keywords;
        }
        public async Task RunAsync()
        {
            foreach(var template in TemplateContainer<NewsTemplate>.Values)
            {
                for(int i=0; i<1; ++i)
                {
                    var url = string.Format(template.Url, i + 1);
                    var body = "";
                    try
                    {
                        var response = await httpClient.GetAsync(url);
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            body = await response.Content.ReadAsStringAsync();
                        }
                    }
                    catch(Exception ex)
                    {
                        throw ex;
                    }
                    var result = await ProcessHtmlParseAsync(body, template);
                    if (result != null)
                    {
                        DataManager.Instance.AddData(template, result);
                    }
                    Console.WriteLine();
                    await Task.Delay(requestDelayMilliseconds);
                }
            }
        }
        private async Task<List<NewsData>> ProcessHtmlParseAsync(string body, NewsTemplate newsTemplate)
        {
            if(string.IsNullOrEmpty(body) == true)
            {
                return null;
            }
            var list = new List<NewsData>();

            var aTagList = ProcessRegexList(newsTemplate.ATagGroup.ATag, body);

            foreach (var aTagData in aTagList)
            {
                var newsData = new NewsData();

                var herf = ProcessRegex(newsTemplate.ATagGroup.Href, aTagData);
                herf = herf.Trim();
                if (herf.StartsWith("\"") == true)
                {
                    herf = herf.Remove(0, 1);
                }
                if (herf.EndsWith("\"") == true)
                {
                    herf = herf.Remove(herf.Length - 1, 1);
                }

                if (herf.StartsWith(newsTemplate.Suffix) == false)
                {
                    herf = $"{newsTemplate.Suffix}{herf}";
                }

                newsData.SetUrl(herf);

                var title = ProcessRegex(newsTemplate.ATagGroup.Title, aTagData);
                newsData.SetTitle(title);
                if (string.IsNullOrEmpty(newsData.Url) == true)
                {
                    continue;
                }

                try
                {
                    var response = await httpClient.GetAsync(newsData.Url);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        body = await response.Content.ReadAsStringAsync();
                        var content = ProcessRegex(newsTemplate.NewsContentRegex, body);
                        newsData.SetContent(content);
                    }
                }
                catch(Exception ex)
                {
                    throw ex;
                }

                if(IsContainKeywords(newsData.Title) || IsContainKeywords(newsData.Content) == true)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{newsTemplate.Name} - {newsData.Title} 추가");
                    list.Add(newsData);
                }

                await Task.Delay(200);
            }

            return list;
        }
        private List<string> ProcessRegexList(string regexPattern, string body)
        {
            var list = new List<string>();

            Regex regex = new Regex(regexPattern);
            foreach (Match match in regex.Matches(body))
            {
                list.Add(match.Value.Trim());
            }
            return list;
        }
        private string ProcessRegex(string regexPattern, string body)
        {
            Regex regex = new Regex(regexPattern);

            var result = regex.Match(body);
            if(result.Success == true)
            {
                return result.Value.Trim();
            }
            return string.Empty;
        }

        private bool IsContainKeywords(string body)
        {
            foreach(var keyword in this.keywords)
            {
                if(body.Contains(keyword) == true)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
