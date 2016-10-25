using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using Newtonsoft.Json;
using NLog;

namespace WebUI.Services
{
    public class NLogExceptionLogger : ExceptionLogger
    {
        private readonly Logger logger;
        public NLogExceptionLogger()
        {
            logger = LogManager.GetCurrentClassLogger();
        }
        public override async Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            var logData = new LogData
            {
                ExceptionMessage = context.Exception.Message,
                RequestUri = context.Request.RequestUri,
                RequestContent = context.Request.Content,
                RequestHeaders = context.Request.Headers,

                ExceptionStackTrace = context.Exception.StackTrace,
                InnerException = context.Exception.InnerException
            };

            logger.Error(JsonConvert.SerializeObject(logData));
            await base.LogAsync(context, cancellationToken);
        }

        private class LogData
        {
            public string ExceptionMessage { get; set; }
            public Uri RequestUri { get; set; }
            public HttpContent RequestContent { get; set; }
            public HttpRequestHeaders RequestHeaders { get; set; }

            public Exception InnerException { get; set; }
            public string ExceptionStackTrace { get; set; }
        }
    }
}