using Spire.Doc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CVParser.Core
{
    public class DocxToStringListConverter : ITextFileConverter
    {
        /// <summary>
        /// Parses the initial file to the string list
        /// </summary>
        /// <param name="file">FileInfo of an docx file</param>
        /// <returns>Returns the IEnumerable of all lines in the file</returns>
        public IEnumerable<String> Convert(FileInfo file)
        {
            if (file.Extension != ".docx")
            {
                throw new ArgumentException("File is not a .docx");
            }
            var doc = new Document(file.FullName);
            return doc.GetText().Split('\r').Select(x => x.Trim(new char[] { '\n', ' ' }));
        }
    }
}
