using System;
using System.Collections.Generic;
using System.IO;

namespace CVParser.Core
{
    public interface ITextFileConverter
    {
        /// <summary>
        /// Parses the initial file to the string list
        /// </summary>
        /// <param name="file">FileInfo of an appropriate file</param>
        /// <returns>Returns the IEnumerable of all lines in the file</returns>
        IEnumerable<String> Convert(FileInfo file);
    }
}
