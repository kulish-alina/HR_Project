using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebUI.Results
{
    public class ForbiddenResult : IHttpActionResult
    {
        string _message;
        public ForbiddenResult(string message)
        {
            _message = message;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.Forbidden)
            {
                Content = new StringContent(_message)
            };
            return Task.FromResult(response);
        }
    }

}