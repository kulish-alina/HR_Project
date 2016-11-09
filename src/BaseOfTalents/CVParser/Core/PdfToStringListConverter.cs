using Aspose.Pdf;

using System;
using System.Collections.Generic;
using System.IO;

namespace CVParser.Core
{
    public class PdfToStringListConverter : ITextFileConverter
    {
        /// <summary>
        /// Parses the initial file to the string list
        /// </summary>
        /// <param name="file">FileInfo of a pdf file</param>
        /// <returns>Returns the IEnumerable of all lines in the file</returns>
        public IEnumerable<String> Convert(FileInfo file)
        {
            if (file.Extension != ".pdf")
            {
                throw new ArgumentException("File is not a .pdf");
            }
            var document = new Aspose.Pdf.Document(file.FullName);
            DocSaveOptions saveOptions = new DocSaveOptions();
            saveOptions.Mode = DocSaveOptions.RecognitionMode.Flow;
            saveOptions.RelativeHorizontalProximity = 2.5f;
            saveOptions.RecognizeBullets = true;
            var convertedFile = $"{file.FullName}.doc";
            document.Save(convertedFile, saveOptions);
            var doc = new DocToStringListConverter().Convert(new FileInfo(convertedFile));
            return doc;
        }
    }
}
