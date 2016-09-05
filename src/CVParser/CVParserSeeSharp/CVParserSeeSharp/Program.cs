using AngleSharp.Parser.Html;
using CVParserSeeSharp.CVStructure;
using CVParserSeeSharp.Parser;
using DocumentFormat.OpenXml.Packaging;
using OpenXmlPowerTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace CVParserSeeSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> stopWords = new List<string> { "a", "about", "above", "above", "across", "after", "afterwards", "again",
                "against", "all", "almost", "alone", "along", "already", "also", "although", "always", "am", "among", "amongst",
                "amoungst", "amount", "an", "and", "another", "any", "anyhow", "anyone", "anything", "anyway", "anywhere", "are", "around",
                "as", "at", "back", "be", "became", "because", "become", "becomes", "becoming", "been", "before", "beforehand", "behind", "being",
                "below", "beside", "besides", "between", "beyond", "bill", "both", "bottom", "but", "by", "call", "can", "cannot", "cant", "co", "con",
                "could", "couldnt", "cry", "de", "describe", "detail", "do", "done", "down", "due", "during", "each", "eg", "eight", "either", "eleven", "else",
                "elsewhere", "empty", "enough", "etc", "even", "ever", "every", "everyone", "everything", "everywhere", "except", "few", "fifteen", "fify", "fill",
                "find", "fire", "first", "five", "for", "former", "formerly", "forty", "found", "four", "from", "front", "full", "further", "get", "give", "go", "had",
                "has", "hasnt", "have", "he", "hence", "her", "here", "hereafter", "hereby", "herein", "hereupon", "hers", "herself", "him", "himself", "his", "how",
                "however", "hundred", "ie", "if", "in", "inc", "indeed", "interest", "into", "is", "it", "its", "itself", "keep", "last", "latter", "latterly", "least",
                "less", "ltd", "made", "many", "may", "me", "meanwhile", "might", "mill", "mine", "more", "moreover", "most", "mostly", "move", "much", "must", "my",
                "myself", "name", "namely", "neither", "never", "nevertheless", "next", "nine", "no", "nobody", "none", "noone", "nor", "not", "nothing", "now",
                "nowhere", "of", "off", "often", "on", "once", "one", "only", "onto", "or", "other", "others", "otherwise", "our", "ours", "ourselves", "out", "over",
                "own", "part", "per", "perhaps", "please", "put", "rather", "re", "same", "see", "seem", "seemed", "seeming", "seems", "serious", "several", "she",
                "should", "show", "side", "since", "sincere", "six", "sixty", "so", "some", "somehow", "someone", "something", "sometime", "sometimes", "somewhere",
                "still", "such", "system", "take", "ten", "than", "that", "the", "their", "them", "themselves", "then", "thence", "there", "thereafter", "thereby",
                "therefore", "therein", "thereupon", "these", "they", "thickv", "thin", "third", "this", "those", "though", "three", "through", "throughout", "thru",
                "thus", "to", "together", "too", "top", "toward", "towards", "twelve", "twenty", "two", "un", "under", "until", "up", "upon", "us", "very", "via", "was",
                "we", "well", "were", "what", "whatever", "when", "whence", "whenever", "where", "whereafter", "whereas", "whereby", "wherein", "whereupon", "wherever",
                "whether", "which", "while", "whither", "who", "whoever", "whole", "whom", "whose", "why", "will", "with", "within", "without", "would", "yet", "you",
                "your", "yours", "yourself", "yourselves", "the" };

            var d = new DirectoryInfo(@"d:\Docs\");
            List<List<string>> cvToParagraphs = new List<List<string>>();
            foreach (var file in d.GetFiles("*.docx"))
            {
                var wPDoc = WordprocessingDocument.Open(file.FullName, true);
                var parser = new HtmlParser();
                XElement result;
                try
                {
                    result = HtmlConverter.ConvertToHtml(wPDoc, new HtmlConverterSettings());
                }
                catch (Exception)
                {
                    continue;
                }
                //File.WriteAllText(file.FullName + ".html", result.ToString());
                var documnt = parser.Parse(result.ToString());
                var h1 = documnt.GetElementsByTagName("h1");
                var h2 = documnt.GetElementsByTagName("h2");
                var header = documnt.GetElementsByTagName("header");
                var nodes = documnt.GetElementsByTagName("p").ToList();
                nodes.AddRange(h1);
                nodes.AddRange(h2);
                nodes.AddRange(header);
                nodes = documnt.QuerySelectorAll("p, h1, h2").ToList();
                var spaceRegEx = new Regex("[ \n]+");
                var superpupernodes = nodes.Select(x => spaceRegEx.Replace(x.TextContent, " ", 999).Trim(new char[] { ' ', '\n' })).ToList();
                cvToParagraphs.Add(superpupernodes);
            }
            var cvs = new List<CV>();
            cvToParagraphs.ForEach(x => cvs.Add(CVParser.Parse(x)));
            foreach (var cv in cvs)
            {
                StringBuilder cvBuilder = new StringBuilder();
                cv.Blocks.ForEach(x =>
                {
                    x.RelatedInformation.ForEach(y => cvBuilder.AppendLine(y));
                    cvBuilder.AppendLine();
                });
                File.WriteAllText(@"d:\Docs\" + cv.GetHashCode() + ".1th" + ".txt", cvBuilder.ToString());
            }

            foreach (var cv in cvs)
            {
                StringBuilder curiculumVitae = new StringBuilder();
                foreach (var block in cv.Blocks)
                {
                    var prediction = LogicalPredictor.Predict(block);
                    curiculumVitae.AppendLine(string.Format("--------{0}---{1}%---------", prediction.Key, prediction.Value * 100));
                    block.RelatedInformation.ForEach(x => curiculumVitae.AppendLine(x));
                }
                File.WriteAllText(@"d:\Docs\" + curiculumVitae.GetHashCode() + ".2th" + ".txt", curiculumVitae.ToString());
            }
        }

        private static string resolveLineMesto(string firstLine)
        {
            if (firstLine.ToLower().Contains("personal") && firstLine.ToLower().Contains("contact"))
            {
                return "personal";
            }
            else if (firstLine.ToLower().Contains("skill"))
            {
                return "skill";
            }
            else if (firstLine.ToLower().Contains("experience"))
            {
                return "experience";
            }
            else if (firstLine.ToLower().Contains("education"))
            {
                return "education";
            }
            return "additional";
        }
    }
}
