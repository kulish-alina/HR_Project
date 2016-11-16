using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebUI.Results
{
    public class ConflictResult : IHttpActionResult
    {
        public ConflictResult(string reasonPhrase)
        {
            ReasonPhrase = reasonPhrase;
        }

        public string ReasonPhrase { get; private set; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }
        private HttpResponseMessage Execute()
        {
            return new HttpResponseMessage(HttpStatusCode.Conflict)
            {
                ReasonPhrase = ReasonPhrase
            };
        }
    }
}