using Spire.Doc;
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
            var doc = new Document(file.FullName);
            return doc.GetText().Split('\r').Select(x => x.Trim(new char[] { '\n', ' ' }));
        }
    }
}
