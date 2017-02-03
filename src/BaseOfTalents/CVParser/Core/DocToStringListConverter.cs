using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CVParser.Core
{
    using FuncParser = Func<string, string>;

    public class DocToStringListConverter : ITextFileConverter
    {
        /// <summary>
        /// Parses the initial file to the string list
        /// </summary>
        /// <param name="file">FileInfo of an doc file</param>
        /// <returns>Returns the IEnumerable of all lines in the file</returns>
        public IEnumerable<string> Convert(FileInfo file)
        {
            if (file.Extension != ".doc")
            {
                throw new ArgumentException("File is not a .doc");
            }
            string text = string.Empty;
            if(!TryReadText(file.FullName, out  text))
            {
                throw new Exception("Document not loaded, call the programmer");
            }
            return text.Split('\r').Select(x => x.Trim(new char[] { '\n', ' ' }));
        }
        
        private static string AsposeTextRead(string path)
        {
            return new Aspose.Words.Document(path).GetText();
        }
        private static string SpireTextRead(string path)
        {
            return new Spire.Doc.Document(path).GetText();
        }

        /// <summary>
        /// Method tries to read the text if the doc file by path. Ref string will contains the text of it. Returns true if load was successful false if no
        /// </summary>
        /// <param name="path">Full path to the .doc file to read</param>
        /// <param name="text">Text of doc</param> 
        /// <returns>Bool which determines load status</returns>
        private bool TryReadText(string path, out string text)
        {
            var defaultTextLoaders = new List<FuncParser>()
            {
                AsposeTextRead,
                SpireTextRead
            };
            return TryReadText(path, out text, defaultTextLoaders);
        }

        /// <summary>
        /// Method tries to read the text if the doc file by path. Ref string will contains the text of it. Returns true if load was successful false if no
        /// </summary>
        /// <param name="path">Full path to the .doc file to read</param>
        /// <param name="text">Text of doc</param>
        /// <param name="textLoaders">loaders, which can be specified, used to read the file</param>
        /// <returns>Bool which determines load status</returns>
        private bool TryReadText(string path, out string text, List<FuncParser> textLoaders)
        {
            text = GetFirstText(path, textLoaders);
            return !string.IsNullOrEmpty(text);
        }

        private static string GetFirstText(string path, List<FuncParser> actualLoaders)
        {
            var text = String.Empty;
            List <Exception> errors = new List<Exception>();
            foreach (var loader in actualLoaders)
            {
                try
                {
                    text = loader(path);
                }
                catch (Exception e)
                {
                    //intentional exception supression, because we need to go thru all the loaders before throw one
                    errors.Add(e);
                }
                if (!String.IsNullOrEmpty(text)) break;
            }
            return text;
        }
    }
}