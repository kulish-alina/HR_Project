using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CVParser.Core
{
    public class DocToStringListConverter : ITextFileConverter
    {
        /// <summary>
        /// Parses the initial file to the string list
        /// </summary>
        /// <param name="file">FileInfo of an doc file</param>
        /// <returns>Returns the IEnumerable of all lines in the file</returns>
        public IEnumerable<String> Convert(FileInfo file)
        {
            if (file.Extension != ".doc")
            {
                throw new ArgumentException("File is not a .doc");
            }
            String text = String.Empty;
            if(!TryReadText(file.FullName, ref text))
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
        /// <param name="textLoaders">loaders, which can be specified, used to read the file</param>
        /// <returns>Bool which determines load status</returns>
        private bool TryReadText(string path, ref string text, List<Func<string, string>> textLoaders = null)
        {
            List<Func<string, string>> defaultTextLoaders = new List<Func<string, string>>()
            {
                AsposeTextRead,
                SpireTextRead
            };
            var actualLoaders = textLoaders != null && textLoaders.Count != 0 ? textLoaders : defaultTextLoaders;
            List<Exception> errors = new List<Exception>();
            foreach (var loader in actualLoaders)
            {
                try
                {
                    text = loader(path);
                }
                catch(Exception e)
                {
                    //intentional exception supression, because we need to go thru all the loaders before throw one
                    errors.Add(e);
                }
                if (!String.IsNullOrEmpty(text)) break; 
            }
            return !String.IsNullOrEmpty(text);
        }
    }
}
