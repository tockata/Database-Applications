namespace _01_SoftUniRSSFeed
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.UI;
    using System.Xml;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using Formatting = Newtonsoft.Json.Formatting;

    public class SoftUniRssFeed
    {
        public static void Main()
        {
            // 01-Problem Download the content of the SoftUni RSS feed
            WebClient wc = new WebClient();
            wc.DownloadFile("https://softuni.bg/Feed/News", "../../../softuni-feed.xml");

            // 02-Problem Problem 2.Parse the XML from the feed to JSON
            XmlDocument doc = new XmlDocument();
            doc.Load("../../../softuni-feed.xml");
            string json = JsonConvert.SerializeObject(doc, Formatting.Indented);
            Console.WriteLine(json);
            Console.WriteLine();

            // 03-Problem Using LINQ-to-JSON select all the question titles and print them
            // to the console
            var jsonObj = JObject.Parse(json);
            var titles = jsonObj["rss"]["channel"]["item"]
                .Select(i => i["title"]);

            foreach (var title in titles)
            {
                Console.WriteLine(title);
            }

            Console.WriteLine();

            // 04-Problem Parse the JSON string to POCO
            var template = new
            {
                Rss = new
                {
                    Channel = new
                    {
                        Title = string.Empty,
                        Link = string.Empty,
                        Description = string.Empty,
                        LastBuildDate = new DateTime(),
                        Item = new[]
                        {
                            new
                            {
                                Guid = new
                                {
                                    @isPermaLink = string.Empty
                                },
                                Link = string.Empty,
                                Title = string.Empty,
                                Description = string.Empty
                            }                                       
                        }
                    }
                }
            };

            var rss = JsonConvert.DeserializeAnonymousType(json, template);
            Console.WriteLine("Title: " + rss.Rss.Channel.Title);
            Console.WriteLine("Link: " + rss.Rss.Channel.Link);
            Console.WriteLine("Description: " + rss.Rss.Channel.Description);
            Console.WriteLine("Last build date: " + rss.Rss.Channel.LastBuildDate);
            foreach (var item in rss.Rss.Channel.Item)
            {
                Console.WriteLine("-- Item: ");
                Console.WriteLine("---- Guid: " + item.Guid);
                Console.WriteLine("---- Link: " + item.Link);
                Console.WriteLine("---- Title: " + item.Title);
                Console.WriteLine("---- Description: " + item.Description);
            }

            Console.WriteLine();

            // 05-Problem Using the parsed objects create a HTML page that lists all questions
            // from the RSS their categories and a link to the question’s page
            using (StreamWriter sw = new StreamWriter("../../../questions.html"))
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    htw.WriteLine("<!DOCTYPE html>");
                    htw.RenderBeginTag(HtmlTextWriterTag.Html);

                    htw.RenderBeginTag(HtmlTextWriterTag.Head);
                    htw.RenderBeginTag(HtmlTextWriterTag.Title);
                    htw.Write("RSS Feed Questions");
                    htw.RenderEndTag();
                    htw.RenderEndTag();
                    htw.WriteLine();

                    htw.RenderBeginTag(HtmlTextWriterTag.Body);
                    foreach (var item in rss.Rss.Channel.Item)
                    {
                        htw.RenderBeginTag(HtmlTextWriterTag.Div);
                        htw.RenderBeginTag(HtmlTextWriterTag.Span);
                        htw.Write("Question: {0}", item.Title);
                        htw.RenderEndTag();
                        htw.WriteLine();
                        htw.AddAttribute(HtmlTextWriterAttribute.Href, item.Link);
                        htw.RenderBeginTag(HtmlTextWriterTag.A);
                        htw.WriteLine(item.Link);
                        htw.RenderEndTag();
                        htw.WriteLine();
                        htw.RenderBeginTag(HtmlTextWriterTag.P);

                        // HttpUtility.HtmlEncode - escapes html tags in text:
                        htw.WriteLine(HttpUtility.HtmlEncode(item.Description.Substring(0, 200)));
                        htw.RenderEndTag();
                        htw.RenderEndTag();
                        htw.WriteLine();
                    }

                    htw.RenderEndTag();
                    htw.RenderEndTag();
                }
            }
        }
    }
}
