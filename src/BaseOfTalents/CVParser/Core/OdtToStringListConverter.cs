using Aspose.Words;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CVParser.Core
{
    public class OdtToStringListConverter : ITextFileConverter
    {
        /// <summary>
        /// Parses the initial file to the string list
        /// </summary>
        /// <param name="file">FileInfo of an odt file</param>
        /// <returns>Returns the IEnumerable of all lines in the file</returns>
        public IEnumerable<String> Convert(FileInfo file)
        {
            if (file.Extension != ".odt")
            {
                throw new ArgumentException("File is not a .odt");
            }
            var doc = new Document(file.FullName, new LoadOptions(LoadFormat.Odt, String.Empty, String.Empty));
            return doc.GetText().Split('\r').Skip(1).Select(x => x.Trim(new char[] { '\n', ' ' }));
        }
    }
}
