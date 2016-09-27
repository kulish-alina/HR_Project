using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CVParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var filesDirectories = new DirectoryInfo(@"d:\Docs\");
            List<List<String>> cvToParagraphs = new List<List<String>>();
            var parsedResumes = filesDirectories.GetFiles("*.docx").Select(x => CVParser.Parser.CVParser.Parse((x.FullName))).ToList();
            var skypeRegexp = new Regex(@"skype", RegexOptions.IgnoreCase);
            var ccc = parsedResumes.Select(x => x.Skype).ToList();
        }
    }
}
