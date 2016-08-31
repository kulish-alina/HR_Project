using AngleSharp.Parser.Html;
using DocumentFormat.OpenXml.Packaging;
using OpenXmlPowerTools;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CVParserSeeSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var d = new DirectoryInfo(@"d:\Docs\");
            Dictionary<string, IEnumerable<string>> cvToParagraphs = new Dictionary<string, IEnumerable<string>>();
            foreach (var file in d.GetFiles())
            {
                var wPDoc = WordprocessingDocument.Open(file.FullName, true);
                var parser = new HtmlParser();
                var result = HtmlConverter.ConvertToHtml(wPDoc, new HtmlConverterSettings());
                var documnt = parser.Parse(result.ToString());
                var nodes = documnt.GetElementsByTagName("p").Select(x => x.TextContent.Trim(new char[] { ' ', '\n' }));
                cvToParagraphs.Add(file.FullName, nodes);
            }

        }
    }
}
