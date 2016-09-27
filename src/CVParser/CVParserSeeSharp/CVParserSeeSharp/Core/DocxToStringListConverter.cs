using AngleSharp.Parser.Html;
using DocumentFormat.OpenXml.Packaging;
using OpenXmlPowerTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace CVParser.Core
{
    public class DocxToStringListConverter : ITextFileConverter
    {
        /// <summary>
        /// Parses the initial file to the string list
        /// </summary>
        /// <param name="file">FileInfo of an appropriate file</param>
        /// <returns>Returns the IEnumerable of all lines in the file</returns>
        public IEnumerable<String> Convert(FileInfo file)
        {
            if (file.Extension != ".docx")
            {
                throw new ArgumentException("File is not a .docx");
            }
            var wPDoc = WordprocessingDocument.Open(file.FullName, true);
            var angleSharPparser = new HtmlParser();
            XElement result = new XElement("NaX");
            try
            {
                result = HtmlConverter.ConvertToHtml(wPDoc, new HtmlConverterSettings());
            }
            catch
            {

            }
            var parsedHtml = angleSharPparser.Parse(result.ToString());
            return parsedHtml.QuerySelectorAll("p, h1, h2").Select(x => x.TextContent).ToList();
        }
    }
}
