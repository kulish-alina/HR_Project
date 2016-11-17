using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebUI.Results
{
    public class FileResult : IHttpActionResult
    {
        private string _mediaType;
        private string _fileName;
        private Stream _content;

        public FileResult(string mediaType, string fileName, Stream content)
        {
            _mediaType = mediaType;
            _fileName = fileName;
            _content = content;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            response.Content = new StreamContent(_content);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(_mediaType);
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = _fileName
            };
            return Task.FromResult(response);
        }
    }
}