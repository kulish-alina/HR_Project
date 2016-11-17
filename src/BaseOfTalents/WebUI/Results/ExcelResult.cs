using System.IO;

namespace WebUI.Results
{
    public class ExcelResult : FileResult
    {
        public ExcelResult(string fileName, Stream content)
            : base("application/vnd.ms-excel", fileName, content)
        {
        }
    }
}